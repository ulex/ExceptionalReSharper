// Copyright (c) 2009-2010 Cofinite Solutions. All rights reserved.
using CodeGears.ReSharper.Exceptional.Highlightings;
using CodeGears.ReSharper.Exceptional.Model;

using JetBrains.ReSharper.Daemon;

namespace CodeGears.ReSharper.Exceptional.Analyzers
{
    /// <summary>Analyzes an exception documentation and checks if it is thrown from the documented element.</summary>
    internal class IsDocumentedExceptionThrownAnalyzer : AnalyzerBase
    {
        public IsDocumentedExceptionThrownAnalyzer(ExceptionalDaemonStageProcess process) 
            : base(process) { }

        public override void Visit(ExceptionDocCommentModel exceptionDocumentationModel)
        {
            if (exceptionDocumentationModel == null) return;
            if (exceptionDocumentationModel.AnalyzeUnit.IsPublicOrInternal == false) return;
            if (AnalyzeIfExeptionThrown(exceptionDocumentationModel)) return;

            this.Process.Hightlightings.Add(new HighlightingInfo(exceptionDocumentationModel.DocumentRange, new ExceptionNotThrownHighlighting(exceptionDocumentationModel), null, null));            
        }

        private static bool AnalyzeIfExeptionThrown(ExceptionDocCommentModel exceptionDocumentationModel)
        {
            foreach (var throwStatementModel in exceptionDocumentationModel.AnalyzeUnit.ThrownExceptionModelsNotCatched)
            {
                if (throwStatementModel.Throws(exceptionDocumentationModel.ExceptionType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}