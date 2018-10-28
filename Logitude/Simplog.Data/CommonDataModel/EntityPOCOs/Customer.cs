using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    
    public class Customer
    {
        [Key]
        public string Id { get; set; }      
        public int Tenant { get; set; }
        public string RankId { get; set; }
        public string BillToId { get; set; }
        public string AccountManagerUserId { get; set; }
        public string SalesmanUserId { get; set; }
        public bool StartWorkingManuallySet { get; set; }
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
        public string ClassifierId { get; set; }
        public string CollectorId { get; set; } 
        public string CustomerSizeId { get; set; }

        public virtual Card Card { get; set; }

        [ForeignKey("BillToId")]
        public virtual Card BillToCard { get; set; }

        [ForeignKey("SalesmanUserId")]
        public virtual User SalesmanUser { get; set; }

        public virtual User AccountManagerUser { get; set; }

        [ForeignKey("ClassifierId")]
        public virtual User Classifier { get; set; }

        [ForeignKey("CollectorId")]
        public virtual User Collector { get; set; }
              
        [ForeignKey("RankId")]
        public virtual Rank Rank { get; set; }
               
        public string IndustryId { get; set; }       
        public virtual Industry Industry { get; set; }        

        public string LeadSourceId { get; set; }              
        public virtual LeadSource LeadSource { get; set; }

        public double? CreditLimit { get; set; }
        public string LeadDescription { get; set; }
        public bool IsCustomer { get; set; }
        public string CustomerStatusCode { get; set; }
        public string BeforeDeactiveStatusCode { get; set; }
        public DateTime? ReadyForActivationDate { get; set; }
        public string RegionId { get; set; }

        public DateTime? ActivationDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public DateTime? ActivationRequestDate { get; set; }
        public string ActivatedByUserId { get; set; }
        public string SetAsInactiveByUserId { get; set; }
        public string ActivationRequestedByUserId { get; set; }
        public virtual User ActivatedByUser { get; set; }
        public virtual User SetAsInactiveByUser { get; set; }
        public virtual User ActivationRequestedByUser { get; set; }

        public string FreelancerId { get; set; }
        [ForeignKey("FreelancerId")]
        public virtual User Freelancer { get; set; }

        public string ForwarderId { get; set; }
        [ForeignKey("ForwarderId")]
        public virtual Card Forwarder { get; set; }

        public string CustomsAgentId {get; set;}
        [ForeignKey("CustomsAgentId")]
        public virtual Card CustomsAgent { get; set; }
            
        public string MediatorId { get; set; }
        [ForeignKey("MediatorId")]
        public virtual Card Mediator {get; set;}
       
        [ForeignKey("CustomerStatusCode")]
        public virtual CustomerStatus CustomerStatus { get; set; }

        [ForeignKey("BeforeDeactiveStatusCode")]
        public virtual CustomerStatus BeforeDeactiveStatus { get; set; }

        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        [ForeignKey("CustomerSizeId")]
        public virtual CustomerSize CustomerSize { get; set; }

        public DateTime? LastShipmentDate { get; set; }
        public DateTime? StartWorkingDate { get; set; }
        public DateTime? LastCallDate { get; set; }
        public DateTime? LastMeetingDate { get; set; }
        public DateTime? LastOpportunityDate { get; set; }
        public DateTime? FirstInvoiceDate { get; set; }
        public DateTime? FirstShipmentDate { get; set; }
        public DateTime? LastQuoteDate { get; set; }
        public DateTime? LastInteractionDate { get; set; }
        public bool ActivityWatch { get; set; }
        public string KnownConsignor { get; set; }
        public DateTime? KCExpirationDate { get; set; }
        public bool LogBoxActivated { get; set; }
        public bool IsPrivateLabelCustomer { get; set; }
        public string CompetitorFields { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public double? CreditLimitAmount { get; set; }
        public double? CreditLimitOpenBalance { get; set; }
        public int? CreditLimitWarningPercentage { get; set; }
        public bool BlockNewInvoiceCreation { get; set; }
        public bool BlockNewShipmentCreation { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
    }
}