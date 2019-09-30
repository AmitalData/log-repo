using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CreditLimitSettingMap : EntityTypeConfiguration<CreditLimitSetting>
    {
        public CreditLimitSettingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("CreditLimitSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsCreditLimitEnabled).HasColumnName("IsCreditLimitEnabled");
            this.Property(t => t.InvoiceCreationWarning).HasColumnName("InvoiceCreationWarning");
            this.Property(t => t.InvoiceCreationBlock).HasColumnName("InvoiceCreationBlock");
            this.Property(t => t.ShipmentCreationBlock).HasColumnName("ShipmentCreationBlock");
        }
    }
}
