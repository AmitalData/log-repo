using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardExternalAccountsByProductMap : EntityTypeConfiguration<CardExternalAccountsByProduct>
    {
        public CardExternalAccountsByProductMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.GLAccount).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.CostCenter).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.CardId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductTypeCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("CardExternalAccountsByProducts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.GLAccount).HasColumnName("GLAccount");
            this.Property(t => t.CostCenter).HasColumnName("CostCenter");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            this.HasRequired(t => t.Card).WithMany().HasForeignKey(d => d.CardId);
            this.HasRequired(t => t.ProductType).WithMany().HasForeignKey(d => d.ProductTypeCode);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
