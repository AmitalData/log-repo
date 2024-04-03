using Logitude.BL.InvoiceModel.EntityQueries;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoDoctoRelacionadosCreditNotes : ComplementoPagosPagoDoctoRelacionados
    {
        public static Dictionary<string, CreditNoteMappedValue> GetDetails(List<ARInvoice> arInvoices)
        {
            Dictionary<string, CreditNoteMappedValue> creditNotesARInvoices = new Dictionary<string, CreditNoteMappedValue>();

            arInvoices.ForEach(invoice =>
            {
                AddCreditNoteInvoice(invoice, creditNotesARInvoices);
            });

            return creditNotesARInvoices;
        }

        private static void AddCreditNoteInvoice(ARInvoice invoice, Dictionary<string, CreditNoteMappedValue> creditNotesARInvoices)
        {
            List<ARInvoicePayment> allInvoicePayments = GetConnectedInvoices(invoice);
            decimal invoiceAmountToPay = GetConnectedInvoiceAmountToPay(allInvoicePayments);
            if (invoiceAmountToPay >= 0) return;

            decimal invoiceTaxAmountToPay = GetTaxAmountToPay(invoice);
            creditNotesARInvoices.Add(invoice.RelatedInvoice, new CreditNoteMappedValue { InvoiceAmountToPay = invoiceAmountToPay, InvoiceTaxAmountToPay = invoiceTaxAmountToPay });
        }

        private static decimal GetTaxAmountToPay(ARInvoice invoice)
        {
            const double oneHundredPercentage = 100;
            ARInvoiceLineQuery aRInvoiceLineQuery = new ARInvoiceLineQuery(invoice.Tenant);
            double? invoiceTaxAmount = aRInvoiceLineQuery.GetInvoiceLinePMsByInvoiceId(invoice.Id, invoice.Tenant).Select(a => a.ForiegnCurrencyAmount * (a.VatPercentage / oneHundredPercentage)).Sum();
            decimal invoiceTaxAmountToPay = invoiceTaxAmount == null ? 0 : (decimal)invoiceTaxAmount;
            return invoiceTaxAmountToPay;
        }
    }

    public class CreditNoteMappedValue
    {
        public decimal InvoiceAmountToPay { get; set; }
        public decimal InvoiceTaxAmountToPay { get; set; }
    }
}
