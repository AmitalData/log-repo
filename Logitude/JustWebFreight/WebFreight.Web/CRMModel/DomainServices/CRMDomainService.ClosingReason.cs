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
using Logitude.BL.Helpers;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public OpportunityClosingReasonPM GetSingleClosingReasonPM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            closingReasonQuery = new OpportunityClosingReasonQueryService(crmContext);
            OpportunityClosingReasonPM reason = closingReasonQuery.GetSingle(id, false, false);
            return reason;
        }

        public OpportunityClosingReasonList GetSingleClosingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityClosingReasonListQueryService listService = new OpportunityClosingReasonListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public void UpdateClosingReasonList(OpportunityClosingReasonList list)
        {

        }

        public List<OpportunityClosingReasonList> GetClosingReasonLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityClosingReasonListQueryService listService = new OpportunityClosingReasonListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<OpportunityClosingReasonList> GetOpportunityClosingReasonFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityClosingReasonListQueryService listService = new OpportunityClosingReasonListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetOpportunityClosingReasonFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityClosingReasonListQueryService queryService = new OpportunityClosingReasonListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertOpportunityClosingReason(OpportunityClosingReasonPM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            OpportunityClosingReasonUpdateService service = new OpportunityClosingReasonUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "OpportunityClosingReason");
        }

        public void UpdateOpportunityClosingReason(OpportunityClosingReasonPM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityClosingReason", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            OpportunityClosingReasonUpdateService service = new OpportunityClosingReasonUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "OpportunityClosingReason");
        }
    }
}