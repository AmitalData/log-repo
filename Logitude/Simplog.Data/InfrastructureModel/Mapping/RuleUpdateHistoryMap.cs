using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class RuleUpdateHistoryMap : EntityTypeConfiguration<RuleUpdateHistory>
    {
        public RuleUpdateHistoryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EventName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.RuleCode).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("RuleUpdateHistories");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EventName).HasColumnName("EventName");
            this.Property(t => t.RuleCode).HasColumnName("RuleCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");


            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
