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
   public partial class CertificateOfOriginMandatoryFieldsList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string MappedCertificatOriginId  { get; set; }
       [DataMember]
       public bool IsMandatory  { get; set; }
       [DataMember]
       public int? Location  { get; set; }
       [DataMember]
       public DateTime? LastUpdatedDate  { get; set; }
   }

}
	 