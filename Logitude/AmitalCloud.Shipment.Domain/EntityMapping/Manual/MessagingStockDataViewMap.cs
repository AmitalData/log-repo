using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Shipment.Domain.EntityPOCOs;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class MessagingStockDataViewMap : EntityTypeConfiguration<MessagingStockDataView>
    {
        public MessagingStockDataViewMap()
        {
            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TenantNumber).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.StockType).HasMaxLength(10).IsUnicode(false);

            this.ToTable("MessagingStockDataView");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TenantNumber).HasColumnName("TenantNumber");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Remaining).HasColumnName("Remaining");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.TenantName).HasColumnName("TenantName");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.StockType).HasColumnName("StockType");

        }
    }
}
