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
 
    public class TariffLineMap : EntityTypeConfiguration<TariffLine>
    {
	    string dbms;
        public TariffLineMap()
        { 
				this.ToTable("TariffLines");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate").IsRequired();

            this.Property(t => t.TariffId).HasColumnName("TariffId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Version).HasColumnName("Version");

            this.Property(t => t.MinPrice).HasColumnName("MinPrice");

            this.Property(t => t.Step1Price).HasColumnName("Step1Price");

            this.Property(t => t.Step2Price).HasColumnName("Step2Price");

            this.Property(t => t.Step3Price).HasColumnName("Step3Price");

            this.Property(t => t.Step4Price).HasColumnName("Step4Price");

            this.Property(t => t.Step5Price).HasColumnName("Step5Price");

            this.Property(t => t.Step6Price).HasColumnName("Step6Price");

            this.Property(t => t.Step7Price).HasColumnName("Step7Price");

            this.Property(t => t.Step8Price).HasColumnName("Step8Price");

            this.Property(t => t.OriginPortId).HasColumnName("OriginPortId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 