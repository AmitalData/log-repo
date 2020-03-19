using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators
{
    public class SimulatorArgs
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string MessageIdentifier { get; set; }
        public string AirlineId { get; set; }
        public string House { get; set; }
        public string Master { get; set; }
        public string AirlinePrefix { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool IsViaColoader { get; set; }
        public string ColoaderReference { get; set; }
        public string AnalyzeQueueId { get; set; }
        public string XmlText { get; set; }
        public bool IsGLSHKISAC { get; set; }
        public string ISAC_Sender { get; set; }
        public string ReasonForRejection { get; set; }
        public string ReasonForAcknowledgement { get; set; }
        public SimulatorFSA FSA { get; set; }
        public SimulatorFFA FFA { get; set; }
        public SimulatorFVA FVA { get; set; }
        public bool IsLocalAnalyze { get; set; }
        public bool IsChampSimulator { get; set; }
    }

    public class SimulatorFSA
    {
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal? Weight { get; set; }
        public int NumberOfPieces { get; set; }
        public int EntityNumberOfPieces { get; set; }

        public int DepartureTime { get; set; }
        public int ArrivalTime { get; set; }
        public string TimeOfArrivalInfo { get; set; }
        public string TimeOfDepartureInfo { get; set; }
        public string ArrivalDayChangeIndicator { get; set; }
        public string DepartureDayChangeIndicator { get; set; }
        public string CarrierCode { get; set; }

        public string OSIFirstLine { get; set; }
        public string OSISecondLine { get; set; }        
        public List<string> AllStatusCodes { get; set; }
    }

    public class SimulatorFFA
    {     
        public decimal? Weight { get; set; }
        public int NumberOfPieces { get; set; }
        public string DescriptionOfGoods { get; set; }

        public string OSIFirstLine { get; set; }
        public string OSISecondLine { get; set; }  
        public string SpecialServicesRequest { get; set; }

        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }   
        public DateTime? MainCarriageETD { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string MainCarriageSpaceAllocationCode { get; set; }

        public string Transshipment1FromPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public string Transshipment1CarrierId { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment1SpaceAllocationCode { get; set; }

        public string Transshipment2FromPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public string Transshipment2CarrierId { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment2SpaceAllocationCode { get; set; }
    }

    public class SimulatorFVA
    {
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? Volume { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string Recipient { get; set; }
        public string AnswerOSI { get; set; }
        public string AnswerReasonForNoReply { get; set; }
        public List<FVASimulatorScheduleInformation> ScheduleInformations { get; set; }
    }

    public class FVASimulatorScheduleInformation
    {
        [Key]
        public int Id { get; set; }
        public int FVAClassId { get; set; }
        public string AirlineId { get; set; }
        public string FlightNumber { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }

    }
}
