using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.ShipmentTests.Services.OceanInsight
{
    public class ContainerDetailsService : OceanInsightService
    {
        public void AssertChanging(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            AssertShipmentContainerSimulator(shipmentContainerSimulator);
            AssertCommunicationLogs();
            var shipment = GetShipment();
            AssertChangeContainerDetails(shipment.ShipmentPackages.First());

        }
        private void AssertCommunicationLogs()
        {
            var tryEvreySecound = 4;
            var tineLifeInSecound = 60 * 5;
            var containerObjectTableID = GetObjectTableID("Container");
            var containerEntityId = ShipmentData.ShipmentPM.ShipmentPackages.First().ContainerEntityId;
            var isDone = Waiter.RunAndWait(tryEvreySecound, tineLifeInSecound, () => AssertAddCommunicationLogs(containerObjectTableID, containerEntityId));
            isDone.Should().BeTrue();
        }

        private ShipmentPM GetShipment()
        {
            string singleShipmentUrl = Urls.ShipmentGetSingle(ShipmentData.ShipmentPM.Id);
            ApiResponse<ShipmentPM> getResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;
        }

        public void AssertChangeContainerDetails(PackagePM packagePM)
        {
            XMLOceanInsightAnalyzer xMLOceanInsightAnalyzer = new XMLOceanInsightAnalyzer(ShipmentData.XMLData);
            packagePM.ContainerStatusSourceCode.Should().Be(xMLOceanInsightAnalyzer.ContenerShipment.container_status);
        }

        public ShipmentContainerSimulator CreateShipmentContainerSimulator(string fileName)
        {
            var xmlData = ReadFilebyName(fileName);
            MapXmlData(xmlData);
            return new ShipmentContainerSimulator()
            {
                ShipmentId = ShipmentData.ShipmentPM.Id,
                CarrierId = ShipmentData.ShipmentPM.MainCarriageCarrierId,
                ContainerNumber = ShipmentData.ShipmentPM.ShipmentPackages.First().ContainerNumber,
                IsFromContainer = true,
                XmlString = xmlData
            };
        }


    }
}
