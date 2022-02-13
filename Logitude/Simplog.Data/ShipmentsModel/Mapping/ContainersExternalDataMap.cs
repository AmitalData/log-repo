using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainersExternalDataMap : EntityTypeConfiguration<ContainersExternalData>
    {
        public ContainersExternalDataMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("ContainersExternalDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.GateIn).HasColumnName("GateIn");
            this.Property(t => t.GateOut).HasColumnName("GateOut");
        }
    }
}
