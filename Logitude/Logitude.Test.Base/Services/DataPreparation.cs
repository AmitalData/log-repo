using Logitude.Test.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Services
{
    public class DataPreparation
    {
        public static LocationsVariables GetLocationsVariables()
        {
            return new LocationsVariables {
                PortLHRId = GetPortId("LHR", null),
            };
        }
        public static PartnersVariables GetPartnersVariables()
        {
            return null;
        }

        #region Locations Preparation Variables

        #region Ports
        private static string GetPortId(string code, string countryCode)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
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
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetbyfilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault().Id;
        }
        private static string GetPortIdFromZeroTenant(ApiQueryFilters apiQueryFilters)
        {
            apiQueryFilters.Tenant = 0;
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetTenantImportByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault().Id;
        }

        private static string GetCopiedPortFromTenantZero(string portId)
        {
            ApiResponse<Port> response = APICaller.CallGet<Port>(Urls.CommonDomainGetPortCopyToCurrentTenant(portId), UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #endregion
    }
}
