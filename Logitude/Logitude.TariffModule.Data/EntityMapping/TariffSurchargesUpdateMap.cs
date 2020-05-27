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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data;
 
namespace Logitude.TariffModule.Data.EntityMapping
{
 
    public class TariffSurchargesUpdateMap : EntityTypeConfiguration<TariffSurchargesUpdate>
    {
	    string dbms;
        public TariffSurchargesUpdateMap()
        { 
				this.ToTable("TariffSurchargesUpdates");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffId).HasColumnName("TariffId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.LinesUpdated).HasColumnName("LinesUpdated");

            this.Property(t => t.From).HasColumnName("From").IsMaxLength().IsUnicode(true);

            this.Property(t => t.To).HasColumnName("To").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Version).HasColumnName("Version");

            this.Property(t => t.Surcharges).HasColumnName("Surcharges").IsMaxLength().IsUnicode(true);

            this.Property(t => t.UpdateMethodCode).HasColumnName("UpdateMethodCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 