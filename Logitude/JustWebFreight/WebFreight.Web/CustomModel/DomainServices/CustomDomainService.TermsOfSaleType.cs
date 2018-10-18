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

        public TermsOfSaleTypePM GetSingleTermsOfSaleTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            termsOfSaleTypeQuery = new TermsOfSaleTypeQueryService(customContext);
            TermsOfSaleTypePM TermsOfSaleType = termsOfSaleTypeQuery.GetSingle(code, false, false);
            return TermsOfSaleType;
        }

        public TermsOfSaleTypeList GetSingleTermsOfSaleTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.TermsOfSaleType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TermsOfSaleTypeListQueryService listService = new TermsOfSaleTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<TermsOfSaleTypeList> GetTermsOfSaleTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TermsOfSaleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TermsOfSaleTypeListQueryService listService = new TermsOfSaleTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TermsOfSaleTypeList> GetTermsOfSaleTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TermsOfSaleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TermsOfSaleTypeListQueryService listService = new TermsOfSaleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTermsOfSaleTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TermsOfSaleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TermsOfSaleTypeListQueryService queryService = new TermsOfSaleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}