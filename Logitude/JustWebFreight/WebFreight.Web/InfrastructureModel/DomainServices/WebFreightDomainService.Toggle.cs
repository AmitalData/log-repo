using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
	public partial class WebFreightDomainService
	{
        public List<ToggleList> GetTogglesTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(tenant);
            ToggleListQueryService listService = new ToggleListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public List<ToggleList> GetTogglesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(tenant);
            ToggleListQueryService listService = new ToggleListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetToggleFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(tenant);

            ToggleListQueryService queryService = new ToggleListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}