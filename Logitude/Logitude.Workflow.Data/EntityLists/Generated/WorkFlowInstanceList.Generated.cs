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
   public partial class WorkFlowInstanceList
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
       public string StatusCode  { get; set; }
       [DataMember]
       public DateTime? StartTime  { get; set; }
       [DataMember]
       public DateTime? EndTime  { get; set; }
       [DataMember]
       public string BusinessKey  { get; set; }
       [DataMember]
       public decimal? Duration  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string WorkFlowVersionId  { get; set; }
       [DataMember]
       public int WorkFlowVersionNumber  { get; set; }
       [DataMember]
       public int RetryAttemptsNumber  { get; set; }
       [DataMember]
       public string WorkflowId  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public int NumberOfActivities  { get; set; }
   }

}
	 