using Logitude.Server.Tools;
using Profact.TimbraCFDI40;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class CfdiRelacionados : SATComprobante
    {
        public static ComprobanteCfdiRelacionados[] Get()
        {
            string satXML = arPaymentPM.SATXML;

            ComprobanteCfdiRelacionados[] comprobanteCfdiRelacionados = null;
            if (string.IsNullOrEmpty(satXML)) return comprobanteCfdiRelacionados;
            XmlElement[] paymentComprobanteComplementoAny = GetPaymentComprobanteComplementoAny(satXML);

            if (paymentComprobanteComplementoAny == null) return comprobanteCfdiRelacionados;

            List<XmlElement> myLXmlComplementos = paymentComprobanteComplementoAny.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return comprobanteCfdiRelacionados;

            const string relationShipTypeCode = "04";
            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            comprobanteCfdiRelacionados = new ComprobanteCfdiRelacionados[1];
            comprobanteCfdiRelacionados[0] = new ComprobanteCfdiRelacionados
            {
                TipoRelacion = relationShipTypeCode,
                CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>() { new ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = digitalTi.UUID } }.ToArray()
            };

            return comprobanteCfdiRelacionados;
        }

        private static XmlElement[] GetPaymentComprobanteComplementoAny(string satXML)
        {
            XmlElement[] paymentComprobanteComplementoAny;
            try
            {
                paymentComprobanteComplementoAny = LogitudeXmlSerializer.DeserializeObject<Comprobante>(satXML).Complemento?.Any;
            }
            catch (Exception ex)
            {
                paymentComprobanteComplementoAny = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(satXML).Complemento?.Any;
            }

            return paymentComprobanteComplementoAny;
        }
    }
}
