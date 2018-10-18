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

        public GenderPM GetSingleGenderPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            genderQuery = new GenderQueryService(customContext);
            GenderPM Gender = genderQuery.GetSingle(code, false, false);
            return Gender;
        }

        public GenderList GetSingleGenderList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Gender", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GenderListQueryService listService = new GenderListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<GenderList> GetGenderLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Gender", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GenderListQueryService listService = new GenderListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<GenderList> GetGenderFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Gender", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GenderListQueryService listService = new GenderListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetGenderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Gender", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GenderListQueryService queryService = new GenderListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}