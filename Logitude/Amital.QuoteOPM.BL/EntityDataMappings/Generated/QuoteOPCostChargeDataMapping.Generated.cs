
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
   
   public partial class QuoteOPCostChargeDataMapping: IMapping<QuoteOPCostChargePM, QuoteOPCostCharge>,IMappingEncodeBase64NVARCHARFields<QuoteOPCostChargePM>
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
	         VatTypeId, 
	         VatPercentage, 
	         VatTypeName, 
	         UOMPercentage, 
	         CostMinAmount, 
	         CostMaxAmount, 
	         Id, 
	         Tenant, 
	         QuoteId, 
	         ChargesTypeCode, 
	         ChargesTypeName, 
	         ChargesGroupCode, 
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
	         CostMeasurementCode, 
	         CostMeasurementShortName, 
	         SaleMinAmount, 
	         SaleMaxAmount, 
	         MarkUpText, 
	         ContainerType1MarkUpText, 
	         ContainerType2MarkUpText, 
	         ContainerType3MarkUpText, 
	         ContainerType4MarkUpText, 
	         ContainerType5MarkUpText, 
	         IsRegionalTax,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPCostChargePM entityPM, QuoteOPCostCharge entityPOCO)
        {
			 }

		public void POCOToPM(QuoteOPCostChargePM entityPM, QuoteOPCostCharge entityPOCO)
        {
			 
		}

		public void PMToOldPM(QuoteOPCostChargePM entityPM, QuoteOPCostChargePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPCostChargePM entityPM)
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
	 