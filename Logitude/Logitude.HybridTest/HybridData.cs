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
        public const string GlobalZoneCode = "HZ";
        public const string CountryCode = "HC";
        public const string CityCode = "HCity";
        public const string AgentCode = "HAgent";
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
        public const string DepartmentCode = "HDEP";
        public const string BranchCode = "HBRA"; 
        public const string PackageTypeCode = "HPT";
        public const string ContactCode = "Hybrid Contact"; 
        public const string VesselCode = "HV"; 
        public const string StateCode = "HS";
        public const string SpecialServicesTypeCode = "HSST"; 
        public const string DocumentTypeCode = "HDT";
        public const string UserCode = "HU";
        public const string EventTypeCode = "HET";
        public const string AddressCode = "HA";
        public const string BankCode = "HB";
        public const string IncotermCode = "HI";
        public const string AirlineCode = "HA";

        //Id
        public static string CurrencyId { get; set; }
        public static string CityId { get; set; }
        public static string CountryId { get; set; }
        public static string GlobalZoneId { get; set; }
        public static string VendorId { get; set; }
        public static string VesselId { get; set; }
        public static string TruckerId { get; set; }
        public static string StateId { get; set; }
        public static string SpecialServicesTypeId { get; set; }
        public static string ShippingLineId { get; set; }
        public static string ShippingAgentId { get; set; }
        public static string DepartmentId { get; set; }
        public static string EventTypeId { get; set; }
        public static string CustomAgentId { get; set; }
        public static string BranchId { get; set; }
        public static string AirlineId { get; set; }
        public static string AccountingPartnerId { get; set; }
        public static string AgentId { get; set; }
        public static string UserId { get; set; }
        public static string FromPortId { get; set; }
        public static string ToPortId { get; set; }
    }
}
