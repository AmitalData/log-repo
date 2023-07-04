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
 
    public class CourierHawbFromExcelMap : EntityTypeConfiguration<CourierHawbFromExcel>
    {
	    string dbms;
        public CourierHawbFromExcelMap()
        { 
			  this.ToTable("CourierHawbFromExcels", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NotFound).HasColumnName("NotFound");

            this.Property(t => t.CourierHawb).HasColumnName("CourierHawb").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage").HasMaxLength(100).IsUnicode(true);
        }
    }
}
	 