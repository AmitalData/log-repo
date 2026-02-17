using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class APPaymentDataProvider : BaseDataProvider
    {
        public string APPaymentNo { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string ClientNumber { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string PaidBy { get; set; } // custom
        public string PrintNotes { get; set; }
        public string IssuedByUserName { get; set; }
        public DateTime PrintDate { get; set; }

        public string ChequeOrPaymentRef { get; set; }
        public double? Amount { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }

        //paid to - Vendor
        public string PaidTo { get; set; }
        public string PaidToName { get; set; }
        public string PaidToAddress { get; set; }
        public string PaidToVatNo { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string VendorBankName { get; set; }
        public string VendorBankAddress { get; set; }
        public string VendorSwift { get; set; }
        public string VendorBankAccountNumber { get; set; }
        public string VendorIBANNo { get; set; }

        //tenant data
        public string Signature { get; set; }
        public string TenantData { get; set; }
        public string TenantVat { get; set; }
        public string TelLable { get; set; }
        public string FaxLable { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public double? OutstandingBalance { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalPayment { get; set; }

        public string CustomerField1 { get; set; }
        public string CustomerField2 { get; set; }
        public string CustomerField3 { get; set; }
        public string CustomerField4 { get; set; }
        public string CustomerField5 { get; set; }
        public string CustomerField6 { get; set; }
        public string CustomerField7 { get; set; }
        public string CustomerField8 { get; set; }
        public string CustomerField9 { get; set; }
        public string CustomerField10 { get; set; }
        public string BranchAddress { get; set; }

        public DateTime? TodayLocal { get; set; }

        public List<ReportAPInvoicePayments> PaidAPInvoicesList { get; set; }

        public class ReportAPInvoicePayments
        {
            public string InvoiceNumber { get; set; }
            public string Reference { get; set; }
            public double? AmountPaid { get; set; }
            public double? AmountDue { get; set; }
            public double? Vat { get; set; }
            public double? OriginalAmount { get; set; }
            public DateTime? InvoiceData { get; set; }
        }
    }
}