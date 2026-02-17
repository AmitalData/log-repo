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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class QuestionnaireQuestionMap : EntityTypeConfiguration<QuestionnaireQuestion>
    {
	    string dbms;
        public QuestionnaireQuestionMap()
        { 
				this.ToTable("QuestionnaireQuestions");
		
		    this.HasKey(t => new { t.QuestioneerId, t.VersionNumber, t.QuestionNumber });
	 
            this.Property(t => t.QuestioneerId).HasColumnName("QuestioneerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VersionNumber).HasColumnName("VersionNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.QuestionNumber).HasColumnName("QuestionNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Question).HasColumnName("Question").IsRequired().HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.QuestionTypeCode).HasColumnName("QuestionTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsMandatory).HasColumnName("IsMandatory").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PickListCode).HasColumnName("PickListCode").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.IsAddOther).HasColumnName("IsAddOther").IsRequired();
        }
    }
}
	 