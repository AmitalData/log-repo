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
 
    public class ReconciliationLineMap : EntityTypeConfiguration<ReconciliationLine>
    {
	    string dbms;
        public ReconciliationLineMap()
        { 
				this.ToTable("ReconciliationLines");
		
		    this.HasKey(t => new { t.ReconciliationId, t.Line });
	 
            this.Property(t => t.ReconciliationId).HasColumnName("ReconciliationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransactionId).HasColumnName("TransactionId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReconciliationAmount).HasColumnName("ReconciliationAmount").HasPrecision(16, 2);

            this.Property(t => t.IsPartial).HasColumnName("IsPartial").IsRequired();

            this.Property(t => t.GroupNumber).HasColumnName("GroupNumber");

            this.Property(t => t.IsAdjustTransaction).HasColumnName("IsAdjustTransaction");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 