using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium
using RestSharp;
using NLog;
using elbit.TestTools.Selenium;
using elbit.TestTools.MongoDB;
using elbit.TestTools.RestAPI;
using System.Collections.Generic

namespace elbit.test
{
    [TestFixture]
    // the resposibilility for making your test thread safe is yours
    //all tools here in BaseTest inherited from child test class ,are already thread safe
    //[Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class BaseTest
    {
        private WebDriverWrapper _webDriver;
        protected WebDriverWrapper WebDriver
        {
            get { 
                if(_webDriver ==null)
                {
                    _webDriver=new WebDriverWrapper(WebDriverType.Chrome);
                }
                return _webDriver; 
            }
        }
        protected MongoCollectionWrapper GetMongoCollection(string databaseName,string collectionName)
        {
            return MongoDriverManager.GetMongoCollection(databaseName, collectionName);
        }
        protected List<string> GetCollectionNames(string databaseNames) 
        { 
            return MongoDriverManager.GetCollectionNames(databaseNames);
        }
        private RestApiClientWrapper _resrClient;
        protected RestApiClientWrapper RestClient
        {
            get
            {
                if(_resrClient == null)
                {
                    _resrClient = new RestApiClientWrapper()
                }
                return _resrClient;
            }
        }
        protected Logger logger;
        public BaseTest()
        {

        }
        [SetUp]
        public virtual void BeforeEachTest() 
        { 
            logger=LogManager.GetLogger(GetType().ToString());
            logger.Info($"initiales test tools for test '{TestContext.CurrentContext.Test.MethodName}'")
        }
        [TearDown]
        public void AfterEachTest() 
        { 
            if(TestContext.CurrentContext.Result.FailCount>0 && _webDriver == null) 
            { 
                //cap
            }
        }
        private void DisposeTestTools()
        {
            if(_webDriver!= null) 
            {
                _webDriver.Driver.Dispose();
            }
        }
        protected void CaptureScreenshot()
        {
            var pic=(WebDriver.Driver as ITakesScreenshot).GetScreenshot();
            string filepath=Guid.NewGuid().ToString();
            pic.SaveAsFile(filepath,ScreenshotImageFormat.Png);
            TestContext.AddTestAttachment(filepath);
        }

    }
}
