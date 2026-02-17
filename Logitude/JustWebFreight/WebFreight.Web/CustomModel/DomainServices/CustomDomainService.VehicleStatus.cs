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


        public VehicleStatusPM GetSingleVehicleStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleStatusQuery = new VehicleStatusQueryService(customContext);
            VehicleStatusPM VehicleStatus = vehicleStatusQuery.GetSingle(id, false, false);
            return VehicleStatus;
        }

        public VehicleStatusList GetSingleVehicleStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehicleStatusListQueryService listService = new VehicleStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VehicleStatusList> GetVehicleStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleStatusListQueryService listService = new VehicleStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VehicleStatusList> GetVehicleStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.VehicleStatus", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VehicleStatusListQueryService listService = new VehicleStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVehicleStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.VehicleStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleStatusListQueryService queryService = new VehicleStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}