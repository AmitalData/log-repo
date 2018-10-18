using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityDws
{
 public   class OpportunityDW
    {

   
        public string OpportunityId { get; set; }
        public string Subject { get; set; }
        public string OpportunityTypeId { get; set; }
        public string OpportunityTypeName { get; set; }
        public int Tenant { get; set; }
        public string IncomeCurrency { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string  RatingName { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public DateTime? EstimatedClosingDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string RatingCode { get; set; }
        public string ClosingReasonId { get; set; }
        public string ClosingReasonName { get; set; }
        public string ClosingDescription { get; set; }
        public string  ClientIDDW { get; set; }
        public string ClientId { get; set; }
        public string SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public string LeadsourceName { get; set; }
        public string Field4 { get; set; }
        public int? NumberOfShipments { get; set; }
        public DateTime? ActualClosingDate { get; set; }
        public string Reseller { get; set; }

        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public bool IsCancelled { get; set; }
        



    }
}
