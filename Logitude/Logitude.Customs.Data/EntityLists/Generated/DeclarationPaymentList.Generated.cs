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
   public partial class DeclarationPaymentList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public DateTime? PaymentDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string SignatoryIdentification  { get; set; }
       [DataMember]
       public bool IsProcessA  { get; set; }
       [DataMember]
       public string ProcessADescription  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? FuturePaymentDateTime  { get; set; }
       [DataMember]
       public int AutomaticPayment  { get; set; }
   }

}
	 