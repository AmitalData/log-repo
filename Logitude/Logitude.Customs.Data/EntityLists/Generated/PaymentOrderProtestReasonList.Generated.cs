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
   public partial class PaymentOrderProtestReasonList
   {
   
       [Key]
       [DataMember]
       public string PaymentOrderId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string ProtestTypeCode  { get; set; }
       [DataMember]
       public string CustomsAgentExplanation  { get; set; }
       [DataMember]
       public string InvoiceNumber  { get; set; }
       [DataMember]
       public decimal? GoodsItemLineNumber  { get; set; }
       [DataMember]
       public string GoodsItemClassification  { get; set; }
       [DataMember]
       public decimal? AmountInDispute  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ProtestTypeName  { get; set; }
   }

}
	 