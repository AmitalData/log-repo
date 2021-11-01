using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;


namespace Simplog.Data.InvoiceModel.Mapping
{

    public class ARPaymentBankTranferMap : EntityTypeConfiguration<ARPaymentBankTranfer>
    {
	    string dbms;
        public ARPaymentBankTranferMap()
        { 
				this.ToTable("ARPaymentBankTranfers");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.PaymentId).HasColumnName("PaymentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber");

            this.Property(t => t.PaymentRef).HasColumnName("PaymentRef").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ValueDate).HasColumnName("ValueDate");

            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount").HasPrecision(16, 2);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").HasPrecision(16, 2);

            this.Property(t => t.ExchageRate).HasColumnName("ExchageRate").HasPrecision(16, 2);
        }
    }
}
	 