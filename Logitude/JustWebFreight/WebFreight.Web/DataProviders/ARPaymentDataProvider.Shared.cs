using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ARPaymentDataProvider : BaseDataProvider
    {
        public string PaymentStatus { get; set; }
        public string PaymentId { get; set; }

        public string ARPaymentNo { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentReference { get; set; }
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

        //paid to
        public string PaidTo { get; set; }
        public string PaidToVatNo { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }

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
        public double sumInvoices { get; set; }
        public string BankCode { get; set; }
        public string BranchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankAccountLocalName { get; set; }
        public string BankAccountEnglishName { get; set; }
        public string PaymentMethodRef { get; set; }



        public string BillToAddress { get; set; }
        public string PaymentExchangeRate { get; set; }
        public string DepositBankName { get; set; }
        public string DepositBankAccountNumber { get; set; }
        public string SATTransferStatus { get; set; }
		public string BillToCountryCode { get; set; }

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


        public List<ReportARInvoicePayments> PaidAPInvoicesList { get; set; }

        public class ReportARInvoicePayments
        {
            public string InvoiceNumber { get; set; }
            public string Reference { get; set; }
            public double? AmountPaid { get; set; }
            public double? AmountDue { get; set; }
            public double? Vat { get; set; }
            public double? OriginalAmount { get; set; }
            public string BillTo { get; set; }
            public string ShipmentNumber { set; get; }

            public string UUID { get; set; }
            public string CurrencyCode { get; set; }
            public decimal TipoCambio { get; set; }
            public string MetodoPagoCode { get; set; }
            public string Serie { get; set; }

            public string ARInvoiceField1 { get; set; }
            public string ARInvoiceField2 { get; set; }
            public string ARInvoiceField3 { get; set; }
            public string ARInvoiceField4 { get; set; }
            public string ARInvoiceField5 { get; set; }
            public string ARInvoiceField6 { get; set; }
            public string ARInvoiceField7 { get; set; }
            public string ARInvoiceField8 { get; set; }
            public string ARInvoiceField9 { get; set; }
            public string ARInvoiceField10 { get; set; }

            public DateTime? InvocieDate { get; set; }
            public DateTime? DueDate { get; set; }
            public string MasterNumber { get; set; }
            public string MasterShipmentNumber { get; set; }

        }
    }

    public class SATPayment
    {
        public string FormaPago { get; set; }
        public string UsoCFDI { get; set; }
        public DateTime? Fecha { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }
        public string TipoDeComprobante { get; set; }
        public string RegimenFiscal { get; set; }
        public string LugarExpedicion { get; set; }
        public string NoCertificado { get; set; }
        public string Certificado { get; set; }
        public DateTime? FechaTimbrado { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string SelloCFD { get; set; }
        public string SelloSAT { get; set; }
        public Image QRImage { get; set; }

        public string UUID { get; set; }
        public string MetodoPago { get; set; }
        public string NumOperacion { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal Monto { get; set; }
		public string SATForeignRFC { get; set; }

        public string TipoCadenaPago { get; set; }
        public string CertPago { get; set; }
        public string CadPago { get; set; }
        public string SelloPago { get; set; }

        //public string UUID { get; set; }
        //public string QR { get; set; }
        //public DateTime FechaTimbardo { get; set; }

        //public string PaymentMethodCode { get; set; }
        //public string PaymentMethodName { get; set; }
        //public string PaymentMethodLocalName { get; set; }



        public string CadenaOriginal { get; set; }

        //public string LugardeExpedicion { get; set; }
        //public string BillToZipCode { get; set; }
        //public string MetodoPago { get; set; }

        //public string TipoRelacion { get; set; }
        //public string CFDIRelacionado { get; set; }


        //        FormaPago: code and local name  separated by comma
        //        UsoCFDI
        //Fecha
        //Serie
        //Folio
        //TipoDeComprobante
        //RegimenFiscal
        //LugarExpedicion
        //NoCertificado
        //Certificado
        //FechaTimbrado
        //NoCertificadoSAT
        //SelloCFD
        //SelloSAT


    }


}