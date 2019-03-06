using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class OpenFormatReportData
    {

        public List<B100Data> B100DataList;
        public List<B110Data> B100Data;
        public List<AddressData> AddressDataList;


    }

    public class B100Data
    {
        public int Counter { get; set; }
        public string JournalNumber { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountingEntityReference { get; set; }
        public string AccountingEntityCode { get; set; }
        public string Reference2 { get; set; }
        public string Notes { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public string GLAccountDisplayNumber { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public string CurrencyId { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUser { get; set; }

        public string OppositGLAccount { get; set; }


    }

    public class B110Data
    {

        public string ChartOfAccountsCode { get; set; }
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string ChartOfAccountsName { get; set; }
        public string AccountTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public string GLAccountId { get; set; }
        public string CardId { get; set; }
        public string VatNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressType { get; set; }
        public string BillingAddress { get; set; }
        public string MainAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string CurrecnyId { get; set; }
        public decimal? OpeningBalanceInForegnCurrency { get; set; }
        public decimal? TotalDebitInForeignCurrency { get; set; }
        public decimal? TotalCreditInForeignCurrency { get; set; }
        public string CustomerGLAccountId { get; set; }


    }

    public class AddressData
    {
        public string GLAccountId { get; set; }
        public string CardId { get; set; }
        public string VatNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressType { get; set; }
        public string MainAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }

    public class C100Data
    {
        public string ARInvoiceId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentReference { get; set; }
        public DateTime? DocumentCreateDate { get; set; }
        public DateTime DocumentCreateTime { get; set; }
        public string CustomerVendorName { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHomeNO { get; set; }
        public string AddressCity { get; set; }
        public string AddressZIPCode { get; set; }
        public string AddressCountry { get; set; }
        public string AddressCountryCode { get; set; }
        public string CustomeVendorTelephone { get; set; }
        public string CustomerVendorVatNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public decimal TotalDocumentsAmount { get; set; }
        public string CurrencyCode { get; set; }
        public double? TotalDocumentsAmountBeforeDiscount { get; set; }
        public decimal DocumentsDiscount { get; set; }
        public double? TotalDocumentsAmountAfterDiscount { get; set; }
        public double? VatAmount { get; set; }
        public double? DocumentAmountAndVATAmount { get; set; }
        public decimal TaxWithholdingAmount { get; set; }
        public string CustomerVendorCode { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? DocuemntsReferenceDate { get; set; }
        public string BranchCode { get; set; }
        public string CreatedbyUser { get; set; }
        public string AddressId { get; set; }
        public string GLAccountId { get; set; }
        public string VendorId { get; set; }
        public string APInvoiceId { get; set; }
        public string ARPaymentMethod { get; set; }
        public string DepositId { get; set; }
        public string CashBookType { get; set; }
        public string ARPaymentId { get; set; }

    }

    public class Summary
    {
        public string VatNumber { get; set; }
        public string CompanyName { get; set; }


        public List<ReportData> Data { get; set; }
        public List<ReportTotal> ReportTotals { get; set; }

    }

    public class ReportData
    {

        public string RecordCode { get; set; }
        public string RecordDescription { get; set; }
        public int TotalRecords { get; set; }

    }

    public class ReportTotal
    {
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
