using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class DocumentTypeTest
    {
        [TestMethod]
        public void Test_DocumentType_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response serviceResponse = CallDocumentTypeUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallDocumentTypeUpsert()
        {

            DocumentTypeServiceReference.DocumentTypeWcfServiceClient serviceClient = new DocumentTypeServiceReference.DocumentTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                DocumentTypeServiceReference.DocumentTypePM entityPM = new DocumentTypeServiceReference.DocumentTypePM()
                {
                    Code = HybridCodes.DocumentTypeCode,
                    Name = "Hybrid DocumentType",
                    ObjectTableName = "Shipment",
                    DocumentTypeCategoryCode = "O",
                    IsDocIn = true,
                    IsDocOut = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,

                };
                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
