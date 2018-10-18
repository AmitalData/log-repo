using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DataBasePropertyMap : EntityTypeConfiguration<DataBaseProperty>
    {
        public DataBasePropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.DataBaseNumber);

            // Properties
            this.Property(t => t.DataBaseNumber)
                .HasDatabaseGeneratedOption(null);

            // Table & Column Mappings
            this.ToTable("DataBaseProperties");
            this.Property(t => t.DataBaseNumber).HasColumnName("DataBaseNumber");
            this.Property(t => t.LastBackupDate).HasColumnName("LastBackupDate");
        }
    }
}
