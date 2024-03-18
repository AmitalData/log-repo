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
       public int ID  { get; set; }
       [DataMember]
       public int RegularityRequirementID  { get; set; }
       [DataMember]
       public string CountryID  { get; set; }
   }

}
	 