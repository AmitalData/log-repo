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
        public string ShipmentNumberTenant { get; set; } // ברקוד (מס' תיק + TENANT) 
        // public DateTime? TaxationDateTime { get; set; }//תאריך חישוב מיסים
        public string CustomerName { get; set; }// מס' אינדקס + שם לקוח
        public string DepartmentName { get; set; }
        public string CustomFileNo { get; set; }
        public string ReferentUserName{ get; set; }
        public string DeclarationOfficeName { get; set; }
        public string CarrierCode { get; set; }
        public string Mawb { get; set; }
        public string House { get; set; }
        public string IskaNumber { get; set; }
        public int? NumberOfPackages { get; set; }
        public double? GrossWeight { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public string Vessel { get; set; }
        public string FreightForwarderId { get; set; }
        public string DescriptionOfGoods { get; set; }
        public List<ShipmentReferance> ShipmentReferances { get; set; }
    }

    public class ShipmentReferance
    {
        public int LineNumber { get; set; }
        public string ReferanceType { get; set; }
        public string ReferanceValue { get; set; }
    }
}