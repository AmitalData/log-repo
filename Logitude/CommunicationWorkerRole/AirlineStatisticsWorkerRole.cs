using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole
{
    public class AirlineStatisticsWorkerRole : WorkerEntryPoint
    {
        AirlineStatisticsRepository airlineStatisticsRepository;
        QueueDescription queueDescription;
        QueueClient client;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 3, 0, 0);
                        DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 5, 59, 0);
                        LastActivity = DateTime.UtcNow;

                        if (DateTime.Now >= date1 && DateTime.Now <= date2)
                        {
                            List<TenantManagement> airlineTenants = null;
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                                airlineTenants = tenantManagementRepository.GetAirlineTenants().ToList();

                                scope.Complete();
                            }

                            foreach (TenantManagement item in airlineTenants)
                            {
                                airlineStatisticsRepository = new AirlineStatisticsRepository(item.Id);
                                
                                ParticipantRepository participantRepository = new ParticipantRepository(item.Id);
                                IQueryable<Participant> participants = participantRepository.GetParticipants(item.Id);

                                foreach (Participant participant in participants)
                                {
                                    #region Shipment

                                    ShipmentRepository shipmentRepository = new ShipmentRepository(participant.ForwarderTenant);
                                    IQueryable<ShipmentDataView> allShipments = shipmentRepository.GetSentAWBShipmentDataViews(participant.ForwarderTenant);
                                    allShipments = allShipments.Where(d => !string.IsNullOrEmpty(d.MainCarriageFromPortCode));
                                    allShipments = allShipments.Where(d => !string.IsNullOrEmpty(d.MainCarriageFinalDestinationPortCode));
                                    allShipments = allShipments.Where(d => d.MainCarriageCarrierCode == item.TenantConnectedToAirlineCode);

                                    foreach (ShipmentDataView shipment in allShipments)
                                    {
                                        bool exist = this.IsEntityExists("shipment", shipment.Id, item.Id);

                                        if (exist)
                                        {
                                            this.UpdateAirlineStatistics(shipment, participant.IsDirect);
                                        }

                                        else
                                        {
                                            this.BuildAirlineStatistics(shipment, item, participant.IsDirect);
                                        }
                                    }
                                    
                                    #endregion

                                    #region Booking

                                    IBookingContext bookingContext = BookingContext.GetContext(participant.ForwarderTenant);
                                    BookingListQueryService bookingQuery = new BookingListQueryService(bookingContext);
                                    IQueryable<BookingList> allBookings = bookingQuery.GetBookingsListByForworderTenant(participant.ForwarderTenant);
                                    allBookings = allBookings.Where(d => d.BookingStatusCode != "CRT");
                                    allBookings = allBookings.Where(d => !string.IsNullOrEmpty(d.MainCarriageFromPortCode));
                                    allBookings = allBookings.Where(d => !string.IsNullOrEmpty(d.MainCarriageFinalDestinationPortCode));
                                    allBookings = allBookings.Where(d => d.MainCarriageCarrierCode == item.TenantConnectedToAirlineCode);

                                    foreach (BookingList booking in allBookings)
                                    {
                                        bool exist = this.IsEntityExists("booking", booking.Id, item.Id);

                                        if (exist)
                                        {
                                            this.UpdateAirlineStatistics(booking, participant.IsDirect);
                                        }

                                        else
                                        {
                                            this.BuildAirlineStatistics(booking, item, participant.IsDirect);
                                        }
                                    }

                                    #endregion

                                    airlineStatisticsRepository.SubmitChanges();
                                }
                                LogDoneItemInMemory();
                            }

                            Thread.Sleep(10000);
                        }
                        else
                        {
                            Thread.Sleep(60000);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Airline statistics worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private bool IsEntityExists(string code, string entityId, int tenant)
        {
            bool myResult = false;

            if (code == "booking")
            {
                myResult = (from a in airlineStatisticsRepository.GetAirlineStatistics(tenant)
                            where a.BookingId == entityId
                            select a).Any();
            }

            else if (code == "shipment")
            {
                myResult = (from a in airlineStatisticsRepository.GetAirlineStatistics(tenant)
                            where a.ShipmentId == entityId
                            select a).Any();
            }

            return myResult;
        }

        private void BuildAirlineStatistics(BookingList entity, TenantManagement tenantManagement, bool isDirect)
        {
            TenantRepository tenantRepository = new TenantRepository(entity.Tenant);
            Tenant forwarderTenant = tenantRepository.GetSingleTenant(entity.Tenant);
            
            AirlineStatistics newRecord = new AirlineStatistics()
            {
                Id = IdCounter.GetNumber("AirlineStatistics", tenantManagement.Id),
                Tenant = tenantManagement.Id,
                SourceTenant = entity.Tenant,
                SourceTenantName = forwarderTenant.Company,
                BookingId = entity.Id,
                EntityReference = entity.BookingNumber,
                AWBNumber = entity.Master,
                AirlineCode = entity.MainCarriageCarrierCode,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenantManagement.Id),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenantManagement.Id),
                EntitiyCreateDate = entity.CreateDate.Value,
                EntitiyUpdateDate = entity.UpdateDate.Value,
                EntityCreatedByUserName = entity.CreatedByUserName,
                MessageType = "FFR",
                LastSentDate = entity.FFRStatusDate,
                EntityStatus = entity.BookingStatusName,
                MessagingStatus = entity.FFRStatusName,
                NumberOfPackages = entity.NumberOfPackages,
                ChargeableWeight = entity.ChargeableWeight,
                ChargeableWeightUnitCode = entity.ChargeableWeightUnitCode,
                GrossWeight = entity.GrossWeight,
                GrossWeightUnitCode = entity.GrossWeightUnitCode,
                Volume = entity.Volume,
                VolumeUnitCode = entity.VolumeUnitCode,
                OriginCode = entity.MainCarriageFromPortCode,
                DestinationCode = entity.MainCarriageFinalDestinationPortCode,
                DescriptionOfGoods = entity.DescriptionOfGoods,
                ShipperName = entity.ShipperName,
                ConsigneeName = entity.ConsigneeName,
                Flight1 = entity.MainCarriageCarrierNumber,
                Flight1Date = entity.MainCarriageETD,
                Flight2 = entity.Transshipment1CarrierNumber,
                Flight2Date = entity.Transshipment1ETD,
                Flight3 = entity.Transshipment2CarrierNumber,
                Flight3Date = entity.Transshipment2ETD,
                Allotment = entity.MainCarriageAllotmentIdentification,
                IsCancelled = entity.IsCancelled,
                AirlinePrefix = entity.AirlinePrefix,
                ProductName = entity.BookingProductName,
                Sender = isDirect ? entity.LastSentByUserName : null,
                Direct = isDirect,
            };

            newRecord.SearchFields = BuildSearchFields(newRecord);

            airlineStatisticsRepository.Add(newRecord);
        }
        private void BuildAirlineStatistics(ShipmentDataView entity, TenantManagement tenantManagement, bool isDirect)
        {
            TenantRepository tenantRepository = new TenantRepository(entity.Tenant);
            Tenant forwarderTenant = tenantRepository.GetSingleTenant(entity.Tenant);

            AirlineStatistics newRecord = new AirlineStatistics();

            newRecord.Id = IdCounter.GetNumber("AirlineStatistics", tenantManagement.Id);
            newRecord.Tenant = tenantManagement.Id;
            newRecord.SourceTenant = entity.Tenant;
            newRecord.SourceTenantName = forwarderTenant.Company;
            newRecord.ShipmentId = entity.Id;
            newRecord.ShipmentLevelCode = entity.ShipmentLevelCode;
            newRecord.EntityReference = entity.ShipmentNumber;
            newRecord.AWBNumber = entity.Master;
            newRecord.HWBNumber = entity.House;
            newRecord.AirlineCode = entity.MainCarriageCarrierCode;
            newRecord.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenantManagement.Id);
            newRecord.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenantManagement.Id);
            newRecord.EntitiyCreateDate = entity.CreateDateTime;
            newRecord.EntitiyUpdateDate = entity.LastUpdateDate;
            newRecord.EntityCreatedByUserName = entity.CreatedByUserName;
            newRecord.MessageType = entity.ShipmentLevelCode == "H" ? "FHL" : "FWB";
            newRecord.LastSentDate = entity.ShipmentLevelCode == "H" ? entity.FHLStatusDate : entity.FWBStatusDate;
            newRecord.EntityStatus = entity.ShipmentStatusName;
            newRecord.NumberOfPackages = entity.NumberOfPackages;
            newRecord.ChargeableWeight = entity.ChargeableWeight == null ? 0 : (decimal)entity.ChargeableWeight;
            newRecord.ChargeableWeightUnitCode = entity.ChargeableWeightUnitCode;
            newRecord.GrossWeight = entity.GrossWeight == null ? 0 : (decimal)entity.GrossWeight;
            newRecord.GrossWeightUnitCode = entity.GrossWeightUnitCode;
            newRecord.Volume = entity.Volume == null ? 0 : (decimal)entity.Volume;
            newRecord.VolumeUnitCode = entity.VolumeUnitCode;
            newRecord.OriginCode = entity.MainCarriageFromPortCode;
            newRecord.DestinationCode = entity.MainCarriageFinalDestinationPortCode;
            newRecord.DescriptionOfGoods = entity.DescriptionOfGoods;
            newRecord.ShipperName = entity.ShipperName;
            newRecord.ConsigneeName = entity.ConsigneeName;
            newRecord.Flight1 = entity.MainCarriageCarrierCode + entity.MainCarriageCarrierNumber;
            newRecord.Flight1Date = entity.MainCarriageATD != null ? entity.MainCarriageATD : entity.MainCarriageETD;
            newRecord.Flight2 = entity.Transshipment1CarrierCode + entity.Transshipment1CarrierNumber;
            newRecord.Flight2Date = entity.Transshipment1ATD != null ? entity.Transshipment1ATD : entity.Transshipment1ETD;
            newRecord.Flight3 = entity.Transshipment2CarrierCode + entity.Transshipment2CarrierNumber;
            newRecord.Flight3Date = entity.Transshipment2ATD != null ? entity.Transshipment2ATD : entity.Transshipment2ETD;
            newRecord.OnCarriageTo = entity.OnCarriageToPortCode;
            newRecord.OnCarriageDate = entity.OnCarriageATD != null ? entity.OnCarriageATD : entity.OnCarriageETD;
            newRecord.PreCarriageFrom = entity.PreCarriageFromPortCode;
            newRecord.PreCarriageDate = entity.PreCarriageATD != null ? entity.PreCarriageATD : entity.PreCarriageETD;
            newRecord.IsCancelled = entity.IsCancelled;
            newRecord.AirlinePrefix = entity.AirlinePrefix;
            newRecord.Sender = isDirect ? entity.LastSentByUserName : null;
            newRecord.Direct = isDirect;
            newRecord.SearchFields = BuildSearchFields(newRecord);

            airlineStatisticsRepository.Add(newRecord);
        }

        private void UpdateAirlineStatistics(BookingList booking, bool isDirect)
        {
            AirlineStatistics statistics = airlineStatisticsRepository.GetSingleAirlineStatisticsByBookingId(booking.Id);

            statistics.AWBNumber = booking.Master;
            statistics.AirlineCode = booking.MainCarriageCarrierCode;
            statistics.UpdateDate = TenantServerConfigration.GetCurrentDateTime(statistics.Tenant);
            statistics.EntitiyUpdateDate = booking.UpdateDate.Value;
            statistics.LastSentDate = booking.FFRStatusDate;
            statistics.EntityStatus = booking.BookingStatusName;
            statistics.MessagingStatus = booking.FFRStatusName;
            statistics.NumberOfPackages = booking.NumberOfPackages;
            statistics.ChargeableWeight = booking.ChargeableWeight;
            statistics.ChargeableWeightUnitCode = booking.ChargeableWeightUnitCode;
            statistics.GrossWeight = booking.GrossWeight;
            statistics.GrossWeightUnitCode = booking.GrossWeightUnitCode;
            statistics.Volume = booking.Volume;
            statistics.VolumeUnitCode = booking.VolumeUnitCode;
            statistics.OriginCode = booking.MainCarriageFromPortCode;
            statistics.DestinationCode = booking.MainCarriageFinalDestinationPortCode;
            statistics.DescriptionOfGoods = booking.DescriptionOfGoods;
            statistics.ShipperName = booking.ShipperName;
            statistics.ConsigneeName = booking.ConsigneeName;
            statistics.Flight1 = booking.MainCarriageCarrierNumber;
            statistics.Flight1Date = booking.MainCarriageETD;
            statistics.Flight2 = booking.Transshipment1CarrierNumber;
            statistics.Flight2Date = booking.Transshipment1ETD;
            statistics.Flight3 = booking.Transshipment2CarrierNumber;
            statistics.Flight3Date = booking.Transshipment2ETD;
            statistics.Allotment = booking.MainCarriageAllotmentIdentification;
            statistics.SearchFields = this.BuildSearchFields(statistics);
            statistics.IsCancelled = booking.IsCancelled;
            statistics.AirlinePrefix = booking.AirlinePrefix;
            statistics.ProductName = booking.BookingProductName;
            statistics.Sender = isDirect ? booking.LastSentByUserName : null;
            statistics.Direct = isDirect;

            airlineStatisticsRepository.Update(statistics);
        }
        private void UpdateAirlineStatistics(ShipmentDataView shipment, bool isDirect)
        {
            AirlineStatistics statistics = airlineStatisticsRepository.GetSingleAirlineStatisticsByShipmentId(shipment.Id);

            statistics.AWBNumber = shipment.Master;
            statistics.HWBNumber = shipment.House;
            statistics.AirlineCode = shipment.MainCarriageCarrierCode;
            statistics.UpdateDate = TenantServerConfigration.GetCurrentDateTime(statistics.Tenant);
            statistics.EntitiyUpdateDate = shipment.LastUpdateDate;
            statistics.LastSentDate = shipment.ShipmentLevelCode == "H" ? shipment.FHLStatusDate : shipment.FWBStatusDate;
            statistics.EntityStatus = shipment.ShipmentStatusName;
            statistics.NumberOfPackages = shipment.NumberOfPackages;
            statistics.ChargeableWeight = shipment.ChargeableWeight == null ? 0 : (decimal)shipment.ChargeableWeight;
            statistics.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
            statistics.GrossWeight = shipment.GrossWeight == null ? 0 : (decimal)shipment.GrossWeight;
            statistics.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
            statistics.Volume = shipment.Volume == null ? 0 :(decimal)shipment.Volume;
            statistics.VolumeUnitCode = shipment.VolumeUnitCode;
            statistics.OriginCode = shipment.MainCarriageFromPortCode;
            statistics.DestinationCode = shipment.MainCarriageFinalDestinationPortCode;
            statistics.DescriptionOfGoods = shipment.DescriptionOfGoods;
            statistics.ShipperName = shipment.ShipperName;
            statistics.ConsigneeName = shipment.ConsigneeName;
            statistics.Flight1 = shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber;
            statistics.Flight1Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD;
            statistics.Flight2 = shipment.Transshipment1CarrierCode + shipment.Transshipment1CarrierNumber;
            statistics.Flight2Date = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD;
            statistics.Flight3 = shipment.Transshipment2CarrierCode + shipment.Transshipment2CarrierNumber;
            statistics.Flight3Date = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD;
            statistics.OnCarriageTo = shipment.OnCarriageToPortCode;
            statistics.OnCarriageDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD;
            statistics.PreCarriageFrom = shipment.PreCarriageFromPortCode;
            statistics.PreCarriageDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD;
            statistics.SearchFields = this.BuildSearchFields(statistics);
            statistics.IsCancelled = shipment.IsCancelled;
            statistics.AirlinePrefix = shipment.AirlinePrefix;
            statistics.Sender = isDirect ? shipment.LastSentByUserName : null;
            statistics.Direct = isDirect;

            airlineStatisticsRepository.Update(statistics);
        }

        private string BuildSearchFields(AirlineStatistics entity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entity.SourceTenant.ToString());
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.SourceTenantName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.EntityReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.AWBNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.MessageType);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.ShipperName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.ConsigneeName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.EntityCreatedByUserName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            return mySearchFields;
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AirlineStatistics";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ServicePointManager.DefaultConnectionLimit = 12;
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
