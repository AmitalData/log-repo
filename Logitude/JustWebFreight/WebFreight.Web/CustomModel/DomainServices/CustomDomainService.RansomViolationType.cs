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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public RansomViolationTypePM GetSingleRansomViolationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            ransomViolationTypeQuery = new RansomViolationTypeQueryService(customContext);
            RansomViolationTypePM RansomViolationType = ransomViolationTypeQuery.GetSingle(code, false, false);
            return RansomViolationType;
        }

        public RansomViolationTypeList GetSingleRansomViolationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RansomViolationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            RansomViolationTypeListQueryService listService = new RansomViolationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<RansomViolationTypeList> GetRansomViolationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RansomViolationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RansomViolationTypeListQueryService listService = new RansomViolationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<RansomViolationTypeList> GetRansomViolationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RansomViolationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RansomViolationTypeListQueryService listService = new RansomViolationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetRansomViolationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RansomViolationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RansomViolationTypeListQueryService queryService = new RansomViolationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}