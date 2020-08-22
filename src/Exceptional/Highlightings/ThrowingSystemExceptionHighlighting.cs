namespace ReSharper.Exceptional.Highlightings
{
    using JetBrains.ReSharper.Feature.Services.Daemon;
    using JetBrains.ReSharper.Psi.CSharp;

    [RegisterConfigurableSeverity(
        Id,
        Constants.CompoundName,
        HighlightingGroupIds.BestPractice,
        "Exceptional.ThrowingSystemException",
        "Exceptional.ThrowingSystemException",
        Severity.SUGGESTION)]
    [ConfigurableSeverityHighlighting(Id, CSharpLanguage.Name)]
    public class ThrowingSystemExceptionHighlighting : HighlightingBase
    {
        #region constants

        public const string Id = "ThrowingSystemException";

        #endregion

        #region properties

        /// <summary>Gets the message which is shown in the editor. </summary>
        protected override string Message => string.Format(Resources.HighlightThrowingSystemException);

        #endregion
    }
}