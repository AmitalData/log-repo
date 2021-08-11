using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.PackageTypesPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Test.Base.Services
{
    public static class PackageTypesDataPreparation
    {
        #region Get Package Types Variables

        public static PackageTypesVariables GetVariables()
        {
            return new PackageTypesVariables
            {
                PackageTypeOceanPC1Id = GetPackageTypeId("PC1", "O", true),
                PackageTypeOceanPC2Id = GetPackageTypeId("PC2", "O", true),
                PackageTypeAirPP1Id = GetPackageTypeId("PP1", "A", false),
                PackageTypeAirPP2Id = GetPackageTypeId("PP2", "A", false),

            };
        }

        #region PackageType

        private static string GetPackageTypeId(string code, string transportModeCode, bool isContainer)
        {
            ApiQueryFilters apiQueryFilters = BuildPackageTypesApiQueryFilters(null, code);
            string UserTenantPackageTypeId = GetPackageTypeIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantPackageTypeId))
            {
                UserTenantPackageTypeId = GetCreatedPackageTypeFromTenantZero(code, transportModeCode, isContainer);
            }
            return UserTenantPackageTypeId;
        }

        private static string GetPackageTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<PackageType>> response = APICaller.CallGetByFilters<IEnumerable<PackageType>>(Urls.PackageTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedPackageTypeFromTenantZero(string code, string transportModeCode, bool isContainer)
        {
            PackageType packageType = CreatePackageType(code, transportModeCode, isContainer);
            ApiResponse<PackageType> response = APICaller.CallPost<PackageType>(packageType, Urls.PackageTypesController, UserTenant.Token);
            return response.Data?.Id;

        }

        private static PackageType CreatePackageType(string packageTypeCode, string transportModeCode, bool isContainer)
        {
            PackageType PackageType = new PackageType();
            PackageType.Tenant = UserTenant.Tenant;
            PackageType.Code = packageTypeCode;
            PackageType.EnglishName = isContainer ? "ContainerId" : "PackageId" + packageTypeCode;
            PackageType.PrintAs = packageTypeCode;
            PackageType.IsAir = transportModeCode == "A" ? true : false;
            PackageType.IsOcean = transportModeCode == "O" ? true : false;
            PackageType.IsInland = transportModeCode == "I" ? true : false;
            PackageType.IsContainer = isContainer;
            return PackageType;
        }

        #endregion
        #region Build ApiQueryFilters
        private static ApiQueryFilters BuildPackageTypesApiQueryFilters(string code, string SearchFieldsCode)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Operator("equals")
                .Filter1Value(code)
                .Filter2Name("SearchFields")
                .Filter2Operator("Contains")
                .Filter2Value(SearchFieldsCode)
                .Build();
        }
        #endregion


        #endregion
    }
}
