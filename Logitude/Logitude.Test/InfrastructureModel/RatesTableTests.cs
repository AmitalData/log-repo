using Logitude.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Telerik.JustMock;
using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InfrastructureModel;
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
    public class RatesTableTests
    {
        [TestMethod]
        public void GetSingleRatesTablePMTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            RatesTablePM ratesTablePm = webFreightDomainService.GetSingleRatesTable("1-1", 1);
            Assert.AreEqual("1-1", ratesTablePm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleRatesTablePMWithAutenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            RatesTablePM ratesTablePm = webFreightDomainService.GetSingleRatesTable("1-1", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleRatesTablePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();

    

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            RatesTablePM ratesTablePm = webFreightDomainService.GetSingleRatesTable("1-1",2);
     
        }

        [TestMethod]
        public void CreateRatesTableTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();

            int tenant = 1;

            RatesTablePM newRatesTable = new RatesTablePM()
            {
                Id = "1-5",
                Tenant = tenant,


            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());


            webFreightDomainService.InsertRatesTable(newRatesTable);
            RatesTable ratesTable = webContext.RatesTable.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(ratesTable, null);


        }

        [TestMethod]
        public void UpdateRatesTableTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockWebFreightContext webContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());


            RatesTablePM ratesTablePm = webFreightDomainService.GetSingleRatesTable("1-1", 1);

            ratesTablePm.Rate = 4.5;
            webFreightDomainService.UpdateRatesTable(ratesTablePm);

            RatesTable ratesTable = webContext.RatesTable.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual(4.5, ratesTable.Rate);

        }
        [TestMethod]
        public void GetRatesTableFiltersTest()
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
            queryoperations.ObjectTableName = "RatesTable";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = false, FieldName = "Rate" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<RatesTableList> list = webFreightDomainService.GetRatesTableFilters(bytearray, 1);


        }

    }
}
