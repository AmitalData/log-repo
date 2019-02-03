using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPayable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ShipmentPayableLineStatusCode { get; set; }
        public string MeasurementId { get; set; }        
        public double? Quantity { get; set; }
        public bool AWBPrint { get; set; }
        public string CurrencyId { get; set; }
        public double? UnitPrice { get; set; }
        public double? Rate { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? ExpectedAmountLocal { get; set; }
        public string Notes { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime ValueDate { get; set; }       
        public string VendorId { get; set; }
        public string DueTypeCode { get; set; }
        public string PrepaidCollectId { get; set; }
        public double? MinAmount { get; set; }
        public double? MaxAmount { get; set; }
        public double? ExpectedAmountInProfitCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public string ShipmentPayableParentId { get; set; }
        public bool IsEditedByUser { get; set; }
        public double? AccountedAmount { get; set; }
        public double? AccountedAmountInLocalCurrency { get; set; }
        public double? AccountedAmountInProfitCurrency { get; set; }
        public double? OpenAmount { get; set; }
        public double? OpenAmountInLocalCurrency { get; set; }
        public double? OpenAmountInProfitCurrency { get; set; }
        public string ShipmentPayableAmountTypeCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public double? CorrectionAmount { get; set; }
        public string CorrectionByUserId { get; set; }
        public string CorrectionNote { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string IATACodeId { get; set; }
        public bool IsFromQuote { get; set; }
        public string QuoteChargeId { get; set; }
        public bool IsChargeBySteps { get; set; }
        public double? QuoteCostMinAmount { get; set; }
        public double? QuoteCostMaxAmount { get; set; }

        public string VatTypeId { get; set; }
        public bool IsBackToBack { get; set; }
        public string ReceivableId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        [ForeignKey("IATACodeId")]
        public virtual IATACode IATACode { get; set; }

        [ForeignKey("CorrectionByUserId")]
        public User CorrectionByUser { get; set; }

        public ShipmentPayable ShipmentPayableParent { get; set; }
        public List<ShipmentPayable> ChildShipmentPayables { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }

        [ForeignKey("DueTypeCode")]
        public virtual DueType DueType { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }

        [ForeignKey("ShipmentPayableLineStatusCode")]
        public virtual ShipmentPayableLineStatus ShipmentPayableLineStatus { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }

        [ForeignKey("ShipmentPayableAmountTypeCode")]
        public virtual ShipmentPayableAmountType ShipmentPayableAmountType { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        [ForeignKey("UpdateByUserId")]
        public virtual User UpdateByUser { get; set; }

        [ForeignKey("VendorId")]
        public virtual Card VendorCard { get; set; }      
    }
}
