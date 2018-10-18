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
        public ConstraintApprovalDecisionPM GetSingleConstraintApprovalDecisionPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            constraintApprovalDecisionQuery = new ConstraintApprovalDecisionQueryService(customContext);
            ConstraintApprovalDecisionPM ConstraintApprovalDecision = constraintApprovalDecisionQuery.GetSingle(id, false, false);
            return ConstraintApprovalDecision;
        }

        public ConstraintApprovalDecisionList GetSingleConstraintApprovalDecisionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.ConstraintApprovalDecision", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConstraintApprovalDecisionListQueryService listService = new ConstraintApprovalDecisionListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ConstraintApprovalDecisionList> GetConstraintApprovalDecisionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.ConstraintApprovalDecision", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintApprovalDecisionListQueryService listService = new ConstraintApprovalDecisionListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConstraintApprovalDecisionList> GetConstraintApprovalDecisionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ConstraintApprovalDecision", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ConstraintApprovalDecisionListQueryService listService = new ConstraintApprovalDecisionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConstraintApprovalDecisionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ConstraintApprovalDecision", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConstraintApprovalDecisionListQueryService queryService = new ConstraintApprovalDecisionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}