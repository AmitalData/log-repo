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
 
    public class CustomsItemMap : EntityTypeConfiguration<CustomsItem>
    {
	    string dbms;
        public CustomsItemMap()
        { 
			  this.ToTable("CustomsItems", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID");

            this.Property(t => t.FullClassification).HasColumnName("FullClassification").HasMaxLength(13).IsUnicode(false);

            this.Property(t => t.CustomsItemCategoryID).HasColumnName("CustomsItemCategoryID");

            this.Property(t => t.CustomsItemHierarchicLocatioID).HasColumnName("CustomsItemHierarchicLocatioID");

            this.Property(t => t.ComputedCheckDigit).HasColumnName("ComputedCheckDigit").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(9).IsUnicode(false);
        }
    }
}
	 