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
        public DecCargoSplitCargoIdentifierPM GetSingleDecCargoSplitCargoIdentifierPM(string DeclarationCargoSplitId, int linenumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            decCargoSplitCargoIdentifierQueryService = new DecCargoSplitCargoIdentifierQueryService(customContext);
            DecCargoSplitCargoIdentifierPM DecCargoSplitCargoIdentifier = decCargoSplitCargoIdentifierQueryService.GetSingle(DeclarationCargoSplitId, linenumber, false, false);
            return DecCargoSplitCargoIdentifier;
        }

        public DecCargoSplitCargoIdentifierList GetSingleDecCargoSplitCargoIdentifierList(string DeclarationCargoSplitId, int linenumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DecCargoSplitCargoIdentifier", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitCargoIdentifierListQueryService listService = new DecCargoSplitCargoIdentifierListQueryService(customContext);
            return listService.GetSingle(DeclarationCargoSplitId,linenumber);
        }

        public List<DecCargoSplitCargoIdentifierList> GetDecCargoSplitCargoIdentifierLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitCargoIdentifierListQueryService listService = new DecCargoSplitCargoIdentifierListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DecCargoSplitCargoIdentifierList> GetDecCargoSplitCargoIdentifierFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitCargoIdentifierListQueryService listService = new DecCargoSplitCargoIdentifierListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetDecCargoSplitCargoIdentifierFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    DecCargoSplitCargoIdentifierListQueryService queryService = new DecCargoSplitCargoIdentifierListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}