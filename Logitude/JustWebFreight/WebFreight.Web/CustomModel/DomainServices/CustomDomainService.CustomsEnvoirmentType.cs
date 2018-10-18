using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        
        public CustomsEnvoirmentTypePM GetSingleCustomsEnvoirmentTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsEnvoirmentTypeQuery = new CustomsEnvoirmentTypeQueryService(customContext);
            CustomsEnvoirmentTypePM CustomsEnvoirmentType = customsEnvoirmentTypeQuery.GetSingle(id, false, false);
            return CustomsEnvoirmentType;
        }

        public CustomsEnvoirmentTypeList GetSingleCustomsEnvoirmentTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsEnvoirmentTypeListQueryService listService = new CustomsEnvoirmentTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsEnvoirmentTypeList> GetCustomsEnvoirmentTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsEnvoirmentTypeListQueryService listService = new CustomsEnvoirmentTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsEnvoirmentTypeList> GetCustomsEnvoirmentTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsEnvoirmentTypeListQueryService listService = new CustomsEnvoirmentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsEnvoirmentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsEnvoirmentTypeListQueryService queryService = new CustomsEnvoirmentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }

    }
}