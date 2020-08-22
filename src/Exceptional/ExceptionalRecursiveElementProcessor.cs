namespace ReSharper.Exceptional
{
    using System.Collections.Generic;

    using Contexts;

    using JetBrains.ReSharper.Daemon.CSharp.Stages;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    using Models;

    public class ExceptionalRecursiveElementProcessor : IRecursiveElementProcessor
    {
        #region member vars

        private readonly CSharpDaemonStageProcessBase _daemonProcess;
        private readonly List<IDocCommentBlock> _eventComments = new List<IDocCommentBlock>();

        private IProcessContext _currentContext;

        #endregion

        #region constructors and destructors

        public ExceptionalRecursiveElementProcessor(CSharpDaemonStageProcessBase daemonProcess)
        {
            _daemonProcess = daemonProcess;
            _currentContext = new NullProcessContext();
        }

        #endregion

        #region explicit interfaces

        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessAfterInterior(ITreeNode element)
        {
            if (element is IMethodDeclaration)
            {
                _currentContext.RunAnalyzers();
            }
            else if (element is IEventDeclaration)
            {
                _currentContext.RunAnalyzers();
            }
            else if (element is IAccessorOwnerDeclaration)
            {
                _currentContext.RunAnalyzers();
            }
            else if (element is IAccessorDeclaration)
            {
                //_currentContext.RunAnalyzers(_daemonProcess, _settings); // Already analyzed by accessor owner
                _currentContext.LeaveAccessor();
            }
            else if (element is IConstructorDeclaration)
            {
                _currentContext.RunAnalyzers();
            }
            else if (element is ITryStatement)
            {
                _currentContext.LeaveTryBlock();
            }
            else if (element is ICatchClause)
            {
                _currentContext.LeaveCatchClause();
            }
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
            if (element is IThrowStatement)
            {
                _currentContext.Process(element as IThrowStatement);
            }
            else if (element is ICatchVariableDeclaration)
            {
                _currentContext.Process(element as ICatchVariableDeclaration);
            }
            else if (element is IReferenceExpression)
            {
                _currentContext.Process(element as IReferenceExpression);
            }
            else if (element is IObjectCreationExpression)
            {
                _currentContext.Process(element as IObjectCreationExpression);
            }
            if (element is IMethodDeclaration)
            {
                var methodDeclaration = element as IMethodDeclaration;
                _currentContext = new MethodProcessContext();
                _currentContext.StartProcess(new MethodDeclarationModel(methodDeclaration));
            }
            else if (element is IConstructorDeclaration)
            {
                var constructorDeclaration = element as IConstructorDeclaration;
                _currentContext = new ConstructorProcessContext();
                _currentContext.StartProcess(new ConstructorDeclarationModel(constructorDeclaration));
            }
            else if (element is IEventDeclaration)
            {
                var eventDeclaration = element as IEventDeclaration;
                _currentContext = new EventProcessContext();
                _currentContext.StartProcess(new EventDeclarationModel(eventDeclaration));
                foreach (var doc in _eventComments)
                {
                    _currentContext.Process(doc);
                }
                _eventComments.Clear();
            }
            else if (element is IAccessorOwnerDeclaration)
            {
                var accessorOwnerDeclaration = element as IAccessorOwnerDeclaration;
                _currentContext = new AccessorOwnerProcessContext();
                _currentContext.StartProcess(new AccessorOwnerDeclarationModel(accessorOwnerDeclaration));
            }
            else if (element is IAccessorDeclaration)
            {
                _currentContext.EnterAccessor(element as IAccessorDeclaration);
            }
            else if (element is IDocCommentBlock)
            {
                if (_currentContext.Model == null || _currentContext.Model.Node == element.Parent)
                {
                    _currentContext.Process(element as IDocCommentBlock);
                }
                else
                {
                    _eventComments.Add((IDocCommentBlock)element);
                    // HACK: Event documentation blocks are processed before event declaration, 
                    // other documentation blocks are processed after the associated element declaration
                }
            }
            else if (element is IThrowExpression)
            {
                _currentContext.Process(element as IThrowExpression);
            }
            else if (element is ITryStatement)
            {
                _currentContext.EnterTryBlock(element as ITryStatement);
            }
            else if (element is ICatchClause)
            {
                _currentContext.EnterCatchClause(element as ICatchClause);
            }
        }

        public bool ProcessingIsFinished => ServiceLocator.Process.InterruptFlag;

        #endregion
    }
}