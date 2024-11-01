using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class OceanInsightsStatusLogMap : EntityTypeConfiguration<OceanInsightsStatusLog>
    {
        public OceanInsightsStatusLogMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
          
            this.Property(t => t.OceanInsigntRequestId).HasMaxLength(500).IsUnicode(false);
			this.Property(t => t.XML).IsMaxLength().IsUnicode(true);

            this.ToTable("OceanInsightsStatusLogs");
            this.Property(t => t.Id).HasColumnName("Id"); 
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.OceanInsigntRequestId).HasColumnName("OceanInsigntRequestId");
			this.Property(t => t.XML).HasColumnName("XML");
			this.Property(t => t.CreateDate).HasColumnName("CreateDate");
          



        }
    }
}
