using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentsFilingMetaDataValueMap : EntityTypeConfiguration<DocumentsFilingMetaDataValue>
    {
        public DocumentsFilingMetaDataValueMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.DocumentsFilingId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentsMetaDataTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MetaDataValue)
               .HasMaxLength(128)
               .IsUnicode(true);



            this.ToTable("DocumentsFilingMetaDataValues");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentsFilingId).HasColumnName("DocumentsFilingId");
            this.Property(t => t.DocumentsMetaDataTypeId).HasColumnName("DocumentsMetaDataTypeId");
            this.Property(t => t.MetaDataValue).HasColumnName("MetaDataValue");



            this.HasRequired(t => t.DocumentsFiling)
             .WithMany()
             .HasForeignKey(d => d.DocumentsFilingId);


            this.HasRequired(t => t.DocumentsMetaDataType)
            .WithMany()
            .HasForeignKey(d => d.DocumentsMetaDataTypeId);
        }
    }
}
