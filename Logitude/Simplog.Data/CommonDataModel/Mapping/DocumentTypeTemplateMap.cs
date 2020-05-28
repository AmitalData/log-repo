using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentTypeTemplateMap : EntityTypeConfiguration<DocumentTypeTemplate>
    {
        public DocumentTypeTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TemplateType)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.LastUpdatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EditorTool)
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.Subject)
               .HasMaxLength(500)
               .IsUnicode(true);

            this.Property(t => t.TemplateTechnologyCode)
            .HasMaxLength(2)
            .IsUnicode(false);



            this.Property(t => t.CountryCode).IsFixedLength().IsUnicode(false).HasMaxLength(2);

            this.Property(t => t.InActive)
           .IsRequired();

            this.Property(t => t.IsCopiedAtSignup)
                .IsRequired();

            this.Property(t => t.IsEnabledForCustomers)
                .IsRequired();



            this.Property(t => t.InternalRemarks)
               .HasMaxLength(500)
               .IsUnicode(true);

            this.Property(t => t.Language)
               .HasMaxLength(100)
               .IsUnicode(true);

            this.Property(t => t.OriginalTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.From)
           .HasMaxLength(500)
           .IsUnicode(true);

            this.Property(t => t.ReplyTo)
           .HasMaxLength(500)
           .IsUnicode(true);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            if (dbms == "oracle")
            {
                this.Property(t => t.CC)
                .HasMaxLength(500)
                .IsUnicode(true);

                this.Property(t => t.BCC)
             .HasMaxLength(500)
             .IsUnicode(true);

            }
            else
            {
                this.Property(t => t.CC)
                .HasMaxLength(4000)
                .IsUnicode(true);
                this.Property(t => t.BCC)
             .HasMaxLength(4000)
             .IsUnicode(true);

            }
            //OriginalTemplateId 

            // Table & Column Mappings
            this.ToTable("DocumentTypeTemplates");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.TemplateBody).HasColumnName("TemplateBody");
            this.Property(t => t.TemplateBodyHtml).HasColumnName("TemplateBodyHtml");
            this.Property(t => t.TemplateBodyjson).HasColumnName("TemplateBodyjson");

            this.Property(t => t.TemplateFooterHtml).HasColumnName("TemplateFooterHtml");
            this.Property(t => t.TemplateHeaderHtml).HasColumnName("TemplateHeaderHtml");

            this.Property(t => t.TemplateType).HasColumnName("TemplateType");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.LastUpdatedByUserId).HasColumnName("LastUpdatedByUserId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.EditorTool).HasColumnName("EditorTool");
            this.Property(t => t.VerticalShift).HasColumnName("VerticalShift");
            this.Property(t => t.HorizontalShift).HasColumnName("HorizontalShift");
            this.Property(t => t.Subject).HasColumnName("Subject");

            this.Property(t => t.IsCopiedAtSignup).HasColumnName("IsCopiedAtSignup");
            this.Property(t => t.IsEnabledForCustomers).HasColumnName("IsEnabledForCustomers");
            this.Property(t => t.CountryCode).HasColumnName("CountryCode");


            this.Property(t => t.InternalRemarks).HasColumnName("InternalRemarks");
            this.Property(t => t.Language).HasColumnName("Language");
            this.Property(t => t.OriginalTemplateId).HasColumnName("OriginalTemplateId");


            this.Property(t => t.TemplateHeaderHeight).HasColumnName("TemplateHeaderHeight");
            this.Property(t => t.TemplateFooterHeight).HasColumnName("TemplateFooterHeight");
            this.Property(t => t.TemplateTechnologyCode).HasColumnName("TemplateTechnologyCode");


            this.Property(t => t.ReplyTo).HasColumnName("ReplyTo");
            this.Property(t => t.CC).HasColumnName("CC");
            this.Property(t => t.BCC).HasColumnName("BCC");


            //string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            if (dbms == "oracle")
            {
                this.Property(t => t.From).HasColumnName("From1");
                

            }
            else
            {
                this.Property(t => t.From).HasColumnName("From");
                

            }


            // Relationships
            this.HasRequired(t => t.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId);
            this.HasOptional(t => t.LastUpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.LastUpdatedByUserId);



            this.HasOptional(t => t.OriginalTemplate)
                .WithMany()
                .HasForeignKey(d => d.OriginalTemplateId);

        }
    }
}
