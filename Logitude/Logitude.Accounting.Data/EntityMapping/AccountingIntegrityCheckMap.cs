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
 
    public class AccountingIntegrityCheckMap : EntityTypeConfiguration<AccountingIntegrityCheck>
    {
	    string dbms;
        public AccountingIntegrityCheckMap()
        { 
				this.ToTable("AccountingIntegrityChecks");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDateTimeUTC).HasColumnName("CreateDateTimeUTC").IsRequired();

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ParametersXML).HasColumnName("ParametersXML").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ResultXML).HasColumnName("ResultXML").IsMaxLength().IsUnicode(true);

            this.Property(t => t.HasException).HasColumnName("HasException");

            this.Property(t => t.DoneDateTimeUTC).HasColumnName("DoneDateTimeUTC");
        }
    }
}
	 