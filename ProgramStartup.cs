using NUnit.Framework;
using elbit;

namespace elbit
{
    public class ProgramStartup
    {
        [OneTimeSetUp]
        public void RunOnceProgremStartup()
        {
            Globals.LoadConfiguration();
            
        }
        [OneTimeTearDown]
        public void RunOnceProgremShutDown()
        {

        }
    }
}
