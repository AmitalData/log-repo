using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CargoTracking.Data.EntityLists
{
   [DataContract]
   public partial class CargoDisconnectQueueList
   {
   
       [Key]
       [DataMember]
       public int Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 