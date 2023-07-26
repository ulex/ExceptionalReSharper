namespace ReSharper.Exceptional.MF.Models
{
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    internal sealed class CatchVariableModel : TreeElementModelBase<ICatchVariableDeclaration>
    {
        #region constructors and destructors

        public CatchVariableModel(IAnalyzeUnit analyzeUnit, ICatchVariableDeclaration catchVariableDeclaration) : base(analyzeUnit, catchVariableDeclaration)
        {
        }

        #endregion

        #region properties

        public ICSharpIdentifier VariableName => Node.NameIdentifier;

        #endregion
    }
}