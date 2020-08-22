namespace ReSharper.Exceptional.Models
{
    using Analyzers;

    using JetBrains.ReSharper.Psi.Modules;
    using JetBrains.ReSharper.Psi.Tree;

    internal interface IAnalyzeUnit : IBlockModel
    {
        #region methods

        void Accept(AnalyzerBase analyzer);

        IPsiModule GetPsiModule();

        #endregion

        #region properties

        DocCommentBlockModel DocumentationBlock { get; set; }

        bool IsInspectionRequired { get; }

        ITreeNode Node { get; }

        #endregion
    }
}