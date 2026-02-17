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
   public partial class ExternalReconciliationLineList
   {
   
       [Key]
       [DataMember]
       public string ReconciliationId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string LedgerTransactionId  { get; set; }
       [DataMember]
       public int GroupNumber  { get; set; }
       [DataMember]
       public string ExternalPageLineId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 