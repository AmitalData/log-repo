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
   public partial class AccountingEntitiesJournalList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string AccountingEntityId  { get; set; }
       [DataMember]
       public string AccountingEntityCode  { get; set; }
       [DataMember]
       public string Action  { get; set; }
       [DataMember]
       public string ChildEntityId  { get; set; }
   }

}
	 