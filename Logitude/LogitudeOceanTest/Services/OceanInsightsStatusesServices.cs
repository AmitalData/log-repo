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

        public string ReadFilebyName(string xMLFile)
        {
            var path = "./MetaData/" + xMLFile;
            return File.ReadAllText(path);
        }

        public bool CheckIfCommunicationLogsAddSuccessfullyForContener()
        {
            var filters = GetCommunicationLogs("1-18816", OceanData.ShipmentPM.ShipmentPackages.First().ContainerEntityId);
            var communicationLogs = APICaller.CallGetByFilters<List<CommunicationLogList>>(Urls.CommunicationLogViewsGetByFilters, UserTenant.Token, filters)?.Data;
            return new CheckerService().ValidateCommunicationLogs(communicationLogs);
        }
        public bool CheckIfCommunicationLogsAddSuccessfullyForShipment()
        {
            var filters = GetCommunicationLogs("1-4", OceanData.ShipmentPM.Id);
            var communicationLogs = APICaller.CallGetByFilters<List<CommunicationLogList>>(Urls.CommunicationLogViewsGetByFilters, UserTenant.Token, filters)?.Data;
            return new CheckerService().ValidateCommunicationLogs(communicationLogs);
        }
        public ApiQueryFilters GetCommunicationLogs(string objectTableId, string EntityId)
        {
            return new ApiQueryFiltersBuilder()
                .PageIndex(0)
                .PageSize(10)
                .Filter1Name("EntityId")
                .Filter1Value(EntityId)
                .Filter2Name("ObjectTableId")
                .Filter2Value(objectTableId)
                .SortBy("CreateDate")
                .SortDirection("Descending")
                .Build();

        }
    }
}
