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
   public partial class TariffLinesContainersPriceList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string TariffId  { get; set; }
       [DataMember]
       public string TariffLineId  { get; set; }
       [DataMember]
       public string SurchargeId  { get; set; }
       [DataMember]
       public decimal? Price1  { get; set; }
       [DataMember]
       public decimal? Price2  { get; set; }
       [DataMember]
       public decimal? Price3  { get; set; }
       [DataMember]
       public decimal? Price4  { get; set; }
       [DataMember]
       public decimal? Price5  { get; set; }
       [DataMember]
       public decimal? CostPrice  { get; set; }
   }

}
	 