namespace ReSharper.Exceptional.MF.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using ExceptionsOrigins;

    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    /// <summary>Extracts thrown exceptions. </summary>
    internal static class ThrownExceptionsReader
    {
        #region methods

        /// <summary>Reads the specified reference expression. </summary>
        /// <param name="analyzeUnit">The analyze unit. </param>
        /// <param name="exceptionsOrigin">The exceptions origin. </param>
        /// <param name="referenceExpression">The reference expression.</param>
        /// <returns>The list of thrown exceptions. </returns>
        public static IEnumerable<ThrownExceptionModel> Read(
            IAnalyzeUnit analyzeUnit,
            IExceptionsOriginModel exceptionsOrigin,
            IReferenceExpression referenceExpression)
        {
            var result = new List<ThrownExceptionModel>();
            var resolveResult = referenceExpression.Parent is IElementAccessExpression
                ? ((IElementAccessExpression)referenceExpression.Parent).Reference.Resolve()
                : referenceExpression.Reference.Resolve();
            var declaredElement = resolveResult.DeclaredElement;
            if (declaredElement == null)
            {
                return result;
            }
            var declarations = declaredElement.GetDeclarations();
            if (declarations.Count == 0)
            {
                return Read(analyzeUnit, exceptionsOrigin, declaredElement);
            }
            foreach (var declaration in declarations)
            {
                if (!(declaration is IDocCommentBlockOwner docCommentBlockOwnerNode))
                {
                    return result;
                }
                var docCommentBlockNode = docCommentBlockOwnerNode.DocCommentBlock;
                if (docCommentBlockNode == null)
                {
                    return result;
                }
                string accessor = null;
                if (exceptionsOrigin is ReferenceExpressionModel && exceptionsOrigin.ContainingBlock is AccessorDeclarationModel)
                {
                    accessor = ((AccessorDeclarationModel)exceptionsOrigin.ContainingBlock).Node.NameIdentifier.Name;
                }
                var docCommentBlockModel = new DocCommentBlockModel(exceptionsOrigin.ContainingBlock as IAnalyzeUnit, docCommentBlockNode);
                foreach (var comment in docCommentBlockModel.DocumentedExceptions)
                {
                    if (exceptionsOrigin is ReferenceExpressionModel && exceptionsOrigin.ContainingBlock is AccessorDeclarationModel)
                    {
                        comment.AssociatedExceptionModel = new ThrownExceptionModel(
                            analyzeUnit,
                            exceptionsOrigin,
                            comment.ExceptionType,
                            comment.ExceptionDescription,
                            false,
                            accessor);
                    }
                    else
                    {
                        comment.AssociatedExceptionModel = new ThrownExceptionModel(
                            analyzeUnit,
                            exceptionsOrigin,
                            comment.ExceptionType,
                            comment.ExceptionDescription,
                            false,
                            comment.Accessor);
                    }
                    if (exceptionsOrigin is ReferenceExpressionModel)
                    {
                        if (((ReferenceExpressionModel)exceptionsOrigin).IsExceptionValid(comment) == false)
                        {
                            continue;
                        }
                    }
                    result.Add(comment.AssociatedExceptionModel);
                }
            }
            return result;
        }

        public static IEnumerable<ThrownExceptionModel> Read(
            IAnalyzeUnit analyzeUnit,
            IExceptionsOriginModel exceptionsOrigin,
            IDeclaredElement declaredElement)
        {
            if (declaredElement == null)
            {
                return new List<ThrownExceptionModel>();
            }
            var xmlDoc = declaredElement.GetXMLDoc(true);
            if (xmlDoc == null)
            {
                return new List<ThrownExceptionModel>();
            }
            return Read(analyzeUnit, exceptionsOrigin, xmlDoc);
        }

        public static IEnumerable<ThrownExceptionModel> Read(IAnalyzeUnit analyzeUnit, ObjectCreationExpressionModel objectCreationExpression)
        {
            var declaredElement = objectCreationExpression.Node?.ConstructorReference?.Resolve()?.DeclaredElement;
            if (declaredElement == null)
            {
                return [];
            }
            var xmlDoc = declaredElement.GetXMLDoc(true);
            return xmlDoc == null ? [] : Read(analyzeUnit, objectCreationExpression, xmlDoc);
        }

        private static IEnumerable<ThrownExceptionModel> Read(IAnalyzeUnit analyzeUnit, IExceptionsOriginModel exceptionsOrigin, XmlNode xmlDoc)
        {
            var result = new List<ThrownExceptionModel>();
            var exceptionNodes = xmlDoc.SelectNodes("exception");
            if (exceptionNodes == null)
            {
                return result;
            }
            var psiModule = analyzeUnit.GetPsiModule();
            foreach (XmlNode exceptionNode in exceptionNodes)
            {
                if (exceptionNode.Attributes != null)
                {
                    var accessorNode = exceptionNode.Attributes["accessor"];
                    var accessor = accessorNode?.Value;
                    var exceptionType = exceptionNode.Attributes["cref"].Value;
                    if (exceptionType.StartsWith("T:"))
                    {
                        exceptionType = exceptionType.Substring(2);
                    }
                    var exceptionDeclaredType = TypeFactory.CreateTypeByCLRName(exceptionType, psiModule);
                    result.Add(new ThrownExceptionModel(analyzeUnit, exceptionsOrigin, exceptionDeclaredType, exceptionNode.InnerXml, false, accessor));
                }
            }
            return result;
        }

        #endregion
    }
}