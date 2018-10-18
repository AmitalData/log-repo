using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class TicketClassificationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string ParentId  { get; set; }
       [DataMember]
       public string ParentName  { get; set; }
       [DataMember]
       public string DefaultSeverityId  { get; set; }
       [DataMember]
       public string EmployeeGroupId  { get; set; }
       [DataMember]
       public string ManagerUserId  { get; set; }
       [DataMember]
       public string EscalationNotify  { get; set; }
       [DataMember]
       public string ManagerUserEmail  { get; set; }
   }

}
	 