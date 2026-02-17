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

        public FuelTypePM GetSingleFuelTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            fuelTypeQuery = new FuelTypeQueryService(customContext);
            FuelTypePM FuelType = fuelTypeQuery.GetSingle(id, false, false);
            return FuelType;
        }

        public FuelTypeList GetSingleFuelTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.FuelType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            FuelTypeListQueryService listService = new FuelTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<FuelTypeList> GetFuelTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.FuelType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FuelTypeListQueryService listService = new FuelTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<FuelTypeList> GetFuelTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.FuelType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            FuelTypeListQueryService listService = new FuelTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetFuelTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.FuelType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FuelTypeListQueryService queryService = new FuelTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}