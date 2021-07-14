using System;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public  class LogitudeOceanInsightsRequestMap : EntityTypeConfiguration<LogitudeOceanInsightsRequest>
    {
        public LogitudeOceanInsightsRequestMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.OceanInsigntId).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.BLNumber).HasMaxLength(18).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("LogitudeOceanInsightsRequests");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.OceanInsigntId).HasColumnName("OceanInsigntId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.BLNumber).HasColumnName("BLNumber");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
        }
    }
}
