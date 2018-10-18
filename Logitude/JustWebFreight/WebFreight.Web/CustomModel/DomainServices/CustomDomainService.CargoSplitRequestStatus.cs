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
        public CargoSplitRequestStatusPM GetSingleCargoSplitRequestStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            cargoSplitRequestStatusQueryService = new CargoSplitRequestStatusQueryService(customContext);
            CargoSplitRequestStatusPM CargoSplitRequestStatus = cargoSplitRequestStatusQueryService.GetSingle(code, false, false);
            return CargoSplitRequestStatus;
        }

        public CargoSplitRequestStatusList GetSingleCargoSplitRequestStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CargoSplitRequestStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CargoSplitRequestStatusListQueryService listService = new CargoSplitRequestStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CargoSplitRequestStatusList> GetCargoSplitRequestStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CargoSplitRequestStatusListQueryService listService = new CargoSplitRequestStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CargoSplitRequestStatusList> GetCargoSplitRequestStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CargoSplitRequestStatusListQueryService listService = new CargoSplitRequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetCargoSplitRequestStatusFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    CargoSplitRequestStatusListQueryService queryService = new CargoSplitRequestStatusListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}