using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class Aur_PaymentItemList
   {
   
       [Key]
       [DataMember]
       public string PaymentId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public int? PaymentSequence  { get; set; }
       [DataMember]
       public string QuoteId  { get; set; }
       [DataMember]
       public string Project  { get; set; }
       [DataMember]
       public string ProjectNumber  { get; set; }
       [DataMember]
       public string SectionType  { get; set; }
       [DataMember]
       public decimal BaseAmount  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 