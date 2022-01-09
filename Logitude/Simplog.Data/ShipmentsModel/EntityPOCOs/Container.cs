using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Linq;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class Container
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipmentPackagesId { get; set; }
        [ForeignKey("ShipmentPackagesId")]
        public virtual ShipmentPackage ShipmentPackage { get; set; }

        public string MainCarriageCarrierId { get; set; }
        [ForeignKey("MainCarriageCarrierId")]
        public virtual Card CarrierCard { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string Master { get; set; }
        public string MainCarriageVesselId { get; set; }
        [ForeignKey("MainCarriageVesselId")]
        public virtual Vessel VesselCard { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        public DateTime? EstimatedEmptyPickupDate { get; set; }
        public DateTime? ActualEmptyPickupDate { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public string CurrentLocation { get; set; }
        public bool HasContainerException { get; set; }
        public string ShipmentFirstPickupFrom { get; set; }
        public string ShipmentFirstPickupTo { get; set; }
        public string ShipmentPreCarriageFromId { get; set; }
        public string ShipmentPreCarriageToId { get; set; }
        public string ShipmentMainCarriageFromId { get; set; }
        public string ShipmentMainCarriageToId { get; set; }
        public string ShipmentTransshipment1FromId { get; set; }
        public string ShipmentTransshipment1ToId { get; set; }
        public string ShipmentTransshipment2FromId { get; set; }
        public string ShipmentTransshipment2ToId { get; set; }
        public string ShipmentTransshipment3FromId { get; set; }
        public string ShipmentTransshipment3ToId { get; set; }
        public string ShipmentOnCarriageFromId { get; set; }
        public string ShipmentOnCarriageToId { get; set; }
        public string ShipmentLastDeliveryFrom { get; set; }
        public string ShipmentLastDeliveryTo { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? EstimatedPOLArrival { get; set; }
        public DateTime? ActualPOLArrival { get; set; }
        public DateTime? EstimatedPOLLoaded { get; set; }
        public DateTime? ActualPOLLoaded { get; set; }
        public DateTime? EstimatedPOLVesselDeparture { get; set; }
        public DateTime? ActualPOLVesselDeparture { get; set; }
        public string TransshipmentCount { get; set; }
        public DateTime? EstimatedTrans1VesselArrival { get; set; }
        public DateTime? ActualTransshipment1VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment1Discharge { get; set; }
        public DateTime? ActualTransshipment1Discharge { get; set; }
        public DateTime? EstimatedTransshipment1Loaded { get; set; }
        public DateTime? ActualTransshipment1Loaded { get; set; }
        public DateTime? EstimatedTrans1VesselDeparture { get; set; }
        public DateTime? ActualTrans1VesselDeparture { get; set; }
        public DateTime? EstimatedTrans2VesselArrival { get; set; }
        public DateTime? ActualTransshipment2VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment2Discharge { get; set; }
        public DateTime? ActualTransshipment2Discharge { get; set; }
        public DateTime? EstimatedTransshipment2Loaded { get; set; }
        public DateTime? ActualTransshipment2Loaded { get; set; }
        public DateTime? EstimatedTrans2VesselDeparture { get; set; }
        public DateTime? ActualTrans2VesselDeparture { get; set; }
        public DateTime? EstimatedTrans3VesselArrival { get; set; }
        public DateTime? ActualTransshipment3VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment3Discharge { get; set; }
        public DateTime? ActualTransshipment3Discharge { get; set; }
        public DateTime? EstimatedTransshipment3Loaded { get; set; }
        public DateTime? ActualTransshipment3Loaded { get; set; }
        public DateTime? EstimatedTrans3VesselDeparture { get; set; }
        public DateTime? ActualTrans3VesselDeparture { get; set; }
        public DateTime? EstimatedTrans4VesselArrival { get; set; }
        public DateTime? ActualTransshipment4VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment4Discharge { get; set; }
        public DateTime? ActualTransshipment4Discharge { get; set; }
        public DateTime? EstimatedTransshipment4Loaded { get; set; }
        public DateTime? ActualTransshipment4Loaded { get; set; }
        public DateTime? EstimatedTrans4VesselDeparture { get; set; }
        public DateTime? ActualTrans4VesselDeparture { get; set; }
        public string Leg1Vessel { get; set; }
        public string Leg1Voyage { get; set; }
        public string Leg2Vessel { get; set; }
        public string Leg2Voyage { get; set; }
        public string Leg3Vessel { get; set; }
        public string Leg3Voyage { get; set; }
        public string Leg4Vessel { get; set; }
        public string Leg4Voyage { get; set; }
        public string Leg5Vessel { get; set; }
        public string Leg5Voyage { get; set; }
        public DateTime? EstimatedPODVesselArrival { get; set; }
        public DateTime? ActualPODVesselArrival { get; set; }
        public DateTime? EstimatedPODDischarge { get; set; }
        public DateTime? ActualPODDischarge { get; set; }
        public DateTime? EstimatedPODDeparture { get; set; }
        public DateTime? ActualPODDeparture { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? EstimatedLIFArrival { get; set; }
        public DateTime? ActualLIFArrival { get; set; }
        public DateTime? EstimatedOnCarriageDeparture { get; set; }
        public DateTime? ActualOnCarriageDeparture { get; set; }
        public DateTime? GateIn { get; set; }
        public DateTime? GateOut { get; set; }
        public DateTime? EstimatedEmptyReturn { get; set; }
        public DateTime? ActualEmptyReturn { get; set; }
        public string CustomsReleaseState { get; set; }
        public DateTime? CustomsReleaseDate { get; set; }
        public string CarrierReleaseState { get; set; }
        public DateTime? CarrierReleaseDate { get; set; }
        public DateTime? AvailablityDate { get; set; }

        [ForeignKey("ShipmentPreCarriageFromId")]
        public virtual Port ShipmentPreCarriageFromPort { get; set; }

        [ForeignKey("ShipmentPreCarriageToId")]
        public virtual Port ShipmentPreCarriageToPort { get; set; }

        [ForeignKey("ShipmentMainCarriageFromId")]
        public virtual Port ShipmentMainCarriageFromPort { get; set; }

        [ForeignKey("ShipmentMainCarriageToId")]
        public virtual Port ShipmentMainCarriageToPort { get; set; }

        [ForeignKey("ShipmentTransshipment1FromId")]
        public virtual Port ShipmentTransshipment1FromPort { get; set; }

        [ForeignKey("ShipmentTransshipment1ToId")]
        public virtual Port ShipmentTransshipment1ToPort { get; set; }

        [ForeignKey("ShipmentTransshipment2FromId")]
        public virtual Port ShipmentTransshipment2FromPort { get; set; }

        [ForeignKey("ShipmentTransshipment2ToId")]
        public virtual Port ShipmentTransshipment2ToPort { get; set; }

        [ForeignKey("ShipmentTransshipment3FromId")]
        public virtual Port ShipmentTransshipment3FromPort { get; set; }

        [ForeignKey("ShipmentTransshipment3ToId")]
        public virtual Port ShipmentTransshipment3ToPort { get; set; }

        [ForeignKey("ShipmentOnCarriageFromId")]
        public virtual Port ShipmentOnCarriageFromPort { get; set; }

        [ForeignKey("ShipmentOnCarriageToId")]
        public virtual Port ShipmentOnCarriageToPort { get; set; }
        public DateTime? LastFreeDayDate { get; set; }
        public int? FreeDays { get; set; }
        public string ShipmentStatusId { get; set; }
        [ForeignKey("ShipmentStatusId")]
        public virtual EntityStatus ShipmentEntityStatus { get; set; }

        public string EmptyPickupLocation { get; set; }
        public string OnCarriageLocation { get; set; }
        public string EmptyReturnLocation { get; set; }
        public string AvailabilityLocation { get; set; }
        public string PreCarriageLocation { get; set; }
        public string LIFLocation { get; set; }
        public string POLLocation { get; set; }
        public string PODLocation { get; set; }
        public string Transshipment1Location { get; set; }
        public string Transshipment2Location { get; set; }
        public string Transshipment3Location { get; set; }
        public string Transshipment4Location { get; set; }

        public string EmptyPickupLocationPortId { get; set; }
        public string OnCarriageLocationPortId { get; set; }
        public string EmptyReturnLocationPortId { get; set; }
        public string AvailabilityLocationPortId { get; set; }
        public string PreCarriageLocationPortId { get; set; }
        public string LIFLocationPortId { get; set; }
        public string POLLocationPortId { get; set; }
        public string PODLocationPortId { get; set; }
        public string Transshipment1LocationPortId { get; set; }
        public string Transshipment2LocationPortId { get; set; }
        public string Transshipment3LocationPortId { get; set; }
        public string Transshipment4LocationPortId { get; set; }

        [ForeignKey("EmptyPickupLocationPortId")]
        public virtual Port EmptyPickupLocationPort { get; set; }
        [ForeignKey("OnCarriageLocationPortId")]
        public virtual Port OnCarriageLocationPort { get; set; }
        [ForeignKey("EmptyReturnLocation")]
        public virtual Port EmptyReturnLocationPort { get; set; }
        [ForeignKey("AvailabilityLocationPortId")]
        public virtual Port AvailabilityLocationPort { get; set; }
        [ForeignKey("PreCarriageLocationPortId")]
        public virtual Port PreCarriageLocationPort { get; set; }
        [ForeignKey("LIFLocationPortId")]
        public virtual Port LIFLocationPort { get; set; }
        [ForeignKey("POLLocationPortId")]
        public virtual Port POLLocationPort { get; set; }
        [ForeignKey("PODLocationPortId")]
        public virtual Port PODLocationPort { get; set; }
        [ForeignKey("Transshipment1LocationPortId")]
        public virtual Port Transshipment1LocationPort { get; set; }
        [ForeignKey("Transshipment2LocationPortId")]
        public virtual Port Transshipment2LocationPort { get; set; }
        [ForeignKey("Transshipment3LocationPortId")]
        public virtual Port Transshipment3LocationPort { get; set; }
        [ForeignKey("Transshipment4LocationPortId")]
        public virtual Port Transshipment4LocationPort { get; set; }

        public string TerminalId { get; set; }
        [ForeignKey("TerminalId")]
        public virtual Card TerminalCard { get; set; }
        public string TerminalAddress { get; set; }
        public string TerminalPhone { get; set; }
        public string TerminalAddressId { get; set; }
        [ForeignKey("TerminalAddressId")]
        public virtual Address TerminalCardAddress { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }

        public DateTime? ShipmentPickupETA { get; set; }
        public DateTime? ShipmentPickupETD { get; set; }
        public DateTime? ShipmentPickupATA { get; set; }
        public DateTime? ShipmentPickupATD { get; set; }
        public DateTime? ShipmentPreCarriageETA { get; set; }
        public DateTime? ShipmentPreCarriageETD { get; set; }
        public DateTime? ShipmentPreCarriageATA { get; set; }
        public DateTime? ShipmentPreCarriageATD { get; set; }
        public DateTime? ShipmentMainCarriageETA { get; set; }
        public DateTime? ShipmentMainCarriageETD { get; set; }
        public DateTime? ShipmentMainCarriageATA { get; set; }
        public DateTime? ShipmentMainCarriageATD { get; set; }
        public DateTime? ShipmentTransshipment1ETA { get; set; }
        public DateTime? ShipmentTransshipment1ETD { get; set; }
        public DateTime? ShipmentTransshipment1ATA { get; set; }
        public DateTime? ShipmentTransshipment1ATD { get; set; }
        public DateTime? ShipmentTransshipment2ETA { get; set; }
        public DateTime? ShipmentTransshipment2ETD { get; set; }
        public DateTime? ShipmentTransshipment2ATA { get; set; }
        public DateTime? ShipmentTransshipment2ATD { get; set; }
        public DateTime? ShipmentTransshipment3ETA { get; set; }
        public DateTime? ShipmentTransshipment3ETD { get; set; }
        public DateTime? ShipmentTransshipment3ATA { get; set; }
        public DateTime? ShipmentTransshipment3ATD { get; set; }
        public DateTime? ShipmentOnCarriageETA { get; set; }
        public DateTime? ShipmentOnCarriageETD { get; set; }
        public DateTime? ShipmentOnCarriageATA { get; set; }
        public DateTime? ShipmentOnCarriageATD { get; set; }
        public DateTime? ShipmentDeliveryETA { get; set; }
        public DateTime? ShipmentDeliveryETD { get; set; }
        public DateTime? ShipmentDeliveryATA { get; set; }
        public DateTime? ShipmentDeliveryATD { get; set; }
        public string ShipmentOriginAgentId { get; set; }
        public string ShipmentDestinationAgentId { get; set; }
        [ForeignKey("ShipmentOriginAgentId")]
        public virtual Card ShipmentOriginAgent { get; set; }

        [ForeignKey("ShipmentDestinationAgentId")]
        public virtual Card ShipmentDestinationAgent { get; set; }

        public string ShipmentNumber { get; set; }
        public int? ContainersCount { get; set; }
        public string HandlerId { get; set; }
        [ForeignKey("HandlerId")]
        public virtual User Handler { get; set; }

        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Card CustomerCard { get; set; }
        public bool OPClosed { get; set; }
        public string ShipmentTypeId { get; set; }
        [ForeignKey("ShipmentTypeId")]
        public virtual ShipmentType ShipmentType { get; set; }
        public DateTime? ShipmentCreateDate { get; set; }
        public DateTime? PODReceivedOnDate { get; set; }
        public bool IsAutomaticUpdates { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosedDate { get; set; }
        [ForeignKey("StatusId")]
        public virtual EntityStatus EntityStatus { get; set; }
        public string StatusId { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool IsCancelled { get; set; }
        public string ShipmentDeliveryTruckerId { get; set; }
        [ForeignKey("ShipmentDeliveryTruckerId")]
        public virtual Card TruckerCard { get; set; }
        public string Leg1VesselId { get; set; }
        [ForeignKey("Leg1VesselId")]
        public virtual Vessel Vessel1Leg { get; set; }
        public string Leg2VesselId { get; set; }
        [ForeignKey("Leg2VesselId")]
        public virtual Vessel Vessel2Leg { get; set; }
        public string Leg3VesselId { get; set; }
        [ForeignKey("Leg3VesselId")]
        public virtual Vessel Vessel3Leg { get; set; }
        public string Leg4VesselId { get; set; }
        [ForeignKey("Leg4VesselId")]
        public virtual Vessel Vessel4Leg { get; set; }
        public string Leg5VesselId { get; set; }
        [ForeignKey("Leg5VesselId")]
        public virtual Vessel Vessel5Leg { get; set; }

    }
}
