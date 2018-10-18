using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{

    public class ProductTypeModificationViewMap : EntityTypeConfiguration<ProductTypeModificationView>
    {
        public ProductTypeModificationViewMap()
        {
            this.HasKey(d => d.Code);

            this.Property(d => d.Code)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(true);

            this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.ToTable("ProductTypeModificationView");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
           


        }

    }
    
}
