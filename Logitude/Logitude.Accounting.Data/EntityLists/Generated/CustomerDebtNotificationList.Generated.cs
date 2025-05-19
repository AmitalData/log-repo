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
   public partial class CustomerDebtNotificationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string InActive  { get; set; }
       [DataMember]
       public string TypesDebts  { get; set; }
       [DataMember]
       public string DebtLevel  { get; set; }
       [DataMember]
       public decimal DebtLevelAmount  { get; set; }
       [DataMember]
       public string TasksSchedulerId  { get; set; }
       [DataMember]
       public string PaymentNotes  { get; set; }
       [DataMember]
       public string AccountId  { get; set; }
   }

}
	 