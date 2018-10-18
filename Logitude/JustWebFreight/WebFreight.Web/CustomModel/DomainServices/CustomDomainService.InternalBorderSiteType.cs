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

        public InternalBorderSiteTypePM GetSingleInternalBorderSiteTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            internalBorderSiteTypeQuery = new InternalBorderSiteTypeQueryService(customContext);
            InternalBorderSiteTypePM InternalBorderSiteType = internalBorderSiteTypeQuery.GetSingle(code, false, false);
            return InternalBorderSiteType;
        }

        public InternalBorderSiteTypeList GetSingleInternalBorderSiteTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InternalBorderSiteType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            InternalBorderSiteTypeListQueryService listService = new InternalBorderSiteTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<InternalBorderSiteTypeList> GetInternalBorderSiteTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InternalBorderSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InternalBorderSiteTypeListQueryService listService = new InternalBorderSiteTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<InternalBorderSiteTypeList> GetInternalBorderSiteTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InternalBorderSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InternalBorderSiteTypeListQueryService listService = new InternalBorderSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetInternalBorderSiteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InternalBorderSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InternalBorderSiteTypeListQueryService queryService = new InternalBorderSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}