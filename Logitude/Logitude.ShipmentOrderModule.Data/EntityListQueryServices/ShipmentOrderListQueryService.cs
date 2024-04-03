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

using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.EntityLists;

namespace Logitude.ShipmentOrderModule.Data.EntityListQueryServices
{

    public partial class ShipmentOrderListQueryService
    {
        private IQueryable<ShipmentOrderList> GetIqueryableList(IQueryable<ShipmentOrder> iQueryable)
        {
            IQueryable<ShipmentOrderList> query = (from a in iQueryable
                                                   select new ShipmentOrderList()
                                                   {

                                                       Id = a.Id,

                                                       Tenant = a.Tenant,

                                                       CreateDate = a.CreateDate,

                                                       CreatedByUserId = a.CreatedByUserId,

                                                       UpdateDate = a.UpdateDate,

                                                       UpdatedByUserId = a.UpdatedByUserId,

                                                       SearchFields = a.SearchFields,

                                                       OrderNumber = a.OrderNumber,

                                                       TransportModeId = a.TransportModeId,

                                                       ConsigneeId = a.ConsigneeId,

                                                       ShipperId = a.ShipperId,

                                                       AgentId = a.AgentId,

                                                       IncotermId = a.IncotermId,

                                                       AccountManagerId = a.AccountManagerId,

                                                       PONumber = a.PONumber,

                                                       DescriptionOfGoods = a.DescriptionOfGoods,

                                                       House = a.House,

                                                       Master = a.Master,

                                                       CustomsAgentId = a.CustomsAgentId,

                                                       SpecialServicesTypeId = a.SpecialServicesTypeId,

                                                       TransportModeName = a.TransportMode == null ? "" : a.TransportMode.Name,

                                                       BookingConfirmationDate = a.BookingConfirmationDate,

                                                       CustomerReferences = a.CustomerReferences,

                                                       IsReadyForPickup = a.IsReadyForPickup,

                                                       PickupEstimatedDateTime = a.PickupEstimatedDateTime,

                                                       PickupActualDateTime = a.PickupActualDateTime,

                                                       ForwarderId = a.ForwarderId,

                                                       ATA = a.ATA,

                                                       ATD = a.ATD,

                                                       ETD = a.ETD,

                                                       ETA = a.ETA,

                                                       ConsigneeName = a.ConsigneeCard == null ? "" : a.ConsigneeCard.EnglishName,

                                                       ShipperName = a.ShipperCard == null ? "" : a.ShipperCard.EnglishName,

                                                       AgentName = a.AgentCard == null ? "" : a.AgentCard.EnglishName,

                                                       VesselName = a.Vessel == null ? "" : a.Vessel.EnglishName,

                                                       CustomsAgentName = a.CustomsAgentCard == null ? "" : a.CustomsAgentCard.EnglishName,

                                                       SpecialServicesTypeName = a.SpecialServicesType == null ? "" : a.SpecialServicesType.EnglishName,

                                                       ForwarderName = a.FreightForwarderCard == null ? "" : a.FreightForwarderCard.EnglishName,

                                                       IncotermCode = a.Incoterm == null ? "" : a.Incoterm.Code,

                                                       ShipmentNumber = a.ShipmentNumber,

                                                       SupplyDateTime = a.SupplyDateTime,

                                                       OriginPortId = a.OriginPortId,

                                                       OriginPortName = a.OriginPort == null ? "" : a.OriginPort.EnglishName,

                                                       DestinationPortId = a.DestinationPortId,

                                                       DestinationPortName = a.DestinationPort == null ? "" : a.DestinationPort.EnglishName,

                                                       GatewayId = a.GatewayId,

                                                       GatewayName = a.Gateway == null ? "" : a.Gateway.EnglishName,

                                                       CasualImporterName = a.CasualImporterName,

                                                       CasualSupplierName = a.CasualSupplierName,

                                                       ShipmentLevelCode= a.ShipmentLevelCode,

                                                       ShipmentLevelName = a.ShipmentLevel == null ? "" : a.ShipmentLevel.Name,

                                                       PODate = a.PODate,

                                                       BookingConfirmationNumber = a.BookingConfirmationNumber,

                                                       DirectionId = a.DirectionId,

                                                       DirectionName = a.Direction == null ? "" : a.Direction.Name,

                                                       CarrierNumber = a.CarrierNumber,

                                                       CarrierId = a.CarrierId,

                                                       CarrierName = a.Carrier == null ? "" : a.Carrier.EnglishName,

                                                       IsCancelled = a.IsCancelled,

                                                       Quantity = a.Quantity,

                                                       Volume = a.Volume,

                                                       GrossWeight = a.GrossWeight,

                                                       CustomerId = a.CustomerId,

                                                       CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,

                                                       LastExceptionDate = a.LastExceptionDate,

                                                       LastExceptionDescription = a.LastExceptionDescription,

                                                       OnHandDate = a.OnHandDate,

                                                       OnHandNumber = a.OnHandNumber,

                                                       IsOperationalClosed = a.IsOperationalClosed,

                                                       PlaceOfDeliveryId = a.PlaceOfDeliveryId,

                                                       PlaceOfDeliveryName = a.PlaceOfDelivery == null ? "" : a.PlaceOfDelivery.EnglishName,

                                                       DangerousGoods = a.DangerousGoods,

                                                       ShipmentTypeId = a.ShipmentTypeId,

                                                       ShipmentTypeName = a.ShipmentType == null ? "" : a.ShipmentType.Name,

                                                       PackageTypeId = a.PackageTypeId,

                                                       PackageTypeName = a.PackageType == null ? "" : a.PackageType.EnglishName,

                                                       CustomerTenantNumber = a.CustomerTenantNumber,

                                                       CustomerShipmentNumber = a.CustomerShipmentNumber,

                                                   });
            return query;
        }

        private IQueryable<ShipmentOrder> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ShipmentOrder> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<ShipmentOrder> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ShipmentOrder> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
