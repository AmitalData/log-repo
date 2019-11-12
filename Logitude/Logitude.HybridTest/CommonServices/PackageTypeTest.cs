using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class PackageTypeTest
    {
        [TestMethod]
        public void Test_PackageType_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response serviceResponse = CallPackageTypeUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallPackageTypeUpsert()
        {

            PackageTypeServiceReference.PackageTypeWcfServiceClient serviceClient = new PackageTypeServiceReference.PackageTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                PackageTypeServiceReference.PackageTypePM entityPM = new PackageTypeServiceReference.PackageTypePM()
                {
                    Code = HybridCodes.PackageTypeCode,
                    EnglishName = "Hybrid PackageType",
                    LocalName = "Hybrid PackageType",
                    AddedManually = true,
                    PrintAs = "Hybrid PackageType",
                    IsAir = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
