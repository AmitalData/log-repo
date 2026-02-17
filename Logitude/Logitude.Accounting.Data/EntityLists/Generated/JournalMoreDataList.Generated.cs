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
   public partial class JournalMoreDataList
   {
   
       [Key]
       [DataMember]
       public string JournalId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string GeneralData  { get; set; }
       [DataMember]
       public bool IsLedgerCreated  { get; set; }
   }

}
	 