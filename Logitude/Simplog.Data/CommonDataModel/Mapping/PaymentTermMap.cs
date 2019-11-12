using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PaymentTermMap : EntityTypeConfiguration<PaymentTerm>
    {
        public PaymentTermMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Description).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.LocalDescription).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ExternalId).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.FromDateTypeCode).IsRequired().HasMaxLength(5).IsUnicode(true);
            this.Property(t => t.Code).HasMaxLength(4).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("PaymentTerms");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Days).HasColumnName("Days");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.DisplayInLOV).HasColumnName("DisplayInLOV");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LocalDescription).HasColumnName("LocalDescription");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");
            this.Property(t => t.IsManuallySet).HasColumnName("IsManuallySet");
            this.Property(t => t.CurrentMonth).HasColumnName("CurrentMonth");
            this.Property(t => t.FromDateTypeCode).HasColumnName("FromDateTypeCode");
            this.Property(t => t.Code).HasColumnName("Code");

            this.HasRequired(t => t.FromDateType).WithMany().HasForeignKey(d => d.FromDateTypeCode);
        }
    }
}
