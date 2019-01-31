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
 
    public class JournalMoreDataMap : EntityTypeConfiguration<JournalMoreData>
    {
	    string dbms;
        public JournalMoreDataMap()
        { 
				this.ToTable("JournalMoreDatas");
		
		    this.HasKey(t => new { t.JournalId, t.Line });
	 
            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.GeneralData).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.GeneralData).HasMaxLength(4000);
			}


            this.Property(t => t.GeneralData).HasColumnName("GeneralData").IsRequired().IsUnicode(true);
        }
    }
}
	 