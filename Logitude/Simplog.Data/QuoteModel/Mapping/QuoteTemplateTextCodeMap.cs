using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateTextCodeMap : EntityTypeConfiguration<QuoteTemplateTextCode>
    {
      public QuoteTemplateTextCodeMap()
      {
          this.HasKey(t => t.Id);
          // Properties
          this.Property(t => t.Id)
              .IsRequired()
              .HasMaxLength(15)
              .IsUnicode(false);

          this.Property(t => t.Tenant)
             
              .IsRequired();

          this.Property(t => t.TextCode)
               .IsRequired()
           .HasMaxLength(100)
           .IsUnicode(false);


          this.Property(t => t.EnglishName)
               .IsRequired()
           .HasMaxLength(100)
           .IsUnicode(false);

          this.Property(t => t.LocalName)
          .IsRequired()
          .HasMaxLength(100)
          .IsUnicode(true);

          this.Property(t => t.OriginalEnglishName)
          .IsRequired()
          .HasMaxLength(100)
          .IsUnicode(false);

            this.Property(t => t.OriginalLocalName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(true);



            this.Property(t => t.QuoteTemplateId)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);
          
          this.Property(t => t.Area)
               .IsRequired()
              .HasMaxLength(20)
              .IsUnicode(false);
         
          // Table & Column Mappings
          this.ToTable("QuoteTemplateTextCodes");
          this.Property(t => t.Id).HasColumnName("Id");
          this.Property(t => t.Tenant).HasColumnName("Tenant");
          this.Property(t => t.TextCode).HasColumnName("TextCode");
          this.Property(t => t.EnglishName).HasColumnName("EnglishName");
          this.Property(t => t.LocalName).HasColumnName("LocalName");

            this.Property(t => t.OriginalEnglishName).HasColumnName("OriginalEnglishName");
            this.Property(t => t.OriginalLocalName).HasColumnName("OriginalLocalName");

            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId");

          this.Property(t => t.Area).HasColumnName("Area");

          // Relationships
          this.HasRequired(t => t.QuoteTemplate)
              .WithMany()
              .HasForeignKey(d => d.QuoteTemplateId);     
  
      }
    }
}
