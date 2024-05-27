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
   public partial class CustomsItemList
   {
          [DataMember]
       public int CustomsBookTypeID  { get; set; }
       [DataMember]
       public string FullClassification  { get; set; }
       [DataMember]
       public int CustomsItemCategoryID  { get; set; }
       [DataMember]
       public int? CustomsItemHierarchicLocationID  { get; set; }
       [DataMember]
       public string ComputedCheckDigit  { get; set; }

       [Key]
       [DataMember]
       public string ID  { get; set; }
   }

}
	 