using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
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
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CounterLastNumbers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.LastNumber).HasColumnName("LastNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
