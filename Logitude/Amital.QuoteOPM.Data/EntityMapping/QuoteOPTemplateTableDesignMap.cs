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
 
    public class QuoteOPTemplateTableDesignMap : EntityTypeConfiguration<QuoteOPTemplateTableDesign>
    {
	    string dbms;
        public QuoteOPTemplateTableDesignMap()
        { 
				this.ToTable("QuoteOPTemplateTableDesigns");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.BorderTypeCode).HasColumnName("BorderTypeCode").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.BorderColor).HasColumnName("BorderColor").IsRequired().HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.BorderThickness).HasColumnName("BorderThickness");

            this.Property(t => t.HeaderDesignId).HasColumnName("HeaderDesignId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LinesDesignId).HasColumnName("LinesDesignId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GroupByDesignId).HasColumnName("GroupByDesignId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 