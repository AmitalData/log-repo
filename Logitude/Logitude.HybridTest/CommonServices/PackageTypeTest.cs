using System;
using Logitude.Server.Tools;
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
            Response serviceResponse = CallPackageTypeUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_PackageType_GETPACKAGETYPELIST()
        {
            LoginService.GetLoginTokenByCredentials();
            PackageTypeServiceReference.PackageTypeWcfServiceClient serviceClient = new PackageTypeServiceReference.PackageTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters();
                filters.Take = 10;
                filters.SearchFields = HybridData.PackageTypeCode;
                PackageTypeServiceReference.PackageTypeList[] serviceResult = serviceClient.GetPackageTypeList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    string packageTypeCode = serviceResult[0].Code;
                    Assert.IsTrue(packageTypeCode == "HPT", "Hybrid Package Type Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't package Type With This Code!");
                }
            }
        }

        public static Response CallPackageTypeUpsert()
        {
            PackageTypeServiceReference.PackageTypeWcfServiceClient serviceClient = new PackageTypeServiceReference.PackageTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                PackageTypeServiceReference.PackageTypePM entityPM = new PackageTypeServiceReference.PackageTypePM()
                {
                    Code = HybridData.PackageTypeCode,
                    EnglishName = "Hybrid PackageType",
                    LocalName = "Hybrid PackageType",
                    AddedManually = true,
                    PrintAs = "Hybrid PackageType",
                    IsAir = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
