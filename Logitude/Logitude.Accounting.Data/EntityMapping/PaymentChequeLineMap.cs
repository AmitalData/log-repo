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
 
    public class PaymentChequeLineMap : EntityTypeConfiguration<PaymentChequeLine>
    {
	    string dbms;
        public PaymentChequeLineMap()
        { 
				this.ToTable("PaymentChequeLines");
		
		    this.HasKey(t => new { t.PaymentChequeId, t.Line });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.PaymentChequeId).HasColumnName("PaymentChequeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Amount).HasColumnName("Amount");

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric");
        }
    }
}
	 