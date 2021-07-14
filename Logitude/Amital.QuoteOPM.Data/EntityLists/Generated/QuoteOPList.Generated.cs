using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string QuoteTemplateId  { get; set; }
       [DataMember]
       public int LastVersionNumber  { get; set; }
       [DataMember]
       public string FreelancerId  { get; set; }
       [DataMember]
       public string FreelancerAddressId  { get; set; }
       [DataMember]
       public string FreelancerContactId  { get; set; }
       [DataMember]
       public string LastModified  { get; set; }
       [DataMember]
       public string Field1  { get; set; }
       [DataMember]
       public string Field2  { get; set; }
       [DataMember]
       public string Field3  { get; set; }
       [DataMember]
       public string Field4  { get; set; }
       [DataMember]
       public string Field5  { get; set; }
       [DataMember]
       public string Field6  { get; set; }
       [DataMember]
       public string Field7  { get; set; }
       [DataMember]
       public string Field8  { get; set; }
       [DataMember]
       public string Field9  { get; set; }
       [DataMember]
       public string Field10  { get; set; }
       [DataMember]
       public bool IsByKG  { get; set; }
       [DataMember]
       public bool IsByContainer  { get; set; }
       [DataMember]
       public bool EstimateProfitEdited  { get; set; }
       [DataMember]
       public string OpportunityId  { get; set; }
       [DataMember]
       public DateTime? LastStageDate  { get; set; }
       [DataMember]
       public string AgentReference1  { get; set; }
       [DataMember]
       public string AgentReference2  { get; set; }
       [DataMember]
       public bool IsSaleCurrencySameAsCost  { get; set; }
       [DataMember]
       public double? EstimateProfit  { get; set; }
       [DataMember]
       public bool IsFixedPrice  { get; set; }
       [DataMember]
       public string ShipperName  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string FollowUpOwner  { get; set; }
       [DataMember]
       public string FollowUpOwnerId  { get; set; }
       [DataMember]
       public string QuoteNumber  { get; set; }
       [DataMember]
       public string MainCarriageCarrierId  { get; set; }
       [DataMember]
       public string MainCarriageCarrierName  { get; set; }
       [DataMember]
       public string DirectionId  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string DepartmentId  { get; set; }
       [DataMember]
       public string BranchId  { get; set; }
       [DataMember]
       public string ShipmentTypeId  { get; set; }
       [DataMember]
       public string ShipmentType  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string ShipperId  { get; set; }
       [DataMember]
       public string Shipper  { get; set; }
       [DataMember]
       public string ShipperReference1  { get; set; }
       [DataMember]
       public string ShipperReference2  { get; set; }
       [DataMember]
       public string ConsigneeId  { get; set; }
       [DataMember]
       public string Consignee  { get; set; }
       [DataMember]
       public string ConsigneeReference1  { get; set; }
       [DataMember]
       public string ConsigneeReference2  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string FromPort  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public string ToPort  { get; set; }
       [DataMember]
       public string SalesmanUserId  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string CreatedByUser  { get; set; }
       [DataMember]
       public DateTime OpenDate  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public double? ChargeableWeight  { get; set; }
       [DataMember]
       public double? GrossWeight  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public int? NumberOfPackages  { get; set; }
       [DataMember]
       public int? NumberOfContainers  { get; set; }
       [DataMember]
       public bool IsDangerous  { get; set; }
       [DataMember]
       public DateTime? ExpirationDate  { get; set; }
       [DataMember]
       public int? OrderNumberOfPackages  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime FollowUpDate  { get; set; }
       [DataMember]
       public string FollowUpType  { get; set; }
       [DataMember]
       public string FollowUpTypeId  { get; set; }
       [DataMember]
       public string FollowUpNotes  { get; set; }
       [DataMember]
       public string QuoteTypeCode  { get; set; }
       [DataMember]
       public string QuoteTypeName  { get; set; }
       [DataMember]
       public string DepartmentName  { get; set; }
       [DataMember]
       public string BranchName  { get; set; }
       [DataMember]
       public string FromPartnerId  { get; set; }
       [DataMember]
       public string ToPartnerId  { get; set; }
       [DataMember]
       public string FromPartnerAddressId  { get; set; }
       [DataMember]
       public string ToPartnerAddressId  { get; set; }
       [DataMember]
       public string QuoteClosingReasonCode  { get; set; }
       [DataMember]
       public DateTime? SentDate  { get; set; }
       [DataMember]
       public DateTime? AcceptedDate  { get; set; }
       [DataMember]
       public DateTime? DeclinedDate  { get; set; }
       [DataMember]
       public int? UsageCount  { get; set; }
       [DataMember]
       public DateTime? LastUsageDate  { get; set; }
       [DataMember]
       public string BusinessUnitId  { get; set; }
       [DataMember]
       public string BusinessUnitName  { get; set; }
       [DataMember]
       public string CustomerReference1  { get; set; }
       [DataMember]
       public string CustomerReference2  { get; set; }
       [DataMember]
       public string Subject  { get; set; }
       [DataMember]
       public bool IsSubjectEdited  { get; set; }
       [DataMember]
       public string Routing  { get; set; }
       [DataMember]
       public string SalesmanName  { get; set; }
       [DataMember]
       public string IncotermCode  { get; set; }
       [DataMember]
       public string StageId  { get; set; }
       [DataMember]
       public string StageName  { get; set; }
       [DataMember]
       public DateTime? StageDueDate  { get; set; }
       [DataMember]
       public string RatingCode  { get; set; }
       [DataMember]
       public DateTime? LastActivityDate  { get; set; }
       [DataMember]
       public string LastActivitySubject  { get; set; }
       [DataMember]
       public string LastActivityTypeCode  { get; set; }
       [DataMember]
       public DateTime? NextActivityDate  { get; set; }
       [DataMember]
       public string NextActivitySubject  { get; set; }
       [DataMember]
       public string NextActivityTypeCode  { get; set; }
       [DataMember]
       public string RatingName  { get; set; }
       [DataMember]
       public int StageMaxDays  { get; set; }
       [DataMember]
       public int RatingIndexOrder  { get; set; }
       [DataMember]
       public bool IsAutomaticallyClosed  { get; set; }
       [DataMember]
       public DateTime? AutomaticallyCloseDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public int? AutomaticallyCloseDays  { get; set; }
       [DataMember]
       public string QuoteClosingReasonName  { get; set; }
       [DataMember]
       public string ProductCode  { get; set; }
       [DataMember]
       public string TransitTime  { get; set; }
       [DataMember]
       public string DepartureFrequency  { get; set; }
       [DataMember]
       public DateTime? ETD  { get; set; }
       [DataMember]
       public DateTime? ETA  { get; set; }
       [DataMember]
       public string AgentId  { get; set; }
       [DataMember]
       public string AgentName  { get; set; }
       [DataMember]
       public string AgentAddressId  { get; set; }
       [DataMember]
       public string AgentContactId  { get; set; }
       [DataMember]
       public double? TEU  { get; set; }
       [DataMember]
       public double? ValueOfGoods  { get; set; }
       [DataMember]
       public bool IsChargesByVAT  { get; set; }
       [DataMember]
       public string DirectionName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string FreelancerName  { get; set; }
       [DataMember]
       public string FromPortName  { get; set; }
       [DataMember]
       public string FromPortCountry  { get; set; }
       [DataMember]
       public string ToPortName  { get; set; }
       [DataMember]
       public string ToPortCountry  { get; set; }
       [DataMember]
       public string FromCountryCode  { get; set; }
       [DataMember]
       public string ToCountryCode  { get; set; }
       [DataMember]
       public string QuoteViewId  { get; set; }
       [DataMember]
       public string QuoteCutomerTypeCode  { get; set; }
       [DataMember]
       public string CarrierName  { get; set; }
       [DataMember]
       public string HAWBFBLBL  { get; set; }
       [DataMember]
       public string FollowUpId  { get; set; }
       [DataMember]
       public DateTime LastUpdate  { get; set; }
       [DataMember]
       public bool NewMessage  { get; set; }
       [DataMember]
       public bool hasChanges  { get; set; }
       [DataMember]
       public string Salesman  { get; set; }
       [DataMember]
       public string LastQuoteActivityTypeName  { get; set; }
       [DataMember]
       public string LastActivityByUserName  { get; set; }
       [DataMember]
       public DateTime LastQuoteActivityDate  { get; set; }
       [DataMember]
       public string LastActivityTypeName  { get; set; }
       [DataMember]
       public string NextActivityTypeName  { get; set; }
       [DataMember]
       public bool IsQuoteDataExternal  { get; set; }
       [DataMember]
       public bool IsQuoteDocumentExternal  { get; set; }
       [DataMember]
       public bool TotalPerContainer  { get; set; }
       [DataMember]
       public string QuotationSections  { get; set; }
       [DataMember]
       public double? GrossWeightInKG  { get; set; }
       [DataMember]
       public double? GrossWeightPerTon  { get; set; }
       [DataMember]
       public string NotifyId  { get; set; }
       [DataMember]
       public string NotifyAddressId  { get; set; }
       [DataMember]
       public string NotifyContactId  { get; set; }
       [DataMember]
       public string NotifyName  { get; set; }
       [DataMember]
       public int? NumberOfFollowUps  { get; set; }
       [DataMember]
       public string MoveTypeName  { get; set; }
       [DataMember]
       public string NotifyNote  { get; set; }
       [DataMember]
       public double? ChargeableWeightInKG  { get; set; }
       [DataMember]
       public double? VolumeInCBM  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public string Field11  { get; set; }
       [DataMember]
       public string Field12  { get; set; }
       [DataMember]
       public string Field13  { get; set; }
       [DataMember]
       public string Field14  { get; set; }
       [DataMember]
       public string Field15  { get; set; }
       [DataMember]
       public string Field16  { get; set; }
       [DataMember]
       public string Field17  { get; set; }
       [DataMember]
       public string Field18  { get; set; }
       [DataMember]
       public string Field19  { get; set; }
       [DataMember]
       public string Field20  { get; set; }
       [DataMember]
       public DateTime? RequestDate  { get; set; }
       [DataMember]
       public bool IsCreatedFromTicket  { get; set; }
       [DataMember]
       public DateTime? TicketCreateDate  { get; set; }
       [DataMember]
       public double? EstimatedProfitInLocal  { get; set; }
       [DataMember]
       public double? EstimatedProfitInProfit  { get; set; }
       [DataMember]
       public string CountryForStatisticsId  { get; set; }
       [DataMember]
       public string QuoteHTMLDocumentId  { get; set; }
       [DataMember]
       public string QuoteClosingReasonId  { get; set; }
       [DataMember]
       public string ShipmentSubTypeId  { get; set; }
       [DataMember]
       public string ShipmentSubTypeName  { get; set; }
       [DataMember]
       public double? PickupDeliveryChargeableWeight  { get; set; }
       [DataMember]
       public double? PickupDeliveryVolumetricWeight  { get; set; }
       [DataMember]
       public string RegionalTaxId  { get; set; }
       [DataMember]
       public bool IsMultiCurrency  { get; set; }
   }

}
	 