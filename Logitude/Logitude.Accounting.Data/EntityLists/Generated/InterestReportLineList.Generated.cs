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
   public partial class InterestReportLineList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string InterestReportId  { get; set; }
       [DataMember]
       public string InterestTransactionId  { get; set; }
   }

}
	 