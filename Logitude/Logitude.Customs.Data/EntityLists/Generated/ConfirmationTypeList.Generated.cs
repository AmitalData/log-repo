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
   public partial class ConfirmationTypeList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public int? MalamID  { get; set; }
       [DataMember]
       public int? State  { get; set; }
       [DataMember]
       public int? Exempt_CertificateDocument  { get; set; }
       [DataMember]
       public bool? IsImport  { get; set; }
       [DataMember]
       public bool? IsExemptOtherAuthority  { get; set; }
       [DataMember]
       public int? ConfirmationComputerization  { get; set; }
       [DataMember]
       public bool? IsCEO  { get; set; }
       [DataMember]
       public bool? IsNeedDeclaration  { get; set; }
       [DataMember]
       public int? CertificateDocumentCategory  { get; set; }
       [DataMember]
       public int? AuthorityID  { get; set; }
       [DataMember]
       public bool? IsQuotaCheckNeeded  { get; set; }
       [DataMember]
       public int? ExternalIDNumPerAuthority  { get; set; }
       [DataMember]
       public bool? IsForCustomsItem  { get; set; }
       [DataMember]
       public bool? IsPharmacy  { get; set; }
       [DataMember]
       public bool? IsVeterinarian  { get; set; }
       [DataMember]
       public bool? IsVehicleStandardization  { get; set; }
       [DataMember]
       public bool? IsQuantityMandatory  { get; set; }
       [DataMember]
       public bool? IsForCE  { get; set; }
   }

}
	 