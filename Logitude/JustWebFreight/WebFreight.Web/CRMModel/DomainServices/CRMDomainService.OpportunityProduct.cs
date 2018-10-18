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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public OpportunityProductPM GetSingleOpportunityProductPM(string opportunityId, string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            opportunityProductQuery = new OpportunityProductQueryService(crmContext);
            OpportunityProductPM entityPM = opportunityProductQuery.GetSingle(opportunityId, code, true, false);
            return entityPM;
        }

        public OpportunityProductList GetSingleOpportunityProductList(string opportunityId, string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductListQueryService listService = new OpportunityProductListQueryService(crmContext);
            return listService.GetSingle(opportunityId, code);
        }

        public List<OpportunityProductList> GetOpportunityProductLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductListQueryService listService = new OpportunityProductListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OpportunityProductList> GetOpportunityProductsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductListQueryService listService = new OpportunityProductListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetOpportunityProductFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityProductListQueryService queryService = new OpportunityProductListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }


    }
}