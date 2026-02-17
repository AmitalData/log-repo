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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class ClaimsRelatedEntitiesRefundMap : EntityTypeConfiguration<ClaimsRelatedEntitiesRefund>
    {
	    string dbms;
        public ClaimsRelatedEntitiesRefundMap()
        { 
			  this.ToTable("ClaimsRelatedEntitiesRefunds", "Customs");
		
		    this.HasKey(t => new { t.ClaimId, t.CounterKey, t.RefundQuntityLineNo });
	 
            this.Property(t => t.ClaimId).HasColumnName("ClaimId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CounterKey).HasColumnName("CounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.RefundQuntityLineNo).HasColumnName("RefundQuntityLineNo").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric");

            this.Property(t => t.RefundQuntity).HasColumnName("RefundQuntity").HasPrecision(16, 6);
        }
    }
}
	 