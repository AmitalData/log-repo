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
        
        [AssemblyInitialize]
        public void InitialTestMethod()
        {
            Task.Run(async () =>
            {
                
                var token = await LoginService.GetLoginTokenByUserEmailAndTenant();
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
