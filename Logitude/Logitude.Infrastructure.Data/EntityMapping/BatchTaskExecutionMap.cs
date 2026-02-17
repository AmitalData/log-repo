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
 
    public class BatchTaskExecutionMap : EntityTypeConfiguration<BatchTaskExecution>
    {
	    string dbms;
        public BatchTaskExecutionMap()
        { 
				this.ToTable("BatchTaskExecutions");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ClassName).HasColumnName("ClassName").HasMaxLength(120).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.PrametersXml).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.PrametersXml).HasMaxLength(4000);
			}


            this.Property(t => t.PrametersXml).HasColumnName("PrametersXml").IsUnicode(true);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(1).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.ErrorLog).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.ErrorLog).HasMaxLength(4000);
			}


            this.Property(t => t.ErrorLog).HasColumnName("ErrorLog").IsUnicode(true);

            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");

            this.Property(t => t.DoneDateTime).HasColumnName("DoneDateTime");

            this.Property(t => t.ProgressMessage).HasColumnName("ProgressMessage").HasMaxLength(120).IsUnicode(true);

            this.Property(t => t.ProgressPercentage).HasColumnName("ProgressPercentage");

            this.Property(t => t.Subject).HasColumnName("Subject").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.CallStack).HasColumnName("CallStack").IsMaxLength().IsUnicode(true);
        }
    }
}
	 