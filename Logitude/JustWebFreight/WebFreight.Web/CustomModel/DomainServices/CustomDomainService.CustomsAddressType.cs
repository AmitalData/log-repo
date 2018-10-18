using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsAddressTypePM GetSingleCustomsAddressTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsAddressTypeQuery = new CustomsAddressTypeQueryService(customContext);
            CustomsAddressTypePM CustomsAddressType = customsAddressTypeQuery.GetSingle(code, false, false);
            return CustomsAddressType;
        }

        public CustomsAddressTypeList GetSingleCustomsAddressTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsAddressType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsAddressTypeListQueryService listService = new CustomsAddressTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomsAddressTypeList> GetCustomsAddressTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsAddressType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsAddressTypeListQueryService listService = new CustomsAddressTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsAddressTypeList> GetCustomsAddressTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsAddressType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsAddressTypeListQueryService listService = new CustomsAddressTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsAddressTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsAddressType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsAddressTypeListQueryService queryService = new CustomsAddressTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}