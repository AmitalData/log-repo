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

        public VendorTransactionTypePM GetSingleVendorTransactionTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vendorTransactionTypeQuery = new VendorTransactionTypeQueryService(customContext);
            VendorTransactionTypePM VendorTransactionType = vendorTransactionTypeQuery.GetSingle(id, false, false);
            return VendorTransactionType;
        }

        public VendorTransactionTypeList GetSingleVendorTransactionTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.VendorTransactionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VendorTransactionTypeListQueryService listService = new VendorTransactionTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VendorTransactionTypeList> GetVendorTransactionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.VendorTransactionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorTransactionTypeListQueryService listService = new VendorTransactionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VendorTransactionTypeList> GetVendorTransactionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VendorTransactionType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VendorTransactionTypeListQueryService listService = new VendorTransactionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVendorTransactionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VendorTransactionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorTransactionTypeListQueryService queryService = new VendorTransactionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);


        }
    }
}