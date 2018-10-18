using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public OpportunityStagePM GetSingleOpportunityStagePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            opportunityStageQuery = new OpportunityStageQueryService(crmContext);
            OpportunityStagePM opportunityStage = opportunityStageQuery.GetSingle(id, false, false);
            return opportunityStage;
        }

        public OpportunityStageList GetSingleOpportunityStageList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityStage", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityStageListQueryService listService = new OpportunityStageListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<OpportunityStageList> GetOpportunityStageLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityStage", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityStageListQueryService listService = new OpportunityStageListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<OpportunityStageList> GetOpportunityStagesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityStage", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityStageListQueryService listService = new OpportunityStageListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetOpportunityStageFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityStage", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityStageListQueryService queryService = new OpportunityStageListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertOpportunityStage(OpportunityStagePM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityStage", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            OpportunityStageUpdateService service = new OpportunityStageUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateOpportunityStage(OpportunityStagePM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityStage", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            OpportunityStageUpdateService service = new OpportunityStageUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }
    }
}