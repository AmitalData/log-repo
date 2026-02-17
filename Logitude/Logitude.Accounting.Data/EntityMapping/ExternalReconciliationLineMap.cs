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
 
    public class ExternalReconciliationLineMap : EntityTypeConfiguration<ExternalReconciliationLine>
    {
	    string dbms;
        public ExternalReconciliationLineMap()
        { 
				this.ToTable("ExternalReconciliationLines");
		
		    this.HasKey(t => new { t.ReconciliationId, t.Line });
	 
            this.Property(t => t.ReconciliationId).HasColumnName("ReconciliationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LedgerTransactionId).HasColumnName("LedgerTransactionId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GroupNumber).HasColumnName("GroupNumber");

            this.Property(t => t.ExternalPageLineId).HasColumnName("ExternalPageLineId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
        }
    }
}
	 