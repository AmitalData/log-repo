using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class INTTRABranchRegisteredCarrierMap : EntityTypeConfiguration<INTTRABranchRegisteredCarrier>
    {
        public INTTRABranchRegisteredCarrierMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShippingLineId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BranchId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("INTTRABranchRegisteredCarriers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.ShippingLineId).HasColumnName("ShippingLineId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.ShippingLine).WithMany().HasForeignKey(d => d.ShippingLineId);
            this.HasRequired(t => t.Branch).WithMany().HasForeignKey(d => d.BranchId);
        }
    }
}
