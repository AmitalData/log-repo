using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerSizeMap: EntityTypeConfiguration<CustomerSize>
    {
        public CustomerSizeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.Code).IsFixedLength().HasMaxLength(1).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CustomerSizes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
