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
   public partial class SupplierInvioceItemCertificatList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceCounterKey  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }

       [Key]
       [DataMember]
       public int ItemCertificateCounterKey  { get; set; }
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
       [DataMember]
       public string ReqConfirmationTypeName  { get; set; }
       [DataMember]
       public string CertificateExemptionTypeName  { get; set; }
       [DataMember]
       public string AttachmentTypeName  { get; set; }
       [DataMember]
       public string ResConfirmationTypeName  { get; set; }
       [DataMember]
       public int SequenceNumeric  { get; set; }
       [DataMember]
       public string ExternalCertificatCode  { get; set; }
       [DataMember]
       public string ExternalRequestTypeCode  { get; set; }
       [DataMember]
       public string ApprovalRequestNumber  { get; set; }
   }

}
	 