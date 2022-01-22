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
 
    public class TariffMap : EntityTypeConfiguration<Tariff>
    {
	    string dbms;
        public TariffMap()
        { 
				this.ToTable("Tariffs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.SellerId).HasColumnName("SellerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.LastExpirationDate).HasColumnName("LastExpirationDate");

            this.Property(t => t.PriceSteps).HasColumnName("PriceSteps").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.LastStartDate).HasColumnName("LastStartDate");

            this.Property(t => t.LastVersion).HasColumnName("LastVersion");

            this.Property(t => t.ContractNumber).HasColumnName("ContractNumber").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.TariffNumber).HasColumnName("TariffNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge1Id).HasColumnName("Surcharge1Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge2Id).HasColumnName("Surcharge2Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge3Id).HasColumnName("Surcharge3Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge4Id).HasColumnName("Surcharge4Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge5Id).HasColumnName("Surcharge5Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge6Id).HasColumnName("Surcharge6Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge7Id).HasColumnName("Surcharge7Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge8Id).HasColumnName("Surcharge8Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge9Id).HasColumnName("Surcharge9Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge10Id).HasColumnName("Surcharge10Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge1UOM).HasColumnName("Surcharge1UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge2UOM).HasColumnName("Surcharge2UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge3UOM).HasColumnName("Surcharge3UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge4UOM).HasColumnName("Surcharge4UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge5UOM).HasColumnName("Surcharge5UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge6UOM).HasColumnName("Surcharge6UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge7UOM).HasColumnName("Surcharge7UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge8UOM).HasColumnName("Surcharge8UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge9UOM).HasColumnName("Surcharge9UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge10UOM).HasColumnName("Surcharge10UOM").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ContainerType1Id).HasColumnName("ContainerType1Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerType2Id).HasColumnName("ContainerType2Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerType3Id).HasColumnName("ContainerType3Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerType4Id).HasColumnName("ContainerType4Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerType5Id).HasColumnName("ContainerType5Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffProductId).HasColumnName("TariffProductId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SellerPartnerTypeId).HasColumnName("SellerPartnerTypeId").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LastUsedDate).HasColumnName("LastUsedDate");

            this.Property(t => t.FreightChargeId).HasColumnName("FreightChargeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsBrokerId).HasColumnName("CustomsBrokerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsBrokerPartnerTypeId).HasColumnName("CustomsBrokerPartnerTypeId").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.UnitOfMeasurementCode).HasColumnName("UnitOfMeasurementCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 