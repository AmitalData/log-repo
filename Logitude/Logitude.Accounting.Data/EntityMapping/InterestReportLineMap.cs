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
 
    public class InterestReportLineMap : EntityTypeConfiguration<InterestReportLine>
    {
	    string dbms;
        public InterestReportLineMap()
        { 
				this.ToTable("InterestReportLines");
		
		    this.HasKey(t => new { t.InterestReportId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.InterestReportId).HasColumnName("InterestReportId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InterestTransactionId).HasColumnName("InterestTransactionId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 