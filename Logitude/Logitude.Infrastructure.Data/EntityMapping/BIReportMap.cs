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
 
    public class BIReportMap : EntityTypeConfiguration<BIReport>
    {
	    string dbms;
        public BIReportMap()
        { 
				this.ToTable("BIReports");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(80).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.DWQueryId).HasColumnName("DWQueryId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.AGGridOptionsXML).HasColumnName("AGGridOptionsXML").IsMaxLength().IsUnicode(true);

            this.Property(t => t.BIReportFolderId).HasColumnName("BIReportFolderId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastRunDate).HasColumnName("LastRunDate").IsRequired();

            this.Property(t => t.LastRunByUserId).HasColumnName("LastRunByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 