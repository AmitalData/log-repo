using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentFolderMap : EntityTypeConfiguration<DocumentFolder>
    {
        public DocumentFolderMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.LocalName)
                .HasMaxLength(40)
                .IsUnicode(true);

            

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Code)

            .HasMaxLength(5)
            .IsUnicode(false);

            this.ToTable("DocumentFolders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Code).HasColumnName("Code");


            this.HasOptional(t => t.ParentFolder).WithMany().HasForeignKey(d => d.ParentFolderId);


        }
    }
}
