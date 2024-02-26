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
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public int SeriesId  { get; set; }
       [DataMember]
       public int ChequeCounterBegin  { get; set; }
       [DataMember]
       public int ChequeCounterEnd  { get; set; }
       [DataMember]
       public string BankAccountId  { get; set; }
   }

}
	 