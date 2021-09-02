using FluentAssertions;
using Logitude.ShipmentTests.Models;
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
using System.Xml;

namespace Logitude.ShipmentTests.Services.OceanInsight
{
    public class OceanInsightService
    {
        public string ReadFilebyName(string fileName)
        {
            var path = "./MetaData/" + fileName;
            return File.ReadAllText(path);
        }
        public void AssertShipmentContainerSimulator(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            shipmentContainerSimulator.Errors.Should().BeNullOrEmpty(string.Join(", ", shipmentContainerSimulator.Errors));
        }
        public bool AssertAddCommunicationLogs(string objectTableId, string EntityId)
        {
            var filters = CreateCommunicationLogFilter(objectTableId, EntityId);
            var communicationLogs = APICaller.CallGetByFilters<List<CommunicationLogList>>(Urls.CommunicationLogViewsGetByFilters, UserTenant.Token, filters)?.Data;
            return AssertCommunicationLogs(communicationLogs);
        }
        private ApiQueryFilters CreateCommunicationLogFilter(string objectTableId, string EntityId)
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
        public bool AssertCommunicationLogs(List<CommunicationLogList> communicationLogs)
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
        public string GetObjectTableID(string ObjectTableName)
        {
            ObjectTableService objectTableService = new ObjectTableService();
            return objectTableService.GetIdByName(ObjectTableName);
        }
        public void MapXmlData(string xmlData)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            ShipmentData.XMLData = xmlDocument;
            xmlDocument.node
        }
    }
}
