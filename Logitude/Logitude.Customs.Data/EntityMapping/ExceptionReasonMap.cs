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
 
    public class ExceptionReasonMap : EntityTypeConfiguration<ExceptionReason>
    {
	    string dbms;
        public ExceptionReasonMap()
        { 
			  this.ToTable("ExceptionReasons", "Customs");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.IsActive).HasColumnName("IsActive");

            this.Property(t => t.UnifreightStatusCode).HasColumnName("UnifreightStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 