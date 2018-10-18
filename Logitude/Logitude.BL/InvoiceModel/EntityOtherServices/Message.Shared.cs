using System;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class Message
    {
       public string TransactionTypeCode { get; set; }
       public string Reference1 { get; set; }
       public string Reference2 { get; set; }
       public DateTime? TransactionDate { get; set; }
       public DateTime? ValueDate { get; set; }
       public string CostCode { get; set; }
       public string CurrencyCode { get; set; }
       public string Details { get; set; }

       public string DebitAccount1 { get; set; }
       public string DebitAccount2 { get; set; }
       public string CreditAccount1 { get; set; }
       public string CreditAccount2 { get; set; }

       public double? DebitAmount1InLocalCurrency { get; set; }
       public double? DebitAmount2InLocalCurrency { get; set; }
       public double? CreditAmount1InLocalCurrency { get; set; }
       public double? CreditAmount2InLocalCurrency { get; set; }

       public double? DebitAmount1InInvoiceCurrency { get; set; }
       public double? DebitAmount2InInvoiceCurrency { get; set; }
       public double? CreditAmount1InInvoiceCurrency { get; set; }
       public double? CreditAmount2InInvoiceCurrency { get; set; }

       public DateTime ThirdDate { get; set; }
       public string Reference3 { get; set; }
       public double Quantity { get; set; }
       public string File { get; set; }
       public string Remarks { get; set; }
       public string AdditionalRemarks { get; set; }
       public string Branch { get; set; }
       public string VATNumber { get; set; }
    }
}