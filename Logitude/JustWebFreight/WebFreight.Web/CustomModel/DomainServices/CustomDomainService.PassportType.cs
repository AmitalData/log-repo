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

        public PassportTypePM GetSinglePassportTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            passportTypeQuery = new PassportTypeQueryService(customContext);
            PassportTypePM PassportType = passportTypeQuery.GetSingle(code, false, false);
            return PassportType;
        }

        public PassportTypeList GetSinglePassportTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PassportType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PassportTypeListQueryService listService = new PassportTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<PassportTypeList> GetPassportTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PassportType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PassportTypeListQueryService listService = new PassportTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PassportTypeList> GetPassportTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PassportType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PassportTypeListQueryService listService = new PassportTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPassportTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PassportType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PassportTypeListQueryService queryService = new PassportTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}