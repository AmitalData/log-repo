using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
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
        public ModificationAndDiscountTypePM GetSingleModificationAndDiscountTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            modificationAndDiscountTypeQuery = new ModificationAndDiscountTypeQueryService(customContext);
            ModificationAndDiscountTypePM ModificationAndDiscountType = modificationAndDiscountTypeQuery.GetSingle(code, false, false);
            return ModificationAndDiscountType;
        }

        public ModificationAndDiscountTypeList GetSingleModificationAndDiscountTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ModificationAndDiscountTypeListQueryService listService = new ModificationAndDiscountTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ModificationAndDiscountTypeList> GetModificationAndDiscountTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ModificationAndDiscountTypeListQueryService listService = new ModificationAndDiscountTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ModificationAndDiscountTypeList> GetModificationAndDiscountTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ModificationAndDiscountTypeListQueryService listService = new ModificationAndDiscountTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetModificationAndDiscountTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ModificationAndDiscountTypeListQueryService queryService = new ModificationAndDiscountTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}