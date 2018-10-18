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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class ARPChequeLineMap : EntityTypeConfiguration<ARPChequeLine>
    {
	    string dbms;
        public ARPChequeLineMap()
        { 
				this.ToTable("ARPChequeLines");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.SearchFields).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.SearchFields).HasMaxLength(4000);
			}


            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsUnicode(true);

            this.Property(t => t.PaymentId).HasColumnName("PaymentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();

            this.Property(t => t.ChequeNumber).HasColumnName("ChequeNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ValueDate).HasColumnName("ValueDate").IsRequired();

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount").IsRequired().HasPrecision(16, 2);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").IsRequired().HasPrecision(16, 2);

            this.Property(t => t.BankId).HasColumnName("BankId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BankBranch).HasColumnName("BankBranch").IsRequired().HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.BankAccount).HasColumnName("BankAccount").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 