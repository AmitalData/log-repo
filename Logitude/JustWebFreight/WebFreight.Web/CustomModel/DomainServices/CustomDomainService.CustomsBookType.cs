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

        public CustomsBookTypePM GetSingleCustomsBookTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsBookTypeQuery = new CustomsBookTypeQueryService(customContext);
            CustomsBookTypePM CustomsBookType = customsBookTypeQuery.GetSingle(code, false, false);
            return CustomsBookType;
        }

        public CustomsBookTypeList GetSingleCustomsBookTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsBookType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsBookTypeListQueryService listService = new CustomsBookTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomsBookTypeList> GetCustomsBookTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsBookType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsBookTypeListQueryService listService = new CustomsBookTypeListQueryService(customContext);
            return listService.GetList(tenant).Where(d => d.Code != "2").ToList();
        }


        public List<CustomsBookTypeList> GetCustomsBookTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsBookType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsBookTypeListQueryService listService = new CustomsBookTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsBookTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsBookType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsBookTypeListQueryService queryService = new CustomsBookTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}