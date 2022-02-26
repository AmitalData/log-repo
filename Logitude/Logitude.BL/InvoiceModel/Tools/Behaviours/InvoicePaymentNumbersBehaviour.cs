using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours
{
    public class InvoicePaymentNumbersBehaviour
    {
        private int tenant;
        private APInvoicePM aPInvoice;
        private APPaymentPM aPPayment;


        public InvoicePaymentNumbersBehaviour(APInvoicePM aPInvoice)
        {
            this.aPInvoice = aPInvoice;
            this.tenant = aPInvoice.Tenant;
        }
        public InvoicePaymentNumbersBehaviour(APPaymentPM aPPayment)
        {
            this.aPPayment = aPPayment;
            this.tenant = aPPayment.Tenant;
        }

        public string CopmuteAPInvoicePaymentsNumbers()
        {
            string numbersField = "";
            foreach (APInvoicePaymentPM invoicePayment in aPInvoice.InvoicePayments.Where(d=> d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
            {
                this.AddNumberToNumbersField(ref numbersField, invoicePayment.PaymentNumber);
                this.TrimLengthTo1000(ref numbersField);                
            }

            return numbersField;
        }

        public string CopmuteAPPaymentInvoicesNumbers()
        {
            string numbersField = "";
            foreach (APPaymentInvoicePM paymentInvoice in aPPayment.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
            {                
                this.AddNumberToNumbersField(ref numbersField, paymentInvoice.APInvoiceNumber);
                this.TrimLengthTo1000(ref numbersField);                
            }

            return numbersField;
        }

        private void AddNumberToNumbersField(ref string allNumbersField, string connectedNumber)
        {
            if (!string.IsNullOrEmpty(connectedNumber))
            {
                if (!allNumbersField.Contains(connectedNumber))
                {
                    allNumbersField = string.IsNullOrEmpty(allNumbersField) ? connectedNumber : allNumbersField + ", " + connectedNumber;
                }
            }
        }
        private void TrimLengthTo1000(ref string allNumbersField)
        {
            if (!string.IsNullOrEmpty(allNumbersField))
            {
                if (allNumbersField.Length > 1000)
                {
                    allNumbersField = allNumbersField.Substring(0, 1000);
                }
            }
        }
    }
}
