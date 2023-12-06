using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.DataContracts;
using Logitude.CargoTracking.Def.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Common;
using Simplog.Server.Infrastructure;
using System.Configuration;
using System.Runtime.Remoting.Contexts;
using Logitude.CargoTracking.Data.Model;
using System.Globalization;


namespace Logitude.CargoTracking.Data.EntityListQueryServices
{

    public partial class CargoTrackingShipmentListQueryService
    {
        const string OrderType = "O";
        const string ImportDirectionCode = "I";
        const string CustomeImportDirectionCode = "C";
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
                                                               CustomerReference = shipment.EntityType == OrderType ? shipment.CustomerReference + "," + shipment.BookingNotes + "," + shipment.PoNumber : shipment.CustomerReference,
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

                                                               InvoicedDate = shipment.InvoicedDate,
                                                               InvoicedDone = shipment.InvoicedDone,
                                                               InvoicedExceptionReason = shipment.InvoicedExceptionReason,
                                                               InvoicedNotes = shipment.InvoicedNotes,

                                                               ATAETASortingField = shipment.ArrivalDate != null ? shipment.ArrivalDate : shipment.ArrivalEstimationDate,
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
                                                               IsOperationalClosed = shipment.IsOperationalClosed,
                                                               SHOHouse = shipment.SHOHouse,
                                                               ChargeableWeightInKG = shipment.ChargeableWeightInKG,
                                                               ChargeableWeight = shipment.ChargeableWeight,
                                                               ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode,
                                                               IncotermName = shipment.IncotermName

                                                           });
            return query;
        }

        private IQueryable<CargoTrackingShipmentList> GetIqueryableListWithouJoin(int tenant)
        {
            IQueryable<CargoTrackingShipmentList> query =
                context.CargoTrackingShipments.Where(shipment =>
                    shipment.Tenant == tenant                                                                
                    && shipment.IsMainRecord == true)
                .Select(shipment => new CargoTrackingShipmentList()
                {
                    Id = shipment.Id,
                    Tenant = shipment.Tenant,
                    CustomerEnglishName = "",// card.EnglishName,
                    CustomerLocalName = "",//card.LocalName,
                    ShipmentNumber = shipment.ShipmentNumber,
                    CustomerReference = shipment.EntityType == OrderType ? shipment.CustomerReference + "," + shipment.BookingNotes + "," + shipment.PoNumber : shipment.CustomerReference,
                    House = shipment.House,
                    ChargeableWeightInKG = shipment.ChargeableWeightInKG,
                    ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode,
                    ChargeableWeight = shipment.ChargeableWeight,

                    PackagesQuantity = shipment.PackagesQuantity,
                    CurrentMilestoneDate = shipment.CurrentMilestoneDate,
                    TransportModeId = shipment.TransportModeId,
                    FromPortId = shipment.FromPortId,
                    ToPortId = shipment.ToPortId,
                    CreateDate = shipment.CreateDate,
                    CustomerId = shipment.CustomerId,
                    EntityId = shipment.EntityId,
                    EntityType = shipment.EntityType,
                    BookingNotes = shipment.BookingNotes,
                    ATAETASortingField = shipment.ArrivalDate != null ? shipment.ArrivalDate : shipment.ArrivalEstimationDate,
                    ATDETDSortingField = shipment.DepartureDate != null ? shipment.DepartureDate : shipment.DepartureEstimationDate,
                    PoNumber = shipment.PoNumber,
                    NumberOfPackages = shipment.PackagesQuantity,
                    ConsigneeName = shipment.ConsigneeName,
                    ForwardingShipmentLevelCode = shipment.ForwardingShipmentLevelCode,
                    ShipperName = shipment.ShipperName,
                    ForwardingMaster = shipment.ForwardingMaster,
                    ForwardingHouse = shipment.ForwardingHouse,
                    DirectionId = shipment.DirectionId,
                    GrossWeight = shipment.GrossWeight,
                    GrossWeightUnitCode = shipment.GrossWeightUnitCode,
                    ArrivalDate = shipment.ArrivalDate,
                    ShipmentLevelCode = shipment.ShipmentLevelCode,
                    ForwardingShipmentHeaderId = shipment.ForwardingShipmentHeaderId,
                    Master = shipment.Master,
                    ArrivalEstimationDate = shipment.ArrivalEstimationDate,
                    DepartureDate = shipment.DepartureDate,
                    DepartureEstimationDate = shipment.DepartureEstimationDate,
                    CurrentMilestoneExceptions = shipment.CurrentMilestoneExceptions,
                    IsOperationalClosed = shipment.IsOperationalClosed,
                    SecurityKey = shipment.SecurityKey,
                    CurrentMilestoneCode = shipment.CurrentMilestoneCode,

                    //ForwardingShipmentNumber = shipment.ForwardingShipmentNumber,

                    //CustomsShipmentHeaderId = shipment.CustomsShipmentHeaderId,

                    //ShipperId = shipment.ShipperId,
                    //DeliveredDate = shipment.DeliveredDate,
                    //ConsigneeId = shipment.ConsigneeId,
                    //Volume = shipment.Volume,
                    //PickupDone = shipment.PickupDone,
                    //ClearanceDone = shipment.ClearanceDone,
                    //PickupDate = shipment.PickupDate,
                    //PickupEstimationDate = shipment.PickupEstimationDate,
                    //FromWarehouseEstimationDate = shipment.FromWarehouseEstimationDate,
                    //ToWarehouseEstimationDate = shipment.ToWarehouseEstimationDate,
                    //DepartureDone = shipment.DepartureDone,
                    //DeliveredEstimationDate = shipment.DeliveredEstimationDate,
                    //ClearanceDate = shipment.ClearanceDate,
                    //AssignedCustomsAgentDate = shipment.AssignedCustomsAgentDate,
                    //AssignedCustomsAgentDone = shipment.AssignedCustomsAgentDone,
                    //AssignedCustomsAgentEstDate = shipment.AssignedCustomsAgentEstDate,
                    //AssignedCustomsAgentExcReason = shipment.AssignedCustomsAgentExcReason,
                    //AssignedCustomsAgentNotes = shipment.AssignedCustomsAgentNotes,
                    //AssignedTruckerDate = shipment.AssignedTruckerDate,
                    //AssignedTruckerDone = shipment.AssignedTruckerDone,

                    //ArrivalDone = shipment.ArrivalDone,
                    //CustomsPaymentDate = shipment.CustomsPaymentDate,
                    //ContainersNumbers = shipment.ContainersNumbers,
                    //FromWarehouseDate = shipment.FromWarehouseDate,
                    //FromWarehouseNotes = shipment.FromWarehouseNotes,
                    //ToWarehouseDate = shipment.ToWarehouseDate,
                    //ToWarehouseNotes = shipment.ToWarehouseNotes,
                    //DeliveryEstimationDate = shipment.DeliveryEstimationDate,
                    //DeliveryDate = shipment.DeliveryDate,
                    //DeliveryNotes = shipment.DeliveryNotes,
                    //AssignedTruckerEstimationDate = shipment.AssignedTruckerEstimationDate,
                    //AssignedTruckerNotes = shipment.AssignedTruckerNotes,


                    //ImportManifest = shipment.ImportManifest,
                    //GoodsClassificationDate = shipment.GoodsClassificationDate,
                    //GoodsClassificationDone = shipment.GoodsClassificationDone,
                    //GoodsClassificationNotes = shipment.GoodsClassificationNotes,
                    //GoodsClassificationEstDate = shipment.GoodsClassificationEstDate,

                    //DocumentInspectionDate = shipment.DocumentInspectionDate,
                    //DocumentInspectionEstDate = shipment.DocumentInspectionEstDate,
                    //DocumentInspectionDone = shipment.DocumentInspectionDone,
                    //DocumentInspectionNotes = shipment.DocumentInspectionNotes,

                    //BookingDate = shipment.BookingDate,
                    //BookingEstimationDate = shipment.BookingEstimationDate,
                    //BookingDone = shipment.BookingDone,
                    //BookingExceptionReason = shipment.BookingExceptionReason,

                    //GatepassArrivedDate = shipment.GatepassArrivedDate,
                    //GatepassArrivedDone = shipment.GatepassArrivedDone,
                    //GatepassArrivedEstDate = shipment.GatepassArrivedEstDate,
                    //GatepassArrivedNotes = shipment.GatepassArrivedNotes,

                    //PaymentRequiredDone = shipment.PaymentRequiredDone,
                    //PaymentRequiredEstimationDate = shipment.PaymentRequiredEstimationDate,
                    //PaymentRequiredDate = shipment.PaymentRequiredDate,
                    //PaymentRequiredNotes = shipment.PaymentRequiredNotes,

                    //PaymentReceivedDone = shipment.PaymentReceivedDone,
                    //PaymentReceivedEstomationDate = shipment.PaymentReceivedEstomationDate,
                    //PaymentReceivedDate = shipment.PaymentReceivedDate,
                    //PaymentReceivedNotes = shipment.PaymentReceivedNotes,

                    //ShipmentTypeCode = shipment.ShipmentTypeCode,
                    //CustomsPaymentDone = shipment.CustomsPaymentDone,
                    //SupplyDateTime = shipment.SupplyDateTime,
                    //DescriptionOfGoods = shipment.DescriptionOfGoods,

                    //InvoicedDate = shipment.InvoicedDate,
                    //InvoicedDone = shipment.InvoicedDone,
                    //InvoicedExceptionReason = shipment.InvoicedExceptionReason,
                    //InvoicedNotes = shipment.InvoicedNotes,


                    //CreatedDone = shipment.CreatedDone,
                    //DeliveredDone = shipment.DeliveredDone,
                    //DeliveryDone = shipment.DeliveryDone,
                    //FromWarehouseDone = shipment.FromWarehouseDone,
                    //ToWarehouseDone = shipment.ToWarehouseDone,
                    //CustomsClearanceDate = shipment.CustomsClearanceDate,
                    //DeclarationDate = shipment.DeclarationDate,
                    //WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate,
                    //WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate,
                    //SHOHouse = shipment.SHOHouse,
                    //ChargeableWeight = shipment.ChargeableWeight,
                    //IncotermName = shipment.IncotermName

                }).Distinct();
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

        public List<CargoTrackingShipmentList> GetCargoTrackingShipments(int pageIndex, int pageSize, List<string> shipmentIds, CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            bool hasSearchKeyWithNoResults = !string.IsNullOrWhiteSpace(shipmentSearchInput.SearchText) && shipmentIds.Count == 0;
            if (hasSearchKeyWithNoResults)
                return new List<CargoTrackingShipmentList>();

            List<CargoTrackingShipmentList> shipmentsLists = GetFilteredSortedShipmentsByIds(pageIndex, pageSize, shipmentIds, shipmentSearchInput);

            return shipmentsLists;
        }

        private List<CargoTrackingShipmentList> GetFilteredSortedShipmentsByIds(int pageIndex, int pageSize, List<string> shipmentIds, CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(shipmentIds, shipmentSearchInput.Tenant);

            shipments = FilterShipments(shipmentSearchInput, shipments);
            shipments = SortShipments(shipmentSearchInput, shipments);
            List<CargoTrackingShipmentList> shipmentsLists = GetPageOfShipments(pageIndex, pageSize, shipments).ToList();
            return shipmentsLists;
        }

        private IQueryable<CargoTrackingShipmentList> AddShipmentSearch(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            return string.IsNullOrEmpty(shipmentSearchInput.SearchText) ? shipments : shipments.Where(s =>
                context.CargoTrackingShipmentSearches
                .Where(x => x.Tenant == shipmentSearchInput.Tenant && x.SearchFields.Contains(shipmentSearchInput.SearchText))
                .Select(x => x.ShipmentId)
                .Any(x => x == s.EntityId));
        }

        private IQueryable<CargoTrackingShipmentList> AddCards(IQueryable<CargoTrackingShipmentList> shipments)
        {
            return shipments.Join(context.CargoTrackingCards, shipment => shipment.CustomerId, card => card.Id, (shipment, card) => new CargoTrackingShipmentList
            {
                Id = shipment.Id,
                Tenant = shipment.Tenant,
                CustomerEnglishName = card.EnglishName,
                CustomerLocalName = card.LocalName,
                ShipmentNumber = shipment.ShipmentNumber,
                CustomerReference = shipment.CustomerReference,
                House = shipment.House,
                ChargeableWeightInKG = shipment.ChargeableWeightInKG,
                ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode,
                ChargeableWeight = shipment.ChargeableWeight,

                PackagesQuantity = shipment.PackagesQuantity,
                CurrentMilestoneDate = shipment.CurrentMilestoneDate,
                TransportModeId = shipment.TransportModeId,
                FromPortId = shipment.FromPortId,
                ToPortId = shipment.ToPortId,
                CreateDate = shipment.CreateDate,
                CustomerId = shipment.CustomerId,
                EntityId = shipment.EntityId,
                EntityType = shipment.EntityType,
                BookingNotes = shipment.BookingNotes,
                ATAETASortingField = shipment.ATAETASortingField,
                ATDETDSortingField = shipment.ATDETDSortingField,
                PoNumber = shipment.PoNumber,
                NumberOfPackages = shipment.PackagesQuantity,
                ConsigneeName = shipment.ConsigneeName,
                ForwardingShipmentLevelCode = shipment.ForwardingShipmentLevelCode,
                ShipperName = shipment.ShipperName,
                ForwardingMaster = shipment.ForwardingMaster,
                ForwardingHouse = shipment.ForwardingHouse,
                DirectionId = shipment.DirectionId,
                GrossWeight = shipment.GrossWeight,
                GrossWeightUnitCode = shipment.GrossWeightUnitCode,
                ArrivalDate = shipment.ArrivalDate,
                ShipmentLevelCode = shipment.ShipmentLevelCode,
                ForwardingShipmentHeaderId = shipment.ForwardingShipmentHeaderId,
                Master = shipment.Master,
                ArrivalEstimationDate = shipment.ArrivalEstimationDate,
                DepartureDate = shipment.DepartureDate,
                DepartureEstimationDate = shipment.DepartureEstimationDate,
                CurrentMilestoneExceptions = shipment.CurrentMilestoneExceptions,
                IsOperationalClosed = shipment.IsOperationalClosed,
                SecurityKey = shipment.SecurityKey,
                CurrentMilestoneCode = shipment.CurrentMilestoneCode,
            });
        }

        public List<CargoTrackingShipmentList> GetFilteredSortedShipments(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetIqueryableListWithouJoin(shipmentSearchInput.Tenant);
            shipments = FilterShipments(shipmentSearchInput, shipments);
            shipments = AddShipmentSearch(shipmentSearchInput, shipments);
            shipments = SortShipments(shipmentSearchInput, shipments);
            shipments = GetPageOfShipments(shipmentSearchInput.PageIndex, shipmentSearchInput.PageSize, shipments);
            shipments = AddCards(shipments);
            List<CargoTrackingShipmentList> shipmentsLists = shipments.ToList();

            List<CargoTrackingPortList> ports = GetPortFromCache();
            List<CargoTrackingTransportMode> transportModes = new CargoTrackingTransportModeListQueryService(context).GetAllFromCache();

            shipmentsLists.ForEach(shipment =>
            {
                CargoTrackingPortList fromPort = ports.FirstOrDefault(port => shipment.FromPortId == port.Id);
                CargoTrackingPortList toPort = ports.FirstOrDefault(port => shipment.ToPortId == port.Id);
                CargoTrackingTransportMode transportMode = transportModes.FirstOrDefault(t => shipment.TransportModeId == t.Id);

                shipment.ToPortCountryCode = toPort?.CountryCode;
                shipment.FromPortCountryCode = fromPort?.CountryCode;
                shipment.FromPortName = fromPort?.EnglishName;
                shipment.ToPortName = toPort?.EnglishName;
                shipment.FromPortCode = fromPort?.Code;
                shipment.ToPortCode = toPort?.Code;
                shipment.TransportModeName = transportMode?.Name;
            });

            return shipmentsLists;
        }

        private List<CargoTrackingPortList> GetPortFromCache() =>
            CacheManager.GetOrInsertNewObject<List<CargoTrackingPortList>>("ListCargoTrackingPortList", () => GetPorts().ToList());        

        public List<Model.Customer> GetShipmentsCustomers(int tenant)
        {
            CargoTrackingShipmentRepository shipmentRepository = new CargoTrackingShipmentRepository(context);
            return shipmentRepository.GetFilteredShipmentsByTenant(tenant).ToList();
        }
        
        public IQueryable<CargoTrackingShipmentList> GetShipments(List<string> ShipmentIds, CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentSearchInput.Tenant);

            shipments = FilterShipments(shipmentSearchInput, shipments);

            return shipments;
        }
        
        public IQueryable<CargoTrackingShipmentList> GetShipments(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetQueryableShipmentsBySearchText(shipmentSearchInput.SearchText, shipmentSearchInput.Tenant);

            shipments = FilterShipments(shipmentSearchInput, shipments);

            return shipments;
        }

        private IQueryable<CargoTrackingShipmentList> GetPageOfShipments(int pageIndex, int pageSize, IQueryable<CargoTrackingShipmentList> shipments)
        {
            shipments = shipments.Skip(pageIndex * pageSize).Take(pageSize);
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipments(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.SortType == "ASC")
                shipments = SortShipmentsAscending(shipmentSearchInput, shipments);
            else
                shipments = SortShipmentsDescending(shipmentSearchInput, shipments);
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipmentsDescending(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.SortFieldName == "CMD")
            {
                shipments = shipments.OrderByDescending(d => d.CurrentMilestoneDate);
            }
            else if (shipmentSearchInput.SortFieldName == "ATA")
            {
                shipments = shipments.OrderByDescending(d => d.ATAETASortingField);
            }
            else if (shipmentSearchInput.SortFieldName == "ATD")
            {
                shipments = shipments.OrderByDescending(d => d.ATDETDSortingField);
            }
            else
            {
                shipments = shipments.OrderByDescending(d => d.CurrentMilestoneDate);
            }
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> SortShipmentsAscending(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.SortFieldName == "CMD")
            {
                shipments = shipments.OrderBy(d => d.CurrentMilestoneDate);
            }
            else if (shipmentSearchInput.SortFieldName == "ATA")
            {
                shipments = shipments.OrderBy(d => d.ATAETASortingField);
            }
            else if (shipmentSearchInput.SortFieldName == "ATD")
            {
                shipments = shipments.OrderBy(d => d.ATDETDSortingField);
            }
            else
            {
                shipments = shipments.OrderBy(d => d.CurrentMilestoneDate);
            }
            return shipments;
        }

        public int GetShipmentsCount(List<string> ShipmentIds, CargoTrackingShipmentSearchInput shipmentSearchInput)
        {
            IQueryable<CargoTrackingShipmentList> shipments = GetShipmentsQuerableByIds(ShipmentIds, shipmentSearchInput.Tenant);

            shipments = FilterShipments(shipmentSearchInput, shipments);
            return shipments.Count();
        }
        
        public int GetShipmentsCount(CargoTrackingShipmentSearchInput shipmentSearchInput)
        {            
            IQueryable<CargoTrackingShipmentList> shipments = GetIqueryableListWithouJoin(shipmentSearchInput.Tenant);

            shipments = FilterShipments(shipmentSearchInput, shipments);
            shipments = AddShipmentSearch(shipmentSearchInput, shipments);

            return shipments.Count();
        }

        private IQueryable<CargoTrackingShipmentList> FilterShipments(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            shipments = FilterByCustomers(shipmentSearchInput, shipments);
            shipments = FilterByMileStones(shipmentSearchInput, shipments);
            shipments = FilterByOpenDateGreaterThan(shipmentSearchInput, shipments);
            shipments = FilterByClearanceDateGreaterThan(shipmentSearchInput, shipments);
            shipments = FilterByATADateGreaterThan(shipmentSearchInput, shipments);
            shipments = FilterByOpenDateLessThan(shipmentSearchInput, shipments);
            shipments = FilterByClearanceDateLessThan(shipmentSearchInput, shipments);
            shipments = FilterByATADateLessThan(shipmentSearchInput, shipments);
            shipments = FilterTransportMode(shipmentSearchInput, shipments);
            shipments = FilterDirections(shipmentSearchInput, shipments);
            shipments = FilterShipmentsWhichMoreFilter(shipmentSearchInput, shipments);
            shipments = FilterShipmentsDate(shipmentSearchInput, shipments, context);

            return shipments;
        }

        private IQueryable<CargoTrackingShipmentList> FilterDirections(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.DirectionCodes.Count <= 0)
                return shipments;
            if (shipmentSearchInput.DirectionCodes.Contains(ImportDirectionCode))
                shipmentSearchInput.DirectionCodes.Add(CustomeImportDirectionCode);
            shipments = shipments.Where(d =>
                shipmentSearchInput.DirectionCodes.Contains(d.DirectionId)
            );
            return shipments;
        }

        private IQueryable<CargoTrackingShipmentList> FilterTransportMode(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.TransportModeCodes.Count <= 0)
                return shipments;
            shipments = shipments.Where(d =>
                shipmentSearchInput.TransportModeCodes.Contains(d.TransportModeId)
            );
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterByCustomers(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.CustomersIds.Count > 0)
                shipments = shipments.Where(d =>
                            shipmentSearchInput.CustomersIds.Contains(d.CustomerId)
                        );
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterShipmentsWhichMoreFilter(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.HasException)
                shipments = shipments.Where(d => d.CurrentMilestoneExceptions != null);
            if (shipmentSearchInput.OrdersOnly)
                shipments = shipments.Where(d => d.EntityType == OrderType);
            if (shipmentSearchInput.EstimatedArrivalOnly)
                shipments = shipments.Where(d => d.ArrivalEstimationDate != null && d.ArrivalDate == null);
            if (shipmentSearchInput.OperationalOpenedOnly)
                shipments = shipments.Where(d => d.IsOperationalClosed == false);
            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterShipmentsDate(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments, ICargoTrackingContext context)
        {
            if (shipmentSearchInput.FromDate.HasValue || shipmentSearchInput.ToDate.HasValue)
                shipments = shipments.Join(context.CargoTrackingShipmentSearches,
                    shipment => shipment.EntityId,
                    search => search.ShipmentId,
                    (shipment, search) =>
                        new { shipment = shipment, shipmentDate = search.ShipmentDate })
                    .Where(x => 
                       (shipmentSearchInput.FromDate.HasValue && x.shipmentDate >= shipmentSearchInput.FromDate) ||
                       (shipmentSearchInput.ToDate.HasValue && x.shipmentDate <= shipmentSearchInput.ToDate)
                       )
                    .Select(x => x.shipment);

            return shipments;
        }

        private static IQueryable<CargoTrackingShipmentList> FilterByMileStones(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (shipmentSearchInput.MilestonesCodes.Count > 0)
                shipments = shipments.Where(d =>
                            shipmentSearchInput.MilestonesCodes.Contains(d.CurrentMilestoneCode)
                        );
            return shipments;
        }

        //Greater Than
        private static IQueryable<CargoTrackingShipmentList> FilterByOpenDateGreaterThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.OpenDateGreaterThan) &&
                DateTime.TryParseExact(shipmentSearchInput.OpenDateGreaterThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDateGreaterThan))
            {
                shipments = shipments.Where(d => d.CreateDate > openDateGreaterThan);
            }
            return shipments;
        }
        private static IQueryable<CargoTrackingShipmentList> FilterByClearanceDateGreaterThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.ClearanceDateGreaterThan) &&
                DateTime.TryParseExact(shipmentSearchInput.ClearanceDateGreaterThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime clearanceDateGreaterThan))
            {
                shipments = shipments.Where(d => d.ClearanceDate > clearanceDateGreaterThan);
            }
            return shipments;
        }
        private static IQueryable<CargoTrackingShipmentList> FilterByATADateGreaterThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.ATADateGreaterThan) &&
                DateTime.TryParseExact(shipmentSearchInput.ATADateGreaterThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ATADateGreaterThan))
            {
                shipments = shipments.Where(d => d.ArrivalDate > ATADateGreaterThan);
            }
            return shipments;
        }
        // Less Than
        private static IQueryable<CargoTrackingShipmentList> FilterByOpenDateLessThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.OpenDateLessThan) &&
                DateTime.TryParseExact(shipmentSearchInput.OpenDateLessThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDateLessThan))
            {
                shipments = shipments.Where(d => d.CreateDate < openDateLessThan);
            }
            return shipments;
        }
        private static IQueryable<CargoTrackingShipmentList> FilterByClearanceDateLessThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.ClearanceDateLessThan) &&
                DateTime.TryParseExact(shipmentSearchInput.ClearanceDateLessThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime clearanceDateLessThan))
            {
                shipments = shipments.Where(d => d.ClearanceDate < clearanceDateLessThan);
            }
            return shipments;
        }
        private static IQueryable<CargoTrackingShipmentList> FilterByATADateLessThan(CargoTrackingShipmentSearchInput shipmentSearchInput, IQueryable<CargoTrackingShipmentList> shipments)
        {
            if (!string.IsNullOrEmpty(shipmentSearchInput.ATADateLessThan) &&
                DateTime.TryParseExact(shipmentSearchInput.ATADateLessThan, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ATADateLessThan))
            {
                shipments = shipments.Where(d => d.ArrivalDate < ATADateLessThan);
            }
            return shipments;
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
            var result = shipmentsLists.ToList();
            CargoTrackingShipmentSearchListQueryService cargoTrackingShipmentSearchListQueryService = new CargoTrackingShipmentSearchListQueryService(context);
            cargoTrackingShipmentSearchListQueryService.AddSearchsToShipments(result, tenant);
            return result.FirstOrDefault();
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
        public List<Event> Events { get; set; }


    }

}
