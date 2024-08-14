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
 
    public class TariffSurchargesUpdateMethodMap : EntityTypeConfiguration<TariffSurchargesUpdateMethod>
    {
	    string dbms;
        public TariffSurchargesUpdateMethodMap()
        { 
				this.ToTable("TariffSurchargesUpdateMethods");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 