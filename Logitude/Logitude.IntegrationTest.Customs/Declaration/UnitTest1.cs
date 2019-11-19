using System;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTest.Customs.Declaration
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           //this will be taken to master
          
            Task.Run(async () =>
            {
                string email = "angular@fnarsoft.com";
                string pass = "1";
                var token =await LoginService.GetLoginTokenByUserEmailAndTenant(email, pass);
                //Assert.IsNotNull(token);
                // Actual test code here.
              
                using (var client = new HttpClient())
                {
                    string getDeclarationUrl = IntegrationTestLoginParameters.ServerURL + "api/" + "Declarations/getsingle?id=1-1";
                    client.DefaultRequestHeaders.Add("Token", IntegrationTestLoginParameters.Token);
                    var result = await client.GetAsync(getDeclarationUrl);
                    var content = result.Content;
                    Assert.IsNotNull(result);
                }


            }).GetAwaiter().GetResult();
            
        }
    }
}
