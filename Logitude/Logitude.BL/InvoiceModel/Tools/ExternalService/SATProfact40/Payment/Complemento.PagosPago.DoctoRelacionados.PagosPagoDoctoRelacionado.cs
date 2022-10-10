using Logitude.BL.InvoiceModel.EntityPMs;
using Profact.TimbraCFDI40;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPagoDoctoRelacionadosPagosPagoDoctoRelacionado : ComplementoPagosPagoDoctoRelacionados
    {
        public static PagosPagoDoctoRelacionado GetNewInstance(PagoDoctoRelacionadoArgs pagoDoctoRelacionadoArgs)
        {
            ARInvoice invoice = pagoDoctoRelacionadoArgs.Invoice;
            Comprobante invoiceComprobante = pagoDoctoRelacionadoArgs.InvoiceComprobante;
            Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33 = pagoDoctoRelacionadoArgs.InvoiceComprobanteV33;
            CreditNoteMappedValue relatdCreatedInvoiceValue = pagoDoctoRelacionadoArgs.RelatdCreatedInvoiceValue;
            decimal relatdCreatedInvoiceAmount = relatdCreatedInvoiceValue == null ? 0 : relatdCreatedInvoiceValue.InvoiceAmountToPay;

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
                ImpPagado = invoiceAmountToPay + relatdCreatedInvoiceAmount,
                ImpPagadoSpecified = true,
                EquivalenciaDR = GetEquivalenciaDR(invoice, arInvoiceCode, allInvoicePayments),
                EquivalenciaDRSpecified = true,
                IdDocumento = GetPagosPagoDoctoRelacionadoIdDocumento(comprobanteComplementoAnyXmlElements),
                ImpSaldoAnt = GetPagosPagoDoctoRelacionadoImpSaldoAnt(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante, RelatdCreatedInvoiceAmount = relatdCreatedInvoiceAmount }),
                NumParcialidad = GetPagosPagoDoctoRelacionadoImpSaldoAntNumParcialidad(invoice, arPaymentPM, allInvoicePayments),
                ImpSaldoInsoluto = GetGetPagosPagoDoctoRelacionadoImpSaldoInsoluto(new PagosPagoDoctoRelacionadoImpSaldo { ARPaymentPM = arPaymentPM, ARInvoice = invoice, AllInvoicePayments = allInvoicePayments, InvoiceAmount = totalInvoiceComprobante }),
                ImpuestosDR = GetDoctoImpuestosDR(invoice, invoiceAmountToPay, relatdCreatedInvoiceValue),
            };
        }

        private static XmlElement[] GetConnectedInvoiceComplemento(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Complemento.Any : invoiceComprobanteV33.Complemento.Any;
        }

        private static decimal GetConnectedInvoiceTotal(Comprobante invoiceComprobante, Profact.TimbraCFDI33.Comprobante invoiceComprobanteV33)
        {
            return invoiceComprobante != null ? invoiceComprobante.Total : invoiceComprobanteV33.Total;
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

        private static decimal GetEquivalenciaDR(ARInvoice invoice, string arInvoiceCode, List<ARInvoicePayment> allInvoicePayments)
        {
            if (arPaymentPM.PaymentCurrencyCode == arInvoiceCode) return 1;

            if (arPaymentPM.PaymentCurrencyExchangeRate == null) return 0;
            decimal extraExchangeRate = 0.000001M;
            if (currentTenant?.Currency?.Code == arInvoiceCode && invoice.InvoiceCurrencyExchangeRate != null)
            {
                return SATBaseProfact40Service.GetDecimalWith6DigitsAfterPoint(Convert.ToDecimal(arPaymentPM.PaymentCurrencyExchangeRate.Value) / Convert.ToDecimal(invoice.InvoiceCurrencyExchangeRate.Value)) + extraExchangeRate;
            }

            double? invoiceCurrencyExchangeRate = allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == arPaymentPM.Id).ExchangeRate;

            if (invoiceCurrencyExchangeRate == null) return 0;

            return SATBaseProfact40Service.GetDecimalWith6DigitsAfterPoint(Convert.ToDecimal(arPaymentPM.PaymentCurrencyExchangeRate.Value) / Convert.ToDecimal(invoiceCurrencyExchangeRate)) + extraExchangeRate;
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
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal + pagosPagoDoctoRelacionadoImpSaldo.RelatdCreatedInvoiceAmount));
            }

            List<ARInvoicePayment> sentInvoicePayments = pagosPagoDoctoRelacionadoImpSaldo.AllInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != pagosPagoDoctoRelacionadoImpSaldo.ARPaymentPM.Id).ToList();
            if (sentInvoicePayments.Count == 0)
            {
                return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal + pagosPagoDoctoRelacionadoImpSaldo.RelatdCreatedInvoiceAmount));
            }

            sentInvoicePayments.ForEach(p =>
            {
                previouslySentPaymentsTotal += (decimal)(p.ForeignAmount.Value);
            });

            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint((pagosPagoDoctoRelacionadoImpSaldo.InvoiceAmount - previouslySentPaymentsTotal + pagosPagoDoctoRelacionadoImpSaldo.RelatdCreatedInvoiceAmount));
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

        private static PagosPagoDoctoRelacionadoImpuestosDR GetDoctoImpuestosDR(ARInvoice invoice, decimal invoiceAmountToPay, CreditNoteMappedValue relatdCreatedInvoiceValue)
        {
            ARInvoicePM arInvoicePM = aRInvoiceQuery.GetSinglePM(invoice.Id, arPaymentPM.Tenant);
            SATInvoiceComprobante sATInvoiceComprobante = new SATInvoiceComprobante(arInvoicePM, currentTenant, null);

            List<ARInvoiceTotalVATPM> arTotalVats = sATInvoiceComprobante.GetARInvoiceTotalVATPMs();
            ComprobanteTotalAndSubTotal comprobanteTotalAndSubTotal = sATInvoiceComprobante.GetComprobanteTotalAndSubTotal(arTotalVats);
            List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> trasladoDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR>();
            List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> retencionDRList = new List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR>();

            ComprobanteImpuestosResults comprobanteImpuestosResults = sATInvoiceComprobante.GetComprobanteImpuestos(arTotalVats, sATInvoiceComprobante.GetConceptoList().ToArray());
            FullyPatiallyPaidAmountArgs fullyPatiallyPaidAmountArgs = new FullyPatiallyPaidAmountArgs { InvoiceAmountToPay = invoiceAmountToPay, TotalInvoiceAmount = comprobanteTotalAndSubTotal.Total };

            comprobanteImpuestosResults?.ComprobanteImpuestos?.Traslados?.ToList().ForEach(comprobanteImpuestosTraslado =>
            {
                trasladoDRList.Add(GetNewPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(comprobanteImpuestosTraslado, relatdCreatedInvoiceValue, fullyPatiallyPaidAmountArgs));
            });

            comprobanteImpuestosResults?.ComprobanteImpuestosRetencionDRs.ToList().ForEach(comprobanteImpuestosRetencion =>
            {
                retencionDRList.Add(GetNewPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(comprobanteImpuestosRetencion, relatdCreatedInvoiceValue, fullyPatiallyPaidAmountArgs));
            });

            PagosPagoDoctoRelacionadoImpuestosDR pagosPagoDoctoRelacionadoImpuestosDR = new PagosPagoDoctoRelacionadoImpuestosDR();
            if (trasladoDRList.Count > 0) pagosPagoDoctoRelacionadoImpuestosDR.TrasladosDR = trasladoDRList.ToArray();
            if (retencionDRList.Count > 0) pagosPagoDoctoRelacionadoImpuestosDR.RetencionesDR = retencionDRList.ToArray();

            return pagosPagoDoctoRelacionadoImpuestosDR;
        }

        private static PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR GetNewPagosPagoDoctoRelacionadoImpuestosDRTrasladoDR(ComprobanteImpuestosTraslado comprobanteImpuestosTraslado, CreditNoteMappedValue relatdCreatedInvoiceValue, FullyPatiallyPaidAmountArgs fullyPatiallyPaidAmountArgs)
        {
            decimal relatdCreatedInvoiceAmountToPay = relatdCreatedInvoiceValue == null ? 0 : relatdCreatedInvoiceValue.InvoiceAmountToPay;
            decimal invoiceAmountToPayRatio = GetinvoiceAmountToPayRatio(fullyPatiallyPaidAmountArgs.TotalInvoiceAmount, fullyPatiallyPaidAmountArgs.InvoiceAmountToPay + relatdCreatedInvoiceAmountToPay);

            return new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
            {
                BaseDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(comprobanteImpuestosTraslado.Base * invoiceAmountToPayRatio),
                ImporteDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(comprobanteImpuestosTraslado.Importe * invoiceAmountToPayRatio),
                ImporteDRSpecified = comprobanteImpuestosTraslado.TipoFactor != "Exento",
                ImpuestoDR = comprobanteImpuestosTraslado.Impuesto,
                TasaOCuotaDR = comprobanteImpuestosTraslado.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosTraslado.TasaOCuota) : 0,
                TasaOCuotaDRSpecified = comprobanteImpuestosTraslado.TipoFactor != "Exento",
                TipoFactorDR = comprobanteImpuestosTraslado.TipoFactor,
            };
        }


        private static PagosPagoDoctoRelacionadoImpuestosDRRetencionDR GetNewPagosPagoDoctoRelacionadoImpuestosDRRetencionDR(ComprobanteImpuestosRetencionDR comprobanteImpuestosRetencionDR, CreditNoteMappedValue relatdCreatedInvoiceValue, FullyPatiallyPaidAmountArgs fullyPatiallyPaidAmountArgs)
        {
            decimal relatdCreatedInvoiceAmountToPay = relatdCreatedInvoiceValue == null ? 0 : relatdCreatedInvoiceValue.InvoiceAmountToPay;
            decimal invoiceAmountToPayRatio = GetinvoiceAmountToPayRatio(fullyPatiallyPaidAmountArgs.TotalInvoiceAmount, fullyPatiallyPaidAmountArgs.InvoiceAmountToPay + relatdCreatedInvoiceAmountToPay);

            return new PagosPagoDoctoRelacionadoImpuestosDRRetencionDR
            {
                ImporteDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(comprobanteImpuestosRetencionDR.Importe * invoiceAmountToPayRatio),
                ImpuestoDR = comprobanteImpuestosRetencionDR.Impuesto,
                BaseDR = SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(comprobanteImpuestosRetencionDR.Base * invoiceAmountToPayRatio),
                TasaOCuotaDR = comprobanteImpuestosRetencionDR.TasaOCuota != null ? Convert.ToDecimal(comprobanteImpuestosRetencionDR.TasaOCuota) : 0,
                TipoFactorDR = comprobanteImpuestosRetencionDR.TipoFactor,
            };
        }


        private static decimal GetinvoiceAmountToPayRatio(decimal totalInvoiceAmount, decimal invoiceAmountToPay)
        {
            if (invoiceAmountToPay == totalInvoiceAmount) return 1;

            return invoiceAmountToPay / totalInvoiceAmount;
        }
    }

    public class PagoDoctoRelacionadoArgs
    {
        public ARInvoice Invoice { get; set; } 
        public Comprobante InvoiceComprobante { get; set; }
        public Profact.TimbraCFDI33.Comprobante  InvoiceComprobanteV33 { get; set; }
        public CreditNoteMappedValue RelatdCreatedInvoiceValue { get; set; }
    }
}
