using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AWBDescriptionOfGoodsMap : EntityTypeConfiguration<AWBDescriptionOfGoods>
    {
        public AWBDescriptionOfGoodsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.ShortDescriptionOfGoods)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.AirlineCode)
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.ProductCode)
               .HasMaxLength(4)
               .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Service)
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AWBDescriptionOfGoods");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortDescriptionOfGoods).HasColumnName("ShortDescriptionOfGoods");
            this.Property(t => t.AirlineCode).HasColumnName("AirlineCode");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Service).HasColumnName("Service");
            this.Property(t => t.IsTemperatureSensitive).HasColumnName("IsTemperatureSensitive");
        }
    }
}
