using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class SupplierInvioceItemCertificatDefault
    {
	 string dbms;

        [Key]
        [ForeignKey("SupplierInvioceExportDefault")]
        [Column("SupplierInvioceExportDefaultId" ,Order = 1)]
	    public string SupplierInvioceExportDefaultId { get; set; }
	      
        public virtual SupplierInvioceExportDefault SupplierInvioceExportDefault { get; set; }
        [Column("CertificateNumber")]
	    public string CertificateNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("RequestConfirmationType")]
        [Column("ReqConfirmationTypeCode")]
	    public string ReqConfirmationTypeCode { get; set; }
	      
        public virtual ConfirmationType RequestConfirmationType { get; set; }
        [ForeignKey("CertificateExemptionType")]
        [Column("CertificateExemptionTypeCode")]
	    public string CertificateExemptionTypeCode { get; set; }
	      
        public virtual CertificateExemptionType CertificateExemptionType { get; set; }
        [ForeignKey("AttachmentType")]
        [Column("AttachmentTypeCode")]
	    public string AttachmentTypeCode { get; set; }
	      
        public virtual AttachmentType AttachmentType { get; set; }
        [ForeignKey("ResponseConfirmationType")]
        [Column("ResConfirmationTypeCode")]
	    public string ResConfirmationTypeCode { get; set; }
	      
        public virtual ConfirmationType ResponseConfirmationType { get; set; }
        [Column("CustomsAttachmentID")]
	    public string CustomsAttachmentID { get; set; }
     [Key]
        [Column("SequenceNumeric")]
	    public int SequenceNumeric { get; set; }
    }
}
	 