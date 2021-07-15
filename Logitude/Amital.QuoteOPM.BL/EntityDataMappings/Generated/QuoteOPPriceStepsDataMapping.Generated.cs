
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
   
   public partial class QuoteOPPriceStepsDataMapping: IMapping<QuoteOPPriceStepsPM, QuoteOPPriceSteps>,IMappingEncodeBase64NVARCHARFields<QuoteOPPriceStepsPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         QuoteOPChargeId, 
	         Step, 
	         CostUnitPrice, 
	         SaleUnitPrice, 
	         MarkupValue,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         QuoteOPChargeId, 
	         Step, 
	         CostUnitPrice, 
	         SaleUnitPrice, 
	         MarkupValue, 
	         MeasurementUnit,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPPriceStepsPM entityPM, QuoteOPPriceSteps entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPId))
            {
				entityPOCO.QuoteOPId = entityPM.QuoteOPId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPChargeId))
            {
				entityPOCO.QuoteOPChargeId = entityPM.QuoteOPChargeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step))
            {
				entityPOCO.Step = entityPM.Step;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostUnitPrice))
            {
				entityPOCO.CostUnitPrice = entityPM.CostUnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice))
            {
				entityPOCO.SaleUnitPrice = entityPM.SaleUnitPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkupValue))
            {
				entityPOCO.MarkupValue = entityPM.MarkupValue;
			}
			}

		public void POCOToPM(QuoteOPPriceStepsPM entityPM, QuoteOPPriceSteps entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPId))
            {
					entityPM.QuoteOPId = entityPOCO.QuoteOPId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPChargeId))
            {
					entityPM.QuoteOPChargeId = entityPOCO.QuoteOPChargeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step))
            {
					entityPM.Step = entityPOCO.Step;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostUnitPrice))
            {
					entityPM.CostUnitPrice = entityPOCO.CostUnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleUnitPrice))
            {
					entityPM.SaleUnitPrice = entityPOCO.SaleUnitPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarkupValue))
            {
					entityPM.MarkupValue = entityPOCO.MarkupValue;
            }

		}

		public void PMToOldPM(QuoteOPPriceStepsPM entityPM, QuoteOPPriceStepsPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPId))
            {
                oldEntityPM.QuoteOPId = entityPM.QuoteOPId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPChargeId))
            {
                oldEntityPM.QuoteOPChargeId = entityPM.QuoteOPChargeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step))
            {
                oldEntityPM.Step = entityPM.Step;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostUnitPrice))
            {
                oldEntityPM.CostUnitPrice = entityPM.CostUnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleUnitPrice))
            {
                oldEntityPM.SaleUnitPrice = entityPM.SaleUnitPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarkupValue))
            {
                oldEntityPM.MarkupValue = entityPM.MarkupValue;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPPriceStepsPM entityPM)
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
	 