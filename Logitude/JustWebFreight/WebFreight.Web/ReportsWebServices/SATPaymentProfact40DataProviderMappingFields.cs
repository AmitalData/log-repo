using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using WebFreight.Web.DataProviders;
using System.Drawing;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using System.Text;

namespace WebFreight.Web.ReportsWebServices
{
    public class SATPaymentProfact40DataProviderMappingFields
    {

        public static void MapProfact40Fields(ARPayment currentPayment, PaymentDataProvider paymentDataProvider, IInvoiceContext invoiceCotnext, Card billToCard)
        {
            if (billToCard != null) MappingBillToCardFields(paymentDataProvider, billToCard);

            if (string.IsNullOrEmpty(currentPayment.SATXML)) return;

            UsoCFDIRepository usoCFDIRepository = new UsoCFDIRepository(currentPayment.Tenant);
            List<UsoCFDI> allUsoCFDIs = usoCFDIRepository.GetUsoCFDIs().ToList();
            Profact.TimbraCFDI40.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(currentPayment.SATXML);
            if (comprobante.Complemento.Any == null) return;

            List<XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return;

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            if (!string.IsNullOrEmpty(currentPayment.SATPaymentMethodCode))
            {
                SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == currentPayment.SATPaymentMethodCode).FirstOrDefault();
                paymentDataProvider.SAT.FormaPago = satPaymentMethod != null ? (satPaymentMethod.Code + "," + satPaymentMethod.LocalName) : comprobante.FormaPago;
            }

            UsoCFDI usoCFDI = allUsoCFDIs.FirstOrDefault(f => f.Code == comprobante.Receptor.UsoCFDI);
            if (usoCFDI != null)
            {
                paymentDataProvider.SAT.UsoCFDI = usoCFDI.Code + " " + usoCFDI.Name;
            }

            paymentDataProvider.SAT.Fecha = comprobante.Fecha;
            paymentDataProvider.SAT.Serie = comprobante.Serie;
            paymentDataProvider.SAT.Folio = comprobante.Folio;

            paymentDataProvider.SAT.TipoDeComprobante = comprobante.TipoDeComprobante == "E" ? "Egreso" : "Ingreso";

            if (comprobante.Emisor.RegimenFiscal != null && comprobante.Emisor.RegimenFiscal.Length > 0)
            {
                paymentDataProvider.SAT.RegimenFiscal = comprobante.Emisor.RegimenFiscal;
            }
            paymentDataProvider.SAT.LugarExpedicion = comprobante.LugarExpedicion;
            paymentDataProvider.SAT.NoCertificado = comprobante.NoCertificado;
            paymentDataProvider.SAT.Certificado = comprobante.Certificado;
            paymentDataProvider.SAT.FechaTimbrado = digitalTi.FechaTimbrado;
            paymentDataProvider.SAT.NoCertificadoSAT = GetSATTimbreFiscalDigitalValue("NoCertificadoSAT", timbreFiscalDigitalElement);
            paymentDataProvider.SAT.SelloCFD = GetSATTimbreFiscalDigitalValue("SelloCFD", timbreFiscalDigitalElement);
            paymentDataProvider.SAT.SelloSAT = GetSATTimbreFiscalDigitalValue("SelloSAT", timbreFiscalDigitalElement);
            paymentDataProvider.SAT.UUID = digitalTi.UUID;
            if (!string.IsNullOrEmpty(comprobante.MetodoPago))
            {
                paymentDataProvider.SAT.MetodoPago = (comprobante.MetodoPago == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
            }

            MapAdditionalFields(currentPayment, paymentDataProvider);
            MapPagosFields(paymentDataProvider, comprobante);
        }

        private static void MapPagosFields(PaymentDataProvider paymentDataProvider, Profact.TimbraCFDI40.Comprobante comprobante)
        {
            if (!comprobante.Pagos20Specified || comprobante.Complemento.Any == null) return; //////

            List<XmlElement> LXmlComplementos = comprobante.Complemento.Any.ToList();
            XmlElement documentElement = LXmlComplementos.First();

            Profact.TimbraCFDI40.Complementos.Pagos20.Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Profact.TimbraCFDI40.Complementos.Pagos20.Pagos>(documentElement.OuterXml);
            Profact.TimbraCFDI40.Complementos.Pagos20.PagosPago pagoItem = pagos.Pago.ToList().FirstOrDefault();
            if (pagoItem == null) return;

            paymentDataProvider.SAT.NumOperacion = pagoItem.NumOperacion;
            paymentDataProvider.SAT.FechaPago = pagoItem.FechaPago;
            paymentDataProvider.SAT.Monto = pagoItem.Monto;
            paymentDataProvider.SAT.TipoCadenaPago = pagoItem.TipoCadPago;
            paymentDataProvider.SAT.CadPago = pagoItem.CadPago;
            paymentDataProvider.SAT.CertPago = Encoding.ASCII.GetString(pagoItem.CertPago); //////
            paymentDataProvider.SAT.SelloPago = Encoding.ASCII.GetString(pagoItem.SelloPago); //////

            foreach (Profact.TimbraCFDI40.Complementos.Pagos20.PagosPagoDoctoRelacionado doctoItem in pagoItem.DoctoRelacionado.ToList())
            {
                MapPagosPagoDoctoRelacionado(paymentDataProvider, doctoItem);
            }
        }

