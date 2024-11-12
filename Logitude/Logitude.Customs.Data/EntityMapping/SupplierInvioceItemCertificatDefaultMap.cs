using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class SupplierInvioceItemCertificatDefaultMap : EntityTypeConfiguration<SupplierInvioceItemCertificatDefault>
    {
	    string dbms;
        public SupplierInvioceItemCertificatDefaultMap()
        { 
		
     dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
    if (dbms == "oracle")
    {
	  this.ToTable("SupplierInvioceItemCertificatD", "Customs");
	}
    else
    {
	  this.ToTable("SupplierInvioceItemCertificatDefaults", "Customs");
	}

		
		    this.HasKey(t => new { t.SupplierInvioceExportDefaultId, t.SequenceNumeric });
	 
            this.Property(t => t.SupplierInvioceExportDefaultId).HasColumnName("SupplierInvioceExportDefaultId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CertificateNumber).HasColumnName("CertificateNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ReqConfirmationTypeCode).HasColumnName("ReqConfirmationTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CertificateExemptionTypeCode).HasColumnName("CertificateExemptionTypeCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.AttachmentTypeCode).HasColumnName("AttachmentTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ResConfirmationTypeCode).HasColumnName("ResConfirmationTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomsAttachmentID).HasColumnName("CustomsAttachmentID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric").IsRequired().HasDatabaseGeneratedOption(null);
        }
    }
}
	 