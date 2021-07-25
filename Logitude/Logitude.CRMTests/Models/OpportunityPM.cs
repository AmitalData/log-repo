using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRMTests.Models
{
    public class OpportunityPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OwnerId { get; set; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string ContactId { get; set; }
        public string StageId { get; set; }
        public int Probability { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string RatingCode { get; set; }
        public bool IsClosed { get; set; }
        public string CustomerName { get; set; }
        public string SearchFields { get; set; }
        public string OwnerName { get; set; }
        public string StageName { get; set; }
        public string RatingName { get; set; }
        public int NumberOfShipments { get; set; }
        public double ValueField { get; set; }
        public DateTime LastStageDate { get; set; }
        public string BusinessUnitId { get; set; }
        public int StageProbability { get; set; }
        public int RatingIndexOrder { get; set; }
        public string ContactName { get; set; }
        public string ConcurrencyGUID { get; set; }
        public bool IsCancelled { get; set; }
        public string OpportunityTypeId { get; set; }
        public string OpportunityTypeName { get; set; }
        public bool IsClosedLost { get; set; }
        public string CustomerRankCode { get; set; }
        public string CustomerRankName { get; set; }
        public bool PostToFollowersAsWon { get; set; }
        public bool IsCopy { get; set; }
        public bool IsCustomerBlockedBusinessUnit { get; set; }
        public int ChangeSetOp { get; set; }
    }
}
