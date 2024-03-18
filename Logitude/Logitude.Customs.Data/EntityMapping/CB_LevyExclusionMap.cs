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
 
    public class CB_LevyExclusionMap : EntityTypeConfiguration<CB_LevyExclusion>
    {
	    string dbms;
        public CB_LevyExclusionMap()
        { 
			  this.ToTable("CB_LevyExclusions", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LevyExclusionNumber).HasColumnName("LevyExclusionNumber");

            this.Property(t => t.TradeLevyID).HasColumnName("TradeLevyID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VendorID).HasColumnName("VendorID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CountryGroupID).HasColumnName("CountryGroupID").HasMaxLength(2).IsUnicode(false);
        }
    }
}
	 