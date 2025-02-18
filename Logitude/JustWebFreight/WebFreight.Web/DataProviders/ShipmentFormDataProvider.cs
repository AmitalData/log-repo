using CWXSD;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentFormDataProvider : BaseDataProvider
    {
        public string ShipmentNumber { get; set; }
        public string ShipmentNumberTenant { get; set; }
        public string CustomerName { get; set; }
        public string DepartmentName { get; set; }
        public string CustomFileNo { get; set; }
        public string ReferentUserName { get; set; }
        public string DeclarationOfficeName { get; set; }
        public string CarrierCode { get; set; }
        public string Mawb { get; set; }
        public string House { get; set; }
        public string IskaNumber { get; set; }
        public int? NumberOfPackages { get; set; }
        public double? GrossWeight { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public string Vessel { get; set; }
        public string FreightForwarderName { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string ReferanceValue { get; set; }
        public string CarrierName { get; set; }
        public string VendorName { get; set; }
        public string ForwarderFileConnect { get; set; }
    }
}