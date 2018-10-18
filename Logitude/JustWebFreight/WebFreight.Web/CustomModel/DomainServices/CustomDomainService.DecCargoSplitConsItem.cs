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
        public DecCargoSplitConsItemPM GetSingleDecCargoSplitConsItemPM(string DeclarationCargoSplitId, int deccargosplitconslineno, int linenumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            decCargoSplitConsItemQueryService = new DecCargoSplitConsItemQueryService(customContext);
            DecCargoSplitConsItemPM DecCargoSplitConsItem = decCargoSplitConsItemQueryService.GetSingle(DeclarationCargoSplitId, deccargosplitconslineno, linenumber, false, false);
            return DecCargoSplitConsItem;
        }

        public DecCargoSplitConsItemList GetSingleDecCargoSplitConsItemList(string DeclarationCargoSplitId, int deccargosplitconslineno, int linenumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DecCargoSplitConsItem", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsItemListQueryService listService = new DecCargoSplitConsItemListQueryService(customContext);
            return listService.GetSingle(DeclarationCargoSplitId, deccargosplitconslineno, linenumber);
        }

        public List<DecCargoSplitConsItemList> GetDecCargoSplitConsItemLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsItemListQueryService listService = new DecCargoSplitConsItemListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DecCargoSplitConsItemList> GetDecCargoSplitConsItemFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsItemListQueryService listService = new DecCargoSplitConsItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetDecCargoSplitConsItemFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    DecCargoSplitConsItemListQueryService queryService = new DecCargoSplitConsItemListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}