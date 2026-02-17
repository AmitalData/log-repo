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
        public OpportunityTypePM GetSingleOpportunityTypePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            opportunityTypeQuery = new OpportunityTypeQueryService(crmContext);
            OpportunityTypePM OpportunityType = opportunityTypeQuery.GetSingle(id, false, false);
            return OpportunityType;
        }

        public OpportunityTypeList GetSingleOpportunityTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            crmContext = CRMContext.GetContext(tenant);
            OpportunityTypeListQueryService listService = new OpportunityTypeListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<OpportunityTypeList> GetOpportunityTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            OpportunityTypeListQueryService listService = new OpportunityTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<OpportunityTypeList> GetOpportunityTypesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            OpportunityTypeListQueryService listService = new OpportunityTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetOpportunityTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            OpportunityTypeListQueryService queryService = new OpportunityTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertOpportunityType(OpportunityTypePM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityType", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            OpportunityTypeUpdateService service = new OpportunityTypeUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateOpportunityType(OpportunityTypePM entityPm)
        {
            SecurityUtility.CheckContactFeature("OpportunityType", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            OpportunityTypeUpdateService service = new OpportunityTypeUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }
    }
}