using System;

namespace Exceptional.Playground.Fixed
{
    public class InspectConstructor
    {
        public InspectConstructor()
        {
            throw new NotImplementedException("Bar"); // no warning (Constructor)
        }

    }
}
