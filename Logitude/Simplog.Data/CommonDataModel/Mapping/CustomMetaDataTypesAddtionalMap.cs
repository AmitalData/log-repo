using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomMetaDataTypesAddtionalMap : EntityTypeConfiguration<CustomMetaDataTypesAddtional>
    {

               // Primary Key
       public CustomMetaDataTypesAddtionalMap()
        {
            this.HasKey(t => new { t.Id, t.Code });    
     

           this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

           this.Property(t => t.Code)
               .IsRequired()
               .HasMaxLength(6)
               .IsUnicode(false);

           this.Property(t => t.DocumentsMetaDataTypesCode)
               .HasMaxLength(15)
               .IsUnicode(false);


           this.ToTable("CustomMetaDataTypesAddtionals");
           this.Property(t => t.Id).HasColumnName("Id");
           this.Property(t => t.Tenant).HasColumnName("Tenant");
           this.Property(t => t.Code).HasColumnName("Code");
           this.Property(t => t.DocumentsMetaDataTypesCode).HasColumnName("DocumentsMetaDataTypesCode");


           // Relationships
           //this.HasOptional(t => t.CustomMetaDataTypes)
           //        .WithMany()
           //        .HasForeignKey(d => d.Code);

           //this.HasOptional(t => t.DocumentsMetaDataType)
           //  .WithMany()
           //  .HasForeignKey(d => d.DocumentsMetaDataTypesCode);
       }


    }
}
