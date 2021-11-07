using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PortTimeZoneMap : EntityTypeConfiguration<PortTimeZone>
    {
        public PortTimeZoneMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(150).IsUnicode(false);            
            this.Property(t => t.Notes).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.UTCOffset).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.UTCDSTOffset).HasMaxLength(20).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("PortTimeZones");
            this.Property(t => t.Code).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Tenant");
            this.Property(t => t.Notes).HasColumnName("EnglishName");
            this.Property(t => t.SearchFields).HasColumnName("LocalName");
            this.Property(t => t.UTCOffset).HasColumnName("Notes");            
            this.Property(t => t.UTCDSTOffset).HasColumnName("SearchFields");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
