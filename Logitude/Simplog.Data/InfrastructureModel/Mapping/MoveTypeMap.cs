using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class MoveTypeMap : EntityTypeConfiguration<MoveType>
    {
        public MoveTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.MoveTypeEnglishName)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(true);

            this.Property(t => t.MoveTypeLocalName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.TransportModeId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("MoveTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.MoveTypeEnglishName).HasColumnName("MoveTypeEnglishName");
            this.Property(t => t.MoveTypeLocalName).HasColumnName("MoveTypeLocalName");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            // Relationships
            //this.HasRequired(t => t.TransportMode)
            //    .WithMany(t => t.MoveTypes)
            //    .HasForeignKey(d => d.TransportModeId);

        }
    }
}
