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

            this.Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate").IsRequired();

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.SellerId).HasColumnName("SellerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.LastExpirationDate).HasColumnName("LastExpirationDate");

            this.Property(t => t.PriceSteps).HasColumnName("PriceSteps").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.LastStartDate).HasColumnName("LastStartDate");

            this.Property(t => t.LastVersion).HasColumnName("LastVersion");

            this.Property(t => t.ContractNumber).HasColumnName("ContractNumber");

            this.Property(t => t.TariffNumber).HasColumnName("TariffNumber").HasMaxLength(20).IsUnicode(false);
        }
    }
}
	 