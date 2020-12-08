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
   public partial class GLAccountMoreDataList
   {
   
       [Key]
       [DataMember]
       public string AccountId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public decimal BalanceInLocalCurrency  { get; set; }
       [DataMember]
       public decimal LocalBalanceInDue  { get; set; }
       [DataMember]
       public DateTime? NextDueDate  { get; set; }
       [DataMember]
       public decimal? TotalOpenChequesInLocalCur  { get; set; }
       [DataMember]
       public decimal? TotFutureOpenChequesInLocalCur  { get; set; }
   }

}
	 