using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentPayableList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ChargesTypeId { get; set; }
        public string PayableStatusTypeCode { get; set; }
        public string MeasurementId { get; set; }
        public string PrepaidCollectId { get; set; }
        public double? Quentity { get; set; }
        public bool AWBPrint { get; set; }
        public string CurrencyId { get; set; }
        public double? UnitPrice { get; set; }
        public double? Rate { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? ExpectedAmountLocal { get; set; }
        public string Notes { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string DueTypeCode { get; set; }
        public string VendorId { get; set; }
        public bool NotExpected { get; set; }
        public bool OpenAmountEditedByUser { get; set; }

      
        
    }
}