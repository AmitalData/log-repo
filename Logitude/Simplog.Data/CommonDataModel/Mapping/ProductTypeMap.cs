using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ProductTypeMap : EntityTypeConfiguration<ProductType>
    {
        public ProductTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.QuotationDefaultTemplateId)
                 .HasMaxLength(15)
                 .IsUnicode(false);



            this.Property(t => t.RoutingRQuoteDefaultTemplateId)
                 .HasMaxLength(15)
                .IsUnicode(false);




            // Table & Column Mappings
            this.ToTable("ProductTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.QuotationDefaultTemplateId).HasColumnName("QuotationDefaultTemplateId");
            this.Property(t => t.RoutingRQuoteDefaultTemplateId).HasColumnName("RoutingRQuoteDefaultTemplateId");

            this.HasOptional(t => t.QuoteTemplate)
                .WithMany()
                .HasForeignKey(d => d.QuotationDefaultTemplateId);


            this.HasOptional(t => t.RoutingRQuoteDefaultTemplate)
                .WithMany()
                .HasForeignKey(d => d.RoutingRQuoteDefaultTemplateId);
        }
    }
}
