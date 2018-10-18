using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TipsVisibilityMap : EntityTypeConfiguration<TipsVisibility>
    {
        public TipsVisibilityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TipCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TipsVisibilities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TipCode).HasColumnName("TipCode");
            this.Property(t => t.IsVisible).HasColumnName("IsVisible");

            // Relationships
            //this.HasRequired(t => t.Tip)
            //    .WithMany(t => t.TipsVisibilities)
            //    .HasForeignKey(d => d.TipCode);
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

        }
    }
}
