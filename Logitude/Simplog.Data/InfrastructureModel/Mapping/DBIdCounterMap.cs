using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DBIdCounterMap : EntityTypeConfiguration<DBIdCounter>
    {
        public DBIdCounterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("DBIdCounters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.LastIdNumber).HasColumnName("LastIdNumber");
        }
    }
}
