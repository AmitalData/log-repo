using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

using Telerik.JustMock;

using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Security;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class EntityStatusTests
    {
        [TestMethod]
        public void GetSingleEntityStatusPMTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            EntityStatusPM entityStatusPm = webFreightDomainService.GetSingleEntityStatus("111", 1);
            Assert.AreEqual("111", entityStatusPm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleEntityStatusPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            EntityStatusPM entityStatusPm = webFreightDomainService.GetSingleEntityStatus("111", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleEntityStatusPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            EntityStatusPM entityStatusPm = webFreightDomainService.GetSingleEntityStatus("222", 2);
        }

        [TestMethod]
        public void CreateEntityStatusTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            EntityStatusPM newEntityStatus = new EntityStatusPM()
            {
                Code = "ES3",
                Name = "Entity St 3",
                Id = "333",
              
                Tenant = 1,
                InActive = false,
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("333");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            webFreightDomainService.InsertEntityStatus(newEntityStatus);
            EntityStatus entityStatus = webFreightContext.EntityStatus.Where(d => d.Id == "333").FirstOrDefault();
            Assert.AreNotEqual(entityStatus, null);
        }

        [TestMethod]
        public void UpdateEntityStatusTest()
        {
         
            MockCommonContext commonContext = new MockCommonContext();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

            EntityStatusPM entityStatusPm = webFreightDomainService.GetSingleEntityStatus("111", 1);

            entityStatusPm.Name = "Edited Name";

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            webFreightDomainService.UpdateEntityStatus(entityStatusPm);

            EntityStatus entityStatus = webFreightContext.EntityStatus.Where(d => d.Id == "111").FirstOrDefault();

            Assert.AreEqual("Edited Name", entityStatus.Name);
        }

        [TestMethod]
        public void GetEntityStatusFiltersTest()
        {


            MockCommonContext commonContext = new MockCommonContext();

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
            queryoperations.ObjectTableName = "EntityStatus";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = false, FieldName = "InActive", FieldValue=false, Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<EntityStatusList> list = webFreightDomainService.GetEntityStatusFilters(bytearray, 1);


        }
    }
}
