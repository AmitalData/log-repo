using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class ChartingDataClass
    {
        [Key]
        public string Id { get; set; }
        public string GroupedId { get; set; }
        public int IntegerProperty { get; set; }
        public string DataTypeCode { get; set; }
        public string LabelProperty { get; set; }
        public string StringProperty { get; set; }
        public double DoubleProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public string ShortLabelProperty { get; set; }
        public int IndexOrder { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Shipments { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? ReceivablesInLocal { get; set; }
        public double? ReceivablesInProfit { get; set; }
        public double? ProfitInLocal { get; set; }
        public double? ProfitInProfit { get; set; }
        public int TypeIndex { get; set; }
        public string OwnerId { get; set; }
        public string BusinessUnitId { get; set; }
        public string RecordsTypeCode { get; set; }
        public string Code { get; set; }

        public string ClassificationId { get; set; }
        public string TicketStageId { get; set; }
        public string LabelColor { get; set; }

        public string EmployeeGroupId { get; set; }
        public string SeverityId { get; set; }

        public string TicketTypeId { get; set; }
        public string DateRange { get; set; }
        public string GroupByCode { get; set; }

        public string ParticipantId { get; set; }
        public TimeSpan TimeProperty { get; set; }

        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public double Total { get; set; }

        public int Count_All { get; set; }
        public int Count_Convert { get; set; }
        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }
        public string TransportModeDirection { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeName { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeDirection_Display { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
        public double? Value { get; set; }
    }
}