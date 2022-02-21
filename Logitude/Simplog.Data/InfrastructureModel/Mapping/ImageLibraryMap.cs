using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ImageLibraryMap : EntityTypeConfiguration<ImageLibrary>
    {

        public ImageLibraryMap()
        {

            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(200)
                .IsUnicode(true);

            this.Property(t => t.ImageDetailId)
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

            this.Property(t => t.SecurityId)
                .HasMaxLength(60)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ImageLibraries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ImageDetailId).HasColumnName("ImageDetailId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");

            this.HasRequired(t => t.ImageDetail)
                .WithMany()
                .HasForeignKey(d => d.ImageDetailId);

            this.HasRequired(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId);

            this.HasRequired(t => t.UpdatedByUser)
               .WithMany()
               .HasForeignKey(d => d.UpdatedByUserId);

        }

    }
}
