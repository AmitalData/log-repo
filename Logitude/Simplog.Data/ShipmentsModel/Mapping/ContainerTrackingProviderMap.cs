using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainerTrackingProviderMap : EntityTypeConfiguration<ContainerTrackingProvider>
    {
        public ContainerTrackingProviderMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);
            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);


            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.ProviderURL)
                .HasMaxLength(2000)
                .IsUnicode(true);
            this.Property(t => t.CallbackURL)
                .HasMaxLength(2000)
                .IsUnicode(true);
            this.Property(t => t.APIKey)
                .HasMaxLength(2000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ContainerTrackingProviders");
            
        }
    }
}
