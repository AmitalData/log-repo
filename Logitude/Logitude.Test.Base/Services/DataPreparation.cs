using Logitude.Test.Base.Models;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Test.Base.Services
{
    public class DataPreparation
    {
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

        public static PartnersVariables GetPartnersVariables()
        {
            return null;
        }

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
    }
}
