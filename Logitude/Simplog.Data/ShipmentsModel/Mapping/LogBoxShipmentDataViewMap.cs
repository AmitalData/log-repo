using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.ShipmentModel.Mapping
{
    public class LogBoxShipmentDataViewMap : EntityTypeConfiguration<LogBoxShipmentDataView>
    {
        public LogBoxShipmentDataViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Tenant, t.ShipmentNumber, t.IsCancelled, t.IsOperationalClosed, t.DirectionId, t.TransportModeId, t.CreateDateTime });
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.ShipmentNumber).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomerReference1).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CustomerReference2).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DirectionId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.TransportModeId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ShipperName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.Shipper).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            // Table & Column Mappings
            this.ToTable("LogBoxShipmentDataView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");
            this.Property(t => t.CustomerReference1).HasColumnName("CustomerReference1");
            this.Property(t => t.CustomerReference2).HasColumnName("CustomerReference2");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.IsOperationalClosed).HasColumnName("IsOperationalClosed");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.ShipperName).HasColumnName("ShipperName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
