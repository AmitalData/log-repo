using System;


namespace Logitude.FullAccounting.Test.Models
{
    public class ARInvoiceLinePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARInvoiceId { get; set; }

        
        public string ChargesTypeId { get; set; }

        
        public string ForiegnCurrencyId { get; set; }
        public string ForiegnCurrencyCode { get; set; }

        
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }

        
        public double? InvoiceCurrencyAmount { get; set; }        

        
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }

        public double? VatAmount { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        public DateTime? ExchangeRateDate { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public int LineNumber { get; set; }
        public string ReceivableId { get; set; }                
        public string PrepaidCollectId { get; set; } //dummy field
        public string MeasurementCode { get; set; } //dummy field

        
        public double? Quantity { get; set; } //dummy field

        
        public double? UnitPrice { get; set; } //dummy field

        public int ViewOrder { get; set; }
        public string MeasurementId { get; set; }
        public bool IsExchangeRateFixed { get; set; }
        public string EntityId { get; set; }
        //public string ObjectTableId { get; set; }//dummy field
        public string EntityReference { get; set; }//dummy field
        public double? ProfitCurrencyAmount { get; set; }
        public string InvoiceLocalCurrencyCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }

        
        public string CreditAccount { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }

        
        public string Notes { get; set; }

        
        public DateTime? DateForInterest { get; set; }

        
        public DateTime? ValueDate { get; set; }

        
        public string GLAccountId { get; set; }

        
        public string LineActionCode { get; set; }

        public bool IsCustomsCharge { get; set; }

        
        public bool IsBackToBack { get; set; }
        
        public bool IsExpense { get; set; }

        public bool IsRegionalTax { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }

    }
}
