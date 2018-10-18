//using System;

//using Logitude.Test.Helpers;

//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using Simplog.Data.CommonDataModel;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.CommonDataModel.Mocks;
//using Simplog.Data.CommonDataModel.Repositories;

//using Telerik.JustMock;
//using System.Linq;

//using WebFreight.Web.CommonDataModel.DomainServices;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;
//using Simplog.Server.Infrastructure.Helpers;
//using Simplog.Data.Helpers;
//using Simplog.Data.InfrastructureModel;
//using WebFreight.Web.GlobalModelDB;
//using Simplog.Global.Data.GlobalModel;
//using WebFreight.Web.DataContracts;
//using System.Collections.Generic;
//using System.IO;
//using System.Xml.Serialization;
//using System.Text;
//using Logitude.Server.Tools.Counters;

//namespace Logitude.Test.CommonDataModel
//{
//    [TestClass]
//    public class DocumentInTests
//    {
//        [TestMethod]
//        public void GetSingleDocumentInPMTest()
//        {
//            MockCommonContext commonContext = new MockCommonContext();
//            CommonDataDomainService commonDomainService = new CommonDataDomainService();
//            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

//            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
//            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
//            DocumentInPM documentInPM = commonDomainService.GetSingleDocumentInPM("1-1",1);
//            Assert.AreEqual("1-1", documentInPM.Id);
//        }


//        [TestMethod]
//        [ExpectedException(typeof(AutenticationException))]
//        public void GetSingleDocumentInPMWithAuthenticationOnTenantTest()
//        {
//            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
//            MockCommonContext commonContext = new MockCommonContext();
//            CacheManager.CacheWrapper = new MockCacheWrapper();
//            CommonDataDomainService commonDomainService = new CommonDataDomainService();
//            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

//            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
//            DocumentInPM documentInPM = commonDomainService.GetSingleDocumentInPM("1-1", 1);
          
//        }


//        [TestMethod]
//        [ExpectedException(typeof(AutenticationException))]
//        public void GetSingleDocumentInPMFromAnoterTenantTest()
//        {
//            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
//            MockCommonContext commonContext = new MockCommonContext();
//            CacheManager.CacheWrapper = new MockCacheWrapper();
//            CommonDataDomainService commonDomainService = new CommonDataDomainService();
//            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

//            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
//            DocumentInPM documentInPM = commonDomainService.GetSingleDocumentInPM("1-2", 2);
         
//        }


//        [TestMethod]
//        public void CreateDocumentInTest()
//        {

//            MockCommonContext commonContext = new MockCommonContext();
//            int tenant = 1;
//            CommonDataDomainService commonDomainService = new CommonDataDomainService();
//            DocumentInRepository documentInRepository = new DocumentInRepository(commonContext);

//            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


//            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
//            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
//            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
//            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
//            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
//            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();

//            commonDomainService.CreateDocumentIn("1-1","1-1", "1-2", "ref1", "1-1", 1);
//            DocumentIn documentIn = commonContext.DocumentIns.Where(d => d.Id == "1-9").FirstOrDefault();
//            Assert.AreNotEqual(documentIn, null);
//        }

//        [TestMethod]
//        public void UpdateDocumentInTest()
//        {
//            MockCommonContext commonContext = new MockCommonContext();
//            CacheManager.CacheWrapper = new MockCacheWrapper();

//            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
//            CommonDataDomainService commonDomainService = new CommonDataDomainService();
//            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
//            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
//            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
//            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
//            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

//            DocumentInPM documentInPM = commonDomainService.GetSingleDocumentInPM("1-1", 1);

//            documentInPM.Notes = "Edited Note";
//            commonDomainService.UpdateDocumentIn(documentInPM);

//            DocumentIn documentIn = commonContext.DocumentIns.Where(d => d.Id == "1-1").FirstOrDefault();

//            Assert.AreEqual("Edited Note", documentIn.Notes);

//        }



//    }
//}
