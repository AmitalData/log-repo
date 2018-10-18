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
 
    public class TaxReportLineMap : EntityTypeConfiguration<TaxReportLine>
    {
	    string dbms;
        public TaxReportLineMap()
        { 
				this.ToTable("TaxReportLines");
		
		    this.HasKey(t => new { t.TaxReportId, t.Line });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.LastUpdateDateTime).HasColumnName("LastUpdateDateTime").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.TaxReportId).HasColumnName("TaxReportId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.OutputOrInput).HasColumnName("OutputOrInput").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.LineTypeCode).HasColumnName("LineTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.VatNumber).HasColumnName("VatNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Reference).HasColumnName("Reference").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReferecneGroup).HasColumnName("ReferecneGroup").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReferenceDate).HasColumnName("ReferenceDate");

            this.Property(t => t.VatAmount).HasColumnName("VatAmount").HasPrecision(16, 2);

            this.Property(t => t.VatableInvoiceAmount).HasColumnName("VatableInvoiceAmount").HasPrecision(16, 2);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TransmitStatusCode).HasColumnName("TransmitStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsManuallyChanged).HasColumnName("IsManuallyChanged");

            this.Property(t => t.IsEquipment).HasColumnName("IsEquipment");
        }
    }
}
	 