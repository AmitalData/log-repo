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

            InvoiceComprobanteDetails invoiceComprobanteDetails = SATInterfaceHelper.GetInvoiceComprobanteDetails(entitySATXML);

            if (invoiceComprobanteDetails.ComplementoAny == null) return resultadoConsultaEstatusSAT;

            List<XmlElement> myLXmlComplementos = invoiceComprobanteDetails.ComplementoAny.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return resultadoConsultaEstatusSAT;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            string rfcEmisor = invoiceComprobanteDetails.EmisorRfc.Trim();
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

        public static string GetBillToCountryCode(string billToAddressId, int tenant, ICommonDataContext commonContext)
        {
            AddressRepository addressReposirory = new AddressRepository(commonContext);
            Address billToAddress = null;
            if (!string.IsNullOrEmpty(billToAddressId))
            {
                billToAddress = addressReposirory.GetSingleAddress(billToAddressId, tenant);
            }
            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
            string billToCountryCode = GetBillToCountryCodeByBillToAddress(computingPartnerHelper, billToAddress);
            return billToCountryCode;
        }

        private static string GetBillToCountryCodeByBillToAddress(ComputingPartnerTranslationHelper computingPartnerHelper, Address billToAddress)
        {
            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            return billToCountryCode;
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

        public static decimal GetDecimalWith4DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.0000"));
        }

        public static decimal GetDecimalWith6DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.000000"));
        }

        public static decimal TruncateDecimalWithNDigitsAfterPoint(decimal dNumber, double numberOfDigits)
        {
            decimal multiplier = (decimal)Math.Pow(10, numberOfDigits);
            return Math.Truncate(dNumber * multiplier) / multiplier;
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

        public static int GetSATVersion(string satXML)
        {
            return satXML.Contains("cfd/4/cfdv40.xsd") ? 4 : 3;
        }

        internal static decimal GetDecimalWithMatchCurrencyDigitsAfterPoint(decimal dNumber, string currency)
        {
            switch (currency)
            {
                case "CLF": return decimal.Parse(dNumber.ToString("0.0000"));
                case "BHD":
                case "IQD":
                case "JOD":
                case "KWD":
                case "LYD":
                case "OMR":
                case "TND":
                    return decimal.Parse(dNumber.ToString("0.000"));
                case "BIF":
                case "BYR":
                case "CLP":
                case "DJF":
                case "GNF":
                case "ISK":
                case "JPY":
                case "KMF":
                case "KRW":
                case "PYG":
                case "RWF":
                case "UGX":
                case "UYI":
                case "VND":
                case "VUV":
                case "XAF":
                case "XAG":
                case "XAU":
                case "XBA":
                case "XBB":
                case "XBC":
                case "XBD":
                case "XDR":
                case "XOF":
                case "XPD":
                case "XPF":
                case "XPT":
                case "XSU":
                case "XTS":
                case "XUA":
                case "XXX":
                    return decimal.Parse(dNumber.ToString("0"));
                default: return decimal.Parse(dNumber.ToString("0.00"));
            }
        }
    }
}
