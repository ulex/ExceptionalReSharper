using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptional.Playground.Issues
{
    public class InheritDocException : IInheritDocException
    {
        /// <inheritdoc cref="IInheritDocException"/>
        public void InvokeAndThrow(Action Act)
        {
            throw new Exception();
        }
    }
}
