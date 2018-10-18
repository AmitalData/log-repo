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
        public ConstraintProcessTypePM GetSingleConstraintProcessTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            constraintProcessTypeQuery = new ConstraintProcessTypeQueryService(customContext);
            ConstraintProcessTypePM ConstraintProcessType = constraintProcessTypeQuery.GetSingle(id, false, false);
            return ConstraintProcessType;
        }

        public ConstraintProcessTypeList GetSingleConstraintProcessTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.ConstraintProcessType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConstraintProcessTypeListQueryService listService = new ConstraintProcessTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ConstraintProcessTypeList> GetConstraintProcessTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.ConstraintProcessType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintProcessTypeListQueryService listService = new ConstraintProcessTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConstraintProcessTypeList> GetConstraintProcessTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ConstraintProcessType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ConstraintProcessTypeListQueryService listService = new ConstraintProcessTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConstraintProcessTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ConstraintProcessType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintProcessTypeListQueryService queryService = new ConstraintProcessTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}