using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
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
            DocumentTypePM documentTypePM = new DocumentTypePM()
            {
                Code = HybridData.DocumentTypeCodeHDT,
                Name = "Hybrid DocumentType",
                ObjectTableName = "Shipment",
                DocumentTypeCategoryCode = "O",
                IsDocIn = true,
                IsDocOut = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(documentTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.DocumentTypeIdHDT = serviceResponse.Result;
        }
        [TestMethod]
        public void Test_DocumentType_GetDocumentTypeByCode()
        {
            if(HybridData.DocumentTypeIdHDT == null)
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
            object[] serviceParameters = new object[] { HybridData.DocumentTypeCodeHDT, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            DocumentTypePM documentType = (DocumentTypePM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Document Type By Code Failed! " + serviceResponse.Result);
            Assert.AreEqual(documentType.Code, HybridData.DocumentTypeCodeHDT, "Get Document Type By Code Failed!");
        }
        [TestMethod]
        public void Test_DocumentType_GetDocumentTypes()
        {
            if (HybridData.DocumentTypeIdHDT == null)
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
            object[] serviceParameters;
            int skip = 0;
            bool DocumentTypeExist = false;
            do
            {
                serviceParameters = new object[] { "Shipment", TestEnvironmentGlobalParameters.Tenant, 0 + skip, 10, serviceResponse };
                documentTypes = (DocumentTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
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
            Assert.IsFalse(serviceResponse.HasError, "Get Document Type By Code Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Document Type By Code Failed! " + serviceResponse.Result);
            Assert.IsTrue(DocumentTypeExist, "Get Document Type By Code Failed! Doesn't Exist!! ");
        }
    }
}
