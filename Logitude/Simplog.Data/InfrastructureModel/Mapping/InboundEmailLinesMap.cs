using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class InboundEmailLinesMap : EntityTypeConfiguration<InboundEmailLine>
    {
        public InboundEmailLinesMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.InboundEmailId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CommunicationLogId).HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Sender).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.Subject).HasMaxLength(256).IsUnicode(true);
            this.Property(t => t.Recepient).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.Body).IsMaxLength().IsUnicode(true);
            this.Property(t => t.FullBody).IsMaxLength().IsUnicode(true);
            this.Property(t => t.HTMLFullBody).IsMaxLength().IsUnicode(true);

            this.Property(t => t.CCs).HasMaxLength(4000).IsUnicode(false);
            this.Property(t => t.Bcc).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.InternalUsers).HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.Direction).HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.EntityLineId).IsRequired().HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("InboundEmailLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.Recepient).HasColumnName("Recepient");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.FullBody).HasColumnName("FullBody");
            this.Property(t => t.HTMLFullBody).HasColumnName("HTMLFullBody");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CCs).HasColumnName("CCs");

            this.Property(t => t.Bcc).HasColumnName("Bcc");

            this.Property(t => t.InternalUsers).HasColumnName("InternalUsers");

            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.InboundEmailId).HasColumnName("InboundEmailId");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");

            this.Property(t => t.EntityLineId).HasColumnName("EntityLineId");

            // Relations 
            this.HasRequired(t => t.InboundEmail).WithMany().HasForeignKey(d => d.InboundEmailId);
            this.HasOptional(t => t.CommunicationLog).WithMany().HasForeignKey(d => d.CommunicationLogId);
        }
    }
}
