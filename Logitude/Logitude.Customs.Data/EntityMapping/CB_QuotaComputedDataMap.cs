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
 
    public class CB_QuotaComputedDataMap : EntityTypeConfiguration<CB_QuotaComputedData>
    {
	    string dbms;
        public CB_QuotaComputedDataMap()
        { 
			  this.ToTable("CB_QuotaComputedDatas", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.QuotaID).HasColumnName("QuotaID");

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.ValidQuotaDetailsHistoryID).HasColumnName("ValidQuotaDetailsHistoryID");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
	 