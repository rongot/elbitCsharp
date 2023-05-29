using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using elbit.TestTools.RestAPI

namespace elbit.test
{
    [Category("sample")]
    public class SampleTest:BaseTest
    {
        [Test]
        public void SampleGetApi()
        {
            //you can change the baseUrl (recieved from configuration file )before sending request
            RestClient.RestApiClient.Options.BaseUrl = new System.Uri("http://git");
            // here is get request without body
            var res = RestClient.sendGetRequest("onesim/autotesting");
            var res2 = RestClient.sendGetRequest("onesim/autotesting","bla ba",RequestBodyFormat.Json);

        }
        [Test]
        public void SamplePostApi()
        {

        }
    }
}
