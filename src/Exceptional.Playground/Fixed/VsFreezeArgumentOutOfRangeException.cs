namespace Exceptional.Playground.Fixed
{
    using System;

    public class VsFreezeArgumentOutOfRangeException
    {
        #region methods

        public static void Test(int val)
        {
            switch (val)
            {
                case 1:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(val));
            }
        }

        #endregion
    }
}