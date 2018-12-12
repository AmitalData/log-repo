using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class MessagingStockUsageHistoryMap : EntityTypeConfiguration<MessagingStockUsageHistory>
    {
        public MessagingStockUsageHistoryMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StockId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityNumber).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ActionType).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FirstActionByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LastActionByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MessageType).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MAWB).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.HAWB).HasMaxLength(20).IsUnicode(false);

            this.ToTable("MessagingStockUsageHistories");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StockId).HasColumnName("StockId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityNumber).HasColumnName("EntityNumber");
            this.Property(t => t.MessageType).HasColumnName("MessageType");
            this.Property(t => t.MAWB).HasColumnName("MAWB");
            this.Property(t => t.HAWB).HasColumnName("HAWB");
            this.Property(t => t.ActionType).HasColumnName("ActionType");
            this.Property(t => t.FirstActionDate).HasColumnName("FirstActionDate");
            this.Property(t => t.LastActionDate).HasColumnName("LastActionDate");
            this.Property(t => t.FirstActionByUserId).HasColumnName("FirstActionByUserId");
            this.Property(t => t.LastActionByUserId).HasColumnName("LastActionByUserId");

            this.HasRequired(t => t.Stock).WithMany().HasForeignKey(d => d.StockId);
            this.HasRequired(t => t.FirstActionByUser).WithMany().HasForeignKey(d => d.FirstActionByUserId);
            this.HasRequired(t => t.LastActionByUser).WithMany().HasForeignKey(d => d.LastActionByUserId);
        }
    }
}
