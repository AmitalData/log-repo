using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public OpportunityProductLocationPM GetSingleOpportunityProductLocationPM(string opportunityId, string code, int lineNumber, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            opportunityProductLocationQuery = new OpportunityProductLocationQueryService(crmContext);
            OpportunityProductLocationPM entityPM = opportunityProductLocationQuery.GetSingle(opportunityId, code, lineNumber, true, false);
            return entityPM;
        }

        public OpportunityProductLocationList GetSingleOpportunityProductLocationList(string opportunityId, string code, int lineNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductLocationListQueryService listService = new OpportunityProductLocationListQueryService(crmContext);
            return listService.GetSingle(opportunityId, code, lineNumber);
        }

        public List<OpportunityProductLocationList> GetOpportunityProductLocationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductLocationListQueryService listService = new OpportunityProductLocationListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OpportunityProductLocationList> GetOpportunityProductLocationsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductLocationListQueryService listService = new OpportunityProductLocationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetOpportunityProductLocationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductLocationListQueryService queryService = new OpportunityProductLocationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}