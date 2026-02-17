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
   public partial class BankDepositLineList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DepositId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string ARPaymentChequeId  { get; set; }
       [DataMember]
       public bool? IsOutOfDeposit  { get; set; }
       [DataMember]
       public DateTime? OutOfDepositeDate  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string ChequeNumber  { get; set; }
       [DataMember]
       public DateTime DueDate  { get; set; }
       [DataMember]
       public decimal LocalAmount  { get; set; }
       [DataMember]
       public string Currency  { get; set; }
       [DataMember]
       public decimal ForeignAmount  { get; set; }
       [DataMember]
       public string AccountNumber  { get; set; }
       [DataMember]
       public string Bank  { get; set; }
       [DataMember]
       public string Branch  { get; set; }
       [DataMember]
       public string ARPaymentNumber  { get; set; }
       [DataMember]
       public string ARPaymentId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ChequeStatusName  { get; set; }
       [DataMember]
       public string ChequeStatusCode  { get; set; }
   }

}
	 