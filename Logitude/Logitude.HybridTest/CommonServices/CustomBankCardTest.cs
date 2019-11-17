using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomBankCardTest
    {
        [TestMethod]
        public void Test_CustomBankCard_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallCustomBankCardUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_CustomBankCard_DELETE()
        {
            Test_CustomBankCard_UPSERT();
            Response serviceResponse = CallCustomBankCardUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallCustomBankCardUpsert()
        {
            CustomBankCardServiceReference.CustomBankCardWcfServiceClient serviceClient = new CustomBankCardServiceReference.CustomBankCardWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CustomBankCardServiceReference.CustomBankPM entityPM = new CustomBankCardServiceReference.CustomBankPM()
                {
                    BankCode = HybridCodes.BankCode,
                    EnglishName = "Hybrid Custom Bank",
                    LocalName = "Hybrid Custom Bank",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,

                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        public static Response CallCustomBankCardDelete()
        {
            CustomBankCardServiceReference.CustomBankCardWcfServiceClient serviceClient = new CustomBankCardServiceReference.CustomBankCardWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.Delete(HybridCodes.BankCode, "?", TestEnvironmentGlobalParameters.Tenant);
                return serviceResponse;
            }
        }
    }
}