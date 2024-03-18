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
 
    public class CB_TradeAgreementMap : EntityTypeConfiguration<CB_TradeAgreement>
    {
	    string dbms;
        public CB_TradeAgreementMap()
        { 
			  this.ToTable("CB_TradeAgreements", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.AdditionName).HasColumnName("AdditionName").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.CountryGroupID).HasColumnName("CountryGroupID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.TradeAgreementAbbreviation).HasColumnName("TradeAgreementAbbreviation").HasMaxLength(255).IsUnicode(true);
        }
    }
}
	 