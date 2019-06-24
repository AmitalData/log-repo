using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class VDKDataProvider : BaseDataProvider
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public string StatusName { get; set; }
        public string ShipperName { get; set; }


        public List<ShipmentPackageRecord> ShipmentPackages { get; set; }

    }

    public class ShipmentPackageRecord
    {
        public string ProjectNumber { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string CustomerRef { get; set; }
        public string Product { get; set; }
        public double? Weight { get; set; }
        public string Unit { get; set; }
        public string ContainerNr { get; set; }
        public DateTime? RequestETD { get; set; }
        public DateTime? EstimateETD { get; set; }
        public DateTime? ActualETD { get; set; }
        public string ShipmentField2 { get; set; }
        public DateTime? EstimateETA { get; set; }
        public string CustomRef { get; set; }
        public string Shipper { get; set; }
        public int? Pieces { get; set; }
        public string CommodityNumber { get; set; }
        public string ConsigneeAddress { get; set; }
        public DateTime? ActualETA { get; set; }
    }
}