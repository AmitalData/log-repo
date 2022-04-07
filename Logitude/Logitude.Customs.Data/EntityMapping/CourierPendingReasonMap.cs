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
 
    public class CourierPendingReasonMap : EntityTypeConfiguration<CourierPendingReason>
    {
	    string dbms;
        public CourierPendingReasonMap()
        { 
			  this.ToTable("CourierPendingReasons", "Customs");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.ErrorPlace).HasColumnName("ErrorPlace").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.UnifreightStatusCode).HasColumnName("UnifreightStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.MamanSuspendedCode).HasColumnName("MamanSuspendedCode").HasMaxLength(4).IsUnicode(false);
        }
    }
}
	 