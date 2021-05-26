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
   public partial class GLAccountCardsDataList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SalesmanUserId  { get; set; }
       [DataMember]
       public double? CreditLimit  { get; set; }
       [DataMember]
       public string PaymentTermId  { get; set; }
       [DataMember]
       public string CollectorUserId  { get; set; }
       [DataMember]
       public string Phone  { get; set; }
       [DataMember]
       public string VatNumber  { get; set; }
       [DataMember]
       public decimal? TotalOpenShipments  { get; set; }
       [DataMember]
       public double? InsuredcreditLimit  { get; set; }
   }

}
	 