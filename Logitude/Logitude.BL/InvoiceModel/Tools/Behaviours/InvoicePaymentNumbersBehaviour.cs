using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
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
        public InvoicePaymentNumbersBehaviour(int tenant)
        {
            this.tenant = tenant;
        }

        public string CopmuteAPInvoicePaymentsNumbers(APInvoicePM aPInvoice)
        {
            string numbersField = "";
            foreach (APInvoicePaymentPM invoicePayment in aPInvoice.InvoicePayments.Where(d=> d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
            {
                this.AddNumberToNumbersField(ref numbersField, invoicePayment.PaymentNumber);
                this.TrimLength(ref numbersField);                
            }

            return numbersField;
        }

        public string CopmuteAPPaymentInvoicesNumbers(APPaymentPM aPPayment)
        {
            string numbersField = "";
            foreach (APPaymentInvoicePM paymentInvoice in aPPayment.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
            {                
                this.AddNumberToNumbersField(ref numbersField, paymentInvoice.APInvoiceNumber);
                this.TrimLength(ref numbersField);                
            }

            return numbersField;
        }

        public string CopmuteARPaymentInvoicesNumbers(ARPaymentPM aRPayment)
        {
            string numbersField = "";
            foreach (ARPaymentInvoicePM paymentInvoice in aRPayment.PaymentInvoices.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete))
            {
                this.AddNumberToNumbersField(ref numbersField, paymentInvoice.ARInvoiceNumber);
                this.TrimLength(ref numbersField, 4000);
            }

            return numbersField;
        }

        public string CopmuteAPPaymentInvoicesNumbersFromInvoicePayments(IQueryable<APInvoicePayment> payments)
        {
            string numbersField = "";
            foreach (APInvoicePayment payment in payments)
            {
                this.AddNumberToNumbersField(ref numbersField, payment.APInvoice?.InvoiceNumber);
                this.TrimLength(ref numbersField);
            }

            return numbersField;
        }

        public string CopmuteARPaymentInvoicesNumbersFromInvoicePayments(IQueryable<ARInvoicePayment> invoices)
        {
            string numbersField = "";
            foreach (ARInvoicePayment invoicePayment in invoices)
            {
                this.AddNumberToNumbersField(ref numbersField, invoicePayment.ARInvoice?.InvoiceNumber);
                this.TrimLength(ref numbersField, 4000);
            }

            return numbersField;
        }

        public string BuildARPyaymentSingleInvoiveNumber(IQueryable<ARInvoicePayment> invoices, IInvoiceContext objectContext)
        {
            string invoiceNumber = null;
            if (invoices.Count() > 0)
            {
                if (invoices.Count() == 1)
                {
                    string invoiceId = invoices.Select(s => s.ARInvoiceId).FirstOrDefault();

                    var data = (from d in objectContext.ARInvoices
                                where d.Tenant == this.tenant
                                && d.Id == invoiceId
                                select new
                                {
                                    InvoiceNumber = d.InvoiceNumber,
                                }).FirstOrDefault();

                    invoiceNumber = data.InvoiceNumber;
                }

                else
                {
                    invoiceNumber = "Multi";
                }
            }

            return invoiceNumber;
        }

        private void AddNumberToNumbersField(ref string allNumbersField, string connectedNumber)
        {
            if (!string.IsNullOrEmpty(connectedNumber))
            {
                allNumbersField = string.IsNullOrEmpty(allNumbersField) ? connectedNumber : allNumbersField + ", " + connectedNumber;
            }
        }
        private void TrimLength(ref string allNumbersField, int lenght = 1000)
        {
            if (!string.IsNullOrEmpty(allNumbersField))
            {
                if (allNumbersField.Length > lenght)
                {
                    allNumbersField = allNumbersField.Substring(0, lenght);
                }
            }
        }        
    }
}
