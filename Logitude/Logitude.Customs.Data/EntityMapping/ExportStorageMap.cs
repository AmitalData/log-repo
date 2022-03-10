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
 
    public class ExportStorageMap : EntityTypeConfiguration<ExportStorage>
    {
	    string dbms;
        public ExportStorageMap()
        { 
			  this.ToTable("ExportStorages", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExportFileNo).HasColumnName("ExportFileNo").HasMaxLength(20).IsUnicode(true);

            this.Property(t => t.StorageStatus).HasColumnName("StorageStatus").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.OpenDate).HasColumnName("OpenDate");

            this.Property(t => t.CargoType).HasColumnName("CargoType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CustomsStatus).HasColumnName("CustomsStatus").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ExporterID).HasColumnName("ExporterID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipCode).HasColumnName("ShipCode").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.FirstCargoID).HasColumnName("FirstCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.SecondCargoID).HasColumnName("SecondCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ThirdCargoID).HasColumnName("ThirdCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.StorErrorXML).HasColumnName("StorErrorXML").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.StorageNo).HasColumnName("StorageNo").HasMaxLength(20).IsUnicode(true);

            this.Property(t => t.ExportDealIdentification).HasColumnName("ExportDealIdentification").HasMaxLength(16).IsUnicode(true);
        }
    }
}
	 