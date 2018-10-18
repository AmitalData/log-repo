using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AccountingSystemsSettingMap : EntityTypeConfiguration<AccountingSystemsSetting>
    {

        public AccountingSystemsSettingMap()
        {
            this.HasKey(d => d.Id);

            this.Property(d => d.Id)
                .HasMaxLength(15)
                .IsRequired()
                .IsUnicode(false);

            this.ToTable("AccountingSystemsSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.GetExternalCodeInterval).HasColumnName("GetExternalCodeInterval");
            this.Property(t => t.UpdateOnNextRequest).HasColumnName("UpdateOnNextRequest");
        }

    }
}
