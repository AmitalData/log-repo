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
   public partial class CB_CountriesExclusionList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public string RegularityRequirementID  { get; set; }
       [DataMember]
       public string CountryID  { get; set; }
   }

}
	 