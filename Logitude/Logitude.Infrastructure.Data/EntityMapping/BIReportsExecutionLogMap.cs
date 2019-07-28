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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;
 
namespace Logitude.Infrastructure.Data.EntityMapping
{
 
    public class BIReportsExecutionLogMap : EntityTypeConfiguration<BIReportsExecutionLog>
    {
	    string dbms;
        public BIReportsExecutionLogMap()
        { 
				this.ToTable("BIReportsExecutionLogs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(4).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.ExceptionMessage).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.ExceptionMessage).HasMaxLength(8000);
			}


            this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage").IsUnicode(true);

            this.Property(t => t.DoneDate).HasColumnName("DoneDate");

            this.Property(t => t.ReportFilterXML).HasColumnName("ReportFilterXML").IsMaxLength().IsUnicode(true);

            this.Property(t => t.BIReportId).HasColumnName("BIReportId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 