using Profact.TimbraCFDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.SAT
{
    public class SATBaseService
    {
        public static bool IsProductionConector(string token)
        {
            return token != SATData.TestToken;
        }

        public static Profact.TimbraCFDI40.Conector ConnectToSAT(string token)
        {
            bool isProductionConector = IsProductionConector(token);
            Profact.TimbraCFDI40.Conector satConector = new Profact.TimbraCFDI40.Conector(isProductionConector);
            satConector.EstableceCredenciales(token);

            return satConector;
        }

        public static System.Xml.XmlElement GetSATXmlElement(System.Xml.XmlElement[] satXmlElements)
        {
            const string sATXmlElementName = "tfd:TimbreFiscalDigital";
            return satXmlElements.ToList().Where(el => el.Name == sATXmlElementName).FirstOrDefault();
        }

        public static Profact.TimbraCFDI.TimbreFiscalDigital GetTaxStampDigital(string taxStampDigital)
        {
            return Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(taxStampDigital);
        }

        public static bool IsWaitingToCancelledFromSAT(ResultadoCancelacion resultadoCancelacion)
        {
            string transferError = resultadoCancelacion.Descripcion;
            if (transferError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") return true;
            if (transferError == "El comprobante será cancelado") return true;
            if (transferError.Contains("Comprobante ya está en proceso de cancelación")) return true;

            return false;
        }
    }
}
