using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ComputingPartnerTableMap : EntityTypeConfiguration<ComputingPartnerTable>
    {
        public ComputingPartnerTableMap()
        {
            this.HasKey(t => new { t.ObjectTableId, t.ComputingPartnerId ,t.Tenant});
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ComputingPartnerId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("ComputingPartnerTables");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.ComputingPartnerId).HasColumnName("ComputingPartnerId");
            this.Property(t => t.HasPartnerList).HasColumnName("HasPartnerList");
            this.Property(t => t.MustUsePartnerList).HasColumnName("MustUsePartnerList");
            this.Property(t => t.TransalationRequired).HasColumnName("TransalationRequired");
            this.Property(t => t.TenantLevelTranslationBlocked).HasColumnName("TenantLevelTranslationBlocked");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            this.HasRequired(t => t.ObjectTable).WithMany().HasForeignKey(d => d.ObjectTableId);
            this.HasRequired(t => t.ComputingPartner).WithMany().HasForeignKey(d => d.ComputingPartnerId);
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
