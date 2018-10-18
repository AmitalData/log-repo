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
        public DecCargoSplitConPM GetSingleDecCargoSplitConPM(string DeclarationCargoSplitId, int linenumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            decCargoSplitConQueryService = new DecCargoSplitConQueryService(customContext);
            DecCargoSplitConPM DecCargoSplitCon = decCargoSplitConQueryService.GetSingle(DeclarationCargoSplitId, linenumber, false, false);
            return DecCargoSplitCon;
        }

        public DecCargoSplitConList GetSingleDecCargoSplitConList(string DeclarationCargoSplitId, int linenumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DecCargoSplitCon", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConListQueryService listService = new DecCargoSplitConListQueryService(customContext);
            return listService.GetSingle(DeclarationCargoSplitId,linenumber);
        }

        public List<DecCargoSplitConList> GetDecCargoSplitConLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConListQueryService listService = new DecCargoSplitConListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DecCargoSplitConList> GetDecCargoSplitConFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConListQueryService listService = new DecCargoSplitConListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetDecCargoSplitConFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    DecCargoSplitConListQueryService queryService = new DecCargoSplitConListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}