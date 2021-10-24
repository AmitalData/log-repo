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
   public partial class ARPaymentBankTranferList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string PaymentId  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string PaymentRef  { get; set; }
       [DataMember]
       public DateTime ValueDate  { get; set; }
       [DataMember]
       public string BankAccountId  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public decimal LocalAmount  { get; set; }
       [DataMember]
       public decimal ForeignAmount  { get; set; }
       [DataMember]
       public decimal ExchageRate  { get; set; }
   }

}
	 