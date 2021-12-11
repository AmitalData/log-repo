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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPChargeMap : EntityTypeConfiguration<QuoteOPCharge>
    {
	    string dbms;
        public QuoteOPChargeMap()
        { 
				this.ToTable("QuoteOPCharges");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.QuoteOPId).HasColumnName("QuoteOPId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.ValueDate).HasColumnName("ValueDate");

            this.Property(t => t.ContainerType1MarkUpValue).HasColumnName("ContainerType1MarkUpValue");

            this.Property(t => t.ContainerType2MarkUpValue).HasColumnName("ContainerType2MarkUpValue");

            this.Property(t => t.ContainerType3MarkUpValue).HasColumnName("ContainerType3MarkUpValue");

            this.Property(t => t.ContainerType4MarkUpValue).HasColumnName("ContainerType4MarkUpValue");

            this.Property(t => t.ContainerType5MarkUpValue).HasColumnName("ContainerType5MarkUpValue");

            this.Property(t => t.ContainerType1MarkUpTypeCode).HasColumnName("ContainerType1MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ContainerType2MarkUpTypeCode).HasColumnName("ContainerType2MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ContainerType3MarkUpTypeCode).HasColumnName("ContainerType3MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ContainerType4MarkUpTypeCode).HasColumnName("ContainerType4MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ContainerType5MarkUpTypeCode).HasColumnName("ContainerType5MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CostExchangeRate).HasColumnName("CostExchangeRate").IsRequired();

            this.Property(t => t.CostCurrencyId).HasColumnName("CostCurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CostIsFixedRate).HasColumnName("CostIsFixedRate");

            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VendorId).HasColumnName("VendorId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SaleCurrencyId).HasColumnName("SaleCurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SaleExchangeRate).HasColumnName("SaleExchangeRate");

            this.Property(t => t.MarkUpTypeCode).HasColumnName("MarkUpTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.MarkUpValue).HasColumnName("MarkUpValue");

            this.Property(t => t.IsAllIN).HasColumnName("IsAllIN");

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.CostMeasurementId).HasColumnName("CostMeasurementId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CostQuantity).HasColumnName("CostQuantity");

            this.Property(t => t.CostUnitPrice).HasColumnName("CostUnitPrice");

            this.Property(t => t.CostTotalAmount).HasColumnName("CostTotalAmount");

            this.Property(t => t.CostTotalAmountLocal).HasColumnName("CostTotalAmountLocal");

            this.Property(t => t.CostContainerType1UnitPrice).HasColumnName("CostContainerType1UnitPrice");

            this.Property(t => t.CostContainerType2UnitPrice).HasColumnName("CostContainerType2UnitPrice");

            this.Property(t => t.CostContainerType3UnitPrice).HasColumnName("CostContainerType3UnitPrice");

            this.Property(t => t.CostContainerType4UnitPrice).HasColumnName("CostContainerType4UnitPrice");

            this.Property(t => t.CostContainerType5UnitPrice).HasColumnName("CostContainerType5UnitPrice");

            this.Property(t => t.SaleMeasurementId).HasColumnName("SaleMeasurementId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SaleQuantity).HasColumnName("SaleQuantity");

            this.Property(t => t.SaleUnitPrice).HasColumnName("SaleUnitPrice");

            this.Property(t => t.SaleTotalAmount).HasColumnName("SaleTotalAmount");

            this.Property(t => t.SaleTotalAmountLocal).HasColumnName("SaleTotalAmountLocal");

            this.Property(t => t.SaleContainerType1UnitPrice).HasColumnName("SaleContainerType1UnitPrice");

            this.Property(t => t.SaleContainerType2UnitPrice).HasColumnName("SaleContainerType2UnitPrice");

            this.Property(t => t.SaleContainerType3UnitPrice).HasColumnName("SaleContainerType3UnitPrice");

            this.Property(t => t.SaleContainerType4UnitPrice).HasColumnName("SaleContainerType4UnitPrice");

            this.Property(t => t.SaleContainerType5UnitPrice).HasColumnName("SaleContainerType5UnitPrice");

            this.Property(t => t.CostMaxAmount).HasColumnName("CostMaxAmount");

            this.Property(t => t.CostMinAmount).HasColumnName("CostMinAmount");

            this.Property(t => t.SaleMaxAmount).HasColumnName("SaleMaxAmount");

            this.Property(t => t.SaleMinAmount).HasColumnName("SaleMinAmount");

            this.Property(t => t.IsChargeBySteps).HasColumnName("IsChargeBySteps");

            this.Property(t => t.SaleIsFixedRate).HasColumnName("SaleIsFixedRate");

            this.Property(t => t.CostAmountInSaleCurrency).HasColumnName("CostAmountInSaleCurrency");

            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VatPercentage).HasColumnName("VatPercentage");

            this.Property(t => t.SaleUnitPriceInSaleCurrency).HasColumnName("SaleUnitPriceInSaleCurrency");

            this.Property(t => t.SaleUnitPrice1InSaleCurrency).HasColumnName("SaleUnitPrice1InSaleCurrency");

            this.Property(t => t.SaleUnitPrice2InSaleCurrency).HasColumnName("SaleUnitPrice2InSaleCurrency");

            this.Property(t => t.SaleUnitPrice3InSaleCurrency).HasColumnName("SaleUnitPrice3InSaleCurrency");

            this.Property(t => t.SaleUnitPrice4InSaleCurrency).HasColumnName("SaleUnitPrice4InSaleCurrency");

            this.Property(t => t.SaleUnitPrice5InSaleCurrency).HasColumnName("SaleUnitPrice5InSaleCurrency");

            this.Property(t => t.SaleAmountInSaleCurrency).HasColumnName("SaleAmountInSaleCurrency");

            this.Property(t => t.IsCostAllIn).HasColumnName("IsCostAllIn");

            this.Property(t => t.TariffId).HasColumnName("TariffId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffNumber).HasColumnName("TariffNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.TariffVersion).HasColumnName("TariffVersion");

            this.Property(t => t.IsRegionalTax).HasColumnName("IsRegionalTax");

            this.Property(t => t.TariffLineId).HasColumnName("TariffLineId").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.TariffCostNo).HasColumnName("TariffCostNo").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffSaleNo).HasColumnName("TariffSaleNo").HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.TariffCostXML).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.TariffCostXML).HasMaxLength(5000);
			}


            this.Property(t => t.TariffCostXML).HasColumnName("TariffCostXML").IsUnicode(true);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.TariffSaleXML).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.TariffSaleXML).HasMaxLength(5000);
			}


            this.Property(t => t.TariffSaleXML).HasColumnName("TariffSaleXML").IsUnicode(true);
        }
    }
}
	 