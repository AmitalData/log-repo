using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteFollowUpDataView
    {
        
        private string quoteFollowUpId;
        [Key]
        public string QuoteFollowUpId { get { return Id + FollowUpId; } set { quoteFollowUpId = Id + FollowUpId; } }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteNumber { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public System.DateTime OpenDate { get; set; }
        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }
        public bool IsClosed { get; set; }
        public Nullable<double> ChargeableWeight { get; set; }
        public Nullable<double> GrossWeight { get; set; }
        public byte[] LastModified { get; set; }
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
        public string DimensionsUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public Nullable<double> Volume { get; set; }
        public Nullable<int> NumberOfContainers { get; set; }
        public Nullable<int> NumberOfPackages { get; set; }
        public Nullable<double> Ratio { get; set; }
        public string VolumeUnitCode { get; set; }
        public string ShipmentTypeId { get; set; }
        public string ShipmentTypeName { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string ShipperContactId { get; set; }
        public string ConsigneeContactId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string IncotermId { get; set; }
        public string IncotermCode { get; set; }
        public string CreatedByUserId { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public bool IsDangerous { get; set; }
        public Nullable<int> ExpirationDays { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<double> VolumetricWeight { get; set; }

        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string PackageType1Id { get; set; }
        public string PackageType2Id { get; set; }
        public string PackageType3Id { get; set; }
        public string PackageType4Id { get; set; }
        public string PackageType5Id { get; set; }
        public Nullable<int> PackageType1Quantity { get; set; }
        public Nullable<int> PackageType3Quantity { get; set; }
        public Nullable<int> PackageType2Quantity { get; set; }
        public Nullable<int> PackageType4Quantity { get; set; }
        public Nullable<int> PackageType5Quantity { get; set; }
        public bool IsByKG { get; set; }
        public bool IsByContainer { get; set; }
        public string QuoteTypeCode { get; set; }
        public Nullable<double> EstimateProfit { get; set; }
        public bool EstimateProfitEdited { get; set; }
        public Nullable<double> MinimumFreightCost { get; set; }
        public Nullable<double> MinimumFreightSale { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public bool IsFreightBySteps { get; set; }
        public bool IsCancelled { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public string QuoteCustomerTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string SaleCurrencyId { get; set; }
        public double ExchangeRate { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string PickUpAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsFixedPrice { get; set; }
        public string SearchFields { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string ConcurrencyGUID { get; set; }
        public string FromPartnerId { get; set; }
        public string ToPartnerId { get; set; }
        public string FromPartnerAddressId { get; set; }
        public string ToPartnerAddressId { get; set; }
        public string FollowUpId { get; set; }
        public Nullable<System.DateTime> FollowUpDate { get; set; }
        public string FollowUpNotes { get; set; }
        public string FollowUpOwnerUserId { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string FollowUpType { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }

        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string SalesmanUserId { get; set; }

        public string CreatedByUser { get; set; }
        public string FollowUpOwner { get; set; }
        public string QuoteTypeName { get; set; }
        public string FollowUpTypeId { get; set; }
        public string FollowUpOwnerId { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string Subject { get; set; }
        public bool IsSubjectEdited { get; set; }

        public string StageId { get; set; }
        public string StageName { get; set; }
        public DateTime? StageDueDate { get; set; }
        public string RatingCode { get; set; }
        public string LastActivityTypeCode { get; set; }
        public string LastActivitySubject { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string NextActivityTypeCode { get; set; }
        public string NextActivitySubject { get; set; }
        public DateTime? NextActivityDate { get; set; }
        public string OpportunityId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsAutomaticallyClosed { get; set; }
        public DateTime? AutomaticallyCloseDate { get; set; }
        public int? AutomaticallyCloseDays { get; set; }
        public string QuoteClosingReasonCode { get; set; }
        public string QuoteClosingReasonName { get; set; }
        public string ProductCode { get; set; }
        public double? ValueOfGoods { get; set; }

        public double? GrossWeightInKG { get; set; }
        public double? GrossWeightPerTon { get; set; }
    }
}
