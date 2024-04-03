using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class MentionMap : EntityTypeConfiguration<Mention>
    {
        public MentionMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Name)
                .HasMaxLength(70)
                .IsUnicode(true);

            this.Property(t => t.Description)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Mentions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.HasRequired(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId);

            this.HasRequired(t => t.UpdatedByUser)
               .WithMany()
               .HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
