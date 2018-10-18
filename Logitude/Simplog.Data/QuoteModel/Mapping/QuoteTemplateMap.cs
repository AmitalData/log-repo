using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateMap : EntityTypeConfiguration<QuoteTemplate>
    {
   

               public QuoteTemplateMap()

               {
                    // Primary Key

                   this.HasKey(t => t.Id);
                    // Properties
                   this.Property(t => t.Id)
                       .IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.Tenant)
                       .IsRequired();
              
                   this.Property(t => t.SearchFields)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
             

                   this.Property(t => t.FooterDocId)
                       //.IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.HeaderDocId)
                       //.IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.QuoteTemplateSettingId)
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.Name)
                       .IsRequired()
                       .HasMaxLength(60)
                       .IsUnicode(true);

                   this.Property(t => t.IsTemplate)
                       .IsRequired();

                   this.Property(t => t.IsDefault)
                       .IsRequired();
                   
                   this.Property(t => t.OriginalQuoteTemplateId)
                        .HasMaxLength(15)
                        .IsUnicode(false);

                   this.Property(t => t.CreateDate)
                       .IsRequired();

                   this.Property(t => t.UpdateDate);
                   
                   this.Property(t => t.CreatedByUserId)
                       .IsRequired()
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.UpdatedByUserId)
                       .HasMaxLength(15)
                       .IsUnicode(false);

                   this.Property(t => t.TemplateTypeCode)
                           .IsRequired()
                            .HasMaxLength(1)
                            .IsUnicode(false);

                   this.Property(t => t.InActive)
                      .IsRequired();


            // Table & Column Mappings
            this.ToTable("QuoteTemplates");
                   this.Property(t => t.Id).HasColumnName("Id");
                   this.Property(t => t.Tenant).HasColumnName("Tenant");
                   this.Property(t => t.FooterDocId).HasColumnName("FooterDocId");
                   this.Property(t => t.HeaderDocId).HasColumnName("HeaderDocId");
                   this.Property(t => t.QuoteTemplateSettingId).HasColumnName("QuoteTemplateSettingId");
                   this.Property(t => t.Name).HasColumnName("SearchFields");
                   this.Property(t => t.Name).HasColumnName("Name");
                   this.Property(t => t.IsTemplate).HasColumnName("IsTemplate");
                   this.Property(t => t.OriginalQuoteTemplateId).HasColumnName("OriginalQuoteTemplateId");
                   this.Property(t => t.CreateDate).HasColumnName("CreateDate");
                   this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
                   this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
                   this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

                   this.Property(t => t.TemplateTypeCode).HasColumnName("TemplateTypeCode");
                   this.Property(t => t.IsDefault).HasColumnName("IsDefault");
                   this.Property(t => t.InActive).HasColumnName("InActive");
                   this.Property(t => t.IsEnabledForCustomers).HasColumnName("IsEnabledForCustomers");
                   this.Property(t => t.IsCopiedAtSignup).HasColumnName("IsCopiedAtSignup");
              // Relationships
            this.HasRequired(t => t.CreatedByUser)
                       .WithMany()
                       .HasForeignKey(d => d.CreatedByUserId);

                   this.HasOptional(t => t.UpdatedByUser)
                       .WithMany()
                       .HasForeignKey(d => d.UpdatedByUserId);

                   this.HasOptional(t => t.OriginalQuoteTemplate)
                       .WithMany()
                       .HasForeignKey(d => d.OriginalQuoteTemplateId);



                   this.HasRequired(t => t.QuoteType)
                       .WithMany()
                       .HasForeignKey(d => d.TemplateTypeCode);
               }

 

        

    }
}
