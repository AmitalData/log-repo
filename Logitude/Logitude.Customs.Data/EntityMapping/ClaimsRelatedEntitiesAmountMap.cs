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
 
    public class ClaimsRelatedEntitiesAmountMap : EntityTypeConfiguration<ClaimsRelatedEntitiesAmount>
    {
	    string dbms;
        public ClaimsRelatedEntitiesAmountMap()
        { 
			  this.ToTable("ClaimsRelatedEntitiesAmounts", "Customs");
		
		    this.HasKey(t => new { t.ClaimId, t.CounterKey, t.LineNo });
	 
            this.Property(t => t.ClaimId).HasColumnName("ClaimId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CounterKey).HasColumnName("CounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNo).HasColumnName("LineNo").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.PaymentTypeCode).HasColumnName("PaymentTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Amount).HasColumnName("Amount").HasPrecision(16, 2);
        }
    }
}
	 