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
   public partial class WorkFlowInstanceActivityList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public int Sequence  { get; set; }
       [DataMember]
       public string ActionName  { get; set; }
       [DataMember]
       public DateTime? StartTime  { get; set; }
       [DataMember]
       public decimal? Duration  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string WorkflowInstanceId  { get; set; }
       [DataMember]
       public DateTime? EndTime  { get; set; }
       [DataMember]
       public string ErrorMessage  { get; set; }
       [DataMember]
       public string Result  { get; set; }
       [DataMember]
       public string ActionType  { get; set; }
   }

}
	 