using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ComputingPartnerMap : EntityTypeConfiguration<ComputingPartner>
    {
        public ComputingPartnerMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Remarks).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(250).IsUnicode(true);

            this.ToTable("ComputingPartners");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
