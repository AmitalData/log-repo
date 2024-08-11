using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentsMetaDataTypeMap : EntityTypeConfiguration<DocumentsMetaDataType>
    {
        public DocumentsMetaDataTypeMap()
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
                .HasMaxLength(5)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(true);

            this.Property(t => t.LocalName)
               .HasMaxLength(60)
               .IsUnicode(true);

            this.Property(t => t.CustomsMetaDataCode)
                .HasMaxLength(6)
                .IsUnicode(true);


            this.Property(t => t.Format)
                 .IsRequired()
                 .HasMaxLength(20)
                 .IsUnicode(false);

            this.Property(t => t.InActive)
                .IsRequired();



            // Table & Column Mappings
            this.ToTable("DocumentsMetaDataTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.CustomsMetaDataCode).HasColumnName("CustomsMetaDataCode");
            this.Property(t => t.Format).HasColumnName("Format");
            this.Property(t => t.InActive).HasColumnName("InActive");

        }
    }
}
