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
 
    public class PaymentOrderLineMap : EntityTypeConfiguration<PaymentOrderLine>
    {
	    string dbms;
        public PaymentOrderLineMap()
        { 
			  this.ToTable("PaymentOrderLines", "Customs");
		
		    this.HasKey(t => new { t.PaymentOrderId, t.ParagraphTypeCode });
	 
            this.Property(t => t.PaymentOrderId).HasColumnName("PaymentOrderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ParagraphTypeCode).HasColumnName("ParagraphTypeCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Amount).HasColumnName("Amount").HasPrecision(18, 2);
        }
    }
}
	 