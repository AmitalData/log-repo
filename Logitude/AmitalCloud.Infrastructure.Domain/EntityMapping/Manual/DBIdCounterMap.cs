using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
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
                .HasMaxLength(50)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DBIdCounters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.LastIdNumber).HasColumnName("LastIdNumber");
        }
    }
}
