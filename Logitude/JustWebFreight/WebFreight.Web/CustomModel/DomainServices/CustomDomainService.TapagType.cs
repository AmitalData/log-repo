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

        public TapagTypePM GetSingleTapagTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tapagTypeQuery = new TapagTypeQueryService(customContext);
            TapagTypePM TapagType = tapagTypeQuery.GetSingle(code, false, false);
            return TapagType;
        }

        public TapagTypeList GetSingleTapagTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TapagType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TapagTypeListQueryService listService = new TapagTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<TapagTypeList> GetTapagTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TapagType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagTypeListQueryService listService = new TapagTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TapagTypeList> GetTapagTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TapagType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagTypeListQueryService listService = new TapagTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTapagTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TapagType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagTypeListQueryService queryService = new TapagTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}