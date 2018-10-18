using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentModel.Mapping
{
    public class SpecialServicesTypeMap : EntityTypeConfiguration<SpecialServicesType>
    {
        public SpecialServicesTypeMap()
        {
            this.HasKey(d => d.Id);
            this.Property(d => d.Id).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(d => d.Code).IsRequired().HasMaxLength(8).IsUnicode(false);
            this.Property(d => d.EnglishName).HasMaxLength(100).IsRequired().IsUnicode(false);
            this.Property(d => d.LocalName).HasMaxLength(100).IsUnicode(true);
            this.Property(d => d.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("SpecialServicesTypes");
            this.Property(d => d.Id).HasColumnName("Id");
            this.Property(d => d.Code).HasColumnName("Code");
            this.Property(d => d.EnglishName).HasColumnName("EnglishName");
            this.Property(d => d.LocalName).HasColumnName("LocalName");
            this.Property(d => d.Tenant).HasColumnName("Tenant");
            this.Property(d => d.SearchFields).HasColumnName("SearchFields");
            this.Property(d => d.InActive).HasColumnName("InActive");            
        }
    }
}
