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


            this.Property(d => d.RoutingRQuoteDefaultTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(d => d.QuotationDefaultTemplateId)
     .HasMaxLength(15)
     .IsUnicode(false);

            this.ToTable("ProductTypeModifications");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.RoutingRQuoteDefaultTemplateId).HasColumnName("RoutingRQuoteDefaultTemplateId");

            this.Property(t => t.QuotationDefaultTemplateId).HasColumnName("QuotationDefaultTemplateId");
            this.Property(t => t.CostTariffUse).HasColumnName("CostTariffUse");
            this.Property(t => t.SaleTariffUse).HasColumnName("SaleTariffUse");


            this.HasRequired(t => t.ProductType)
                .WithMany()
                .HasForeignKey(d => d.ProductTypeCode);
        }
    }
}
