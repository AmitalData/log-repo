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
   public partial class RevaluationList
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
       public string SearchFields  { get; set; }
       [DataMember]
       public int RevaluationNumber  { get; set; }
       [DataMember]
       public DateTime RevaluationDate  { get; set; }
       [DataMember]
       public string ChartOfAccountsId  { get; set; }
       [DataMember]
       public string ChartOfAccountsName  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public string GLAccountName  { get; set; }
       [DataMember]
       public string GLAccountNumber  { get; set; }
       [DataMember]
       public bool? RevaluationEnabled  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string Status  { get; set; }
       [DataMember]
       public string Message  { get; set; }
       [DataMember]
       public string RevaluationsGLAccountId  { get; set; }
       [DataMember]
       public string RevaluationsGLAccountName  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
   }

}
	 