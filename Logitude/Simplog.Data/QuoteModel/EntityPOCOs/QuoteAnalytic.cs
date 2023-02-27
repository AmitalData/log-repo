using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteAnalytic
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteNumber { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentTypeId { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public string Subject { get; set; }
        public string ConsigneeNotImporterId { get; set; }
        public string ShipperNotExporterId { get; set; }
        public string CustomerId { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string IncotermId { get; set; }
        public string SalesmanUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime OpenDate { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? VolumeInCBM { get; set; }
        public bool IsClosed { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }
        public bool IsDangerous { get; set; }
        public int? ExpirationDays { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string DepartmentId { get; set; }
        public string BranchId { get; set; }
        public string QuoteTypeCode { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IncludePickUp { get; set; }
        public bool IncludeDelivery { get; set; }
        public DateTime? StageDueDate { get; set; }
        public bool IsAutomaticallyClosed { get; set; }
        public DateTime? AutomaticallyCloseDate { get; set; }
        public int? AutomaticallyCloseDays { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string TransitTime { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? LastStageDate { get; set; }
        public string AgentId { get; set; }
        public double? TEU { get; set; }
        public string SaleCurrencyId { get; set; }
        public bool IsFixedPrice { get; set; }
        public bool IsSaleCurrencySameAsCost { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsChargesByVAT { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? GrossWeightPerTon { get; set; }
        public string NotifyId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string BusinessUnitId { get; set; }
        public string RatingCode { get; set; }
        public string StageId { get; set; }
        public string QuoteClosingReasonId { get; set; }
        public int? UsageCount { get; set; }
        public DateTime? LastUsageDate { get; set; }
        public string MoveTypeId { get; set; }
        public double? EstimatedProfitInLocal { get; set; }
        public double? EstimatedProfitInProfit { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string ShipmentSubTypeId { get; set; }
        public string RegionalTaxId { get; set; }
        public double? RegionalTaxPercentage { get; set; }
        public string SpecialServicesTypeId { get; set; }
        public string ValidByTypeCode { get; set; }
        public bool? ConnectedToOpportunity { get; set; }
        public string ProductCode { get; set; }
    }
}
