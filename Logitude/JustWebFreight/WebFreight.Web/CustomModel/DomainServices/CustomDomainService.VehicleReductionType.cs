using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public VehicleReductionTypePM GetSingleVehicleReductionTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleReductionTypeQueryService = new VehicleReductionTypeQueryService(customContext);
            VehicleReductionTypePM VehicleReductionType = vehicleReductionTypeQueryService.GetSingle(code, false, false);
            return VehicleReductionType;
        }

        public VehicleReductionTypeList GetSingleVehicleReductionTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleReductionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehicleReductionTypeListQueryService listService = new VehicleReductionTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<VehicleReductionTypeList> GetVehicleReductionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.VehicleReductionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleReductionTypeListQueryService listService = new VehicleReductionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VehicleReductionTypeList> GetVehicleReductionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.VehicleReductionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleReductionTypeListQueryService listService = new VehicleReductionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVehicleReductionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.VehicleReductionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleReductionTypeListQueryService queryService = new VehicleReductionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}