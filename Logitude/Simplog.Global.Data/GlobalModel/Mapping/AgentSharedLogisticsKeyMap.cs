using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class AgentSharedLogisticsKeyMap : EntityTypeConfiguration<AgentSharedLogisticsKey>
    {
        public AgentSharedLogisticsKeyMap()
        {
            // Primary Key
            this.HasKey(t => t.SharedKey);

            // Properties
            this.Property(t => t.SharedKey)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserEmail)
                .IsRequired()
              .HasMaxLength(70)
              .IsUnicode(false);

            this.Property(t => t.StatusCode)
                  .IsRequired()
              .HasMaxLength(4)
              .IsUnicode(false);

            this.Property(t => t.InactiveByUserEmail)
              .HasMaxLength(70)
              .IsUnicode(false);

            this.Property(t => t.ApprovedByUserEmail)
                .HasMaxLength(70)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AgentSharedLogisticsKeys");

            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserEmail).HasColumnName("CreatedByUserEmail");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.Agent1Tenant).HasColumnName("Agent1Tenant");
            this.Property(t => t.Agent2Tenant).HasColumnName("Agent2Tenant");
            this.Property(t => t.ApproveDate).HasColumnName("ApproveDate");
            this.Property(t => t.ApprovedByUserEmail).HasColumnName("ApprovedByUserEmail");
            this.Property(t => t.InactiveDate).HasColumnName("InactiveDate");
            this.Property(t => t.InactiveByUserEmail).HasColumnName("InactiveByUserEmail");


        }
    }
}