using Logitude.ShipmentTests.Models.Accounting;
using System;

namespace Logitude.ShipmentTests.Models
{
    public class ShipmentReceivablePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ShipmentReceivableLineStatusCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public double? Rate { get; set; }
        public double? TotalAmountLocal { get; set; }
        public double? TotalAmount { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdateByUserId { get; set; }
        public string PrepaidCollectId { get; set; }
        public string IATACodeId { get; set; }
        public string DueTypeCode { get; set; }
        public string VatTypeId { get; set; }
        public double? VatAmountLocal { get; set; }
        public double? VatAmountProfit { get; set; }

    }
}