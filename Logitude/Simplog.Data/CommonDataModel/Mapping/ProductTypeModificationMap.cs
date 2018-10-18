using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ProductTypeModificationMap : EntityTypeConfiguration<ProductTypeModification>
    {
        public ProductTypeModificationMap()
        {

            this.HasKey(d => new { d.ProductTypeCode, d.Tenant });


            this.Property(d => d.ProductTypeCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            this.ToTable("ProductTypeModifications");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.HasRequired(t => t.ProductType)
                .WithMany()
                .HasForeignKey(d => d.ProductTypeCode);
        }
    }
}
