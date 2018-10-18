using System;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;


using Telerik.JustMock;
using System.Linq;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;


namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void GetSingleDocumentTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Document document = commonDomainService.GetDocumentById("1-1", 1);
            Assert.AreEqual("1-1", document.Id);
           
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Document document = commonDomainService.GetDocumentById("1-1", 1);
          
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Document document = commonDomainService.GetDocumentById("1-2", 2);
          
        }

        [TestMethod]
        public void CreateDocumentTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            Document newDocument = new Document()
            {

                
                Id = "1-9",
               
                Tenant = tenant,

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();

            commonDomainService.InsertDocument(newDocument);
            Document documentOut = commonContext.Documents.Where(d => d.Id == "1-9").FirstOrDefault(); 
            Assert.AreNotEqual(documentOut, null);
        }

        [TestMethod]
        public void UpdateDocumentTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            Document document = commonDomainService.GetDocumentById("1-1", 1);

            document.FileSize = 100;
            commonDomainService.UpdateDocument(document);

            Document documenT = commonContext.Documents.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual(100, documenT.FileSize);

        }
    }
}
