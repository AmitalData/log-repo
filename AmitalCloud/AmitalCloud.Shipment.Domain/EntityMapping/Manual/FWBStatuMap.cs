using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Shipment.Domain.EntityPOCOs;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class FWBStatuMap : EntityTypeConfiguration<FWBStatus>
    {
        public FWBStatuMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("FWBStatus");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
