
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
   
   public partial class QuoteOPSaleChargeDataMapping: IMapping<QuoteOPSaleChargePM, QuoteOPSaleCharge>,IMappingEncodeBase64NVARCHARFields<QuoteOPSaleChargePM>
   {
          public enum POCOPropertyNames
          { 
		     None, 
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ChargesTypeId, 
	         CurrencyId, 
	         SaleExchangeRate, 
	         MarkUpTypeCode, 
	         MarkUpValue, 
	         FixedAmountCode, 
	         ForeignAmountFixed, 
	         LocalAmountFixed, 
	         IsAllIN, 
	         Notes, 
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
	         CostMinAmount, 
	         SaleMinAmount, 
	         PriceBreaks, 
	         VatTypeId, 
	         VatPercentage, 
	         VatTypeName, 
	         VatAmount, 
	         UOMPercentage, 
	         SaleMaxAmount, 
	         Id, 
	         Tenant, 
	         QuoteId, 
	         ChargesTypeCode, 
	         ChargesTypeName, 
	         ChargesTypeDescription, 
	         ChargesGroupCode, 
	         ChargesTypeLocalName, 
	         CurrencyCode, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         ValueDate, 
	         QuoteTypeCode, 
	         ContainerType1MarkUpTypeCode, 
	         ContainerType2MarkUpTypeCode, 
	         ContainerType3MarkUpTypeCode, 
	         ContainerType4MarkUpTypeCode, 
	         ContainerType5MarkUpTypeCode, 
	         ContainerType1MarkUpValue, 
	         ContainerType2MarkUpValue, 
	         ContainerType3MarkUpValue, 
	         ContainerType4MarkUpValue, 
	         ContainerType5MarkUpValue, 
	         SaleMeasurementCode, 
	         SaleMeasurementShortName, 
	         SaleMeasurementLocalName, 
	         SaleUnitPriceInSaleCurrency, 
	         SaleUnitPrice1InSaleCurrency, 
	         SaleUnitPrice2InSaleCurrency, 
	         SaleUnitPrice3InSaleCurrency, 
	         SaleUnitPrice4InSaleCurrency, 
	         SaleUnitPrice5InSaleCurrency, 
	         SaleAmountInSaleCurrency, 
	         CostMaxAmount, 
	         MarkUpText, 
	         ContainerType1MarkUpText, 
	         ContainerType2MarkUpText, 
	         ContainerType3MarkUpText, 
	         ContainerType4MarkUpText, 
	         ContainerType5MarkUpText, 
	         SalesWithVATAmount, 
	         IsRegionalTax,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPSaleChargePM entityPM, QuoteOPSaleCharge entityPOCO)
        {
			 }

		public void POCOToPM(QuoteOPSaleChargePM entityPM, QuoteOPSaleCharge entityPOCO)
        {
			 
		}

		public void PMToOldPM(QuoteOPSaleChargePM entityPM, QuoteOPSaleChargePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPSaleChargePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
	 