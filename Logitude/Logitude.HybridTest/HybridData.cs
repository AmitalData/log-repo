using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public struct HybridData
    {
        public static string ActivityId { get; set; }

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
        #region PortsVars
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
        #endregion
        //Country
        public const string CountryCodeGB = "GB";
        public const string CountryCodeUS = "US";
        public const string StateCodeAK = "AK";
        public static string CountryIdGB { get; set; }
        public static string CountryIdUS { get; set; }
        public static string StateIdAK { get; set; }
        //Customer
        public const string CustomerCodeHCustomer = "HCShipper";
        public const string CustomerContactCodeHCustomer = "Shipper Contract";
        public const string CustomerCodeTestShipperImport1 = "HShipper2";
        public const string CustomerCodeTestConsigneeExport1 = "HConsignee";
        public const string CustomerCodeTestConsigneeImport1 = "HConsignee2";

        public static string CustomerIdHCustomer { get; set; }
        public static string CustomerAddressIdHCustomer { get; set; }

        public static string CustomerIdTestShipperImport1 { get; set; }
        public static string CustomerIdTestConsigneeExport1 { get; set; }
        public static string CustomerIdTestConsigneeImport1 { get; set; }

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
        //Agent
        public const string AgentCodeHAgent = "HAgent";

        #endregion

        #region OtherVars

        public const string ShippingAgentCodeHSAG = "HSAG";
        public const string PortCodeHP = "HFP";
        public const string PackageTypeCodeHPT = "HPT";
        public const string IncotermCodeHI = "HI";
        public const string GlobalZoneCodeHZ = "HZ";
        public const string EventTypeCodeHET = "HET";
        public const string EntityStatusCodeHES = "HES";
        public const string DocumentTypeCodeHDT = "HDT";
        public const string DepartmentCodeHDEP = "HDEP";
        public const string UserCodeHU = "HU";
        public const string BranchCodeHBRA = "HBRA";
        public const string AccountingPartnerCodeHAPartner = "HAPartner";
        public const string ShippingLineCodeHSLN = "HSLN";
        public const string SpecialServicesTypeCodeHSST = "HSST";
        public const string AddressCodeHA = "HA";
        public const string ContactCode = "Hybrid Contact";
        public const string CurrencyCodeHCR = "HCR";
        public const string CountryCodeHC = "HC";
        public const string CityCodeHCity = "HCity";
        public const string CustomAgentCodeHCAgent = "HCAgent";
        public const string QuoteCode = "Hybrid Quote";
        public const string DirectShipmentCode = "Hybrid DShipment";

        public static string FirstOpportunityId { get; set; }
        public static string DocumentTypeIdHDT { get; set; }
        public static string UserIdHU { get; set; }
        public static string AddressIdHA { get; set; }
        public static string ContactId { get; set; }
        public static string CityIdHCity { get; set; }
        public static string DirectShipmentId { get; set; }
        public static string QuoteId { get; set; }


        #endregion
    }
}
