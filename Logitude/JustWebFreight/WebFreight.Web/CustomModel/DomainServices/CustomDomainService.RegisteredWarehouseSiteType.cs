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

        public RegisteredWarehouseSiteTypePM GetSingleRegisteredWarehouseSiteTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            registeredWarehouseSiteTypeQuery = new RegisteredWarehouseSiteTypeQueryService(customContext);
            RegisteredWarehouseSiteTypePM RegisteredWarehouseSiteType = registeredWarehouseSiteTypeQuery.GetSingle(code, false, false);
            return RegisteredWarehouseSiteType;
        }

        public RegisteredWarehouseSiteTypeList GetSingleRegisteredWarehouseSiteTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RegisteredWarehouseSiteType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            RegisteredWarehouseSiteTypeListQueryService listService = new RegisteredWarehouseSiteTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<RegisteredWarehouseSiteTypeList> GetRegisteredWarehouseSiteTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RegisteredWarehouseSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RegisteredWarehouseSiteTypeListQueryService listService = new RegisteredWarehouseSiteTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<RegisteredWarehouseSiteTypeList> GetRegisteredWarehouseSiteTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RegisteredWarehouseSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RegisteredWarehouseSiteTypeListQueryService listService = new RegisteredWarehouseSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetRegisteredWarehouseSiteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RegisteredWarehouseSiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RegisteredWarehouseSiteTypeListQueryService queryService = new RegisteredWarehouseSiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}