

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class OceanCarrierStatusAPIconfigMap : EntityTypeConfiguration<OceanCarrierStatusAPIconfig>
    {
        public OceanCarrierStatusAPIconfigMap()
        {
           
                this.ToTable("OceanCarrierStatusAPIconfig");

                this.HasKey(e => e.Id);

                this.Property(e => e.Id)
                    .HasColumnType("varchar")
                    .HasMaxLength(15)
                    .IsRequired();

                this.Property(e => e.Tenant)
                    .IsRequired();

                this.Property(e => e.SCACCode)
                    .HasColumnType("varchar")
                    .HasMaxLength(4)
                    .IsRequired();

                this.Property(e => e.URL)
                    .HasColumnType("varchar")
                    .HasMaxLength(250)
                    .IsRequired();

                this.Property(e => e.ResponseFormat)
                    .HasColumnType("varchar")
                    .HasMaxLength(4)
                    .IsRequired();

                this.Property(e => e.Frequency)
                    .IsRequired();

                this.Property(e => e.RateLimit);
                   

                this.Property(e => e.Inactive)
                    .IsRequired();

                this.Property(e => e.PeriodJourneyEnd)
                    .IsRequired();
           

        }
    }
}
