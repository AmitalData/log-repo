using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CarrierServiceLineMap : EntityTypeConfiguration<CarrierServiceLine>
    {
        public CarrierServiceLineMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CardId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.PartnerTypeId).IsRequired().IsFixedLength().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("CarrierServiceLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");            
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            
            this.HasRequired(t => t.Card).WithMany().HasForeignKey(d => d.CardId);
            this.HasRequired(t => t.PartnerType).WithMany().HasForeignKey(d => d.PartnerTypeId);
        }
    }
}
