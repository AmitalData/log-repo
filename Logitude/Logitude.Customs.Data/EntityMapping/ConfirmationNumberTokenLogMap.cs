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
 
    public class ConfirmationNumberTokenLogMap : EntityTypeConfiguration<ConfirmationNumberTokenLog>
    {
	    string dbms;
        public ConfirmationNumberTokenLogMap()
        { 
			  this.ToTable("ConfirmationNumberTokenLogs", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CompanyIdInvoiceProducer).HasColumnName("CompanyIdInvoiceProducer").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CompanyIdInvoiceRecipient).HasColumnName("CompanyIdInvoiceRecipient").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CallType).HasColumnName("CallType").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.CommunicationType).HasColumnName("CommunicationType");

            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(2000).IsUnicode(true);
        }
    }
}
	 