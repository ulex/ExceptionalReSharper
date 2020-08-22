namespace ReSharper.Exceptional.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Analyzers;

    using ExceptionsOrigins;

    using JetBrains.Application.Progress;
    using JetBrains.ReSharper.Psi.CodeStyle;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI;
    using JetBrains.ReSharper.Psi.Resolve;
    using JetBrains.ReSharper.Psi.Tree;

    /// <summary>Stores data about processed <see cref="IDocCommentBlockNode" />. </summary>
    internal sealed class DocCommentBlockModel : TreeElementModelBase<IDocCommentBlock>
    {
        #region member vars

        private string _documentationText;

        #endregion

        #region constructors and destructors

        public DocCommentBlockModel(IAnalyzeUnit analyzeUnit, IDocCommentBlock docCommentNode) : base(analyzeUnit, docCommentNode)
        {
            References = new List<IReference>();
            DocumentedExceptions = new List<ExceptionDocCommentModel>();
            Update();
        }

        #endregion

        #region methods

        public override void Accept(AnalyzerBase analyzer)
        {
            foreach (var exception in DocumentedExceptions)
            {
                exception.Accept(analyzer);
            }
        }

        public ExceptionDocCommentModel AddExceptionDocumentation(ThrownExceptionModel thrownException, IProgressIndicator progressIndicator)
        {
            if (thrownException.ExceptionType == null)
            {
                return null;
            }
            var exceptionDescription = thrownException.ExceptionDescription.Trim();
            if (thrownException.ExceptionsOrigin is ThrowStatementModel)
            {
                if (thrownException.ExceptionType.GetClrName().FullName == "System.ArgumentNullException")
                {
                    exceptionDescription = ArgumentNullExceptionDescription.CreateFrom(thrownException).GetDescription().Trim();
                }
            }
            else
            {
                exceptionDescription = Regex.Replace(exceptionDescription, "<paramref name=\"(.*?)\"/>", m => m.Groups[1].Value).Trim();
            }
            var exceptionDocumentation = string.IsNullOrEmpty(exceptionDescription)
                ? $"<exception cref=\"T:{thrownException.ExceptionType.GetClrName().FullName}\">" + Constants.ExceptionDescriptionMarker
                                                                                                  + $".</exception>{Environment.NewLine}"
                : $"<exception cref=\"T:{thrownException.ExceptionType.GetClrName().FullName}\">{exceptionDescription}</exception>{Environment.NewLine}";
            if (thrownException.ExceptionsOrigin.ContainingBlock is AccessorDeclarationModel)
            {
                var accessor = ((AccessorDeclarationModel)thrownException.ExceptionsOrigin.ContainingBlock).Node.NameIdentifier.Name;
                exceptionDocumentation = string.IsNullOrEmpty(exceptionDescription)
                    ? $"<exception cref=\"T:{thrownException.ExceptionType.GetClrName().FullName}\" accessor=\"{accessor}\">"
                    + Constants.ExceptionDescriptionMarker + $".</exception>{Environment.NewLine}"
                    : $"<exception cref=\"T:{thrownException.ExceptionType.GetClrName().FullName}\" accessor=\"{accessor}\">{exceptionDescription}</exception>{Environment.NewLine}";
            }
            ChangeDocumentation(_documentationText + "\n" + exceptionDocumentation);
            return DocumentedExceptions.LastOrDefault();
        }

        public void RemoveExceptionDocumentation(ExceptionDocCommentModel exceptionDocumentation, IProgressIndicator progress)
        {
            if (exceptionDocumentation == null)
            {
                return;
            }
            var attributes = "cref=\"" + Regex.Escape(exceptionDocumentation.ExceptionTypeName) + "\"";
            if (exceptionDocumentation.Accessor != null)
            {
                attributes += " accessor=\"" + Regex.Escape(exceptionDocumentation.Accessor) + "\"";
            }
            var regex = "<exception " + attributes + "((>((\r|\n|.)*?)</exception>)|((\r|\n|.)*?/>))";
            var newDocumentation = Regex.Replace(_documentationText, regex, string.Empty);
            ChangeDocumentation(newDocumentation);
        }

        private void ChangeDocumentation(string text)
        {
            text = text.Trim('\r', '\n');
            if (!string.IsNullOrEmpty(text))
            {
                var newNode = GetElementFactory().CreateDocCommentBlock(text);
                if (IsCreated)
                {
                    LowLevelModificationUtil.ReplaceChildRange(Node, Node, newNode);
                }
                else
                {
                    LowLevelModificationUtil.AddChildBefore(AnalyzeUnit.Node.FirstChild, newNode);
                }
                newNode.FormatNode();
                Node = newNode;
            }
            else if (Node != null)
            {
                LowLevelModificationUtil.DeleteChild(Node);
                Node = null;
            }
            Update();
        }

        private string GetDocumentationXml()
        {
            var xml = string.Empty;
            foreach (var node in Node.Children().OfType<IDocCommentNode>())
            {
                xml += node.GetText().Replace("/// ", "").Replace("///", "") + "\n";
            }
            return xml;
        }

        private IEnumerable<ExceptionDocCommentModel> GetDocumentedExceptions()
        {
            var regex = new Regex("<exception cref=\"(.*?)\"( accessor=\"(.*?)\")?(>((\r|\n|.)*?)</exception>)?");
            var exceptions = new List<ExceptionDocCommentModel>();
            foreach (Match match in regex.Matches(_documentationText))
            {
                var exceptionType = match.Groups[1].Value;
                var accessor = !string.IsNullOrEmpty(match.Groups[3].Value) ? match.Groups[3].Value : null;
                var exceptionDescription = match.Groups[5].Value;
                exceptions.Add(new ExceptionDocCommentModel(this, exceptionType, exceptionDescription, accessor));
            }
            return exceptions;
        }

        private void Update()
        {
            if (!IsCreated)
            {
                _documentationText = string.Empty;
                References = new List<IReference>();
                DocumentedExceptions = new List<ExceptionDocCommentModel>();
            }
            else
            {
                _documentationText = GetDocumentationXml();
                References = new List<IReference>(Node.GetFirstClassReferences());
                DocumentedExceptions = GetDocumentedExceptions();
            }
        }

        #endregion

        #region properties

        public IEnumerable<ExceptionDocCommentModel> DocumentedExceptions { get; private set; }

        public bool IsCreated => Node != null;

        public List<IReference> References { get; private set; }

        #endregion
    }
}