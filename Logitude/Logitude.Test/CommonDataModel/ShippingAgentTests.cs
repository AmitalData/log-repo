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
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;


namespace Logitude.Test.CommonDataModel
{
    [TestClass]
    public class ShippingAgentTests
    {
        [TestMethod]
        public void GetSingleShippingAgentPMTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShippingAgentPM shippingAgentPM = partnerDomainService.GetShippingAgentById("1-1", 1);
            Assert.AreEqual("1-1", shippingAgentPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShippingAgentPMWithAutenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShippingAgentPM shippingAgentPM = partnerDomainService.GetShippingAgentById("1-1", 1);
         
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShippingAgentPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShippingAgentPM shippingAgentPM = partnerDomainService.GetShippingAgentById("1-1", 2);
        }


        [TestMethod]
        public void CreateShippingAgentTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            int tenant = 1;
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            ShippingAgentRepository shippingAgentRepository = new ShippingAgentRepository(commonContext);
            CacheManager.CacheWrapper = new MockCacheWrapper();
            ShippingAgentPM newShippingAgent = new ShippingAgentPM()
            {


                Id = "1-9",

                Tenant = tenant,


            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => CodeCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns(1);

            partnerDomainService.InsertShippingAgent(newShippingAgent);
            ShippingAgent shippingAgent = commonContext.ShippingAgents.Where(d => d.Id == "1-9").FirstOrDefault();
            Assert.AreNotEqual(shippingAgent, null);
        }

        [TestMethod]
        public void UpdateShippingAgentTest()
        {
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            ShippingAgentPM shippingAgentPM = partnerDomainService.GetShippingAgentById("1-1", 1);

            shippingAgentPM.ForwarderCreditNumber = "125";
            partnerDomainService.UpdateShippingAgent(shippingAgentPM);

            ShippingAgent shippingAgent = commonContext.ShippingAgents.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("125", shippingAgent.ForwarderCreditNumber);

        }

        [TestMethod]
        public void GetShippingAgentFiltersTest()
        {

            CommonDataDomainService commonDomainService = new CommonDataDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            PartnersDomainService partnerDomainService = new PartnersDomainService();

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
            queryoperations.ObjectTableName = "ShippingAgent";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "Code", FieldValue = "New", Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ShippingAgentList> list = partnerDomainService.GetShippingAgentFilters(bytearray, 1);


        }

    }
}
