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

        public VehicleSafetyAccessoryTypePM GetSingleVehicleSafetyAccessoryTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleSafetyAccessoryTypeQuery = new VehicleSafetyAccessoryTypeQueryService(customContext);
            VehicleSafetyAccessoryTypePM VehicleSafetyAccessoryType = vehicleSafetyAccessoryTypeQuery.GetSingle(id, false, false);
            return VehicleSafetyAccessoryType;
        }

        public VehicleSafetyAccessoryTypeList GetSingleVehicleSafetyAccessoryTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.VehicleSafetyAccessoryType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehicleSafetyAccessoryTypeListQueryService listService = new VehicleSafetyAccessoryTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VehicleSafetyAccessoryTypeList> GetVehicleSafetyAccessoryTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.VehicleSafetyAccessoryType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleSafetyAccessoryTypeListQueryService listService = new VehicleSafetyAccessoryTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VehicleSafetyAccessoryTypeList> GetVehicleSafetyAccessoryTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VehicleSafetyAccessoryType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VehicleSafetyAccessoryTypeListQueryService listService = new VehicleSafetyAccessoryTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVehicleSafetyAccessoryTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VehicleSafetyAccessoryType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleSafetyAccessoryTypeListQueryService queryService = new VehicleSafetyAccessoryTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}