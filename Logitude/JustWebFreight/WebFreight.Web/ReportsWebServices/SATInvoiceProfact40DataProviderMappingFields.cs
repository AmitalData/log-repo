using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices
{
    public class SATInvoiceProfact40DataProviderMappingFields
    {

        public static void MapProfact40Fields(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider, Tenant tenantSettings)
        {
            UsoCFDIRepository usoCFDIRepository = new UsoCFDIRepository(currentInvoice.Tenant);
            List<UsoCFDI> allUsoCFDIs = usoCFDIRepository.GetUsoCFDIs().ToList();
            Profact.TimbraCFDI40.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(currentInvoice.SATXML);
            if (comprobante.Complemento.Any == null)
                return;

            List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            invoicedataprovider.SAT.SelloSAT = GetSATTimbreFiscalDigitalValue("SelloSAT", timbreFiscalDigitalElement);
            invoicedataprovider.SAT.NoCertificadoSAT = GetSATTimbreFiscalDigitalValue("NoCertificadoSAT", timbreFiscalDigitalElement);
            invoicedataprovider.SAT.SelloCFD = GetSATTimbreFiscalDigitalValue("SelloCFD", timbreFiscalDigitalElement);
            invoicedataprovider.SAT.UUID = digitalTi.UUID;
            invoicedataprovider.SAT.FechaTimbardo = digitalTi.FechaTimbrado;
            invoicedataprovider.SAT.NoCertificado = comprobante.NoCertificado;

            string invtotal = invoicedataprovider.TotalInvoiceCurr.ToString();
            string fe = invoicedataprovider.SAT.SelloCFD.Substring(invoicedataprovider.SAT.SelloCFD.Length - 8, 8);

            invoicedataprovider.SAT.QR = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?" + "&id=" + digitalTi.UUID + "&re=" + tenantSettings.VatNumber + "&rr=" + invoicedataprovider.BillToVatNumber + "&tt=" + invtotal
                + "&fe=" + fe;

            MapBillToAddressDetails(currentInvoice, invoicedataprovider);
            invoicedataprovider.SAT.FormadePago = comprobante.FormaPago;
            MapRegimenFiscal(invoicedataprovider, comprobante);
            MapTipoDeComprobante(invoicedataprovider, comprobante);
            invoicedataprovider.SAT.LugardeExpedicion = comprobante.LugarExpedicion;
            MapMetodoPago(invoicedataprovider, comprobante);
            MapUsoCFDI(invoicedataprovider, allUsoCFDIs, comprobante);
            MapRegimenFiscalReceptor(currentInvoice, invoicedataprovider, comprobante);
            MapCadenaOriginal(currentInvoice, invoicedataprovider);
            MapCFDIRelacionadoDetails(invoicedataprovider, comprobante);
            MapBillToCardDetails(currentInvoice, invoicedataprovider, comprobante);
        }

        private static void MapBillToCardDetails(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            invoicedataprovider.SAT.BillToSATName = comprobante.Receptor?.Nombre;
            invoicedataprovider.SAT.SATForeignRFC = GetBillToSATForeignRFC(currentInvoice.BillToId, currentInvoice.Tenant);
        }

        private static string GetBillToSATForeignRFC(string billToId, int tenant)
        {
            CardRepository cardReposirory = new CardRepository(tenant);
            Card billToCard = cardReposirory.GetSingleCard(billToId, tenant);

            if (billToCard == null) return "";

            return billToCard.SATForeignRFC;
        }

        private static void MapRegimenFiscalReceptor(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            invoicedataprovider.SAT.RegimenFiscalReceptorCode = comprobante.Receptor?.RegimenFiscalReceptor;
            invoicedataprovider.SAT.RegimenFiscalReceptor = GetRegimenFiscalReceptorName(comprobante.Receptor.RegimenFiscalReceptor, currentInvoice.Tenant);
        }

        private static void MapBillToAddressDetails(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider)
        {
            if (string.IsNullOrEmpty(currentInvoice.BillToAddressId)) return;

            AddressRepository addressReposirory = new AddressRepository(currentInvoice.Tenant);
            Address billToAddress = addressReposirory.GetSingleAddress(currentInvoice.BillToAddressId, currentInvoice.Tenant);

            if (billToAddress == null) return;

            invoicedataprovider.SAT.BillToCity = billToAddress.City;
            invoicedataprovider.SAT.BillToCountry = billToAddress.Country?.EnglishName;
            invoicedataprovider.SAT.BillToPostalCode = billToAddress.ZipCode;
        }

        private static void MapRegimenFiscal(InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            if (comprobante.Emisor.RegimenFiscal != null && comprobante.Emisor.RegimenFiscal.Length > 0)
            {
                invoicedataprovider.SAT.RegimenFiscal = comprobante.Emisor.RegimenFiscal;
            }
        }

        private static void MapMetodoPago(InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            if (!string.IsNullOrEmpty(comprobante.MetodoPago))
            {
                invoicedataprovider.SAT.MetodoPago = (comprobante.MetodoPago == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
            }
        }

        private static void MapTipoDeComprobante(InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            if (comprobante.TipoDeComprobante == "E")
            {
                invoicedataprovider.SAT.TipoDeComprobante = "Egreso";
            }
            else
            {
                invoicedataprovider.SAT.TipoDeComprobante = "Ingreso";
            }
        }

        private static void MapCadenaOriginal(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider)
        {
            if (string.IsNullOrEmpty(currentInvoice.SATAdditionalFieldsXML))
            {
                return;
            }

            SATAdditionalFields additionalFields = LogitudeXmlSerializer.DeserializeObject<SATAdditionalFields>(currentInvoice.SATAdditionalFieldsXML);
            invoicedataprovider.SAT.CadenaOriginal = additionalFields.CadenaOriginal;
        }

        private static void MapUsoCFDI(InvoiceDataProvider invoicedataprovider, List<UsoCFDI> allUsoCFDIs, Comprobante comprobante)
        {
            UsoCFDI usoCFDI = allUsoCFDIs.FirstOrDefault(f => f.Code == comprobante.Receptor.UsoCFDI);
            if (usoCFDI != null)
            {
                invoicedataprovider.SAT.usoCFDI = usoCFDI.Code + " " + usoCFDI.Name;
            }
        }

        public static string GetRegimenFiscalReceptorName(string regimenFiscalReceptorCode, int tenant)
        {
            RegimenFiscalRepository regimenFiscalRepository = new RegimenFiscalRepository(tenant);
            RegimenFiscal regimenFiscalReceptor = regimenFiscalRepository.GetRegimenFiscals().Where(reg => reg.Code == regimenFiscalReceptorCode).FirstOrDefault();
            if (regimenFiscalReceptor != null) {
                return regimenFiscalReceptor.Name; 
            }
            return null;
        }

        private static void MapCFDIRelacionadoDetails(InvoiceDataProvider invoicedataprovider, Comprobante comprobante)
        {
            if (comprobante.CfdiRelacionados == null) return;
            if (comprobante.CfdiRelacionados.Length == 0) return;

            invoicedataprovider.SAT.TipoRelacion = comprobante.CfdiRelacionados[0].TipoRelacion;
            if (invoicedataprovider.SAT.TipoRelacion == "01")
            {
                invoicedataprovider.SAT.TipoRelacion = "01 Nota de crédito de los documentos relacionados";
            }
            else if (invoicedataprovider.SAT.TipoRelacion == "02")
            {
                invoicedataprovider.SAT.TipoRelacion = "02 Nota de débito de los documentos relacionados";
            }
            else if (invoicedataprovider.SAT.TipoRelacion == "04")
            {
                invoicedataprovider.SAT.TipoRelacion = "04 Sustitución de los CFDI previos";
            }

            if (comprobante.CfdiRelacionados[0].CfdiRelacionado != null && comprobante.CfdiRelacionados[0].CfdiRelacionado.Length != 0)
            {
                invoicedataprovider.SAT.CFDIRelacionado = comprobante.CfdiRelacionados[0].CfdiRelacionado[0].UUID;
            }
        }

        private static string GetSATTimbreFiscalDigitalValue(string attributeName, System.Xml.XmlElement timbreFiscalDigitalElement)
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