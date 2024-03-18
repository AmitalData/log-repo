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
 
    public class CB_TradeLevyMap : EntityTypeConfiguration<CB_TradeLevy>
    {
	    string dbms;
        public CB_TradeLevyMap()
        { 
			  this.ToTable("CB_TradeLevys", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.LevyNumber).HasColumnName("LevyNumber").IsMaxLength().IsUnicode(false);

            this.Property(t => t.EndOfInquiryDate).HasColumnName("EndOfInquiryDate");

            this.Property(t => t.EndOfLevyDate).HasColumnName("EndOfLevyDate");

            this.Property(t => t.InceptionCodeID).HasColumnName("InceptionCodeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.TradeLevyStatusID).HasColumnName("TradeLevyStatusID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ComputationMethodDataID).HasColumnName("ComputationMethodDataID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LevyTrustID).HasColumnName("LevyTrustID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ParagraphTypeID).HasColumnName("ParagraphTypeID").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 