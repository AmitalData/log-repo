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
 
    public class QuoteOPTemplateMap : EntityTypeConfiguration<QuoteOPTemplate>
    {
	    string dbms;
        public QuoteOPTemplateMap()
        { 
				this.ToTable("QuoteOPTemplates");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.HeaderDocId).HasColumnName("HeaderDocId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FooterDocId).HasColumnName("FooterDocId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteOPTemplateSettingId).HasColumnName("QuoteOPTemplateSettingId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.IsTemplate).HasColumnName("IsTemplate").IsRequired();

            this.Property(t => t.OriginalQuoteOPTemplateId).HasColumnName("OriginalQuoteOPTemplateId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(false);

            this.Property(t => t.TemplateTypeCode).HasColumnName("TemplateTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsDefault).HasColumnName("IsDefault");

            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.IsCopiedAtSignup).HasColumnName("IsCopiedAtSignup");

            this.Property(t => t.IsEnabledForCustomers).HasColumnName("IsEnabledForCustomers");
        }
    }
}
	 