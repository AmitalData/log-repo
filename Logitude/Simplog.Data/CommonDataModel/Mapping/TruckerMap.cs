using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TruckerMap : EntityTypeConfiguration<Trucker>
    {
        public TruckerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PrimaryContactName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Truckers");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");
            this.Property(t => t.TransmitToPort).HasColumnName("TransmitToPort");

            // Relationships
            this.HasRequired(t => t.Card)
                .WithOptional(t => t.Trucker);

        }
    }
}
