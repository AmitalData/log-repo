using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.AccountingModel.Reports.PaymentCheque
{
    public class PaymentChequeDataProvider:BaseDataProvider
    {


       public string ChequeNumber { get; set; }
       public DateTime? ValueDate { get; set; }
       public string BranchName { get; set; }
       public decimal TotalAmount { get; set; }
        public List<PaymentChequeLine> PaymentChequeLines { get; set; }
        public string PayToName { get; set; }
        public string VatNumber { get; set; }
        public string AddressName { get; set; }
        public string FAX { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BranchNumber { get; set; }
        public string Signature { get; set; }
        public string BankAddress { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankCity { get; set; }
        public string BankCode { get; set; }
        public byte[] CompanyLogo { get; set; }
        public string AmountInHebrew { get; set; }
        public Image AccountingLogo { get; set; }
        public Image BankLogo { get; set; }
        public string Telephone { get; set; }
        public string APPaymentNumber { get; set; }
        public decimal? TaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string PrintNotes { get; set; }
        public string AccountDisplayNumber { get; set; }
        public DateTime? PrintDate { get; set; }
        public string BranchNumberPrint { get; set; }
        public string BankAccountNumberPrint { get; set; }
    }

    

    public class PaymentChequeLine
    {
        public decimal? Amount { get; set; }
        public string Note { get; set; }
    }
}