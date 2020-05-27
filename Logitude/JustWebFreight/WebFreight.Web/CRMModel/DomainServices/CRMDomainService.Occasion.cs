using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OccasionList> GetOccasionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Occasion", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OccasionListQueryService listService = new OccasionListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<OccasionList> myResult = listService.GetList(queryOperations, tenant);

            return myResult;
        }
        
        public int GetOccasionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Occasion", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OccasionListQueryService queryService = new OccasionListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}