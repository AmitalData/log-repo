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
   public partial class SLALineList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SLAHeaderId  { get; set; }
       [DataMember]
       public string SeverityId  { get; set; }
       [DataMember]
       public string BusinessHoursId  { get; set; }
       [DataMember]
       public int? FirstResponseTime  { get; set; }
       [DataMember]
       public string FirstResponseTimeUnit  { get; set; }
       [DataMember]
       public int? FirstResponseTimeInMinute  { get; set; }
       [DataMember]
       public int? ResolveWithinTime  { get; set; }
       [DataMember]
       public string ResolveWithinTimeUnit  { get; set; }
       [DataMember]
       public int? ResolveWithinTimeInMinute  { get; set; }
       [DataMember]
       public bool FirstResponseEscalate  { get; set; }
       [DataMember]
       public bool ResolveWithinEscalate  { get; set; }
       [DataMember]
       public string SeverityName  { get; set; }
   }

}
	 