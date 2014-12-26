using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using ReSharper.Exceptional;
using ReSharper.Exceptional.Highlightings;
using ReSharper.Exceptional.Models;

#if R9
using JetBrains.ReSharper.Feature.Services.Daemon;
#endif

[assembly: RegisterConfigurableSeverity(ExceptionNotDocumentedOptionalHighlighting.Id, Constants.CompoundName, HighlightingGroupIds.BestPractice,
    "Exceptional.ExceptionNotDocumentedOptional",
    "Exceptional.ExceptionNotDocumentedOptional",
    Severity.HINT,
    false)]

namespace ReSharper.Exceptional.Highlightings
{
    [ConfigurableSeverityHighlighting(Id, CSharpLanguage.Name)]
    public class ExceptionNotDocumentedOptionalHighlighting : HighlightingBase
    {
        public const string Id = "ExceptionNotDocumentedOptional";

        /// <summary>Initializes a new instance of the <see cref="ExceptionNotDocumentedOptionalHighlighting"/> class. </summary>
        /// <param name="thrownException">The thrown exception. </param>
        internal ExceptionNotDocumentedOptionalHighlighting(ThrownExceptionModel thrownException)
        {
            ThrownException = thrownException;
        }

        /// <summary>Gets the thrown exception. </summary>
        internal ThrownExceptionModel ThrownException { get; private set; }

        /// <summary>Gets the message which is shown in the editor. </summary>
        protected override string Message
        {
            get
            {
                var exceptionType = ThrownException.ExceptionType;
                var exceptionTypeName = exceptionType != null ? exceptionType.GetClrName().ShortName : "[NOT RESOLVED]";
                return Constants.OptionalPrefix + String.Format(Resources.HighlightNotDocumentedExceptions, exceptionTypeName);
            }
        }
    }
}