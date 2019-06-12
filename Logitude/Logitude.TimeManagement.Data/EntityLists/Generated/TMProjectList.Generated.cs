using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TimeManagement.Data.EntityLists
{
   [DataContract]
   public partial class TMProjectList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string ProjectNumber  { get; set; }
       [DataMember]
       public string MyAllOpenProjects  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public bool IsInnerProject  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string BudgetId  { get; set; }
       [DataMember]
       public string CategoryId  { get; set; }
       [DataMember]
       public bool IsProrated  { get; set; }
       [DataMember]
       public string ExternalProjectNumber  { get; set; }
       [DataMember]
       public string CategoryName  { get; set; }
       [DataMember]
       public bool ExcludeFromProrating  { get; set; }
       [DataMember]
       public string DayOffTypeCode  { get; set; }
       [DataMember]
       public bool BlockedForDataEntry  { get; set; }
   }

}
	 