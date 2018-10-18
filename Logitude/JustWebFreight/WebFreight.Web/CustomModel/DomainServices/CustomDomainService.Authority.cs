using System.Collections.Generic;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public AuthorityPM GetSingleAuthorityPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            authorityQuery = new AuthorityQueryService(customContext);
            AuthorityPM Authority = authorityQuery.GetSingle(id, false, false);
            return Authority;
        }

        public AuthorityList GetSingleAuthorityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AuthorityListQueryService listService = new AuthorityListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AuthorityList> GetAuthorityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            AuthorityListQueryService listService = new AuthorityListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AuthorityList> GetAuthorityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            AuthorityListQueryService listService = new AuthorityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAuthorityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            AuthorityListQueryService queryService = new AuthorityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}