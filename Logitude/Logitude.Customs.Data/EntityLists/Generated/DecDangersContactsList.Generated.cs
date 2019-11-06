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
   public partial class DecDangersContactList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string CompanyName  { get; set; }
       [DataMember]
       public string CompanyCommNumber  { get; set; }
       [DataMember]
       public string CompanyCommTypeCode  { get; set; }
       [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public string ContactCommNumber  { get; set; }
       [DataMember]
       public string ContactCommTypeCode  { get; set; }
       [DataMember]
       public string ContactId  { get; set; }
   }

}
	 