        private static void MapPagosPagoDoctoRelacionado(PaymentDataProvider paymentDataProvider, PagosPagoDoctoRelacionado doctoItem)
        {
            PaymentDataProvider.InvoicePayments invoicePayment = null;
            string invoicenumber = doctoItem.Folio;
            invoicenumber = GetInvoiceNumberValue(paymentDataProvider, doctoItem, invoicenumber);
            invoicePayment = GetInvoicePaymentValue(paymentDataProvider, doctoItem, invoicenumber);

            if (invoicePayment == null) return;
            invoicePayment.UUID = doctoItem.IdDocumento;
            invoicePayment.CurrencyCode = doctoItem.MonedaDR;
            //invoicePayment.TipoCambio = doctoItem.TipoCambioDR; /////
            //invoicePayment.MetodoPagoCode = doctoItem.MetodoDePagoDR; /////
            invoicePayment.Serie = doctoItem.Serie;
            invoicePayment.Folio = doctoItem.Folio;
            invoicePayment.NumParcialidad = doctoItem.NumParcialidad;
            invoicePayment.ImpSaldoAnt = doctoItem.ImpSaldoAnt;
            invoicePayment.ImpPagado = doctoItem.ImpPagado;
            invoicePayment.ImpSaldoInsoluto = doctoItem.ImpSaldoInsoluto;

        }

        private static PaymentDataProvider.InvoicePayments GetInvoicePaymentValue(PaymentDataProvider paymentDataProvider, PagosPagoDoctoRelacionado doctoItem, string invoicenumber)
        {
            PaymentDataProvider.InvoicePayments invoicePayment;
            if (!string.IsNullOrEmpty(doctoItem.Serie) && doctoItem.Serie != "A")
            {
                return paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber);
            }

            invoicePayment = paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber);
            return invoicePayment == null ? paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber) : invoicePayment;

        }

        private static string GetInvoiceNumberValue(PaymentDataProvider paymentDataProvider, PagosPagoDoctoRelacionado doctoItem, string invoicenumber)
        {
            if (!string.IsNullOrEmpty(doctoItem.Serie) && doctoItem.Serie != "A")
            {
                return invoicenumber = doctoItem.Serie + doctoItem.Folio;
            }
            else if (paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber) == null)
            {
                return invoicenumber = doctoItem.Serie + doctoItem.Folio;
            }

            return invoicenumber;
        }

        private static void MapAdditionalFields(ARPayment currentPayment, PaymentDataProvider paymentDataProvider)
        {
            if (string.IsNullOrEmpty(currentPayment.SATAdditionalFieldsXML)) return;
            SATAdditionalFields additionalFields = LogitudeXmlSerializer.DeserializeObject<SATAdditionalFields>(currentPayment.SATAdditionalFieldsXML);
            paymentDataProvider.SAT.CadenaOriginal = additionalFields.CadenaOriginal;
            MapQRImageField(paymentDataProvider, additionalFields);
        }

        private static void MapQRImageField(PaymentDataProvider paymentDataProvider, SATAdditionalFields additionalFields)
        {
            if (!string.IsNullOrEmpty(additionalFields.QRImage))
            {
                paymentDataProvider.SAT.QRImage = Image.FromStream(new MemoryStream(Convert.FromBase64String(additionalFields.QRImage)));
            }
        }

        private static void MappingBillToCardFields(PaymentDataProvider paymentDataProvider, Card billToCard)
        {
            paymentDataProvider.SAT.SATForeignRFC = (billToCard.SATForeignRFC ?? null);
            paymentDataProvider.ForeignRFC = (billToCard.SATForeignRFC ?? null);
        }

        private static string GetSATTimbreFiscalDigitalValue(string attributeName, XmlElement timbreFiscalDigitalElement)
        {
            string value = "";
            if (timbreFiscalDigitalElement != null && timbreFiscalDigitalElement.Attributes[attributeName] != null)
            {
                value = timbreFiscalDigitalElement.Attributes[attributeName].Value;
            }

            return value;
        }
    }
}