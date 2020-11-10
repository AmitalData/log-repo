using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class EntityChange
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public string AutomationConditionFieldsXml { get; set; }
        public DateTime? CheckStartDate  { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool HasExecutedRecord { get; set; }
        public string ChangesFieldsXml { get; set; }
        public string ChangesAutomationFieldsXml { get; set; }

        public string SetAutomationSsucceedXml { get; set; }
        public string EmailAutomationSsucceedXml { get; set; }



        public string SetAutomationFailedXml { get; set; }
        public string EmailAutomationFailedXml { get; set; }

        public string FollowUpAutomationFailedXml { get; set; }
        public string SetSLAAutomationFailedXml { get; set; }
        public string FollowUpAutomationSsucceedXml { get; set; }
        public string SetSLAAutomationSsucceedXml { get; set; }

        public string QueuedTaskAutomationFailedXml { get; set; }
        public string QueuedTaskAutomationSsucceedXml { get; set; }


        public string SendInterfaceAutomationFailedXml { get; set; }
        public string SendInterfaceAutomationSsucceedXml { get; set; }

        public int ExecutionTime { get; set; }

        [ForeignKey("CreateByUserId")]
        public virtual User CreateByUser { get; set; }
    }
}
