using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public struct HybridData
    {
        //Codes
        public const string CountryCode = "HC";
        public const string CityCode = "HCity";
        public const string CustomAgentCode = "HCAgent";
        public const string CustomerCode = "HCustomer";
        public const string AccountingPartnerCode = "HAPartner";
        public const string ShippingLineCode = "HSLN";
        public const string VendorCode = "HVEN";
        public const string TruckerCode = "HTRU";
        public const string FromPortCode = "HFP";
        public const string ToPortCode = "HTP";
        public const string CurrencyCode = "HCR";
        public const string ShippingAgentCode = "HSAG";
        public const string PackageTypeCode = "HPT";
        public const string VesselCode = "HV";
        public const string StateCode = "HS";
        public const string SpecialServicesTypeCode = "HSST";
        public const string DocumentTypeCode = "HDT";
        public const string EventTypeCode = "HET";
        public const string AddressCode = "HA";
        public const string IncotermCode = "HI";
        public const string AirlineCode = "HA";
        public const string CardContactCode = "HCC";
        public const string HouseShipmentCode = "Hybrid HShipment";
        public const string EntityStatusCode = "HES";

        //Id
        public static string CurrencyId { get; set; }
        public static string CityId { get; set; }
        public static string CountryId { get; set; }
        public static string VendorId { get; set; }
        public static string VesselId { get; set; }
        public static string TruckerId { get; set; }
        public static string StateId { get; set; }
        public static string SpecialServicesTypeId { get; set; }
        public static string ShippingLineId { get; set; }
        public static string ShippingAgentId { get; set; }
        public static string EventTypeId { get; set; }
        public static string CustomAgentId { get; set; }
        public static string AirlineId { get; set; }
        public static string AccountingPartnerId { get; set; }
        public static string ToPortId { get; set; }
        public static string AddressId { get; set; }
        public static string IncotermId { get; set; }
        public static string PackageTypeId { get; set; }
        public static string DocumentTypeId { get; set; }
        public static string CustomBanksCardId { get; set; }
        public static string CustomerId { get; set; }
        public static string SomeOpportunityId { get; set; }
        public static string DirectShipmentId { get; set; }
        public static string HouseShipmentId { get; set; }
        public static string ContactId { get; set; }
        public static string CardContactId { get; set; }
        public static string EntityStatusId { get; set; }

        #region ShipmentVars
        //prepare Shipment Vars

        //Currency
        public const string CurrencyCodeEUR = "EUR";
        public static string CurrencyIdEUR { get; set; }
        //Incoterm
        public const string IncotermCodeCIF = "CIF";
        public const string IncotermCodeLDE = "LDE";
        public static string IncotermIdCIF { get; set; }
        public static string IncotermIdLDE { get; set; }
        //ChargeType
        public const string ChargeTypeCodeAFT = "AFT";
        public static string ChargeTypeIdAFT { get; set; }
        public static string ChargeTypeIATACodeId { get; set; }
        public static string ChargeTypeVatTypeId { get; set; }
        //port
        public const string PortCodeLHR = "LHR";
        public const string PortCodeLAS = "LAS";
        public const string PortCodeMIA = "MIA";
        public const string PortCodeAirJFK = "JFK";
        public const string PortCodeOceanSOU = "SOU";
        public const string PortCodeInlandNYC = "NYC";
        public const string PortCodeLON = "LON";
        public const string PortCodeMAN = "MAN";
        public static string PortIdLHR { get; set; }
        public static string CountryIdForPortLHR { get; set; }
        public static string PortIdLAS { get; set; }
        public static string PortIdMIA { get; set; }
        public static string PortIdAirJFK { get; set; }
        public static string CountryIdForPortJFK { get; set; }
        public static string PortIdOceanSOU { get; set; }
        public static string PortIdInlandNYC { get; set; }
        public static string PortIdLON { get; set; }
        public static string PortIdMAN { get; set; }
        //Country
        public const string CountryCodeGB = "GB";
        public const string CountryCodeUS = "US";
        public const string StateCodeAK = "AK";
        public static string CountryIdGB { get; set; }
        public static string CountryIdUS { get; set; }
        public static string StateIdAK { get; set; }
        //Customer

        //AirLine
        public const string AirlineCodeHA = "HA";
        public const string AirlineCodeHL = "HL";
        public static string AirlineIdHA { get; set; }
        public static string AirlineIdHL { get; set; }
        //Shipping Line
        public const string ShippingLineCodeHSL = "HSLN";
        public static string ShippingLineIdHSL { get; set; }
        public const string ShippingLineCodeHSL2 = "HSL2";
        public static string ShippingLineIdHSL2 { get; set; }
        //Trucker
        public const string TruckerCodeHT = "HTRU";
        public const string TruckerCodeHT2 = "HTR2";
        public static string TruckerIdHT { get; set; }
        public static string TruckerIdHT2 { get; set; }
        //LogTenant!

        //MoveType!

        //Vessel
        public const string VesselCodeHV = "HV";
        public static string VesselIdHV { get; set; }
        //PackageType
        public const string PackageTypeCodePC1 = "PC1";
        public const string PackageTypeCodePC2 = "PC2";
        public const string PackageTypeCodePP1 = "PP1";
        public const string PackageTypeCodePP2 = "PP2";
        public static string PackageTypeIdPC1 { get; set; }
        public static string PackageTypeIdPC2 { get; set; }
        public static string PackageTypeIdPP1 { get; set; }
        public static string PackageTypeIdPP2 { get; set; }
        //Vendor
        public const string VendorCodeHVEN = "HVEN";
        public static string VendorIdHVEN { get; set; }
        #endregion

        #region OtherVars
        public const string DepartmentCode = "HDEP";
        public const string UserCode = "HU";
        public const string BranchCode = "HBRA";
        public const string AgentCode = "HAgent";
        public const string GlobalZoneCode = "HZ";
        public const string PortCode = "HFP";
        public const string BankCode = "HB";
        public const string QuoteCode = "Hybrid Quote";
        public const string ContactCode = "Hybrid Contact";
        public const string DirectShipmentCode = "Hybrid DShipment";


        public static string DepartmentId { get; set; }
        public static string UserId { get; set; }
        public static string BranchId { get; set; }
        public static string AgentId { get; set; }
        public static string GlobalZoneId { get; set; }
        public static string PortId { get; set; }
        public static string QuoteId { get; set; }
        public static string ActivityId { get; set; }


        #endregion
    }
}
