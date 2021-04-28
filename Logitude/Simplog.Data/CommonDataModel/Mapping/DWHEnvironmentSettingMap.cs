using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DWHEnvironmentSettingMap : EntityTypeConfiguration<DWHEnvironmentSetting>
    {
        public DWHEnvironmentSettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(null);
            this.Property(t => t.FactCodes).HasMaxLength(200).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("DWHEnvironmentSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FactCodes).HasColumnName("FactCodes");


        }
    }
}
