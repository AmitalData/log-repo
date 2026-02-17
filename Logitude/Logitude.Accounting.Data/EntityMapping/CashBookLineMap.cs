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
 
    public class CashBookLineMap : EntityTypeConfiguration<CashBookLine>
    {
	    string dbms;
        public CashBookLineMap()
        { 
				this.ToTable("CashBookLines");
		
		    this.HasKey(t => new { t.CashBookId, t.ARPChequeId });
	 
            this.Property(t => t.CashBookId).HasColumnName("CashBookId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ARPChequeId).HasColumnName("ARPChequeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsDeposited).HasColumnName("IsDeposited");

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
        }
    }
}
	 