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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPDocumentVersionMap : EntityTypeConfiguration<QuoteOPDocumentVersion>
    {
	    string dbms;
        public QuoteOPDocumentVersionMap()
        { 
				this.ToTable("QuoteOPDocumentVersions");
		
		    this.HasKey(t => new { t.QuoteOPId, t.VersionNumber });
	 
            this.Property(t => t.QuoteOPId).HasColumnName("QuoteOPId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VersionNumber).HasColumnName("VersionNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VersionType).HasColumnName("VersionType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DocumentId).HasColumnName("DocumentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsSent).HasColumnName("IsSent").IsRequired();

            this.Property(t => t.QuoteOPTemplateId).HasColumnName("QuoteOPTemplateId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 