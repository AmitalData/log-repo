using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentCustomsTransmissionMap : EntityTypeConfiguration<ShipmentCustomsTransmission>
    {
        public ShipmentCustomsTransmissionMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SentByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CommunicationLogId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MessageCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Status).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Error).IsMaxLength().IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ShipmentCustomsTransmissions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LastSendDate).HasColumnName("LastSendDate");
            this.Property(t => t.SentByUserId).HasColumnName("SentByUserId");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");
            this.Property(t => t.Error).HasColumnName("Error");
            this.Property(t => t.MessageCode).HasColumnName("MessageCode");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relations 
            this.HasOptional(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasOptional(t => t.CommunicationLog).WithMany().HasForeignKey(d => d.CommunicationLogId);
            this.HasOptional(t => t.ShipmentCustomsMessageType).WithMany().HasForeignKey(d => d.MessageCode);
            this.HasOptional(t => t.CustomsTransmissionsStatus).WithMany().HasForeignKey(d => d.Status);
        }
    }
}
