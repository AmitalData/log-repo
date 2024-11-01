using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class DocumentTypeMetaDataMap : EntityTypeConfiguration<DocumentTypeMetaData>
    {
        public DocumentTypeMetaDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.DocumentsMetaDataTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);




            this.ToTable("DocumentTypeMetaDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentsMetaDataTypeId).HasColumnName("DocumentsMetaDataTypeId");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.Mandatory).HasColumnName("Mandatory");



            this.HasRequired(t => t.DocumentsMetaDataType)
             .WithMany()
             .HasForeignKey(d => d.DocumentsMetaDataTypeId);

            this.HasRequired(t => t.DocumentType)
            .WithMany()
            .HasForeignKey(d => d.DocumentTypeId);
        }
    }
}
