
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
   
   public partial class QuoteOPSettingDataMapping: IMapping<QuoteOPSettingPM, QuoteOPSetting>,IMappingEncodeBase64NVARCHARFields<QuoteOPSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CopyExchangeRates, 
	         AutomaticallyCloseDays, 
	         CopyShipper, 
	         CopyConsignee, 
	         CopyMainCarriage, 
	         CopyPickup, 
	         CopyDelivery, 
	         CopyChargesTypes, 
	         CopyChargesCost, 
	         CopyChargesSale, 
	         EditMainCarriage, 
	         CopyAgent, 
	         CopyNotify, 
	         IsSaleAsCostCurrency, 
	         IsMultiCurrency, 
	         QuoteExpirationDays,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CopyExchangeRates, 
	         AutomaticallyCloseDays, 
	         CopyShipper, 
	         CopyConsignee, 
	         CopyMainCarriage, 
	         CopyPickup, 
	         CopyDelivery, 
	         CopyChargesTypes, 
	         CopyChargesCost, 
	         CopyChargesSale, 
	         EditMainCarriage, 
	         CopyAgent, 
	         CopyNotify, 
	         IsSaleAsCostCurrency, 
	         IsMultiCurrency, 
	         QuoteExpirationDays,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPSettingPM entityPM, QuoteOPSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyExchangeRates))
            {
				entityPOCO.CopyExchangeRates = entityPM.CopyExchangeRates;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDays))
            {
				entityPOCO.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyShipper))
            {
				entityPOCO.CopyShipper = entityPM.CopyShipper;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyConsignee))
            {
				entityPOCO.CopyConsignee = entityPM.CopyConsignee;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyMainCarriage))
            {
				entityPOCO.CopyMainCarriage = entityPM.CopyMainCarriage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyPickup))
            {
				entityPOCO.CopyPickup = entityPM.CopyPickup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyDelivery))
            {
				entityPOCO.CopyDelivery = entityPM.CopyDelivery;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesTypes))
            {
				entityPOCO.CopyChargesTypes = entityPM.CopyChargesTypes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesCost))
            {
				entityPOCO.CopyChargesCost = entityPM.CopyChargesCost;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesSale))
            {
				entityPOCO.CopyChargesSale = entityPM.CopyChargesSale;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EditMainCarriage))
            {
				entityPOCO.EditMainCarriage = entityPM.EditMainCarriage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyAgent))
            {
				entityPOCO.CopyAgent = entityPM.CopyAgent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyNotify))
            {
				entityPOCO.CopyNotify = entityPM.CopyNotify;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSaleAsCostCurrency))
            {
				entityPOCO.IsSaleAsCostCurrency = entityPM.IsSaleAsCostCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
				entityPOCO.IsMultiCurrency = entityPM.IsMultiCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteExpirationDays))
            {
				entityPOCO.QuoteExpirationDays = entityPM.QuoteExpirationDays;
			}
			}

		public void POCOToPM(QuoteOPSettingPM entityPM, QuoteOPSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyExchangeRates))
            {
					entityPM.CopyExchangeRates = entityPOCO.CopyExchangeRates;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutomaticallyCloseDays))
            {
					entityPM.AutomaticallyCloseDays = entityPOCO.AutomaticallyCloseDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyShipper))
            {
					entityPM.CopyShipper = entityPOCO.CopyShipper;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyConsignee))
            {
					entityPM.CopyConsignee = entityPOCO.CopyConsignee;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyMainCarriage))
            {
					entityPM.CopyMainCarriage = entityPOCO.CopyMainCarriage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyPickup))
            {
					entityPM.CopyPickup = entityPOCO.CopyPickup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyDelivery))
            {
					entityPM.CopyDelivery = entityPOCO.CopyDelivery;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyChargesTypes))
            {
					entityPM.CopyChargesTypes = entityPOCO.CopyChargesTypes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyChargesCost))
            {
					entityPM.CopyChargesCost = entityPOCO.CopyChargesCost;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyChargesSale))
            {
					entityPM.CopyChargesSale = entityPOCO.CopyChargesSale;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EditMainCarriage))
            {
					entityPM.EditMainCarriage = entityPOCO.EditMainCarriage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyAgent))
            {
					entityPM.CopyAgent = entityPOCO.CopyAgent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CopyNotify))
            {
					entityPM.CopyNotify = entityPOCO.CopyNotify;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSaleAsCostCurrency))
            {
					entityPM.IsSaleAsCostCurrency = entityPOCO.IsSaleAsCostCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMultiCurrency))
            {
					entityPM.IsMultiCurrency = entityPOCO.IsMultiCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteExpirationDays))
            {
					entityPM.QuoteExpirationDays = entityPOCO.QuoteExpirationDays;
            }

		}

		public void PMToOldPM(QuoteOPSettingPM entityPM, QuoteOPSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyExchangeRates))
            {
                oldEntityPM.CopyExchangeRates = entityPM.CopyExchangeRates;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDays))
            {
                oldEntityPM.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyShipper))
            {
                oldEntityPM.CopyShipper = entityPM.CopyShipper;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyConsignee))
            {
                oldEntityPM.CopyConsignee = entityPM.CopyConsignee;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyMainCarriage))
            {
                oldEntityPM.CopyMainCarriage = entityPM.CopyMainCarriage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyPickup))
            {
                oldEntityPM.CopyPickup = entityPM.CopyPickup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyDelivery))
            {
                oldEntityPM.CopyDelivery = entityPM.CopyDelivery;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesTypes))
            {
                oldEntityPM.CopyChargesTypes = entityPM.CopyChargesTypes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesCost))
            {
                oldEntityPM.CopyChargesCost = entityPM.CopyChargesCost;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyChargesSale))
            {
                oldEntityPM.CopyChargesSale = entityPM.CopyChargesSale;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EditMainCarriage))
            {
                oldEntityPM.EditMainCarriage = entityPM.EditMainCarriage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyAgent))
            {
                oldEntityPM.CopyAgent = entityPM.CopyAgent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CopyNotify))
            {
                oldEntityPM.CopyNotify = entityPM.CopyNotify;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSaleAsCostCurrency))
            {
                oldEntityPM.IsSaleAsCostCurrency = entityPM.IsSaleAsCostCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
                oldEntityPM.IsMultiCurrency = entityPM.IsMultiCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteExpirationDays))
            {
                oldEntityPM.QuoteExpirationDays = entityPM.QuoteExpirationDays;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPSettingPM entityPM)
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
	 