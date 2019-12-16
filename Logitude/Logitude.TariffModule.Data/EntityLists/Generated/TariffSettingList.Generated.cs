using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TariffModule.Data.EntityLists
{
   [DataContract]
   public partial class TariffSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public double? DefaultWarningPercentage  { get; set; }
       [DataMember]
       public string AirDefaultStepsId  { get; set; }
       [DataMember]
       public string LCLDefaultStepsId  { get; set; }
   }

}
	 