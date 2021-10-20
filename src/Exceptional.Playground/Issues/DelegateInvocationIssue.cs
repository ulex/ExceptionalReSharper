namespace Exceptional.Playground.Issues
{
    using System;
    using System.Security;

    public class DelegateInvocationIssue
    {
        #region member vars

        private Action _myAction;

        #endregion

        #region methods

        public void Foo(Action foo)
        {
            _myAction = foo; // No suggestions 
            this["foo"] = null; // No suggestion
            this["bar"](); // Suggestion => Issue: Suggestion not shown
        }

        #endregion

        #region properties

        public Action this[string i]
        {
            get => throw new SecurityException("Bar");
            set => throw new SecurityException("Bar");
        }

        #endregion
    }
}