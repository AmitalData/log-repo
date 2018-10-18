using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerCompetitorMap : EntityTypeConfiguration<CustomerCompetitor>
    {
        public CustomerCompetitorMap()
        {
            // Primary Keys
            this.HasKey(d => new { d.CustomerId, d.CompetitorId });

            // Properties
            this.Property(t => t.CustomerId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CompetitorId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CustomerCompetitors");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CompetitorId).HasColumnName("CompetitorId");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId);

            this.HasRequired(t => t.Competitor)
                .WithMany()
                .HasForeignKey(d => d.CompetitorId);
        }
    }
}
