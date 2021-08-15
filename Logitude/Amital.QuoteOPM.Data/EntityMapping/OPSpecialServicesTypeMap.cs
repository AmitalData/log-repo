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
 
    public class OPSpecialServicesTypeMap : EntityTypeConfiguration<OPSpecialServicesType>
    {
	    string dbms;
        public OPSpecialServicesTypeMap()
        { 
				this.ToTable("OPSpecialServicesTypes");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(8).IsUnicode(false);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive").IsRequired();

            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");
        }
    }
}
	 