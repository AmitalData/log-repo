using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateSectionMap : EntityTypeConfiguration<QuoteTemplateSection>
    {
        public QuoteTemplateSectionMap()
        {
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.QuoteTemplateId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .HasMaxLength(500)
                .IsUnicode(true);










            this.Property(t => t.SectionDocId)
                .HasMaxLength(15)
                .IsUnicode(false);

      
            this.Property(t => t.Order)
                .IsRequired();

            this.Property(t => t.IsCancel)
                      .IsRequired();
           
           


            this.Property(t => t.Name)
                .IsRequired()
               .HasMaxLength(60)
                .IsUnicode(true);


            this.Property(t => t.QuoteTemplateSectionTypeCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);
            // Table & Column Mappings
            this.ToTable("QuoteTemplateSections");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId");
            this.Property(t => t.SectionDocId).HasColumnName("SectionDocId");
            this.Property(t => t.Order).HasColumnName("ColumnOrder");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.QuoteTemplateSectionTypeCode).HasColumnName("QuoteTemplateSectionTypeCode");
            this.Property(t => t.IsCancel).HasColumnName("IsCancel");
            this.Property(t => t.Description).HasColumnName("Description");


//#if ORACLE_DB
           string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
           if (dbms == "oracle")
           {
               this.Property(t => t.Order).HasColumnName("ColumnOrder");
           }
           //#else
           else
           {
               this.Property(t => t.Order).HasColumnName("Order");
           }
//#endif

            // Relationships
            this.HasRequired(t => t.QuoteTemplate)
                .WithMany()
                .HasForeignKey(d => d.QuoteTemplateId);

            this.HasOptional(t => t.SectionDoc)
                .WithMany()
                .HasForeignKey(d => d.SectionDocId);

            this.HasRequired(t => t.QuoteTemplateSectionType)
                .WithMany()
                .HasForeignKey(d => d.QuoteTemplateSectionTypeCode);

        }
    }
}
