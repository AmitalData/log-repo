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
 
    public class CB_RegularityRequirementMap : EntityTypeConfiguration<CB_RegularityRequirement>
    {
	    string dbms;
        public CB_RegularityRequirementMap()
        { 
			  this.ToTable("CB_RegularityRequirements", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.CountryID).HasColumnName("CountryID").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.IsAllCountries).HasColumnName("IsAllCountries");

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID");

            this.Property(t => t.IsAllCustomsItems).HasColumnName("IsAllCustomsItems");

            this.Property(t => t.IsLimitedCountryRegularRequire).HasColumnName("IsLimitedCountryRegularRequire");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.InceptionCodeID).HasColumnName("InceptionCodeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RegularityPublicationCodeID).HasColumnName("RegularityPublicationCodeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RegularitySourceCodeID).HasColumnName("RegularitySourceCodeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID").HasMaxLength(4).IsUnicode(false);
        }
    }
}
	 