using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class CustomerCompetitorProductMap: EntityTypeConfiguration<CustomerCompetitorProduct>
    {
        public CustomerCompetitorProductMap()
        {
            // Primary Keys
            this.HasKey(t => new { t.CustomerId, t.CompetitorId, t.ProductTypeCode });

            // Properties
            this.Property(t => t.CustomerId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CompetitorId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ProductTypeCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CustomerCompetitorProducts");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CompetitorId).HasColumnName("CompetitorId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");

            // Relationships
            //this.HasOptional(t => t.ProductType)
            //    .WithMany()
            //    .HasForeignKey(d => d.ProductTypeCode);

            //this.HasOptional(t => t.Competitor)
            //    .WithMany()
            //    .HasForeignKey(d => d.CompetitorId);

            //this.HasOptional(t => t.Customer)
            //    .WithMany()
            //    .HasForeignKey(d => d.CustomerId);
        }
    }
}
