using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AccountingTransferHeaderMap : EntityTypeConfiguration<AccountingTransferHeader>
    {
        public AccountingTransferHeaderMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.TransferNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.AccountingTransferTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.TransferDate).IsOptional();
            this.Property(t => t.FileName).HasMaxLength(80).IsRequired().IsUnicode(true);
            this.Property(t => t.UserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsOptional().IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsOptional().IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("AccountingTransferHeaders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TransferNumber).HasColumnName("TransferNumber");
            this.Property(t => t.AccountingTransferTypeCode).HasColumnName("AccountingTransferTypeCode");
            this.Property(t => t.TransferDate).HasColumnName("TransferDate");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Notes).HasColumnName("Notes");

            this.HasRequired(t => t.TransferType).WithMany().HasForeignKey(d => d.AccountingTransferTypeCode);
            this.HasRequired(t => t.User).WithMany().HasForeignKey(t => t.UserId);
        }
    }
}
