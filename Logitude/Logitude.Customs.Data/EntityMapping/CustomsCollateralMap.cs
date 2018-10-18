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
 
    public class CustomsCollateralMap : EntityTypeConfiguration<CustomsCollateral>
    {
	    string dbms;
        public CustomsCollateralMap()
        { 
			  this.ToTable("CustomsCollaterals", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CollateralRequestNumber).HasColumnName("CollateralRequestNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.RequestValidityDate).HasColumnName("RequestValidityDate");

            this.Property(t => t.CollateralValidityDate).HasColumnName("CollateralValidityDate");

            this.Property(t => t.CollateralRequestStatusCode).HasColumnName("CollateralRequestStatusCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.RequestedCollateralTypeCode).HasColumnName("RequestedCollateralTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.OrganizationUnitTypeCode).HasColumnName("OrganizationUnitTypeCode").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.CustomsHouseTypeCode).HasColumnName("CustomsHouseTypeCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.WorkerName).HasColumnName("WorkerName").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.Remarks).HasColumnName("Remarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.FileNo).HasColumnName("FileNo").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CustomsEntityTypeCode).HasColumnName("CustomsEntityTypeCode").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.EntityIdKey1).HasColumnName("EntityIdKey1").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.EntityIdKey2).HasColumnName("EntityIdKey2").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.EntityIdKey3).HasColumnName("EntityIdKey3").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.IncludingThirdPartyGuarantee).HasColumnName("IncludingThirdPartyGuarantee");

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 