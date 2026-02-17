using System;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.CommonDataModel.Repositories;


using Telerik.JustMock;
using System.Linq;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.Server.Tools.Counters;

namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class DocumentOutTests
    {
        [TestMethod]
        public void GetSingleDocumentOutPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentOutPM documentOutPM = commonDomainService.GetSingleDocumentOutPM("1-1",1);
            Assert.AreEqual("1-1", documentOutPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentOutPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentOutPM documentOutPM = commonDomainService.GetSingleDocumentOutPM("1-1", 1);
          
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleDocumentOutPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            DocumentOutPM documentOutPM = commonDomainService.GetSingleDocumentOutPM("1-2", 2);
          
        }


        [TestMethod]
        public void CreateDocumentOutTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);


            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();

            commonDomainService.CreateDocumentOut("1-1", "1-1", "1-2", "ref1", "1-1", 1);
            DocumentOut documentOut = commonContext.DocumentOuts.Where(d => d.Id == "1-9").FirstOrDefault();
            Assert.AreNotEqual(documentOut, null);

        }

        //[TestMethod]
        //public void UpdateDocumentOutTest()
        //{
        //    MockCommonContext commonContext = new MockCommonContext();
        //    CacheManager.CacheWrapper = new MockCacheWrapper();
        //    int tenant = 1;
        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    CommonDataDomainService commonDomainService = new CommonDataDomainService();
        //    //DocumentOutService service = new DocumentOutService(commonContext, tenant);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
        //    Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
        //    Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
        //    Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

        //    DocumentOutPM documentOutPM = commonDomainService.GetSingleDocumentOutPM("1-1", 1);

        //    documentOutPM.Note = "Edited Note";

        //    commonDomainService.UpdateDocumentOut(documentOutPM);

        //    DocumentOut documentOut = commonContext.DocumentOuts.Where(d => d.Id == "1-1").FirstOrDefault();

        //    Assert.AreEqual("Edited Note", documentOut.Note);

        //}


    
    }
}
