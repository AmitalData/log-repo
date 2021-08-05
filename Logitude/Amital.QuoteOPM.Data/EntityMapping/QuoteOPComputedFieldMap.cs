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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPComputedFieldMap : EntityTypeConfiguration<QuoteOPComputedField>
    {
	    string dbms;
        public QuoteOPComputedFieldMap()
        { 
				this.ToTable("QuoteOPComputedFields");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConnectedToShipment).HasColumnName("ConnectedToShipment");

            this.Property(t => t.ConnectedToTicket).HasColumnName("ConnectedToTicket");

            this.Property(t => t.ToLocation).HasColumnName("ToLocation").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.FromLocation).HasColumnName("FromLocation").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.DeliveryTo).HasColumnName("DeliveryTo").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.PickupFrom).HasColumnName("PickupFrom").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.EstimatedPayablesInSales).HasColumnName("EstimatedPayablesInSales");

            this.Property(t => t.EstimatedPayablesInLocal).HasColumnName("EstimatedPayablesInLocal");

            this.Property(t => t.EstimatedReceivablesInLocal).HasColumnName("EstimatedReceivablesInLocal");

            this.Property(t => t.EstimatedReceivablesInSales).HasColumnName("EstimatedReceivablesInSales");

            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");
        }
    }
}
	 