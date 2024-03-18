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
 
    public class CB_CountriesExclusionMap : EntityTypeConfiguration<CB_CountriesExclusion>
    {
	    string dbms;
        public CB_CountriesExclusionMap()
        { 
			  this.ToTable("CB_CountriesExclusions", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.RegularityRequirementID).HasColumnName("RegularityRequirementID");

            this.Property(t => t.CountryID).HasColumnName("CountryID").HasMaxLength(6).IsUnicode(false);
        }
    }
}
	 