using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AccountingPartnerTest
    {
        [TestMethod]
        public void Test_AccountingPartner_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallAccountingPartnerUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallAccountingPartnerUpsert()
        {

            AccountingPartnerServiceReference.AccountingPartnerWcfServiceClient serviceClient = new AccountingPartnerServiceReference.AccountingPartnerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                AccountingPartnerServiceReference.AccountingPartnerPM entityPM = new AccountingPartnerServiceReference.AccountingPartnerPM()
                {
                    Code = HybridCodes.AccountingPartnerCode,
                    EnglishName = "Hybrid AccountingPartner",
                    LocalName = "Hybrid AccountingPartner",
                    CityName = "Hybrid City",
                    CountryCode = HybridCodes.CountryCode,
                    PartnerTypeId = "AG",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
