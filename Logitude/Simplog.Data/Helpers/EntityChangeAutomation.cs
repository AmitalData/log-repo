using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Simplog.Data.Helpers
{
   

    public class EntityChangeAutomation
    {
        public string Id { get; set; }
        public string AutomationId { get; set; }
        public bool IsConditionTrue { get; set; }
        public string ComunicationLogId { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ResultCode { get; set; }
        public string AutomationType { get; set; }
        public string AutomationName { get; set; }
        public string AutomationDescription { get; set; }
        public string type { get; set; }
        public int ExecutionTime { get; set; } 
        public List<AutomationCondition> ConditionsList { get; set; }
    } 

}
