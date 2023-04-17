using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class EventTypeList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public bool AddedManually { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool IsManualEntry { get; set; }
        public string EntityStatusId { get; set; }
        public string ObjectTableId { get; set; }
        public string EventGroupCode { get; set; }
        public bool ShortView { get; set; }
        public bool IsFollowUp { get; set; }
        public string FollowUpEnglishName { get; set; }
        public string FollowUpLocalName { get; set; }
        public bool ManualActivatedFollowUp { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string EntityStatusName { get; set; }
        public string EventTypeCategoryCode { get; set; }
        public bool IsCustomerView { get; set; }
        public bool IsAgentView { get; set; }
        public bool IsSharedLogisticsEnabled { get; set; }
        public DateTime? EventDateTime { get; set; }
        public bool AllowedInAutomation { get; set; }
        public string CustomField { get; set; }
        public int? EntityStatusWeight { get; set; }
        public string EventTrigger { get; set; }
        public virtual List<EventRemark> EventRemarks { get; set; }

    }
}