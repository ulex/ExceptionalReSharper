namespace ReSharper.Exceptional.Models
{
    using Analyzers;

    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.Modules;
    using JetBrains.ReSharper.Psi.Tree;

    internal abstract class AnalyzeUnitModelBase<T> : BlockModelBase<T>,
        IAnalyzeUnit
        where T : ITreeNode
    {
        #region constructors and destructors

        protected AnalyzeUnitModelBase(IAnalyzeUnit analyzeUnit, T node) : base(analyzeUnit, node)
        {
            DocumentationBlock = new DocCommentBlockModel(this, null);
        }

        #endregion

        #region explicit interfaces

        public override void Accept(AnalyzerBase analyzer)
        {
            DocumentationBlock?.Accept(analyzer);
            base.Accept(analyzer);
        }

        public DocCommentBlockModel DocumentationBlock { get; set; }

        public IPsiModule GetPsiModule()
        {
            return Node.GetPsiModule();
        }

        public bool IsInspectionRequired
        {
            get
            {
                var accessRightsOwner = Node as IAccessRightsOwner;
                if (accessRightsOwner == null)
                {
                    return false;
                }
                var inspectPublicMethods = ServiceLocator.Settings.InspectPublicMethods;
                var inspectInternalMethods = ServiceLocator.Settings.InspectInternalMethods;
                var inspectProtectedMethods = ServiceLocator.Settings.InspectProtectedMethods;
                var inspectPrivateMethods = ServiceLocator.Settings.InspectPrivateMethods;
                var rights = accessRightsOwner.GetAccessRights();
                return rights == AccessRights.PUBLIC && inspectPublicMethods || rights == AccessRights.INTERNAL && inspectInternalMethods
                                                                             || rights == AccessRights.PROTECTED && inspectProtectedMethods
                                                                             || rights == AccessRights.PRIVATE && inspectPrivateMethods;
            }
        }

        ITreeNode IAnalyzeUnit.Node => Node;

        #endregion
    }
}