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
   public partial class CB_VendorList
   {
   
       [Key]
       [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public string Title  { get; set; }
       [DataMember]
       public int State  { get; set; }
       [DataMember]
       public string EnglishCountryName  { get; set; }
       [DataMember]
       public string VendorSingleStringAddress  { get; set; }
   }

}
	 