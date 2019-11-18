using System;
using Logitude.Server.Tools;
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
            Response serviceResponse = CallDocumentTypeUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallDocumentTypeUpsert()
        {
            DocumentTypeServiceReference.DocumentTypeWcfServiceClient serviceClient = new DocumentTypeServiceReference.DocumentTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
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
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_DocumentType_GetDocumentTypeByCode()
        {
            Test_DocumentType_UPSERT();
            DocumentTypeServiceReference.DocumentTypeWcfServiceClient serviceClient = new DocumentTypeServiceReference.DocumentTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                DocumentTypeServiceReference.DocumentTypePM entityPM = serviceClient.GetDocumentTypeByCode(HybridCodes.DocumentTypeCode, TestEnvironmentGlobalParameters.Tenant,ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    Assert.AreEqual(entityPM.Code, HybridCodes.DocumentTypeCode, "Get Document Type By Code Failed! ");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Document Type With This Code!");
                }
            }
        }

        [TestMethod]
        public void Test_DocumentType_GetDocumentTypes()
        {
            Test_DocumentType_UPSERT();
            DocumentTypeServiceReference.DocumentTypeWcfServiceClient serviceClient = new DocumentTypeServiceReference.DocumentTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Boolean foundDocumentType = false;
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                DocumentTypeServiceReference.DocumentTypeList[] serviceResult = null;
                int skip = 0;
                do {
                   serviceResult = serviceClient.GetDocumentTypes("Shipment", TestEnvironmentGlobalParameters.Tenant, 0+skip, 10, ref serviceResponse);
                    foreach (DocumentTypeServiceReference.DocumentTypeList documentType in serviceResult)
                    {
                        if (documentType.Code == HybridCodes.DocumentTypeCode)
                        {
                            foundDocumentType = true;
                            break;
                        }
                    }
                    skip += 10;
                } while (!foundDocumentType && serviceResult.Length !=0);
                Assert.IsTrue(foundDocumentType, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
                Assert.IsFalse(serviceResponse.HasError, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
            }
        }
    }
}
