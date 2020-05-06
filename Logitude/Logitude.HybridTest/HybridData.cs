using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public struct HybridData
    {
        #region ShipmentVars
        //Currency
        public const string CurrencyCodeEUR = "EUR";
        //Incoterm
        public const string IncotermCodeCIF = "CIF";
        public const string IncotermCodeLDE = "LDE";
        //ChargeType
        public const string ChargeTypeCodeAFT = "AFT";
        //Vessel
        public const string VesselCodeHV = "HV";
        //Vendor
        public const string VendorCodeHVEN = "HVEN";
        //Agent
        public const string AgentCodeHAgent = "HAgent";
        #endregion

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
        public static string PortIdLON { get; set; }
        #endregion

        #region CountriesVars
        //Country
        public const string CountryCodeGB = "GB";
        public const string CountryCodeUS = "US";
        public const string StateCodeAK = "AK";
        #endregion

        #region CustomersVars
        //Customer
        public const string CustomerCodeHCustomer = "HCShipper";
        public const string CustomerContactCodeHCustomer = "Shipper Contract";
        public const string CustomerCodeTestShipperImport1 = "HShipper2";
        public const string CustomerCodeTestConsigneeExport1 = "HConsignee";
        public const string CustomerCodeTestConsigneeImport1 = "HConsignee2";

        public static string CustomerIdHCustomer { get; set; }
        #endregion

        #region AirLinesVars
        //AirLine
        public const string AirlineCodeHA = "HA";
        public const string AirlineCodeHL = "HL";
        #endregion

        #region ShippingLinesVars
        //Shipping Line
        public const string ShippingLineCodeHSL = "HSLN";
        public const string ShippingLineCodeHSL2 = "HSL2";
        #endregion

        #region TruckersVars
        //Trucker
        public const string TruckerCodeHT = "HTRU";
        public const string TruckerCodeHT2 = "HTR2";
        #endregion

        #region PackageTypesVars
        //PackageType
        public const string PackageTypeCodePC1 = "PC1";
        public const string PackageTypeCodePC2 = "PC2";
        public const string PackageTypeCodePP1 = "PP1";
        public const string PackageTypeCodePP2 = "PP2";
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
        public const string HouseShipmentCode = "Hybrid HShipment";

        public static string FirstOpportunityId { get; set; }
        public static string AddressIdHA { get; set; }
        public static string CityIdHCity { get; set; }
        public static string DirectShipmentId { get; set; }

        ///AC
        public static string ActivityId { get; set; }

        #endregion
    }
}
