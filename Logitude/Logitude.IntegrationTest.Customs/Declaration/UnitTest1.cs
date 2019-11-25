using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Logitude.IntegrationTest.Customs.Declaration
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestInitialize]
        public void InitialTestMethod()
        {
            Task.Run(async () =>
            {
                string email = "angular@fnarsoft.com";
                string pass = "1";
                var token = await LoginService.GetLoginTokenByUserEmailAndTenant(email, pass);
                Assert.IsNotNull(token);

            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void TestMethod1()
        {
           //this will be taken to master
          
            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync("Declarations/getsingle?id=1-1");
                var stringResult = response.Content.ReadAsStringAsync().Result;
                DeclarationPM declarationPM = JsonConvert.DeserializeObject<DeclarationPM>(stringResult);
                Assert.AreEqual("1-1", declarationPM.Id);
                

            }).GetAwaiter().GetResult();
            
        }

        [TestMethod]
        public void TestMethod2()
        {
            //this will be taken to master

            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync("Declarations/getsingle?id=1-1");
                var stringResult = response.Content.ReadAsStringAsync().Result;
                DeclarationPM declarationPM = JsonConvert.DeserializeObject<DeclarationPM>(stringResult);
                Assert.AreEqual("1-1", declarationPM.Id);


            }).GetAwaiter().GetResult();

        }
    }
}
