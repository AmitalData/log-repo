
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPChargeDataMapping: IMapping<QuoteOPChargePM, QuoteOPCharge>,IMappingEncodeBase64NVARCHARFields<QuoteOPChargePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteId, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         ValueDate, 
	         ContainerType1MarkUpValue, 
	         ContainerType2MarkUpValue, 
	         ContainerType3MarkUpValue, 
	         ContainerType4MarkUpValue, 
	         ContainerType5MarkUpValue, 
	         ContainerType1MarkUpTypeCode, 
	         ContainerType2MarkUpTypeCode, 
	         ContainerType3MarkUpTypeCode, 
	         ContainerType4MarkUpTypeCode, 
	         ContainerType5MarkUpTypeCode, 
	         CostExchangeRate, 
	         CostCurrencyId, 
	         CostIsFixedRate, 
	         ChargesTypeId, 
	         VendorId, 
	         SaleCurrencyId, 
	         SaleExchangeRate, 
	         MarkUpTypeCode, 
	         MarkUpValue, 
	         IsAllIN, 
	         Notes, 
	         CostMeasurementId, 
	         CostQuantity, 
	         CostUnitPrice, 
	         CostTotalAmount, 
	         CostTotalAmountLocal, 
	         CostContainerType1UnitPrice, 
	         CostContainerType2UnitPrice, 
	         CostContainerType3UnitPrice, 
	         CostContainerType4UnitPrice, 
	         CostContainerType5UnitPrice, 
	         SaleMeasurementId, 
	         SaleQuantity, 
	         SaleUnitPrice, 
	         SaleTotalAmount, 
	         SaleTotalAmountLocal, 
	         SaleContainerType1UnitPrice, 
	         SaleContainerType2UnitPrice, 
	         SaleContainerType3UnitPrice, 
	         SaleContainerType4UnitPrice, 
	         SaleContainerType5UnitPrice, 
	         CostMaxAmount, 
	         CostMinAmount, 
	         SaleMaxAmount, 
	         SaleMinAmount, 
	         IsChargeBySteps, 
	         SaleIsFixedRate, 
	         CostAmountInSaleCurrency, 
	         VatTypeId, 
	         VatPercentage, 
	         SaleUnitPriceInSaleCurrency, 
	         SaleUnitPrice1InSaleCurrency, 
	         SaleUnitPrice2InSaleCurrency, 
	         SaleUnitPrice3InSaleCurrency, 
	         SaleUnitPrice4InSaleCurrency, 
	         SaleUnitPrice5InSaleCurrency, 
	         SaleAmountInSaleCurrency, 
	         IsCostAllIn, 
	         TariffId, 
	         TariffNumber, 
	         TariffVersion, 
	         IsRegionalTax, 
	         TariffLineId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteId, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         ValueDate, 
	         ContainerType1MarkUpValue, 
	         ContainerType2MarkUpValue, 
	         ContainerType3MarkUpValue, 
	         ContainerType4MarkUpValue, 
	         ContainerType5MarkUpValue, 
	         ContainerType1MarkUpTypeCode, 
	         ContainerType2MarkUpTypeCode, 
	         ContainerType3MarkUpTypeCode, 
	         ContainerType4MarkUpTypeCode, 
	         ContainerType5MarkUpTypeCode, 
	         CostExchangeRate, 
	         CostCurrencyId, 
	         CostIsFixedRate, 
	         ChargesTypeId, 
	         VendorId, 
	         SaleCurrencyId, 
	         SaleExchangeRate, 
	         MarkUpTypeCode, 
	         MarkUpValue, 
	         IsAllIN, 
	         Notes, 
	         CostMeasurementId, 
	         CostQuantity, 
	         CostUnitPrice, 
	         CostTotalAmount, 
	         CostTotalAmountLocal, 
	         CostContainerType1UnitPrice, 
	         CostContainerType2UnitPrice, 
	         CostContainerType3UnitPrice, 
	         CostContainerType4UnitPrice, 
	         CostContainerType5UnitPrice, 
	         SaleMeasurementId, 
	         SaleQuantity, 
	         SaleUnitPrice, 
	         SaleTotalAmount, 
	         SaleTotalAmountLocal, 
	         SaleContainerType1UnitPrice, 
	         SaleContainerType2UnitPrice, 
	         SaleContainerType3UnitPrice, 
	         SaleContainerType4UnitPrice, 
	         SaleContainerType5UnitPrice, 
	         CostMaxAmount, 
	         CostMinAmount, 
	         SaleMaxAmount, 
	         SaleMinAmount, 
	         IsChargeBySteps, 
	         SaleIsFixedRate, 
	         ViewOrder, 
	         QuoteTypeCode, 
	         ChargesTypeCode, 
	         ChargesTypeName, 
	         ChargesTypeLocalName, 
	         ChargesGroupCode, 
	         VendorName, 
	         CostMeasurementCode, 
	         CostMeasurementShortName, 
	         SaleMeasurementCode, 
	         SaleMeasurementShortName, 
	         CostCurrencyCode, 
	         SaleCurrencyCode, 
	         CostUnitPriceInSaleCurrency, 
	         CostUnitPrice1InSaleCurrency, 
	         CostUnitPrice2InSaleCurrency, 
	         CostUnitPrice3InSaleCurrency, 
	         CostUnitPrice4InSaleCurrency, 
	         CostUnitPrice5InSaleCurrency, 
	         CostAmountInSaleCurrency, 
	         VatTypeId, 
	         VatPercentage, 
	         VatTypeName, 
	         VatAmount, 
	         SaleUnitPriceInSaleCurrency, 
	         SaleUnitPrice1InSaleCurrency, 
	         SaleUnitPrice2InSaleCurrency, 
	         SaleUnitPrice3InSaleCurrency, 
	         SaleUnitPrice4InSaleCurrency, 
	         SaleUnitPrice5InSaleCurrency, 
	         SaleAmountInSaleCurrency, 
	         ChargesTypeDescription, 
	         ExternalVATCard, 
	         ExternalTAXItemId, 
	         VatIsMultiPercentage, 
	         IsBackToBack, 
	         MarkUpText, 
	         ContainerType1MarkUpText, 
	         ContainerType2MarkUpText, 
	         ContainerType3MarkUpText, 
	         ContainerType4MarkUpText, 
	         ContainerType5MarkUpText, 
	         IsCostAllIn, 
	         TariffId, 
	         TariffNumber, 
	         TariffVersion, 
	         VendorCode, 
	         HasPickup, 
	         HasDelivery, 
	         IsRegionalTax, 
	         CostRatio, 
	         SaleRatio, 
	         TariffLineId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
				entityPOCO.QuoteId = entityPM.QuoteId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
				entityPOCO.ValueDate = entityPM.ValueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType1MarkUpValue))
            {
				entityPOCO.ContainerType1MarkUpValue = entityPM.ContainerType1MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType2MarkUpValue))
            {
				entityPOCO.ContainerType2MarkUpValue = entityPM.ContainerType2MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType3MarkUpValue))
            {
				entityPOCO.ContainerType3MarkUpValue = entityPM.ContainerType3MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType4MarkUpValue))
            {
				entityPOCO.ContainerType4MarkUpValue = entityPM.ContainerType4MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType5MarkUpValue))
            {
				entityPOCO.ContainerType5MarkUpValue = entityPM.ContainerType5MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType1MarkUpTypeCode))
            {
				entityPOCO.ContainerType1MarkUpTypeCode = entityPM.ContainerType1MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType2MarkUpTypeCode))
            {
				entityPOCO.ContainerType2MarkUpTypeCode = entityPM.ContainerType2MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType3MarkUpTypeCode))
            {
				entityPOCO.ContainerType3MarkUpTypeCode = entityPM.ContainerType3MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType4MarkUpTypeCode))
            {
				entityPOCO.ContainerType4MarkUpTypeCode = entityPM.ContainerType4MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType5MarkUpTypeCode))
            {
				entityPOCO.ContainerType5MarkUpTypeCode = entityPM.ContainerType5MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostExchangeRate))
            {
				entityPOCO.CostExchangeRate = entityPM.CostExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostCurrencyId))
            {
				entityPOCO.CostCurrencyId = entityPM.CostCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostIsFixedRate))
            {
				entityPOCO.CostIsFixedRate = entityPM.CostIsFixedRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargesTypeId))
            {
				entityPOCO.ChargesTypeId = entityPM.ChargesTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
				entityPOCO.VendorId = entityPM.VendorId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleCurrencyId))
            {
				entityPOCO.SaleCurrencyId = entityPM.SaleCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleExchangeRate))
            {
				entityPOCO.SaleExchangeRate = entityPM.SaleExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkUpTypeCode))
            {
				entityPOCO.MarkUpTypeCode = entityPM.MarkUpTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkUpValue))
            {
				entityPOCO.MarkUpValue = entityPM.MarkUpValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAllIN))
            {
				entityPOCO.IsAllIN = entityPM.IsAllIN;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMeasurementId))
            {
				entityPOCO.CostMeasurementId = entityPM.CostMeasurementId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostQuantity))
            {
				entityPOCO.CostQuantity = entityPM.CostQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostUnitPrice))
            {
				entityPOCO.CostUnitPrice = entityPM.CostUnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostTotalAmount))
            {
				entityPOCO.CostTotalAmount = entityPM.CostTotalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostTotalAmountLocal))
            {
				entityPOCO.CostTotalAmountLocal = entityPM.CostTotalAmountLocal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType1UnitPrice))
            {
				entityPOCO.CostContainerType1UnitPrice = entityPM.CostContainerType1UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType2UnitPrice))
            {
				entityPOCO.CostContainerType2UnitPrice = entityPM.CostContainerType2UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType3UnitPrice))
            {
				entityPOCO.CostContainerType3UnitPrice = entityPM.CostContainerType3UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType4UnitPrice))
            {
				entityPOCO.CostContainerType4UnitPrice = entityPM.CostContainerType4UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType5UnitPrice))
            {
				entityPOCO.CostContainerType5UnitPrice = entityPM.CostContainerType5UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMeasurementId))
            {
				entityPOCO.SaleMeasurementId = entityPM.SaleMeasurementId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleQuantity))
            {
				entityPOCO.SaleQuantity = entityPM.SaleQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice))
            {
				entityPOCO.SaleUnitPrice = entityPM.SaleUnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleTotalAmount))
            {
				entityPOCO.SaleTotalAmount = entityPM.SaleTotalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleTotalAmountLocal))
            {
				entityPOCO.SaleTotalAmountLocal = entityPM.SaleTotalAmountLocal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType1UnitPrice))
            {
				entityPOCO.SaleContainerType1UnitPrice = entityPM.SaleContainerType1UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType2UnitPrice))
            {
				entityPOCO.SaleContainerType2UnitPrice = entityPM.SaleContainerType2UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType3UnitPrice))
            {
				entityPOCO.SaleContainerType3UnitPrice = entityPM.SaleContainerType3UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType4UnitPrice))
            {
				entityPOCO.SaleContainerType4UnitPrice = entityPM.SaleContainerType4UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType5UnitPrice))
            {
				entityPOCO.SaleContainerType5UnitPrice = entityPM.SaleContainerType5UnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMaxAmount))
            {
				entityPOCO.CostMaxAmount = entityPM.CostMaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMinAmount))
            {
				entityPOCO.CostMinAmount = entityPM.CostMinAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMaxAmount))
            {
				entityPOCO.SaleMaxAmount = entityPM.SaleMaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMinAmount))
            {
				entityPOCO.SaleMinAmount = entityPM.SaleMinAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChargeBySteps))
            {
				entityPOCO.IsChargeBySteps = entityPM.IsChargeBySteps;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleIsFixedRate))
            {
				entityPOCO.SaleIsFixedRate = entityPM.SaleIsFixedRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostAmountInSaleCurrency))
            {
				entityPOCO.CostAmountInSaleCurrency = entityPM.CostAmountInSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatTypeId))
            {
				entityPOCO.VatTypeId = entityPM.VatTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatPercentage))
            {
				entityPOCO.VatPercentage = entityPM.VatPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPriceInSaleCurrency))
            {
				entityPOCO.SaleUnitPriceInSaleCurrency = entityPM.SaleUnitPriceInSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice1InSaleCurrency))
            {
				entityPOCO.SaleUnitPrice1InSaleCurrency = entityPM.SaleUnitPrice1InSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice2InSaleCurrency))
            {
				entityPOCO.SaleUnitPrice2InSaleCurrency = entityPM.SaleUnitPrice2InSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice3InSaleCurrency))
            {
				entityPOCO.SaleUnitPrice3InSaleCurrency = entityPM.SaleUnitPrice3InSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice4InSaleCurrency))
            {
				entityPOCO.SaleUnitPrice4InSaleCurrency = entityPM.SaleUnitPrice4InSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice5InSaleCurrency))
            {
				entityPOCO.SaleUnitPrice5InSaleCurrency = entityPM.SaleUnitPrice5InSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleAmountInSaleCurrency))
            {
				entityPOCO.SaleAmountInSaleCurrency = entityPM.SaleAmountInSaleCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCostAllIn))
            {
				entityPOCO.IsCostAllIn = entityPM.IsCostAllIn;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
				entityPOCO.TariffId = entityPM.TariffId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffNumber))
            {
				entityPOCO.TariffNumber = entityPM.TariffNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffVersion))
            {
				entityPOCO.TariffVersion = entityPM.TariffVersion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRegionalTax))
            {
				entityPOCO.IsRegionalTax = entityPM.IsRegionalTax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffLineId))
            {
				entityPOCO.TariffLineId = entityPM.TariffLineId;
			}
			}

		public void POCOToPM(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteId))
            {
					entityPM.QuoteId = entityPOCO.QuoteId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueDate))
            {
					entityPM.ValueDate = entityPOCO.ValueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType1MarkUpValue))
            {
					entityPM.ContainerType1MarkUpValue = entityPOCO.ContainerType1MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType2MarkUpValue))
            {
					entityPM.ContainerType2MarkUpValue = entityPOCO.ContainerType2MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType3MarkUpValue))
            {
					entityPM.ContainerType3MarkUpValue = entityPOCO.ContainerType3MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType4MarkUpValue))
            {
					entityPM.ContainerType4MarkUpValue = entityPOCO.ContainerType4MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType5MarkUpValue))
            {
					entityPM.ContainerType5MarkUpValue = entityPOCO.ContainerType5MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType1MarkUpTypeCode))
            {
					entityPM.ContainerType1MarkUpTypeCode = entityPOCO.ContainerType1MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType2MarkUpTypeCode))
            {
					entityPM.ContainerType2MarkUpTypeCode = entityPOCO.ContainerType2MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType3MarkUpTypeCode))
            {
					entityPM.ContainerType3MarkUpTypeCode = entityPOCO.ContainerType3MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType4MarkUpTypeCode))
            {
					entityPM.ContainerType4MarkUpTypeCode = entityPOCO.ContainerType4MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerType5MarkUpTypeCode))
            {
					entityPM.ContainerType5MarkUpTypeCode = entityPOCO.ContainerType5MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostExchangeRate))
            {
					entityPM.CostExchangeRate = entityPOCO.CostExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostCurrencyId))
            {
					entityPM.CostCurrencyId = entityPOCO.CostCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostIsFixedRate))
            {
					entityPM.CostIsFixedRate = entityPOCO.CostIsFixedRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargesTypeId))
            {
					entityPM.ChargesTypeId = entityPOCO.ChargesTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorId))
            {
					entityPM.VendorId = entityPOCO.VendorId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleCurrencyId))
            {
					entityPM.SaleCurrencyId = entityPOCO.SaleCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleExchangeRate))
            {
					entityPM.SaleExchangeRate = entityPOCO.SaleExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarkUpTypeCode))
            {
					entityPM.MarkUpTypeCode = entityPOCO.MarkUpTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarkUpValue))
            {
					entityPM.MarkUpValue = entityPOCO.MarkUpValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAllIN))
            {
					entityPM.IsAllIN = entityPOCO.IsAllIN;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostMeasurementId))
            {
					entityPM.CostMeasurementId = entityPOCO.CostMeasurementId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostQuantity))
            {
					entityPM.CostQuantity = entityPOCO.CostQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostUnitPrice))
            {
					entityPM.CostUnitPrice = entityPOCO.CostUnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostTotalAmount))
            {
					entityPM.CostTotalAmount = entityPOCO.CostTotalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostTotalAmountLocal))
            {
					entityPM.CostTotalAmountLocal = entityPOCO.CostTotalAmountLocal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostContainerType1UnitPrice))
            {
					entityPM.CostContainerType1UnitPrice = entityPOCO.CostContainerType1UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostContainerType2UnitPrice))
            {
					entityPM.CostContainerType2UnitPrice = entityPOCO.CostContainerType2UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostContainerType3UnitPrice))
            {
					entityPM.CostContainerType3UnitPrice = entityPOCO.CostContainerType3UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostContainerType4UnitPrice))
            {
					entityPM.CostContainerType4UnitPrice = entityPOCO.CostContainerType4UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostContainerType5UnitPrice))
            {
					entityPM.CostContainerType5UnitPrice = entityPOCO.CostContainerType5UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleMeasurementId))
            {
					entityPM.SaleMeasurementId = entityPOCO.SaleMeasurementId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleQuantity))
            {
					entityPM.SaleQuantity = entityPOCO.SaleQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice))
            {
					entityPM.SaleUnitPrice = entityPOCO.SaleUnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleTotalAmount))
            {
					entityPM.SaleTotalAmount = entityPOCO.SaleTotalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleTotalAmountLocal))
            {
					entityPM.SaleTotalAmountLocal = entityPOCO.SaleTotalAmountLocal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleContainerType1UnitPrice))
            {
					entityPM.SaleContainerType1UnitPrice = entityPOCO.SaleContainerType1UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleContainerType2UnitPrice))
            {
					entityPM.SaleContainerType2UnitPrice = entityPOCO.SaleContainerType2UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleContainerType3UnitPrice))
            {
					entityPM.SaleContainerType3UnitPrice = entityPOCO.SaleContainerType3UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleContainerType4UnitPrice))
            {
					entityPM.SaleContainerType4UnitPrice = entityPOCO.SaleContainerType4UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleContainerType5UnitPrice))
            {
					entityPM.SaleContainerType5UnitPrice = entityPOCO.SaleContainerType5UnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostMaxAmount))
            {
					entityPM.CostMaxAmount = entityPOCO.CostMaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostMinAmount))
            {
					entityPM.CostMinAmount = entityPOCO.CostMinAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleMaxAmount))
            {
					entityPM.SaleMaxAmount = entityPOCO.SaleMaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleMinAmount))
            {
					entityPM.SaleMinAmount = entityPOCO.SaleMinAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsChargeBySteps))
            {
					entityPM.IsChargeBySteps = entityPOCO.IsChargeBySteps;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleIsFixedRate))
            {
					entityPM.SaleIsFixedRate = entityPOCO.SaleIsFixedRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostAmountInSaleCurrency))
            {
					entityPM.CostAmountInSaleCurrency = entityPOCO.CostAmountInSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatTypeId))
            {
					entityPM.VatTypeId = entityPOCO.VatTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatPercentage))
            {
					entityPM.VatPercentage = entityPOCO.VatPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPriceInSaleCurrency))
            {
					entityPM.SaleUnitPriceInSaleCurrency = entityPOCO.SaleUnitPriceInSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice1InSaleCurrency))
            {
					entityPM.SaleUnitPrice1InSaleCurrency = entityPOCO.SaleUnitPrice1InSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice2InSaleCurrency))
            {
					entityPM.SaleUnitPrice2InSaleCurrency = entityPOCO.SaleUnitPrice2InSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice3InSaleCurrency))
            {
					entityPM.SaleUnitPrice3InSaleCurrency = entityPOCO.SaleUnitPrice3InSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice4InSaleCurrency))
            {
					entityPM.SaleUnitPrice4InSaleCurrency = entityPOCO.SaleUnitPrice4InSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice5InSaleCurrency))
            {
					entityPM.SaleUnitPrice5InSaleCurrency = entityPOCO.SaleUnitPrice5InSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleAmountInSaleCurrency))
            {
					entityPM.SaleAmountInSaleCurrency = entityPOCO.SaleAmountInSaleCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCostAllIn))
            {
					entityPM.IsCostAllIn = entityPOCO.IsCostAllIn;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffNumber))
            {
					entityPM.TariffNumber = entityPOCO.TariffNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffVersion))
            {
					entityPM.TariffVersion = entityPOCO.TariffVersion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRegionalTax))
            {
					entityPM.IsRegionalTax = entityPOCO.IsRegionalTax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffLineId))
            {
					entityPM.TariffLineId = entityPOCO.TariffLineId;
            }

		}

		public void PMToOldPM(QuoteOPChargePM entityPM, QuoteOPChargePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
                oldEntityPM.QuoteId = entityPM.QuoteId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
                oldEntityPM.ValueDate = entityPM.ValueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType1MarkUpValue))
            {
                oldEntityPM.ContainerType1MarkUpValue = entityPM.ContainerType1MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType2MarkUpValue))
            {
                oldEntityPM.ContainerType2MarkUpValue = entityPM.ContainerType2MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType3MarkUpValue))
            {
                oldEntityPM.ContainerType3MarkUpValue = entityPM.ContainerType3MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType4MarkUpValue))
            {
                oldEntityPM.ContainerType4MarkUpValue = entityPM.ContainerType4MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType5MarkUpValue))
            {
                oldEntityPM.ContainerType5MarkUpValue = entityPM.ContainerType5MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType1MarkUpTypeCode))
            {
                oldEntityPM.ContainerType1MarkUpTypeCode = entityPM.ContainerType1MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType2MarkUpTypeCode))
            {
                oldEntityPM.ContainerType2MarkUpTypeCode = entityPM.ContainerType2MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType3MarkUpTypeCode))
            {
                oldEntityPM.ContainerType3MarkUpTypeCode = entityPM.ContainerType3MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType4MarkUpTypeCode))
            {
                oldEntityPM.ContainerType4MarkUpTypeCode = entityPM.ContainerType4MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerType5MarkUpTypeCode))
            {
                oldEntityPM.ContainerType5MarkUpTypeCode = entityPM.ContainerType5MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostExchangeRate))
            {
                oldEntityPM.CostExchangeRate = entityPM.CostExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostCurrencyId))
            {
                oldEntityPM.CostCurrencyId = entityPM.CostCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostIsFixedRate))
            {
                oldEntityPM.CostIsFixedRate = entityPM.CostIsFixedRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargesTypeId))
            {
                oldEntityPM.ChargesTypeId = entityPM.ChargesTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
                oldEntityPM.VendorId = entityPM.VendorId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleCurrencyId))
            {
                oldEntityPM.SaleCurrencyId = entityPM.SaleCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleExchangeRate))
            {
                oldEntityPM.SaleExchangeRate = entityPM.SaleExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkUpTypeCode))
            {
                oldEntityPM.MarkUpTypeCode = entityPM.MarkUpTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkUpValue))
            {
                oldEntityPM.MarkUpValue = entityPM.MarkUpValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAllIN))
            {
                oldEntityPM.IsAllIN = entityPM.IsAllIN;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMeasurementId))
            {
                oldEntityPM.CostMeasurementId = entityPM.CostMeasurementId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostQuantity))
            {
                oldEntityPM.CostQuantity = entityPM.CostQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostUnitPrice))
            {
                oldEntityPM.CostUnitPrice = entityPM.CostUnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostTotalAmount))
            {
                oldEntityPM.CostTotalAmount = entityPM.CostTotalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostTotalAmountLocal))
            {
                oldEntityPM.CostTotalAmountLocal = entityPM.CostTotalAmountLocal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType1UnitPrice))
            {
                oldEntityPM.CostContainerType1UnitPrice = entityPM.CostContainerType1UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType2UnitPrice))
            {
                oldEntityPM.CostContainerType2UnitPrice = entityPM.CostContainerType2UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType3UnitPrice))
            {
                oldEntityPM.CostContainerType3UnitPrice = entityPM.CostContainerType3UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType4UnitPrice))
            {
                oldEntityPM.CostContainerType4UnitPrice = entityPM.CostContainerType4UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostContainerType5UnitPrice))
            {
                oldEntityPM.CostContainerType5UnitPrice = entityPM.CostContainerType5UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMeasurementId))
            {
                oldEntityPM.SaleMeasurementId = entityPM.SaleMeasurementId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleQuantity))
            {
                oldEntityPM.SaleQuantity = entityPM.SaleQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice))
            {
                oldEntityPM.SaleUnitPrice = entityPM.SaleUnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleTotalAmount))
            {
                oldEntityPM.SaleTotalAmount = entityPM.SaleTotalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleTotalAmountLocal))
            {
                oldEntityPM.SaleTotalAmountLocal = entityPM.SaleTotalAmountLocal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType1UnitPrice))
            {
                oldEntityPM.SaleContainerType1UnitPrice = entityPM.SaleContainerType1UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType2UnitPrice))
            {
                oldEntityPM.SaleContainerType2UnitPrice = entityPM.SaleContainerType2UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType3UnitPrice))
            {
                oldEntityPM.SaleContainerType3UnitPrice = entityPM.SaleContainerType3UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType4UnitPrice))
            {
                oldEntityPM.SaleContainerType4UnitPrice = entityPM.SaleContainerType4UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleContainerType5UnitPrice))
            {
                oldEntityPM.SaleContainerType5UnitPrice = entityPM.SaleContainerType5UnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMaxAmount))
            {
                oldEntityPM.CostMaxAmount = entityPM.CostMaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostMinAmount))
            {
                oldEntityPM.CostMinAmount = entityPM.CostMinAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMaxAmount))
            {
                oldEntityPM.SaleMaxAmount = entityPM.SaleMaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleMinAmount))
            {
                oldEntityPM.SaleMinAmount = entityPM.SaleMinAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChargeBySteps))
            {
                oldEntityPM.IsChargeBySteps = entityPM.IsChargeBySteps;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleIsFixedRate))
            {
                oldEntityPM.SaleIsFixedRate = entityPM.SaleIsFixedRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostAmountInSaleCurrency))
            {
                oldEntityPM.CostAmountInSaleCurrency = entityPM.CostAmountInSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatTypeId))
            {
                oldEntityPM.VatTypeId = entityPM.VatTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatPercentage))
            {
                oldEntityPM.VatPercentage = entityPM.VatPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPriceInSaleCurrency))
            {
                oldEntityPM.SaleUnitPriceInSaleCurrency = entityPM.SaleUnitPriceInSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice1InSaleCurrency))
            {
                oldEntityPM.SaleUnitPrice1InSaleCurrency = entityPM.SaleUnitPrice1InSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice2InSaleCurrency))
            {
                oldEntityPM.SaleUnitPrice2InSaleCurrency = entityPM.SaleUnitPrice2InSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice3InSaleCurrency))
            {
                oldEntityPM.SaleUnitPrice3InSaleCurrency = entityPM.SaleUnitPrice3InSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice4InSaleCurrency))
            {
                oldEntityPM.SaleUnitPrice4InSaleCurrency = entityPM.SaleUnitPrice4InSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice5InSaleCurrency))
            {
                oldEntityPM.SaleUnitPrice5InSaleCurrency = entityPM.SaleUnitPrice5InSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleAmountInSaleCurrency))
            {
                oldEntityPM.SaleAmountInSaleCurrency = entityPM.SaleAmountInSaleCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCostAllIn))
            {
                oldEntityPM.IsCostAllIn = entityPM.IsCostAllIn;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
                oldEntityPM.TariffId = entityPM.TariffId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffNumber))
            {
                oldEntityPM.TariffNumber = entityPM.TariffNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffVersion))
            {
                oldEntityPM.TariffVersion = entityPM.TariffVersion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRegionalTax))
            {
                oldEntityPM.IsRegionalTax = entityPM.IsRegionalTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffLineId))
            {
                oldEntityPM.TariffLineId = entityPM.TariffLineId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPChargePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
			  
   }
}
	 