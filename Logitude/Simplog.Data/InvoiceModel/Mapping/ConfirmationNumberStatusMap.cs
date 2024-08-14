using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ConfirmationNumberStatusMap : EntityTypeConfiguration<ConfirmationNumberStatus>
    {
        public ConfirmationNumberStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(true);

            this.Property(t => t.LocalName)
              .HasMaxLength(60)
              .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.InActive)
               .IsRequired();
            

            // Table & Column Mappings
            this.ToTable("ConfirmationNumberStatuses");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
        }
    }
}
