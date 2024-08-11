using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CounterLastNumberMap : EntityTypeConfiguration<CounterLastNumber>
    {
        public CounterLastNumberMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("CounterLastNumbers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.LastNumber).HasColumnName("LastNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
