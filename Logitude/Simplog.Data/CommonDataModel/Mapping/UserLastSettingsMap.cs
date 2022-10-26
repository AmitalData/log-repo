using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserLastSettingsMap : EntityTypeConfiguration<UserLastSettings>
    {
        public UserLastSettingsMap()
        {
            this.ToTable("UserLastSettings");

            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ControlNameSpace).HasColumnName("ControlNameSpace").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.FilterName).HasColumnName("FilterName").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.FilterValue).HasColumnName("FilterValue").HasMaxLength(50).IsUnicode(false);
        }
    }
}
