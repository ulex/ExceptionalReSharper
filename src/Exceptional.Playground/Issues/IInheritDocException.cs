namespace Exceptional.Playground.Issues
{
    using System;

    public interface IInheritDocException
    {
        #region methods

        /// <summary>
        /// Invokes the given action, and logs then throws an <see cref="Exception" /> if any occurs.
        /// </summary>
        /// <param name="Act">The action to invoke.</param>
        /// <exception cref="Exception">Thrown if the given <paramref name="Act" /> throws any exception.</exception>
        void InvokeAndThrow(Action Act);

        #endregion
    }
}