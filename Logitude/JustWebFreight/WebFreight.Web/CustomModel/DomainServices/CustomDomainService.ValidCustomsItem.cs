using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public ValidCustomsItemPM GetSingleValidCustomsItemPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            validCustomsItemQuery = new ValidCustomsItemQueryService(customContext);
            ValidCustomsItemPM ValidCustomsItem = validCustomsItemQuery.GetSingle(code, false, false);
            return ValidCustomsItem;
        }

        public ValidCustomsItemList GetSingleValidCustomsItemList(string code, int tenant)
        {
           

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ValidCustomsItemListQueryService listService = new ValidCustomsItemListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ValidCustomsItemList> GetValidCustomsItemLists(int tenant)
        {
           
            customContext = CustomContext.GetContext(tenant);
            ValidCustomsItemListQueryService listService = new ValidCustomsItemListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ValidCustomsItemList> GetValidCustomsItemFilters(byte[] xmlFilters, int tenant)
        {
        
            customContext = CustomContext.GetContext(tenant);
            ValidCustomsItemListQueryService listService = new ValidCustomsItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetValidCustomsItemFiltersCount(byte[] xmlFilters, int tenant)
        {
        
            customContext = CustomContext.GetContext(tenant);
            ValidCustomsItemListQueryService queryService = new ValidCustomsItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}