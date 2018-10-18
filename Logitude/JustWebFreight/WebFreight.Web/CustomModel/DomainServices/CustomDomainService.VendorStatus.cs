using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public VendorStatusPM GetSingleVendorStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vendorStatusQuery = new VendorStatusQueryService(customContext);
            VendorStatusPM VendorStatus = vendorStatusQuery.GetSingle(id, false, false);
            return VendorStatus;
        }

        public VendorStatusList GetSingleVendorStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.VendorStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VendorStatusListQueryService listService = new VendorStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VendorStatusList> GetVendorStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.VendorStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorStatusListQueryService listService = new VendorStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VendorStatusList> GetVendorStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VendorStatus", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VendorStatusListQueryService listService = new VendorStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVendorStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VendorStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorStatusListQueryService queryService = new VendorStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}