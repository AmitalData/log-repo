using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public ClaimEntityPM GetSingleClaimEntityPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            claimEntityQuery = new ClaimEntityQueryService(customContext);
            ClaimEntityPM ClaimEntity = claimEntityQuery.GetSingle(code, false, false);
            return ClaimEntity;
        }

        public ClaimEntityList GetSingleClaimEntityList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClaimEntity", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ClaimEntityList> GetClaimEntityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ClaimEntityList> GetClaimEntityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetClaimEntityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimEntityListQueryService queryService = new ClaimEntityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}