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
   public partial class JournalExternalReconcileList
   {
          [DataMember]
       public string ReconcileExternalPageLineId  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public string OriginalJournalId  { get; set; }
   }

}
	 