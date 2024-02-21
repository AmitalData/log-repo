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
   public partial class CB_CustomsItemList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string FullClassification  { get; set; }
       [DataMember]
       public string Parent_CustomsItemID  { get; set; }
       [DataMember]
       public string ComputedCheckDigit  { get; set; }
       [DataMember]
       public string CustomsBookTypeID  { get; set; }
       [DataMember]
       public string CustomsItemCategoryID  { get; set; }
       [DataMember]
       public string CustomsItemHierarchicLocationID  { get; set; }
       [DataMember]
       public string GoodsDescription  { get; set; }
       [DataMember]
       public string Rules  { get; set; }
       [DataMember]
       public string Remarks  { get; set; }
       [DataMember]
       public string Agreements  { get; set; }
       [DataMember]
       public string CustomsRate  { get; set; }
       [DataMember]
       public string PurchaseTax  { get; set; }
       [DataMember]
       public decimal? OptionalTaxAddition  { get; set; }
       [DataMember]
       public string MeasurementUnit  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
   }

}
	 