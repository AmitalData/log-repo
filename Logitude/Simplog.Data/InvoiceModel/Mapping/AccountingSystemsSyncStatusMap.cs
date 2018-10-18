using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AccountingSystemsSyncStatusMap : EntityTypeConfiguration<AccountingSystemsSyncStatus>
    {

        public AccountingSystemsSyncStatusMap()
        {
            this.HasKey(d => d.Id);

            this.Property(d => d.Id)
                .HasMaxLength(15)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.LastError)
                .HasMaxLength(6000)
                .IsUnicode(true);

            this.ToTable("AccountingSystemsSyncStatuses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LastError).HasColumnName("LastError");
            this.Property(t => t.LastRequestDate).HasColumnName("LastRequestDate");
            this.Property(t => t.SyncInterval).HasColumnName("SyncInterval");
            this.Property(t => t.ExternalCodesLastUpdate).HasColumnName("ExternalCodesLastUpdate");
            this.Property(t => t.LastErrorDate).HasColumnName("LastErrorDate");


        }
    }
}
