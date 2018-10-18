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
        public DeclarationCargoSplitPM GetSingleDeclarationCargoSplitPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationCargoSplitQueryService = new DeclarationCargoSplitQueryService(customContext);
            DeclarationCargoSplitPM DeclarationCargoSplit = declarationCargoSplitQueryService.GetSingle(code, false, false);
            return DeclarationCargoSplit;
        }

        public DeclarationCargoSplitList GetSingleDeclarationCargoSplitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DeclarationCargoSplit", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<DeclarationCargoSplitList> GetDeclarationCargoSplitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationCargoSplitList> GetDeclarationCargoSplitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetDeclarationCargoSplitFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    DeclarationCargoSplitListQueryService queryService = new DeclarationCargoSplitListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}