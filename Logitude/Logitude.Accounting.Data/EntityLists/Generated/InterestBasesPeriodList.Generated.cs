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
   public partial class InterestBasesPeriodList
   {
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

       [Key]
       [DataMember]
       public string InterestBaseTypeId  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public DateTime InterestBaseStartDate  { get; set; }
       [DataMember]
       public decimal InterestRate  { get; set; }
   }

}
	 