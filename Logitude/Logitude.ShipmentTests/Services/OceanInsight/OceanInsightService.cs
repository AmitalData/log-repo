using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
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
            var path = "./Data/" + fileName;
            return File.ReadAllText(path);
        }
        public ApiQueryFilters CreateCommunicationLogFilter(string objectTableId, string EntityId)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .PageSize(10)
                .Filter1Name("EntityId")
                .Filter1Value(EntityId)
                .Filter2Name("ObjectTableId")
                .Filter2Value(objectTableId)
                .SortBy("CreateDate")
                .SortDirection("Descending")
                .Build();

        }
        public string GetObjectTableId(string ObjectTableName)
        {
            ObjectTableService objectTableService = new ObjectTableService();
            return objectTableService.GetIdByName(ObjectTableName);
        }
        public void MapXmlData(string xmlData)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            ShipmentData.XMLData = xmlDocument;
        }
        public ContainerPM GetContainer(string containerEntityId)
        {
            string singleShipmentUrl = Urls.ContainerGetSingle(containerEntityId);
            ApiResponse<ContainerPM> getResponse = APICaller.CallGet<ContainerPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;

        }
        public ShipmentPM GetShipment()
        {
            string singleShipmentUrl = Urls.ShipmentGetSingle(ShipmentData.ShipmentPM.Id);
            ApiResponse<ShipmentPM> getResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return getResponse.Data;
        }
    }
}
