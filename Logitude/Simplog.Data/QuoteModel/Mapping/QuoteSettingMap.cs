using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteSettingMap: EntityTypeConfiguration<QuoteSetting>
    {
        public QuoteSettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("QuoteSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CopyShipper).HasColumnName("CopyShipper");
            this.Property(t => t.CopyConsignee).HasColumnName("CopyConsignee");
            this.Property(t => t.CopyMainCarriage).HasColumnName("CopyMainCarriage");
            this.Property(t => t.CopyPickup).HasColumnName("CopyPickup");
            this.Property(t => t.CopyDelivery).HasColumnName("CopyDelivery");
            this.Property(t => t.CopyChargesTypes).HasColumnName("CopyChargesTypes");
            this.Property(t => t.CopyChargesCost).HasColumnName("CopyChargesCost");
            this.Property(t => t.CopyChargesSale).HasColumnName("CopyChargesSale");
            this.Property(t => t.EditMainCarriage).HasColumnName("EditMainCarriage");
            this.Property(t => t.CopyAgent).HasColumnName("CopyAgent");
            this.Property(t => t.CopyNotify).HasColumnName("CopyNotify");
        }
    }
}
