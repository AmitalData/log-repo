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
 
    public class CargoSealIdentifierMap : EntityTypeConfiguration<CargoSealIdentifier>
    {
	    string dbms;
        public CargoSealIdentifierMap()
        { 
			  this.ToTable("CargoSealIdentifiers", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CargoRowNumber).HasColumnName("CargoRowNumber").IsRequired().HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber").HasMaxLength(11).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.ImporterId).HasColumnName("ImporterId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CargoIdentifierTypeCode).HasColumnName("CargoIdentifierTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey1).HasColumnName("CargoIdentifierKey1").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey2).HasColumnName("CargoIdentifierKey2").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey3).HasColumnName("CargoIdentifierKey3").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(1).IsUnicode(false);
        }
    }
}
	 