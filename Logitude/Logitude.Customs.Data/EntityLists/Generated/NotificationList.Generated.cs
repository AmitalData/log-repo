using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class NotificationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string NotificationDefinitionCode  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string AssigneToId  { get; set; }
       [DataMember]
       public string AssigneToNotificationTypeCode  { get; set; }
       [DataMember]
       public string DeclarationOfficeCode  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public bool IsClosedBCustomOffice  { get; set; }
       [DataMember]
       public bool IsClosedByAssignee  { get; set; }
       [DataMember]
       public DateTime? DueDate  { get; set; }
       [DataMember]
       public bool IsSeenByAssignee  { get; set; }
       [DataMember]
       public string ResponseNotes  { get; set; }
       [DataMember]
       public string CreatedByRequestID  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ObjectTableName  { get; set; }
       [DataMember]
       public string NotificationDefinitionName  { get; set; }
       [DataMember]
       public string AssigneToName  { get; set; }
       [DataMember]
       public bool IsHandledByCustomOffice  { get; set; }
       [DataMember]
       public string Reference1Number  { get; set; }
       [DataMember]
       public string Reference2Number  { get; set; }
       [DataMember]
       public string DepartmentId  { get; set; }
       [DataMember]
       public string ClosedByCustomOfficeUserId  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string DepartmentName  { get; set; }
       [DataMember]
       public string ClosedByAssignee  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public bool BadjCount  { get; set; }
       [DataMember]
       public string ClosedByCustomOfficeUserName  { get; set; }
       [DataMember]
       public string ClosedByAssigneeName  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string ResponseToMessage  { get; set; }
   }

}
	 