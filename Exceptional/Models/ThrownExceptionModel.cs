using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using ReSharper.Exceptional.Analyzers;
using ReSharper.Exceptional.Settings;

namespace ReSharper.Exceptional.Models
{
    internal class ThrownExceptionModel : ModelBase
    {
        public bool IsCaught { get; private set; }

        public bool IsDocumented { get; private set; }

        public IDeclaredType ExceptionType { get; private set; }

        public string ExceptionDescription { get; set; }

        public IExceptionsOriginModel ExceptionsOrigin { get; private set; }

        public override DocumentRange DocumentRange
        {
            get { return ExceptionsOrigin.DocumentRange; }
        }

        public ThrownExceptionModel(IAnalyzeUnit analyzeUnit, IExceptionsOriginModel exceptionsOrigin, 
            IDeclaredType exceptionType, string exceptionDescription)
            : base(analyzeUnit)
        {
            ExceptionType = exceptionType;
            ExceptionDescription = exceptionDescription;
            ExceptionsOrigin = exceptionsOrigin;

            IsCaught = CheckIfCaught();
            IsDocumented = CheckIfDocumented();
        }

        private bool CheckIfDocumented()
        {
            var docCommentBlockNode = AnalyzeUnit.DocumentationBlock;
            if (docCommentBlockNode == null)
                return false;

            return docCommentBlockNode
                .DocumentedExceptions
                .Any(m => Throws(m.ExceptionType));
        }

        private bool CheckIfCaught()
        {
            if (ExceptionType == null)
                return false;

            return ExceptionsOrigin.ContainingBlock.CatchesException(ExceptionType);
        }

        /// <summary>Checks whether this thrown exception equals to <paramref name="exceptionType"/>.</summary>
        public bool Throws(IDeclaredType exceptionType)
        {
            if (ExceptionType == null)
                return false;

            if (exceptionType == null)
                return false;

            return ExceptionType.GetClrName().ShortName.Equals(exceptionType.GetClrName().ShortName);
        }

        public override void Accept(AnalyzerBase analyzerBase)
        {
            analyzerBase.Visit(this);
        }

        public bool IsSubtypeOf(OptionalMethodExceptionConfiguration optionalMethodException, ExceptionalDaemonStageProcess process)
        {
            var exceptionType = optionalMethodException.GetExceptionType(process);
            if (exceptionType == null)
                return false;

            return ExceptionType.IsSubtypeOf(optionalMethodException.GetExceptionType(process));
        }
    }
}