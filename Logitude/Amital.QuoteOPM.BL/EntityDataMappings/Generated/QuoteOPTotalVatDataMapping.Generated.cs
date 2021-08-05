
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
   
   public partial class QuoteOPTotalVATDataMapping: IMapping<QuoteOPTotalVATPM, QuoteOPTotalVAT>,IMappingEncodeBase64NVARCHARFields<QuoteOPTotalVATPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         QuoteCurrencyVATAmount, 
	         QuoteCurrencyVatableAmount, 
	         LocalCurrencyVATAmount, 
	         LocalCurrencyVatableAmount, 
	         ProfitCurrencyVATAmount, 
	         ProfitCurrencyVatableAmount, 
	         VatPercent, 
	         ExternalVATCard, 
	         ExternalTAXItemId, 
	         VatOPTypeId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         QuoteCurrencyVATAmount, 
	         QuoteCurrencyVatableAmount, 
	         LocalCurrencyVATAmount, 
	         LocalCurrencyVatableAmount, 
	         ProfitCurrencyVATAmount, 
	         ProfitCurrencyVatableAmount, 
	         VatPercent, 
	         ExternalVATCard, 
	         ExternalTAXItemId, 
	         VatOPTypeId, 
	         VatTypeName, 
	         VatTypeCell,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTotalVATPM entityPM, QuoteOPTotalVAT entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPId))
            {
				entityPOCO.QuoteOPId = entityPM.QuoteOPId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCurrencyVATAmount))
            {
				entityPOCO.QuoteCurrencyVATAmount = entityPM.QuoteCurrencyVATAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCurrencyVatableAmount))
            {
				entityPOCO.QuoteCurrencyVatableAmount = entityPM.QuoteCurrencyVatableAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCurrencyVATAmount))
            {
				entityPOCO.LocalCurrencyVATAmount = entityPM.LocalCurrencyVATAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCurrencyVatableAmount))
            {
				entityPOCO.LocalCurrencyVatableAmount = entityPM.LocalCurrencyVatableAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyVATAmount))
            {
				entityPOCO.ProfitCurrencyVATAmount = entityPM.ProfitCurrencyVATAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyVatableAmount))
            {
				entityPOCO.ProfitCurrencyVatableAmount = entityPM.ProfitCurrencyVatableAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatPercent))
            {
				entityPOCO.VatPercent = entityPM.VatPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalVATCard))
            {
				entityPOCO.ExternalVATCard = entityPM.ExternalVATCard;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalTAXItemId))
            {
				entityPOCO.ExternalTAXItemId = entityPM.ExternalTAXItemId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatOPTypeId))
            {
				entityPOCO.VatOPTypeId = entityPM.VatOPTypeId;
			}
			}

		public void POCOToPM(QuoteOPTotalVATPM entityPM, QuoteOPTotalVAT entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteCurrencyVATAmount))
            {
					entityPM.QuoteCurrencyVATAmount = entityPOCO.QuoteCurrencyVATAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteCurrencyVatableAmount))
            {
					entityPM.QuoteCurrencyVatableAmount = entityPOCO.QuoteCurrencyVatableAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalCurrencyVATAmount))
            {
					entityPM.LocalCurrencyVATAmount = entityPOCO.LocalCurrencyVATAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalCurrencyVatableAmount))
            {
					entityPM.LocalCurrencyVatableAmount = entityPOCO.LocalCurrencyVatableAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfitCurrencyVATAmount))
            {
					entityPM.ProfitCurrencyVATAmount = entityPOCO.ProfitCurrencyVATAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfitCurrencyVatableAmount))
            {
					entityPM.ProfitCurrencyVatableAmount = entityPOCO.ProfitCurrencyVatableAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatPercent))
            {
					entityPM.VatPercent = entityPOCO.VatPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalVATCard))
            {
					entityPM.ExternalVATCard = entityPOCO.ExternalVATCard;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalTAXItemId))
            {
					entityPM.ExternalTAXItemId = entityPOCO.ExternalTAXItemId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatOPTypeId))
            {
					entityPM.VatOPTypeId = entityPOCO.VatOPTypeId;
            }

		}

		public void PMToOldPM(QuoteOPTotalVATPM entityPM, QuoteOPTotalVATPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCurrencyVATAmount))
            {
                oldEntityPM.QuoteCurrencyVATAmount = entityPM.QuoteCurrencyVATAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCurrencyVatableAmount))
            {
                oldEntityPM.QuoteCurrencyVatableAmount = entityPM.QuoteCurrencyVatableAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCurrencyVATAmount))
            {
                oldEntityPM.LocalCurrencyVATAmount = entityPM.LocalCurrencyVATAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalCurrencyVatableAmount))
            {
                oldEntityPM.LocalCurrencyVatableAmount = entityPM.LocalCurrencyVatableAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyVATAmount))
            {
                oldEntityPM.ProfitCurrencyVATAmount = entityPM.ProfitCurrencyVATAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyVatableAmount))
            {
                oldEntityPM.ProfitCurrencyVatableAmount = entityPM.ProfitCurrencyVatableAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatPercent))
            {
                oldEntityPM.VatPercent = entityPM.VatPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalVATCard))
            {
                oldEntityPM.ExternalVATCard = entityPM.ExternalVATCard;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalTAXItemId))
            {
                oldEntityPM.ExternalTAXItemId = entityPM.ExternalTAXItemId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatOPTypeId))
            {
                oldEntityPM.VatOPTypeId = entityPM.VatOPTypeId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTotalVATPM entityPM)
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
	 