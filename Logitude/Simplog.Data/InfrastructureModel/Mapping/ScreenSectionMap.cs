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
            this.HasKey(t => new { t.Id});
            this.Property(t => t.Id)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Type)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.RelatedScreenCode)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.ScreenCode)
               .HasMaxLength(100)
               .IsUnicode(false);



            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ScreenSections");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.ScreenCode).HasColumnName("ScreenCode");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.RelatedScreenCode).HasColumnName("RelatedScreenCode");
            this.Property(t => t.NumberOfRows).HasColumnName("NumberOfRows");
            this.Property(t => t.Number).HasColumnName("Number");

            // Relationships
            this.HasOptional(t => t.CreatedByUser)
              .WithMany()
              .HasForeignKey(d => d.CreatedByUserId);


            //this.HasOptional(t => t.Screen)
            //    .WithMany()
            //    .HasForeignKey(d => d.ScreenCode);
  


        }
    }
}
