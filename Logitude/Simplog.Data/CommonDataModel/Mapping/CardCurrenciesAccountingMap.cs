using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardCurrenciesAccountingMap : EntityTypeConfiguration<CardCurrenciesAccounting>
    {
        public CardCurrenciesAccountingMap()
        {
            this.Property(t => t.Id).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant);
            this.Property(t => t.CardId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.CurrencyId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.PayableDebitAccount).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableCreditAccount).HasMaxLength(15).IsUnicode(false);

            this.ToTable("CardCurrenciesAccountings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.PayableDebitAccount).HasColumnName("PayableDebitAccount");
            this.Property(t => t.ReceivableCreditAccount).HasColumnName("ReceivableCreditAccount");


            this.HasRequired(t => t.Card).WithMany().HasForeignKey(t => t.CardId);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(t => t.CurrencyId);
        }
    }
}
