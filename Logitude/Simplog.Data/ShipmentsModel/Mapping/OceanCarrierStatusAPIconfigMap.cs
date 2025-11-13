

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
                    .HasColumnType("varchar(15)")
                    .IsRequired();

                this.Property(e => e.Tenant)
                    .IsRequired();

                this.Property(e => e.SCACCode)
                    .HasColumnType("varchar(4)")
                    .IsRequired();

                this.Property(e => e.URL)
                    .HasColumnType("varchar(250)")
                    .IsRequired();

                this.Property(e => e.ResponseFormat)
                    .HasColumnType("varchar(4)")
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
