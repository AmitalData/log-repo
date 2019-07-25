using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardContactProductMap : EntityTypeConfiguration<CardContactProduct>
    {
        public CardContactProductMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CardContactId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductTypeCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            
            // Table & Column Mappings
            this.ToTable("CardContactProducts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");            
            this.Property(t => t.CardContactId).HasColumnName("CardContactId");
            
            // Relations
            this.HasRequired(t => t.CardContact).WithMany().HasForeignKey(d => d.CardContactId);
            this.HasRequired(t => t.ProductType).WithMany().HasForeignKey(d => d.ProductTypeCode);            
        }
    }
}
