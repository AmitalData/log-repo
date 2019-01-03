

using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{

    public class CustomsShipperMap : EntityTypeConfiguration<CustomsShipper>
    {
        public CustomsShipperMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomsShipperCode).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ValidDepositionNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(false);
            
            // Table & Column Mappings
            this.ToTable("CustomsShippers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomsShipperCode).HasColumnName("CustomsShipperCode");
            this.Property(t => t.ValidDepositionNumber).HasColumnName("ValidDepositionNumber");
            this.Property(t => t.ValidityStartDate).HasColumnName("ValidityStartDate");
            this.Property(t => t.ValidityEndDate).HasColumnName("ValidityEndDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            
            this.HasRequired(t => t.Card).WithOptional(t => t.CustomsShipper);

        }
    }

}
