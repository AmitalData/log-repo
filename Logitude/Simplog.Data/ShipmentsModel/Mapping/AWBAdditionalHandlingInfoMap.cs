using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class AWBAdditionalHandlingInfoMap : EntityTypeConfiguration<AWBAdditionalHandlingInfo>
    {
        public AWBAdditionalHandlingInfoMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrintDescription).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("AWBAdditionalHandlingInfos");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PrintDescription).HasColumnName("PrintDescription");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
