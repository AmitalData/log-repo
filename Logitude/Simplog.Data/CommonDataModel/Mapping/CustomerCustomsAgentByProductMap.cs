using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class CustomerCustomsAgentByProductMap: EntityTypeConfiguration<CustomerCustomsAgentByProduct>
    {

       public CustomerCustomsAgentByProductMap()
        {
            this.HasKey(d => new { d.ProductTypeCode,  d.CustomerId});

            this.Property(d => d.CustomsAgentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(d => d.ProductTypeCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.CustomerId)
              .HasMaxLength(15)
              .IsUnicode(false);

            this.ToTable("CustomerCustomsAgentByProducts");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId");
            this.Property(d => d.ProductTypeCode).HasColumnName("ProductTypeCode");

            //this.HasOptional(d => d.CustomsAgentCard)
            //    .WithMany()
            //    .HasForeignKey(d => d.CustomsAgentId);
        }
    }
    
    
}
