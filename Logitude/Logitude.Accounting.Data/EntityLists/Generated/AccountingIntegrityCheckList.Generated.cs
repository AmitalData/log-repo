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
   public partial class AccountingIntegrityCheckList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDateTimeUTC  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string ParametersXML  { get; set; }
       [DataMember]
       public string ResultXML  { get; set; }
       [DataMember]
       public bool HasException  { get; set; }
       [DataMember]
       public DateTime? DoneDateTimeUTC  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public DateTime FromMonthInclusive  { get; set; }
       [DataMember]
       public DateTime ToMonthInclusive  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool ShouldFix  { get; set; }
   }

}
	 