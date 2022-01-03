using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using WebFreight.Web.Security;

namespace WebFreight.Web.CargoTrackingModel.DomainServices
{
	public class CargoTrackingDomainService
    {
        private ICargoTrackingContext context;
        public List<CargoTrackingShipmentList> GetCargoTrackingShipmentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            context = CargoTrackingContext.GetContext(tenant);
            CargoTrackingShipmentListQueryService listService = new CargoTrackingShipmentListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetCargoTrackingShipmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            context = CargoTrackingContext.GetContext(tenant);
            CargoTrackingShipmentListQueryService queryService = new CargoTrackingShipmentListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}
