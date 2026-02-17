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
 
    public class PaymentOrderConnectionTableMap : EntityTypeConfiguration<PaymentOrderConnectionTable>
    {
	    string dbms;
        public PaymentOrderConnectionTableMap()
        { 
			  this.ToTable("PaymentOrderConnectionTables", "Customs");
		
		    this.HasKey(t => new { t.PaymentOrderId, t.ConnectedEntityId });
	 
            this.Property(t => t.PaymentOrderId).HasColumnName("PaymentOrderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConnectedEntityCode).HasColumnName("ConnectedEntityCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ConnectedEntityId).HasColumnName("ConnectedEntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 