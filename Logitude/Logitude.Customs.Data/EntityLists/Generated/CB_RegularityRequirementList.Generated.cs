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
   public partial class CB_RegularityRequirementList
   {
   
       [Key]
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string CountryID  { get; set; }
       [DataMember]
       public bool IsAllCountries  { get; set; }
       [DataMember]
       public int? CustomsItemID  { get; set; }
       [DataMember]
       public bool IsAllCustomsItems  { get; set; }
       [DataMember]
       public bool IsLimitedCountryRegularRequire  { get; set; }
       [DataMember]
       public DateTime StartDate  { get; set; }
       [DataMember]
       public DateTime EndDate  { get; set; }
       [DataMember]
       public string InceptionCodeID  { get; set; }
       [DataMember]
       public string RegularityPublicationCodeID  { get; set; }
       [DataMember]
       public string RegularitySourceCodeID  { get; set; }
       [DataMember]
       public string CustomsBookTypeID  { get; set; }
   }

}
	 