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
            IQueryable<CargoTrackingPortList> ports = GetPorts();
            IQueryable<CargoTrackingShipmentList> query = (from a in iQueryable join fp in ports on a.FromPortId equals fp.Id
                                                           join tp in ports on a.ToPortId equals tp.Id
                                                           join s in context.CargoTrackingShipmentSearches  on a.EntityId equals s.ShipmentId 
                                                           join m in context.CargoTrackingMilestones on a.CurrentMilestoneCode equals m.Code into lm
                                                           from m in lm.DefaultIfEmpty()
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
                                                               DeliveredDate= a.DeliveredDate,
                                                               ConsigneeId = a.ConsigneeId,

                                                               GrossWeight = a.GrossWeight,

                                                               Volume = a.Volume,

                                                               PickupDone = a.PickupDone,

                                                               ClearanceDone = a.ClearanceDone,

                                                               PickupDate = a.PickupDate,
                                                               PickupEstimationDate= a.PickupEstimationDate,
                                                               FromWarehouseEstimationDate = a.FromWarehouseEstimationDate,
                                                               ToWarehouseEstimationDate = a.ToWarehouseEstimationDate,
                                                               DepartureEstimationDate = a.DepartureEstimationDate,
                                                               ArrivalEstimationDate= a.ArrivalEstimationDate,
                                                               DeliveredEstimationDate= a.DeliveredEstimationDate,
                                                               
                                                               ClearanceDate = a.ClearanceDate,

                                                           });
            return query;
        }

        private IQueryable<CargoTrackingPortList> GetPorts()
        {
            return  (from p in context.CargoTrackingPorts
                                                       join c in context.CargoTrackingCountries on p.CountryId equals c.Id
                                                       select new CargoTrackingPortList()
                                                       {
                                                           Id = p.Id,
                                                           EnglishName = p.EnglishName,
                                                           CountryCode = c.Code,
                                                       });
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

                    PickupEstimationDate = poco.PickupEstimationDate,

                    FromWarehouseDate = poco.FromWarehouseDate,

                    FromWarehouseDone = poco.FromWarehouseDone,

                    FromWarehouseEstimationDate = poco.FromWarehouseEstimationDate,

                    FromWarehouseNotes = poco.FromWarehouseNotes,

                    DepartureDate = poco.DepartureDate,

                    DepartureDone = poco.DepartureDone,

                    DepartureEstimationDate = poco.DepartureEstimationDate,

                    ArrivalEstimationDate = poco.ArrivalEstimationDate,

                    ArrivalDate = poco.ArrivalDate,

                    ArrivalDone = poco.ArrivalDone,

                   ToWarehouseDate = poco.ToWarehouseDate,

                   ToWarehouseDone = poco.ToWarehouseDone,

                   ToWarehouseEstimationDate = poco.ToWarehouseEstimationDate,

                   ToWarehouseNotes = poco.ToWarehouseNotes,

                   CustomsPaymentDate = poco.CustomsPaymentDate,

                   CustomsPaymentDone = poco.CustomsPaymentDone,

                   CustomsClearanceDate = poco.CustomsClearanceDate,

                   DeliveredDate = poco.DeliveredDate,

                   DeliveredDone = poco.DeliveredDone,

                   DeclarationDate = poco.DeclarationDate,

                   DeliveredEstimationDate = poco.DeliveredEstimationDate,

                   CreateDate = poco.CreateDate,

                   ConsigneeName = poco.ConsigneeName,

                   
                    

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

        public List<CargoTrackingShipmentList> GetShipments(List<string> ShipmentIds, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetByShipmentIds(ShipmentIds, tenant);
            List<CargoTrackingShipmentList> shipmetsLists = GetIqueryableList(shipments).ToList();
            foreach (var Id in ShipmentIds)
            {
                string[] references = shipmetsLists.Where(d => d.EntityId == Id).Select(d => d.SearchReferences).ToArray();
                shipmetsLists = shipmetsLists.GroupBy(p => p.SecurityKey).Select(g => g.Last()).ToList();
                foreach (CargoTrackingShipmentList shipment in shipmetsLists)
                {
                    if (shipment.EntityId == Id)
                    {
                        shipment.SearchReferences = String.Join(",", references);
                        if(shipment.CurrentMilestoneCode == null)
                        {
                            GetMilestonesFieldsFromCargoTrackingShipment(shipment);
                            
                        }
                    }
                }            
            }
            return shipmetsLists;
        }
       
        public List<Milestone> GetMilestonesFieldsFromCargoTrackingShipment( CargoTrackingShipmentList  Shipment)
        {
            List<Milestone> milestones = new List<Milestone>();

            milestones.Add(new Milestone() {Id = 2, Code= "Pickup", Name= "Pickup", Date= Shipment.PickupDate,EstimationDate= Shipment.PickupEstimationDate, Done= Shipment.PickupDone, Notes= null,IsCurrent=false,IsEstimation= Shipment.PickupDone==true? false : true });
            milestones.Add(new Milestone() {Id = 3, Code = "FromWarehouse", Name = "From Warehouse", Date = Shipment.FromWarehouseDate, EstimationDate = Shipment.FromWarehouseEstimationDate, Done = Shipment.FromWarehouseDone, Notes = Shipment.FromWarehouseNotes, IsCurrent = false, IsEstimation = Shipment.FromWarehouseDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 6, Code = "ToWarehouse", Name = "To Warehouse", Date = Shipment.ToWarehouseDate, EstimationDate = Shipment.ToWarehouseEstimationDate, Done = Shipment.ToWarehouseDone, Notes = Shipment.ToWarehouseNotes, IsCurrent = false, IsEstimation = Shipment.ToWarehouseDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 4, Code = "Departure", Name = "Departure", Date = Shipment.DepartureDate, EstimationDate = Shipment.DepartureEstimationDate, Done = Shipment.DepartureDone, Notes = null, IsCurrent = false, IsEstimation = Shipment.DepartureDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 5, Code = "Arrival", Name = "Arrival", Date = Shipment.ArrivalDate, EstimationDate = Shipment.ArrivalEstimationDate, Done = Shipment.ArrivalDone, Notes = null, IsCurrent = false, IsEstimation = Shipment.ArrivalDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 8, Code = "CustomsPayment", Name = "Customs Payment", Date = Shipment.CustomsPaymentDate, EstimationDate = null, Done = Shipment.CustomsPaymentDone, Notes = null, IsCurrent = false, IsEstimation = Shipment.CustomsPaymentDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 9, Code = "Clearance", Name = "Clearance", Date = Shipment.ClearanceDate, EstimationDate = null, Done = Shipment.ClearanceDone, Notes = null, IsCurrent = false, IsEstimation = Shipment.ClearanceDone == true ? false : true });
            milestones.Add(new Milestone() {Id = 11, Code = "Delivered", Name = "Delivered", Date = Shipment.DeliveredDate, EstimationDate = Shipment.DeliveredEstimationDate, Done = Shipment.DeliveredDone, Notes = null, IsCurrent = false, IsEstimation = Shipment.DeliveredDone == true ? false : true });
            milestones = milestones.OrderByDescending(s=>s.IsEstimation==true? s.EstimationDate : s.Date).ThenByDescending(s => s.Id).ToList();
            string CurrentMilestoneCode = milestones.Where(s=>s.IsEstimation==false).OrderByDescending(s => s.Date).ThenByDescending(s=>s.Id).Select(s => s.Code).FirstOrDefault();
            if (CurrentMilestoneCode == null)
            {
                Shipment.FutureMilstoneName = milestones.Where(s => s.IsEstimation == true && s.EstimationDate != null).OrderByDescending(s => s.Date).ThenByDescending(s => s.Id).Select(s => s.Name).FirstOrDefault();
                Shipment.FutureMilstoneDate = milestones.Where(s => s.IsEstimation == true && s.Name == Shipment.FutureMilstoneName).OrderByDescending(s => s.Date).ThenByDescending(s => s.Id).Select(s => s.EstimationDate).FirstOrDefault();
            }

            for (int i=0; i < milestones.Count; i++)
            {
                if (milestones[i].Code == CurrentMilestoneCode)
                {
                    milestones[i].IsCurrent = true;
                    break;
                }

            }
            return milestones;
        }
        public CargoTrackingShipmentList GetShipment(string SecurityKey, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(tenant);
            CargoTrackingShipment shipment = repo.GetBySecurityKey(SecurityKey, tenant);

            CargoTrackingShipmentList shipmentList = GetEntityList(shipment);

            return shipmentList;
        }

    }


    public class Milestone
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EstimationDate { get; set; }
        public bool? Done { get; set; }
        public bool? IsEstimation { get; set; }
        public bool? IsCurrent { get; set; }
      
    }
    public class CargoTrackingShipmentWithMilestones
    {
        public List<Milestone> Milestones { get; set; }
        public CargoTrackingShipmentList  ShipmentList { get; set; }

    }

}
	