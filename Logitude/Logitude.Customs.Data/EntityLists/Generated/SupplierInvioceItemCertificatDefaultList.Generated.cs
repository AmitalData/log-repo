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
   public partial class SupplierInvioceItemCertificatDefaultList
   {
   
       [Key]
       [DataMember]
       public string SupplierInvioceExportDefaultId  { get; set; }
       [DataMember]
       public string CertificateNumber  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ReqConfirmationTypeCode  { get; set; }
       [DataMember]
       public string CertificateExemptionTypeCode  { get; set; }
       [DataMember]
       public string AttachmentTypeCode  { get; set; }
       [DataMember]
       public string ResConfirmationTypeCode  { get; set; }
       [DataMember]
       public string CustomsAttachmentID  { get; set; }

       [Key]
       [DataMember]
       public int SequenceNumeric  { get; set; }
   }

}
	 