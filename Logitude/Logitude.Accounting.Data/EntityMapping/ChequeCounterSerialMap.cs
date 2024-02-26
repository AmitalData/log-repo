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
 
    public class ChequeCounterSerialMap : EntityTypeConfiguration<ChequeCounterSerial>
    {
	    string dbms;
        public ChequeCounterSerialMap()
        { 
				this.ToTable("ChequeCounterSerials");
		
		    this.HasKey(t => new { t.SeriesId, t.BankAccountId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SeriesId).HasColumnName("SeriesId").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ChequeCounterBegin).HasColumnName("ChequeCounterBegin").IsRequired();

            this.Property(t => t.ChequeCounterEnd).HasColumnName("ChequeCounterEnd").IsRequired();

            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 