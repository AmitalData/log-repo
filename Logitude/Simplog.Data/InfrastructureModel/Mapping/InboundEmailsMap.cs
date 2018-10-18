using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class InboundEmailsMap : EntityTypeConfiguration<InboundEmail>
    {
       public InboundEmailsMap()
       {
           this.HasKey(t => t.Id);
           this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.EntityId).HasMaxLength(40).IsUnicode(false);
           this.Property(t => t.Uniquekey).HasMaxLength(10).IsUnicode(false);
           this.Property(t => t.CreatedByContactId).HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.AnalyzeQueueId).HasMaxLength(15).IsUnicode(false);

           // Table & Column Mappings
           this.ToTable("InboundEmails");
           this.Property(t => t.Id).HasColumnName("Id");
           this.Property(t => t.Tenant).HasColumnName("Tenant");
           this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
           this.Property(t => t.EntityId).HasColumnName("EntityId");
           this.Property(t => t.CreateDate).HasColumnName("CreateDate");
           this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
           this.Property(t => t.Uniquekey).HasColumnName("Uniquekey");
           this.Property(t => t.CreatedByContactId).HasColumnName("CreatedByContactId");
           this.Property(t => t.IsRejected).HasColumnName("IsRejected");
           this.Property(t => t.AnalyzeQueueId).HasColumnName("AnalyzeQueueId");


           this.HasOptional(t => t.CreatedByContact).WithMany().HasForeignKey(d => d.CreatedByContactId);
       }
    }
}
