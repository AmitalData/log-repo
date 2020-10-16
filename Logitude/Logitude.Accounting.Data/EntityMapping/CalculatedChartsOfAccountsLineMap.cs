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
 
    public class CalculatedChartsOfAccountsLineMap : EntityTypeConfiguration<CalculatedChartsOfAccountsLine>
    {
	    string dbms;
        public CalculatedChartsOfAccountsLineMap()
        { 
		
      dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
      if (dbms == "oracle")
      {
		this.ToTable("CalculatedChartsOfAccountsLines");
      }
	  else
	  {
	    this.ToTable("CalculatedChartsOfAccountsLine");
	  }

		
		    this.HasKey(t => new { t.Id, t.CreateDateTime });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedDateTime).HasColumnName("UpdatedDateTime").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsDetailedGLAccount).HasColumnName("IsDetailedGLAccount").IsRequired();

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled").IsRequired();

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChartOfAccountId).HasColumnName("ChartOfAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineTypeCode).HasColumnName("LineTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CalculatedChartsOfAccountsId).HasColumnName("CalculatedChartsOfAccountsId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 