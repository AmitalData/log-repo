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
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .HasMaxLength(200)
                .IsUnicode(true);

            this.Property(t => t.DocumentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ImageLibrarys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.Description).HasColumnName("Description");

              this.HasRequired(t => t.Document)
              .WithMany()
              .HasForeignKey(d => d.DocumentId);

       }

    }
}
