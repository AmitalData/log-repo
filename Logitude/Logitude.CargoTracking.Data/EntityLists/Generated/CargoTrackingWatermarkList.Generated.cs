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
   public partial class CargoTrackingWatermarkList
   {
   
       [Key]
       [DataMember]
       public string TableName  { get; set; }
       [DataMember]
       public DateTime? LastUpdateDate  { get; set; }
   }

}
	 