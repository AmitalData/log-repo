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

        public ConstraintStatusPM GetSingleConstraintStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            constraintStatusQuery = new ConstraintStatusQueryService(customContext);
            ConstraintStatusPM ConstraintStatus = constraintStatusQuery.GetSingle(code, false, false);
            return ConstraintStatus;
        }

        public ConstraintStatusList GetSingleConstraintStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConstraintStatusListQueryService listService = new ConstraintStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ConstraintStatusList> GetConstraintStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintStatusListQueryService listService = new ConstraintStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConstraintStatusList> GetConstraintStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintStatusListQueryService listService = new ConstraintStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConstraintStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintStatusListQueryService queryService = new ConstraintStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}