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
   public partial class VendorCommissionList
   {
   
       [Key]
       [DataMember]
       public string VendorId  { get; set; }

       [Key]
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public decimal? CommisionPercentage  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 