using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class DocumentTypeTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_DocumentType_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                DocumentTypePM documentTypePM = new DocumentTypePM()
                {
                    Code = HybridData.DocumentTypeCodeHDT,
                    Name = "Hybrid DocumentType",
                    ObjectTableName = "Shipment",
                    DocumentTypeCategoryCode = "O",
                    IsDocIn = true,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(documentTypePM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_DocumentType_GetDocumentTypeByCode()
        {
            Assert.Inconclusive("cache prob!");
            Test_DocumentType_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "DocumentType",
                ServiceOperation = "GetDocumentTypeByCode",
                ServiceResponseIndex = 2,
                ServiceType = typeof(DocumentTypePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.DocumentTypeCodeHDT, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            DocumentTypePM documentType = (DocumentTypePM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Document Type By Code Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Document Type By Code Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(documentType.Code, HybridData.DocumentTypeCodeHDT, "Get Document Type By Code Failed!");
        }

        [TestMethod]
        public void Test_DocumentType_GetDocumentTypes()
        {
            Assert.Inconclusive("upsert document type to prepare vars!");
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "DocumentType",
                ServiceOperation = "GetDocumentTypes",
                ServiceResponseIndex = 4,
                ServiceType = typeof(DocumentTypeList),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            DocumentTypeList[] documentTypes;
            ServiceOutcome serviceOutcome;
            object[] serviceParameters;
            int skip = 0;
            bool DocumentTypeExist = false;
            do
            {
                serviceParameters = new object[] { "Shipment", EnvironmentGlobalParams.MainTenant, 0 + skip, 10, serviceResponse };
                serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
                documentTypes = (DocumentTypeList[])serviceOutcome.Result;

                foreach (DocumentTypeList documentType in documentTypes)
                {
                    if (documentType.Code == HybridData.DocumentTypeCodeHDT)
                    {
                        DocumentTypeExist = true;
                        break;
                    }
                }
                skip += 10;
            } while (!DocumentTypeExist && documentTypes.Length != 0);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Document Type By Code Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Document Type By Code Failed! " + serviceOutcome.Response.Result);
            Assert.IsTrue(DocumentTypeExist, "Get Document Type By Code Failed! Doesn't Exist!! ");
        }
    }
}
