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
    public class DocumentTypeCusotmFieldsTests
    {
        [TestMethod]
        public void GetSingleDocumentTypeCusotmFieldPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeCustomFieldPM documentTypeCustomFieldPM = commonDomainService.GetSingleDocumentTypeCustomField("1-1", 1);
            Assert.AreEqual("1-1", documentTypeCustomFieldPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentTypeCusotmFieldPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeCustomFieldPM documentTypeCustomFieldPM = commonDomainService.GetSingleDocumentTypeCustomField("1-1", 1);
           
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentTypeCusotmFieldPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentTypeCustomFieldPM documentTypeCustomFieldPM = commonDomainService.GetSingleDocumentTypeCustomField("1-2", 2);
           
        }

        [TestMethod]
        public void CreateDocumentTypeCusotmFieldTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(commonContext);
            DocumentTypeCustomFieldPM newDocumentTypeCustomField = new DocumentTypeCustomFieldPM()
            {

                DocumentTypeId = "1-1",
                Id = "1-9",
                InActive = false,
                IsRequired = true,
                Tenant = tenant,
                MultiLine = true,
                IndexOrder = 3,

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();

            commonDomainService.InsertDocumentTypeCustomField(newDocumentTypeCustomField);
            DocumentTypeCustomField DocumentTypeCustomField = commonContext.DocumentTypeCustomFields.Where(d => d.Id == "1-9").FirstOrDefault();
            Assert.AreNotEqual(DocumentTypeCustomField, null);
        }

        [TestMethod]
        public void UpdateDocumentTypeCustomFieldTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            DocumentTypeCustomFieldPM documentTypeCustomFieldPM = commonDomainService.GetSingleDocumentTypeCustomField("1-1", 1);

            documentTypeCustomFieldPM.DefaultValue = "Edited value";

            commonDomainService.UpdateDocumentTypeCustomField(documentTypeCustomFieldPM);

            DocumentTypeCustomField documentTypeCustomField = commonContext.DocumentTypeCustomFields.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited value", documentTypeCustomField.DefaultValue);

        }
    }
}
