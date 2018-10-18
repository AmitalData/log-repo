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

        public CityPM GetSingleCityPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            cityQuery = new CityQueryService(customContext);
            CityPM City = cityQuery.GetSingle(code, false, false);
            return City;
        }

        public CityList GetSingleCityList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.City", "READ", tenant);    

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CityListQueryService listService = new CityListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CityList> GetCityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.City", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CityListQueryService listService = new CityListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CityList> GetCityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.City", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CityListQueryService listService = new CityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.City", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CityListQueryService queryService = new CityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}