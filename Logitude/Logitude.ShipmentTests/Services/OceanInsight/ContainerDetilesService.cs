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
        public AssertionService assertionService { get; set; }
        public ContainerDetailsService()
        {
            assertionService = new AssertionService();
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
        public void Assert(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            assertionService.AssertShipmentContainerSimulator(shipmentContainerSimulator);
            assertionService.AssertAddCommunicationLogs();
            assertionService.AssertChangeDetails();
            
        }


    }
}
