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
 
    public class JournalAdditionalDataMap : EntityTypeConfiguration<JournalAdditionalData>
    {
	    string dbms;
        public JournalAdditionalDataMap()
        { 
				this.ToTable("JournalAdditionalDatas");
		
		    this.HasKey(t => new { t.JournalId, t.JournalLineNumber });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TaxReportId).HasColumnName("TaxReportId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TaxReportTransmitStatusCode).HasColumnName("TaxReportTransmitStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.JournalLineNumber).HasColumnName("JournalLineNumber").HasDatabaseGeneratedOption(null);
        }
    }
}
	 