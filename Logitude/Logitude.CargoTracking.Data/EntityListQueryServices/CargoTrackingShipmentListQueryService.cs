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
            IQueryable<CargoTrackingPortList> ports = (from p in context.CargoTrackingPorts
                                                 join c in context.CargoTrackingCountries on p.CountryId equals c.Id
                                                 select new CargoTrackingPortList() {
                                                     Id = p.Id ,
                                                     EnglishName = p.EnglishName,
                                                     CountryCode = c.Code,


                                                 });
            IQueryable<CargoTrackingShipmentList> query = (from a in iQueryable join fp in ports on a.FromPortId equals fp.Id
                                                           join tp in ports on a.ToPortId equals tp.Id
                                                           join s in context.CargoTrackingShipmentSearches  on a.SecurityKey equals s.SecurityKey 
                                                          join m in context.CargoTrackingMilestones on a.CurrentMilestoneCode equals m.Code 
                                                          join t in context.CargoTrackingTransportModes on a.TransportModeId equals t.Id
                                                         
                                                               //  group g by new {a.SecurityKey , g.FirstOrDefault().SearchFields} into gp
                                                           select new CargoTrackingShipmentList()
                                                           {

                                                               Id= a.Id,

                                                               Tenant = a.Tenant,

                                                               EntityId = a.EntityId,
                                                               
                                                               SecurityKey = a.SecurityKey,

                                                               ForwardingShipmentHeaderId = a.ForwardingShipmentHeaderId,
                                                               ToPortCountryCode = tp.CountryCode,
                                                               CustomsShipmentHeaderId = a.CustomsShipmentHeaderId,
                                                               FromPortCountryCode = fp.CountryCode,
                                                               EntityType = a.EntityType,
                                                               FromPortName = fp.EnglishName,
                                                               ToPortName = tp.EnglishName,
                                                               CurrentMilestoneCode = a.CurrentMilestoneCode,
                                                               CurrentMilestoneName = m.EnglishName,
                                                               CurrentMilestoneDate = a.CurrentMilestoneDate,
                                                               SearchReferences = s.SearchFields ,
                                                               CustomerId = a.CustomerId,
                                                               TransportModeName = t.Name,
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
            if(list != null)
            {
             list=   FillShipmentListExtraFileds(list);

            }
            return list;
        }
        private CargoTrackingShipmentList FillShipmentListExtraFileds(CargoTrackingShipmentList list)
        {
            CargoTrackingPortListQueryService cargoTrackingPortListQuery = new CargoTrackingPortListQueryService(context);
            CargoTrackingTransportModeListQueryService cargoTrackingTransportModeListQueryService = new CargoTrackingTransportModeListQueryService(context);
            CargoTrackingPortList fromPort = cargoTrackingPortListQuery.GetSingle(list.FromPortId);
            CargoTrackingPortList toPort = cargoTrackingPortListQuery.GetSingle(list.ToPortId);
            CargoTrackingTransportModeList transportModeList = cargoTrackingTransportModeListQueryService.GetSingle(list.TransportModeId);
            list.TransportModeName = transportModeList != null ? transportModeList.Name : null;
            list.FromPortName = fromPort != null ? fromPort.EnglishName : null;
            list.ToPortName = toPort != null ? toPort.EnglishName : null;
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
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetBySecurityKeies(shipmentsSecurityKeies, tenant);

            List<CargoTrackingShipmentList> shipmetsLists = GetIqueryableList(shipments).ToList();
            
            foreach (var key in shipmentsSecurityKeies)
            {

                string[] references = shipmetsLists.Where(d => d.SecurityKey == key).Select(d => d.SearchReferences).ToArray();
            //  List< CargoTrackingShipmentList> shipmetsList = shipmetsLists.ToList();
                shipmetsLists.Where(d => d.SecurityKey == key).ToList().ForEach(d => { d.SearchReferences = String.Join(",", references); });
                shipmetsLists = shipmetsLists.GroupBy(p =>  p.SecurityKey ).Select(g => g.Last()).ToList();
            }
            return shipmetsLists;
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
	