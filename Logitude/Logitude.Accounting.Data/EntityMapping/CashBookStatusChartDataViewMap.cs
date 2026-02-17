using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CashBookStatusChartDataViewMap : EntityTypeConfiguration<CashBookStatusChartDataView>
    {
        public CashBookStatusChartDataViewMap()
        {
            this.ToTable("CashBookStatusChartDataView");
            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
         
            // Table & Column Mappings

            // GLAccount
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CashBookTypeCode).HasColumnName("CashBookTypeCode");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.CurrencyCode).HasColumnName("CurrencyCode");
            this.Property(t => t.ChequeValueDate).HasColumnName("ChequeValueDate");
            this.Property(t => t.ChequeAmount).HasColumnName("ChequeAmount");

        }
    }
}
