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

using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.BookingLib.Data.CustomFilters;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BookingLib.Data.EntityListQueryServices
{

    public partial class BookingListQueryService
    {
        private IQueryable<BookingList> GetIqueryableList(IQueryable<Booking> iQueryable)
        {
            IQueryable<BookingList> myResult = (from a in iQueryable.Include("Direction").Include("TransportMode").Include("MainCarriageCarrier").Include("FFRStatus").Include("BookingStatus").Include("BookingSpaceAllocation").Include("BookingLevel").Include("CreatedByUser").Include("BookingProduct").Include("Shipment")
                                                select new BookingList()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    BookingNumber = a.BookingNumber,
                                                    CreateDate = a.CreateDate,
                                                    UpdateDate = a.UpdateDate,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    ShipmentId = a.ShipmentId,
                                                    DirectionCode = a.DirectionCode,
                                                    TransportModeCode = a.TransportModeCode,
                                                    AWBCarrierTarrifReference = a.AWBCarrierTarrifReference,
                                                    BookingStatusCode = a.BookingStatusCode,
                                                    CASSCode = a.CASSCode,
                                                    ConsigneeAddressId = a.ConsigneeAddressId,
                                                    ConsigneeId = a.ConsigneeId,
                                                    ConsigneeReference = a.ConsigneeReference,
                                                    DescriptionOfGoods = a.DescriptionOfGoods,
                                                    FFRStatusCode = a.FFRStatusCode,
                                                    FFRStatusDate = a.FFRStatusDate,
                                                    FNAReason = a.FNAReason,
                                                    IATACodeId = a.IATACodeId,
                                                    IssuingCarrierAddressId = a.IssuingCarrierAddressId,
                                                    IssuingCarrierAgentId = a.IssuingCarrierAgentId,
                                                    MainCarriageAllotmentIdentification = a.MainCarriageAllotmentIdentification,
                                                    MainCarriageCarrierId = a.MainCarriageCarrierId,
                                                    MainCarriageCarrierNumber = a.MainCarriageCarrierNumber,
                                                    MainCarriageCarrierPrefix = a.MainCarriageCarrierPrefix,
                                                    MainCarriageETD = a.MainCarriageETD,
                                                    MainCarriageFromPortId = a.MainCarriageFromPortId,
                                                    MainCarriageIsFromStack = a.MainCarriageIsFromStack,
                                                    MainCarriageSpaceAllocationCode = a.MainCarriageSpaceAllocationCode,
                                                    MainCarriageToPortId = a.MainCarriageToPortId,
                                                    Master = a.Master,
                                                    Notes = a.Notes,
                                                    OtherServicesInformation = a.OtherServicesInformation,
                                                    ShipperAddressId = a.ShipperAddressId,
                                                    ShipperId = a.ShipperId,
                                                    ShipperReference = a.ShipperReference,
                                                    SpaceAllocationCode = a.SpaceAllocationCode,
                                                    SpecialServicesRequest = a.SpecialServicesRequest,
                                                    Transshipment1AllotmentIdentification = a.Transshipment1AllotmentIdentification,
                                                    Transshipment1CarrierId = a.Transshipment1CarrierId,
                                                    Transshipment1CarrierNumber = a.Transshipment1CarrierNumber,
                                                    Transshipment1CarrierPrefix = a.Transshipment1CarrierPrefix,
                                                    Transshipment1ETD = a.Transshipment1ETD,
                                                    Transshipment1FromPortId = a.Transshipment1FromPortId,
                                                    Transshipment1SpaceAllocationCode = a.Transshipment1SpaceAllocationCode,
                                                    Transshipment1ToPortId = a.Transshipment1ToPortId,
                                                    Transshipment2AllotmentIdentification = a.Transshipment2AllotmentIdentification,
                                                    Transshipment2CarrierId = a.Transshipment2CarrierId,
                                                    Transshipment2CarrierNumber = a.Transshipment2CarrierNumber,
                                                    Transshipment2CarrierPrefix = a.Transshipment2CarrierPrefix,
                                                    Transshipment2ETD = a.Transshipment2ETD,
                                                    Transshipment2FromPortId = a.Transshipment2FromPortId,
                                                    Transshipment2SpaceAllocationCode = a.Transshipment2SpaceAllocationCode,
                                                    Transshipment2ToPortId = a.Transshipment2ToPortId,
                                                    DirectionName = a.Direction == null ? null : a.Direction.Name,
                                                    TransportModeName = a.TransportMode == null ? null : a.TransportMode.Name,
                                                    Airline = a.MainCarriageCarrier == null ? null : a.MainCarriageCarrier.EnglishName,
                                                    BookingStatusName = a.BookingStatus == null ? null : a.BookingStatus.Name,
                                                    FFRStatusName = a.FFRStatus == null ? null : a.FFRStatus.Name,
                                                    SpaceAllocationName = a.BookingSpaceAllocation == null ? null : a.BookingSpaceAllocation.Name,
                                                    FirstFlight = a.MainCarriageCarrierPrefix + a.MainCarriageCarrierNumber,
                                                    Routing = a.Routing,
                                                    SearchFields = a.SearchFields,
                                                    IsCancelled = a.IsCancelled,
                                                    BookingLevelCode = a.BookingLevelCode,
                                                    BookingLevelName = a.BookingLevel == null ? null : a.BookingLevel.Name,
                                                    CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                    BookingProductId = a.BookingProductId,
                                                    BookingProductName = a.BookingProduct == null ? null : a.BookingProduct.Name,
                                                    LongMaster = a.TransportModeCode == "A" ? (a.AirlinePrefix != null && a.Master != null ? a.AirlinePrefix + "-" + a.Master : a.Master) : a.Master,
                                                    LastSentByUserId = a.LastSentByUserId,
                                                    ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                                                    AirlinePrefix = a.AirlinePrefix,
                                                    Volume = a.Volume,
                                                    ChargeableWeight = a.ChargeableWeight,
                                                    GrossWeight = a.GrossWeight,
                                                    HasErrors = a.HasErrors,
                                                    HasResponse = a.HasResponse,
                                                    WaitingForResponse = a.WaitingForResponse,
                                                    IsDangerous = a.IsDangerous,
                                                    FMAAcknowledgementReason = a.FMAAcknowledgementReason,
                                                    AccountNumber = a.AccountNumber,
                                                    ConsigneeName = a.Consignee != null ? a.Consignee.EnglishName : "",
                                                    DimFactor = a.DimFactor,
                                                    GrossWeightInKG = a.GrossWeightInKG,
                                                    AWBCommodityItemNumber = a.AWBCommodityItemNumber,
                                                    NumberOfPackages = a.NumberOfPackages,
                                                    Ratio = a.Ratio,
                                                    ShipperName = !string.IsNullOrEmpty(a.Shipper.EnglishName) ? a.Shipper.EnglishName : "",
                                                    VolumetricWeight = a.VolumetricWeight,
                                                    ChargeableWeightInKG = a.ChargeableWeightInKG,
                                                    IssuingCarrierAgentName = !string.IsNullOrEmpty(a.IssuingCarrierAgent.EnglishName) ? a.IssuingCarrierAgent.EnglishName : "",
                                                    MainCarriageCarrierName = !string.IsNullOrEmpty(a.MainCarriageCarrier.EnglishName) ? a.MainCarriageCarrier.EnglishName : ""
                                                });
            return myResult;
        }

        private IQueryable<Booking> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Booking> iQueryable, int tenant)
        {
            return BookingCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
        }

        private IQueryable<Booking> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Booking> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<BookingList> GetRecentEntityLists(int tenant, string userId, string objectTableId)
        {
            List<BookingList> entityList = new List<BookingList>();

            AirlineRepository airlineRepository = new AirlineRepository(tenant);
            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            BookingRepository entityRepository = new BookingRepository(tenant);
            IQueryable<Booking> entities = entityRepository.GetAllFromIdList(ids, tenant);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Booking a = (from d in entities.Include("Direction").Include("TransportMode").Include("MainCarriageCarrier").Include("FFRStatus").Include("BookingStatus").Include("BookingSpaceAllocation").Include("Interline").Include("CreatedByUser").Include("BookingProduct").Include("Shipment")
                             where d.Id == lastActivity.EntityId
                             select d).FirstOrDefault();

                if (a != null)
                {
                    BookingList list = new BookingList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        BookingNumber = a.BookingNumber,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdatedByUserId = a.UpdatedByUserId,
                        ShipmentId = a.ShipmentId,
                        DirectionCode = a.DirectionCode,
                        TransportModeCode = a.TransportModeCode,
                        AWBCarrierTarrifReference = a.AWBCarrierTarrifReference,
                        BookingStatusCode = a.BookingStatusCode,
                        CASSCode = a.CASSCode,
                        ConsigneeAddressId = a.ConsigneeAddressId,
                        ConsigneeId = a.ConsigneeId,
                        ConsigneeReference = a.ConsigneeReference,
                        DescriptionOfGoods = a.DescriptionOfGoods,
                        FFRStatusCode = a.FFRStatusCode,
                        FFRStatusDate = a.FFRStatusDate,
                        FNAReason = a.FNAReason,
                        IATACodeId = a.IATACodeId,
                        IssuingCarrierAddressId = a.IssuingCarrierAddressId,
                        IssuingCarrierAgentId = a.IssuingCarrierAgentId,
                        MainCarriageAllotmentIdentification = a.MainCarriageAllotmentIdentification,
                        MainCarriageCarrierId = a.MainCarriageCarrierId,
                        MainCarriageCarrierNumber = a.MainCarriageCarrierNumber,
                        MainCarriageCarrierPrefix = a.MainCarriageCarrierPrefix,
                        MainCarriageETD = a.MainCarriageETD,
                        MainCarriageFromPortId = a.MainCarriageFromPortId,
                        MainCarriageIsFromStack = a.MainCarriageIsFromStack,
                        MainCarriageSpaceAllocationCode = a.MainCarriageSpaceAllocationCode,
                        MainCarriageToPortId = a.MainCarriageToPortId,
                        Master = a.Master,
                        Notes = a.Notes,
                        OtherServicesInformation = a.OtherServicesInformation,
                        ShipperAddressId = a.ShipperAddressId,
                        ShipperId = a.ShipperId,
                        ShipperReference = a.ShipperReference,
                        SpaceAllocationCode = a.SpaceAllocationCode,
                        SpecialServicesRequest = a.SpecialServicesRequest,
                        Transshipment1AllotmentIdentification = a.Transshipment1AllotmentIdentification,
                        Transshipment1CarrierId = a.Transshipment1CarrierId,
                        Transshipment1CarrierNumber = a.Transshipment1CarrierNumber,
                        Transshipment1CarrierPrefix = a.Transshipment1CarrierPrefix,
                        Transshipment1ETD = a.Transshipment1ETD,
                        Transshipment1FromPortId = a.Transshipment1FromPortId,
                        Transshipment1SpaceAllocationCode = a.Transshipment1SpaceAllocationCode,
                        Transshipment1ToPortId = a.Transshipment1ToPortId,
                        Transshipment2AllotmentIdentification = a.Transshipment2AllotmentIdentification,
                        Transshipment2CarrierId = a.Transshipment2CarrierId,
                        Transshipment2CarrierNumber = a.Transshipment2CarrierNumber,
                        Transshipment2CarrierPrefix = a.Transshipment2CarrierPrefix,
                        Transshipment2ETD = a.Transshipment2ETD,
                        Transshipment2FromPortId = a.Transshipment2FromPortId,
                        Transshipment2SpaceAllocationCode = a.Transshipment2SpaceAllocationCode,
                        Transshipment2ToPortId = a.Transshipment2ToPortId,
                        DirectionName = a.Direction == null ? null : a.Direction.Name,
                        TransportModeName = a.TransportMode == null ? null : a.TransportMode.Name,
                        Airline = a.MainCarriageCarrier == null ? null : a.MainCarriageCarrier.EnglishName,
                        BookingStatusName = a.BookingStatus == null ? null : a.BookingStatus.Name,
                        FFRStatusName = a.FFRStatus == null ? null : a.FFRStatus.Name,
                        SpaceAllocationName = a.BookingSpaceAllocation == null ? null : a.BookingSpaceAllocation.Name,
                        FirstFlight = a.MainCarriageCarrierPrefix + a.MainCarriageCarrierNumber,
                        Routing = a.Routing,
                        SearchFields = a.SearchFields,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        MainCarriageCarrierName = a.MainCarriageCarrier == null ? null : a.MainCarriageCarrier.EnglishName,
                        IsCancelled = a.IsCancelled,
                        CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                        BookingProductId = a.BookingProductId,
                        BookingProductName = a.BookingProduct == null ? null : a.BookingProduct.Name,
                        LongMaster = a.TransportModeCode == "A" ? (a.AirlinePrefix != null && a.Master != null ? a.AirlinePrefix + "-" + a.Master : a.Master) : a.Master,
                        LastSentByUserId = a.LastSentByUserId,
                        ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                        AirlinePrefix = a.AirlinePrefix,
                        Volume = a.Volume,
                        ChargeableWeight = a.ChargeableWeight,
                        GrossWeight = a.GrossWeight,
                        AccountNumber = a.AccountNumber,
                    };

                    if (!string.IsNullOrEmpty(a.InterlineId))
                    {
                        //Airline myAirline = airlineRepository.GetSingleAirline(a.InterlineId, tenant);
                        //if (myAirline != null)
                        //{
                        //    list.Prefix = myAirline.Prefix;

                        //    if (!string.IsNullOrEmpty(list.Prefix) && !string.IsNullOrEmpty(a.Master))
                        //    {
                        //        list.LongMaster = list.Prefix + "-" + a.Master;
                        //    }
                        //}
                    }

                    if (!string.IsNullOrEmpty(a.MainCarriageCarrierId))
                    {
                        Airline airline = airlineRepository.GetSingleAirline(a.MainCarriageCarrierId, tenant);
                        if (airline != null)
                        {
                            if (string.IsNullOrEmpty(a.InterlineId))
                            {
                                list.Prefix = airline.Prefix;

                                if (!string.IsNullOrEmpty(list.Prefix) && !string.IsNullOrEmpty(a.Master))
                                {
                                    list.LongMaster = list.Prefix + "-" + a.Master;
                                }
                            }
                        }
                    }

                    entityList.Add(list);
                }
            }

            return entityList;
        }

        public IQueryable<BookingList> GetBookingsList()
        {
            IQueryable<BookingList> myResult = (from a in context.Bookings.Include("BookingStatus").Include("BookingProduct").Include("MainCarriageFromPort").Include("MainCarriageFinalDestinationPort").Include("LastSentByUser").Include("LastSentByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact")
                                                select new BookingList()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    BookingNumber = a.BookingNumber,
                                                    CreateDate = a.CreateDate,
                                                    UpdateDate = a.UpdateDate,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    ShipmentId = a.ShipmentId,
                                                    DirectionCode = a.DirectionCode,
                                                    TransportModeCode = a.TransportModeCode,
                                                    MainCarriageAllotmentIdentification = a.MainCarriageAllotmentIdentification,
                                                    MainCarriageCarrierId = a.MainCarriageCarrierId,
                                                    MainCarriageCarrierNumber = a.MainCarriageCarrierNumber,
                                                    MainCarriageCarrierPrefix = a.MainCarriageCarrierPrefix,
                                                    MainCarriageETD = a.MainCarriageETD,
                                                    MainCarriageFromPortId = a.MainCarriageFromPortId,
                                                    Master = a.Master,
                                                    Notes = a.Notes,
                                                    CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                    BookingProductId = a.BookingProductId,
                                                    BookingProductName = a.BookingProduct == null ? null : a.BookingProduct.Name,
                                                    LongMaster = a.TransportModeCode == "A" ? (a.AirlinePrefix != null && a.Master != null ? a.AirlinePrefix + "-" + a.Master : a.Master) : a.Master,
                                                    LastSentByUserId = a.LastSentByUserId,
                                                    ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                                                    AirlinePrefix = a.AirlinePrefix,
                                                    Volume = a.Volume,
                                                    ChargeableWeight = a.ChargeableWeight,
                                                    GrossWeight = a.GrossWeight,
                                                    GrossWeightInKG = a.GrossWeightInKG,
                                                    BookingStatusName = a.BookingStatus == null ? null : a.BookingStatus.Name,
                                                    FFRStatusDate = a.FFRStatusDate,
                                                    LastSentByUserName = a.LastSentByUser == null ? null : (a.LastSentByUser.Contact == null ? null : a.LastSentByUser.Contact.EnglishName),
                                                    MainCarriageFromPortCode = a.MainCarriageFromPort == null ? null : a.MainCarriageFromPort.Code,
                                                    MainCarriageFinalDestinationPortCode = a.MainCarriageFinalDestinationPort == null ? null : a.MainCarriageFinalDestinationPort.Code,
                                                    IsCancelled = a.IsCancelled,
                                                    AccountNumber = a.AccountNumber,
                                                });
            return myResult;
        }

        public IQueryable<BookingList> GetBookingsListByForworderTenant(int tenant)
        {
            IQueryable<BookingList> myResult = (from a in context.Bookings.Include("BookingStatus").Include("FFRStatus").Include("BookingProduct").Include("MainCarriageFromPort").Include("MainCarriageFinalDestinationPort").Include("LastSentByUser").Include("LastSentByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("MainCarriageCarrier").Include("Shipper").Include("Consignee")
                                                where a.Tenant == tenant
                                                select new BookingList()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    BookingStatusCode = a.BookingStatusCode,
                                                    BookingNumber = a.BookingNumber,
                                                    CreateDate = a.CreateDate,
                                                    UpdateDate = a.UpdateDate,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    ShipmentId = a.ShipmentId,
                                                    DirectionCode = a.DirectionCode,
                                                    TransportModeCode = a.TransportModeCode,
                                                    MainCarriageAllotmentIdentification = a.MainCarriageAllotmentIdentification,
                                                    MainCarriageCarrierId = a.MainCarriageCarrierId,
                                                    MainCarriageCarrierNumber = a.MainCarriageCarrierNumber,
                                                    MainCarriageCarrierPrefix = a.MainCarriageCarrierPrefix,
                                                    MainCarriageETD = a.MainCarriageETD,
                                                    MainCarriageFromPortId = a.MainCarriageFromPortId,
                                                    Master = a.Master,
                                                    Notes = a.Notes,
                                                    CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                    BookingProductId = a.BookingProductId,
                                                    BookingProductName = a.BookingProduct == null ? null : a.BookingProduct.Name,
                                                    LongMaster = a.TransportModeCode == "A" ? (a.AirlinePrefix != null && a.Master != null ? a.AirlinePrefix + "-" + a.Master : a.Master) : a.Master,
                                                    LastSentByUserId = a.LastSentByUserId,
                                                    ShipmentNumber = a.Shipment == null ? null : a.Shipment.ShipmentNumber,
                                                    AirlinePrefix = a.AirlinePrefix,
                                                    Volume = a.Volume,
                                                    ChargeableWeight = a.ChargeableWeight,
                                                    GrossWeight = a.GrossWeight,
                                                    GrossWeightInKG = a.GrossWeightInKG,
                                                    BookingStatusName = a.BookingStatus == null ? null : a.BookingStatus.Name,
                                                    FFRStatusDate = a.FFRStatusDate,
                                                    LastSentByUserName = a.LastSentByUser == null ? null : (a.LastSentByUser.Contact == null ? null : a.LastSentByUser.Contact.EnglishName),
                                                    MainCarriageFromPortCode = a.MainCarriageFromPort == null ? null : a.MainCarriageFromPort.Code,
                                                    MainCarriageFinalDestinationPortCode = a.MainCarriageFinalDestinationPort == null ? null : a.MainCarriageFinalDestinationPort.Code,
                                                    IsCancelled = a.IsCancelled,
                                                    MainCarriageCarrierCode = a.MainCarriageCarrier == null ? null : a.MainCarriageCarrier.Code,
                                                    NumberOfPackages = a.NumberOfPackages,
                                                    ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                    GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                    VolumeUnitCode = a.VolumeUnitCode,
                                                    DescriptionOfGoods = a.DescriptionOfGoods,
                                                    ShipperName = a.Shipper == null ? null : a.Shipper.EnglishName,
                                                    ConsigneeName = a.Consignee == null ? null : a.Consignee.EnglishName,
                                                    Transshipment1CarrierNumber = a.Transshipment1CarrierNumber,
                                                    Transshipment1ETD = a.Transshipment1ETD,
                                                    Transshipment2CarrierNumber = a.Transshipment2CarrierNumber,
                                                    Transshipment2ETD = a.Transshipment2ETD,
                                                    AccountNumber = a.AccountNumber,
                                                    FFRStatusCode = a.FFRStatusCode,
                                                    FFRStatusName = a.FFRStatus == null ? null : a.FFRStatus.Name,
                                                });
            return myResult;
        }
    }
}
