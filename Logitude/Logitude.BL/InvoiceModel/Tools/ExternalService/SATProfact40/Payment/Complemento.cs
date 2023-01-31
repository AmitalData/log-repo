using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Profact.TimbraCFDI40;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class Complemento: SATComprobante
    {
        public static Pagos pagos;
        public static List<Currency> currencies;
        public static ComprobanteComplemento Get(Tenant currentTenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);
            currencies = currencyRepository.GetCurrencies(arPaymentPM.Tenant).ToList();

            pagos = GetNewPagos();

            PagosPago pagoItem = ComplementoPagosPago.Get(currentTenant);

            List<PagosPago> pagosPagoList = new List<PagosPago>
            {
                pagoItem
            };
            pagos.Pago = pagosPagoList.ToArray();
            pagos.Totales.MontoTotalPagos = GetMontoTotalPagos(pagoItem);

            return GetLXmlComplementos();
        }

        private static decimal GetMontoTotalPagos(PagosPago pagoItem)
        {
            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(pagoItem.Monto * pagoItem.TipoCambioP);
        }

        private static Pagos GetNewPagos()
        {
            const string profact4PagosVersion = "2.0";
            PagosTotales pagosTotales = new PagosTotales
            {
                TotalTrasladosBaseIVA16 = 0.0M,
                TotalTrasladosImpuestoIVA16 = 0.0M,
                TotalTrasladosBaseIVA8 = 0.0M,
                TotalTrasladosImpuestoIVA8 = 0.0M,
                TotalTrasladosBaseIVAExento = 0.0M,
                TotalTrasladosBaseIVA0 = 0.0M,
                TotalTrasladosImpuestoIVA0 = 0.0M
            };

            return new Pagos
            {
                Version = profact4PagosVersion,
                Totales = pagosTotales,
            };
        }

        private static ComprobanteComplemento GetLXmlComplementos()
        {
            List<XmlElement> LXmlComplementos = new List<XmlElement>();
            System.Xml.Serialization.XmlSerializerNamespaces nsPagos = new System.Xml.Serialization.XmlSerializerNamespaces();
            nsPagos.Add("pago20", "http://www.sat.gob.mx/Pagos20");
            string xmlPagos = Profact.TimbraCFDI.XMLUtilerias.SerializaObjeto(pagos, typeof(Pagos), nsPagos);
            XmlDocument docNominas = new XmlDocument();
            docNominas.LoadXml(xmlPagos);
            LXmlComplementos.Add(docNominas.DocumentElement);

            return new ComprobanteComplemento
            {
                Any = LXmlComplementos.ToArray<XmlElement>()
            };
        }
    }
}
