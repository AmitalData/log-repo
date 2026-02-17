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
   public partial class ClaimsRelatedEntitiesAmountList
   {
   
       [Key]
       [DataMember]
       public string ClaimId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int CounterKey  { get; set; }

       [Key]
       [DataMember]
       public int LineNo  { get; set; }
       [DataMember]
       public string PaymentTypeCode  { get; set; }
       [DataMember]
       public string PaymentTypeName  { get; set; }
       [DataMember]
       public decimal? Amount  { get; set; }
   }

}
	 