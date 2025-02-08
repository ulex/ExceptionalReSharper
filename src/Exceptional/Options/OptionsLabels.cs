namespace ReSharper.Exceptional.MF.Options
{
    public static class OptionsLabels
    {
        public static class InspectionLevel
        {
            #region constants

            public const string InspectInternalMethodsAndProperties = "Internal methods";
            public const string InspectPrivateMethodsAndProperties = "Private methods";
            public const string InspectProtectedMethodsAndProperties = "Protected methods";
            public const string InspectPublicMethodsAndProperties = "Public methods";
            public const string InspectConstructorsProperties = "Constructors";

            #endregion
        }

        public static class General
        {
            #region constants

            private static readonly System.Reflection.Assembly Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            private static readonly System.Diagnostics.FileVersionInfo Fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.Location);
            private static readonly string Version = Fvi.FileVersion;
            public const string DelegateInvocationsMayThrowSystemException = "Delegate invocations may throw System.Exception";

            public const string DocumentationOfThrownExceptionsSubtypeHeader = "Documentation of thrown exception's subtype is sufficient...";
            public const string IsDocumentationOfExceptionSubtypeSufficientForReferenceExpressions = "... for method or property invocations";
            public const string IsDocumentationOfExceptionSubtypeSufficientForThrowStatements = "... for throw statements";
            public static readonly string ExtensionVersion = @$"Exceptional by MF - Version: {Version}";

            #endregion
        }

        public static class ExceptionTypesAsHint
        {
            #region constants

            public const string Description = "Defines exception types which are shown as hints instead of warnings.";
            public const string Note = "Format: ExceptionType,[Always|InvocationOnly|ThrowOnly]";
            public const string ShowPredefined = "Show predefined optional exceptions";
            public const string UsePredefined = "Use predefined otional exceptions";

            #endregion
        }

        public static class ExceptionTypesAsHintForMethodOrProperty
        {
            #region constants

            public const string Description = "Define exceptions that are thrown from methods or properties which are shown as hints instead of warnings.";
            public const string Note = "Format: FullMethodOrPropertyPath,ExceptionType";
            public const string ShowPredefined = "Show predefined optional method and property exceptions";
            public const string UsePredefined = "Use predefined optional method and property exceptions";

            #endregion
        }

        public static class AccessorOverrides
        {
            #region constants

            public const string Description = "Override the property accessors for thrown exceptions of existing types. ";
            public const string Note = "Format: FullPropertyPath,ExceptionType,[get|set]";
            public const string ShowPredefined = "Show predefined accessor overrides";
            public const string UsePredefined = "Use predefined accessor overrides";

            #endregion
        }
    }
}