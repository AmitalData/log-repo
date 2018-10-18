using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
   public class AutomationConditionFields
    {


        public bool IsRunMasterHouseAutomation { get; set; }
        public string OtherObjectTableIdWithLastUpdate { get; set; }

        public DateTime? LastUpdateDate { get; set; }
        public string ObjectTableId { get; set; }

        private List<Field> fields;
       [DataMember]
       public List<Field> Fields
       {
           get
           {
               return this.fields;
           }
           set
           {
               this.fields = value;
           }
       }


    }
}
