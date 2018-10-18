using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {


        public AuthorizedSignerPermitPM GetSingleAuthorizedSignerPermitPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            authorizedSignerPermitQuery = new AuthorizedSignerPermitQueryService(customContext);
            AuthorizedSignerPermitPM AuthorizedSignerPermit = authorizedSignerPermitQuery.GetSingle(code, false, false);
            return AuthorizedSignerPermit;
        }

        public AuthorizedSignerPermitList GetSingleAuthorizedSignerPermitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AuthorizedSignerPermit", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AuthorizedSignerPermitListQueryService listService = new AuthorizedSignerPermitListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<AuthorizedSignerPermitList> GetAuthorizedSignerPermitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AuthorizedSignerPermit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AuthorizedSignerPermitListQueryService listService = new AuthorizedSignerPermitListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AuthorizedSignerPermitList> GetAuthorizedSignerPermitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AuthorizedSignerPermit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AuthorizedSignerPermitListQueryService listService = new AuthorizedSignerPermitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAuthorizedSignerPermitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AuthorizedSignerPermit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AuthorizedSignerPermitListQueryService queryService = new AuthorizedSignerPermitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}