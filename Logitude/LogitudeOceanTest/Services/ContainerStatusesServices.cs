using Logitude.OceanTest.Models;
using Logitude.OceanTest.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.OceanTest.Services
{
    public class ContainerStatusesServices
    {
        internal ShipmentContainerSimulator CreateShipmentContainerSimulator(string xMLFile)
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

        private string ReadFilebyName(string xMLFile)
        {
            //C:\Projects\log-repo\Logitude\LogitudeOceanTest\MetaData\Sample1.xml
            //C:\Projects\log-repo\Logitude\LogitudeOceanTest\Services\ContainerStatusesServices.cs
            var path = "./MetaData/" + xMLFile;
            return File.ReadAllText(path);
        }

        internal void ValidateShipmentContainerSimulator(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            if (shipmentContainerSimulator.Errors.Any())
            {
                throw new InvalidOperationException("Failed Run Shipment Container Simulator", new Exception(string.Join(", ", shipmentContainerSimulator.Errors)));
            }

        }

        internal bool CheckIfCommunicationLogsAddSuccessfully(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            var filters = GetCommunicationLogs(shipmentContainerSimulator);
            var communicationLogs = APICaller.CallGetByFilters<List<CommunicationLogList>>(Urls.CommunicationLogViews, UserTenant.Token, filters)?.Data;
            return ValidateCommunicationLogs(communicationLogs);
        }

        private bool ValidateCommunicationLogs(List<CommunicationLogList> communicationLogs)
        {
            if (communicationLogs == null || communicationLogs.Count <= 0)
                return false;

            var lastCommunicationLog = communicationLogs.First();
            if (lastCommunicationLog.CommunicationStatusTypeCode != "D")
                return false;

            if (lastCommunicationLog.From != "Amital")
                return false;

            return true;
        }

        private ApiQueryFilters GetCommunicationLogs(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            return new ApiQueryFiltersBuilder()
                .PageIndex(0)
                .PageSize(10)
                .Filter1Name("EntityId")
                .Filter1Value("1-356")
                .Filter2Name("ObjectTableId")
                .Filter2Value("1-18474")
                .SortBy("CreateDate")
                .SortDirection("Descending")
                .Build();

        }
    }
}
