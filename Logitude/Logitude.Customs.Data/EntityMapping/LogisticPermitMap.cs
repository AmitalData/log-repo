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
 
    public class LogisticPermitMap : EntityTypeConfiguration<LogisticPermit>
    {
	    string dbms;
        public LogisticPermitMap()
        { 
			  this.ToTable("LogisticPermits", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.TransmitDate).HasColumnName("TransmitDate");

            this.Property(t => t.ActionCode).HasColumnName("ActionCode").HasMaxLength(1).IsUnicode(true);

            this.Property(t => t.CargoIdentifierType).HasColumnName("CargoIdentifierType").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey1).HasColumnName("CargoIdentifierKey1").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CargoIdentifierKey2).HasColumnName("CargoIdentifierKey2").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CargoIdentifierKey3).HasColumnName("CargoIdentifierKey3").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 