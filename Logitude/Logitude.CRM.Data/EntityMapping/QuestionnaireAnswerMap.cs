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
 
    public class QuestionnaireAnswerMap : EntityTypeConfiguration<QuestionnaireAnswer>
    {
	    string dbms;
        public QuestionnaireAnswerMap()
        { 
				this.ToTable("QuestionnaireAnswers");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.QuestioneerId).HasColumnName("QuestioneerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VersionNumber).HasColumnName("VersionNumber").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.HasTwoColumn).HasColumnName("HasTwoColumn").IsRequired();
        }
    }
}
	 