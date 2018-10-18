using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerProductMap : EntityTypeConfiguration<CustomerProduct>
    {
        public CustomerProductMap()
        {
            // Primary Key
            this.HasKey(d => new { d.CustomerId, d.ProductTypeCode });

            // Properties
            this.Property(t => t.CustomerId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ProductTypeCode)
               .IsRequired()
               .HasMaxLength(2)
               .IsUnicode(false);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.PrepaidCollectId)
                .HasMaxLength(1)
                .IsUnicode(false);
                        
            // Table & Column Mappings
            this.ToTable("CustomerProducts");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.PotentialChargeableWeight).HasColumnName("PotentialChargeableWeight");
            this.Property(t => t.CommitmentChargeableWeight).HasColumnName("CommitmentChargeableWeight");
            this.Property(t => t.PotentialTEU).HasColumnName("PotentialTEU");
            this.Property(t => t.CommitmentTEU).HasColumnName("CommitmentTEU");
            this.Property(t => t.PotentialNumberOfShipments).HasColumnName("PotentialNumberOfShipments");
            this.Property(t => t.CommitmentNumberOfShipments).HasColumnName("CommitmentNumberOfShipments");
            this.Property(t => t.PotentialRevenue).HasColumnName("PotentialRevenue");
            this.Property(t => t.CommitmentRevenue).HasColumnName("CommitmentRevenue");
            this.Property(t => t.LastShipmentDate).HasColumnName("LastShipmentDate");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.NotesRightToLeft).HasColumnName("NotesRightToLeft");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId);

            this.HasRequired(t => t.ProductType)
                .WithMany()
                .HasForeignKey(d => d.ProductTypeCode);

            this.HasOptional(t => t.PrepaidCollect)
                .WithMany()
                .HasForeignKey(d => d.PrepaidCollectId);
        }
    }
}
