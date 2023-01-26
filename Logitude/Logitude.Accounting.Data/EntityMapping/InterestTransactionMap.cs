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
 
    public class InterestTransactionMap : EntityTypeConfiguration<InterestTransaction>
    {
	    string dbms;
        public InterestTransactionMap()
        { 
				this.ToTable("InterestTransactions");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime").IsRequired();

            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InterestEntityTypeCode).HasColumnName("InterestEntityTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OriginalEntityLineNumber).HasColumnName("OriginalEntityLineNumber").IsRequired();

            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount").HasPrecision(16, 2);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").HasPrecision(16, 2);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InterestValueDate).HasColumnName("InterestValueDate").IsRequired();

            this.Property(t => t.InterestReportId).HasColumnName("InterestReportId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Notes).HasMaxLength(2000);
            }
            else
            {
                this.Property(t => t.Notes).HasMaxLength(4000);
            }


            this.Property(t => t.Notes).HasColumnName("Notes").IsUnicode(true);

        }
    }
}
	 