using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerSalesmanByProductMap : EntityTypeConfiguration<CustomerSalesmanByProduct>
    {

        public CustomerSalesmanByProductMap()
        {
            this.HasKey(d => new { d.ProductTypeCode,  d.CustomerId});

            this.Property(d => d.SalesmanUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(d => d.ProductTypeCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.CustomerId)
              .HasMaxLength(15)
              .IsUnicode(false);

             this.ToTable("CustomerSalesmanByProducts");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(d => d.ProductTypeCode).HasColumnName("ProductTypeCode");
        }
    }
}
