namespace ReSharper.Exceptional.MF.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Analyzers;

    using JetBrains.ReSharper.Psi.CSharp.Tree;

    internal sealed class EventDeclarationModel : AnalyzeUnitModelBase<IEventDeclaration>
    {
        #region constructors and destructors

        public EventDeclarationModel(IEventDeclaration node) : base(null, node)
        {
            Accessors = new List<AccessorDeclarationModel>();
        }

        #endregion

        #region methods

        /// <summary>Analyzes the object and its children. </summary>
        /// <param name="analyzer">The analyzer. </param>
        public override void Accept(AnalyzerBase analyzer)
        {
            foreach (var accessorDeclarationModel in Accessors)
            {
                accessorDeclarationModel.Accept(analyzer);
            }
            base.Accept(analyzer);
        }

        #endregion

        #region properties

        public List<AccessorDeclarationModel> Accessors { get; }

        /// <summary>Gets the content block of the object. </summary>
        public override IBlock Content => null;

        /// <summary>Gets the list of not caught thrown exceptions. </summary>
        public override IEnumerable<ThrownExceptionModel> UncaughtThrownExceptions
        {
            get
            {
                return Accessors.SelectMany(m => m.UncaughtThrownExceptions);
            }
        }

        #endregion
    }
}