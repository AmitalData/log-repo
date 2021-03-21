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
   public partial class GLAccountRecocileDataList
   {
   
       [Key]
       [DataMember]
       public string AccountId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 