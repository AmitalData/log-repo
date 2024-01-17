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
 
    public class CB_AdditionRulesDetailsHistoryMap : EntityTypeConfiguration<CB_AdditionRulesDetailsHistory>
    {
	    string dbms;
        public CB_AdditionRulesDetailsHistoryMap()
        { 
		
     dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
    if (dbms == "oracle")
    {
	  this.ToTable("CB_AdditionRulesDetailsHistory", "Customs");
	}
    else
    {
	  this.ToTable("CB_AdditionRulesDetailsHistorys", "Customs");
	}

		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Rules).HasColumnName("Rules").IsMaxLength().IsUnicode(false);

            this.Property(t => t.EnglishRules).HasColumnName("EnglishRules").IsMaxLength().IsUnicode(false);

            this.Property(t => t.RulesRTF).HasColumnName("RulesRTF").IsMaxLength().IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");

            this.Property(t => t.CustomsBookAdditionID).HasColumnName("CustomsBookAdditionID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EnglishRulesRTF).HasColumnName("EnglishRulesRTF").IsMaxLength().IsUnicode(false);
        }
    }
}
	 