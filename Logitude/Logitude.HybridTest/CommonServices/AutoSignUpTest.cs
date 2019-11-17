using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AutoSignUpTest
    {
        [TestMethod]
        public void Test_AutoSignUp_INSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("FILL ?!");
            Response serviceResponse = CallAccountingPartnerUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallAccountingPartnerUpsert()
        {
            AutoSignUpServiceReference.AutoSignUpWcfServiceClient serviceClient = new AutoSignUpServiceReference.AutoSignUpWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                AutoSignUpServiceReference.AutoSignUpData entityPM = new AutoSignUpServiceReference.AutoSignUpData()
                {
                    //FILL
                };
                Response serviceResponse = serviceClient.Insert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
