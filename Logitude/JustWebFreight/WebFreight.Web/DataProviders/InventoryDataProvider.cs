using Logitude.WarehouseLib.BL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class InventoryDataProvider: BaseDataProvider
    {
        public string Warehouse { get; set; }
        public string PartnerName { get; set; }
        public string ShipperConsignee { get; set; }
        
        public List<WarehouseEntryPackageItem> WarehouseEntryPackageList { get; set; }

        public List<InventoryGroup> InventoryGroupList { get; set; }
        public decimal? TotalVolume { get; internal set; }

        public class InventoryGroup
        {
            public string Warehouse { get; set; }
            public List<WarehouseEntryPackageItem> WarehouseList { get; set; }
        }
    }

 
}