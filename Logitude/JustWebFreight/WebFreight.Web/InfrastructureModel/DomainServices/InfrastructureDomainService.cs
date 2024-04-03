using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class InfrastructureDomainService : LogitudeDomainService
    {
        public List<FeatureToggleList> GetFeatureToggleFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FeatureToggle", "READ", tenant);

            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);

            FeatureToggleListQueryService listService = new FeatureToggleListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<FeatureToggleList> listQuery = listService.GetList(queryOperations, tenant);

            return listQuery;
        }

        public int GetFeatureToggleFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("FeatureToggle", "READ", tenant);

            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);

            FeatureToggleListQueryService queryService = new FeatureToggleListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}