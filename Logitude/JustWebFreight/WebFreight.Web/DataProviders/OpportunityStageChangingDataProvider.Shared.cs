using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class OpportunityStageChangingDataProvider:BaseDataProvider
    {

        public string TenantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Signature { get; set; }
        public string TenantPhone { get; set; }
        public string TenantFax { get; set; }


        public List<StageChangingRecord> RecordsList { get; set; }

        public class StageChangingRecord
        {
            public string OpportunityName { get; set; }
            public string Country { get; set; }
            public string FromStage { get; set; }
            public string ToStage { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string StageDuration { get; set; }
            public DateTime? CreateDate { get; set; }
            public string Customer { get; set; }
            public string OpportunityType { get; set; }
            public string Salesman { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string CustomerPrimaryContactName { get; set; }
            public string CustomerPrimaryContactEmail { get; set; }
        }
    }
}