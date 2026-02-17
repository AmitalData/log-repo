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
   public partial class GLAccountCurrencyList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string CurrencyName  { get; set; }
       [DataMember]
       public string MainGLAccountId  { get; set; }
       [DataMember]
       public string GLAccountNumber  { get; set; }
       [DataMember]
       public string GLAccountName  { get; set; }
   }

}
	 