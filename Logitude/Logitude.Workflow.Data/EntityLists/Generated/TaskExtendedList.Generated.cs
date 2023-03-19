using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Workflow.Data.EntityLists
{
   [DataContract]
   public partial class TaskExtendedList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Fields  { get; set; }
       [DataMember]
       public string ToDoConditions  { get; set; }
       [DataMember]
       public string DoneConditions  { get; set; }
       [DataMember]
       public string Description  { get; set; }
   }

}
	 