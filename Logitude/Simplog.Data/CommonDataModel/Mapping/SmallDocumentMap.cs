using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class SmallDocumentMap: EntityTypeConfiguration<SmallDocument>
    {
        public SmallDocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)//GUID
                .IsUnicode(false);

            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(8000)
                .IsUnicode(true);

           

           


            // Table & Column Mappings
            this.ToTable("SmallDocuments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Content).HasColumnName("Content");
            
           

           

        }
    }
}
