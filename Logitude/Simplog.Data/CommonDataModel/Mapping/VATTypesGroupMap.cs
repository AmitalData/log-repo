using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class VATTypesGroupMap : EntityTypeConfiguration<VATTypesGroup>
    {
        public VATTypesGroupMap()
        {
            this.HasKey(d => new { d.GroupVATTypeId, d.SingleVATTypeId });
            this.Property(t => t.GroupVATTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SingleVATTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("VATTypesGroups");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.GroupVATTypeId).HasColumnName("GroupVATTypeId");
            this.Property(t => t.SingleVATTypeId).HasColumnName("SingleVATTypeId");

            this.HasRequired(t => t.GroupVATType).WithMany().HasForeignKey(d => d.GroupVATTypeId);
            this.HasRequired(t => t.SingleVATType).WithMany().HasForeignKey(d => d.SingleVATTypeId);
        }
    }
}
