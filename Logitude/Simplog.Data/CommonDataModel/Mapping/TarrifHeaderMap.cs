using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TarrifHeaderMap : EntityTypeConfiguration<TarrifHeader>
    {
        public TarrifHeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CardId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TarrifTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.TransitTimeNotes)
                .HasMaxLength(250)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("TarrifHeaders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.TarrifTypeCode).HasColumnName("TarrifTypeCode");
            this.Property(t => t.ToDate).HasColumnName("ToDate");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.TransitTimeNotes).HasColumnName("TransitTimeNotes");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            this.HasRequired(t => t.Card)
                .WithMany()
                .HasForeignKey(d => d.CardId);
            this.HasRequired(t => t.TarrifType)
                .WithMany()
                .HasForeignKey(d => d.TarrifTypeCode);

        }
    }
}
