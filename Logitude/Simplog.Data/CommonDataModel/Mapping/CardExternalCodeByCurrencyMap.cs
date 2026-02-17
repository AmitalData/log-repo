using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardExternalCodeByCurrencyMap : EntityTypeConfiguration<CardExternalCodeByCurrency>
    {
        public CardExternalCodeByCurrencyMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasMaxLength(15)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.CardId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CurrencyId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ExternalRecievableTableId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ExternalPayableTableId)
               .HasMaxLength(15)
               .IsUnicode(false);

            this.ToTable("CardExternalCodeByCurrencies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.ExternalRecievableTableId).HasColumnName("ExternalRecievableTableId");
            this.Property(t => t.ExternalPayableTableId).HasColumnName("ExternalPayableTableId");

        }



    }
}
