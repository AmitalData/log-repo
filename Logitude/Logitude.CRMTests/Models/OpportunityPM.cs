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
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string OwnerId { get; set; }
        public string BusinessUnitId { get; set; }
        public string RatingCode { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public int Probability { get; set; }
        public string OpportunityTypeId { get; set; }
        public string Subject { get; set; }
        public object NumberOfShipments { get; set; }
        public string CustomerId { get; set; }
        public string ContactId { get; set; }
    }
}
