using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Infrastructure.Data.EntityLists
{
   [DataContract]
   public partial class LastRunDetailList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime LastRunDate  { get; set; }
       [DataMember]
       public string LastRunByUserId  { get; set; }
       [DataMember]
       public string LastRunByUserName  { get; set; }
   }

}
	 