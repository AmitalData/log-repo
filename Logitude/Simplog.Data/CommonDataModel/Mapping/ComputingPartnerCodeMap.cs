using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ComputingPartnerCodeMap : EntityTypeConfiguration<ComputingPartnerCode>
    {
        public ComputingPartnerCodeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ComputingPartnerId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PartnerCode).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.PartnerName).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("ComputingPartnerCodes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.ComputingPartnerId).HasColumnName("ComputingPartnerId");
            this.Property(t => t.PartnerCode).HasColumnName("PartnerCode");
            this.Property(t => t.PartnerName).HasColumnName("PartnerName");
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
