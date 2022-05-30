using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainerTrackingRequestMap : EntityTypeConfiguration<ContainerTrackingRequest>
    {
        public ContainerTrackingRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Master).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.CarrierCode).HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.Status).HasMaxLength(200).IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.Provider)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.RequestId)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ContainerTrackingRequests");
            
        }
    }
}
