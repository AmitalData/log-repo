using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteDocumentVersionMap : EntityTypeConfiguration<QuoteDocumentVersion>
    {

      public QuoteDocumentVersionMap()

               {
                    // Primary Key
                   this.HasKey(t => new { t.QuoteId, t.VersionNumber });
    
                    // Properties
                   this.Property(t => t.QuoteId)
                       .IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);


                   this.Property(t => t.VersionNumber)
                           .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


                   this.Property(t => t.CreatedByUserId)
                       .IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.UpdatedByUserId)
                       .HasMaxLength(15)
                       .IsUnicode(false);


                   this.Property(t => t.VersionType)
                       .HasMaxLength(2)
                       .IsUnicode(false);
          
                   this.Property(t => t.QuoteTemplateId)
                       .HasMaxLength(15)
                       .IsUnicode(false);
          
          
                   this.Property(t => t.DocumentId)
                       .IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.IsSent)
                             .IsRequired();

                   this.Property(t => t.CreateDate)
                          .IsRequired();

                   this.Property(t => t.UpdateDate);
                   this.Property(t => t.SendDate);

                   // Table & Column Mappings
                   this.ToTable("QuoteDocumentVersions");
                   this.Property(t => t.QuoteId).HasColumnName("QuoteId");
                   this.Property(t => t.VersionNumber).HasColumnName("VersionNumber");
                   this.Property(t => t.Tenant).HasColumnName("Tenant");
                   this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId");
          
                   this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
                   this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
                   this.Property(t => t.VersionType).HasColumnName("VersionType");
                   this.Property(t => t.DocumentId).HasColumnName("DocumentId");
                   this.Property(t => t.IsSent).HasColumnName("IsSent");

                   this.Property(t => t.CreateDate).HasColumnName("CreateDate");
                   this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

                   this.Property(t => t.SendDate).HasColumnName("SendDate");

                   // Relationships
                   this.HasRequired(t => t.CreatedByUser)
                       .WithMany()
                       .HasForeignKey(d => d.CreatedByUserId);

                   this.HasOptional(t => t.UpdatedByUser)
                       .WithMany()
                       .HasForeignKey(d => d.UpdatedByUserId);





                   this.HasOptional(t => t.QuoteTemplate)
                       .WithMany()
                       .HasForeignKey(d => d.QuoteTemplateId);
          


                   this.HasRequired(t => t.Quote)
                       .WithMany()
                       .HasForeignKey(d => d.QuoteId);

                   this.HasRequired(t => t.Doc)
                       .WithMany()
                       .HasForeignKey(d => d.DocumentId);
                }
    }
}
