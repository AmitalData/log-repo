using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    
    public class ARInvoiceLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }      
        public double? InvoiceCurrencyAmount { get; set; }                
        public DateTime? ExchangeRateDate { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public int LineNumber { get; set; }
        public string ReceivableId { get; set; }
        public double? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public bool IsExchangeRateFixed { get; set; }
        public string EntityId { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public double? VatPercentage { get; set; }
        public string CreditAccount { get; set; }
        public string Notes { get; set; }
        public DateTime? DateForInterest { get; set; }
        public DateTime? ValueDate { get; set; }
        public string GLAccountId { get; set; }
        public bool IsBackToBack { get; set; }
        public bool IsExpense { get; set; }
        public string PrepaidCollectId { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }

        [ForeignKey("LineActionCode")]
        public virtual ARInvoiceLineAction ARInvoiceLineAction { get; set; }
        public string LineActionCode { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }
        public string MeasurementId { get; set; }

        [ForeignKey("ARInvoiceId")]
        public virtual ARInvoice ARInvoice { get; set; }
        public string ARInvoiceId { get; set; }       

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }
        public string ChargesTypeId { get; set; }      

        [ForeignKey("ForiegnCurrencyId")]
        public virtual Currency Currency { get; set; }
        public string ForiegnCurrencyId { get; set; }              

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
        public string VatTypeId { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }

        public bool IsRegionalTax { get; set; }
    }
}