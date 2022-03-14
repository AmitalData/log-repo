using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;
using Profact.TimbraCFDI40;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATBaseProfact40Service
    {
        public static Profact.TimbraCFDI.ResultadoConsultaEstatusSAT GetSATStatus(int tenant, string entitySATXML)
        {
            Profact.TimbraCFDI.ResultadoConsultaEstatusSAT resultadoConsultaEstatusSAT = null;
            Conector conector = GetProfactConnector(tenant);
            Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Comprobante>(entitySATXML);
            if (comprobante.Complemento.Any == null) return resultadoConsultaEstatusSAT;

            List<XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return resultadoConsultaEstatusSAT;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            string rfcEmisor = comprobante.Emisor.Rfc.Trim();
            string uuID = digitalTi.UUID.Trim();
            resultadoConsultaEstatusSAT = conector.ConsultaEstatusSAT(uuID);


            return resultadoConsultaEstatusSAT;
        }

        private static Conector GetProfactConnector(int tenant)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);

            bool isProduction = satSetting.Token != SATData.TestSATToken;
            Conector conector = new Conector(isProduction);
            conector.EstableceCredenciales(satSetting.Token);

            return conector;
        }

        public static decimal GetDecimalWith3DigitsAfterPointIfZero(decimal dNumber)
        {
            decimal result = decimal.Parse(dNumber.ToString("0.00"));
            if (result == 0 && dNumber != 0)
                result = decimal.Parse(dNumber.ToString("0.000"));
            return result;
        }

        public static decimal GetDecimalWith2DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.00"));
        }

        public static decimal GetDecimalWith6DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.000000"));
        }

        public static string GetBillToAddressZipCode(Address billToAddress, int tenant)
        {
            PostalCodeQuery postalCodeQuery = new PostalCodeQuery(tenant);
            PostalCodePM postalCodePM = postalCodeQuery.GetSinglePM(billToAddress.ZipCode);
            if (postalCodePM != null)
            {
                return billToAddress.ZipCode;
            }

            return null;
        }
    }
}
