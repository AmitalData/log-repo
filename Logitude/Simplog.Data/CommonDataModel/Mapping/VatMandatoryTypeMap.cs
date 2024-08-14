using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class VatMandatoryTypeMap : EntityTypeConfiguration<VatMandatoryType>
    {
        public VatMandatoryTypeMap()
        {
            this.HasKey(t => t.Code);

            this.Property(t => t.Code).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("VatMandatoryType");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ViewOrder).HasColumnName("ViewOrder");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
