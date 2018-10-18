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

        public SalesTaxExemptionTypePM GetSingleSalesTaxExemptionTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            salesTaxExemptionTypeQuery = new SalesTaxExemptionTypeQueryService(customContext);
            SalesTaxExemptionTypePM SalesTaxExemptionType = salesTaxExemptionTypeQuery.GetSingle(code, false, false);
            return SalesTaxExemptionType;
        }

        public SalesTaxExemptionTypeList GetSingleSalesTaxExemptionTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SalesTaxExemptionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SalesTaxExemptionTypeListQueryService listService = new SalesTaxExemptionTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<SalesTaxExemptionTypeList> GetSalesTaxExemptionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SalesTaxExemptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SalesTaxExemptionTypeListQueryService listService = new SalesTaxExemptionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SalesTaxExemptionTypeList> GetSalesTaxExemptionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SalesTaxExemptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SalesTaxExemptionTypeListQueryService listService = new SalesTaxExemptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSalesTaxExemptionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SalesTaxExemptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SalesTaxExemptionTypeListQueryService queryService = new SalesTaxExemptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}