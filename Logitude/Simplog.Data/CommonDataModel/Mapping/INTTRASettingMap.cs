using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class INTTRASettingMap : EntityTypeConfiguration<INTTRASetting>
    {
        public INTTRASettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OutSettingsId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InSettingsId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.INTTRASettingModeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.INTTRAId).HasMaxLength(35).IsUnicode(false);
            this.Property(t => t.INTTRAAlias).HasMaxLength(35).IsUnicode(false);

            this.ToTable("INTTRASettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.OutSettingsId).HasColumnName("OutSettingsId");
            this.Property(t => t.InSettingsId).HasColumnName("InSettingsId");
            this.Property(t => t.INTTRASettingModeCode).HasColumnName("INTTRASettingModeCode");
            this.Property(t => t.INTTRAId).HasColumnName("INTTRAId");
            this.Property(t => t.INTTRAAlias).HasColumnName("INTTRAAlias");

            this.HasOptional(t => t.OutFTPDetail).WithMany().HasForeignKey(d => d.OutSettingsId);
            this.HasRequired(t => t.InFTPDetail).WithMany().HasForeignKey(d => d.InSettingsId);
            this.HasRequired(t => t.INTTRASettingMode).WithMany().HasForeignKey(d => d.INTTRASettingModeCode);
        }
    }
}
