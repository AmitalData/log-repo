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
   public partial class CustomsCollateralsConditionList
   {
   
       [Key]
       [DataMember]
       public string CustomsCollateralId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string ConditionCode  { get; set; }
       [DataMember]
       public decimal? RequestedAmount  { get; set; }
       [DataMember]
       public string ConditionName  { get; set; }
   }

}
	 