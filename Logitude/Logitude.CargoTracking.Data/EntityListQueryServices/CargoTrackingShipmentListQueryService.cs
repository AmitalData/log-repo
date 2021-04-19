using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentListQueryService
    {
        private IQueryable<CargoTrackingShipmentList> GetIqueryableList(IQueryable<CargoTrackingShipment> iQueryable)
        {
            IQueryable<CargoTrackingPortList> ports = GetPorts();
            IQueryable<CargoTrackingShipmentList> query = (from shipment in iQueryable
                                                           join fromPort in ports on shipment.FromPortId equals fromPort.Id
                                                           join toPort in ports on shipment.ToPortId equals toPort.Id
                                                           join customer in context.CargoTrackingCards on shipment.CustomerId equals customer.Id
                                                           join shipper in context.CargoTrackingCards on shipment.ShipperId equals shipper.Id
                                                           
                                                           join milestone in context.CargoTrackingMilestones on shipment.CurrentMilestoneCode equals milestone.Code into lm
                                                           from milestone in lm.DefaultIfEmpty()
                                                           
                                                           join transportMode in context.CargoTrackingTransportModes on shipment.TransportModeId equals transportMode.Id

                                                           select new CargoTrackingShipmentList()
                                                           {

                                                               Id = shipment.Id,
                                                               Tenant = shipment.Tenant,
                                                               EntityId = shipment.EntityId,
                                                               SecurityKey = shipment.SecurityKey,
                                                               ForwardingShipmentHeaderId = shipment.ForwardingShipmentHeaderId,
                                                               CustomsShipmentHeaderId = shipment.CustomsShipmentHeaderId,
                                                               EntityType = shipment.EntityType,
                                                               CurrentMilestoneCode = shipment.CurrentMilestoneCode,
                                                               CurrentMilestoneName = milestone.EnglishName,
                                                               CurrentMilestoneDate = shipment.CurrentMilestoneDate,
                                                               CustomerId = shipment.CustomerId,
                                                               TransportModeId = shipment.TransportModeId,
                                                               Master = shipment.Master,
                                                               House = shipment.House,
                                                               ShipmentNumber = shipment.ShipmentNumber,
                                                               FromPortId = shipment.FromPortId,
                                                               ToPortId = shipment.ToPortId,
                                                               ShipperId = shipment.ShipperId,
                                                               DeliveredDate = shipment.DeliveredDate,
                                                               ConsigneeId = shipment.ConsigneeId,
                                                               GrossWeight = shipment.GrossWeight,
                                                               Volume = shipment.Volume,
                                                               PickupDone = shipment.PickupDone,
                                                               ClearanceDone = shipment.ClearanceDone,
                                                               PickupDate = shipment.PickupDate,
                                                               PickupEstimationDate = shipment.PickupEstimationDate,
                                                               FromWarehouseEstimationDate = shipment.FromWarehouseEstimationDate,
                                                               ToWarehouseEstimationDate = shipment.ToWarehouseEstimationDate,
                                                               DepartureDate = shipment.DepartureDate,
                                                               DepartureDone = shipment.DepartureDone,
                                                               DepartureEstimationDate = shipment.DepartureEstimationDate,
                                                               ArrivalEstimationDate = shipment.ArrivalEstimationDate,
                                                               DeliveredEstimationDate = shipment.DeliveredEstimationDate,
                                                               ClearanceDate = shipment.ClearanceDate,
                                                               CreateDate = shipment.CreateDate,
                                                               DirectionId = shipment.DirectionId,
                                                               CustomerReference = shipment.CustomerReference,
                                                               AssignedCustomsAgentDate = shipment.AssignedCustomsAgentDate,
                                                               AssignedCustomsAgentDone = shipment.AssignedCustomsAgentDone,
                                                               AssignedCustomsAgentEstDate = shipment.AssignedCustomsAgentEstDate,
                                                               AssignedCustomsAgentExcReason = shipment.AssignedCustomsAgentExcReason,
                                                               AssignedCustomsAgentNotes= shipment.AssignedCustomsAgentNotes,
                                                               ShipmentLevelCode = shipment.ShipmentLevelCode,
                                                               AssignedTruckerDate = shipment.AssignedTruckerDate,
                                                               AssignedTruckerDone = shipment.AssignedTruckerDone,
                                                               GrossWeightUnitCode = shipment.GrossWeightUnitCode,
                                                               ArrivalDate = shipment.ArrivalDate,
                                                               ArrivalDone = shipment.ArrivalDone,
                                                               CustomsPaymentDate = shipment.CustomsPaymentDate,
                                                               ContainersNumbers = shipment.ContainersNumbers,
                                                               FromWarehouseDate = shipment.FromWarehouseDate,
                                                               FromWarehouseNotes = shipment.FromWarehouseNotes,
                                                               ToWarehouseDate = shipment.ToWarehouseDate,
                                                               ToWarehouseNotes = shipment.ToWarehouseNotes,
                                                               DeliveryEstimationDate = shipment.DeliveryEstimationDate,
                                                               DeliveryDate = shipment.DeliveryDate,
                                                               DeliveryNotes = shipment.DeliveryNotes,
                                                               AssignedTruckerEstimationDate = shipment.AssignedTruckerEstimationDate,
                                                               AssignedTruckerNotes = shipment.AssignedTruckerNotes,
                                                               NumberOfPackages = shipment.PackagesQuantity,

                                                               // port fields
                                                               ToPortCountryCode = toPort.CountryCode,
                                                               FromPortCountryCode = fromPort.CountryCode,
                                                               FromPortName = fromPort.EnglishName,
                                                               ToPortName = toPort.EnglishName,
                                                               FromPortCode = fromPort.Code,
                                                               ToPortCode = toPort.Code,

                                                               // transport mode
                                                               TransportModeName = transportMode.Name,

                                                               // card
                                                               CustomerEnglishName = customer.EnglishName,
                                                               CustomerLocalName = customer.LocalName,
                                                               ShipperName = shipper.EnglishName,


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
                        Code = p.Code,
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

                    ContainersNumbers= poco.ContainersNumbers,

                    PackagesQuantity = poco.PackagesQuantity,

                    CustomerReference = poco.CustomerReference,
                    ShipmentLevelCode = poco.ShipmentLevelCode,

                    AssignedTruckerDate = poco.AssignedTruckerDate,
                    AssignedTruckerDone  = poco.AssignedTruckerDone,
                    AssignedTruckerEstimationDate = poco.AssignedTruckerEstimationDate,
                    AssignedTruckerNotes = poco.AssignedTruckerNotes,

                    AssignedCustomsAgentDate = poco.AssignedCustomsAgentDate,
                    AssignedCustomsAgentDone = poco.AssignedCustomsAgentDone,
                    AssignedCustomsAgentEstDate = poco.AssignedCustomsAgentEstDate,
                    AssignedCustomsAgentExcReason = poco.AssignedCustomsAgentExcReason,
                    AssignedCustomsAgentNotes = poco.AssignedCustomsAgentNotes,

                    DirectionId =poco.DirectionId,
                    DeliveryDone = poco.DeliveryDone,

                    DeliveryDate = poco.DeliveryDate,

                    DeliveryEstimationDate= poco.DeliveryEstimationDate,

                    DeliveryNotes= poco.DeliveryNotes,

                    DeliveryExceptionReason = poco.DeliveryExceptionReason,

                    GrossWeightUnitCode = poco.GrossWeightUnitCode

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

        public List<CargoTrackingShipmentList> GetShipments(List<string> shipmentIds, int tenant)
        {
            List<CargoTrackingShipmentList> shipments = GetShipmentsByIds(shipmentIds, tenant);

            foreach (CargoTrackingShipmentList shipment in shipments)
            {
                if (shipment.CurrentMilestoneCode == null)
                {
                    List<Milestone> shipmentMilestones = BuildShipmentMilstones(shipment);
                    SetMilestonesStatus(shipment, shipmentMilestones);
                }
            }

            return shipments;
        }

        public List<CargoTrackingShipmentList> GetShipmentsByIds(List<string> ShipmentIds, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetByShipmentIds(ShipmentIds, tenant);
            List<CargoTrackingShipmentList> shipmetsLists = GetIqueryableList(shipments).ToList();
            return shipmetsLists;
        }

        public CargoTrackingShipmentList GetCargoTrackingShipmentByEntityId(string shipmentId,int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            CargoTrackingShipment shipment = repo.GetCargoTrackingShipmentByEntityId(shipmentId, tenant);
            return CreateCargoTrackingShipmentListInstanceFromPOCO(shipment);
        }

        private CargoTrackingShipmentList CreateCargoTrackingShipmentListInstanceFromPOCO(CargoTrackingShipment shipment)
        {
            return new CargoTrackingShipmentList()
            {
                Id = shipment.Id,
                Tenant = shipment.Tenant,
                EntityId = shipment.EntityId,
                SecurityKey = shipment.SecurityKey,
                ForwardingShipmentHeaderId = shipment.ForwardingShipmentHeaderId,
                CustomsShipmentHeaderId = shipment.CustomsShipmentHeaderId,
                EntityType = shipment.EntityType,
                CurrentMilestoneCode = shipment.CurrentMilestoneCode,
                CurrentMilestoneDate = shipment.CurrentMilestoneDate,
                CustomerId = shipment.CustomerId,
                TransportModeId = shipment.TransportModeId,
                Master = shipment.Master,
                House = shipment.House,
                ShipmentNumber = shipment.ShipmentNumber,
                FromPortId = shipment.FromPortId,
                ToPortId = shipment.ToPortId,
                ShipperId = shipment.ShipperId,
                ConsigneeId = shipment.ConsigneeId,
                GrossWeight = shipment.GrossWeight,
                Volume = shipment.Volume,
                PickupDone = shipment.PickupDone,
                ClearanceDone = shipment.ClearanceDone,
                PickupDate = shipment.PickupDate,
                PickupEstimationDate = shipment.PickupEstimationDate,
                FromWarehouseEstimationDate = shipment.FromWarehouseEstimationDate,
                FromWarehouseDate = shipment.FromWarehouseDate,
                FromWarehouseNotes = shipment.FromWarehouseNotes,
                ToWarehouseEstimationDate = shipment.ToWarehouseEstimationDate,
                ToWarehouseDate = shipment.ToWarehouseDate,
                ToWarehouseNotes = shipment.ToWarehouseNotes,
                DepartureEstimationDate = shipment.DepartureEstimationDate,
                DepartureDate = shipment.DepartureDate,
                DepartureDone = shipment.DepartureDone,
                ArrivalEstimationDate = shipment.ArrivalEstimationDate,
                ArrivalDate = shipment.ArrivalDate,
                DeliveredEstimationDate = shipment.DeliveredEstimationDate,
                DeliveredDate = shipment.DeliveredDate,
                ClearanceDate = shipment.ClearanceDate,
                CreateDate = shipment.CreateDate,
                DirectionId = shipment.DirectionId,
                CustomerReference = shipment.CustomerReference,
                AssignedCustomsAgentDate = shipment.AssignedCustomsAgentDate,
                AssignedCustomsAgentDone = shipment.AssignedCustomsAgentDone,
                AssignedCustomsAgentEstDate = shipment.AssignedCustomsAgentEstDate,
                AssignedCustomsAgentExcReason = shipment.AssignedCustomsAgentExcReason,
                AssignedCustomsAgentNotes = shipment.AssignedCustomsAgentNotes,
                ShipmentLevelCode = shipment.ShipmentLevelCode,
                AssignedTruckerDate = shipment.AssignedTruckerDate,
                AssignedTruckerDone = shipment.AssignedTruckerDone,
                GrossWeightUnitCode = shipment.GrossWeightUnitCode,
                CustomsPaymentDate = shipment.CustomsPaymentDate,
                DeliveryEstimationDate = shipment.DeliveryEstimationDate,
                DeliveryDate =shipment.DeliveryDate,
                DeliveryNotes = shipment.DeliveryNotes,
                AssignedTruckerEstimationDate = shipment.AssignedTruckerEstimationDate,
                AssignedTruckerNotes = shipment.AssignedTruckerNotes,

            };
        }

        public List<CargoTrackingShipmentList> GetShipments(int pageIndex, int pageSize, List<string> ShipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            shipments = SortShipments(shipmentFilters, shipments);
            List<CargoTrackingShipmentList> shipmentsLists = GetPageOfShipmentsLists(pageIndex, pageSize, shipments);

            shipmentsLists = AddMilstonesToShipments(shipmentsLists);

            return shipmentsLists;
        }
        public IQueryable<CargoTrackingShipmentList> GetShipments(List<string> ShipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);

            return shipments;
        }

        private List<CargoTrackingShipmentList> GetPageOfShipmentsLists(int pageIndex, int pageSize, IQueryable<CargoTrackingShipmentList> shipments)
        {
            List<CargoTrackingShipmentList> shipmentsLists = shipments.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return shipmentsLists;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipments(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.SortDescending == true)
                shipments = shipments.OrderByDescending(d => d.CreateDate);
            else
                shipments = shipments.OrderBy(d => d.CreateDate);
            return shipments;
        }

        public int GetShipmentsCount(List<string> ShipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            return shipments.Count();
        }

        private List<CargoTrackingShipmentList> AddMilstonesToShipments(List<CargoTrackingShipmentList> shipmetsLists)
        {
            foreach (CargoTrackingShipmentList shipment in shipmetsLists)
            {
                //if (shipment.CurrentMilestoneCode == null)
                //{
                    List<Milestone> shipmentMilestones = BuildShipmentMilstones(shipment);
                    SetMilestonesStatus(shipment, shipmentMilestones);
                //}
            }
            return shipmetsLists;
        }

        private IQueryable<CargoTrackingShipmentList> FilterShipments(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            shipments = FilterByCustomers(shipmentFilters, shipments);
            shipments = FilterTransportMode(shipmentFilters, shipments);
            shipments = FilterDirections(shipmentFilters, shipments);

            return shipments;
        }

        private IQueryable<CargoTrackingShipmentList> FilterDirections(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            List<string> directions = GetDirectionsFilterValues(shipmentFilters);
            shipments = shipments.Where(d =>
                directions.Contains(d.DirectionId)
            );
            return shipments;
        }

        private IQueryable<CargoTrackingShipmentList> FilterTransportMode(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            List<string> modes = GetTransportModesToFilterBy(shipmentFilters);
            shipments = shipments.Where(d =>
                modes.Contains(d.TransportModeId)
            );
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterByCustomers(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if(shipmentFilters.CustomersIds.Count > 0)
                shipments = shipments.Where(d =>
                            shipmentFilters.CustomersIds.Contains(d.CustomerId)
                        );
            return shipments;
        }

        private List<string> GetTransportModesToFilterBy(CargoTrackingShipmentFilters shipmentFilters)
        {
            List<string> modes = new List<string>() { "A", "O", "I"};
            if (!string.IsNullOrEmpty(shipmentFilters.TransportModeCodes))
                modes = shipmentFilters.TransportModeCodes.Split(',').ToList();
            return modes;
        }
        private List<string> GetDirectionsFilterValues(CargoTrackingShipmentFilters shipmentFilters)
        {
            string toggleFilterImportValue = "IM";
            string toggleFilterExportValue = "EX";

            string filterImportValue = "I";
            string filterExportValue = "E";
            List<string> directions = new List<string>() { filterImportValue, filterExportValue };

            if (!string.IsNullOrEmpty(shipmentFilters.DirectionCodes))
                directions = shipmentFilters.DirectionCodes
                                            .Replace(toggleFilterImportValue, filterImportValue)
                                            .Replace(toggleFilterExportValue, filterExportValue).Split(',').ToList();
            return directions;
        }
        private IQueryable<CargoTrackingShipmentList> GetShipmentsQuerableByIds(List<string> ShipmentIds, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetByShipmentIds(ShipmentIds, tenant);
            //var xx = shipments.Count();

            IQueryable<CargoTrackingShipmentList> shipmentsListQuerable = GetIqueryableList(shipments);

            //var xsx = shipmentsListQuerable.Count();

            return shipmentsListQuerable;
        }


        public void SetMilestonesStatus(CargoTrackingShipmentList shipment, List<Milestone> shipmentMilestones)
        {

            SetCurrentMilestone(shipmentMilestones);
            SetDoneMilstones(shipmentMilestones);
            SetFutureMilstoneForShipment(shipment, shipmentMilestones);
        }

        private void SetDoneMilstones(List<Milestone> shipmentMilestones )
        {
            Milestone currentMilstone = shipmentMilestones.FirstOrDefault(d => d.IsCurrent == true);
            if (currentMilstone != null)
            {
                var doneMilstones = shipmentMilestones.Where(milstone => milstone.Id < currentMilstone.Id).ToList();
                doneMilstones.ForEach(doneMilstone =>
                {
                    doneMilstone.Done = true;
                    doneMilstone.IsEstimation = false;
                });
            }
        }

        private void SetCurrentMilestone(List<Milestone> milestones)
        {
            Milestone currentMilstone = GetMostRecentNotEstimatedMilestone(milestones);
            if (currentMilstone != null)
                currentMilstone.IsCurrent = true;
        }
        private Milestone GetMostRecentNotEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation == false && s.Date != null).OrderByDescending(s => s.Date).ThenByDescending(s => s.Id).FirstOrDefault();
        }

        public List<Milestone> BuildShipmentMilstones(CargoTrackingShipmentList shipment)
        {
            List<Milestone> milestones = new List<Milestone>();


            milestones.Add(new Milestone()
            {
                Id = 2,
                Code = "Pickup",
                Name = "Pickup",
                Date = shipment.PickupDate,
                EstimationDate = shipment.PickupEstimationDate,
                Done = shipment.PickupDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.PickupDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 3,
                Code = "FromWarehouse",
                Name = "From Warehouse",
                Date = shipment.FromWarehouseDate,
                EstimationDate = shipment.FromWarehouseEstimationDate,
                Done = shipment.FromWarehouseDone,
                Notes = shipment.FromWarehouseNotes,
                IsCurrent = false,
                IsEstimation = shipment.FromWarehouseDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 4,
                Code = "Departure",
                Name = "Departure",
                Date = shipment.DepartureDate,
                EstimationDate = shipment.DepartureEstimationDate,
                Done = shipment.DepartureDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.DepartureDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 5,
                Code = "Arrival",
                Name = "Arrival",
                Date = shipment.ArrivalDate,
                EstimationDate = shipment.ArrivalEstimationDate,
                Done = shipment.ArrivalDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.ArrivalDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 6,
                Code = "ToWarehouse",
                Name = "To Warehouse",
                Date = shipment.ToWarehouseDate,
                EstimationDate = shipment.ToWarehouseEstimationDate,
                Done = shipment.ToWarehouseDone,
                Notes = shipment.ToWarehouseNotes,
                IsCurrent = false,
                IsEstimation = shipment.ToWarehouseDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 7,
                Code = "AssignedToCustomsAgent",
                Name = "Assigned To Customs Agent",
                Date = shipment.AssignedCustomsAgentDate,
                EstimationDate =shipment.AssignedCustomsAgentEstDate,
                Done = shipment.AssignedCustomsAgentDone,
                Notes = shipment.AssignedCustomsAgentNotes,
                IsCurrent = false,
                IsEstimation = shipment.AssignedCustomsAgentDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 8,
                Code = "CustomsProcess",
                Name = "Customs Process",
                //Date = Shipment.process,
                EstimationDate = null,
                //Done = Shipment.CustomsPaymentDone,
                Notes = null,
                IsCurrent = false,
                //IsEstimation = !Shipment.CustomsPaymentDone
            });

            milestones.Add(new Milestone()
            {
                Id = 9,
                Code = "CustomsPayment",
                Name = "Customs Payment",
                Date = shipment.CustomsPaymentDate,
                EstimationDate = null,
                Done = shipment.CustomsPaymentDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.CustomsPaymentDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 10,
                Code = "Clearance",
                Name = "Clearance",
                Date = shipment.ClearanceDate,
                EstimationDate = null,
                Done = shipment.ClearanceDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.ClearanceDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 11,
                Code = "AssignedToTrucker",
                Name = "Assigned To Trucker",
                Date = shipment.AssignedTruckerDate,
                EstimationDate = shipment.AssignedTruckerEstimationDate,
                Done = shipment.AssignedTruckerDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.AssignedTruckerDone != true
            });
            milestones.Add(new Milestone()
            {
                Id = 12,
                Code = "DeliveryOut",
                Name = "Delivery Out",
                Date = shipment.DeliveryDate,
                EstimationDate = shipment.DeliveryEstimationDate,
                Done = shipment.DeliveryDone,
                Notes = shipment.DeliveryNotes,
                IsCurrent = false,
                IsEstimation = !shipment.DeliveryDone
            });
            milestones.Add(new Milestone()
            {
                Id = 13,
                Code = "Delivered",
                Name = "Delivered",
                Date = shipment.DeliveredDate,
                EstimationDate = shipment.DeliveredEstimationDate,
                Done = shipment.DeliveredDone,
                Notes = null,
                IsCurrent = false,
                IsEstimation = shipment.DeliveredEstimationDate != null
            });
            milestones.Add(new Milestone()
            {
                Id = 14,
                Code = "Invoiced",
                Name = "Invoiced",
                //Date = Shipment.invoi,
                //EstimationDate = Shipment.DeliveredEstimationDate,
                //Done = Shipment.DeliveredDone,
                Notes = null,
                IsCurrent = false,
                //IsEstimation = !Shipment.DeliveredDone
            });

            milestones = milestones
                            .OrderByDescending(s => s.IsEstimation == true ? s.EstimationDate : s.Date)
                            .ThenByDescending(s => s.Id)
                            .ToList();


            return milestones;
        }

        private void SetFutureMilstoneForShipment(CargoTrackingShipmentList Shipment, List<Milestone> milestones)
        {
            Milestone futureMilstone = GetMostRecentEstimatedMilestone(milestones);

            if (futureMilstone != null)
            {
                Shipment.FutureMilstoneCode = futureMilstone.Code;
                Shipment.FutureMilstoneName = futureMilstone.Name;
                Shipment.FutureMilstoneDate = futureMilstone.EstimationDate;
            }
        }

        private static Milestone GetMostRecentEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation == true && s.EstimationDate != null)
                                                        .OrderByDescending(s => s.Date).ThenByDescending(s => s.Id)
                                                        .FirstOrDefault();
        }

        public CargoTrackingShipmentList GetShipment(string SecurityKey, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(tenant);

            IQueryable<CargoTrackingShipment> shipments = (from shipment in context.CargoTrackingShipments
                                                           where shipment.Tenant == tenant && shipment.SecurityKey == SecurityKey
                                                           select shipment);

            var shipmentsLists = GetIqueryableList(shipments);
            return shipmentsLists.FirstOrDefault();
        }

        public List<string> GetShipmentPublicReferences(string SecurityKey, int tenant)
        {
            CargoTrackingShipment shipment = GetShipmentBySecurityKey(SecurityKey, tenant);

            List<string> references = GetPublicReferencesForShipment(shipment);

            return references;
        }

        private List<string> GetPublicReferencesForShipment(CargoTrackingShipment shipment)
        {
            CargoTrackingShipmentRepository repository = new CargoTrackingShipmentRepository(shipment.Tenant);
            List<string> references = repository.GetPublicReferencesForShipment(shipment.EntityId, shipment.Tenant);
            return references;
        }

        private CargoTrackingShipment GetShipmentBySecurityKey(string SecurityKey, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(tenant);
            var shipmets = repo.GetBySecurityKey(SecurityKey, tenant);
            return shipmets.FirstOrDefault();
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
