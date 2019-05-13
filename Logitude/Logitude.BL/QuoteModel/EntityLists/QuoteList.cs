using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteList
    {
       
        public string Id { get; set; }
        [Key]
        public string QuoteViewId { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public string ShipmentTypeId { get; set; }
        public string QuoteTemplateId { get; set; }
        public string QuoteCutomerTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public int LastVersionNumber { get; set; }
        public string Shipper { get; set; }
        public string ShipperId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeId { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string Subject { get; set; }
        public bool IsSubjectEdited { get; set; }
        public bool TotalPerContainer { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? ExpirationDate { get; set; }        

        public string QuoteTypeName { get; set; }
        public string CarrierName { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? GrossWeight { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? GrossWeightPerTon { get; set; }

        public string ShipmentType { get; set; }
        
        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }
        public string FromCountryCode { get; set; }
        
        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }
        public string ToCountryCode { get; set; }

        public string HAWBFBLBL { get; set; }
        public bool IsFixedPrice { get; set; }

        public string FollowUpType { get; set; }
        public string FollowUpTypeId { get; set; }
        public string FollowUpId { get; set; }
        public DateTime? FollowUpDate { get; set; }
              
       
        public bool IsClosed { get; set; }
        public DateTime LastUpdate { get; set; }
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
        public bool NewMessage { get; set; }
        
        public string FollowUpNotes { get; set; }

        public string Notes { get; set; }

        public int? NumberOfContainers { get; set; }

        public byte[] LastModified { get; set; }
        public bool hasChanges { get; set; }

        public string DepartmentId { get; set; }
        public string BranchId { get; set; }

        public string DepartmentName { get; set; }
        public string BranchName { get; set; }
        public string MoveTypeName { get; set; }
        public string QuoteTypeCode { get; set; }
        public bool IsByKG { get; set; }
        public bool IsByContainer { get; set; }
        public double? EstimateProfit { get; set; }
        public bool EstimateProfitEdited { get; set; }
        public string CreatedByUser { get; set; }
        public string CreatedByUserId { get; set; }
        public string FollowUpOwner { get; set; }
        public string FollowUpOwnerId { get; set; }               
        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public bool IsCancelled { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string SearchFields { get; set; }
        public string FromPartnerId { get; set; }
        public string ToPartnerId { get; set; }
        public string FromPartnerAddressId { get; set; }
        public string ToPartnerAddressId { get; set; }
        public int? NumberOfPackages { get; set; }
        public string Salesman { get; set; }
        public string SalesmanName { get; set; }
        public string Routing { get; set; }
        public string LastQuoteActivityTypeName { get; set; }
        public string LastActivityByUserName { get; set; }
        public DateTime LastQuoteActivityDate { get; set; }       
        public string QuoteClosingReasonCode { get; set; }
        public string QuoteClosingReasonName { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public int? UsageCount { get; set; }
        public DateTime? LastUsageDate { get; set; }
        public string FreelancerId { get; set; }
        public string FreelancerAddressId { get; set; }
        public string FreelancerContactId { get; set; }
        public string FreelancerName { get; set; }
        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string SalesmanUserId { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public DateTime? StageDueDate { get; set; }
        public int? StageMaxDays { get; set; }
        public string RatingCode { get; set; }
        public string RatingName { get; set; }
        public int RatingIndexOrder { get; set; }
        public string LastActivityTypeCode { get; set; }
        public string LastActivityTypeName { get; set; }
        public string LastActivitySubject { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string NextActivityTypeCode { get; set; }
        public string NextActivityTypeName { get; set; }
        public string NextActivitySubject { get; set; }
        public DateTime? NextActivityDate { get; set; }
        public string OpportunityId { get; set; }
        public bool IsAutomaticallyClosed { get; set; }
        public DateTime? AutomaticallyCloseDate { get; set; }
        public int? AutomaticallyCloseDays { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ProductCode { get; set; }
        public string IncotermCode { get; set; }
        public string TransitTime { get; set; }
        public string DepartureFrequency { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? LastStageDate { get; set; }
        public string AgentId { get; set; }
        public string AgentAddressId { get; set; }
        public string AgentContactId { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public string AgentName { get; set; }
        public double? TEU { get; set; }
        public bool IsSaleCurrencySameAsCost { get; set; }
        public bool IsChargesByVAT { get; set; }
        public double? ValueOfGoods { get; set; }

        public bool IsQuoteDataExternal { get; set; }
        public bool IsQuoteDocumentExternal { get; set; }
        public string QuotationSections { get; set; }

        public string NotifyId { get; set; }
        public string NotifyAddressId { get; set; }
        public string NotifyContactId { get; set; }
        public string NotifyName { get; set; }
        public string NotifyNote { get; set; }
        public int? NumberOfFollowUps { get; set; }
        public bool IsDangerous { get; set; }
    }
}