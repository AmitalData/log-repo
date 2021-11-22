using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerPM
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
        public string MainCarriageCarrierId { get; set; }
        public string CarrierName { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string Master { get; set; }
        public string MainCarriageVesselId { get; set; }
        public string VesselName { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string ShipmentId { get; set; }
        public DateTime? EstimatedEmptyPickupDate { get; set; }
        public DateTime? ActualEmptyPickupDate { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public string CurrentLocation { get; set; }
        public bool HasContainerException { get; set; }

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
        public string ShipmentFirstPickupFrom { get; set; }
        public string ShipmentFirstPickupTo { get; set; }
        public string ShipmentPreCarriageFrom { get; set; }
        public string ShipmentPreCarriageTo { get; set; }
        public string ShipmentMainCarriageFrom { get; set; }
        public string ShipmentMainCarriageTo { get; set; }
        public string ShipmentTransshipment1From { get; set; }
        public string ShipmentTransshipment1To { get; set; }
        public string ShipmentTransshipment2From { get; set; }
        public string ShipmentTransshipment2To { get; set; }
        public string ShipmentTransshipment3From { get; set; }
        public string ShipmentTransshipment3To { get; set; }
        public string ShipmentOnCarriageFrom { get; set; }
        public string ShipmentOnCarriageTo { get; set; }
        public string ShipmentLastDeliveryFrom { get; set; }
        public string ShipmentLastDeliveryTo { get; set; }
        public DateTime? EstimatedOriginPickup { get; set; }
        public DateTime? ActualOriginPickup { get; set; }
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
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
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
        public DateTime? LastFreeDayDate { get; set; }
        public int? FreeDays { get; set; }
        public string ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public string EmptyPickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string EmptyReturnLocation { get; set; }
        public string AvailabilityLocation { get; set; }
        public string OriginLocation { get; set; }
        public string LIFLocation { get; set; }
        public string POLLocation { get; set; }
        public string PODLocation { get; set; }
        public string Transshipment1Location { get; set; }
        public string Transshipment2Location { get; set; }
        public string Transshipment3Location { get; set; }
        public string Transshipment4Location { get; set; }
        public string EmptyPickupLocationPortId { get; set; }
        public string DeliveryLocationPortId { get; set; }
        public string EmptyReturnLocationPortId { get; set; }
        public string AvailabilityLocationPortId { get; set; }
        public string OriginLocationPortId { get; set; }
        public string LIFLocationPortId { get; set; }
        public string POLLocationPortId { get; set; }
        public string PODLocationPortId { get; set; }
        public string Transshipment1LocationPortId { get; set; }
        public string Transshipment2LocationPortId { get; set; }
        public string Transshipment3LocationPortId { get; set; }
        public string Transshipment4LocationPortId { get; set; }
        public string TerminalId { get; set; }
        public string TerminalAddress { get; set; }
        public string TerminalPhone { get; set; }
        public string TerminalName { get; set; }
        public string TerminalAddressId { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field5 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field6 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field7 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field8 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field9 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field10 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPickupETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPickupETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPickupATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPickupATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPreCarriageETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPreCarriageETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPreCarriageATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentPreCarriageATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentMainCarriageETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentMainCarriageETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentMainCarriageATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentMainCarriageATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment1ETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment1ETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment1ATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment1ATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment2ETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment2ETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment2ATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment2ATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment3ETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment3ETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment3ATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentTransshipment3ATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentOnCarriageETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentOnCarriageETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentOnCarriageATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentOnCarriageATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentDeliveryETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentDeliveryETD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentDeliveryATA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ShipmentDeliveryATD { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentOriginAgentId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentDestinationAgentId { get; set; }
        public string ShipmentOriginAgentName { get; set; }
        public string ShipmentDestinationAgentName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? ContainersCount { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string HandlerId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string HandlerName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerName { get; set; }
        public bool OPClosed { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeName { get; set; }
        public DateTime? ShipmentCreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? PODReceivedOnDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutomaticUpdates { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsClosed { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ClosedDate { get; set; }
        public bool IsUpdateByAutomation { get; set; }
        public string MasterEntityId { get; set; }
    }
}
