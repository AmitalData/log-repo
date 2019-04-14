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
 
    public class TariffVersionMap : EntityTypeConfiguration<TariffVersion>
    {
	    string dbms;
        public TariffVersionMap()
        { 
				this.ToTable("TariffVersions");
		
		    this.HasKey(t => new { t.TariffId, t.Version });
	 
            this.Property(t => t.TariffId).HasColumnName("TariffId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.Version).HasColumnName("Version").HasDatabaseGeneratedOption(null);
        }
    }
}
	 