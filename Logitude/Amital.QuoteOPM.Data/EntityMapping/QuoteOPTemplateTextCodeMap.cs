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
 
    public class QuoteOPTemplateTextCodeMap : EntityTypeConfiguration<QuoteOPTemplateTextCode>
    {
	    string dbms;
        public QuoteOPTemplateTextCodeMap()
        { 
				this.ToTable("QuoteOPTemplateTextCodes");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.TextCode).HasColumnName("TextCode").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Area).HasColumnName("Area").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.OriginalEnglishName).HasColumnName("OriginalEnglishName").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.OriginalLocalName).HasColumnName("OriginalLocalName").IsRequired().HasMaxLength(100).IsUnicode(true);
        }
    }
}
	 