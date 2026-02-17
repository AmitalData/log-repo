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

        public VehiclePoolTypePM GetSingleVehiclePoolTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehiclePoolTypeQuery = new VehiclePoolTypeQueryService(customContext);
            VehiclePoolTypePM VehiclePoolType = vehiclePoolTypeQuery.GetSingle(id, false, false);
            return VehiclePoolType;
        }

        public VehiclePoolTypeList GetSingleVehiclePoolTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehiclePoolType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehiclePoolTypeListQueryService listService = new VehiclePoolTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VehiclePoolTypeList> GetVehiclePoolTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehiclePoolType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehiclePoolTypeListQueryService listService = new VehiclePoolTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VehiclePoolTypeList> GetVehiclePoolTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.VehiclePoolType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VehiclePoolTypeListQueryService listService = new VehiclePoolTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVehiclePoolTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.VehiclePoolType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehiclePoolTypeListQueryService queryService = new VehiclePoolTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}