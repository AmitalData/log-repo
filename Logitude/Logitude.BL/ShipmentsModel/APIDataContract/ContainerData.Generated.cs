
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
   
    public partial class ContainerData
    {

	    
    public string Id { get; set; }
    
    public User UpdatedByUser { get; set; }
    
	[XmlAttribute]
    public string ContainerNumber { get; set; }
    
    public DateTime? DischargeDate { get; set; }
    
    public DateTime? EstimatedEmptyPickupDate { get; set; }
    
    public DateTime? ActualEmptyPickupDate { get; set; }
    
    public string EmptyPickupLocation { get; set; }
    
    public string DepartureLocation { get; set; }
    
    public string DestinationLocation { get; set; }
    
    public string PreCarriageLocation { get; set; }
    
    public DateTime? PreCarriageETD { get; set; }
    
    public DateTime? PreCarriageATD { get; set; }
    
    public string POLLocation { get; set; }
    
    public DateTime? EstimatedPOLArrival { get; set; }
    
    public DateTime? ActualPOLArrival { get; set; }
    
    public DateTime? EstimatedPOLLoaded { get; set; }
    
    public DateTime? ActualPOLLoaded { get; set; }
    
    public DateTime? EstimatedPOLVesselDeparture { get; set; }
    
    public DateTime? ActualPOLVesselDeparture { get; set; }
    
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
    
    public string Leg1Voyage { get; set; }
    
    public string Leg2Voyage { get; set; }
    
    public string Leg4Voyage { get; set; }
    
    public string Leg5Voyage { get; set; }
    
    public string PODLocation { get; set; }
    
    public DateTime? EstimatedPODVesselArrival { get; set; }
    
    public DateTime? ActualPODVesselArrival { get; set; }
    
    public DateTime? EstimatedPODDischarge { get; set; }
    
    public DateTime? ActualPODDischarge { get; set; }
    
    public DateTime? EstimatedPODDeparture { get; set; }
    
    public DateTime? ActualPODDeparture { get; set; }
    
    public string OnCarriageLocation { get; set; }
    
    public DateTime? OnCarriageETD { get; set; }
    
    public DateTime? OnCarriageATD { get; set; }
    
    public string LIFLocation { get; set; }
    
    public DateTime? EstimatedLIFArrival { get; set; }
    
    public DateTime? ActualLIFArrival { get; set; }
    
    public DateTime? EstimatedOnCarriageDeparture { get; set; }
    
    public DateTime? ActualOnCarriageDeparture { get; set; }
    
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
    
    public Port EmptyPickupLocationPort { get; set; }
    
    public Port OnCarriageLocationPort { get; set; }
    
    public Port EmptyReturnLocationPort { get; set; }
    
    public Port AvailabilityLocationPort { get; set; }
    
    public Port PreCarriageLocationPort { get; set; }
    
    public Port LIFLocationPort { get; set; }
    
    public Port POLLocationPort { get; set; }
    
    public Port PODLocationPort { get; set; }
    
    public Port Transshipment1LocationPort { get; set; }
    
    public Port Transshipment2LocationPort { get; set; }
    
    public Port Transshipment3LocationPort { get; set; }
    
    public Port Transshipment4LocationPort { get; set; }
    
    public Card Terminal { get; set; }
    
    public string TerminalPhone { get; set; }
    
    public bool IsClosed { get; set; }
    
    public DateTime? ClosedDate { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public DateTime? CancelledDate { get; set; }
    
    public Vessel Leg1Vessel { get; set; }
    
    public Vessel Leg2Vessel { get; set; }
    
    public Vessel Leg3Vessel { get; set; }
    
    public Vessel Leg4Vessel { get; set; }
    
    public Vessel Leg5Vessel { get; set; }
    
    public DateTime? OnCarriageGateOut { get; set; }
    
    public DateTime? PreCarriageGateIn { get; set; }
    
    public string Leg3Voyage { get; set; }
    
	[XmlAttribute]
    public string ShipmentNumber { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 