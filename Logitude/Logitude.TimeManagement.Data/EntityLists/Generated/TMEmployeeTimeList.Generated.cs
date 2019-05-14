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
   public partial class TMEmployeeTimeList
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
       public string EmployeeUserId  { get; set; }
       [DataMember]
       public DateTime DateOfWork  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public int TimeInMinutes  { get; set; }
       [DataMember]
       public string WINumber  { get; set; }
       [DataMember]
       public string ProjectId  { get; set; }
       [DataMember]
       public string LocationCode  { get; set; }
       [DataMember]
       public string ProjectName  { get; set; }
       [DataMember]
       public string ProjectDescription  { get; set; }
       [DataMember]
       public string AnalyzeQueueId  { get; set; }
       [DataMember]
       public string SprintId  { get; set; }
       [DataMember]
       public double ProratedDuration  { get; set; }
       [DataMember]
       public double FullDuration  { get; set; }
       [DataMember]
       public bool NeedsProrating  { get; set; }
       [DataMember]
       public string LocationName  { get; set; }
       [DataMember]
       public string SprintName  { get; set; }
   }

}
	 