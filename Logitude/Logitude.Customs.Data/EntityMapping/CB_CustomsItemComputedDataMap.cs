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
 
    public class CB_CustomsItemComputedDataMap : EntityTypeConfiguration<CB_CustomsItemComputedData>
    {
	    string dbms;
        public CB_CustomsItemComputedDataMap()
        { 
			  this.ToTable("CB_CustomsItemComputedDatas", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID");

            this.Property(t => t.FullClassification).HasColumnName("FullClassification").HasMaxLength(13).IsUnicode(false);

            this.Property(t => t.IsLeaf).HasColumnName("IsLeaf");

            this.Property(t => t.CustomsItemDetailsHistoryID).HasColumnName("CustomsItemDetailsHistoryID");

            this.Property(t => t.PropertiesDetailsHistoryID).HasColumnName("PropertiesDetailsHistoryID");

            this.Property(t => t.PH_MeasurementUnitID).HasColumnName("PH_MeasurementUnitID");

            this.Property(t => t.IsHistoryExists).HasColumnName("IsHistoryExists");

            this.Property(t => t.IsRulesExists).HasColumnName("IsRulesExists");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.CI_Parent_CustomsItemIDNum).HasColumnName("CI_Parent_CustomsItemIDNum");

            this.Property(t => t.CI_BaseFullClassification).HasColumnName("CI_BaseFullClassification").HasMaxLength(11).IsUnicode(false);

            this.Property(t => t.CI_ComputedCheckDigit).HasColumnName("CI_ComputedCheckDigit").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CI_CustomsBookTypeIDNum).HasColumnName("CI_CustomsBookTypeIDNum").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CI_CustomsItemCategoryIDNum).HasColumnName("CI_CustomsItemCategoryIDNum").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ItemHierarchicLocationID).HasColumnName("ItemHierarchicLocationID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CIH_Title).HasColumnName("CIH_Title").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.CIH_GoodsDescription).HasColumnName("CIH_GoodsDescription").IsMaxLength().IsUnicode(true);

            this.Property(t => t.CustomsItemEntityStatusIDNum).HasColumnName("CustomsItemEntityStatusIDNum");

            this.Property(t => t.PH_IsCarItem).HasColumnName("PH_IsCarItem");

            this.Property(t => t.FullGoodsDescription).HasColumnName("FullGoodsDescription").IsMaxLength().IsUnicode(true);
        }
    }
}
	 