using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Workflow.Data.EntityLists
{
   [DataContract]
   public partial class TaskList : CustomFieldList
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
       public string Subject  { get; set; }
       [DataMember]
       public DateTime DueDate  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string PriorityId  { get; set; }
       [DataMember]
       public string StatusId  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string TaskTypeId  { get; set; }
       [DataMember]
       public string EntityObjectTableId  { get; set; }
       [DataMember]
       public string ClosedByUserId  { get; set; }
       [DataMember]
       public DateTime? ClosedDate  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string CheckWithId  { get; set; }
       [DataMember]
       public string EntityNumber  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string PriorityName  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string TaskTypeName  { get; set; }
       [DataMember]
       public string EntityObjectTableName  { get; set; }
       [DataMember]
       public string ClosedByUserName  { get; set; }
       [DataMember]
       public string CheckWithName  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string Fields  { get; set; }
       [DataMember]
       public string ToDoConditions  { get; set; }
       [DataMember]
       public string DoneConditions  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public bool IsAssigned  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
   }

}
	 