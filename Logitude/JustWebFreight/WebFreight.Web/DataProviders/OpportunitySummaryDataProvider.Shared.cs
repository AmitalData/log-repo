using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class OpportunitySummaryDataProvider : BaseDataProvider
    {
        public string CustomerName { get; set; }
        public string CustomerLocalName { get; set; }
        public string CustomerMainAddress { get; set; }
        public string CustomerVat { get; set; }
        public string CustomerIndustry { get; set; }        
        public string OpportunityOwner { get; set; }
        public string MeetingSummary { get; set; }
        public string Subject { get; set; }
        public string TenantName { get; set; }
        public bool MeetingSummaryRightToLeft { get; set; }

        public List<OpportunityContactClass> OpportunityContacts { get; set; }
        public List<OpportunityProductClass> OpportunityProducts { get; set; }
        public List<OpportunityAdditionalServiceClass> OpportunityAdditionalServices { get; set; }
        public List<OpportunityTaskClass> OpportunityTasks { get; set; }
    }

    public class OpportunityContactClass
    {
        public string ContactName { get; set; }
        public string ContactPosition { get; set; }
        public string ContactPhone { get; set; }
        public string ContactNotes { get; set; }
        public string ContactEmail { get; set; }
    }

    public class OpportunityProductClass
    {
        public string ProductName { get; set; }
        public string ProductSummary { get; set; }
        public string ProductNote { get; set; }
        public string ProductPrepaidCollect { get; set; }
        public bool NotesRightToLeft { get; set; }
    }

    public class OpportunityAdditionalServiceClass
    {
        public string AdditionalServiceName { get; set; }
        public string AdditionalServiceNote { get; set; }
        public bool NotesRightToLeft { get; set; }
    }

    public class OpportunityTaskClass
    {
        public string TaskSubject { get; set; }
        public string TaskOwner { get; set; }
        public DateTime? TaskDueDate { get; set; }
    }
}