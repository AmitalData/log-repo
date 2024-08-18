using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DistributorMap : EntityTypeConfiguration<Distributor>
    {
        public DistributorMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);
                

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.LocalName)
                .HasMaxLength(60)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Distributors");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
          
        }
    }
}