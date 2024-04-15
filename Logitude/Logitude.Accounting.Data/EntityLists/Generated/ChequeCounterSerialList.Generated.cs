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
   public partial class ChequeCounterSerialList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int SeriesId  { get; set; }
       [DataMember]
       public int ChequeCounterBegin  { get; set; }
       [DataMember]
       public int ChequeCounterEnd  { get; set; }

       [Key]
       [DataMember]
       public string BankAccountId  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
   }

}
	 