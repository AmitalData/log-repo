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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;
 
namespace Logitude.Infrastructure.Data.EntityMapping
{
 
    public class SatisfactionSurveyMap : EntityTypeConfiguration<SatisfactionSurvey>
    {
	    string dbms;
        public SatisfactionSurveyMap()
        { 
				this.ToTable("SatisfactionSurveys");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Rating).HasColumnName("Rating").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.Comments).HasColumnName("Comments").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 