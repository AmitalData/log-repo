using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{

  [DataContract(Namespace = "")]
    public class AutomationCondition
    {
   
        [DataMember]
        public int Tenant { get; set; }
  
        [DataMember]
        public string ObjectFieldId { get; set; }
        [DataMember]        
        public string OperatorCode { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ConditionType { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public DateTime? UpdateDate { get; set; }
        [DataMember]
        public string CreatedByUserId { get; set; }
        [DataMember]
        public string UpdatedByUserId { get; set; }

        [DataMember]
        public string ObjectFieldType { get; set; }

 
    }
}

