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
   public partial class SupplierInvoicePaymentList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceCounterKey  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int SequenceNumeric  { get; set; }
       [DataMember]
       public string PaymentTypeCode  { get; set; }
       [DataMember]
       public decimal PaymentAmount  { get; set; }
       [DataMember]
       public string PaymentTypeName  { get; set; }
   }

}
	 