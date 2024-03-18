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
 
    public class CB_LevyConditionMap : EntityTypeConfiguration<CB_LevyCondition>
    {
	    string dbms;
        public CB_LevyConditionMap()
        { 
			  this.ToTable("CB_LevyConditions", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LevyConditionNumber).HasColumnName("LevyConditionNumber");

            this.Property(t => t.LevyGoodsDescription).HasColumnName("LevyGoodsDescription").HasMaxLength(512).IsUnicode(false);

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID");

            this.Property(t => t.VendorID).HasColumnName("VendorID");

            this.Property(t => t.CountryGroupID).HasColumnName("CountryGroupID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsCountriesGroup).HasColumnName("IsCountriesGroup");

            this.Property(t => t.CountryID).HasColumnName("CountryID").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.TradeLevyID).HasColumnName("TradeLevyID");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
	 