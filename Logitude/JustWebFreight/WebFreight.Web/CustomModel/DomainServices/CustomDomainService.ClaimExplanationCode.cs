using WebFreight.Web.Security;
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

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public ClaimExplanationCodePM GetSingleClaimExplanationCodePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            claimExplanationCodeQuery = new ClaimExplanationCodeQueryService(customContext);
            ClaimExplanationCodePM ClaimExplanationCode = claimExplanationCodeQuery.GetSingle(code, false, false);
            return ClaimExplanationCode;
        }

        public ClaimExplanationCodeList GetSingleClaimExplanationCodeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClaimExplanationCode", "READ", tenant);    

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClaimExplanationCodeListQueryService listService = new ClaimExplanationCodeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ClaimExplanationCodeList> GetClaimExplanationCodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClaimExplanationCode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimExplanationCodeListQueryService listService = new ClaimExplanationCodeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ClaimExplanationCodeList> GetClaimExplanationCodeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClaimExplanationCode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimExplanationCodeListQueryService listService = new ClaimExplanationCodeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetClaimExplanationCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClaimExplanationCode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClaimExplanationCodeListQueryService queryService = new ClaimExplanationCodeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}