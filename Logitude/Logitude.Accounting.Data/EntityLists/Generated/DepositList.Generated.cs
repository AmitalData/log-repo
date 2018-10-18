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
   public partial class DepositList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public int DepositNumber  { get; set; }
       [DataMember]
       public DateTime DepositDate  { get; set; }
       [DataMember]
       public string DepositCurrencyId  { get; set; }
       [DataMember]
       public decimal LocalDepositAmount  { get; set; }
       [DataMember]
       public decimal ForeignAmount  { get; set; }
       [DataMember]
       public string DepositBankAccountId  { get; set; }
       [DataMember]
       public string CashBookId  { get; set; }
   }

}
	 