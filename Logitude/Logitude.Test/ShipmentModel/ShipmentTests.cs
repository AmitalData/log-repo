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
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Mocks;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.ShipmentsModel.Tools.TraceEvents;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.Helpers;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.ShipmentModel
{
    [TestClass]
    public class ShipmentTests
    {
  
       [TestMethod]
        public void GetSingleShipmentPMTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();

            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(new MockInvoiceContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentPM entityPM = shipmentsDomainService.GetSingleShipmentPresentationModel("1-1", 1);
            Assert.AreEqual("1-1", entityPM.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
          
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(new MockInvoiceContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentPM entityPM = shipmentsDomainService.GetSingleShipmentPM("1-1", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(new MockInvoiceContext());
           

            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentPM entityPM = shipmentsDomainService.GetSingleShipmentPM("1-1", 2);
        }

        [TestMethod]
        public void CreateShipmentTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            MockWebFreightContext webContext = new MockWebFreightContext();

            int tenant = 1;

            ShipmentPM newShipment = new ShipmentPM()
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
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);


            shipmentsDomainService.InsertShipmentPM(newShipment);
            Shipment shipment = shipmentsContext.Shipments.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(shipment, null);


        }
        [TestMethod]
        public void GetShipmentFiltersTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            MockWebFreightContext webContext = new MockWebFreightContext();
           // MockShipmentDataView shipmentDataViewContext = new MockShipmentDataView();
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
        //    Mock.Arrange(() => ShipmentDataViewContext.GetContext(Arg.IsAny<int>())).Returns(shipmentDataViewContext);
       
            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "Shipment";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "IsOperationalClosed", FieldValue=true, Operator="Equals"});

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ShipmentList> list = shipmentsDomainService.GetShipmentFilters(bytearray, 1);
            

        }


    }
}
