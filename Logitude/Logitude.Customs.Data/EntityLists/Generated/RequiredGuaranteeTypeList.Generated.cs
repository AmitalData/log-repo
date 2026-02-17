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
   public partial class RequiredGuaranteeTypeList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string GuaranteeId  { get; set; }
       [DataMember]
       public string GuaranteeTypeCode  { get; set; }
       [DataMember]
       public decimal? GuaranteeAmount  { get; set; }
       [DataMember]
       public string GuaranteeTypeName  { get; set; }
   }

}
	 