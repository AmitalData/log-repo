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

    public partial class CargoTrackingShipmentListQueryService
    {
        private IQueryable<CargoTrackingShipmentList> GetIqueryableList(IQueryable<CargoTrackingShipment> iQueryable)
        {
          
            IQueryable<CargoTrackingShipmentList> query = (from a in iQueryable join p in context.CargoTrackingPorts on a.FromPortId equals p.Id 
                                                           join entity in context.CargoTrackingShipmentSearches on a.SecurityKey equals entity.SecurityKey 

                                                           select new CargoTrackingShipmentList()
                                                           {

                                                               Id = a.Id,

                                                               Tenant = a.Tenant,

                                                               EntityId = a.EntityId,
                                                               
                                                               SecurityKey = a.SecurityKey,

                                                               ForwardingShipmentHeaderId = a.ForwardingShipmentHeaderId,

                                                               CustomsShipmentHeaderId = a.CustomsShipmentHeaderId,

                                                               EntityType = a.EntityType,

                                                               CurrentMilestoneCode = a.CurrentMilestoneCode,

                                                               CurrentMilestoneDate = a.CurrentMilestoneDate,
                                                               SearchReferences = string.Join(entity.SearchFields,","),
                                                               CustomerId = a.CustomerId,

                                                               TransportModeId = a.TransportModeId,

                                                               Master = a.Master,

                                                               House = a.House,

                                                               ShipmentNumber = a.ShipmentNumber,

                                                               FromPortId = a.FromPortId,

                                                               ToPortId = a.ToPortId,

                                                               ShipperId = a.ShipperId,

                                                               ConsigneeId = a.ConsigneeId,

                                                               GrossWeight = a.GrossWeight,

                                                               Volume = a.Volume,

                                                               PickupDone = a.PickupDone,

                                                               ClearanceDone = a.ClearanceDone,

                                                               PickupDate = a.PickupDate,

                                                               ClearanceDate = a.ClearanceDate,

                                                           });
            return query;
        }

        public CargoTrackingShipmentList GetEntityList(CargoTrackingShipment poco)
        {
            CargoTrackingShipmentList list = null;
            if (poco != null)
                list = new CargoTrackingShipmentList()
                {

                    Id = poco.Id,

                    Tenant = poco.Tenant,

                    EntityId = poco.EntityId,

                    ForwardingShipmentHeaderId = poco.ForwardingShipmentHeaderId,

                    CustomsShipmentHeaderId = poco.CustomsShipmentHeaderId,

                    EntityType = poco.EntityType,

                    CurrentMilestoneCode = poco.CurrentMilestoneCode,

                    CurrentMilestoneDate = poco.CurrentMilestoneDate,

                    CustomerId = poco.CustomerId,

                    TransportModeId = poco.TransportModeId,

                    Master = poco.Master,

                    House = poco.House,

                    ShipmentNumber = poco.ShipmentNumber,

                    FromPortId = poco.FromPortId,

                    ToPortId = poco.ToPortId,
                    
                    SecurityKey = poco.SecurityKey,

                    ShipperId = poco.ShipperId,

                    ConsigneeId = poco.ConsigneeId,

                    GrossWeight = poco.GrossWeight,

                    Volume = poco.Volume,

                    PickupDone = poco.PickupDone,

                    ClearanceDone = poco.ClearanceDone,

                    PickupDate = poco.PickupDate,

                    ClearanceDate = poco.ClearanceDate,

                };

            return list;
        }

        private IQueryable<CargoTrackingShipment> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipment> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<CargoTrackingShipment> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CargoTrackingShipment> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<CargoTrackingShipmentList> GetShipments(List<string> shipmentsSecurityKeies, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(tenant);
            IQueryable<CargoTrackingShipment> shipments = repo.GetBySecurityKeies(shipmentsSecurityKeies, tenant);

            IQueryable<CargoTrackingShipmentList> shipmetsLists = GetIqueryableList(shipments);

            return shipmetsLists.ToList();
        }
        public CargoTrackingShipmentList GetShipment(string SecurityKey, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(tenant);
            CargoTrackingShipment shipment = repo.GetBySecurityKey(SecurityKey, tenant);

            CargoTrackingShipmentList shipmentList = GetEntityList(shipment);

            return shipmentList;
        }

    }


}
	