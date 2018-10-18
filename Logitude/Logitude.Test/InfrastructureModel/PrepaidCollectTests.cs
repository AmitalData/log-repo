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
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class PrepaidCollectTests
    {
        [TestMethod]
        public void GetSinglePrepaidCollectPMTest()
        {
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            PrepaidCollectPM prepaidCollectPM = webFreightDomainService.GetSinglePrepaidCollect("1-1", 1);
            Assert.AreEqual("1-1", prepaidCollectPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSinglePrepaidCollectPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());


            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            PrepaidCollectPM prepaidCollectPM = webFreightDomainService.GetSinglePrepaidCollect("1-1", 1);
           
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSinglePrepaidCollectPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockWebFreightContext webContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            PrepaidCollectPM prepaidCollectPM = webFreightDomainService.GetSinglePrepaidCollect("1-1", 2);
           
        }

        [TestMethod]
        public void GetPrepaidCollectFiltersTest()
        {


            MockCommonContext commonContext = new MockCommonContext();
            GeneralDomainService generalDomainService = new GeneralDomainService();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();


            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "PrepaidCollect";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = false, FieldName = "DisplayInLOV", FieldValue = false, Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<PrepaidCollectList> list = webFreightDomainService.GetPrepaidCollectFilters(bytearray, 1);


        }

    }
}
