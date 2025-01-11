using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Shipment.Domain.EntityPOCOs;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class NextLegMap : EntityTypeConfiguration<NextLeg>
    {
        public NextLegMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("NextLegs");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
