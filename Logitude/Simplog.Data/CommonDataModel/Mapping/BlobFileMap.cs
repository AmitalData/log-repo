using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class BlobFileMap : EntityTypeConfiguration<BlobFile>
    {
        public BlobFileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);



            this.ToTable("BlobFiles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Blob).HasColumnName("Blob");
        }
    }
}
