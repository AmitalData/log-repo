	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.Repositories;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentSearchListQueryService
    {
        private IQueryable<CargoTrackingShipmentSearchList> GetIqueryableList(IQueryable<CargoTrackingShipmentSearch> iQueryable)
        {
            IQueryable<CargoTrackingShipmentSearchList> query = (from a in iQueryable
                                                                 select new CargoTrackingShipmentSearchList()
                                                                 {

                                                                     Id = a.Id,

                                                                     Tenant = a.Tenant,

                                                                     SearchFields = a.SearchFields,

                                                                     SecurityKey = a.SecurityKey,

                                                                     ShipmentDate = a.ShipmentDate,

                                                                 });
            return query;
        }

        private IQueryable<CargoTrackingShipmentSearch> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<CargoTrackingShipmentSearch> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<CargoTrackingShipmentList> GetShipments(string searchText, int tenant)
        {
            CargoTrackingShipmentSearchRepository repo = new CargoTrackingShipmentSearchRepository(tenant);
            IQueryable<CargoTrackingShipmentSearch> shipmentsSearchEntities = repo.GetShipmentSearchEntities(searchText, tenant);

            List<string> shipmentsSecurityKeies = shipmentsSearchEntities.Select(d => d.SecurityKey).ToList();

            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(context);
            List<CargoTrackingShipmentList> shipments = shipmentsQuery.GetShipments(shipmentsSecurityKeies, tenant);


            //foreach (CargoTrackingShipmentList shipment in shipments)
            //{
            //  List< CargoTrackingShipmentSearch> shipmentSeachEntities=  repo.GetShipmentSearchBySecurityKeys(shipment.SecurityKey, shipment.Tenant);
            //    string[] references = shipmentSeachEntities.Select(d => d.SearchFields).ToArray();
            //    shipment.SearchReferences = String.Join(",", references);
            //    CargoTrackingTransportModeListQueryService cargoTrackingTransportModeListQueryService = new CargoTrackingTransportModeListQueryService(tenant);
            //      cargoTrackingTransportModeListQueryService.GetSingle(shipment.TransportModeId);
                
            //}
            return shipments;
        }

    }


}
	