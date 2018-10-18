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
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.CommonDataModel.DomainServices;
using Simplog.Data.Helpers;
using System;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.InfrastructureModel.EntityLists;
using System.Linq;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.Helpers;

namespace Logitude.Test.InfrastructureModel
{
    [TestClass]
    public class ChargesGroupTests
    {
        [TestMethod]
        public void GetSingleChargesGroupPMTest()
        {
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            MockCommonContext commonContext = new MockCommonContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();

            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            ChargesGroupPM chargesGroupPm = webFreightDomainService.GetSingleChargeGroup("CG", 1);
            Assert.AreEqual("CG", chargesGroupPm.Code);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleChargesGroupPMWithAutenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notExistUser@fnarsoft.com");
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            ChargesGroupPM chargesGroupPm = webFreightDomainService.GetSingleChargeGroup("CG", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleChargesGroupPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();

            ChargesGroupPM chargesGroupPm = webFreightDomainService.GetSingleChargeGroup("CG", 2);
      
        }


        [TestMethod]
        public void GetChargeGroupFiltersTest()
        {

            CommonDataDomainService commonDomainService = new CommonDataDomainService();
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
            queryoperations.ObjectTableName = "ChargeGroup";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = false, FieldName = "Code", Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ChargesGroupList> list = webFreightDomainService.GetChargeGroupFilters(bytearray, 1);


        }
    }
}
