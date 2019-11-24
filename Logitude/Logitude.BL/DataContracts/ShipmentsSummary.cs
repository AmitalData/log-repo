using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class ShipmentsSummaryDataItem
    {
        [Key]
        public string Id { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentStatusId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool IsOperationalClosed { get; set; }
        public bool IsAccountingClosed { get; set; }
        public bool NoFreightFile { get; set; }
        public bool IsNewARInvoiceBlocked { get; set; }
        public string ShipmentPayableStatusCode { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public DateTime? CarrierLastStatusDate { get; set; }
        public DateTime? LastFSRStatusRequestDate { get; set; }
    }

    public class ShipmentsSummary
    {
        [Key]
        public int Id { get; set; }
        public int OperationalOpenCount_DH { get; set; }
        public int OperationalOpenCount_DC { get; set; }
        public int AccountingOpenCount_DH { get; set; }
        public int AccountingOpenCount_DC { get; set; }
        public int AllFollowUpsCount { get; set; }
        public int MyFollowUpsCount { get; set; }
        public int OperationalOpenCount_ETD { get; set; }
        public int OperationalOpenCount_LWU { get; set; }
        public int LastSentFSRCount { get; set; }
        public int ImportShipmentsCount { get; set; }
        public int CreditLimitBlockedCount { get; set; }
        public int ExpectedDeparturesNotTransmittedCount { get; set; }
        public int ShippingInstructionsLast7DaysCount { get; set; }
        public int ContainerStatusLast7DaysCount { get; set; }
        public int EBookingInProgressCount { get; set; }
    }

    public class FlightSummary
    {
        [Key]
        public string Id { get; set; }
        public string ShipmentId { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string CarrierId { get; set; }
        public string CarrierNumber { get; set; }        
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public string DateFilterField { get; set; }
        public string ComputedStatusId { get; set; }
        public string ActualDateCode { get; set; }
        public string ExpectedDateCode { get; set; }
    }
}