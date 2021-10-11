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
   public partial class ARPaymentsJournalList
   {
   
       [Key]
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string PaymentId  { get; set; }

       [Key]
       [DataMember]
       public bool IsVoided  { get; set; }
   }

}
	 