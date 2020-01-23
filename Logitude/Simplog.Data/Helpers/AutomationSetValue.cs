using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
  

    [DataContract(Namespace = "")]
    public class AutomationSetValue
    {
  
        [DataMember]
        public string OperatorCode { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string ObjectFieldId { get; set; }
      
       [DataMember]
        public string FieldName { get; set; }

       [DataMember]
       public string DataTypeCode { get; set; }


        [DataMember]
        public string ObjectFieldCode { get; set; }
    }
}
