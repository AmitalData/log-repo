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
        private static ARInvoiceQuery aRInvoiceQuery;
        private static Tenant currentTenant;

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
            List<PagosPagoDoctoRelacionado> doctos = new List<PagosPagoDoctoRelacionado>();
            paymentARInvoices.ForEach(invoice =>
            {
                doctos.Add(GetNewDocto(invoice));
            });
            return doctos;
        }

        private static PagosPagoDoctoRelacionado GetNewDocto(ARInvoice invoice)
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
            PagosPagoDoctoRelacionado doctoItem = GetNewInstancePagosPagoDoctoRelacionado(invoice, invoiceComprobante, invoiceComprobanteV33);
            

            return doctoItem;
        }

        private static PagosPagoDoctoRelacionado GetNewInstancePagosPagoDoctoRelacionado(ARInvoice invoice, Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {

            List<ARInvoicePayment> allInvoicePayments = GetConnectedInvoices(invoice);
            XmlElement[] comprobanteComplementoAnyXmlElements = GetConnectedInvoiceComplemento(invoiceComprobante, invoiceComprobanteV33);
            decimal totalInvoiceComprobante = GetConnectedInvoiceTotal(invoiceComprobante, invoiceComprobanteV33);
            decimal invoiceAmountToPay = GetConnectedInvoiceAmountToPay(allInvoicePayments);
            string arInvoiceCode = GetARInvoiceCode(currencies, invoice);

            return new PagosPagoDoctoRelacionado
            {
                MonedaDR = arInvoiceCode,
                Serie = GetConnectedInvoiceSerie(invoiceComprobante, invoiceComprobanteV33),
                Folio = GetConnectedFolio(invoiceComprobante, invoiceComprobanteV33),
                ObjetoImpDR = SATData.IncludeTaxObjetoImp,
                ImpPagado = invoiceAmountToPay,
                ImpPagadoSpecified = true,
                EquivalenciaDR = GetEquivalenciaDR(invoice, arInvoiceCode),
                EquivalenciaDRSpecified = true,
                IdDocumento = GetPagosPagoDoctoRelacionadoIdDocumento(comprobanteComplementoAnyXmlElements),
                ImpSaldoAnt = GetPagosPagoDoctoRelacionadoImpSaldoAnt(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante }),
                NumParcialidad = GetPagosPagoDoctoRelacionadoImpSaldoAntNumParcialidad(invoice, arPaymentPM, allInvoicePayments),
                ImpSaldoInsoluto = GetGetPagosPagoDoctoRelacionadoImpSaldoInsoluto(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante }),
                ImpuestosDR = GetDoctoImpuestosDR(invoice, invoiceAmountToPay),
            };
        }

        private static List<ARInvoicePayment> GetConnectedInvoices(ARInvoice invoice)
        {
            return (from a in invoiceContext.ARInvoicePayments.Include("ARPayment")
                    where a.ARInvoiceId == invoice.Id && a.Tenant == invoice.Tenant
                    select a).ToList();
        }

        private static XmlElement[] GetConnectedInvoiceComplemento(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Complemento.Any : invoiceComprobanteV33.Complemento.Any;
        }

        private static decimal GetConnectedInvoiceTotal(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Total : invoiceComprobanteV33.Total;
        }

        private static decimal GetConnectedInvoiceAmountToPay(List<ARInvoicePayment> allInvoicePayments)
        {
            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)(allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == arPaymentPM.Id)).ForeignAmount);
        }

        private static string GetARInvoiceCode(List<Currency> currencies, ARInvoice invoice)
        {
            Currency invoiceCurrency = currencies.FirstOrDefault(c => c.Id == invoice.InvoiceCurrencyId);
            return invoiceCurrency.Code;
        }

        private static string GetConnectedInvoiceSerie(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Serie : invoiceComprobanteV33.Serie;
        }

        private static string GetConnectedFolio(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Folio : invoiceComprobanteV33.Folio;
        }

        private static decimal GetEquivalenciaDR(ARInvoice invoice, string arInvoiceCode)
        {
            if (arPaymentPM.PaymentCurrencyCode == arInvoiceCode)
                return 1;

            if (invoice.InvoiceCurrencyExchangeRate == null || arPaymentPM.PaymentCurrencyExchangeRate == null)
                return 0;

            return SATBaseProfact40Service.GetDecimalWith6DigitsAfterPoint(Convert.ToDecimal(arPaymentPM.PaymentCurrencyExchangeRate.Value) / Convert.ToDecimal(invoice.InvoiceCurrencyExchangeRate.Value));
        }

        private static string GetPagosPagoDoctoRelacionadoIdDocumento(XmlElement[] comprobanteComplementoAnyXmlElements)
        {
            if (comprobanteComplementoAnyXmlElements == null) return null;

            List<System.Xml.XmlElement> myLXmlComplementos = comprobanteComplementoAnyXmlElements.ToList<System.Xml.XmlElement>();
            System.Xml.XmlElement timbreFiscalDigitalXmlElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalXmlElement != null)
            {
                return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalXmlElement.OuterXml).UUID;
            }

            return null;
        }

        private static decimal GetPagosPagoDoctoRelacionadoImpSaldoAnt(PagosPagoDoctoRelacionadoImpSaldo pagosPagoDoctoRelacionadoImpSaldo)
        {
            decimal previouslySentPaymentsTotal = 0;
            if (pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.AmountInPaymentCurrency == pagosPagoDoctoRelacionadoImpSaldo.ARInvoice.AmountInInvoiceCurrency)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
            }

            List<ARInvoicePayment> sentInvoicePayments = pagosPagoDoctoRelacionadoImpSaldo.AllInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.Id).ToList();
            if (sentInvoicePayments.Count == 0)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
            }

            sentInvoicePayments.ForEach(p =>
            {
                previouslySentPaymentsTotal += (decimal)(p.ForeignAmount.Value);
            });

            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal));
        }

        private static string GetPagosPagoDoctoRelacionadoImpSaldoAntNumParcialidad(ARInvoice invoice, ARPaymentPM entityPM, List<ARInvoicePayment> allInvoicePayments)
        {
            List<ARInvoicePayment> sentInvoicePayments = allInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == SATData.TransferedSATTransferStatusCode && a.ARPaymentId != entityPM.Id).ToList();
            return entityPM.AmountInPaymentCurrency == invoice.AmountInInvoiceCurrency || sentInvoicePayments.Count == 0
                ? "1"
                : (sentInvoicePayments.Count() + 1).ToString();
        }

        private static decimal GetGetPagosPagoDoctoRelacionadoImpSaldoInsoluto(PagosPagoDoctoRelacionadoImpSaldo pagosPagoDoctoRelacionadoImpSaldo)
        {
            decimal pagosPagoDoctoRelacionadoImpSaldoAnt = GetPagosPagoDoctoRelacionadoImpSaldoAnt(pagosPagoDoctoRelacionadoImpSaldo);
            decimal invoicePaymentForeignAmount = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((decimal)(pagosPagoDoctoRelacionadoImpSaldo.AllInvoicePayments.FirstOrDefault(p => p.ARPaymentId == pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.Id)).ForeignAmount);
            if (pagosPagoDoctoRelacionadoImpSaldoAnt != 0)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagosPagoDoctoRelacionadoImpSaldoAnt - invoicePaymentForeignAmount);
            }
            return 0;
        }

        private static PagosPagoDoctoRelacionadoImpuestosDR GetDoctoImpuestosDR(ARInvoice invoice, decimal invoiceAmountToPay)
        {
            ARInvoicePM arInvoicePM = aRInvoiceQuery.GetSinglePM(invoice.Id, arPaymentPM.Tenant);
            SATInvoiceComprobante sATInvoiceComprobante = new SATInvoiceComprobante(arInvoicePM, currentTenant, null);

            List<ARInvoiceTotalVATPM> arTotalVats = sATInvoiceComprobante.GetARInvoiceTotalVATPMs();
            ComprobanteTotalAndSubTotal comprobanteTotalAndSubTotal = sATInvoiceComprobante.GetComprobanteTotalAndSubTotal(arTotalVats);
            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> trasladoDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> retencionDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();

            ComprobanteImpuestosResults comprobanteImpuestosResults = sATInvoiceComprobante.GetComprobanteImpuestos(arTotalVats, sATInvoiceComprobante.GetConceptoList().ToArray());

            comprobanteImpuestosResults?.ComprobanteImpuestos?.Traslados?.ToList().ForEach(comprobanteImpuestosTraslado =>
            {
                trasladoDRList.Add(GetNewPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(comprobanteImpuestosTraslado));
            });

            comprobanteImpuestosResults?.ComprobanteImpuestosRetencionDRs.ToList().ForEach(comprobanteImpuestosRetencion =>
            {
                retencionDRList.Add(GetNewPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(comprobanteImpuestosRetencion));
            });

            MapPaidAmounts(new FullyPatiallyPaidAmountArgs { InvoiceAmountToPay = invoiceAmountToPay, TrasladoDRs = trasladoDRList, RetencionDRs = retencionDRList, TotalInvoiceAmount = comprobanteTotalAndSubTotal.Total });

            PagosPagoDoctoRelacionadoImpuestosDR pagosPagoDoctoRelacionadoImpuestosDR = new PagosPagoDoctoRelacionadoImpuestosDR();
            if (trasladoDRList.Count > 0) pagosPagoDoctoRelacionadoImpuestosDR.TrasladosDR = trasladoDRList.ToArray();
            if (retencionDRList.Count > 0) pagosPagoDoctoRelacionadoImpuestosDR.RetencionesDR = retencionDRList.ToArray();

            return pagosPagoDoctoRelacionadoImpuestosDR;
        }

        private static void MapPaidAmounts(FullyPatiallyPaidAmountArgs fullyPatiallyPaidAmountArgs)
        {
            decimal invoiceAmountToPayRatio = GetinvoiceAmountToPayRatio(fullyPatiallyPaidAmountArgs.TotalInvoiceAmount, fullyPatiallyPaidAmountArgs.InvoiceAmountToPay);
            fullyPatiallyPaidAmountArgs.TrasladoDRs.ForEach(trasladoDR =>
            {
                trasladoDR.BaseDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(trasladoDR.BaseDR * invoiceAmountToPayRatio);
                trasladoDR.ImporteDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(trasladoDR.ImporteDR * invoiceAmountToPayRatio);
            });
            fullyPatiallyPaidAmountArgs.RetencionDRs.ForEach(retencionDR =>
            {
                retencionDR.BaseDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(retencionDR.BaseDR * invoiceAmountToPayRatio);
                retencionDR.ImporteDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(retencionDR.ImporteDR * invoiceAmountToPayRatio);
            });
        }

        private static PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR GetNewPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(ComprobanteImpuestosTraslado comprobanteImpuestosTraslado)
        {
            return new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
            {
                BaseDR = comprobanteImpuestosTraslado.Base,
                ImporteDR = comprobanteImpuestosTraslado.Importe,
                ImporteDRSpecified = comprobanteImpuestosTraslado.TipoFactor != "Exento",
                ImpuestoDR = comprobanteImpuestosTraslado.Impuesto,
                TasaOCuotaDR = comprobanteImpuestosTraslado.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosTraslado.TasaOCuota) : 0,
                TasaOCuotaDRSpecified = comprobanteImpuestosTraslado.TipoFactor != "Exento",
                TipoFactorDR = comprobanteImpuestosTraslado.TipoFactor,
            };
        }

        private static decimal GetinvoiceAmountToPayRatio(decimal totalInvoiceAmount, decimal invoiceAmountToPay)
        {
            if (invoiceAmountToPay == totalInvoiceAmount) return 1;

            return invoiceAmountToPay / totalInvoiceAmount;
        }

        private static PagosPagoDoctoRelacionadoImpuestosDRRetencionDR GetNewPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(ComprobanteImpuestosRetencionDR comprobanteImpuestosRetencionDR)
        {
            return new PagosPagoDoctoRelacionadoImpuestosDRRetencionDR
            {
                ImporteDR = comprobanteImpuestosRetencionDR.Importe,
                ImpuestoDR = comprobanteImpuestosRetencionDR.Impuesto,
                BaseDR = comprobanteImpuestosRetencionDR.Base,
                TasaOCuotaDR = comprobanteImpuestosRetencionDR.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosRetencionDR.TasaOCuota) : 0,
                TipoFactorDR = comprobanteImpuestosRetencionDR.TipoFactor,
            };
        }
    }

    public class PagosPagoDoctoRelacionadoImpSaldo
    {
        public ARPaymentPM ARPaymentPM { get; set; }
        public ARInvoice ARInvoice { get; set; }
        public List<ARInvoicePayment> AllInvoicePayments { get; set; }
        public decimal InvoiceAmount { get; set; }
    }

    public class FullyPatiallyPaidAmountArgs
    {
        public List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> TrasladoDRs { get; set; }
        public List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> RetencionDRs { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public decimal InvoiceAmountToPay { get; set; }
    }
}
