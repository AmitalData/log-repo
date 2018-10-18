using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
 
  public  class EntityChangeAutomationsSummary
    {
      [Key]
      public string Id { get; set; } 
      public List<ChangeField> ChangeFieldsList { get; set; }
      public List<EntityChangeAutomation> EntityChangeAutomationList { get; set; }



    }

     public  class ChangeField
     {
         public string FieldName { get; set; }
         public string NewValue { get; set; }
         public string OldValue { get; set; }
         public string Via { get; set; }

     }

}
