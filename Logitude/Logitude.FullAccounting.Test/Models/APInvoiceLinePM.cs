namespace Logitude.FullAccounting.Test.Models
{
    public class APInvoiceLinePM
    {
        public int Tenant { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeName { get; set; }
        public string Description { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double? VatPercentage { get; set; }
        public string EntityId { get; set; }
        public string APInvoiceId { get; set; }
        public int LineNumber { get; set; }

        
        public double? InvoiceCurrencyAmount { get; set; }

        
        public string Notes { get; set; }

        
        public string ChargesTypeId { get; set; }

        
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }
        public double? VatRecognizedPercentage { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        
        public string VendorId { get; set; }

        
        public double? OpenAmount { get; set; }

        
        public string ForiegnCurrencyId { get; set; }

        
        public double? ForiegnCurrencyAmount { get; set; }

        
        public double? ForiegnExchangeRate { get; set; }

        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string EntityId { get; set; }
        public string EntityPayableId { get; set; }
        public double? RefundAmount { get; set; }

        
        public string ChargesTypeCode { get; set; }

        
        public string ChargesTypeName { get; set; }

        public string ObjectTableId { get; set; }
        public string EntityReference { get; set; }
        public string VendorName { get; set; }
        public double? OtherInvoicesAmounts { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? CorrectionAmount { get; set; }
        public string CorrectionNote { get; set; }
        public string CorrectionByUserId { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string AmountTypeCode { get; set; }
        public string ForiegnCurrencyCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        
        public string DebitAccount { get; set; }

        
        public string Description { get; set; }

        
        public string LocalDescription { get; set; }

        
        public string ChargeTypeGLAccountId { get; set; }
        public bool AuthorizedSignatory { get; set; }

        public string PrepaidCollectId { get; set; }

        public string ContainerTypeId { get; set; }
        public string ContainerTypeCode { get; set; }
        public int? Quantity { get; set; }
        public double? ForiegnAmountWithRecognizedVat { get; set; }
        public double? LocalAmountWithVatRecognized { get; set; }
    }
}
