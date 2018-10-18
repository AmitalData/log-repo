using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CustomTableMap : EntityTypeConfiguration<CustomTable>
    {
        public CustomTableMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Field1)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.Field2)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field3)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field4)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field5)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field6)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field7)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field8)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field9)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field10)
                .HasMaxLength(250)
                 .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("CustomTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");

            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.CustomTables)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
