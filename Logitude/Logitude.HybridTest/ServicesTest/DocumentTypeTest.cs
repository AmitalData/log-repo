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
        [TestMethod]
        public void Test_DocumentType_UPSERT()
        {
            DocumentTypePM documentTypePM = new DocumentTypePM()
            {
                Code = HybridData.DocumentTypeCodeHDT,
                Name = "Hybrid DocumentType",
                ObjectTableName = "Shipment",
                DocumentTypeCategoryCode = "O",
                IsDocIn = true,
                IsDocOut = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(documentTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_DocumentType_GetDocumentTypeByCode()
        {
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
            Test_DocumentType_UPSERT();
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
