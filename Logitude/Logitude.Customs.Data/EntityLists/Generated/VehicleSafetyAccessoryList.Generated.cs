using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class VehicleSafetyAccessoryList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string VehicleId  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string VehicleSafetyAccessoryCode  { get; set; }
       [DataMember]
       public string VehicleSafAccessoryInstlTypCod  { get; set; }
       [DataMember]
       public string VehicleSafAccessoryInstlTypName  { get; set; }
       [DataMember]
       public string VehicleSafetyAccessoryName  { get; set; }
   }

}
	 