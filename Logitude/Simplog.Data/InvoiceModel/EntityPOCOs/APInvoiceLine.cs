using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APInvoiceLine
    {
        [Key]
        public string APInvoiceId { get; set; }
        [Key]
        public int LineNumber { get; set; }
        public int Tenant { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string Notes { get; set; }
        public string ChargesTypeId { get; set; }
        public string VatTypeId { get; set; }
        public string EntityId { get; set; }
        public string EntityPayableId { get; set; }
        public double? RefundAmount { get; set; }
        public double? VatPercentage { get; set; }
        public double? VatAmount { get; set; }
        public string ForiegnCurrencyId { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public string DebitAccount { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string ChargeTypeGLAccountId { get; set; }
        public bool AuthorizedSignatory { get; set; }
        public string PrepaidCollectId { get; set; }

        [ForeignKey("ForiegnCurrencyId")]
        public virtual Currency Currency { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }

        [ForeignKey("APInvoiceId")]
        public virtual APInvoice APInvoice { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }
    }
}