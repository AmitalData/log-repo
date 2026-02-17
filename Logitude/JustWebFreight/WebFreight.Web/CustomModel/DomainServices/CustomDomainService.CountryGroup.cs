using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CountryGroupPM GetSingleCountryGroupPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            countryGroupQuery = new CountryGroupQueryService(customContext);
            CountryGroupPM CountryGroup = countryGroupQuery.GetSingle(id, false, false);
            return CountryGroup;
        }

        public CountryGroupList GetSingleCountryGroupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CountryGroup", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CountryGroupListQueryService listService = new CountryGroupListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CountryGroupList> GetCountryGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CountryGroup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CountryGroupListQueryService listService = new CountryGroupListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CountryGroupList> GetCountryGroupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CountryGroup", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CountryGroupListQueryService listService = new CountryGroupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCountryGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.CountryGroup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CountryGroupListQueryService queryService = new CountryGroupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}