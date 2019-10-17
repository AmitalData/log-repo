using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class CustomsTransferLineMap : EntityTypeConfiguration<CustomsTransferLine>
    {
        public CustomsTransferLineMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.CustomsTransferHeaderId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);

            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShipmentNumber)
                .IsOptional()
                .HasMaxLength(20)
                .IsUnicode(false);
            

            // Table & Column Mappings
            this.ToTable("CustomsTransferLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomsTransferHeaderId).HasColumnName("CustomsTransferHeaderId");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");

            // Relationships
            this.HasRequired(t => t.CustomsTransferHeader).WithMany().HasForeignKey(d => d.CustomsTransferHeaderId);
        }
    }
}