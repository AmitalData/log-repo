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
   public partial class PaymentOrderLineList
   {
   
       [Key]
       [DataMember]
       public string PaymentOrderId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string ParagraphTypeCode  { get; set; }
       [DataMember]
       public decimal? Amount  { get; set; }
       [DataMember]
       public string ParagraphTypeName  { get; set; }
   }

}
	 