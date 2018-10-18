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
 
    public class CustomsDocumentsTicketMap : EntityTypeConfiguration<CustomsDocumentsTicket>
    {
	    string dbms;
        public CustomsDocumentsTicketMap()
        { 
			  this.ToTable("CustomsDocumentsTickets", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DocumentsFilingId).HasColumnName("DocumentsFilingId").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.DocumentTypeCode).HasColumnName("DocumentTypeCode").IsRequired().HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.RequestedCustomsDocId).HasColumnName("RequestedCustomsDocId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Remarks).HasColumnName("Remarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.UploadApproved).HasColumnName("UploadApproved");

            this.Property(t => t.VerificationStatusTypeCode).HasColumnName("VerificationStatusTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.VerificationRemarks).HasColumnName("VerificationRemarks").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.UserRemarks).HasColumnName("UserRemarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.IsSendMandatory).HasColumnName("IsSendMandatory");
        }
    }
}
	 