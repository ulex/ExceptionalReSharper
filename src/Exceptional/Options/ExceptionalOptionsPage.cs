namespace ReSharper.Exceptional.MF.Options
{
    using JetBrains.Application.UI.Options;
    using JetBrains.Application.UI.Options.OptionsDialog;

    [OptionsPage(Pid, Name, null, Sequence = 5.0)]
    public class ExceptionalOptionsPage : AEmptyOptionsPage
    {
        #region constants

        public const string Name = "Exceptional - MF";
        public const string Pid = "Exceptional";

        #endregion
    }
}