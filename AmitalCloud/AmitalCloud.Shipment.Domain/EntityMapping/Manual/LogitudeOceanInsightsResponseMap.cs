using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class LogitudeOceanInsightsResponseMap : EntityTypeConfiguration<LogitudeOceanInsightsResponse>
    {
        public LogitudeOceanInsightsResponseMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.CarrierName).HasMaxLength(100).IsUnicode(false);


            this.ToTable("LogitudeOceanInsightsResponses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.SCACCode).HasColumnName("SCACCode");
            this.Property(t => t.CarrierName).HasColumnName("CarrierName");
            this.Property(t => t.FirstResponseDate).HasColumnName("FirstResponseDate");
            this.Property(t => t.LastResponseDate).HasColumnName("LastResponseDate");
        }
    }
}
