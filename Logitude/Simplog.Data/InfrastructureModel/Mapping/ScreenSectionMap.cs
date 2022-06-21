using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ScreenSectionMap : EntityTypeConfiguration<ScreenSection>
    {
        public ScreenSectionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.ScreenCode)
               .HasMaxLength(100)
               .IsUnicode(false);



            this.Property(t => t.CreateByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ScreenSections");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreateByUserId).HasColumnName("CreateByUserId");
            this.Property(t => t.ScreenCode).HasColumnName("ScreenCode");

            // Relationships
            this.HasOptional(t => t.CreateByUser)
              .WithMany()
              .HasForeignKey(d => d.CreateByUserId);


            this.HasOptional(t => t.Screen)
                .WithMany()
                .HasForeignKey(d => d.ScreenCode);
  


        }
    }
}
