using Logitude.OceanTest.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Logitude.OceanTest.Services
{
    public class OceanInsightsStatusesServices
    {
        public ShipmentContainerSimulator CreateShipmentContainerSimulatorForContener(string xMLFile)
        {
            var xmlData = ReadFilebyName(xMLFile);
            return new ShipmentContainerSimulator()
            {
                ShipmentId = OceanData.ShipmentPM.Id,
                CarrierId = OceanData.ShipmentPM.MainCarriageCarrierId,
                ContainerNumber = OceanData.ShipmentPM.ShipmentPackages.First().ContainerNumber,
                IsFromContainer = true,
                XmlString = xmlData
            };
        }
        public ShipmentContainerSimulator CreateShipmentContainerSimulatorForShipment(string xMLFile)
        {
            var xmlData = ReadFilebyName(xMLFile);
            return new ShipmentContainerSimulator()
            {
                ShipmentId = OceanData.ShipmentPM.Id,
                CarrierId = OceanData.ShipmentPM.MainCarriageCarrierId,
                IsFromContainer = false,
                XmlString = xmlData
            };
        }

        public string ReadFilebyName(string fileName)
        {
            var path = "./MetaData/" + fileName;
            return File.ReadAllText(path);
        }

       
        
    }
}
