namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class EventTypeDetails
    {
        public int Tenant { get; set; }       
        public string Code { get; set; }
        public bool AddedManually { get; set; }      
        public string EnglishName { get; set; }       
        public string LocalName { get; set; }
        public string LocalNameBack_up { get; set; }

        public bool IsManualEntry { get; set; }
        public string EntityStatusId { get; set; }
        public string ObjectTableId { get; set; }     
        public bool ShortView { get; set; }
        public bool IsFollowUp { get; set; }
        public string FollowUpEnglishName { get; set; }
        public string FollowUpLocalName { get; set; }
        public string FollowUpLocalNameBack_up { get; set; }

        public bool ManualActivatedFollowUp { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string EventTypeCategoryCode { get; set; }
        public bool IsCustomerView { get; set; }
        public bool IsAgentView { get; set; }
        public bool IsSharedLogisticsEnabled { get; set; }
        public bool AllowedInAutomation { get; set; }

        //public int StatusWeight { get; set; }
    }
}