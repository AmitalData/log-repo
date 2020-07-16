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
 
    public class InterestLastBatchServiceMap : EntityTypeConfiguration<InterestLastBatchService>
    {
	    string dbms;
        public InterestLastBatchServiceMap()
        { 
				this.ToTable("InterestLastBatchServices");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateReportsBatchId).HasColumnName("CreateReportsBatchId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateInvoicesBatchId).HasColumnName("CreateInvoicesBatchId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 