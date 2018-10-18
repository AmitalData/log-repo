using System;
using System.Linq;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.CommonDataModel.Repositories;

using Telerik.JustMock;

using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class DocumentTypeTemplateTests
    {
        [TestMethod]
        public void GetSingleDocumentTypeTemplateTestPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeTemplatePM documentTypeTemplatePM = commonDomainService.GetSingleDocumentTypeTemplate("1-1", 1);
            Assert.AreEqual("1-1", documentTypeTemplatePM.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentTypeTemplatePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeTemplatePM documentTypeTemplatePM = commonDomainService.GetSingleDocumentTypeTemplate("1-1", 1);
         
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentTypeTemplatePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeTemplatePM documentTypeTemplatePM = commonDomainService.GetSingleDocumentTypeTemplate("1-2", 2);
         
        }

        [TestMethod]
        public void CreateDocumentTypeTemplateTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(commonContext);
            CacheManager.CacheWrapper = new MockCacheWrapper();
            DocumentTypeTemplatePM newDocumentTypeTemplate = new DocumentTypeTemplatePM()
            {

                DocumentTypeId = "1-1",
                Id = "1-9",
                LastUpdatedByUserId = "1-1",
                Tenant = tenant,
                LastUpdateByUserName = "user1",
                IsDefault = true,
                InActive = false,
                EditorTool ="5",

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();

            commonDomainService.InsertDocumentTypeTemplate(newDocumentTypeTemplate);
            DocumentTypeTemplate documentTypeTemplate = commonContext.DocumentTypeTemplates.Where(d => d.Id == "1-9").FirstOrDefault(); 
            Assert.AreNotEqual(documentTypeTemplate, null);
        }

        [TestMethod]
        public void UpdateDocumentTypeTemplateTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            DocumentTypeTemplatePM documentTypeTemplatePM = commonDomainService.GetSingleDocumentTypeTemplate("1-1", 1);

            documentTypeTemplatePM.Description = "Edited Description";

            commonDomainService.UpdateDocumentTypeTemplate(documentTypeTemplatePM);

            DocumentTypeTemplate documentTypeTemplate = commonContext.DocumentTypeTemplates.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited Description", documentTypeTemplate.Description);

        }




    }
}
