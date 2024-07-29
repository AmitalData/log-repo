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
 
    public class TariffTypeMap : EntityTypeConfiguration<TariffType>
    {
	    string dbms;
        public TariffTypeMap()
        { 
				this.ToTable("TariffTypes");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.TransportModeCode).HasColumnName("TransportModeCode").HasMaxLength(1).IsFixedLength();

            this.Property(t => t.DirectionCode).HasColumnName("DirectionCode").HasMaxLength(1).IsFixedLength();
        }
    }
}
	 