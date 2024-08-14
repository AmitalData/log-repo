using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class BankAccountLiteMap : EntityTypeConfiguration<BankAccountLite>
    {
        public BankAccountLiteMap()
        {
            this.ToTable("BankAccountLites");
            this.HasKey(t => new { t.Id });
            this.Property(t => t.VatNumber).HasColumnName("VatNumber").HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(30).IsUnicode(true);
            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.BranchNumber).HasColumnName("BranchNumber").HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber").IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.IBAN).HasColumnName("IBAN").HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.SwiftCode).HasColumnName("SwiftCode").HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.BankCode).HasColumnName("BankCode").IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.BranchAddress).HasColumnName("BranchAddress").HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(d => d.CurrencyId);
        }
    }
}
