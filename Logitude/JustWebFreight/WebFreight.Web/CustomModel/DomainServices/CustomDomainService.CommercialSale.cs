using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CommercialSalePM GetSingleCommercialSalePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            commercialSaleQuery = new CommercialSaleQueryService(customContext);
            CommercialSalePM CommercialSale = commercialSaleQuery.GetSingle(id, false, false);
            return CommercialSale;
        }

        public CommercialSaleList GetSingleCommercialSaleList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CommercialSaleListQueryService listService = new CommercialSaleListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CommercialSaleList> GetCommercialSaleLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CommercialSaleListQueryService listService = new CommercialSaleListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CommercialSaleList> GetCommercialSaleFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CommercialSaleListQueryService listService = new CommercialSaleListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCommercialSaleFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CommercialSaleListQueryService queryService = new CommercialSaleListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}