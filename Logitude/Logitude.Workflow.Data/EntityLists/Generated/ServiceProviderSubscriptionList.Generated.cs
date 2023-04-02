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
   public partial class ServiceProviderSubscriptionList
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
       public string UserEmail  { get; set; }
       [DataMember]
       public string AccessToken  { get; set; }
       [DataMember]
       public string RefreshToken  { get; set; }
       [DataMember]
       public string EmailProvider  { get; set; }
       [DataMember]
       public string WorkflowNumber  { get; set; }
       [DataMember]
       public string AdditionalSettings  { get; set; }
       [DataMember]
       public DateTime SubscriptionExpirationDateTime  { get; set; }
       [DataMember]
       public DateTime AccessTokenExpirationDateTime  { get; set; }
       [DataMember]
       public string WebhookParams  { get; set; }
   }

}
	 