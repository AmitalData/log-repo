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
   public partial class ModificationAndDiscountTypeList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public bool IsRelevantGoodsItem  { get; set; }
       [DataMember]
       public bool IsRelevantInvoice  { get; set; }
       [DataMember]
       public bool IsRelevantInvoiceExport  { get; set; }
       [DataMember]
       public bool IsRelevantGoodsItemExport  { get; set; }
   }

}
	 