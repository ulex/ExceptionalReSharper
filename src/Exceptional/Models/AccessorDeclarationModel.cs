namespace ReSharper.Exceptional.MF.Models
{
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    internal sealed class AccessorDeclarationModel : BlockModelBase<IAccessorDeclaration>
    {
        #region constructors and destructors

        public AccessorDeclarationModel(IAnalyzeUnit analyzeUnit, IAccessorDeclaration node, IBlockModel parentBlock) : base(analyzeUnit, node)
        {
            ParentBlock = parentBlock;
        }

        #endregion

        #region properties

        /// <summary>Gets the content block of the object. </summary>
        public override IBlock Content => Node.Body;

        #endregion
    }
}