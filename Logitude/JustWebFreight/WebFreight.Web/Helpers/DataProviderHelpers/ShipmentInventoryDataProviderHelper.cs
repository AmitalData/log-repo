using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.WarehouseLib.BL.DataContracts;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class ShipmentInventoryDataProviderHelper
    {

        public byte[] LoadDataToShipmentInventoryDataProvider(string entityId, int tenant)
        {
            ShipmentInventoryDataProvider dataprovider = LoadShipmentInventoryDataProvider(entityId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(ShipmentInventoryDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;

        }

        private ShipmentInventoryDataProvider LoadShipmentInventoryDataProvider(string entityId, int tenant)
        {
            ShipmentInventoryDataProvider dataProvider = new ShipmentInventoryDataProvider();
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentList shipmentList = shipmentQuery.GetShipmentListByIdForInventoryReport(entityId, tenant);

            if (shipmentList != null)
            {
                dataProvider.Customer = shipmentList.CustomerName;
                dataProvider.CustomerReferences = shipmentList.CustomerReference1;
                dataProvider.ShipmentNumber = shipmentList.ShipmentNumber;
                dataProvider.Routing = shipmentList.FromPortName + " > " + shipmentList.ToPortName;
                dataProvider.House = shipmentList.House;
                dataProvider.Master = shipmentList.Master;
                if (!string.IsNullOrEmpty(shipmentList.CustomerReference1) && !string.IsNullOrEmpty(shipmentList.CustomerReference2)) dataProvider.CustomerReferences += ",";
                if (!string.IsNullOrEmpty(shipmentList.CustomerReference2)) dataProvider.CustomerReferences += shipmentList.CustomerReference2;
                List<string> cardIds = new List<string>();
                if (!string.IsNullOrEmpty(shipmentList.WarehouseLegWarehouseId)) cardIds.Add(shipmentList.WarehouseLegWarehouseId);
                if (!string.IsNullOrEmpty(shipmentList.CustomerId) && !cardIds.Contains(shipmentList.CustomerId)) cardIds.Add(shipmentList.CustomerId);

                List<CardList> cardLists = null;
                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByCardIds(cardIds, tenant);
                }

                if (!string.IsNullOrEmpty(shipmentList.WarehouseLegWarehouseId))
                {
                    CardList cardList = cardLists.Where(d => d.Id == shipmentList.WarehouseLegWarehouseId).FirstOrDefault();
                    if (cardList != null)
                    {
                        dataProvider.Warehouse = cardList.EnglishName;
                    }
                }
                if (!string.IsNullOrEmpty(shipmentList.CustomerId))
                {
                    CardList cardList = cardLists.Where(d => d.Id == shipmentList.CustomerId).FirstOrDefault();
                    if (cardList != null)
                    {
                        dataProvider.Customer = cardList.EnglishName;
                    }
                }

            }

            WarehouseEntryPackageQueryService warehouseEntryPackageQueryService = new WarehouseEntryPackageQueryService(tenant);

            List<WarehouseEntryPackageItem> result = warehouseEntryPackageQueryService.GetWarehouseEntryPackageItemForInventoryReport(new WarehouseEntryPackageArgs() { Tenant = tenant , ShipmentId = entityId});

            List<ShipmentInventoryDataProvider.InventoryGroup> finalResults = (from a in result
                                                                       group a by new { a.WarehouseName, a.WarehouseId, }
                                                                       into g
                                                                       select new ShipmentInventoryDataProvider.InventoryGroup()
                                                                       {
                                                                           Warehouse = g.Key.WarehouseName,
                                                                           WarehouseList = g.ToList(),
                                                                       }).ToList();

            dataProvider.InventoryGroupList = finalResults.OrderBy(d => d.Warehouse).ToList();

            return dataProvider;
        }

    }
}