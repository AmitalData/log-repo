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
   public partial class DeclarationExportRecipientList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int? LineNumber  { get; set; }
       [DataMember]
       public string RecipientName  { get; set; }
       [DataMember]
       public string RecipientAddress  { get; set; }
       [DataMember]
       public string RecipientIssueCountryCode  { get; set; }
   }

}
	 