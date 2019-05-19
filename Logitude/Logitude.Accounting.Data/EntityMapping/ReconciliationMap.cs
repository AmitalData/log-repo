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
 
    public class ReconciliationMap : EntityTypeConfiguration<Reconciliation>
    {
	    string dbms;
        public ReconciliationMap()
        { 
				this.ToTable("Reconciliations");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            if (dbms == "oracle")
            {
                this.Property(t => t.Number).HasColumnName("RNumber").HasMaxLength(15).IsUnicode(false);
            }
            else
            {
                this.Property(t => t.Number).HasColumnName("Number").HasMaxLength(15).IsUnicode(false);
            }
                

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
        }
    }
}
	 