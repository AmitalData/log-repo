using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerProductActualDataMap : EntityTypeConfiguration<CustomerProductActualData>
    {
        public CustomerProductActualDataMap()
        {
            this.HasKey(d => new { d.CustomerId, d.ProductTypeCode, d.Month, d.Year });
            this.Property(t => t.CustomerId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductTypeCode).IsRequired().HasMaxLength(2).IsUnicode(false);
                                    
            this.ToTable("CustomerProductActualDatas");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TEU).HasColumnName("TEU");
            this.Property(t => t.NumberOfShipments).HasColumnName("NumberOfShipments");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.Revenue).HasColumnName("Revenue");

            this.HasRequired(t => t.Customer).WithMany().HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.ProductType).WithMany().HasForeignKey(d => d.ProductTypeCode);
        }
    }
}
