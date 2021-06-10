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
 
    public class GLAccountCardsDataMap : EntityTypeConfiguration<GLAccountCardsData>
    {
	    string dbms;
        public GLAccountCardsDataMap()
        { 
				this.ToTable("GLAccountCardsDatas");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreditLimit).HasColumnName("CreditLimit");

            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CollectorUserId).HasColumnName("CollectorUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Phone).HasColumnName("Phone").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.VatNumber).HasColumnName("VatNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.TotalOpenShipments).HasColumnName("TotalOpenShipments").HasPrecision(18, 2);

            this.Property(t => t.InsuredcreditLimit).HasColumnName("InsuredcreditLimit");
        }
    }
}
	 