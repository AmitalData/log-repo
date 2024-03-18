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
 
    public class CB_RegularityRequiredCertificateMap : EntityTypeConfiguration<CB_RegularityRequiredCertificate>
    {
	    string dbms;
        public CB_RegularityRequiredCertificateMap()
        { 
		
     dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
    if (dbms == "oracle")
    {
	  this.ToTable("CB_RegularityRequiredCertifica", "Customs");
	}
    else
    {
	  this.ToTable("CB_RegularityRequiredCertificates", "Customs");
	}

		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RegularityInceptionID).HasColumnName("RegularityInceptionID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConfirmationTypeID).HasColumnName("ConfirmationTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Number).HasColumnName("Number");

            this.Property(t => t.TextualCondition).HasColumnName("TextualCondition").IsMaxLength().IsUnicode(false);

            this.Property(t => t.TrNumber).HasColumnName("TrNumber");

            this.Property(t => t.AuthorityID).HasColumnName("AuthorityID").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 