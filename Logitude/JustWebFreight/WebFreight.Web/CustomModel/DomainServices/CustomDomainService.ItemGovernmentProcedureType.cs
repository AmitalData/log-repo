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
        public ItemGovernmentProcedureTypePM GetSingleItemGovernmentProcedureTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            itemGovernmentProcedureTypeQuery = new ItemGovernmentProcedureTypeQueryService(customContext);
            ItemGovernmentProcedureTypePM ItemGovernmentProcedureType = itemGovernmentProcedureTypeQuery.GetSingle(code, false, false);
            return ItemGovernmentProcedureType;
        }

        public ItemGovernmentProcedureTypeList GetSingleItemGovernmentProcedureTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.ItemGovernmentProcedureType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ItemGovernmentProcedureTypeListQueryService listService = new ItemGovernmentProcedureTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ItemGovernmentProcedureTypeList> GetItemGovernmentProcedureTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ItemGovernmentProcedureType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ItemGovernmentProcedureTypeListQueryService listService = new ItemGovernmentProcedureTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ItemGovernmentProcedureTypeList> GetItemGovernmentProcedureTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ItemGovernmentProcedureType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ItemGovernmentProcedureTypeListQueryService listService = new ItemGovernmentProcedureTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetItemGovernmentProcedureTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ItemGovernmentProcedureType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ItemGovernmentProcedureTypeListQueryService queryService = new ItemGovernmentProcedureTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}