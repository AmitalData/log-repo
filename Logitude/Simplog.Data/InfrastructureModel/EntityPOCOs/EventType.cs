using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class EventType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string  Code { get; set; }
        public bool AddedManually { get; set; }      
        public string EnglishName { get; set; }   
        public string LocalName { get; set; }
        public bool IsManualEntry { get; set; }
        public string EntityStatusId { get; set; }
        public string ObjectTableId { get; set; }
        public bool ShortView { get; set; }
        public bool IsFollowUp { get; set; }
        public string FollowUpEnglishName { get; set; }
        public string FollowUpLocalName { get; set; }
        public bool ManualActivatedFollowUp { get; set; }
        public bool InActive { get; set; }
        public string CustomerRoleId { get; set; }
        public string AgentRoleId { get; set; }
        public bool IsCustomerView { get; set; }
        public bool IsAgentView { get; set; }
        public bool IsSharedLogisticsEnabled { get; set; }
        public string EventTypeCategoryCode { get; set; }
        public string SearchFields { get; set; }
        public bool AllowedInAutomation { get; set; }
        public string CustomField { get; set; }

        public DateTime? UpdateDate { get; set; }

        
        [ForeignKey("CustomerRoleId")]
        public Role CustomerRole { get; set; }

        [ForeignKey("AgentRoleId")]
        public Role AgentRole { get; set; }

        [ForeignKey("EntityStatusId")]
        public virtual EntityStatus EntityStatus { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("EventTypeCategoryCode")]
        public virtual EventTypeCategory EventTypeCategory { get; set; }

        public bool IsStatusNotModified { get; set; }
        public string EventTrigger { get; set; }
    }
}