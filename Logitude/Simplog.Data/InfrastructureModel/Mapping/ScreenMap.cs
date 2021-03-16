using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ScreenMap : EntityTypeConfiguration<Screen>
    {
        public ScreenMap()
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
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(200)
                .IsUnicode(false);


            this.Property(t => t.QuerySection)
           .HasMaxLength(100)
           .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("Screens");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.NumberOfColumns).HasColumnName("NumberOfColumns");
            this.Property(t => t.NumberOfRows).HasColumnName("NumberOfRows");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsReadOnly).HasColumnName("IsReadOnly");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.QuerySection).HasColumnName("QuerySection");

            
            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.Screens)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
