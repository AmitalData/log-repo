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

        public VehicleTecnologyTypePM GetSingleVehicleTecnologyTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleTecnologyTypeQuery = new VehicleTecnologyTypeQueryService(customContext);
            VehicleTecnologyTypePM VehicleTecnologyType = vehicleTecnologyTypeQuery.GetSingle(id, false, false);
            return VehicleTecnologyType;
        }

        public VehicleTecnologyTypeList GetSingleVehicleTecnologyTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleTecnologyType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehicleTecnologyTypeListQueryService listService = new VehicleTecnologyTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<VehicleTecnologyTypeList> GetVehicleTecnologyTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleTecnologyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleTecnologyTypeListQueryService listService = new VehicleTecnologyTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VehicleTecnologyTypeList> GetVehicleTecnologyTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.VehicleTecnologyType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VehicleTecnologyTypeListQueryService listService = new VehicleTecnologyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVehicleTecnologyTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.VehicleTecnologyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleTecnologyTypeListQueryService queryService = new VehicleTecnologyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}