using Logitude.WarehouseLib.BL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentInventoryDataProvider: BaseDataProvider
    {
        public string Warehouse { get; set; }
        public string Customer { get; set; }
        public string CustomerReferences { get; set; }
        public string ShipmentNumber { get; set; }
        public string Routing { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        
        public List<WarehouseEntryPackageItem> WarehouseEntryPackageList { get; set; }

        public List<InventoryGroup> InventoryGroupList { get; set; }
        public class InventoryGroup
        {
            public string Warehouse { get; set; }
            public List<WarehouseEntryPackageItem> WarehouseList { get; set; }
        }
    }
}