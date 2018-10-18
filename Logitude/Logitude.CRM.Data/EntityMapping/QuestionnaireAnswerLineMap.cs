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
 
    public class QuestionnaireAnswerLineMap : EntityTypeConfiguration<QuestionnaireAnswerLine>
    {
	    string dbms;
        public QuestionnaireAnswerLineMap()
        { 
				this.ToTable("QuestionnaireAnswerLines");
		
		    this.HasKey(t => new { t.QuestionnaireAnswerId, t.QuestionNumber });
	 
            this.Property(t => t.QuestionnaireAnswerId).HasColumnName("QuestionnaireAnswerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.QuestionNumber).HasColumnName("QuestionNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.AnswerValue).HasColumnName("AnswerValue").IsRequired().HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 