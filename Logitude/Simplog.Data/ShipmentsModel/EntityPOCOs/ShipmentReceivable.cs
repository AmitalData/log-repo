using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Collections.Generic;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{

    public class ShipmentReceivable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ShipmentReceivableLineStatusCode { get; set; }
        public string MeasurementId { get; set; }
        public double? Quantity { get; set; }
        public string CurrencyId { get; set; }
        public double? Rate { get; set; }
        public double? UnitPrice { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalAmountLocal { get; set; }
        public string Notes { get; set; }
        public decimal PayableLocal { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PrepaidCollectId { get; set; }
        public bool AWBPrint { get; set; }
        public string DueTypeCode { get; set; }
        public string ARInvoiceLineId { get; set; }
        public bool IsExchangeRateFixed { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public string ARInvoiceId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string IATACodeId { get; set; }
        public bool IsFromQuote { get; set; }
        public bool IsFixedPrice { get; set; }
        public string QuoteChargeId { get; set; }
        public bool IsChargeBySteps { get; set; }
        public double? QuoteSaleMinAmount { get; set; }
        public double? QuoteSaleMaxAmount { get; set; }
        public bool IsExpense { get; set; }
        public string VatTypeId { get; set; }
        public bool IsBackToBack { get; set; }
        public string ShipmentReceivableParentId { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public double? VatAmountLocal { get; set; }
        public double? VatAmountProfit { get; set; }        
        public string PayableVendorId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }

        [ForeignKey("DueTypeCode")]
        public virtual DueType DueType { get; set; }

        [ForeignKey("IATACodeId")]
        public virtual IATACode IATACode { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        [ForeignKey("ARInvoiceLineId")]
        public virtual ARInvoiceLine ARInvoiceLine { get; set; }

        [ForeignKey("ARInvoiceId")]
        public virtual ARInvoice ARInvoice { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }

        [ForeignKey("ShipmentReceivableLineStatusCode")]
        public virtual ShipmentReceivableLineStatus ShipmentReceivableLineStatus { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        [ForeignKey("UpdateByUserId")]
        public virtual User UpdateByUser { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }

        public ShipmentReceivable ShipmentReceivableParent { get; set; }
        public List<ShipmentReceivable> ChildShipmentReceivables { get; set; }

        [ForeignKey("PayableVendorId")]
        public virtual Card PayableVendor { get; set; }
    }
}