using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class RegionMap: EntityTypeConfiguration<Region>
    {
        public RegionMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(15);

            this.Property(t => t.Name)
                .IsUnicode(true)
                .HasMaxLength(200);

            this.Property(t => t.SearchFields)
                .IsUnicode(true)
                .HasMaxLength(1000);

            this.Property(t => t.LocalName)
                .IsUnicode(true)
                .HasMaxLength(100);

            this.ToTable("Regions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.InActive).HasColumnName("InActive");
        }
    }
}
