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
   public partial class WorkFlowVersionList
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
       public int VersionNumber  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string WorkflowId  { get; set; }
       [DataMember]
       public string FlowJson  { get; set; }
       [DataMember]
       public string Entity  { get; set; }
       [DataMember]
       public string Trigger  { get; set; }
       [DataMember]
       public DateTime? ActivatedDate  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
   }

}
	 