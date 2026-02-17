using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class AccountingPeriodList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public int Year  { get; set; }
       [DataMember]
       public string PeriodTypeCode  { get; set; }
       [DataMember]
       public string PeriodTypeName  { get; set; }
       [DataMember]
       public int OpenMonth  { get; set; }
       [DataMember]
       public int? ClosedMonth  { get; set; }
   }

}
	 