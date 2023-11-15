using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
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
        public CustomsCountryPM GetSingleCustomsCountryPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsCountryQuery = new CustomsCountryQueryService(customContext);
            CustomsCountryPM CustomsCountry = customsCountryQuery.GetSingleCustomsCountryWithTenant(id, tenant);
            return CustomsCountry;
        }

        public CustomsCountryList GetSingleCustomsCountryList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.CustomsCountry", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsCountryListQueryService listService = new CustomsCountryListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsCountryList> GetCustomsCountryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsCountry", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsCountryListQueryService listService = new CustomsCountryListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsCountryList> GetCustomsCountryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsCountry", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsCountryListQueryService listService = new CustomsCountryListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsCountryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CustomsCountry", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsCountryListQueryService queryService = new CustomsCountryListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}