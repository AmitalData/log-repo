using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.DataContracts;
using Logitude.CargoTracking.Def.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentListQueryService
    {
        const string OrderType = "O";
        private IQueryable<CargoTrackingShipmentList> GetIqueryableList(IQueryable<CargoTrackingShipment> iQueryable)
        {
            IQueryable<CargoTrackingPortList> ports = GetPorts();
            IQueryable<CargoTrackingShipmentList> query = (from shipment in iQueryable
                                                           join fromPort in ports on shipment.FromPortId equals fromPort.Id
                                                           join toPort in ports on shipment.ToPortId equals toPort.Id
                                                           join customer in context.CargoTrackingCards on shipment.CustomerId equals customer.Id

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
                                                               AssignedCustomsAgentNotes = shipment.AssignedCustomsAgentNotes,
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
                                                               PackagesQuantity = shipment.PackagesQuantity,
                                                               ConsigneeName = shipment.ConsigneeName,
                                                               CurrentMilestoneExceptions = shipment.CurrentMilestoneExceptions,
                                                               ForwardingShipmentNumber = shipment.ForwardingShipmentNumber,
                                                               ForwardingShipmentLevelCode = shipment.ForwardingShipmentLevelCode,
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
                                                               ShipperName = shipment.ShipperName,
                                                               ForwardingMaster = shipment.ForwardingMaster,
                                                               ForwardingHouse = shipment.ForwardingHouse,
                                                               ImportManifest = shipment.ImportManifest,
                                                               GoodsClassificationDate = shipment.GoodsClassificationDate,
                                                               GoodsClassificationDone = shipment.GoodsClassificationDone,
                                                               GoodsClassificationNotes = shipment.GoodsClassificationNotes,
                                                               GoodsClassificationEstDate = shipment.GoodsClassificationEstDate,

                                                               DocumentInspectionDate = shipment.DocumentInspectionDate,
                                                               DocumentInspectionEstDate = shipment.DocumentInspectionEstDate,
                                                               DocumentInspectionDone = shipment.DocumentInspectionDone,
                                                               DocumentInspectionNotes = shipment.DocumentInspectionNotes,

                                                               BookingDate = shipment.BookingDate,
                                                               BookingEstimationDate = shipment.BookingEstimationDate,
                                                               BookingDone = shipment.BookingDone,
                                                               BookingExceptionReason = shipment.BookingExceptionReason,
                                                               BookingNotes = shipment.BookingNotes,

                                                               GatepassArrivedDate = shipment.GatepassArrivedDate,
                                                               GatepassArrivedDone = shipment.GatepassArrivedDone,
                                                               GatepassArrivedEstDate = shipment.GatepassArrivedEstDate,
                                                               GatepassArrivedNotes = shipment.GatepassArrivedNotes,

                                                               PaymentRequiredDone = shipment.PaymentRequiredDone,
                                                               PaymentRequiredEstimationDate = shipment.PaymentRequiredEstimationDate,
                                                               PaymentRequiredDate = shipment.PaymentRequiredDate,
                                                               PaymentRequiredNotes = shipment.PaymentRequiredNotes,

                                                               PaymentReceivedDone = shipment.PaymentReceivedDone,
                                                               PaymentReceivedEstomationDate = shipment.PaymentReceivedEstomationDate,
                                                               PaymentReceivedDate = shipment.PaymentReceivedDate,
                                                               PaymentReceivedNotes = shipment.PaymentReceivedNotes,

                                                               ShipmentTypeCode = shipment.ShipmentTypeCode,
                                                               CustomsPaymentDone = shipment.CustomsPaymentDone,
                                                               PoNumber = shipment.PoNumber,
                                                               SupplyDateTime = shipment.SupplyDateTime,
                                                               DescriptionOfGoods = shipment.DescriptionOfGoods,



                                                               ATAETASortingField = shipment.ArrivalDate != null ? shipment.ArrivalDate: shipment.ArrivalEstimationDate,
                                                               ATDETDSortingField = shipment.DepartureDate != null ? shipment.DepartureDate : shipment.DepartureEstimationDate,
                                                               CreatedDone = shipment.CreatedDone,
                                                               DeliveredDone = shipment.DeliveredDone,
                                                               DeliveryDone = shipment.DeliveryDone,
                                                               FromWarehouseDone = shipment.FromWarehouseDone,
                                                               ToWarehouseDone = shipment.ToWarehouseDone,
                                                               CustomsClearanceDate = shipment.CustomsClearanceDate,
                                                               DeclarationDate = shipment.DeclarationDate,
                                                               WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate,
                                                               WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate,
                                                               
                                                           });
            return query;
        }

        private IQueryable<CargoTrackingPortList> GetPorts()
        {
            return (from p in context.CargoTrackingPorts
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

                    ContainersNumbers = poco.ContainersNumbers,

                    PackagesQuantity = poco.PackagesQuantity,

                    CustomerReference = poco.CustomerReference,
                    ShipmentLevelCode = poco.ShipmentLevelCode,

                    BookingDate = poco.BookingDate,
                    BookingEstimationDate = poco.BookingEstimationDate,
                    BookingDone = poco.BookingDone,
                    BookingExceptionReason = poco.BookingExceptionReason,
                    BookingNotes = poco.BookingNotes,

                    AssignedTruckerDate = poco.AssignedTruckerDate,
                    AssignedTruckerDone = poco.AssignedTruckerDone,
                    AssignedTruckerEstimationDate = poco.AssignedTruckerEstimationDate,
                    AssignedTruckerNotes = poco.AssignedTruckerNotes,

                    AssignedCustomsAgentDate = poco.AssignedCustomsAgentDate,
                    AssignedCustomsAgentDone = poco.AssignedCustomsAgentDone,
                    AssignedCustomsAgentEstDate = poco.AssignedCustomsAgentEstDate,
                    AssignedCustomsAgentExcReason = poco.AssignedCustomsAgentExcReason,
                    AssignedCustomsAgentNotes = poco.AssignedCustomsAgentNotes,

                    DirectionId = poco.DirectionId,
                    DeliveryDone = poco.DeliveryDone,

                    DeliveryDate = poco.DeliveryDate,

                    DeliveryEstimationDate = poco.DeliveryEstimationDate,

                    DeliveryNotes = poco.DeliveryNotes,

                    DeliveryExceptionReason = poco.DeliveryExceptionReason,

                    GrossWeightUnitCode = poco.GrossWeightUnitCode,

                    GoodsClassificationDate = poco.GoodsClassificationDate,
                    GoodsClassificationDone = poco.GoodsClassificationDone,
                    GoodsClassificationNotes = poco.GoodsClassificationNotes,
                    GoodsClassificationEstDate = poco.GoodsClassificationEstDate,

                    DocumentInspectionDate = poco.DocumentInspectionDate,
                    DocumentInspectionEstDate = poco.DocumentInspectionEstDate,
                    DocumentInspectionDone = poco.DocumentInspectionDone,
                    DocumentInspectionNotes = poco.DocumentInspectionNotes,

                    GatepassArrivedDate = poco.GatepassArrivedDate,
                    GatepassArrivedDone = poco.GatepassArrivedDone,
                    GatepassArrivedEstDate = poco.GatepassArrivedEstDate,
                    GatepassArrivedNotes = poco.GatepassArrivedNotes,

                    PaymentRequiredDone = poco.PaymentRequiredDone,
                    PaymentRequiredEstimationDate = poco.PaymentRequiredEstimationDate,
                    PaymentRequiredDate = poco.PaymentRequiredDate,
                    PaymentRequiredNotes = poco.PaymentRequiredNotes,

                    PaymentReceivedDone = poco.PaymentReceivedDone,
                    PaymentReceivedEstomationDate = poco.PaymentReceivedEstomationDate,
                    PaymentReceivedDate = poco.PaymentReceivedDate,
                    PaymentReceivedNotes = poco.PaymentReceivedNotes,

                };
            if (list != null)
            {
                list = FillShipmentListExtraFileds(list);

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

            //foreach (CargoTrackingShipmentList shipment in shipments)
            //{
            //    if (shipment.CurrentMilestoneCode == null)
            //    {
            //        //List<Milestone> shipmentMilestones = BuildShipmentMilstones(shipment);
            //        //SetMilestonesStatus(shipment, shipmentMilestones);
            //    }
            //}

            return shipments;
        }

        public List<CargoTrackingShipmentList> GetShipmentsByIds(List<string> ShipmentIds, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetFilteredShipmentsByIds(ShipmentIds, tenant);
            List<CargoTrackingShipmentList> shipmetsLists = GetIqueryableList(shipments).ToList();
            return shipmetsLists;
        }

        public CargoTrackingShipmentList GetCargoTrackingShipmentByEntityId(string shipmentId, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            CargoTrackingShipment shipment = repo.GetCargoTrackingShipmentByEntityId(shipmentId, tenant);
            if (shipment == null) throw new Exception("No shipment found");
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
                CurrentMilestoneExceptions = shipment.CurrentMilestoneExceptions,
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
                DeliveryDate = shipment.DeliveryDate,
                DeliveryNotes = shipment.DeliveryNotes,
                AssignedTruckerEstimationDate = shipment.AssignedTruckerEstimationDate,
                AssignedTruckerNotes = shipment.AssignedTruckerNotes,

                GoodsClassificationDate = shipment.GoodsClassificationDate,
                GoodsClassificationDone = shipment.GoodsClassificationDone,
                GoodsClassificationNotes = shipment.GoodsClassificationNotes,
                GoodsClassificationEstDate = shipment.GoodsClassificationEstDate,

                DocumentInspectionDate = shipment.DocumentInspectionDate,
                DocumentInspectionEstDate = shipment.DocumentInspectionEstDate,
                DocumentInspectionDone = shipment.DocumentInspectionDone,
                DocumentInspectionNotes = shipment.DocumentInspectionNotes,

                BookingDate = shipment.BookingDate,
                BookingEstimationDate = shipment.BookingEstimationDate,
                BookingDone = shipment.BookingDone,
                BookingExceptionReason = shipment.BookingExceptionReason,
                BookingNotes = shipment.BookingNotes,

                GatepassArrivedDate = shipment.GatepassArrivedDate,
                GatepassArrivedDone = shipment.GatepassArrivedDone,
                GatepassArrivedEstDate = shipment.GatepassArrivedEstDate,
                GatepassArrivedNotes = shipment.GatepassArrivedNotes,

                PaymentRequiredDone = shipment.PaymentRequiredDone,
                PaymentRequiredEstimationDate = shipment.PaymentRequiredEstimationDate,
                PaymentRequiredDate = shipment.PaymentRequiredDate,
                PaymentRequiredNotes = shipment.PaymentRequiredNotes,

                PaymentReceivedDone = shipment.PaymentReceivedDone,
                PaymentReceivedEstomationDate = shipment.PaymentReceivedEstomationDate,
                PaymentReceivedDate = shipment.PaymentReceivedDate,
                PaymentReceivedNotes = shipment.PaymentReceivedNotes,
            };
        }

        public List<CargoTrackingShipmentList> GetCargoTrackingShipments(int pageIndex, int pageSize, List<string> shipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            bool hasSearchKeyWithNoResults = !string.IsNullOrWhiteSpace(shipmentFilters.SearchText) && shipmentIds.Count == 0;
            if (hasSearchKeyWithNoResults)
                return new List<CargoTrackingShipmentList>();

            List<CargoTrackingShipmentList> shipmentsLists = GetFilteredSortedShipmentsByIds(pageIndex, pageSize, shipmentIds, shipmentFilters);

            return shipmentsLists;
        }
        public List<CargoTrackingShipmentList> GetFilteredShipmentsWithMilstones(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            List<CargoTrackingShipmentList> shipmentsLists = GetFilteredSortedShipments(pageIndex, pageSize, shipmentFilters);

            return shipmentsLists;
        }
        private List<CargoTrackingShipmentList> GetFilteredSortedShipmentsByIds(int pageIndex, int pageSize, List<string> shipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(shipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            shipments = SortShipments(shipmentFilters, shipments);
            List<CargoTrackingShipmentList> shipmentsLists = GetPageOfShipmentsLists(pageIndex, pageSize, shipments);
            return shipmentsLists;
        }
        private List<CargoTrackingShipmentList> GetFilteredSortedShipments(int pageIndex, int pageSize, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetQueryableShipmentsBySearchText(shipmentFilters.SearchText, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            shipments = SortShipments(shipmentFilters, shipments);

            List<CargoTrackingShipmentList> shipmentsLists = GetPageOfShipmentsLists(pageIndex, pageSize, shipments);
            return shipmentsLists;
        }
        public IQueryable<CargoTrackingShipmentList> GetShipments(List<string> ShipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);

            return shipments;
        }
        public IQueryable<CargoTrackingShipmentList> GetShipments(CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetQueryableShipmentsBySearchText(shipmentFilters.SearchText, shipmentFilters.Tenant);

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
            if (shipmentFilters.SortDescending == "ASC")
                shipments = SortShipmentsAscending(shipmentFilters, shipments);
            else
                shipments = SortShipmentsDescending(shipmentFilters, shipments);
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipmentsDescending(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.SortFieldName == "CMD")
            {
                shipments = shipments.OrderByDescending(d => d.CurrentMilestoneDate);
            }
            else if (shipmentFilters.SortFieldName == "ATA")
            {
                shipments = shipments.OrderByDescending(d => d.ATAETASortingField);
            }
            else if (shipmentFilters.SortFieldName == "ATD")
            {
                shipments = shipments.OrderByDescending(d => d.ATDETDSortingField);
            }
            else
            {
                shipments = shipments.OrderByDescending(d => d.CurrentMilestoneDate);
            }
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipmentsAscending(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.SortFieldName == "CMD")
            {
                shipments = shipments.OrderBy(d => d.CurrentMilestoneDate);
            }
            else if (shipmentFilters.SortFieldName == "ATA")
            {
                shipments = shipments.OrderBy(d => d.ATAETASortingField);
            }
            else if (shipmentFilters.SortFieldName == "ATD")
            {
                shipments = shipments.OrderBy(d => d.ATDETDSortingField);
            }
            else
            {
                shipments = shipments.OrderBy(d => d.CurrentMilestoneDate);
            }
            return shipments;
        }

        public int GetShipmentsCount(List<string> ShipmentIds, CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            return shipments.Count();
        }
        public int GetShipmentsCount(CargoTrackingShipmentFilters shipmentFilters)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetQueryableShipmentsBySearchText(shipmentFilters.SearchText, shipmentFilters.Tenant);

            shipments = FilterShipments(shipmentFilters, shipments);
            return shipments.Count();
        }


        private IQueryable<CargoTrackingShipmentList> FilterShipments(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            shipments = FilterByCustomers(shipmentFilters, shipments);
            shipments = FilterByMileStones(shipmentFilters, shipments);
            shipments = FilterTransportMode(shipmentFilters, shipments);
            shipments = FilterDirections(shipmentFilters, shipments);
            shipments = FilterShipmentsWhichMoreFilter(shipmentFilters, shipments);
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
            if (shipmentFilters.CustomersIds.Count > 0)
                shipments = shipments.Where(d =>
                            shipmentFilters.CustomersIds.Contains(d.CustomerId)
                        );
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterShipmentsWhichMoreFilter(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.HasException)
                shipments = shipments.Where(d => d.CurrentMilestoneExceptions != null);
            if (shipmentFilters.OrdersOnly)
                shipments = shipments.Where(d => d.EntityType == OrderType);
            if (shipmentFilters.EstimatedArrivalOnly)
                shipments = shipments.Where(d => d.ArrivalEstimationDate != null && d.ArrivalDate == null);
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterByMileStones(CargoTrackingShipmentFilters shipmentFilters, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentFilters.MilestonesCodes.Count > 0)
                shipments = shipments.Where(d =>
                            shipmentFilters.MilestonesCodes.Contains(d.CurrentMilestoneCode)
                        );
            return shipments;
        }

        private List<string> GetTransportModesToFilterBy(CargoTrackingShipmentFilters shipmentFilters)
        {
            List<string> modes = new List<string>() { "A", "O", "I" };
            if (!string.IsNullOrEmpty(shipmentFilters.TransportModeCodes))
                modes = shipmentFilters.TransportModeCodes.Split(',').ToList();
            return modes;
        }
        private List<string> GetDirectionsFilterValues(CargoTrackingShipmentFilters shipmentFilters)
        {
            string toggleFilterImportValue = "IM";
            string toggleFilterExportValue = "EX";

            string filterImportValue = "I";
            string filterCustomImportValue = "C";
            string filterExportValue = "E";
            string filterDrop = "R";
            List<string> directions = new List<string>() { filterImportValue, filterCustomImportValue, filterExportValue, filterDrop };

            if (!string.IsNullOrEmpty(shipmentFilters.DirectionCodes))
            {
                directions = new List<string>();
                if (shipmentFilters.DirectionCodes.Contains(toggleFilterImportValue))
                    directions.AddRange(new List<string>() { filterImportValue, filterCustomImportValue });

                if (shipmentFilters.DirectionCodes.Contains(toggleFilterExportValue))
                    directions.AddRange(new List<string>() { filterExportValue });
                if (shipmentFilters.DirectionCodes.Contains(filterDrop))
                    directions.AddRange(new List<string>() { filterDrop });

            }

            return directions;
        }
        private IQueryable<CargoTrackingShipmentList> GetShipmentsQuerableByIds(List<string> ShipmentIds, int tenant)
        {
            CargoTrackingShipmentRepository repo = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = repo.GetByShipmentIds(ShipmentIds, tenant);

            IQueryable<CargoTrackingShipmentList> shipmentsListQuerable = GetIqueryableList(shipments);
            return shipmentsListQuerable;
        }
        private IQueryable<CargoTrackingShipmentList> GetQueryableShipmentsBySearchText(string searchKey, int tenant)
        {
            CargoTrackingShipmentRepository shipmentRepository = new CargoTrackingShipmentRepository(context);
            IQueryable<CargoTrackingShipment> shipments = shipmentRepository.GetFilteredShipmentsSearchKeyword(searchKey, tenant);

            IQueryable<CargoTrackingShipmentList> shipmentsListQuerable = GetIqueryableList(shipments);
            return shipmentsListQuerable;
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




        private void SetFutureMilstoneForShipment(CargoTrackingShipmentList cargoShipmentPM, List<Milestone> milestone)
        {
            Milestone futureMilstone = GetMostRecentEstimatedMilestone(milestone);

            if (futureMilstone != null)
            {
                cargoShipmentPM.FutureMilstoneCode = futureMilstone.Code;
                cargoShipmentPM.FutureMilstoneName = futureMilstone.Name;
                cargoShipmentPM.FutureMilstoneDate = futureMilstone.EstimationDate;
            }
        }

        private static Milestone GetMostRecentEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation == true && s.EstimationDate != null)
                                                        .OrderByDescending(s => s.Weight)
                                                        .FirstOrDefault();
        }
    }


    //public class Milestone
    //{
    //    public int Id { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public string Notes { get; set; }
    //    public DateTime? Date { get; set; }
    //    public DateTime? EstimationDate { get; set; }
    //    public bool? Done { get; set; }
    //    public bool? IsEstimation { get; set; }
    //    public bool? IsCurrent { get; set; }

    //}
    public class CargoTrackingShipmentWithMilestones
    {
        public List<Milestone> Milestones { get; set; }
        public CargoTrackingShipmentList ShipmentList { get; set; }

    }

}
