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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data;
 
namespace Logitude.TariffModule.Data.EntityMapping
{
 
    public class TariffVersionUploadedExcelMap : EntityTypeConfiguration<TariffVersionUploadedExcel>
    {
	    string dbms;
        public TariffVersionUploadedExcelMap()
        { 
				this.ToTable("TariffVersionUploadedExcels");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.UploadDate).HasColumnName("UploadDate").IsRequired();

            this.Property(t => t.UploadedByUserId).HasColumnName("UploadedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffId).HasColumnName("TariffId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Version).HasColumnName("Version").IsRequired();

            this.Property(t => t.DocumentId).HasColumnName("DocumentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NumberOfLines).HasColumnName("NumberOfLines").IsRequired();
        }
    }
}
	 