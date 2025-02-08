using System;

namespace Exceptional.Playground.Fixed
{
    public class DisableWarnings
    {
        public void Foo()
        {
            throw new NotImplementedException("Bar"); // no warning
            // ReSharper restore once ExceptionNotDocumentedOptional
            throw new NotImplementedException("Bar"); // warning
        }
    }
}
