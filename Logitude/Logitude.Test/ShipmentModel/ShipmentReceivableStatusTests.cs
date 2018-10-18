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
namespace Logitude.Test.ShipmentModel
{
    [TestClass]
    public class ShipmentReceivableStatusTests
    {
        [TestMethod]
        public void GetSingleShipmentReceivableStatusPMTest()
        {
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();

            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentReceivableStatusPM entityPM = shipmentsDomainService.GetSingleShipmentReceivableStatus("SRS", 1);
            Assert.AreEqual("SRS", entityPM.Code);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentReceivableStatusPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentReceivableStatusPM entityPM = shipmentsDomainService.GetSingleShipmentReceivableStatus("SRS", 1);

        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleShipmentReceivableStatusPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockCommonContext commonContext = new MockCommonContext();
            ShipmentsDomainService shipmentsDomainService = new ShipmentsDomainService();
            MockShipmentContext shipmentsContext = new MockShipmentContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ShipmentReceivableStatusPM entityPM = shipmentsDomainService.GetSingleShipmentReceivableStatus("SRS", 2);

        }

     

    }
}
