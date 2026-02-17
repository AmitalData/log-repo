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
 
    public class OpenFormatReportMap : EntityTypeConfiguration<OpenFormatReport>
    {
	    string dbms;
        public OpenFormatReportMap()
        { 
				this.ToTable("OpenFormatReports");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ReportNumber).HasColumnName("ReportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromDate).HasColumnName("FromDate").IsRequired();

            this.Property(t => t.ToDate).HasColumnName("ToDate").IsRequired();

            this.Property(t => t.StatusTypeCode).HasColumnName("StatusTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage").IsMaxLength().IsUnicode(true);

            this.Property(t => t.PDFRerportXML).HasColumnName("PDFRerportXML").IsMaxLength().IsUnicode(true);
        }
    }
}
	 