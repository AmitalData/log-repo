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
   public partial class DepositConditionList
   {
   
       [Key]
       [DataMember]
       public string DepositId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DepositConditionCode  { get; set; }
       [DataMember]
       public decimal? DepositAmount  { get; set; }
       [DataMember]
       public string DepositConditionName  { get; set; }
   }

}
	 