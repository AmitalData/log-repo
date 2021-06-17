using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class PaymentDataProvider : BaseDataProvider
    {
        public string PaymentNo { get; set; }
        public string ReceivedFrom { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToVatNo { get; set; }
        public DateTime Today { get; set; }

        public DateTime? RegisterDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodLocalName { get; set; }
        public string PaidBy { get; set; } // custom
        public string ClientNumber { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string AccountingCard { get; set; }
        public string InternalNotes { get; set; }
        public double? TotalPayment { get; set; }
        public string IssuedByUserName { get; set; }
        public DateTime PrintDate { get; set; }
        public string StatusName { get; set; }
        public string PrintNotes { get; set; }

        //tenant data
        public string Signature { get; set; }
        public string TenantName { get; set; }
        public string TenantVatNo { get; set; }
        public string TenantAddress { get; set; }
        public string TenantData { get; set; }
        public string TelLable { get; set; }
        public string FaxLable { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public double? OutstandingBalance  { get; set; }
        public double? TotalAmount { get; set; }
        public string TotalAmountInWordsEnglish { get; set; }
        public string TotalAmountInWordsSpanish { get; set; }
        public string ReceivedFromInLocal { get; set; }
        public string CopyName { get; set; }
        public string ChequeOrPaymentRef { get; set; }
        public double Amount { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }
        public string VATNumber { get; set; }
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
        public string BankCode { get; set; }
        public string BranchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankAccountLocalName { get; set; }
        public string BankAccountEnglishName { get; set; }
  
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string BranchAddress { get; set; }
		public string BillToCountryCode { get; set; }
        public string ForeignRFC { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string IssuedByLocalName { get; set; }

        public double? AmountInLocalCurrency { get; set; }

        public List<InvoicePayments> PaidInvoicesList { get; set; }
        public List<ARPaymentCheque> ARPaymentCheques { get; set; }

        public string ARPaymentField1 { get; set; }
        public string ARPaymentField2 { get; set; }
        public string ARPaymentField3 { get; set; }
        public string ARPaymentField4 { get; set; }
        public string ARPaymentField5 { get; set; }
        public string ARPaymentField6 { get; set; }
        public string ARPaymentField7 { get; set; }
        public string ARPaymentField8 { get; set; }
        public string ARPaymentField9 { get; set; }
        public string ARPaymentField10 { get; set; }

        public class InvoicePayments
        {
            public string InvoiceNumber { get; set; }
            public string Reference { get; set; }
            public double? AmountPaid { get; set; }
            public double? AmountDue { get; set; }
            public double? Vat { get; set; }
            public double? OriginalAmount { get; set; }
            public string HAWB { get; set; }
            public string InvoiceCurrency { get; set; }

            public string UUID { get; set; }
            public string CurrencyCode { get; set; }
            public decimal TipoCambio { get; set; }
            public string MetodoPagoCode { get; set; }
            public string Serie { get; set; }
            public string Folio { get; set; }
            public string NumParcialidad { get; set; }
            public decimal ImpSaldoAnt { get; set; }
            public decimal ImpPagado { get; set; }
            public decimal ImpSaldoInsoluto { get; set; }
            public double? InvoicePaymentExchangeRate { get; set; }


            //            UUID
            //            CurrencyCode
            //TipoCambio: exchange rate
            //MetodoPagoCode
            //Serie
            //            Folio
            //NumParcialidad
            //ImpSaldoAnt
            //ImpPagado
            //ImpSaldoInsoluto

        }

        public class ARPaymentCheque
        {
            public string ChequeOrPaymentRef { get; set; }
            public string Bank { get; set; }
            public string Branch { get; set; }
            public string Account { get; set; }
            public string ValueDate { get; set; }
            public string CurrencyCode { get; set; }
            public decimal LocalAmount { get; set; }
            public decimal ForeignAmount { get; set; }
            public string StatusName { get; set; }

        }



        public double? PaymentExchangeRate { get; set; }
        public string DepositBankName { get; set; }
        public string DepositBankAccountNumber { get; set; }
        public string SATTransferStatus { get; set; }
        public string BankName { get; set; }
        public string BankEnglishName { get; set; }
        public string BankAccountNumber { get; set; }
        //        BillToAddress
        //PaymentExchangeRate
        //DepositBankName  from General tab
        //DepositBankAccountNumber
        //SATTransferStatus


        private SATPayment sAT;
        public SATPayment SAT
        {
            get
            {
                if (this.sAT == null)
                {
                    this.sAT = new SATPayment();
                }

                return sAT;
            }

            set
            {

                sAT = value;
            }
        }
    }
}