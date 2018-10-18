using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class OceanInsightsStatusesMap : EntityTypeConfiguration<OceanInsightsStatuses>
    {
        public OceanInsightsStatusesMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CommunicationLogId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContentDocumentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OceanInsightsRequestId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("OceanInsightsStatuses");
            this.Property(t => t.Id).HasColumnName("Id"); 
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");
            this.Property(t => t.ContentDocumentId).HasColumnName("ContentDocumentId");
            this.Property(t => t.OceanInsightsRequestId).HasColumnName("OceanInsightsRequestId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.HasRequired(t => t.CommunicationLog)
             .WithMany()
             .HasForeignKey(d => d.CommunicationLogId)
             .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.ContentDocumentId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.OceanInsightsRequest)
               .WithMany()
               .HasForeignKey(d => d.OceanInsightsRequestId)
               .WillCascadeOnDelete(false);
        
             

        }
    }
}
