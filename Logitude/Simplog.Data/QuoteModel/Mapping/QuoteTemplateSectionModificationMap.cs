using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateSectionModificationMap : EntityTypeConfiguration<QuoteTemplateSectionModification>
    {
        public QuoteTemplateSectionModificationMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
              .IsRequired()
              .HasMaxLength(15)
              .IsUnicode(false);

            // Properties
            this.Property(t => t.QuoteTemplateSectionId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);



            // Properties
            this.Property(t => t.QuoteId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);




            this.Property(t => t.Tenant)
                      .IsRequired();


            this.Property(t => t.SectionDocId)
            .HasMaxLength(15)
            .IsUnicode(false);

//#if ORACLE_DB
   string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
   if (dbms == "oracle")
   {
       this.ToTable("TemplateSectionModifications");
   }
   //#else
   else
   {
       this.ToTable("QuoteTemplateSectionModifications");
   }

//#endif

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QuoteTemplateSectionId).HasColumnName("QuoteTemplateSectionId");
            this.Property(t => t.SectionDocId).HasColumnName("SectionDocId");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");


            // Relationships
            this.HasRequired(t => t.QuoteTemplateSection)
                .WithMany()
                .HasForeignKey(d => d.QuoteTemplateSectionId);

            this.HasOptional(t => t.SectionDoc)
                .WithMany()
                .HasForeignKey(d => d.SectionDocId);



        }
    }
}