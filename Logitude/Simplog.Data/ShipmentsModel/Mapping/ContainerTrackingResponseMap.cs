using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainerTrackingResponseMap : EntityTypeConfiguration<ContainerTrackingResponse>
    {
        public ContainerTrackingResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.ContainerTrackingRequestId)
                            .HasMaxLength(15)
                            .IsUnicode(false);

            this.HasRequired(e => e.ContainerTrackingRequest).WithMany().HasForeignKey(e => e.ContainerTrackingRequestId);


            // Table & Column Mappings
            this.ToTable("ContainerTrackingResponses");

        }
    }
}
