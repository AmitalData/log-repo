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
 
    public class InterestReportsConnectInvoiceMap : EntityTypeConfiguration<InterestReportsConnectInvoice>
    {
	    string dbms;
        public InterestReportsConnectInvoiceMap()
        { 
				this.ToTable("InterestReportsConnectInvoices");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ReportId).HasColumnName("ReportId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 