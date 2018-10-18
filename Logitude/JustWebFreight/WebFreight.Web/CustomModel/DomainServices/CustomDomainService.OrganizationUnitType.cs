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

        public OrganizationUnitTypePM GetSingleOrganizationUnitTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(customContext);
            OrganizationUnitTypePM OrganizationUnitType = organizationUnitTypeQueryService.GetSingle(id, false, false);
            return OrganizationUnitType;
        }

        public OrganizationUnitTypeList GetSingleOrganizationUnitTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            OrganizationUnitTypeListQueryService listService = new OrganizationUnitTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<OrganizationUnitTypeList> GetOrganizationUnitTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            OrganizationUnitTypeListQueryService listService = new OrganizationUnitTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<OrganizationUnitTypeList> GetOrganizationUnitTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            OrganizationUnitTypeListQueryService listService = new OrganizationUnitTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetOrganizationUnitTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            OrganizationUnitTypeListQueryService queryService = new OrganizationUnitTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }



    }
}