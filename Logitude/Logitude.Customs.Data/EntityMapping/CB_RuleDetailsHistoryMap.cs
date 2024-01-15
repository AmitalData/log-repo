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
 
    public class CB_RuleDetailsHistoryMap : EntityTypeConfiguration<CB_RuleDetailsHistory>
    {
	    string dbms;
        public CB_RuleDetailsHistoryMap()
        { 
			  this.ToTable("CB_RuleDetailsHistorys", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.RuleID).HasColumnName("RuleID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Rules).HasColumnName("Rules").IsMaxLength().IsUnicode(false);

            this.Property(t => t.EnglishRules).HasColumnName("EnglishRules").IsMaxLength().IsUnicode(false);

            this.Property(t => t.OrderinalPostion).HasColumnName("OrderinalPostion");

            this.Property(t => t.Parent_RuleDetailsHistoryID).HasColumnName("Parent_RuleDetailsHistoryID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");

            this.Property(t => t.RulesRTF).HasColumnName("RulesRTF").IsMaxLength().IsUnicode(false);

            this.Property(t => t.EnglishRulesRTF).HasColumnName("EnglishRulesRTF").IsMaxLength().IsUnicode(false);
        }
    }
}
	 