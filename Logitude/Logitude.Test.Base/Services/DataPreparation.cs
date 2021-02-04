using Logitude.Test.Base.Models;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Test.Base.Services
{
    public static class DataPreparation
    {
        #region Get Locations Variables

        public static LocationsVariables GetLocationsVariables()
        {
            return new LocationsVariables {
                PortLHRId = GetPortId("LHR", null),
                PortLASDomesticId = GetPortId("LAS", "us"),
                PortMIADomesticId = GetPortId("MIA", "us"),
                PortAirJFKId = GetPortId("JFK", "us"),
                PortOceanSOUId = GetPortId("USSOU", null, true),
                PortInlandNYCId = GetPortId("NYC", null),
                PortLONId = GetPortId("LON", null),
                PortMANId = GetPortId("MAN", null),
                StateAKId = GetStateId("AK"),
                CountryUSId = GetCountryId("US"),
                CountryGBId = GetCountryId("GB")
            };
        }

        #endregion

        #region Get Partners Variables

        public static PartnersVariables GetPartnersVariables()
        {
            return new PartnersVariables
            {
                VendorId = GetPartnerId("VD", "TestVendor", null),
                AgentId = GetPartnerId("AG", "TestAgent", null),
                CustomerId = GetPartnerId("CS", "TestCustomer", null),
                PotentialCustomerId = GetPartnerId("PO", "TestPotentialCustomer", null),
                CustomAgentId = GetPartnerId("CG", "TestCustomAgent", null),
                ShippingAgentId = GetPartnerId("SG", "TestShippingAgent", null),
                TruckerTLONId = GetPartnerId("TR", "TestTLONTrucker", "TLON"),
                TruckerTNYCId = GetPartnerId("TR", "TestTNYCTrucker", "TNYC"),
                ShipperExportId = GetPartnerId("CS", "TestShipperExport", null),
                AirlineAAId = GetPartnerId("AL", "TestAAAirline", "AA", true),
                AirlineBAId = GetPartnerId("AL", "TestBAAirline", "BA", true),
                WarehouseId = GetPartnerId("WH", "TestWarehouse", "TSWHE"),
                ShippingLineMAEUId = GetPartnerId("SL", "TestMAEUShippingLine", "MAEU", true),
                ShippingLineMSCUId = GetPartnerId("SL", "TestMSCUShippingLine", "MSCU", true)
            };
        }

        #endregion

        #region Locations Preparation Variables

        #region Ports
        private static string GetPortId(string code, string countryCode, bool isCombined = false)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, countryCode);
            if (isCombined)
            {
                apiQueryFilters.Filter1Name = "CombinedCode";
            }
            string UserTenantPortId = GetPortIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantPortId))
            {
                string ZeroTenantPortId = GetPortIdFromZeroTenant(apiQueryFilters);
                UserTenantPortId = GetCopiedPortFromTenantZero(ZeroTenantPortId);
            }
            
            return UserTenantPortId;
        }

        private static string GetPortIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetPortIdFromZeroTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetTenantImportByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCopiedPortFromTenantZero(string portId)
        {
            ApiResponse<Port> response = APICaller.CallGet<Port>(Urls.CommonDomainGetPortCopyToCurrentTenant(portId), UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Countries
        private static string GetCountryId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            return GetCountryIdFromUserTenant(apiQueryFilters);
        }

        private static string GetCountryIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Country>> response = APICaller.CallGetByFilters<IEnumerable<Country>>(Urls.CountryViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault().Id;
        }
        #endregion

        #region States
        private static string GetStateId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            string UserTenantStateId =  GetStateIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantStateId))
            {
                UserTenantStateId = CreateStateForUserTenant(code);
            }

            return UserTenantStateId;
        }

        private static string GetStateIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.StateViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string CreateStateForUserTenant(string code)
        {
            State state = new State
            {
                Tenant = UserTenant.Tenant,
                Code = code,
                EnglishName = code  +" State",
                CountryId = GetCountryId("US")
            };

            ApiResponse<State> response = APICaller.CallPost<State>(state, Urls.StatesController, UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Build ApiQueryFilters
        private static ApiQueryFilters BuildApiQueryFilters(string code, string countryCode)
        {
            return new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "Code",
                Filter1Operator = "equals",
                Filter1Value = code,
                Filter2Name = "CountryCode",
                Filter2Operator = "contains",
                Filter2Value = countryCode
            };
        }
        #endregion

        #endregion

        #region Partners Data Preparation

        private static string GetPartnerId(string partnerTypeCode, string partnerName, string partnerCode, bool copyFromTenantZero = false)
        {
            string userTenantPartnerId = GetPartnerIdFromTenant(partnerTypeCode, partnerName, partnerCode, false);
            if (string.IsNullOrEmpty(userTenantPartnerId))
            {
                if (!copyFromTenantZero)
                {
                    userTenantPartnerId = CreatePartnerForUserTenant(partnerTypeCode, partnerName, partnerCode);
                }
                else
                {
                    string tenantZeroPartnerId = GetPartnerIdFromTenant(partnerTypeCode, partnerName, partnerCode, true);
                    userTenantPartnerId = GetCopiedPartnerFromTenantZero(tenantZeroPartnerId);
                }
            }

            return userTenantPartnerId;
        }

        private static string GetPartnerIdFromTenant(string partnerTypeCode, string partnerName, string partnerCode, bool getFromTenantZero)
        {
            string requestUrl = getFromTenantZero ? Urls.CarrierViewsGetTenantImportByFilters : GetUrlForUserTenantPartnerRequest(partnerTypeCode);

            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = string.IsNullOrEmpty(partnerCode) ? "EnglishName" : "Code",
                Filter1Operator = "equals",
                Filter1Value = string.IsNullOrEmpty(partnerCode) ? partnerName : partnerCode,
            };

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(requestUrl, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        private static string CreatePartnerForUserTenant(string partnerTypeCode, string partnerName, string partnerCode)
        {
            Partner partner = BuildPartner(partnerTypeCode, partnerName, partnerCode);
            ApiResponse<Partner> response = APICaller.CallPost<Partner>(partner, Urls.PartnersDomainController, UserTenant.Token);//
            return response.Data?.PartnerId;
        }

        private static string GetCopiedPartnerFromTenantZero(string tenantZeroPartnerId)
        {
            string requestUrl = Urls.PartnersDomainGetCarrierCopyToCurrentTenant(tenantZeroPartnerId);
            ApiResponse<dynamic> response = APICaller.CallGet<dynamic>(requestUrl, UserTenant.Token);
            return response.Data?["Id"];
        }

        private static Partner BuildPartner(string partnerTypeCode, string partnerName, string partnerCode)
        {
            Partner partner = new Partner
            {
                Tenant = UserTenant.Tenant,
                PartnerTypeId = partnerTypeCode
            };

            PartnerInformation partnerInformation = new PartnerInformation
            {
                Tenant = UserTenant.Tenant,
                EnglishName = partnerName,
                LocalName = partnerName,
                PartnerTypeId = partnerTypeCode,
                Code = partnerCode
            };

            switch (partnerTypeCode)
            {
                case "VD":
                    partner.Vendor = partnerInformation;
                    return partner;
                case "AG":
                    partner.Agent = partnerInformation;
                    return partner;
                case "CS":
                case "PO":
                    partner.Customer = partnerInformation;
                    return partner;
                case "CG":
                    partner.CustomAgent = partnerInformation;
                    return partner;
                case "SG":
                    partner.ShippingAgent = partnerInformation;
                    return partner;
                case "TR":
                    partner.Trucker = partnerInformation;
                    partner.Trucker.CarrierTypeId = partnerTypeCode;
                    return partner;
                case "WH":
                    partner.Warehouse = partnerInformation;
                    return partner;
                default:
                    return null;
            }
        }

        private static string GetUrlForUserTenantPartnerRequest(string partnerTypeCode)
        {
            switch (partnerTypeCode)
            {
                case "VD":
                    return Urls.VendorViewsGetByFilters;
                case "AG":
                    return Urls.AgentViewsGetByFilters;
                case "CS":
                case "PO":
                    return Urls.CustomerViewsGetByFilters;
                case "CG":
                    return Urls.CustomAgentViewsGetByFilters;
                case "SG":
                    return Urls.ShippingAgentViewsGetByFilters;
                case "TR":
                    return Urls.TruckerViewsGetByFilters;
                case "AL":
                    return Urls.AirlineViewsGetByFilters;
                case "WH":
                    return Urls.WarehouseViewsGetByFilters;
                case "SL":
                    return Urls.ShippingLineViewsGetByFilters;
                default:
                    return null;
            }
        }

        #endregion
    }
}