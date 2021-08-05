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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPTemplateSectionMap : EntityTypeConfiguration<QuoteOPTemplateSection>
    {
	    string dbms;
        public QuoteOPTemplateSectionMap()
        { 
				this.ToTable("QuoteOPTemplateSections");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.IsCancel).HasColumnName("IsCancel");

            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SectionDocId).HasColumnName("SectionDocId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Order).HasColumnName("Order").IsRequired();

            this.Property(t => t.QuoteOPTemplateSectionTypeCode).HasColumnName("QuoteOPTemplateSectionTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);
        }
    }
}
	 