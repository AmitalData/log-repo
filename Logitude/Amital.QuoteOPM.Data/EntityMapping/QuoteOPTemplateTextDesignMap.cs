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
 
    public class QuoteOPTemplateTextDesignMap : EntityTypeConfiguration<QuoteOPTemplateTextDesign>
    {
	    string dbms;
        public QuoteOPTemplateTextDesignMap()
        { 
				this.ToTable("QuoteOPTemplateTextDesigns");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.FontSize).HasColumnName("FontSize");

            this.Property(t => t.TextColor).HasColumnName("TextColor").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.FontFamily).HasColumnName("FontFamily").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.BackgroundColor).HasColumnName("BackgroundColor").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.FontWeight).HasColumnName("FontWeight").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Italic).HasColumnName("Italic");

            this.Property(t => t.UnDerLine).HasColumnName("UnDerLine");

            this.Property(t => t.Alignment).HasColumnName("Alignment").HasMaxLength(10).IsUnicode(false);
        }
    }
}
	 