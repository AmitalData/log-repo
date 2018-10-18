using Logitude.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.InfrastructureModel;
using Telerik.JustMock;
using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.Helpers;
using System;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Global.Data.GlobalModel;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class VolumeUnitTests
    {

        [TestMethod]
        public void GetSingleVolumeUnitPMTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            VolumeUnitPM volumeUnitPm = webFreightDomainService.GetSingleVolumeUnit("VU", 1);
            Assert.AreEqual("VU", volumeUnitPm.Code);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleVolumeUnitPMWithAutenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);


            VolumeUnitPM volumeUnitPm = webFreightDomainService.GetSingleVolumeUnit("VU", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleVolumeUnitPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();

            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            VolumeUnitPM volumeUnitPm = webFreightDomainService.GetSingleVolumeUnit("VU",2);
      
        }
        [TestMethod]
        public void GetVolumeUnitFiltersTest()
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
            queryoperations.ObjectTableName = "VolumeUnit";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = false, FieldName = "Name" , Operator="Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<VolumeUnitList> list = webFreightDomainService.GetVolumeUnitFilters(bytearray, 1);


        }


    }
}
