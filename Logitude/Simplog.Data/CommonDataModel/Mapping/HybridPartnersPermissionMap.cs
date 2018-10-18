using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class HybridPartnersPermissionMap : EntityTypeConfiguration<HybridPartnersPermission>
    {
        public HybridPartnersPermissionMap()
        {
            this.HasKey(t => new { t.HybridPartnerId, t.AllowedByHybridPartnerId });


            this.Property(t => t.HybridPartnerId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.AllowedByHybridPartnerId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.InActive)
            .IsRequired();
            
            // Table & Column Mappings
            this.ToTable("HybridPartnersPermissions");
            this.Property(t => t.HybridPartnerId).HasColumnName("HybridPartnerId");
            this.Property(t => t.AllowedByHybridPartnerId).HasColumnName("AllowedByHybridPartnerId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            
            //relationships
            this.HasRequired(t => t.HybridPartner).WithMany().HasForeignKey(d => d.HybridPartnerId);
            this.HasRequired(t => t.HybridPartner).WithMany().HasForeignKey(d => d.AllowedByHybridPartnerId);
        }
    }
}
