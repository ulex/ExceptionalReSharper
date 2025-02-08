
namespace Exceptional.Playground
{
    using System;
    using System.Security;

    public class CatchAllClause
    {
        #region methods
        
        public CatchAllClause()
        {
            throw new SecurityException();
        }

        public void Test01()
        {
            try
            {
                throw new SecurityException();
            }
            catch (Exception exception) // Suggestion: When catching System.Exception a warning should be shown
            {
                Console.WriteLine(exception.Message); // Warning: IOException not documented
            }
        }

        public void Test02()
        {
            try
            {
                throw new SecurityException();
            }
            catch (Exception) // Two warnings: Same as above and one from ReSharper
            {
                // ignored
            }
        }

        public void Test03()
        {
            try
            {
                throw new SecurityException();
            }
            catch // Two warnings: Same as above and one from ReSharper
            {
                // ignored
            }
        }

        public void Test04()
        {
            try
            {
                throw new ArgumentNullException();
            }
            catch (Exception e) when (e is ArgumentNullException)
            {
                // there shouldn't be any warnings in the catch
            }
        }

        public void Test05()
        {
            try
            {
                throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw; // one warning about un-documented Exception
            }
        }

        /// <exception cref="Exception"></exception>
        public void Test06()
        {
            try
            {
                throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw; // no warnings
            }
        }

        #endregion
    }
}