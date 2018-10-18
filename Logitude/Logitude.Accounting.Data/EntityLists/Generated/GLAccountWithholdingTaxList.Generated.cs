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
   public partial class GLAccountWithholdingTaxList
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
       public string GLAccountId  { get; set; }
       [DataMember]
       public DateTime FromDate  { get; set; }
       [DataMember]
       public DateTime ToDate  { get; set; }
       [DataMember]
       public int Percentage  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
   }

}
	 