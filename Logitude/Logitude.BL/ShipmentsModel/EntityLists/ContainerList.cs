using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ContainerList
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
        public string EmptyPickupLocation { get; set; }
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
        public string OriginLocation { get; set; }
        public DateTime? EstimatedOriginPickup { get; set; }
        public DateTime? ActualOriginPickup { get; set; }

        public string POLLocation { get; set; }
        public DateTime? EstimatedPOLArrival { get; set; }
        public DateTime? ActualPOLArrival { get; set; }
        public DateTime? EstimatedPOLLoaded { get; set; }
        public DateTime? ActualPOLLoaded { get; set; }
        public DateTime? EstimatedPOLVesselDeparture { get; set; }
        public DateTime? ActualPOLVesselDeparture { get; set; }
        public string TransshipmentCount { get; set; }

        public string Transshipment1Location { get; set; }
        public DateTime? EstimatedTrans1VesselArrival { get; set; }
        public DateTime? ActualTransshipment1VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment1Discharge { get; set; }
        public DateTime? ActualTransshipment1Discharge { get; set; }
        public DateTime? EstimatedTransshipment1Loaded { get; set; }
        public DateTime? ActualTransshipment1Loaded { get; set; }
        public DateTime? EstimatedTrans1VesselDeparture { get; set; }
        public DateTime? ActualTrans1VesselDeparture { get; set; }

        public string Transshipment2Location { get; set; }
        public DateTime? EstimatedTrans2VesselArrival { get; set; }
        public DateTime? ActualTransshipment2VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment2Discharge { get; set; }
        public DateTime? ActualTransshipment2Discharge { get; set; }
        public DateTime? EstimatedTransshipment2Loaded { get; set; }
        public DateTime? ActualTransshipment2Loaded { get; set; }
        public DateTime? EstimatedTrans2VesselDeparture { get; set; }
        public DateTime? ActualTrans2VesselDeparture { get; set; }

        public string Transshipment3Location { get; set; }
        public DateTime? EstimatedTrans3VesselArrival { get; set; }
        public DateTime? ActualTransshipment3VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment3Discharge { get; set; }
        public DateTime? ActualTransshipment3Discharge { get; set; }
        public DateTime? EstimatedTransshipment3Loaded { get; set; }
        public DateTime? ActualTransshipment3Loaded { get; set; }
        public DateTime? EstimatedTrans3VesselDeparture { get; set; }
        public DateTime? ActualTrans3VesselDeparture { get; set; }

        public string Transshipment4Location { get; set; }
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

        public string PODLocation { get; set; }
        public DateTime? EstimatedPODVesselArrival { get; set; }
        public DateTime? ActualPODVesselArrival { get; set; }
        public DateTime? EstimatedPODDischarge { get; set; }
        public DateTime? ActualPODDischarge { get; set; }
        public DateTime? EstimatedPODDeparture { get; set; }
        public DateTime? ActualPODDeparture { get; set; }

        public string DeliveryLocation { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }

        public string LIFLocation { get; set; }
        public DateTime? EstimatedLIFArrival { get; set; }
        public DateTime? ActualLIFArrival { get; set; }
        public DateTime? EstimatedLIFDeparture { get; set; }
        public DateTime? ActualLIFDeparture { get; set; }

        public DateTime? GateIn { get; set; }
        public DateTime? GateOut { get; set; }

        public string EmptyReturnLocation { get; set; }
        public DateTime? EstimatedEmptyReturn { get; set; }
        public DateTime? ActualEmptyReturn { get; set; }

        public string CustomsReleaseState { get; set; }
        public DateTime? CustomsReleaseDate { get; set; }

        public string CarrierReleaseState { get; set; }
        public DateTime? CarrierReleaseDate { get; set; }
        public DateTime? AvailablityDate { get; set; }
        public string AvailabilityLocation { get; set; }
        public DateTime? LastFreeDayDate { get; set; }
        public int? FreeDays { get; set; }
        public string ShipmentStatusId { get; set; }

        public string ShipmentStatusName { get; set; }
    }
}
