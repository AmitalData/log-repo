using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DigitalContactLastSettingMap : EntityTypeConfiguration<DigitalContactLastSetting>
    {
        public DigitalContactLastSettingMap()
        {
            this.ToTable("DigitalContactLastSettings");

            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ContactId).HasColumnName("ContactId").IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FilterCode).HasColumnName("FilterCode").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.FilterName).HasColumnName("FilterName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.IsChecked).HasColumnName("IsChecked");

        }
    }
}
