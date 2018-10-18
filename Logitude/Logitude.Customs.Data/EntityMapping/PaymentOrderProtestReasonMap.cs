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
 
    public class PaymentOrderProtestReasonMap : EntityTypeConfiguration<PaymentOrderProtestReason>
    {
	    string dbms;
        public PaymentOrderProtestReasonMap()
        { 
			  this.ToTable("PaymentOrderProtestReasons", "Customs");
		
		    this.HasKey(t => new { t.PaymentOrderId, t.Line });
	 
            this.Property(t => t.PaymentOrderId).HasColumnName("PaymentOrderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").HasDatabaseGeneratedOption(null);

            this.Property(t => t.ProtestTypeCode).HasColumnName("ProtestTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomsAgentExplanation).HasColumnName("CustomsAgentExplanation").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.GoodsItemLineNumber).HasColumnName("GoodsItemLineNumber").HasPrecision(5, 0);

            this.Property(t => t.GoodsItemClassification).HasColumnName("GoodsItemClassification").HasMaxLength(13).IsUnicode(false);

            this.Property(t => t.AmountInDispute).HasColumnName("AmountInDispute").HasPrecision(16, 2);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 