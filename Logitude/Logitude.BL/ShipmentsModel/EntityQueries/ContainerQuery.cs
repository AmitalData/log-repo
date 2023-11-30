using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.Security;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerQuery
    {
        ContainerRepository repository;
        private bool isMultipleUpdate = false;
        public ContainerQuery(int tenant)
        {
            repository = new ContainerRepository(tenant);
        }
        public ContainerQuery(ContainerRepository myRepository)
        {
            this.repository = myRepository;
        }

        public ContainerPM GetSinglePM(string id, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = repository.GetSingleContainer(id, tenant);
            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "",
                    VesselName = container.VesselName,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                    ShipmentPickupFrom = container.ShipmentPickupFrom,
                    ShipmentPickupTo = container.ShipmentPickupTo,
                    ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                    ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                    ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                    ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                    ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                    ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                    ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                    ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                    ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                    ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                    ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                    ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                    ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                    ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                    ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                    ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                    ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                    ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                    ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                    ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                    ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                    ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                    ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                    ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                    ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                    ShipmentLastLegATA = container.ShipmentLastLegATA,
                    ShipmentLastLegETA = container.ShipmentLastLegETA,
                    ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                    PreCarriageLocation = container.PreCarriageLocation,
                    PreCarriageETD = container.PreCarriageETD,
                    PreCarriageATD = container.PreCarriageATD,
                    POLLocation = container.POLLocation,
                    EstimatedPOLArrival = container.EstimatedPOLArrival,
                    ActualPOLArrival = container.ActualPOLArrival,
                    EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                    ActualPOLLoaded = container.ActualPOLLoaded,
                    EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                    ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                    TransshipmentCount = container.TransshipmentCount,
                    Transshipment1Location = container.Transshipment1Location,
                    EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                    ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                    EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                    ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                    EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                    ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                    EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                    ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                    Transshipment2Location = container.Transshipment2Location,
                    EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                    ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                    EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                    ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                    EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                    ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                    EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                    ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                    Transshipment3Location = container.Transshipment3Location,
                    EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                    ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                    EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                    ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                    EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                    ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                    EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                    ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                    Transshipment4Location = container.Transshipment4Location,
                    EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                    ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                    EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                    ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                    EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                    ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                    EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                    ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                    Leg1Vessel = container.Leg1Vessel,
                    Leg1Voyage = container.Leg1Voyage,
                    Leg2Vessel = container.Leg2Vessel,
                    Leg2Voyage = container.Leg2Voyage,
                    Leg3Vessel = container.Leg3Vessel,
                    Leg3Voyage = container.Leg3Voyage,
                    Leg4Vessel = container.Leg4Vessel,
                    Leg4Voyage = container.Leg4Voyage,
                    Leg5Vessel = container.Leg5Vessel,
                    Leg5Voyage = container.Leg5Voyage,
                    PODLocation = container.PODLocation,
                    EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                    ActualPODVesselArrival = container.ActualPODVesselArrival,
                    EstimatedPODDischarge = container.EstimatedPODDischarge,
                    ActualPODDischarge = container.ActualPODDischarge,
                    EstimatedPODDeparture = container.EstimatedPODDeparture,
                    ActualPODDeparture = container.ActualPODDeparture,
                    OnCarriageLocation = container.OnCarriageLocation,
                    OnCarriageETD = container.OnCarriageETD,
                    OnCarriageATD = container.OnCarriageATD,
                    LIFLocation = container.LIFLocation,
                    EstimatedLIFArrival = container.EstimatedLIFArrival,
                    ActualLIFArrival = container.ActualLIFArrival,
                    EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                    ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                    GateIn = container.GateIn,
                    GateOut = container.GateOut,
                    EmptyReturnLocation = container.EmptyReturnLocation,
                    EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                    ActualEmptyReturn = container.ActualEmptyReturn,
                    CustomsReleaseState = container.CustomsReleaseState,
                    CustomsReleaseDate = container.CustomsReleaseDate,
                    CarrierReleaseState = container.CarrierReleaseState,
                    CarrierReleaseDate = container.CarrierReleaseDate,
                    AvailablityDate = container.AvailablityDate,
                    AvailabilityLocation = container.AvailabilityLocation,
                    FreeDays = container.FreeDays,
                    LastFreeDayDate = container.LastFreeDayDate,
                    ShipmentStatusId = container.ShipmentStatusId,
                    ShipmentStatusName = container.ShipmentEntityStatus?.Name,
                    EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                    EmptyPickupLocationName = container.EmptyPickupLocationPort == null ? null : container.EmptyPickupLocationPort.EnglishName,
                    PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                    PreCarriageLocationName = container.PreCarriageLocationPort == null ? null : container.PreCarriageLocationPort.EnglishName,
                    EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                    EmptyReturnLocationName = container.EmptyReturnLocationPort == null ? null : container.EmptyReturnLocationPort.EnglishName,
                    AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                    OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                    OnCarriageLocationName = container.OnCarriageLocationPort == null ? null : container.OnCarriageLocationPort.EnglishName,
                    LIFLocationPortId = container.LIFLocationPortId,
                    POLLocationPortId = container.POLLocationPortId,
                    POLLocationName = container.POLLocationPort == null ? null : container.POLLocationPort.EnglishName,
                    PODLocationPortId = container.PODLocationPortId,
                    PODLocationName = container.PODLocationPort == null ? null : container.PODLocationPort.EnglishName,
                    Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                    Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                    Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                    Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                    Transshipment1LocationName = container.Transshipment1LocationPort == null ? null : container.Transshipment1LocationPort.EnglishName,
                    Transshipment2LocationName = container.Transshipment2LocationPort == null ? null : container.Transshipment2LocationPort.EnglishName,
                    Transshipment3LocationName = container.Transshipment3LocationPort == null ? null : container.Transshipment3LocationPort.EnglishName,
                    Transshipment4LocationName = container.Transshipment4LocationPort == null ? null : container.Transshipment4LocationPort.EnglishName,
                    TerminalId = container.TerminalId,
                    TerminalAddress = container.TerminalAddress,
                    TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                    TerminalPhone = container.TerminalPhone,
                    TerminalAddressId = container.TerminalAddressId,
                    ShipmentPickupETA = container.ShipmentPickupETA,
                    ShipmentPickupETD = container.ShipmentPickupETD,
                    ShipmentPickupATA = container.ShipmentPickupATA,
                    ShipmentPickupATD = container.ShipmentPickupATD,
                    ShipmentPreCarriageETA = container.ShipmentPreCarriageETA,
                    ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                    ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                    ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                    ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                    ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                    ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                    ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                    ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                    ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                    ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                    ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                    ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                    ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                    ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                    ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                    ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                    ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                    ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                    ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                    ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                    ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                    ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                    ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                    ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                    ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                    ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                    ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                    ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                    ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                    ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                    ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                    ShipmentNumber = container.ShipmentNumber,
                    ShipmentTypeId = container.ShipmentTypeId,
                    ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                    OPClosed = container.OPClosed,
                    ContainersCount = container.ContainersCount,
                    HandlerId = container.HandlerId,
                    HandlerName = container.Handler?.Contact?.EnglishName,
                    CustomerId = container.CustomerId,
                    CustomerName = container.CustomerCard?.EnglishName,
                    ShipmentCreateDate = container.ShipmentCreateDate,
                    PODReceivedOnDate = container.PODReceivedOnDate,
                    IsAutomaticUpdates = container.IsAutomaticUpdates,
                    IsClosed = container.IsClosed,
                    ClosedDate = container.ClosedDate,
                    MasterEntityId = container.ShipmentId,
                    CustomerContactId = container.CustomerCard?.PrimaryContactId,
                    HandlerContactId = container.Handler?.Contact?.Id,
                    ConsigneeContactId = container.Shipment?.ConsigneeContactId,
                    ShipperContactId = container.Shipment?.ShipperContactId,
                    ShipperNotExporterContactId = container.Shipment?.ShipperNotExporterContactId,
                    FreightForwarderContactId = container.Shipment?.FreightForwarderContactId,
                    StatusId = container.StatusId,
                    IsCancelled = container.IsCancelled,
                    CancelledDate = container.CancelledDate,
                    ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                    ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                    Leg1VesselId = container.Leg1VesselId,
                    Leg2VesselId = container.Leg2VesselId,
                    Leg3VesselId = container.Leg3VesselId,
                    Leg4VesselId = container.Leg4VesselId,
                    Leg5VesselId = container.Leg5VesselId,
                    ExceptionDate = container.ExceptionDate,
                    ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                    HasException = container.HasException,
                    LastExceptionDescription = container.LastExceptionDescription,
                    ExceptionDescription = container.ExceptionDescription,
                    IsExceptionResolved = container.IsExceptionResolved,
                    EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                    EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                    EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                    EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                    EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                    EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                    OnCarriageGateOut = container.OnCarriageGateOut,
                    PreCarriageGateIn = container.PreCarriageGateIn,
                    ConcurrencyGUID = container.ConcurrencyGUID,
                    UpdatedByPartner = container.UpdatedByPartner,
                    ContainerTypeId = container.ContainerTypeId,
                    GrossWeight = container.GrossWeight,
                    Volume = container.Volume,
                    VolumeUnitCode = container.VolumeUnitCode,
                    GrossWeightUnitCode = container.GrossWeightUnitCode,
                    AdditionalReference1 = container.AdditionalReference1,
                    AdditionalReference2 = container.AdditionalReference2,
                    AdditionalReference3 = container.AdditionalReference3,
                    AdditionalReference4 = container.AdditionalReference4,
                    HasTransshipments = container.HasTransshipments,
                    MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                    MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                    MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                    MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                    MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                    MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                    OnCarriageETA = container.OnCarriageETA,
                    OnCarriageATA = container.OnCarriageATA,
                    RequestDate = container.RequestDate,
                    RecentResponseDate = container.RecentResponseDate,
                    ShipmentPreCarriageFromName = container.ShipmentPreCarriageFromPort == null ? null : container.ShipmentPreCarriageFromPort.EnglishName,
                    ShipmentMainCarriageFromName = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.EnglishName,
                    ShipmentMainCarriageToName = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.EnglishName,
                    ShipmentOnCarriageToName = container.ShipmentOnCarriageToPort == null ? null : container.ShipmentOnCarriageToPort.EnglishName,
                    ShipmentTransshipment1FromName = container.ShipmentTransshipment1FromPort == null ? null : container.ShipmentTransshipment1FromPort.EnglishName,
                    ShipmentTransshipment2FromName = container.ShipmentTransshipment2FromPort == null ? null : container.ShipmentTransshipment2FromPort.EnglishName,
                    ShipmentTransshipment3FromName = container.ShipmentTransshipment3FromPort == null ? null : container.ShipmentTransshipment3FromPort.EnglishName,
                };

                if (container.EntityStatus != null)
                {
                    containerPM.StatusName = container.EntityStatus.Name;
                    containerPM.StatusWeight = container.EntityStatus.StatusWeight;
                }
                MapCustomFields(containerPM, container);
                new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = containerPM, ParentEntityId = containerPM.Id, ParentObjectTableName = "Container", Tenant = containerPM.Tenant }).Set();
            }

            return containerPM;
        }

        private void MapCustomFields(ContainerPM containerPM, Container entityPoco)
        {
            containerPM.Field1 = new CustomFieldClass("Field1", "Container", entityPoco.Field1);
            containerPM.Field2 = new CustomFieldClass("Field2", "Container", entityPoco.Field2);
            containerPM.Field3 = new CustomFieldClass("Field3", "Container", entityPoco.Field3);
            containerPM.Field4 = new CustomFieldClass("Field4", "Container", entityPoco.Field4);
            containerPM.Field5 = new CustomFieldClass("Field5", "Container", entityPoco.Field5);
            containerPM.Field6 = new CustomFieldClass("Field6", "Container", entityPoco.Field6);
            containerPM.Field7 = new CustomFieldClass("Field7", "Container", entityPoco.Field7);
            containerPM.Field8 = new CustomFieldClass("Field8", "Container", entityPoco.Field8);
            containerPM.Field9 = new CustomFieldClass("Field9", "Container", entityPoco.Field9);
            containerPM.Field10 = new CustomFieldClass("Field10", "Container", entityPoco.Field10);

            containerPM.Field11 = new CustomFieldClass("Field11", "Container", entityPoco.Field11);
            containerPM.Field12 = new CustomFieldClass("Field12", "Container", entityPoco.Field12);
            containerPM.Field13 = new CustomFieldClass("Field13", "Container", entityPoco.Field13);
            containerPM.Field14 = new CustomFieldClass("Field14", "Container", entityPoco.Field14);
            containerPM.Field15 = new CustomFieldClass("Field15", "Container", entityPoco.Field15);
            containerPM.Field16 = new CustomFieldClass("Field16", "Container", entityPoco.Field16);
            containerPM.Field17 = new CustomFieldClass("Field17", "Container", entityPoco.Field17);
            containerPM.Field18 = new CustomFieldClass("Field18", "Container", entityPoco.Field18);
            containerPM.Field19 = new CustomFieldClass("Field19", "Container", entityPoco.Field19);
            containerPM.Field20 = new CustomFieldClass("Field20", "Container", entityPoco.Field20);

            containerPM.Field21 = new CustomFieldClass("Field21", "Container", entityPoco.Field21);
            containerPM.Field22 = new CustomFieldClass("Field22", "Container", entityPoco.Field22);
            containerPM.Field23 = new CustomFieldClass("Field23", "Container", entityPoco.Field23);
            containerPM.Field24 = new CustomFieldClass("Field24", "Container", entityPoco.Field24);
            containerPM.Field25 = new CustomFieldClass("Field25", "Container", entityPoco.Field25);
            containerPM.Field26 = new CustomFieldClass("Field26", "Container", entityPoco.Field26);
            containerPM.Field27 = new CustomFieldClass("Field27", "Container", entityPoco.Field27);
            containerPM.Field28 = new CustomFieldClass("Field28", "Container", entityPoco.Field28);
            containerPM.Field29 = new CustomFieldClass("Field29", "Container", entityPoco.Field29);
            containerPM.Field30 = new CustomFieldClass("Field30", "Container", entityPoco.Field30);

            containerPM.Field31 = new CustomFieldClass("Field31", "Container", entityPoco.Field31);
            containerPM.Field32 = new CustomFieldClass("Field32", "Container", entityPoco.Field32);
            containerPM.Field33 = new CustomFieldClass("Field33", "Container", entityPoco.Field33);
            containerPM.Field34 = new CustomFieldClass("Field34", "Container", entityPoco.Field34);
            containerPM.Field35 = new CustomFieldClass("Field35", "Container", entityPoco.Field35);
            containerPM.Field36 = new CustomFieldClass("Field36", "Container", entityPoco.Field36);
            containerPM.Field37 = new CustomFieldClass("Field37", "Container", entityPoco.Field37);
            containerPM.Field38 = new CustomFieldClass("Field38", "Container", entityPoco.Field38);
            containerPM.Field39 = new CustomFieldClass("Field39", "Container", entityPoco.Field39);
            containerPM.Field40 = new CustomFieldClass("Field40", "Container", entityPoco.Field40);
            MapConcurrencyFields(containerPM);
        }

        private void MapConcurrencyFields(ContainerPM containerPM)
        {
            containerPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        public List<ContainerPM> GetContainers(string id, int tenant)
        {
            return (from container in repository.context.Containers.Include("CarrierCard").Include("VesselCard").Include("ShipmentOnCarriageToPort").Include("ShipmentOnCarriageFromPort").
                    Include("ShipmentTransshipment3ToPort").Include("ShipmentTransshipment3FromPort").Include("ShipmentTransshipment2ToPort").Include("ShipmentTransshipment2FromPort")
                    .Include("ShipmentTransshipment1ToPort").Include("ShipmentTransshipment1FromPort").Include("ShipmentMainCarriageToPort").Include("ShipmentMainCarriageFromPort")
                    .Include("ShipmentPreCarriageToPort").Include("ShipmentPreCarriageFromPort").Include("ShipmentEntityStatus").Include("TerminalCard").Include("TerminalCardAddress")
                    .Include("ShipmentOriginAgent").Include("ShipmentDestinationAgent").Include("ShipmentType").Include("CustomerCard").Include("Handler")
                    .Include("EntityStatus").Include("TruckerCard").Include("ShipmentMainCarriageToPort.Country").Include("ShipmentMainCarriageFromPort.Country").Include("ShipmentMainCarriageToPort.CountryName").Include("ShipmentMainCarriageFromPort.CountryName").Include("ShipmentMainCarriageToPort.CountryCode").Include("ShipmentMainCarriageFromPort.CountryCode")
                    where container.Id == id && container.Tenant == tenant
                    select new ContainerPM()
                    {
                        Id = container.Id,
                        Tenant = container.Tenant,
                        CreateDate = container.CreateDate,
                        CreatedByUserId = container.CreatedByUserId,
                        UpdateDate = container.UpdateDate,
                        UpdatedByUserId = container.UpdatedByUserId,
                        MainCarriageCarrierId = container.MainCarriageCarrierId,
                        MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                        MainCarriageATA = container.MainCarriageATA,
                        MainCarriageATD = container.MainCarriageATD,
                        MainCarriageETA = container.MainCarriageETA,
                        MainCarriageETD = container.MainCarriageETD,
                        ContainerNumber = container.ContainerNumber,
                        MainCarriageVesselId = container.MainCarriageVesselId,
                        ShipmentPackagesId = container.ShipmentPackagesId,
                        SearchFields = container.SearchFields,
                        DischargeDate = container.DischargeDate,
                        Master = container.Master,
                        CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "",
                        VesselName = container.VesselName,
                        ShipmentId = container.ShipmentId,
                        ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                        EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                        CurrentStatus = container.CurrentStatus,
                        CurrentStatusDate = container.CurrentStatusDate,
                        HasContainerException = container.HasContainerException,
                        CurrentLocation = container.CurrentLocation,
                        EmptyPickupLocation = container.EmptyPickupLocation,
                        DepartureLocation = container.DepartureLocation,
                        DestinationLocation = container.DestinationLocation,
                        ShipmentPickupFrom = container.ShipmentPickupFrom,
                        ShipmentPickupTo = container.ShipmentPickupTo,
                        ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                        ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                        ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                        ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                        ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                        ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                        ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                        ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                        ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                        ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                        ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                        ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                        ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                        ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                        ShipmentLastLegATA = container.ShipmentLastLegATA,
                        ShipmentLastLegETA = container.ShipmentLastLegETA,
                        ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                        ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                        ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                        ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                        ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                        ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                        ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                        ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                        ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                        ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                        ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                        ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                        PreCarriageLocation = container.PreCarriageLocation,
                        PreCarriageETD = container.PreCarriageETD,
                        PreCarriageATD = container.PreCarriageATD,
                        POLLocation = container.POLLocation,
                        EstimatedPOLArrival = container.EstimatedPOLArrival,
                        ActualPOLArrival = container.ActualPOLArrival,
                        EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                        ActualPOLLoaded = container.ActualPOLLoaded,
                        EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                        ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                        TransshipmentCount = container.TransshipmentCount,
                        Transshipment1Location = container.Transshipment1Location,
                        EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                        ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                        EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                        ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                        EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                        ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                        EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                        ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                        Transshipment2Location = container.Transshipment2Location,
                        EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                        ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                        EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                        ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                        EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                        ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                        EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                        ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                        Transshipment3Location = container.Transshipment3Location,
                        EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                        ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                        EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                        ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                        EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                        ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                        EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                        ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                        Transshipment4Location = container.Transshipment4Location,
                        EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                        ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                        EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                        ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                        EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                        ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                        EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                        ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                        Leg1Vessel = container.Leg1Vessel,
                        Leg1Voyage = container.Leg1Voyage,
                        Leg2Vessel = container.Leg2Vessel,
                        Leg2Voyage = container.Leg2Voyage,
                        Leg3Vessel = container.Leg3Vessel,
                        Leg3Voyage = container.Leg3Voyage,
                        Leg4Vessel = container.Leg4Vessel,
                        Leg4Voyage = container.Leg4Voyage,
                        Leg5Vessel = container.Leg5Vessel,
                        Leg5Voyage = container.Leg5Voyage,
                        PODLocation = container.PODLocation,
                        EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                        ActualPODVesselArrival = container.ActualPODVesselArrival,
                        EstimatedPODDischarge = container.EstimatedPODDischarge,
                        ActualPODDischarge = container.ActualPODDischarge,
                        EstimatedPODDeparture = container.EstimatedPODDeparture,
                        ActualPODDeparture = container.ActualPODDeparture,
                        OnCarriageLocation = container.OnCarriageLocation,
                        OnCarriageETD = container.OnCarriageETD,
                        OnCarriageATD = container.OnCarriageATD,
                        LIFLocation = container.LIFLocation,
                        EstimatedLIFArrival = container.EstimatedLIFArrival,
                        ActualLIFArrival = container.ActualLIFArrival,
                        EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                        ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                        GateIn = container.GateIn,
                        GateOut = container.GateOut,
                        EmptyReturnLocation = container.EmptyReturnLocation,
                        EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                        ActualEmptyReturn = container.ActualEmptyReturn,
                        CustomsReleaseState = container.CustomsReleaseState,
                        CustomsReleaseDate = container.CustomsReleaseDate,
                        CarrierReleaseState = container.CarrierReleaseState,
                        CarrierReleaseDate = container.CarrierReleaseDate,
                        AvailablityDate = container.AvailablityDate,
                        AvailabilityLocation = container.AvailabilityLocation,
                        FreeDays = container.FreeDays,
                        LastFreeDayDate = container.LastFreeDayDate,
                        ShipmentStatusId = container.ShipmentStatusId,
                        ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                        EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                        PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                        EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                        AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                        OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                        LIFLocationPortId = container.LIFLocationPortId,
                        POLLocationPortId = container.POLLocationPortId,
                        PODLocationPortId = container.PODLocationPortId,
                        Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                        Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                        Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                        Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                        TerminalId = container.TerminalId,
                        TerminalAddress = container.TerminalAddress,
                        TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                        TerminalPhone = container.TerminalPhone,
                        TerminalAddressId = container.TerminalAddressId,
                        ShipmentPickupETA = container.ShipmentPickupETA,
                        ShipmentPickupETD = container.ShipmentPickupETD,
                        ShipmentPickupATA = container.ShipmentPickupATA,
                        ShipmentPickupATD = container.ShipmentPickupATD,
                        ShipmentPreCarriageETA = container.ShipmentPickupATD,
                        ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                        ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                        ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                        ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                        ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                        ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                        ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                        ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                        ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                        ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                        ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                        ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                        ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                        ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                        ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                        ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                        ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                        ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                        ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                        ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                        ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                        ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                        ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                        ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                        ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                        ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                        ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                        ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                        ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                        ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                        ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                        ShipmentNumber = container.ShipmentNumber,
                        ShipmentTypeId = container.ShipmentTypeId,
                        ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                        OPClosed = container.OPClosed,
                        ContainersCount = container.ContainersCount,
                        HandlerId = container.HandlerId,
                        HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                        CustomerId = container.CustomerId,
                        CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                        ShipmentCreateDate = container.ShipmentCreateDate,
                        PODReceivedOnDate = container.PODReceivedOnDate,
                        IsAutomaticUpdates = container.IsAutomaticUpdates,
                        IsClosed = container.IsClosed,
                        ClosedDate = container.ClosedDate,
                        MasterEntityId = container.ShipmentId,
                        CustomerContactId = container.CustomerCard == null ? null : container.CustomerCard.PrimaryContactId,
                        HandlerContactId = (container.Handler == null || container.Handler.Contact == null) ? null : container.Handler.Contact.Id,
                        ConsigneeContactId = container.Shipment == null ? null : container.Shipment.ConsigneeContactId,
                        ShipperContactId = container.Shipment == null ? null : container.Shipment.ShipperContactId,
                        ShipperNotExporterContactId = container.Shipment == null ? null : container.Shipment.ShipperNotExporterContactId,
                        FreightForwarderContactId = container.Shipment == null ? null : container.Shipment.FreightForwarderContactId,
                        StatusId = container.StatusId,
                        StatusName = container.EntityStatus != null ? container.EntityStatus.Name : null,
                        StatusWeight = container.EntityStatus != null ? container.EntityStatus.StatusWeight : 0,
                        IsCancelled = container.IsCancelled,
                        CancelledDate = container.CancelledDate,
                        ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                        ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                        Leg1VesselId = container.Leg1VesselId,
                        Leg2VesselId = container.Leg2VesselId,
                        Leg3VesselId = container.Leg3VesselId,
                        Leg4VesselId = container.Leg4VesselId,
                        Leg5VesselId = container.Leg5VesselId,
                        ExceptionDate = container.ExceptionDate,
                        ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                        HasException = container.HasException,
                        LastExceptionDescription = container.LastExceptionDescription,
                        ExceptionDescription = container.ExceptionDescription,
                        EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                        EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                        EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                        EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                        EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                        EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                        OnCarriageGateOut = container.OnCarriageGateOut,
                        PreCarriageGateIn = container.PreCarriageGateIn,
                        UpdatedByPartner = container.UpdatedByPartner,
                        OnCarriageETA = container.OnCarriageETA,
                        OnCarriageATA = container.OnCarriageATA,

                        MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                        MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                        MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                        MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                        MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                        MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                        RequestDate = container.RequestDate,
                        RecentResponseDate = container.RecentResponseDate,
                    }).ToList();
        }

        public IQueryable<ContainerList> GetIQueryableEntityList(IQueryable<Container> iQueryable)
        {
            IQueryable<ContainerList> result = from container in iQueryable.Include("CarrierCard").Include("VesselCard").Include("ShipmentOnCarriageToPort").Include("ShipmentOnCarriageFromPort").
                    Include("ShipmentTransshipment3ToPort").Include("ShipmentTransshipment3FromPort").Include("ShipmentTransshipment2ToPort").Include("ShipmentTransshipment2FromPort")
                    .Include("ShipmentTransshipment1ToPort").Include("ShipmentTransshipment1FromPort").Include("ShipmentMainCarriageToPort").Include("ShipmentMainCarriageFromPort")
                    .Include("ShipmentPreCarriageToPort").Include("ShipmentPreCarriageFromPort").Include("ShipmentPreCarriageFromPort").Include("ShipmentEntityStatus").Include("TerminalCard")
                    .Include("ShipmentOriginAgent").Include("ShipmentDestinationAgent").Include("ShipmentType").Include("CustomerCard").Include("Handler").Include("EntityStatus").Include("TruckerCard")
                    .Include("ContainerType").Include("ShipmentMainCarriageToPort.Country").Include("ShipmentMainCarriageFromPort.Country").Include("ShipmentMainCarriageFromPort.CountryName").Include("ShipmentMainCarriageToPort.CountryName").Include("ShipmentMainCarriageFromPort.CountryCode").Include("ShipmentMainCarriageToPort.CountryCode").Include("ShipmentDepartment.EnglishName")
                                               select new ContainerList()
                                               {
                                                   Id = container.Id,
                                                   Tenant = container.Tenant,
                                                   CreateDate = container.CreateDate,
                                                   CreatedByUserId = container.CreatedByUserId,
                                                   UpdateDate = container.UpdateDate,
                                                   UpdatedByUserId = container.UpdatedByUserId,
                                                   MainCarriageCarrierId = container.MainCarriageCarrierId,
                                                   MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                                                   MainCarriageATA = container.MainCarriageATA,
                                                   MainCarriageATD = container.MainCarriageATD,
                                                   MainCarriageETA = container.MainCarriageETA,
                                                   MainCarriageETD = container.MainCarriageETD,
                                                   ContainerNumber = container.ContainerNumber,
                                                   MainCarriageVesselId = container.MainCarriageVesselId,
                                                   ShipmentPackagesId = container.ShipmentPackagesId,
                                                   SearchFields = container.SearchFields,
                                                   DischargeDate = container.DischargeDate,
                                                   Master = container.Master,
                                                   CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "",
                                                   VesselName = container.VesselName,
                                                   ShipmentId = container.ShipmentId,
                                                   ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                                                   EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                                                   CurrentStatus = container.CurrentStatus,
                                                   CurrentStatusDate = container.CurrentStatusDate,
                                                   HasContainerException = container.HasContainerException,
                                                   CurrentLocation = container.CurrentLocation,
                                                   EmptyPickupLocation = container.EmptyPickupLocation,
                                                   DepartureLocation = container.DepartureLocation,
                                                   DestinationLocation = container.DestinationLocation,
                                                   ShipmentPickupFrom = container.ShipmentPickupFrom,
                                                   ShipmentPickupTo = container.ShipmentPickupTo,
                                                   ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                                                   ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                                                   ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                                                   ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                                                   ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                                                   ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                                                   ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                                                   ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                                                   ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                                                   ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                                                   ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                                                   ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                                                   ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                                                   ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                                                   ShipmentLastLegATA = container.ShipmentLastLegATA,
                                                   ShipmentLastLegETA = container.ShipmentLastLegETA,
                                                   ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                                                   ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                                                   ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                                                   ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                                                   ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                                                   ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                                                   ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                                                   ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                                                   ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                                                   ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                                                   ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                                                   ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                                                   PreCarriageLocation = container.PreCarriageLocation,
                                                   PreCarriageETD = container.PreCarriageETD,
                                                   PreCarriageATD = container.PreCarriageATD,
                                                   POLLocation = container.POLLocation,
                                                   EstimatedPOLArrival = container.EstimatedPOLArrival,
                                                   ActualPOLArrival = container.ActualPOLArrival,
                                                   EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                                                   ActualPOLLoaded = container.ActualPOLLoaded,
                                                   EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                                                   ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                                                   TransshipmentCount = container.TransshipmentCount,
                                                   Transshipment1Location = container.Transshipment1Location,
                                                   EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                                                   ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                                                   EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                                                   ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                                                   EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                                                   ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                                                   EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                                                   ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                                                   Transshipment2Location = container.Transshipment2Location,
                                                   EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                                                   ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                                                   EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                                                   ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                                                   EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                                                   ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                                                   EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                                                   ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                                                   Transshipment3Location = container.Transshipment3Location,
                                                   EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                                                   ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                                                   EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                                                   ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                                                   EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                                                   ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                                                   EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                                                   ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                                                   Transshipment4Location = container.Transshipment4Location,
                                                   EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                                                   ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                                                   EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                                                   ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                                                   EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                                                   ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                                                   EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                                                   ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                                                   Leg1Vessel = container.Leg1Vessel,
                                                   Leg1Voyage = container.Leg1Voyage,
                                                   Leg2Vessel = container.Leg2Vessel,
                                                   Leg2Voyage = container.Leg2Voyage,
                                                   Leg3Vessel = container.Leg3Vessel,
                                                   Leg3Voyage = container.Leg3Voyage,
                                                   Leg4Vessel = container.Leg4Vessel,
                                                   Leg4Voyage = container.Leg4Voyage,
                                                   Leg5Vessel = container.Leg5Vessel,
                                                   Leg5Voyage = container.Leg5Voyage,
                                                   PODLocation = container.PODLocation,
                                                   EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                                                   ActualPODVesselArrival = container.ActualPODVesselArrival,
                                                   EstimatedPODDischarge = container.EstimatedPODDischarge,
                                                   ActualPODDischarge = container.ActualPODDischarge,
                                                   EstimatedPODDeparture = container.EstimatedPODDeparture,
                                                   ActualPODDeparture = container.ActualPODDeparture,
                                                   OnCarriageLocation = container.OnCarriageLocation,
                                                   OnCarriageETD = container.OnCarriageETD,
                                                   OnCarriageATD = container.OnCarriageATD,
                                                   LIFLocation = container.LIFLocation,
                                                   EstimatedLIFArrival = container.EstimatedLIFArrival,
                                                   ActualLIFArrival = container.ActualLIFArrival,
                                                   EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                                                   ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                                                   GateIn = container.GateIn,
                                                   GateOut = container.GateOut,
                                                   EmptyReturnLocation = container.EmptyReturnLocation,
                                                   EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                                                   ActualEmptyReturn = container.ActualEmptyReturn,
                                                   CustomsReleaseState = container.CustomsReleaseState,
                                                   CustomsReleaseDate = container.CustomsReleaseDate,
                                                   CarrierReleaseState = container.CarrierReleaseState,
                                                   CarrierReleaseDate = container.CarrierReleaseDate,
                                                   AvailablityDate = container.AvailablityDate,
                                                   AvailabilityLocation = container.AvailabilityLocation,
                                                   FreeDays = container.FreeDays,
                                                   LastFreeDayDate = container.LastFreeDayDate,
                                                   ShipmentStatusId = container.ShipmentStatusId,
                                                   ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                                                   EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                                                   PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                                                   EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                                                   AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                                                   OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                                                   LIFLocationPortId = container.LIFLocationPortId,
                                                   POLLocationPortId = container.POLLocationPortId,
                                                   PODLocationPortId = container.PODLocationPortId,
                                                   Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                                                   Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                                                   Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                                                   Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                                                   TerminalId = container.TerminalId,
                                                   TerminalAddress = container.TerminalAddress,
                                                   TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                                                   TerminalPhone = container.TerminalPhone,
                                                   TerminalAddressId = container.TerminalAddressId,
                                                   Field1 = container.Field1,
                                                   Field2 = container.Field2,
                                                   Field3 = container.Field3,
                                                   Field4 = container.Field4,
                                                   Field5 = container.Field5,
                                                   Field6 = container.Field6,
                                                   Field7 = container.Field7,
                                                   Field8 = container.Field8,
                                                   Field9 = container.Field9,
                                                   Field10 = container.Field10,
                                                   ShipmentPickupETA = container.ShipmentPickupETA,
                                                   ShipmentPickupETD = container.ShipmentPickupETD,
                                                   ShipmentPickupATA = container.ShipmentPickupATA,
                                                   ShipmentPickupATD = container.ShipmentPickupATD,
                                                   ShipmentPreCarriageETA = container.ShipmentPickupATD,
                                                   ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                                                   ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                                                   ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                                                   ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                                                   ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                                                   ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                                                   ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                                                   ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                                                   ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                                                   ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                                                   ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                                                   ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                                                   ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                                                   ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                                                   ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                                                   ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                                                   ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                                                   ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                                                   ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                                                   ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                                                   ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                                                   ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                                                   ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                                                   ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                                                   ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                                                   ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                                                   ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                                                   ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                                                   ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                                                   ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                                                   ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                                                   ShipmentNumber = container.ShipmentNumber,
                                                   ShipmentTypeId = container.ShipmentTypeId,
                                                   ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                                                   OPClosed = container.OPClosed,
                                                   ContainersCount = container.ContainersCount,
                                                   HandlerId = container.HandlerId,
                                                   HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                                                   CustomerId = container.CustomerId,
                                                   CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                                                   ShipmentCreateDate = container.ShipmentCreateDate,
                                                   PODReceivedOnDate = container.PODReceivedOnDate,
                                                   IsAutomaticUpdates = container.IsAutomaticUpdates,
                                                   IsClosed = container.IsClosed,
                                                   ClosedDate = container.ClosedDate,
                                                   StatusName = container.EntityStatus != null ? container.EntityStatus.Name : null,
                                                   StatusWeight = container.EntityStatus != null ? container.EntityStatus.StatusWeight : 0,
                                                   StatusId = container.StatusId,
                                                   IsCancelled = container.IsCancelled,
                                                   CancelledDate = container.CancelledDate,
                                                   ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                                                   ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                                                   Leg1VesselId = container.Leg1VesselId,
                                                   Leg2VesselId = container.Leg2VesselId,
                                                   Leg3VesselId = container.Leg3VesselId,
                                                   Leg4VesselId = container.Leg4VesselId,
                                                   Leg5VesselId = container.Leg5VesselId,
                                                   ExceptionDate = container.ExceptionDate,
                                                   ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                                                   HasException = container.HasException,
                                                   LastExceptionDescription = container.LastExceptionDescription,
                                                   ExceptionDescription = container.ExceptionDescription,
                                                   IsExceptionResolved = container.IsExceptionResolved,
                                                   EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                                                   EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                                                   EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                                                   EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                                                   EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                                                   EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                                                   OnCarriageGateOut = container.OnCarriageGateOut,
                                                   PreCarriageGateIn = container.PreCarriageGateIn,
                                                   UpdatedByPartner = container.UpdatedByPartner,
                                                   ContainerTypeId = container.ContainerTypeId,
                                                   ContainerTypeName = container.ContainerType == null ? null : container.ContainerType.EnglishName,
                                                   GrossWeight = container.GrossWeight,
                                                   Volume = container.Volume,
                                                   VolumeUnitCode = container.VolumeUnitCode,
                                                   GrossWeightUnitCode = container.GrossWeightUnitCode,
                                                   AdditionalReference1 = container.AdditionalReference1,
                                                   AdditionalReference2 = container.AdditionalReference2,
                                                   AdditionalReference3 = container.AdditionalReference3,
                                                   AdditionalReference4 = container.AdditionalReference4,
                                                   HasTransshipments = container.HasTransshipments,
                                                   Field11 = container.Field11,
                                                   Field12 = container.Field12,
                                                   Field13 = container.Field13,
                                                   Field14 = container.Field14,
                                                   Field15 = container.Field15,
                                                   Field16 = container.Field16,
                                                   Field17 = container.Field17,
                                                   Field18 = container.Field18,
                                                   Field19 = container.Field19,
                                                   Field20 = container.Field20,
                                                   Field21 = container.Field21,
                                                   Field22 = container.Field22,
                                                   Field23 = container.Field23,
                                                   Field24 = container.Field24,
                                                   Field25 = container.Field25,
                                                   Field26 = container.Field26,
                                                   Field27 = container.Field27,
                                                   Field28 = container.Field28,
                                                   Field29 = container.Field29,
                                                   Field30 = container.Field30,
                                                   Field31 = container.Field31,
                                                   Field32 = container.Field32,
                                                   Field33 = container.Field33,
                                                   Field34 = container.Field34,
                                                   Field35 = container.Field35,
                                                   Field36 = container.Field36,
                                                   Field37 = container.Field37,
                                                   Field38 = container.Field38,
                                                   Field39 = container.Field39,
                                                   Field40 = container.Field40,
                                                   MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                                                   MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                                                   MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                                                   MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                                                   MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                                                   MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                                                   OnCarriageETA = container.OnCarriageETA,
                                                   OnCarriageATA = container.OnCarriageATA,
                                                   RequestDate = container.RequestDate,
                                                   RecentResponseDate = container.RecentResponseDate,
                                                   ShipmentDepartmentId = container.ShipmentDepartmentId,
                                                   ShipmentDepartmentName = container.ShipmentDepartment != null ? container.ShipmentDepartment.EnglishName : "",
                                               };
            return result;
        }

        public ContainerPM GetContainerByShipmentPackagesId(string shipmentPackageId, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = repository.GetContainerByShipmentPackagesId(shipmentPackageId, tenant);
            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                    ShipmentPickupFrom = container.ShipmentPickupFrom,
                    ShipmentPickupTo = container.ShipmentPickupTo,
                    ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                    ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                    ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                    ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                    ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                    ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                    ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                    ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                    ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                    ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                    ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                    ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                    ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                    ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                    ShipmentLastLegATA = container.ShipmentLastLegATA,
                    ShipmentLastLegETA = container.ShipmentLastLegETA,
                    ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                    ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                    ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                    ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                    ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                    ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                    ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                    ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                    ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                    ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                    ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                    ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                    PreCarriageLocation = container.PreCarriageLocation,
                    PreCarriageETD = container.PreCarriageETD,
                    PreCarriageATD = container.PreCarriageATD,
                    POLLocation = container.POLLocation,
                    EstimatedPOLArrival = container.EstimatedPOLArrival,
                    ActualPOLArrival = container.ActualPOLArrival,
                    EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                    ActualPOLLoaded = container.ActualPOLLoaded,
                    EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                    ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                    TransshipmentCount = container.TransshipmentCount,
                    Transshipment1Location = container.Transshipment1Location,
                    EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                    ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                    EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                    ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                    EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                    ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                    EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                    ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                    Transshipment2Location = container.Transshipment2Location,
                    EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                    ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                    EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                    ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                    EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                    ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                    EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                    ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                    Transshipment3Location = container.Transshipment3Location,
                    EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                    ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                    EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                    ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                    EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                    ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                    EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                    ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                    Transshipment4Location = container.Transshipment4Location,
                    EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                    ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                    EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                    ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                    EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                    ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                    EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                    ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                    Leg1Vessel = container.Leg1Vessel,
                    Leg1Voyage = container.Leg1Voyage,
                    Leg2Vessel = container.Leg2Vessel,
                    Leg2Voyage = container.Leg2Voyage,
                    Leg3Vessel = container.Leg3Vessel,
                    Leg3Voyage = container.Leg3Voyage,
                    Leg4Vessel = container.Leg4Vessel,
                    Leg4Voyage = container.Leg4Voyage,
                    Leg5Vessel = container.Leg5Vessel,
                    Leg5Voyage = container.Leg5Voyage,
                    PODLocation = container.PODLocation,
                    EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                    ActualPODVesselArrival = container.ActualPODVesselArrival,
                    EstimatedPODDischarge = container.EstimatedPODDischarge,
                    ActualPODDischarge = container.ActualPODDischarge,
                    EstimatedPODDeparture = container.EstimatedPODDeparture,
                    ActualPODDeparture = container.ActualPODDeparture,
                    OnCarriageLocation = container.OnCarriageLocation,
                    OnCarriageETD = container.OnCarriageETD,
                    OnCarriageATD = container.OnCarriageATD,
                    LIFLocation = container.LIFLocation,
                    EstimatedLIFArrival = container.EstimatedLIFArrival,
                    ActualLIFArrival = container.ActualLIFArrival,
                    EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                    ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                    GateIn = container.GateIn,
                    GateOut = container.GateOut,
                    EmptyReturnLocation = container.EmptyReturnLocation,
                    EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                    ActualEmptyReturn = container.ActualEmptyReturn,
                    CustomsReleaseState = container.CustomsReleaseState,
                    CustomsReleaseDate = container.CustomsReleaseDate,
                    CarrierReleaseState = container.CarrierReleaseState,
                    CarrierReleaseDate = container.CarrierReleaseDate,
                    AvailablityDate = container.AvailablityDate,
                    AvailabilityLocation = container.AvailabilityLocation,
                    FreeDays = container.FreeDays,
                    LastFreeDayDate = container.LastFreeDayDate,
                    ShipmentStatusId = container.ShipmentStatusId,
                    ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                    EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                    PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                    EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                    AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                    OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                    LIFLocationPortId = container.LIFLocationPortId,
                    POLLocationPortId = container.POLLocationPortId,
                    PODLocationPortId = container.PODLocationPortId,
                    Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                    Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                    Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                    Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                    TerminalId = container.TerminalId,
                    TerminalAddress = container.TerminalAddress,
                    TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                    TerminalPhone = container.TerminalPhone,
                    TerminalAddressId = container.TerminalAddressId,
                    ShipmentPickupETA = container.ShipmentPickupETA,
                    ShipmentPickupETD = container.ShipmentPickupETD,
                    ShipmentPickupATA = container.ShipmentPickupATA,
                    ShipmentPickupATD = container.ShipmentPickupATD,
                    ShipmentPreCarriageETA = container.ShipmentPickupATD,
                    ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                    ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                    ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                    ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                    ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                    ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                    ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                    ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                    ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                    ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                    ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                    ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                    ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                    ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                    ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                    ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                    ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                    ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                    ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                    ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                    ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                    ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                    ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                    ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                    ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                    ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                    ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                    ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                    ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                    ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                    ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                    ShipmentNumber = container.ShipmentNumber,
                    ShipmentTypeId = container.ShipmentTypeId,
                    ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                    OPClosed = container.OPClosed,
                    ContainersCount = container.ContainersCount,
                    HandlerId = container.HandlerId,
                    HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                    CustomerId = container.CustomerId,
                    CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                    ShipmentCreateDate = container.ShipmentCreateDate,
                    PODReceivedOnDate = container.PODReceivedOnDate,
                    IsAutomaticUpdates = container.IsAutomaticUpdates,
                    IsClosed = container.IsClosed,
                    ClosedDate = container.ClosedDate,
                    StatusId = container.StatusId,
                    IsCancelled = container.IsCancelled,
                    CancelledDate = container.CancelledDate,
                    ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                    ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                    Leg1VesselId = container.Leg1VesselId,
                    Leg2VesselId = container.Leg2VesselId,
                    Leg3VesselId = container.Leg3VesselId,
                    Leg4VesselId = container.Leg4VesselId,
                    Leg5VesselId = container.Leg5VesselId,
                    ExceptionDate = container.ExceptionDate,
                    ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                    HasException = container.HasException,
                    LastExceptionDescription = container.LastExceptionDescription,
                    ExceptionDescription = container.ExceptionDescription,
                    IsExceptionResolved = container.IsExceptionResolved,
                    EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                    EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                    EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                    EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                    EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                    EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                    OnCarriageGateOut = container.OnCarriageGateOut,
                    PreCarriageGateIn = container.PreCarriageGateIn,
                    UpdatedByPartner = container.UpdatedByPartner,
                    ContainerTypeId = container.ContainerTypeId,
                    GrossWeight = container.GrossWeight,
                    Volume = container.Volume,
                    VolumeUnitCode = container.VolumeUnitCode,
                    GrossWeightUnitCode = container.GrossWeightUnitCode,
                    AdditionalReference1 = container.AdditionalReference1,
                    AdditionalReference2 = container.AdditionalReference2,
                    AdditionalReference3 = container.AdditionalReference3,
                    AdditionalReference4 = container.AdditionalReference4,
                    MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                    MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                    MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                    MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                    MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                    MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                    OnCarriageETA = container.OnCarriageETA,
                    OnCarriageATA = container.OnCarriageATA,
                    RequestDate = container.RequestDate,
                    RecentResponseDate = container.RecentResponseDate,
                };

                MapCustomFields(containerPM, container);
            }
            return containerPM;
        }

        public ContainerPM GetContainerByNumberAndShipmentIdAndTenant(string containerNumber, string shipmentId, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = (from a in repository.context.Containers
                                   where a.Tenant == tenant && a.ContainerNumber == containerNumber && a.ShipmentId == shipmentId
                                   select a).FirstOrDefault();

            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "",
                    VesselName = container.VesselName,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                    ShipmentPickupFrom = container.ShipmentPickupFrom,
                    ShipmentPickupTo = container.ShipmentPickupTo,
                    ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                    ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                    ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                    ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                    ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                    ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                    ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                    ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                    ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                    ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                    ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                    ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                    ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                    ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                    ShipmentLastLegATA = container.ShipmentLastLegATA,
                    ShipmentLastLegETA = container.ShipmentLastLegETA,
                    ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                    ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                    ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                    ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                    ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                    ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                    ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                    ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                    ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                    ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                    ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                    ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                    PreCarriageLocation = container.PreCarriageLocation,
                    PreCarriageETD = container.PreCarriageETD,
                    PreCarriageATD = container.PreCarriageATD,
                    POLLocation = container.POLLocation,
                    EstimatedPOLArrival = container.EstimatedPOLArrival,
                    ActualPOLArrival = container.ActualPOLArrival,
                    EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                    ActualPOLLoaded = container.ActualPOLLoaded,
                    EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                    ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                    TransshipmentCount = container.TransshipmentCount,
                    Transshipment1Location = container.Transshipment1Location,
                    EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                    ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                    EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                    ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                    EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                    ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                    EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                    ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                    Transshipment2Location = container.Transshipment2Location,
                    EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                    ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                    EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                    ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                    EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                    ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                    EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                    ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                    Transshipment3Location = container.Transshipment3Location,
                    EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                    ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                    EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                    ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                    EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                    ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                    EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                    ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                    Transshipment4Location = container.Transshipment4Location,
                    EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                    ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                    EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                    ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                    EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                    ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                    EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                    ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                    Leg1Vessel = container.Leg1Vessel,
                    Leg1Voyage = container.Leg1Voyage,
                    Leg2Vessel = container.Leg2Vessel,
                    Leg2Voyage = container.Leg2Voyage,
                    Leg3Vessel = container.Leg3Vessel,
                    Leg3Voyage = container.Leg3Voyage,
                    Leg4Vessel = container.Leg4Vessel,
                    Leg4Voyage = container.Leg4Voyage,
                    Leg5Vessel = container.Leg5Vessel,
                    Leg5Voyage = container.Leg5Voyage,
                    PODLocation = container.PODLocation,
                    EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                    ActualPODVesselArrival = container.ActualPODVesselArrival,
                    EstimatedPODDischarge = container.EstimatedPODDischarge,
                    ActualPODDischarge = container.ActualPODDischarge,
                    EstimatedPODDeparture = container.EstimatedPODDeparture,
                    ActualPODDeparture = container.ActualPODDeparture,
                    OnCarriageLocation = container.OnCarriageLocation,
                    OnCarriageETD = container.OnCarriageETD,
                    OnCarriageATD = container.OnCarriageATD,
                    LIFLocation = container.LIFLocation,
                    EstimatedLIFArrival = container.EstimatedLIFArrival,
                    ActualLIFArrival = container.ActualLIFArrival,
                    EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                    ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                    GateIn = container.GateIn,
                    GateOut = container.GateOut,
                    EmptyReturnLocation = container.EmptyReturnLocation,
                    EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                    ActualEmptyReturn = container.ActualEmptyReturn,
                    CustomsReleaseState = container.CustomsReleaseState,
                    CustomsReleaseDate = container.CustomsReleaseDate,
                    CarrierReleaseState = container.CarrierReleaseState,
                    CarrierReleaseDate = container.CarrierReleaseDate,
                    AvailablityDate = container.AvailablityDate,
                    AvailabilityLocation = container.AvailabilityLocation,
                    FreeDays = container.FreeDays,
                    LastFreeDayDate = container.LastFreeDayDate,
                    ShipmentStatusId = container.ShipmentStatusId,
                    ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                    EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                    PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                    EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                    AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                    OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                    LIFLocationPortId = container.LIFLocationPortId,
                    POLLocationPortId = container.POLLocationPortId,
                    PODLocationPortId = container.PODLocationPortId,
                    Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                    Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                    Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                    Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                    TerminalId = container.TerminalId,
                    TerminalAddress = container.TerminalAddress,
                    TerminalAddressId = container.TerminalAddressId,
                    TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                    TerminalPhone = container.TerminalPhone,
                    ShipmentPickupETA = container.ShipmentPickupETA,
                    ShipmentPickupETD = container.ShipmentPickupETD,
                    ShipmentPickupATA = container.ShipmentPickupATA,
                    ShipmentPickupATD = container.ShipmentPickupATD,
                    ShipmentPreCarriageETA = container.ShipmentPickupATD,
                    ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                    ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                    ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                    ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                    ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                    ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                    ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                    ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                    ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                    ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                    ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                    ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                    ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                    ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                    ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                    ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                    ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                    ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                    ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                    ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                    ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                    ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                    ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                    ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                    ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                    ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                    ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                    ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                    ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                    ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                    ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                    ShipmentNumber = container.ShipmentNumber,
                    ShipmentTypeId = container.ShipmentTypeId,
                    ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                    OPClosed = container.OPClosed,
                    ContainersCount = container.ContainersCount,
                    HandlerId = container.HandlerId,
                    HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                    CustomerId = container.CustomerId,
                    CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                    ShipmentCreateDate = container.ShipmentCreateDate,
                    PODReceivedOnDate = container.PODReceivedOnDate,
                    IsAutomaticUpdates = container.IsAutomaticUpdates,
                    IsClosed = container.IsClosed,
                    ClosedDate = container.ClosedDate,
                    MasterEntityId = container.ShipmentId,
                    CustomerContactId = container.CustomerCard?.PrimaryContactId,
                    HandlerContactId = container.Handler?.Contact?.Id,
                    ConsigneeContactId = container.Shipment?.ConsigneeContactId,
                    ShipperContactId = container.Shipment?.ShipperContactId,
                    ShipperNotExporterContactId = container.Shipment?.ShipperNotExporterContactId,
                    FreightForwarderContactId = container.Shipment?.FreightForwarderContactId,
                    StatusId = container.StatusId,
                    IsCancelled = container.IsCancelled,
                    CancelledDate = container.CancelledDate,
                    ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                    ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                    Leg1VesselId = container.Leg1VesselId,
                    Leg2VesselId = container.Leg2VesselId,
                    Leg3VesselId = container.Leg3VesselId,
                    Leg4VesselId = container.Leg4VesselId,
                    Leg5VesselId = container.Leg5VesselId,
                    ExceptionDate = container.ExceptionDate,
                    ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                    HasException = container.HasException,
                    LastExceptionDescription = container.LastExceptionDescription,
                    ExceptionDescription = container.ExceptionDescription,
                    IsExceptionResolved = container.IsExceptionResolved,
                    EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                    EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                    EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                    EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                    EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                    EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                    OnCarriageGateOut = container.OnCarriageGateOut,
                    PreCarriageGateIn = container.PreCarriageGateIn,
                    UpdatedByPartner = container.UpdatedByPartner,
                    ContainerTypeId = container.ContainerTypeId,
                    GrossWeight = container.GrossWeight,
                    Volume = container.Volume,
                    VolumeUnitCode = container.VolumeUnitCode,
                    GrossWeightUnitCode = container.GrossWeightUnitCode,
                    AdditionalReference1 = container.AdditionalReference1,
                    AdditionalReference2 = container.AdditionalReference2,
                    AdditionalReference3 = container.AdditionalReference3,
                    AdditionalReference4 = container.AdditionalReference4,
                    MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                    MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                    MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                    MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                    MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                    MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                    OnCarriageETA = container.OnCarriageETA,
                    OnCarriageATA = container.OnCarriageATA,
                    RequestDate = container.RequestDate,
                    RecentResponseDate = container.RecentResponseDate,
                };

                MapCustomFields(containerPM, container);
            }

            return containerPM;
        }

        public ContainerPM GetCancelledContainerByShipmentId(string shipmentId, string containerNumber, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = repository.GetCancelledContainerByShipmentId(shipmentId, containerNumber, tenant);
            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                    ShipmentPickupFrom = container.ShipmentPickupFrom,
                    ShipmentPickupTo = container.ShipmentPickupTo,
                    ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                    ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                    ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                    ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                    ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                    ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                    ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                    ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                    ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                    ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                    ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                    ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                    ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                    ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                    ShipmentLastLegATA = container.ShipmentLastLegATA,
                    ShipmentLastLegETA = container.ShipmentLastLegETA,
                    ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                    ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                    ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                    ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                    ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                    ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                    ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                    ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                    ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                    ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                    ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                    ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                    OnCarriageLocation = container.OnCarriageLocation,
                    OnCarriageETD = container.OnCarriageETD,
                    OnCarriageATD = container.OnCarriageATD,
                    POLLocation = container.POLLocation,
                    EstimatedPOLArrival = container.EstimatedPOLArrival,
                    ActualPOLArrival = container.ActualPOLArrival,
                    EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                    ActualPOLLoaded = container.ActualPOLLoaded,
                    EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                    ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                    TransshipmentCount = container.TransshipmentCount,
                    Transshipment1Location = container.Transshipment1Location,
                    EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                    ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                    EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                    ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                    EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                    ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                    EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                    ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                    Transshipment2Location = container.Transshipment2Location,
                    EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                    ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                    EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                    ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                    EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                    ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                    EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                    ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                    Transshipment3Location = container.Transshipment3Location,
                    EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                    ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                    EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                    ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                    EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                    ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                    EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                    ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                    Transshipment4Location = container.Transshipment4Location,
                    EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                    ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                    EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                    ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                    EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                    ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                    EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                    ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                    Leg1Vessel = container.Leg1Vessel,
                    Leg1Voyage = container.Leg1Voyage,
                    Leg2Vessel = container.Leg2Vessel,
                    Leg2Voyage = container.Leg2Voyage,
                    Leg3Vessel = container.Leg3Vessel,
                    Leg3Voyage = container.Leg3Voyage,
                    Leg4Vessel = container.Leg4Vessel,
                    Leg4Voyage = container.Leg4Voyage,
                    Leg5Vessel = container.Leg5Vessel,
                    Leg5Voyage = container.Leg5Voyage,
                    PODLocation = container.PODLocation,
                    EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                    ActualPODVesselArrival = container.ActualPODVesselArrival,
                    EstimatedPODDischarge = container.EstimatedPODDischarge,
                    ActualPODDischarge = container.ActualPODDischarge,
                    EstimatedPODDeparture = container.EstimatedPODDeparture,
                    ActualPODDeparture = container.ActualPODDeparture,
                    PreCarriageLocation = container.PreCarriageLocation,
                    PreCarriageETD = container.PreCarriageETD,
                    PreCarriageATD = container.PreCarriageATD,
                    LIFLocation = container.LIFLocation,
                    EstimatedLIFArrival = container.EstimatedLIFArrival,
                    ActualLIFArrival = container.ActualLIFArrival,
                    EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                    ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                    GateIn = container.GateIn,
                    GateOut = container.GateOut,
                    EmptyReturnLocation = container.EmptyReturnLocation,
                    EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                    ActualEmptyReturn = container.ActualEmptyReturn,
                    CustomsReleaseState = container.CustomsReleaseState,
                    CustomsReleaseDate = container.CustomsReleaseDate,
                    CarrierReleaseState = container.CarrierReleaseState,
                    CarrierReleaseDate = container.CarrierReleaseDate,
                    AvailablityDate = container.AvailablityDate,
                    AvailabilityLocation = container.AvailabilityLocation,
                    FreeDays = container.FreeDays,
                    LastFreeDayDate = container.LastFreeDayDate,
                    ShipmentStatusId = container.ShipmentStatusId,
                    ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                    EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                    PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                    EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                    AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                    OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                    LIFLocationPortId = container.LIFLocationPortId,
                    POLLocationPortId = container.POLLocationPortId,
                    PODLocationPortId = container.PODLocationPortId,
                    Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                    Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                    Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                    Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                    TerminalId = container.TerminalId,
                    TerminalAddress = container.TerminalAddress,
                    TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                    TerminalPhone = container.TerminalPhone,
                    TerminalAddressId = container.TerminalAddressId,
                    ShipmentPickupETA = container.ShipmentPickupETA,
                    ShipmentPickupETD = container.ShipmentPickupETD,
                    ShipmentPickupATA = container.ShipmentPickupATA,
                    ShipmentPickupATD = container.ShipmentPickupATD,
                    ShipmentPreCarriageETA = container.ShipmentPickupATD,
                    ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                    ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                    ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                    ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                    ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                    ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                    ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                    ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                    ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                    ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                    ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                    ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                    ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                    ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                    ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                    ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                    ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                    ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                    ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                    ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                    ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                    ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                    ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                    ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                    ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                    ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                    ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                    ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                    ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                    ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                    ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                    ShipmentNumber = container.ShipmentNumber,
                    ShipmentTypeId = container.ShipmentTypeId,
                    ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                    OPClosed = container.OPClosed,
                    ContainersCount = container.ContainersCount,
                    HandlerId = container.HandlerId,
                    HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                    CustomerId = container.CustomerId,
                    CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                    ShipmentCreateDate = container.ShipmentCreateDate,
                    PODReceivedOnDate = container.PODReceivedOnDate,
                    IsAutomaticUpdates = container.IsAutomaticUpdates,
                    IsClosed = container.IsClosed,
                    ClosedDate = container.ClosedDate,
                    StatusId = container.StatusId,
                    IsCancelled = container.IsCancelled,
                    CancelledDate = container.CancelledDate,
                    ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                    ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                    Leg1VesselId = container.Leg1VesselId,
                    Leg2VesselId = container.Leg2VesselId,
                    Leg3VesselId = container.Leg3VesselId,
                    Leg4VesselId = container.Leg4VesselId,
                    Leg5VesselId = container.Leg5VesselId,
                    ExceptionDate = container.ExceptionDate,
                    ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                    HasException = container.HasException,
                    LastExceptionDescription = container.LastExceptionDescription,
                    ExceptionDescription = container.ExceptionDescription,
                    IsExceptionResolved = container.IsExceptionResolved,
                    EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                    EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                    EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                    EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                    EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                    EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                    OnCarriageGateOut = container.OnCarriageGateOut,
                    PreCarriageGateIn = container.PreCarriageGateIn,
                    UpdatedByPartner = container.UpdatedByPartner,
                    ContainerTypeId = container.ContainerTypeId,
                    GrossWeight = container.GrossWeight,
                    Volume = container.Volume,
                    VolumeUnitCode = container.VolumeUnitCode,
                    GrossWeightUnitCode = container.GrossWeightUnitCode,
                    AdditionalReference1 = container.AdditionalReference1,
                    AdditionalReference2 = container.AdditionalReference2,
                    AdditionalReference3 = container.AdditionalReference3,
                    AdditionalReference4 = container.AdditionalReference4,
                    MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.CountryId,
                    MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.CountryId,
                    MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                    MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                    MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                    MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                    RequestDate = container.RequestDate,
                    RecentResponseDate = container.RecentResponseDate,
                };

                MapCustomFields(containerPM, container);
            }
            return containerPM;
        }

        public ContainerPM GetContainerByNumberAndShipmentNumber(string containerNumber, string shipmentNumber, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = (from a in repository.context.Containers
                                   where a.Tenant == tenant && a.ContainerNumber == containerNumber && a.ShipmentNumber == shipmentNumber
                                   select a).FirstOrDefault();

            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "",
                    VesselName = container.VesselName,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                    ShipmentPickupFrom = container.ShipmentPickupFrom,
                    ShipmentPickupTo = container.ShipmentPickupTo,
                    ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId,
                    ShipmentPreCarriageToId = container.ShipmentPreCarriageToId,
                    ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId,
                    ShipmentMainCarriageToId = container.ShipmentMainCarriageToId,
                    ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId,
                    ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId,
                    ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId,
                    ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId,
                    ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId,
                    ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId,
                    ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId,
                    ShipmentOnCarriageToId = container.ShipmentOnCarriageToId,
                    ShipmentDeliveryFrom = container.ShipmentDeliveryFrom,
                    ShipmentDeliveryTo = container.ShipmentDeliveryTo,
                    ShipmentLastLegATA = container.ShipmentLastLegATA,
                    ShipmentLastLegETA = container.ShipmentLastLegETA,
                    ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "",
                    ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "",
                    ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "",
                    ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "",
                    ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "",
                    ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "",
                    ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "",
                    ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "",
                    ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "",
                    ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "",
                    ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "",
                    ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "",
                    PreCarriageLocation = container.PreCarriageLocation,
                    PreCarriageETD = container.PreCarriageETD,
                    PreCarriageATD = container.PreCarriageATD,
                    POLLocation = container.POLLocation,
                    EstimatedPOLArrival = container.EstimatedPOLArrival,
                    ActualPOLArrival = container.ActualPOLArrival,
                    EstimatedPOLLoaded = container.EstimatedPOLLoaded,
                    ActualPOLLoaded = container.ActualPOLLoaded,
                    EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture,
                    ActualPOLVesselDeparture = container.ActualPOLVesselDeparture,
                    TransshipmentCount = container.TransshipmentCount,
                    Transshipment1Location = container.Transshipment1Location,
                    EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival,
                    ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival,
                    EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge,
                    ActualTransshipment1Discharge = container.ActualTransshipment1Discharge,
                    EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded,
                    ActualTransshipment1Loaded = container.ActualTransshipment1Loaded,
                    EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture,
                    ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture,
                    Transshipment2Location = container.Transshipment2Location,
                    EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival,
                    ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival,
                    EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge,
                    ActualTransshipment2Discharge = container.ActualTransshipment2Discharge,
                    EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded,
                    ActualTransshipment2Loaded = container.ActualTransshipment2Loaded,
                    EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture,
                    ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture,
                    Transshipment3Location = container.Transshipment3Location,
                    EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival,
                    ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival,
                    EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge,
                    ActualTransshipment3Discharge = container.ActualTransshipment3Discharge,
                    EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded,
                    ActualTransshipment3Loaded = container.ActualTransshipment3Loaded,
                    EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture,
                    ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture,
                    Transshipment4Location = container.Transshipment4Location,
                    EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival,
                    ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival,
                    EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge,
                    ActualTransshipment4Discharge = container.ActualTransshipment4Discharge,
                    EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded,
                    ActualTransshipment4Loaded = container.ActualTransshipment4Loaded,
                    EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture,
                    ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture,
                    Leg1Vessel = container.Leg1Vessel,
                    Leg1Voyage = container.Leg1Voyage,
                    Leg2Vessel = container.Leg2Vessel,
                    Leg2Voyage = container.Leg2Voyage,
                    Leg3Vessel = container.Leg3Vessel,
                    Leg3Voyage = container.Leg3Voyage,
                    Leg4Vessel = container.Leg4Vessel,
                    Leg4Voyage = container.Leg4Voyage,
                    Leg5Vessel = container.Leg5Vessel,
                    Leg5Voyage = container.Leg5Voyage,
                    PODLocation = container.PODLocation,
                    EstimatedPODVesselArrival = container.EstimatedPODVesselArrival,
                    ActualPODVesselArrival = container.ActualPODVesselArrival,
                    EstimatedPODDischarge = container.EstimatedPODDischarge,
                    ActualPODDischarge = container.ActualPODDischarge,
                    EstimatedPODDeparture = container.EstimatedPODDeparture,
                    ActualPODDeparture = container.ActualPODDeparture,
                    OnCarriageLocation = container.OnCarriageLocation,
                    OnCarriageETD = container.OnCarriageETD,
                    OnCarriageATD = container.OnCarriageATD,
                    LIFLocation = container.LIFLocation,
                    EstimatedLIFArrival = container.EstimatedLIFArrival,
                    ActualLIFArrival = container.ActualLIFArrival,
                    EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture,
                    ActualOnCarriageDeparture = container.ActualOnCarriageDeparture,
                    GateIn = container.GateIn,
                    GateOut = container.GateOut,
                    EmptyReturnLocation = container.EmptyReturnLocation,
                    EstimatedEmptyReturn = container.EstimatedEmptyReturn,
                    ActualEmptyReturn = container.ActualEmptyReturn,
                    CustomsReleaseState = container.CustomsReleaseState,
                    CustomsReleaseDate = container.CustomsReleaseDate,
                    CarrierReleaseState = container.CarrierReleaseState,
                    CarrierReleaseDate = container.CarrierReleaseDate,
                    AvailablityDate = container.AvailablityDate,
                    AvailabilityLocation = container.AvailabilityLocation,
                    FreeDays = container.FreeDays,
                    LastFreeDayDate = container.LastFreeDayDate,
                    ShipmentStatusId = container.ShipmentStatusId,
                    ShipmentStatusName = container.ShipmentEntityStatus != null ? container.ShipmentEntityStatus.Name : null,
                    EmptyPickupLocationPortId = container.EmptyPickupLocationPortId,
                    PreCarriageLocationPortId = container.PreCarriageLocationPortId,
                    EmptyReturnLocationPortId = container.EmptyReturnLocationPortId,
                    AvailabilityLocationPortId = container.AvailabilityLocationPortId,
                    OnCarriageLocationPortId = container.OnCarriageLocationPortId,
                    LIFLocationPortId = container.LIFLocationPortId,
                    POLLocationPortId = container.POLLocationPortId,
                    PODLocationPortId = container.PODLocationPortId,
                    Transshipment1LocationPortId = container.Transshipment1LocationPortId,
                    Transshipment2LocationPortId = container.Transshipment2LocationPortId,
                    Transshipment3LocationPortId = container.Transshipment3LocationPortId,
                    Transshipment4LocationPortId = container.Transshipment4LocationPortId,
                    TerminalId = container.TerminalId,
                    TerminalAddress = container.TerminalAddress,
                    TerminalAddressId = container.TerminalAddressId,
                    TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "",
                    TerminalPhone = container.TerminalPhone,
                    ShipmentPickupETA = container.ShipmentPickupETA,
                    ShipmentPickupETD = container.ShipmentPickupETD,
                    ShipmentPickupATA = container.ShipmentPickupATA,
                    ShipmentPickupATD = container.ShipmentPickupATD,
                    ShipmentPreCarriageETA = container.ShipmentPickupATD,
                    ShipmentPreCarriageETD = container.ShipmentPreCarriageETD,
                    ShipmentPreCarriageATA = container.ShipmentPreCarriageATA,
                    ShipmentPreCarriageATD = container.ShipmentPreCarriageATD,
                    ShipmentMainCarriageETA = container.ShipmentMainCarriageETA,
                    ShipmentMainCarriageETD = container.ShipmentMainCarriageETD,
                    ShipmentMainCarriageATA = container.ShipmentMainCarriageATA,
                    ShipmentMainCarriageATD = container.ShipmentMainCarriageATD,
                    ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA,
                    ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD,
                    ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA,
                    ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD,
                    ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA,
                    ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD,
                    ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA,
                    ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD,
                    ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA,
                    ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD,
                    ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA,
                    ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD,
                    ShipmentOnCarriageETA = container.ShipmentOnCarriageETA,
                    ShipmentOnCarriageETD = container.ShipmentOnCarriageETD,
                    ShipmentOnCarriageATA = container.ShipmentOnCarriageATA,
                    ShipmentOnCarriageATD = container.ShipmentOnCarriageATD,
                    ShipmentDeliveryETA = container.ShipmentDeliveryETA,
                    ShipmentDeliveryETD = container.ShipmentDeliveryETD,
                    ShipmentDeliveryATA = container.ShipmentDeliveryATA,
                    ShipmentDeliveryATD = container.ShipmentDeliveryATD,
                    ShipmentOriginAgentId = container.ShipmentOriginAgentId,
                    ShipmentDestinationAgentId = container.ShipmentDestinationAgentId,
                    ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "",
                    ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "",
                    ShipmentNumber = container.ShipmentNumber,
                    ShipmentTypeId = container.ShipmentTypeId,
                    ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "",
                    OPClosed = container.OPClosed,
                    ContainersCount = container.ContainersCount,
                    HandlerId = container.HandlerId,
                    HandlerName = container.Handler != null ? (container.Handler.Contact != null ? container.Handler.Contact.EnglishName : "") : "",
                    CustomerId = container.CustomerId,
                    CustomerName = container.CustomerCard != null ? container.CustomerCard.EnglishName : "",
                    ShipmentCreateDate = container.ShipmentCreateDate,
                    PODReceivedOnDate = container.PODReceivedOnDate,
                    IsAutomaticUpdates = container.IsAutomaticUpdates,
                    IsClosed = container.IsClosed,
                    ClosedDate = container.ClosedDate,
                    MasterEntityId = container.ShipmentId,
                    CustomerContactId = container.CustomerCard?.PrimaryContactId,
                    HandlerContactId = container.Handler?.Contact?.Id,
                    ConsigneeContactId = container.Shipment?.ConsigneeContactId,
                    ShipperContactId = container.Shipment?.ShipperContactId,
                    ShipperNotExporterContactId = container.Shipment?.ShipperNotExporterContactId,
                    FreightForwarderContactId = container.Shipment?.FreightForwarderContactId,
                    StatusId = container.StatusId,
                    IsCancelled = container.IsCancelled,
                    CancelledDate = container.CancelledDate,
                    ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId,
                    ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "",
                    Leg1VesselId = container.Leg1VesselId,
                    Leg2VesselId = container.Leg2VesselId,
                    Leg3VesselId = container.Leg3VesselId,
                    Leg4VesselId = container.Leg4VesselId,
                    Leg5VesselId = container.Leg5VesselId,
                    ExceptionDate = container.ExceptionDate,
                    ExceptionResolvedDescription = container.ExceptionResolvedDescription,
                    HasException = container.HasException,
                    LastExceptionDescription = container.LastExceptionDescription,
                    ExceptionDescription = container.ExceptionDescription,
                    IsExceptionResolved = container.IsExceptionResolved,
                    EmptyContainerReturnTo = container.EmptyContainerReturnTo,
                    EmptyContainerReturnFrom = container.EmptyContainerReturnFrom,
                    EmptyContainerReturnETA = container.EmptyContainerReturnETA,
                    EmptyContainerReturnATA = container.EmptyContainerReturnATA,
                    EmptyContainerReturnATD = container.EmptyContainerReturnATD,
                    EmptyContainerReturnETD = container.EmptyContainerReturnETD,
                    OnCarriageGateOut = container.OnCarriageGateOut,
                    PreCarriageGateIn = container.PreCarriageGateIn,
                    UpdatedByPartner = container.UpdatedByPartner,
                    ContainerTypeId = container.ContainerTypeId,
                    GrossWeight = container.GrossWeight,
                    Volume = container.Volume,
                    VolumeUnitCode = container.VolumeUnitCode,
                    GrossWeightUnitCode = container.GrossWeightUnitCode,
                    AdditionalReference1 = container.AdditionalReference1,
                    AdditionalReference2 = container.AdditionalReference2,
                    AdditionalReference3 = container.AdditionalReference3,
                    AdditionalReference4 = container.AdditionalReference4,
                    MainCarriageFromCountryId = container.ShipmentMainCarriageFromPort == null ? null : container.ShipmentMainCarriageFromPort.Country.Id,
                    MainCarriageToCountryId = container.ShipmentMainCarriageToPort == null ? null : container.ShipmentMainCarriageToPort.Country.Id,
                    MainCarriageFromCountryName = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryName : "",
                    MainCarriageToCountryName = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryName : "",
                    MainCarriageFromCountryCode = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CountryCode : "",
                    MainCarriageToCountryCode = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CountryCode : "",
                    OnCarriageETA = container.OnCarriageETA,
                    OnCarriageATA = container.OnCarriageATA,
                    RequestDate = container.RequestDate,
                    RecentResponseDate = container.RecentResponseDate,
                };
            }

            return containerPM;
        }

        public List<ContainerPM> GetContainerPMsByIds(List<string> containerIds, int tenant)
        {
            isMultipleUpdate = true;
            if (containerIds.Count() == 0) return null;

            List<Container> containers = repository.GetContainersFromIds(containerIds, tenant);
            if (containers.Count() == 0) return null;

            List<ContainerPM> containerPMs = new List<ContainerPM>();
            foreach (Container container in containers)
            {
                containerPMs.Add(new ContainerPM() { Id = container.Id });
            }

            Parallel.ForEach(containers, (container) =>
            {
                ContainerPM containerPM = containerPMs.Where(a => a.Id == container.Id).FirstOrDefault();
                containerPM = MapContainerToContainerPM(containerPM, container);
            });

            return containerPMs;
        }

        public ContainerPM MapContainerToContainerPM(ContainerPM containerPM, Container container)
        {
            containerPM.Id = container.Id;
            containerPM.Tenant = container.Tenant;
            containerPM.CreateDate = container.CreateDate;
            containerPM.CreatedByUserId = container.CreatedByUserId;
            containerPM.UpdateDate = container.UpdateDate;
            containerPM.UpdatedByUserId = container.UpdatedByUserId;
            containerPM.MainCarriageCarrierId = container.MainCarriageCarrierId;
            containerPM.MainCarriageCarrierNumber = container.MainCarriageCarrierNumber;
            containerPM.MainCarriageATA = container.MainCarriageATA;
            containerPM.MainCarriageATD = container.MainCarriageATD;
            containerPM.MainCarriageETA = container.MainCarriageETA;
            containerPM.MainCarriageETD = container.MainCarriageETD;
            containerPM.ContainerNumber = container.ContainerNumber;
            containerPM.MainCarriageVesselId = container.MainCarriageVesselId;
            containerPM.ShipmentPackagesId = container.ShipmentPackagesId;
            containerPM.SearchFields = container.SearchFields;
            containerPM.DischargeDate = container.DischargeDate;
            containerPM.Master = container.Master;
            containerPM.CarrierName = container.CarrierCard != null ? container.CarrierCard.EnglishName : "";
            containerPM.VesselName = container.VesselName;
            containerPM.ShipmentId = container.ShipmentId;
            containerPM.ActualEmptyPickupDate = container.ActualEmptyPickupDate;
            containerPM.EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate;
            containerPM.CurrentStatus = container.CurrentStatus;
            containerPM.CurrentStatusDate = container.CurrentStatusDate;
            containerPM.HasContainerException = container.HasContainerException;
            containerPM.CurrentLocation = container.CurrentLocation;
            containerPM.EmptyPickupLocation = container.EmptyPickupLocation;
            containerPM.DepartureLocation = container.DepartureLocation;
            containerPM.DestinationLocation = container.DestinationLocation;
            containerPM.ShipmentPickupFrom = container.ShipmentPickupFrom;
            containerPM.ShipmentPickupTo = container.ShipmentPickupTo;
            containerPM.ShipmentPreCarriageFromId = container.ShipmentPreCarriageFromId;
            containerPM.ShipmentPreCarriageToId = container.ShipmentPreCarriageToId;
            containerPM.ShipmentMainCarriageFromId = container.ShipmentMainCarriageFromId;
            containerPM.ShipmentMainCarriageToId = container.ShipmentMainCarriageToId;
            containerPM.ShipmentTransshipment1FromId = container.ShipmentTransshipment1FromId;
            containerPM.ShipmentTransshipment1ToId = container.ShipmentTransshipment1ToId;
            containerPM.ShipmentTransshipment2FromId = container.ShipmentTransshipment2FromId;
            containerPM.ShipmentTransshipment2ToId = container.ShipmentTransshipment2ToId;
            containerPM.ShipmentTransshipment3FromId = container.ShipmentTransshipment3FromId;
            containerPM.ShipmentTransshipment3ToId = container.ShipmentTransshipment3ToId;
            containerPM.ShipmentOnCarriageFromId = container.ShipmentOnCarriageFromId;
            containerPM.ShipmentOnCarriageToId = container.ShipmentOnCarriageToId;
            containerPM.ShipmentPreCarriageFrom = container.ShipmentPreCarriageFromPort != null ? container.ShipmentPreCarriageFromPort.CombinedCode : "";
            containerPM.ShipmentPreCarriageTo = container.ShipmentPreCarriageToPort != null ? container.ShipmentPreCarriageToPort.CombinedCode : "";
            containerPM.ShipmentMainCarriageFrom = container.ShipmentMainCarriageFromPort != null ? container.ShipmentMainCarriageFromPort.CombinedCode : "";
            containerPM.ShipmentMainCarriageTo = container.ShipmentMainCarriageToPort != null ? container.ShipmentMainCarriageToPort.CombinedCode : "";
            containerPM.ShipmentTransshipment1From = container.ShipmentTransshipment1FromPort != null ? container.ShipmentTransshipment1FromPort.CombinedCode : "";
            containerPM.ShipmentTransshipment1To = container.ShipmentTransshipment1ToPort != null ? container.ShipmentTransshipment1ToPort.CombinedCode : "";
            containerPM.ShipmentTransshipment2From = container.ShipmentTransshipment2FromPort != null ? container.ShipmentTransshipment2FromPort.CombinedCode : "";
            containerPM.ShipmentTransshipment2To = container.ShipmentTransshipment2ToPort != null ? container.ShipmentTransshipment2ToPort.CombinedCode : "";
            containerPM.ShipmentTransshipment3From = container.ShipmentTransshipment3FromPort != null ? container.ShipmentTransshipment3FromPort.CombinedCode : "";
            containerPM.ShipmentTransshipment3To = container.ShipmentTransshipment3ToPort != null ? container.ShipmentTransshipment3ToPort.CombinedCode : "";
            containerPM.ShipmentOnCarriageFrom = container.ShipmentOnCarriageFromPort != null ? container.ShipmentOnCarriageFromPort.CombinedCode : "";
            containerPM.ShipmentOnCarriageTo = container.ShipmentOnCarriageToPort != null ? container.ShipmentOnCarriageToPort.CombinedCode : "";
            containerPM.ShipmentDeliveryFrom = container.ShipmentDeliveryFrom;
            containerPM.ShipmentDeliveryTo = container.ShipmentDeliveryTo;
            containerPM.ShipmentLastLegATA = container.ShipmentLastLegATA;
            containerPM.ShipmentLastLegETA = container.ShipmentLastLegETA;
            containerPM.PreCarriageLocation = container.PreCarriageLocation;
            containerPM.PreCarriageETD = container.PreCarriageETD;
            containerPM.PreCarriageATD = container.PreCarriageATD;
            containerPM.POLLocation = container.POLLocation;
            containerPM.EstimatedPOLArrival = container.EstimatedPOLArrival;
            containerPM.ActualPOLArrival = container.ActualPOLArrival;
            containerPM.EstimatedPOLLoaded = container.EstimatedPOLLoaded;
            containerPM.ActualPOLLoaded = container.ActualPOLLoaded;
            containerPM.EstimatedPOLVesselDeparture = container.EstimatedPOLVesselDeparture;
            containerPM.ActualPOLVesselDeparture = container.ActualPOLVesselDeparture;
            containerPM.TransshipmentCount = container.TransshipmentCount;
            containerPM.Transshipment1Location = container.Transshipment1Location;
            containerPM.EstimatedTrans1VesselArrival = container.EstimatedTrans1VesselArrival;
            containerPM.ActualTransshipment1VesselArrival = container.ActualTransshipment1VesselArrival;
            containerPM.EstimatedTransshipment1Discharge = container.EstimatedTransshipment1Discharge;
            containerPM.ActualTransshipment1Discharge = container.ActualTransshipment1Discharge;
            containerPM.EstimatedTransshipment1Loaded = container.EstimatedTransshipment1Loaded;
            containerPM.ActualTransshipment1Loaded = container.ActualTransshipment1Loaded;
            containerPM.EstimatedTrans1VesselDeparture = container.EstimatedTrans1VesselDeparture;
            containerPM.ActualTrans1VesselDeparture = container.ActualTrans1VesselDeparture;
            containerPM.Transshipment2Location = container.Transshipment2Location;
            containerPM.EstimatedTrans2VesselArrival = container.EstimatedTrans2VesselArrival;
            containerPM.ActualTransshipment2VesselArrival = container.ActualTransshipment2VesselArrival;
            containerPM.EstimatedTransshipment2Discharge = container.EstimatedTransshipment2Discharge;
            containerPM.ActualTransshipment2Discharge = container.ActualTransshipment2Discharge;
            containerPM.EstimatedTransshipment2Loaded = container.EstimatedTransshipment2Loaded;
            containerPM.ActualTransshipment2Loaded = container.ActualTransshipment2Loaded;
            containerPM.EstimatedTrans2VesselDeparture = container.EstimatedTrans2VesselDeparture;
            containerPM.ActualTrans2VesselDeparture = container.ActualTrans2VesselDeparture;
            containerPM.Transshipment3Location = container.Transshipment3Location;
            containerPM.EstimatedTrans3VesselArrival = container.EstimatedTrans3VesselArrival;
            containerPM.ActualTransshipment3VesselArrival = container.ActualTransshipment3VesselArrival;
            containerPM.EstimatedTransshipment3Discharge = container.EstimatedTransshipment3Discharge;
            containerPM.ActualTransshipment3Discharge = container.ActualTransshipment3Discharge;
            containerPM.EstimatedTransshipment3Loaded = container.EstimatedTransshipment3Loaded;
            containerPM.ActualTransshipment3Loaded = container.ActualTransshipment3Loaded;
            containerPM.EstimatedTrans3VesselDeparture = container.EstimatedTrans3VesselDeparture;
            containerPM.ActualTrans3VesselDeparture = container.ActualTrans3VesselDeparture;
            containerPM.Transshipment4Location = container.Transshipment4Location;
            containerPM.EstimatedTrans4VesselArrival = container.EstimatedTrans4VesselArrival;
            containerPM.ActualTransshipment4VesselArrival = container.ActualTransshipment4VesselArrival;
            containerPM.EstimatedTransshipment4Discharge = container.EstimatedTransshipment4Discharge;
            containerPM.ActualTransshipment4Discharge = container.ActualTransshipment4Discharge;
            containerPM.EstimatedTransshipment4Loaded = container.EstimatedTransshipment4Loaded;
            containerPM.ActualTransshipment4Loaded = container.ActualTransshipment4Loaded;
            containerPM.EstimatedTrans4VesselDeparture = container.EstimatedTrans4VesselDeparture;
            containerPM.ActualTrans4VesselDeparture = container.ActualTrans4VesselDeparture;
            containerPM.Leg1Vessel = container.Leg1Vessel;
            containerPM.Leg1Voyage = container.Leg1Voyage;
            containerPM.Leg2Vessel = container.Leg2Vessel;
            containerPM.Leg2Voyage = container.Leg2Voyage;
            containerPM.Leg3Vessel = container.Leg3Vessel;
            containerPM.Leg3Voyage = container.Leg3Voyage;
            containerPM.Leg4Vessel = container.Leg4Vessel;
            containerPM.Leg4Voyage = container.Leg4Voyage;
            containerPM.Leg5Vessel = container.Leg5Vessel;
            containerPM.Leg5Voyage = container.Leg5Voyage;
            containerPM.PODLocation = container.PODLocation;
            containerPM.EstimatedPODVesselArrival = container.EstimatedPODVesselArrival;
            containerPM.ActualPODVesselArrival = container.ActualPODVesselArrival;
            containerPM.EstimatedPODDischarge = container.EstimatedPODDischarge;
            containerPM.ActualPODDischarge = container.ActualPODDischarge;
            containerPM.EstimatedPODDeparture = container.EstimatedPODDeparture;
            containerPM.ActualPODDeparture = container.ActualPODDeparture;
            containerPM.OnCarriageLocation = container.OnCarriageLocation;
            containerPM.OnCarriageETD = container.OnCarriageETD;
            containerPM.OnCarriageATD = container.OnCarriageATD;
            containerPM.LIFLocation = container.LIFLocation;
            containerPM.EstimatedLIFArrival = container.EstimatedLIFArrival;
            containerPM.ActualLIFArrival = container.ActualLIFArrival;
            containerPM.EstimatedOnCarriageDeparture = container.EstimatedOnCarriageDeparture;
            containerPM.ActualOnCarriageDeparture = container.ActualOnCarriageDeparture;
            containerPM.GateIn = container.GateIn;
            containerPM.GateOut = container.GateOut;
            containerPM.EmptyReturnLocation = container.EmptyReturnLocation;
            containerPM.EstimatedEmptyReturn = container.EstimatedEmptyReturn;
            containerPM.ActualEmptyReturn = container.ActualEmptyReturn;
            containerPM.CustomsReleaseState = container.CustomsReleaseState;
            containerPM.CustomsReleaseDate = container.CustomsReleaseDate;
            containerPM.CarrierReleaseState = container.CarrierReleaseState;
            containerPM.CarrierReleaseDate = container.CarrierReleaseDate;
            containerPM.AvailablityDate = container.AvailablityDate;
            containerPM.AvailabilityLocation = container.AvailabilityLocation;
            containerPM.FreeDays = container.FreeDays;
            containerPM.LastFreeDayDate = container.LastFreeDayDate;
            containerPM.ShipmentStatusId = container.ShipmentStatusId;
            containerPM.ShipmentStatusName = container.ShipmentEntityStatus?.Name;
            containerPM.EmptyPickupLocationPortId = container.EmptyPickupLocationPortId;
            containerPM.PreCarriageLocationPortId = container.PreCarriageLocationPortId;
            containerPM.EmptyReturnLocationPortId = container.EmptyReturnLocationPortId;
            containerPM.AvailabilityLocationPortId = container.AvailabilityLocationPortId;
            containerPM.OnCarriageLocationPortId = container.OnCarriageLocationPortId;
            containerPM.LIFLocationPortId = container.LIFLocationPortId;
            containerPM.POLLocationPortId = container.POLLocationPortId;
            containerPM.PODLocationPortId = container.PODLocationPortId;
            containerPM.Transshipment1LocationPortId = container.Transshipment1LocationPortId;
            containerPM.Transshipment2LocationPortId = container.Transshipment2LocationPortId;
            containerPM.Transshipment3LocationPortId = container.Transshipment3LocationPortId;
            containerPM.Transshipment4LocationPortId = container.Transshipment4LocationPortId;
            containerPM.TerminalId = container.TerminalId;
            containerPM.TerminalAddress = container.TerminalAddress;
            containerPM.TerminalName = container.TerminalCard != null ? container.TerminalCard.EnglishName : "";
            containerPM.TerminalPhone = container.TerminalPhone;
            containerPM.TerminalAddressId = container.TerminalAddressId;
            containerPM.ShipmentPickupETA = container.ShipmentPickupETA;
            containerPM.ShipmentPickupETD = container.ShipmentPickupETD;
            containerPM.ShipmentPickupATA = container.ShipmentPickupATA;
            containerPM.ShipmentPickupATD = container.ShipmentPickupATD;
            containerPM.ShipmentPreCarriageETA = container.ShipmentPreCarriageETA;
            containerPM.ShipmentPreCarriageETD = container.ShipmentPreCarriageETD;
            containerPM.ShipmentPreCarriageATA = container.ShipmentPreCarriageATA;
            containerPM.ShipmentPreCarriageATD = container.ShipmentPreCarriageATD;
            containerPM.ShipmentMainCarriageETA = container.ShipmentMainCarriageETA;
            containerPM.ShipmentMainCarriageETD = container.ShipmentMainCarriageETD;
            containerPM.ShipmentMainCarriageATA = container.ShipmentMainCarriageATA;
            containerPM.ShipmentMainCarriageATD = container.ShipmentMainCarriageATD;
            containerPM.ShipmentTransshipment1ETA = container.ShipmentTransshipment1ETA;
            containerPM.ShipmentTransshipment1ETD = container.ShipmentTransshipment1ETD;
            containerPM.ShipmentTransshipment1ATA = container.ShipmentTransshipment1ATA;
            containerPM.ShipmentTransshipment1ATD = container.ShipmentTransshipment1ATD;
            containerPM.ShipmentTransshipment2ETA = container.ShipmentTransshipment2ETA;
            containerPM.ShipmentTransshipment2ETD = container.ShipmentTransshipment2ETD;
            containerPM.ShipmentTransshipment2ATA = container.ShipmentTransshipment2ATA;
            containerPM.ShipmentTransshipment2ATD = container.ShipmentTransshipment2ATD;
            containerPM.ShipmentTransshipment3ETA = container.ShipmentTransshipment3ETA;
            containerPM.ShipmentTransshipment3ETD = container.ShipmentTransshipment3ETD;
            containerPM.ShipmentTransshipment3ATA = container.ShipmentTransshipment3ATA;
            containerPM.ShipmentTransshipment3ATD = container.ShipmentTransshipment3ATD;
            containerPM.ShipmentOnCarriageETA = container.ShipmentOnCarriageETA;
            containerPM.ShipmentOnCarriageETD = container.ShipmentOnCarriageETD;
            containerPM.ShipmentOnCarriageATA = container.ShipmentOnCarriageATA;
            containerPM.ShipmentOnCarriageATD = container.ShipmentOnCarriageATD;
            containerPM.ShipmentDeliveryETA = container.ShipmentDeliveryETA;
            containerPM.ShipmentDeliveryETD = container.ShipmentDeliveryETD;
            containerPM.ShipmentDeliveryATA = container.ShipmentDeliveryATA;
            containerPM.ShipmentDeliveryATD = container.ShipmentDeliveryATD;
            containerPM.ShipmentOriginAgentId = container.ShipmentOriginAgentId;
            containerPM.ShipmentDestinationAgentId = container.ShipmentDestinationAgentId;
            containerPM.ShipmentOriginAgentName = container.ShipmentOriginAgent != null ? container.ShipmentOriginAgent.EnglishName : "";
            containerPM.ShipmentDestinationAgentName = container.ShipmentDestinationAgent != null ? container.ShipmentDestinationAgent.EnglishName : "";
            containerPM.ShipmentNumber = container.ShipmentNumber;
            containerPM.ShipmentTypeId = container.ShipmentTypeId;
            containerPM.ShipmentTypeName = container.ShipmentType != null ? container.ShipmentType.Name : "";
            containerPM.OPClosed = container.OPClosed;
            containerPM.ContainersCount = container.ContainersCount;
            containerPM.HandlerId = container.HandlerId;
            containerPM.HandlerName = container.Handler?.Contact?.EnglishName;
            containerPM.CustomerId = container.CustomerId;
            containerPM.CustomerName = container.CustomerCard?.EnglishName;
            containerPM.ShipmentCreateDate = container.ShipmentCreateDate;
            containerPM.PODReceivedOnDate = container.PODReceivedOnDate;
            containerPM.IsAutomaticUpdates = container.IsAutomaticUpdates;
            containerPM.IsClosed = container.IsClosed;
            containerPM.ClosedDate = container.ClosedDate;
            containerPM.MasterEntityId = container.ShipmentId;
            containerPM.CustomerContactId = container.CustomerCard?.PrimaryContactId;
            containerPM.HandlerContactId = container.Handler?.Contact?.Id;
            containerPM.ConsigneeContactId = container.Shipment?.ConsigneeContactId;
            containerPM.ShipperContactId = container.Shipment?.ShipperContactId;
            containerPM.ShipperNotExporterContactId = container.Shipment?.ShipperNotExporterContactId;
            containerPM.FreightForwarderContactId = container.Shipment?.FreightForwarderContactId;
            containerPM.StatusId = container.StatusId;
            containerPM.IsCancelled = container.IsCancelled;
            containerPM.CancelledDate = container.CancelledDate;
            containerPM.ShipmentDeliveryTruckerId = container.ShipmentDeliveryTruckerId;
            containerPM.ShipmentDeliveryTruckerName = container.TruckerCard != null ? container.TruckerCard.EnglishName : "";
            containerPM.Leg1VesselId = container.Leg1VesselId;
            containerPM.Leg2VesselId = container.Leg2VesselId;
            containerPM.Leg3VesselId = container.Leg3VesselId;
            containerPM.Leg4VesselId = container.Leg4VesselId;
            containerPM.Leg5VesselId = container.Leg5VesselId;
            containerPM.ExceptionDate = container.ExceptionDate;
            containerPM.ExceptionResolvedDescription = container.ExceptionResolvedDescription;
            containerPM.HasException = container.HasException;
            containerPM.LastExceptionDescription = container.LastExceptionDescription;
            containerPM.ExceptionDescription = container.ExceptionDescription;
            containerPM.IsExceptionResolved = container.IsExceptionResolved;
            containerPM.EmptyContainerReturnTo = container.EmptyContainerReturnTo;
            containerPM.EmptyContainerReturnFrom = container.EmptyContainerReturnFrom;
            containerPM.EmptyContainerReturnETA = container.EmptyContainerReturnETA;
            containerPM.EmptyContainerReturnATA = container.EmptyContainerReturnATA;
            containerPM.EmptyContainerReturnATD = container.EmptyContainerReturnATD;
            containerPM.EmptyContainerReturnETD = container.EmptyContainerReturnETD;
            containerPM.OnCarriageGateOut = container.OnCarriageGateOut;
            containerPM.PreCarriageGateIn = container.PreCarriageGateIn;
            containerPM.ConcurrencyGUID = container.ConcurrencyGUID;
            containerPM.UpdatedByPartner = container.UpdatedByPartner;
            containerPM.ContainerTypeId = container.ContainerTypeId;
            containerPM.GrossWeight = container.GrossWeight;
            containerPM.Volume = container.Volume;
            containerPM.VolumeUnitCode = container.VolumeUnitCode;
            containerPM.GrossWeightUnitCode = container.GrossWeightUnitCode;
            containerPM.AdditionalReference1 = container.AdditionalReference1;
            containerPM.AdditionalReference2 = container.AdditionalReference2;
            containerPM.AdditionalReference3 = container.AdditionalReference3;
            containerPM.AdditionalReference4 = container.AdditionalReference4;
            containerPM.HasTransshipments = container.HasTransshipments;
            containerPM.OnCarriageETA = container.OnCarriageETA;
            containerPM.OnCarriageATA = container.OnCarriageATA;
            containerPM.RequestDate = container.RequestDate;
            containerPM.RecentResponseDate = container.RecentResponseDate;

            if (container.EntityStatus != null)
            {
                containerPM.StatusName = container.EntityStatus.Name;
                containerPM.StatusWeight = container.EntityStatus.StatusWeight;
            }

            MapCustomFields(containerPM, container);
            return containerPM;
        }

        public ContainerViewsGraphData GetViewsGraphData(int tenant)
        {
            var datas = new List<ContainerViewsGraphDataItem>();
            var queries = new ContainerViewsQueries().BuilQueries(tenant, repository.context);
            foreach (var item in queries) datas.Add(GetViewGraphDataByCode(tenant, item));
            return new ContainerViewsGraphData
            {
                Datas = datas.Where(x => x != null).ToList()
            };
        }

        private ContainerViewsGraphDataItem GetViewGraphDataByCode(int tenant, ContainerViewsQueries item)
        {
            if (!SecurityUtility.CheckFeature("Container", item.FeatureCode, tenant)) return null;
            return new ContainerViewsGraphDataItem
            {
                Label = item.Label,
                QueryCode = item.QueryCode,
                Value = item.Query.Count(),
                ToolTip = item.ToolTip
            };
        }
    }
}
