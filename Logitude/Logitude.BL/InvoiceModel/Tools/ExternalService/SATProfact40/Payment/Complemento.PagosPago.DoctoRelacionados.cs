using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools;
using Profact.TimbraCFDI40;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoDoctoRelacionados : ComplementoPagosPago
    {
        public static ARInvoiceQuery aRInvoiceQuery;
        public static Tenant currentTenant;

        public static List<PagosPagoDoctoRelacionado> Get(Tenant tenant)
        {
            aRInvoiceQuery = new ARInvoiceQuery(arPaymentPM.Tenant);
            currentTenant = tenant;
            List<ARInvoice> paymentARInvoices = GetPaymentARInvoices();
            List<PagosPagoDoctoRelacionado> doctos = BuildDoctos(paymentARInvoices);

            return doctos;
        }

        private static List<ARInvoice> GetPaymentARInvoices()
        {
            List<string> invoiceIds = arPaymentPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();
            List<ARInvoice> paymentARInvoices = (from a in invoiceContext.ARInvoices
                                                 where a.Tenant == arPaymentPM.Tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();
            return paymentARInvoices;
        }

        private static List<PagosPagoDoctoRelacionado> BuildDoctos(List<ARInvoice> paymentARInvoices)
        {
            Dictionary<string, CreditNoteMappedValue> creditNotesARInvoices = ComplementoPagosPagoDoctoRelacionadosCreditNotes.GetDetails(paymentARInvoices);
            List<PagosPagoDoctoRelacionado> doctos = new List<PagosPagoDoctoRelacionado>();
            paymentARInvoices.ForEach(invoice =>
            {
                AddNewDocto(invoice, creditNotesARInvoices, doctos);
            });
            return doctos;
        }

        private static void AddNewDocto(ARInvoice invoice, Dictionary<string, CreditNoteMappedValue> creditNotesARInvoices, List<PagosPagoDoctoRelacionado> doctos)
        {
            List<ARInvoicePayment> allInvoicePayments = GetConnectedInvoices(invoice);
            decimal invoiceAmountToPay = GetConnectedInvoiceAmountToPay(allInvoicePayments);
            if (invoiceAmountToPay < 0) return;

            doctos.Add(GetNewDocto(invoice, creditNotesARInvoices));
        }

        private static PagosPagoDoctoRelacionado GetNewDocto(ARInvoice invoice, Dictionary<string, CreditNoteMappedValue> creditNotesARInvoices)
        {
            Comprobante invoiceComprobante = null;
            Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33 = null;
            try
            {
                invoiceComprobante = LogitudeXmlSerializer.DeserializeObject<Comprobante>(invoice.SATXML);
            }
            catch (Exception ex)
            {
                invoiceComprobanteV33 = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(invoice.SATXML);
            }

            KeyValuePair<string, CreditNoteMappedValue> creditNotesARInvoice = creditNotesARInvoices.Where(creditInvoice => creditInvoice.Key == invoice.InvoiceNumber).FirstOrDefault();
            CreditNoteMappedValue relatdCreatedInvoiceValue = !creditNotesARInvoice.Equals(default(KeyValuePair<string, CreditNoteMappedValue>)) ? creditNotesARInvoice.Value : null;
            PagosPagoDoctoRelacionado doctoItem = ComplementoPagosPagoDoctoRelacionadosPagosPagoDoctoRelacionado.GetNewInstance(new PagoDoctoRelacionadoArgs { Invoice = invoice, InvoiceComprobante = invoiceComprobante, InvoiceComprobanteV33 = invoiceComprobanteV33, RelatdCreatedInvoiceValue = relatdCreatedInvoiceValue });

            return doctoItem;
        }

        public static List<ARInvoicePayment> GetConnectedInvoices(ARInvoice invoice)
        {
            return (from a in invoiceContext.ARInvoicePayments.Include("ARPayment")
                    where a.ARInvoiceId == invoice.Id && a.Tenant == invoice.Tenant
                    select a).ToList();
        }

        public static decimal GetConnectedInvoiceAmountToPay(List<ARInvoicePayment> allInvoicePayments)
        {
            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)(allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == arPaymentPM.Id)).ForeignAmount);
        }


        public class PagosPagoDoctoRelacionadoImpSaldo
        {
            public ARPaymentPM ARPaymentPM { get; set; }
            public ARInvoice ARInvoice { get; set; }
            public List<ARInvoicePayment> AllInvoicePayments { get; set; }
            public decimal InvoiceAmount { get; set; }
            public decimal RelatdCreatedInvoiceAmount { get; set; }
        }

        public class FullyPatiallyPaidAmountArgs
        {
            public List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> TrasladoDRs { get; set; }
            public List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> RetencionDRs { get; set; }
            public decimal TotalInvoiceAmount { get; set; }
            public decimal InvoiceAmountToPay { get; set; }
        }
    }
}