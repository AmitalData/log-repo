using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class CustomsTransferHeaderMap : EntityTypeConfiguration<CustomsTransferHeader>
    {
        public CustomsTransferHeaderMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.TransferNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.FileName).HasMaxLength(80).IsRequired().IsUnicode(true);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomsTransferTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            //this.Property(t => t.TransferDate).IsOptional();
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsOptional().IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsOptional().IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("CustomsTransferHeaders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TransferNumber).HasColumnName("TransferNumber");
            this.Property(t => t.CustomsTransferTypeCode).HasColumnName("CustomsTransferTypeCode");
            this.Property(t => t.TransferDate).HasColumnName("TransferDate");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");

            this.HasRequired(t => t.CustomsTransferType).WithMany().HasForeignKey(d => d.CustomsTransferTypeCode);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(t => t.CreatedByUserId);

        }
    }
}
