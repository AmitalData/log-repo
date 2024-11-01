using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class OceanInsightsRequestMap : EntityTypeConfiguration<OceanInsightsRequest>
    {
        public OceanInsightsRequestMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SCACCode).IsRequired().HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.OceanInsigntId).HasMaxLength(500).IsUnicode(false); 
            this.Property(t => t.Type).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BLNumber).HasMaxLength(18).IsUnicode(false);
            this.Property(t => t.FromPushPage).IsRequired();

            this.ToTable("OceanInsightsRequests");
            this.Property(t => t.Id).HasColumnName("Id"); 
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SCACCode).HasColumnName("SCACCode");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.OceanInsigntId).HasColumnName("OceanInsigntId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.BLNumber).HasColumnName("BLNumber");
            this.Property(t => t.FromPushPage).HasColumnName("FromPushPage");



        }
    }
}
