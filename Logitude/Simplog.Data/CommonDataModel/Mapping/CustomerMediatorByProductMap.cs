using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class CustomerMediatorByProductMap: EntityTypeConfiguration<CustomerMediatorByProduct>
    {

       public CustomerMediatorByProductMap()
        {
            this.HasKey(d => new { d.ProductTypeCode,  d.CustomerId});

            this.Property(d => d.MediatorId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(d => d.ProductTypeCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.CustomerId)
              .HasMaxLength(15)
              .IsUnicode(false);

            this.ToTable("CustomerMediatorByProducts");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.MediatorId).HasColumnName("MediatorId");
            this.Property(d => d.ProductTypeCode).HasColumnName("ProductTypeCode");
        }
    }
}
