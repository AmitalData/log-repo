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
 
    public class CB_CustomsItemDetailsHistoryMap : EntityTypeConfiguration<CB_CustomsItemDetailsHistory>
    {
	    string dbms;
        public CB_CustomsItemDetailsHistoryMap()
        { 
			  this.ToTable("CB_CustomsItemDetailsHistorys", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EnglishGoodsDescription).HasColumnName("EnglishGoodsDescription").IsMaxLength().IsUnicode(false);

            this.Property(t => t.GoodsDescription).HasColumnName("GoodsDescription").IsMaxLength().IsUnicode(false);

            this.Property(t => t.GoodsDescriptionRTF).HasColumnName("GoodsDescriptionRTF").IsMaxLength().IsUnicode(false);

            this.Property(t => t.EnglishGoodsDescriptionRTF).HasColumnName("EnglishGoodsDescriptionRTF").IsMaxLength().IsUnicode(false);

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");
        }
    }
}
	 