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
 
    public class ARPaymentsJournalMap : EntityTypeConfiguration<ARPaymentsJournal>
    {
	    string dbms;
        public ARPaymentsJournalMap()
        { 
				this.ToTable("ARPaymentsJournals");
		
		    this.HasKey(t => new { t.Tenant, t.PaymentId, t.IsVoided });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.PaymentId).HasColumnName("PaymentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsVoided).HasColumnName("IsVoided").IsRequired();
        }
    }
}
	 