
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffLineDataMapping: IMapping<TariffLinePM, TariffLine>,IMappingEncodeBase64NVARCHARFields<TariffLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         TariffId, 
	         Version, 
	         MinPrice, 
	         Step1Price, 
	         Step2Price, 
	         Step3Price, 
	         Step4Price, 
	         Step5Price, 
	         Step6Price, 
	         Step7Price, 
	         Step8Price, 
	         Surcharge1Price, 
	         Surcharge2Price, 
	         Surcharge3Price, 
	         Surcharge4Price, 
	         Surcharge5Price, 
	         Surcharge6Price, 
	         Surcharge7Price, 
	         Surcharge8Price, 
	         Surcharge9Price, 
	         Surcharge10Price, 
	         OriginPortId, 
	         DestinationPortId, 
	         OriginPortText, 
	         DestinationPortText, 
	         MinPriceText, 
	         Step1PriceText, 
	         Step2PriceText, 
	         Step3PriceText, 
	         Step4PriceText, 
	         Step5PriceText, 
	         Step6PriceText, 
	         Step7PriceText, 
	         Step8PriceText, 
	         Surcharge1PriceText, 
	         Surcharge2PriceText, 
	         Surcharge3PriceText, 
	         Surcharge4PriceText, 
	         Surcharge5PriceText, 
	         Surcharge6PriceText, 
	         Surcharge7PriceText, 
	         Surcharge8PriceText, 
	         Surcharge9PriceText, 
	         Surcharge10PriceText, 
	         HasErrors, 
	         ErrorText, 
	         LineUniqueKey, 
	         LineUniqueKeyText, 
	         Index, 
	         Notes, 
	         IsFromAllOtherPorts, 
	         IsToAllOtherPorts, 
	         Surcharge1MinPrice, 
	         Surcharge2MinPrice, 
	         Surcharge3MinPrice, 
	         Surcharge4MinPrice, 
	         Surcharge5MinPrice, 
	         Surcharge6MinPrice, 
	         Surcharge7MinPrice, 
	         Surcharge8MinPrice, 
	         Surcharge9MinPrice, 
	         Surcharge10MinPrice, 
	         CurrencyId, 
	         TransitTime, 
	         IsDifferentCurrenciesPerCharge, 
	         Surcharge1CurrencyId, 
	         Surcharge2CurrencyId, 
	         Surcharge3CurrencyId, 
	         Surcharge4CurrencyId, 
	         Surcharge5CurrencyId, 
	         Surcharge6CurrencyId, 
	         Surcharge7CurrencyId, 
	         Surcharge8CurrencyId, 
	         Surcharge9CurrencyId, 
	         Surcharge10CurrencyId, 
	         ViaPortId, 
	         ViaPortText,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         TariffId, 
	         Version, 
	         MinPrice, 
	         Step1Price, 
	         Step2Price, 
	         Step3Price, 
	         Step4Price, 
	         Step5Price, 
	         Step6Price, 
	         Step7Price, 
	         Step8Price, 
	         Surcharge1Price, 
	         Surcharge2Price, 
	         Surcharge3Price, 
	         Surcharge4Price, 
	         Surcharge5Price, 
	         Surcharge6Price, 
	         Surcharge7Price, 
	         Surcharge8Price, 
	         Surcharge9Price, 
	         Surcharge10Price, 
	         OriginPortId, 
	         DestinationPortId, 
	         OriginPortName, 
	         OriginPortCode, 
	         DestinationPortName, 
	         DestinationPortCode, 
	         OriginPortText, 
	         DestinationPortText, 
	         MinPriceText, 
	         Step1PriceText, 
	         Step2PriceText, 
	         Step3PriceText, 
	         Step4PriceText, 
	         Step5PriceText, 
	         Step6PriceText, 
	         Step7PriceText, 
	         Step8PriceText, 
	         Surcharge1PriceText, 
	         Surcharge2PriceText, 
	         Surcharge3PriceText, 
	         Surcharge4PriceText, 
	         Surcharge5PriceText, 
	         Surcharge6PriceText, 
	         Surcharge7PriceText, 
	         Surcharge8PriceText, 
	         Surcharge9PriceText, 
	         Surcharge10PriceText, 
	         HasErrors, 
	         ErrorText, 
	         LineUniqueKey, 
	         LineUniqueKeyText, 
	         Index, 
	         Notes, 
	         AddedManually, 
	         IsFromAllOtherPorts, 
	         IsToAllOtherPorts, 
	         Surcharge1MinPrice, 
	         Surcharge2MinPrice, 
	         Surcharge3MinPrice, 
	         Surcharge4MinPrice, 
	         Surcharge5MinPrice, 
	         Surcharge6MinPrice, 
	         Surcharge7MinPrice, 
	         Surcharge8MinPrice, 
	         Surcharge9MinPrice, 
	         Surcharge10MinPrice, 
	         CurrencyId, 
	         CurrencyCode, 
	         OriginPortCombinedCode, 
	         DestinationPortCombinedCode, 
	         TransitTime, 
	         OriginPortHasWrongTransMode, 
	         DestinationPortHasWrongTransMode, 
	         IsMinPriceMinus, 
	         IsPrice1Minus, 
	         IsPrice2Minus, 
	         IsPrice3Minus, 
	         IsPrice4Minus, 
	         IsPrice5Minus, 
	         IsPrice6Minus, 
	         IsPrice7Minus, 
	         IsPrice8Minus, 
	         LineEdited, 
	         IsDifferentCurrenciesPerCharge, 
	         Surcharge1CurrencyId, 
	         Surcharge2CurrencyId, 
	         Surcharge3CurrencyId, 
	         Surcharge4CurrencyId, 
	         Surcharge5CurrencyId, 
	         Surcharge6CurrencyId, 
	         Surcharge7CurrencyId, 
	         Surcharge8CurrencyId, 
	         Surcharge9CurrencyId, 
	         Surcharge10CurrencyId, 
	         ViaPortId, 
	         ViaPortText, 
	         ViaPortName, 
	         ViaPortCombinedCode, 
	         ViaPortCode, 
	         ViaPortHasWrongTransMode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffLinePM entityPM, TariffLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
				entityPOCO.ExpirationDate = entityPM.ExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
				entityPOCO.TariffId = entityPM.TariffId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
				entityPOCO.Version = entityPM.Version;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinPrice))
            {
				entityPOCO.MinPrice = entityPM.MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step1Price))
            {
				entityPOCO.Step1Price = entityPM.Step1Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step2Price))
            {
				entityPOCO.Step2Price = entityPM.Step2Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step3Price))
            {
				entityPOCO.Step3Price = entityPM.Step3Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step4Price))
            {
				entityPOCO.Step4Price = entityPM.Step4Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step5Price))
            {
				entityPOCO.Step5Price = entityPM.Step5Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step6Price))
            {
				entityPOCO.Step6Price = entityPM.Step6Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step7Price))
            {
				entityPOCO.Step7Price = entityPM.Step7Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step8Price))
            {
				entityPOCO.Step8Price = entityPM.Step8Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1Price))
            {
				entityPOCO.Surcharge1Price = entityPM.Surcharge1Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2Price))
            {
				entityPOCO.Surcharge2Price = entityPM.Surcharge2Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3Price))
            {
				entityPOCO.Surcharge3Price = entityPM.Surcharge3Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4Price))
            {
				entityPOCO.Surcharge4Price = entityPM.Surcharge4Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5Price))
            {
				entityPOCO.Surcharge5Price = entityPM.Surcharge5Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6Price))
            {
				entityPOCO.Surcharge6Price = entityPM.Surcharge6Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7Price))
            {
				entityPOCO.Surcharge7Price = entityPM.Surcharge7Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8Price))
            {
				entityPOCO.Surcharge8Price = entityPM.Surcharge8Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9Price))
            {
				entityPOCO.Surcharge9Price = entityPM.Surcharge9Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10Price))
            {
				entityPOCO.Surcharge10Price = entityPM.Surcharge10Price;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
				entityPOCO.OriginPortId = entityPM.OriginPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
				entityPOCO.DestinationPortId = entityPM.DestinationPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortText))
            {
				entityPOCO.OriginPortText = entityPM.OriginPortText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortText))
            {
				entityPOCO.DestinationPortText = entityPM.DestinationPortText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinPriceText))
            {
				entityPOCO.MinPriceText = entityPM.MinPriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step1PriceText))
            {
				entityPOCO.Step1PriceText = entityPM.Step1PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step2PriceText))
            {
				entityPOCO.Step2PriceText = entityPM.Step2PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step3PriceText))
            {
				entityPOCO.Step3PriceText = entityPM.Step3PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step4PriceText))
            {
				entityPOCO.Step4PriceText = entityPM.Step4PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step5PriceText))
            {
				entityPOCO.Step5PriceText = entityPM.Step5PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step6PriceText))
            {
				entityPOCO.Step6PriceText = entityPM.Step6PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step7PriceText))
            {
				entityPOCO.Step7PriceText = entityPM.Step7PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step8PriceText))
            {
				entityPOCO.Step8PriceText = entityPM.Step8PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1PriceText))
            {
				entityPOCO.Surcharge1PriceText = entityPM.Surcharge1PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2PriceText))
            {
				entityPOCO.Surcharge2PriceText = entityPM.Surcharge2PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3PriceText))
            {
				entityPOCO.Surcharge3PriceText = entityPM.Surcharge3PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4PriceText))
            {
				entityPOCO.Surcharge4PriceText = entityPM.Surcharge4PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5PriceText))
            {
				entityPOCO.Surcharge5PriceText = entityPM.Surcharge5PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6PriceText))
            {
				entityPOCO.Surcharge6PriceText = entityPM.Surcharge6PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7PriceText))
            {
				entityPOCO.Surcharge7PriceText = entityPM.Surcharge7PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8PriceText))
            {
				entityPOCO.Surcharge8PriceText = entityPM.Surcharge8PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9PriceText))
            {
				entityPOCO.Surcharge9PriceText = entityPM.Surcharge9PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10PriceText))
            {
				entityPOCO.Surcharge10PriceText = entityPM.Surcharge10PriceText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasErrors))
            {
				entityPOCO.HasErrors = entityPM.HasErrors;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorText))
            {
				entityPOCO.ErrorText = entityPM.ErrorText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineUniqueKey))
            {
				entityPOCO.LineUniqueKey = entityPM.LineUniqueKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineUniqueKeyText))
            {
				entityPOCO.LineUniqueKeyText = entityPM.LineUniqueKeyText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
				entityPOCO.Index = entityPM.Index;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFromAllOtherPorts))
            {
				entityPOCO.IsFromAllOtherPorts = entityPM.IsFromAllOtherPorts;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsToAllOtherPorts))
            {
				entityPOCO.IsToAllOtherPorts = entityPM.IsToAllOtherPorts;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1MinPrice))
            {
				entityPOCO.Surcharge1MinPrice = entityPM.Surcharge1MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2MinPrice))
            {
				entityPOCO.Surcharge2MinPrice = entityPM.Surcharge2MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3MinPrice))
            {
				entityPOCO.Surcharge3MinPrice = entityPM.Surcharge3MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4MinPrice))
            {
				entityPOCO.Surcharge4MinPrice = entityPM.Surcharge4MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5MinPrice))
            {
				entityPOCO.Surcharge5MinPrice = entityPM.Surcharge5MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6MinPrice))
            {
				entityPOCO.Surcharge6MinPrice = entityPM.Surcharge6MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7MinPrice))
            {
				entityPOCO.Surcharge7MinPrice = entityPM.Surcharge7MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8MinPrice))
            {
				entityPOCO.Surcharge8MinPrice = entityPM.Surcharge8MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9MinPrice))
            {
				entityPOCO.Surcharge9MinPrice = entityPM.Surcharge9MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10MinPrice))
            {
				entityPOCO.Surcharge10MinPrice = entityPM.Surcharge10MinPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransitTime))
            {
				entityPOCO.TransitTime = entityPM.TransitTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDifferentCurrenciesPerCharge))
            {
				entityPOCO.IsDifferentCurrenciesPerCharge = entityPM.IsDifferentCurrenciesPerCharge;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1CurrencyId))
            {
				entityPOCO.Surcharge1CurrencyId = entityPM.Surcharge1CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2CurrencyId))
            {
				entityPOCO.Surcharge2CurrencyId = entityPM.Surcharge2CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3CurrencyId))
            {
				entityPOCO.Surcharge3CurrencyId = entityPM.Surcharge3CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4CurrencyId))
            {
				entityPOCO.Surcharge4CurrencyId = entityPM.Surcharge4CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5CurrencyId))
            {
				entityPOCO.Surcharge5CurrencyId = entityPM.Surcharge5CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6CurrencyId))
            {
				entityPOCO.Surcharge6CurrencyId = entityPM.Surcharge6CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7CurrencyId))
            {
				entityPOCO.Surcharge7CurrencyId = entityPM.Surcharge7CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8CurrencyId))
            {
				entityPOCO.Surcharge8CurrencyId = entityPM.Surcharge8CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9CurrencyId))
            {
				entityPOCO.Surcharge9CurrencyId = entityPM.Surcharge9CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10CurrencyId))
            {
				entityPOCO.Surcharge10CurrencyId = entityPM.Surcharge10CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ViaPortId))
            {
				entityPOCO.ViaPortId = entityPM.ViaPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ViaPortText))
            {
				entityPOCO.ViaPortText = entityPM.ViaPortText;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TariffLinePM entityPM, TariffLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpirationDate))
            {
					entityPM.ExpirationDate = entityPOCO.ExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Version))
            {
					entityPM.Version = entityPOCO.Version;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MinPrice))
            {
					entityPM.MinPrice = entityPOCO.MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step1Price))
            {
					entityPM.Step1Price = entityPOCO.Step1Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step2Price))
            {
					entityPM.Step2Price = entityPOCO.Step2Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step3Price))
            {
					entityPM.Step3Price = entityPOCO.Step3Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step4Price))
            {
					entityPM.Step4Price = entityPOCO.Step4Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step5Price))
            {
					entityPM.Step5Price = entityPOCO.Step5Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step6Price))
            {
					entityPM.Step6Price = entityPOCO.Step6Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step7Price))
            {
					entityPM.Step7Price = entityPOCO.Step7Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step8Price))
            {
					entityPM.Step8Price = entityPOCO.Step8Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1Price))
            {
					entityPM.Surcharge1Price = entityPOCO.Surcharge1Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2Price))
            {
					entityPM.Surcharge2Price = entityPOCO.Surcharge2Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3Price))
            {
					entityPM.Surcharge3Price = entityPOCO.Surcharge3Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4Price))
            {
					entityPM.Surcharge4Price = entityPOCO.Surcharge4Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5Price))
            {
					entityPM.Surcharge5Price = entityPOCO.Surcharge5Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6Price))
            {
					entityPM.Surcharge6Price = entityPOCO.Surcharge6Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7Price))
            {
					entityPM.Surcharge7Price = entityPOCO.Surcharge7Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8Price))
            {
					entityPM.Surcharge8Price = entityPOCO.Surcharge8Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9Price))
            {
					entityPM.Surcharge9Price = entityPOCO.Surcharge9Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10Price))
            {
					entityPM.Surcharge10Price = entityPOCO.Surcharge10Price;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginPortId))
            {
					entityPM.OriginPortId = entityPOCO.OriginPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationPortId))
            {
					entityPM.DestinationPortId = entityPOCO.DestinationPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginPortText))
            {
					entityPM.OriginPortText = entityPOCO.OriginPortText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationPortText))
            {
					entityPM.DestinationPortText = entityPOCO.DestinationPortText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MinPriceText))
            {
					entityPM.MinPriceText = entityPOCO.MinPriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step1PriceText))
            {
					entityPM.Step1PriceText = entityPOCO.Step1PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step2PriceText))
            {
					entityPM.Step2PriceText = entityPOCO.Step2PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step3PriceText))
            {
					entityPM.Step3PriceText = entityPOCO.Step3PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step4PriceText))
            {
					entityPM.Step4PriceText = entityPOCO.Step4PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step5PriceText))
            {
					entityPM.Step5PriceText = entityPOCO.Step5PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step6PriceText))
            {
					entityPM.Step6PriceText = entityPOCO.Step6PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step7PriceText))
            {
					entityPM.Step7PriceText = entityPOCO.Step7PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Step8PriceText))
            {
					entityPM.Step8PriceText = entityPOCO.Step8PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1PriceText))
            {
					entityPM.Surcharge1PriceText = entityPOCO.Surcharge1PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2PriceText))
            {
					entityPM.Surcharge2PriceText = entityPOCO.Surcharge2PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3PriceText))
            {
					entityPM.Surcharge3PriceText = entityPOCO.Surcharge3PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4PriceText))
            {
					entityPM.Surcharge4PriceText = entityPOCO.Surcharge4PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5PriceText))
            {
					entityPM.Surcharge5PriceText = entityPOCO.Surcharge5PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6PriceText))
            {
					entityPM.Surcharge6PriceText = entityPOCO.Surcharge6PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7PriceText))
            {
					entityPM.Surcharge7PriceText = entityPOCO.Surcharge7PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8PriceText))
            {
					entityPM.Surcharge8PriceText = entityPOCO.Surcharge8PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9PriceText))
            {
					entityPM.Surcharge9PriceText = entityPOCO.Surcharge9PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10PriceText))
            {
					entityPM.Surcharge10PriceText = entityPOCO.Surcharge10PriceText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HasErrors))
            {
					entityPM.HasErrors = entityPOCO.HasErrors;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorText))
            {
					entityPM.ErrorText = entityPOCO.ErrorText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineUniqueKey))
            {
					entityPM.LineUniqueKey = entityPOCO.LineUniqueKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineUniqueKeyText))
            {
					entityPM.LineUniqueKeyText = entityPOCO.LineUniqueKeyText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Index))
            {
					entityPM.Index = entityPOCO.Index;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsFromAllOtherPorts))
            {
					entityPM.IsFromAllOtherPorts = entityPOCO.IsFromAllOtherPorts;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsToAllOtherPorts))
            {
					entityPM.IsToAllOtherPorts = entityPOCO.IsToAllOtherPorts;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1MinPrice))
            {
					entityPM.Surcharge1MinPrice = entityPOCO.Surcharge1MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2MinPrice))
            {
					entityPM.Surcharge2MinPrice = entityPOCO.Surcharge2MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3MinPrice))
            {
					entityPM.Surcharge3MinPrice = entityPOCO.Surcharge3MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4MinPrice))
            {
					entityPM.Surcharge4MinPrice = entityPOCO.Surcharge4MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5MinPrice))
            {
					entityPM.Surcharge5MinPrice = entityPOCO.Surcharge5MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6MinPrice))
            {
					entityPM.Surcharge6MinPrice = entityPOCO.Surcharge6MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7MinPrice))
            {
					entityPM.Surcharge7MinPrice = entityPOCO.Surcharge7MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8MinPrice))
            {
					entityPM.Surcharge8MinPrice = entityPOCO.Surcharge8MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9MinPrice))
            {
					entityPM.Surcharge9MinPrice = entityPOCO.Surcharge9MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10MinPrice))
            {
					entityPM.Surcharge10MinPrice = entityPOCO.Surcharge10MinPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransitTime))
            {
					entityPM.TransitTime = entityPOCO.TransitTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDifferentCurrenciesPerCharge))
            {
					entityPM.IsDifferentCurrenciesPerCharge = entityPOCO.IsDifferentCurrenciesPerCharge;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1CurrencyId))
            {
					entityPM.Surcharge1CurrencyId = entityPOCO.Surcharge1CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2CurrencyId))
            {
					entityPM.Surcharge2CurrencyId = entityPOCO.Surcharge2CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3CurrencyId))
            {
					entityPM.Surcharge3CurrencyId = entityPOCO.Surcharge3CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4CurrencyId))
            {
					entityPM.Surcharge4CurrencyId = entityPOCO.Surcharge4CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5CurrencyId))
            {
					entityPM.Surcharge5CurrencyId = entityPOCO.Surcharge5CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6CurrencyId))
            {
					entityPM.Surcharge6CurrencyId = entityPOCO.Surcharge6CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7CurrencyId))
            {
					entityPM.Surcharge7CurrencyId = entityPOCO.Surcharge7CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8CurrencyId))
            {
					entityPM.Surcharge8CurrencyId = entityPOCO.Surcharge8CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9CurrencyId))
            {
					entityPM.Surcharge9CurrencyId = entityPOCO.Surcharge9CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10CurrencyId))
            {
					entityPM.Surcharge10CurrencyId = entityPOCO.Surcharge10CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ViaPortId))
            {
					entityPM.ViaPortId = entityPOCO.ViaPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ViaPortText))
            {
					entityPM.ViaPortText = entityPOCO.ViaPortText;
            }

		}

		public void PMToOldPM(TariffLinePM entityPM, TariffLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
                oldEntityPM.ExpirationDate = entityPM.ExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
                oldEntityPM.TariffId = entityPM.TariffId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
                oldEntityPM.Version = entityPM.Version;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinPrice))
            {
                oldEntityPM.MinPrice = entityPM.MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step1Price))
            {
                oldEntityPM.Step1Price = entityPM.Step1Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step2Price))
            {
                oldEntityPM.Step2Price = entityPM.Step2Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step3Price))
            {
                oldEntityPM.Step3Price = entityPM.Step3Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step4Price))
            {
                oldEntityPM.Step4Price = entityPM.Step4Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step5Price))
            {
                oldEntityPM.Step5Price = entityPM.Step5Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step6Price))
            {
                oldEntityPM.Step6Price = entityPM.Step6Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step7Price))
            {
                oldEntityPM.Step7Price = entityPM.Step7Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step8Price))
            {
                oldEntityPM.Step8Price = entityPM.Step8Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1Price))
            {
                oldEntityPM.Surcharge1Price = entityPM.Surcharge1Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2Price))
            {
                oldEntityPM.Surcharge2Price = entityPM.Surcharge2Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3Price))
            {
                oldEntityPM.Surcharge3Price = entityPM.Surcharge3Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4Price))
            {
                oldEntityPM.Surcharge4Price = entityPM.Surcharge4Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5Price))
            {
                oldEntityPM.Surcharge5Price = entityPM.Surcharge5Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6Price))
            {
                oldEntityPM.Surcharge6Price = entityPM.Surcharge6Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7Price))
            {
                oldEntityPM.Surcharge7Price = entityPM.Surcharge7Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8Price))
            {
                oldEntityPM.Surcharge8Price = entityPM.Surcharge8Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9Price))
            {
                oldEntityPM.Surcharge9Price = entityPM.Surcharge9Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10Price))
            {
                oldEntityPM.Surcharge10Price = entityPM.Surcharge10Price;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
                oldEntityPM.OriginPortId = entityPM.OriginPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
                oldEntityPM.DestinationPortId = entityPM.DestinationPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortText))
            {
                oldEntityPM.OriginPortText = entityPM.OriginPortText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortText))
            {
                oldEntityPM.DestinationPortText = entityPM.DestinationPortText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinPriceText))
            {
                oldEntityPM.MinPriceText = entityPM.MinPriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step1PriceText))
            {
                oldEntityPM.Step1PriceText = entityPM.Step1PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step2PriceText))
            {
                oldEntityPM.Step2PriceText = entityPM.Step2PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step3PriceText))
            {
                oldEntityPM.Step3PriceText = entityPM.Step3PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step4PriceText))
            {
                oldEntityPM.Step4PriceText = entityPM.Step4PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step5PriceText))
            {
                oldEntityPM.Step5PriceText = entityPM.Step5PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step6PriceText))
            {
                oldEntityPM.Step6PriceText = entityPM.Step6PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step7PriceText))
            {
                oldEntityPM.Step7PriceText = entityPM.Step7PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Step8PriceText))
            {
                oldEntityPM.Step8PriceText = entityPM.Step8PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1PriceText))
            {
                oldEntityPM.Surcharge1PriceText = entityPM.Surcharge1PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2PriceText))
            {
                oldEntityPM.Surcharge2PriceText = entityPM.Surcharge2PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3PriceText))
            {
                oldEntityPM.Surcharge3PriceText = entityPM.Surcharge3PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4PriceText))
            {
                oldEntityPM.Surcharge4PriceText = entityPM.Surcharge4PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5PriceText))
            {
                oldEntityPM.Surcharge5PriceText = entityPM.Surcharge5PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6PriceText))
            {
                oldEntityPM.Surcharge6PriceText = entityPM.Surcharge6PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7PriceText))
            {
                oldEntityPM.Surcharge7PriceText = entityPM.Surcharge7PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8PriceText))
            {
                oldEntityPM.Surcharge8PriceText = entityPM.Surcharge8PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9PriceText))
            {
                oldEntityPM.Surcharge9PriceText = entityPM.Surcharge9PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10PriceText))
            {
                oldEntityPM.Surcharge10PriceText = entityPM.Surcharge10PriceText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasErrors))
            {
                oldEntityPM.HasErrors = entityPM.HasErrors;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorText))
            {
                oldEntityPM.ErrorText = entityPM.ErrorText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineUniqueKey))
            {
                oldEntityPM.LineUniqueKey = entityPM.LineUniqueKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineUniqueKeyText))
            {
                oldEntityPM.LineUniqueKeyText = entityPM.LineUniqueKeyText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
                oldEntityPM.Index = entityPM.Index;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFromAllOtherPorts))
            {
                oldEntityPM.IsFromAllOtherPorts = entityPM.IsFromAllOtherPorts;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsToAllOtherPorts))
            {
                oldEntityPM.IsToAllOtherPorts = entityPM.IsToAllOtherPorts;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1MinPrice))
            {
                oldEntityPM.Surcharge1MinPrice = entityPM.Surcharge1MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2MinPrice))
            {
                oldEntityPM.Surcharge2MinPrice = entityPM.Surcharge2MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3MinPrice))
            {
                oldEntityPM.Surcharge3MinPrice = entityPM.Surcharge3MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4MinPrice))
            {
                oldEntityPM.Surcharge4MinPrice = entityPM.Surcharge4MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5MinPrice))
            {
                oldEntityPM.Surcharge5MinPrice = entityPM.Surcharge5MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6MinPrice))
            {
                oldEntityPM.Surcharge6MinPrice = entityPM.Surcharge6MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7MinPrice))
            {
                oldEntityPM.Surcharge7MinPrice = entityPM.Surcharge7MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8MinPrice))
            {
                oldEntityPM.Surcharge8MinPrice = entityPM.Surcharge8MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9MinPrice))
            {
                oldEntityPM.Surcharge9MinPrice = entityPM.Surcharge9MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10MinPrice))
            {
                oldEntityPM.Surcharge10MinPrice = entityPM.Surcharge10MinPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransitTime))
            {
                oldEntityPM.TransitTime = entityPM.TransitTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDifferentCurrenciesPerCharge))
            {
                oldEntityPM.IsDifferentCurrenciesPerCharge = entityPM.IsDifferentCurrenciesPerCharge;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1CurrencyId))
            {
                oldEntityPM.Surcharge1CurrencyId = entityPM.Surcharge1CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2CurrencyId))
            {
                oldEntityPM.Surcharge2CurrencyId = entityPM.Surcharge2CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3CurrencyId))
            {
                oldEntityPM.Surcharge3CurrencyId = entityPM.Surcharge3CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4CurrencyId))
            {
                oldEntityPM.Surcharge4CurrencyId = entityPM.Surcharge4CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5CurrencyId))
            {
                oldEntityPM.Surcharge5CurrencyId = entityPM.Surcharge5CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6CurrencyId))
            {
                oldEntityPM.Surcharge6CurrencyId = entityPM.Surcharge6CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7CurrencyId))
            {
                oldEntityPM.Surcharge7CurrencyId = entityPM.Surcharge7CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8CurrencyId))
            {
                oldEntityPM.Surcharge8CurrencyId = entityPM.Surcharge8CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9CurrencyId))
            {
                oldEntityPM.Surcharge9CurrencyId = entityPM.Surcharge9CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10CurrencyId))
            {
                oldEntityPM.Surcharge10CurrencyId = entityPM.Surcharge10CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ViaPortId))
            {
                oldEntityPM.ViaPortId = entityPM.ViaPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ViaPortText))
            {
                oldEntityPM.ViaPortText = entityPM.ViaPortText;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffLinePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LineUniqueKey)) //T4 find type == nText 
            {
                entityPM.LineUniqueKey = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LineUniqueKey));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LineUniqueKeyText)) //T4 find type == nText 
            {
                entityPM.LineUniqueKeyText = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LineUniqueKeyText));
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
		
		private void BuildSearchFieldsGenerated(TariffLinePM entityPM, TariffLine entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 