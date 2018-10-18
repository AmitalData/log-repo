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

        public CurrencyTypePM GetSingleCurrencyTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            currencyTypeQuery = new CurrencyTypeQueryService(customContext);
            CurrencyTypePM CurrencyType = currencyTypeQuery.GetSingle(code, false, false);
            return CurrencyType;
        }

        public CurrencyTypeList GetSingleCurrencyTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CurrencyType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CurrencyTypeListQueryService listService = new CurrencyTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CurrencyTypeList> GetCurrencyTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CurrencyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CurrencyTypeListQueryService listService = new CurrencyTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CurrencyTypeList> GetCurrencyTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CurrencyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CurrencyTypeListQueryService listService = new CurrencyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCurrencyTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CurrencyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CurrencyTypeListQueryService queryService = new CurrencyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}