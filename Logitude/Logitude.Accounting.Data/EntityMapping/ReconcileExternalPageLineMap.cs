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
 
    public class ReconcileExternalPageLineMap : EntityTypeConfiguration<ReconcileExternalPageLine>
    {
	    string dbms;
        public ReconcileExternalPageLineMap()
        { 
				this.ToTable("ReconcileExternalPageLines");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.ReconcileExternalPageId).HasColumnName("ReconcileExternalPageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DebitAmount).HasColumnName("DebitAmount").HasPrecision(15, 2);

            this.Property(t => t.ReferenceDate).HasColumnName("ReferenceDate");

            this.Property(t => t.Reference).HasColumnName("Reference").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.IsReconciled).HasColumnName("IsReconciled");

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

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReconcileRemarks).HasColumnName("ReconcileRemarks").HasMaxLength(400).IsUnicode(true);

            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount").HasPrecision(15, 2);
        }
    }
}
	 