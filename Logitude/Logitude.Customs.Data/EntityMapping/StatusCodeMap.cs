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
 
    public class StatusCodeMap : EntityTypeConfiguration<StatusCode>
    {
	    string dbms;
        public StatusCodeMap()
        { 
			  this.ToTable("StatusCodes", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Status_Code).HasColumnName("Status_Code").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.StatusNameHeb).HasColumnName("StatusNameHeb").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.StatusNameEng).HasColumnName("StatusNameEng").HasMaxLength(30).IsUnicode(false);
        }
    }
}
	 