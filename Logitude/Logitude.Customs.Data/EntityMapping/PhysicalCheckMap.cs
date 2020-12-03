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
 
    public class PhysicalCheckMap : EntityTypeConfiguration<PhysicalCheck>
    {
	    string dbms;
        public PhysicalCheckMap()
        { 
			  this.ToTable("PhysicalChecks", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StorageSiteCode).HasColumnName("StorageSiteCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.CheckSiteCode).HasColumnName("CheckSiteCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.QueueTypeCode).HasColumnName("QueueTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.OperationCode).HasColumnName("OperationCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CheckId).HasColumnName("CheckId").IsRequired().HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ContainerNubmer).HasColumnName("ContainerNubmer").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OpenDate).HasColumnName("OpenDate").IsRequired();

            this.Property(t => t.LimitDate).HasColumnName("LimitDate");

            this.Property(t => t.CargoIdentifierKey1).HasColumnName("CargoIdentifierKey1").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey2).HasColumnName("CargoIdentifierKey2").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey3).HasColumnName("CargoIdentifierKey3").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.RowNumber).HasColumnName("RowNumber").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CheckEssence).HasColumnName("CheckEssence").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.IsClosed).HasColumnName("IsClosed").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.StatusMessageCode).HasColumnName("StatusMessageCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.InitiatorTypeCode).HasColumnName("InitiatorTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ImporterNumber).HasColumnName("ImporterNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.CargoIdentifierTypeCode).HasColumnName("CargoIdentifierTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.IsComprehensiveCheck).HasColumnName("IsComprehensiveCheck");

            this.Property(t => t.CheckTypeCode).HasColumnName("CheckTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NoEscortRequired).HasColumnName("NoEscortRequired");

            this.Property(t => t.VehicleChassisNumber).HasColumnName("VehicleChassisNumber").HasMaxLength(20).IsUnicode(false);
        }
    }
}
	 