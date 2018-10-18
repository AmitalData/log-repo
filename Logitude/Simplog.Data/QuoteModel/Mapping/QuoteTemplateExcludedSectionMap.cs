using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateExcludedSectionMap : EntityTypeConfiguration<QuoteTemplateExcludedSection>
    {

      public QuoteTemplateExcludedSectionMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
              .IsRequired()
              .HasMaxLength(15)
              .IsUnicode(false);

            // Properties
            this.Property(t => t.QuoteTemplateSectionId)
          
                .HasMaxLength(15)
                .IsUnicode(false);


           
            // Properties
            this.Property(t => t.QuoteId)
           
                .HasMaxLength(15)
                .IsUnicode(false);


           

            this.Property(t => t.Tenant)
                      .IsRequired();


            this.Property(t => t.QuoteTemplateId)
            .HasMaxLength(15)
            .IsUnicode(false);

          
            this.ToTable("QuoteTemplateExcludedSections");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QuoteTemplateSectionId).HasColumnName("QuoteTemplateSectionId");
            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
           

            // Relationships
            this.HasRequired(t => t.QuoteTemplateSection)
                .WithMany()
                .HasForeignKey(d => d.QuoteTemplateSectionId);

            this.HasOptional(t => t.QuoteTemplate)
                .WithMany()
                .HasForeignKey(d => d.QuoteTemplateId);

            this.HasOptional(t => t.Quote)
                .WithMany()
                .HasForeignKey(d => d.QuoteId);
            
        }


    }
}
