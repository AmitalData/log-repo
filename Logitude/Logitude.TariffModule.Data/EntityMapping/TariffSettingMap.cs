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
 
    public class TariffSettingMap : EntityTypeConfiguration<TariffSetting>
    {
	    string dbms;
        public TariffSettingMap()
        { 
				this.ToTable("TariffSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultPriceSteps).HasColumnName("DefaultPriceSteps").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DefaultWarningPercentage).HasColumnName("DefaultWarningPercentage");

            this.Property(t => t.AirDefaultStepsId).HasColumnName("AirDefaultStepsId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LCLDefaultStepsId).HasColumnName("LCLDefaultStepsId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 