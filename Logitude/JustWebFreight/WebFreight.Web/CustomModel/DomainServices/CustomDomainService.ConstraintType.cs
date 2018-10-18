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

        public ConstraintTypePM GetSingleConstraintTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            constraintTypeQuery = new ConstraintTypeQueryService(customContext);
            ConstraintTypePM ConstraintType = constraintTypeQuery.GetSingle(code, false, false);
            return ConstraintType;
        }

        public ConstraintTypeList GetSingleConstraintTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConstraintTypeListQueryService listService = new ConstraintTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ConstraintTypeList> GetConstraintTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintTypeListQueryService listService = new ConstraintTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConstraintTypeList> GetConstraintTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintTypeListQueryService listService = new ConstraintTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConstraintTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ConstraintType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintTypeListQueryService queryService = new ConstraintTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}