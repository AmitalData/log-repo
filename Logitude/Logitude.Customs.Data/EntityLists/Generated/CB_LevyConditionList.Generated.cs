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
   public partial class CB_LevyConditionList
   {
   
       [Key]
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public int? LevyConditionNumber  { get; set; }
       [DataMember]
       public string LevyGoodsDescription  { get; set; }
       [DataMember]
       public int CustomsItemID  { get; set; }
       [DataMember]
       public int? VendorID  { get; set; }
       [DataMember]
       public string CountryGroupID  { get; set; }
       [DataMember]
       public bool IsCountriesGroup  { get; set; }
       [DataMember]
       public string CountryID  { get; set; }
       [DataMember]
       public int TradeLevyID  { get; set; }
       [DataMember]
       public DateTime StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
   }

}
	 