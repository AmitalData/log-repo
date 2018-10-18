using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.InfrastructureModel;
using Logitude.Test.Helpers;
using Simplog.Data.ShipmentsModel.Mocks;
using WebFreight.Web.ShipmentsModel.DomainServices;
using Telerik.JustMock;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.GlobalModelDB;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.ShipmentModel
{
    [TestClass]
    public class ShipmentCustomerTypeTests
    {
        [TestMethod]
        public void GetSingleShipmentCustomerTypePMTest()
        {
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();

            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentCustomerTypePM entityPM = shipmentsDomainService.GetSingleShipmentCustomerTypePM("SCT", 1);
            Assert.AreEqual("SCT", entityPM.Code);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentCustomerTypePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentCustomerTypePM entityPM = shipmentsDomainService.GetSingleShipmentCustomerTypePM("SCT",1);
        
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentCustomerTypePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentCustomerTypePM entityPM = shipmentsDomainService.GetSingleShipmentCustomerTypePM("SCT", 2);
          
        }

        [TestMethod]
        public void GetShipmentCustomerTypeFiltersTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
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
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "ShipmentCustomerType";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "Code", Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ShipmentCustomerTypeList> list = shipmentsDomainService.GetShipmentCustomerTypeFilters(bytearray, 1);


        } 

    }
}
