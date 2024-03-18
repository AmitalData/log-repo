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
 
    public class CB_CustomsItemMap : EntityTypeConfiguration<CB_CustomsItem>
    {
	    string dbms;
        public CB_CustomsItemMap()
        { 
			  this.ToTable("CB_CustomsItems", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.FullClassification).HasColumnName("FullClassification").HasMaxLength(13).IsUnicode(false);

            this.Property(t => t.Parent_CustomsItemID).HasColumnName("Parent_CustomsItemID");

            this.Property(t => t.ComputedCheckDigit).HasColumnName("ComputedCheckDigit").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomsItemCategoryID).HasColumnName("CustomsItemCategoryID").HasMaxLength(4).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.CustomsItemHierarchicLocationID).HasColumnName("CustomsItemHierarchicLocationI").HasMaxLength(2).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.CustomsItemHierarchicLocationID).HasColumnName("CustomsItemHierarchicLocationID").HasMaxLength(2).IsUnicode(false);
			}

        }
    }
}
	 