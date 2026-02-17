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
   public partial class ExternalPageAdditionalDataList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string ObjectTableId  { get; set; }

       [Key]
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string LastPageNumber  { get; set; }
       [DataMember]
       public DateTime? LastPageEndDate  { get; set; }
       [DataMember]
       public decimal? LastPageCloseBalance  { get; set; }
   }

}
	 