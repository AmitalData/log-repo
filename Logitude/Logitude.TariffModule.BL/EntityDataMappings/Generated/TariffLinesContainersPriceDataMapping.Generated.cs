
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
   
   public partial class TariffLinesContainersPriceDataMapping: IMapping<TariffLinesContainersPricePM, TariffLinesContainersPrice>,IMappingEncodeBase64NVARCHARFields<TariffLinesContainersPricePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TariffId, 
	         TariffLineId, 
	         SurchargeId, 
	         Price1, 
	         Price2, 
	         Price3, 
	         Price4, 
	         Price5, 
	         CostPrice,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TariffId, 
	         TariffLineId, 
	         SurchargeId, 
	         Price1, 
	         Price2, 
	         Price3, 
	         Price4, 
	         Price5, 
	         CostPrice,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffLinesContainersPricePM entityPM, TariffLinesContainersPrice entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
				entityPOCO.TariffId = entityPM.TariffId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffLineId))
            {
				entityPOCO.TariffLineId = entityPM.TariffLineId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SurchargeId))
            {
				entityPOCO.SurchargeId = entityPM.SurchargeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price1))
            {
				entityPOCO.Price1 = entityPM.Price1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price2))
            {
				entityPOCO.Price2 = entityPM.Price2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price3))
            {
				entityPOCO.Price3 = entityPM.Price3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price4))
            {
				entityPOCO.Price4 = entityPM.Price4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price5))
            {
				entityPOCO.Price5 = entityPM.Price5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostPrice))
            {
				entityPOCO.CostPrice = entityPM.CostPrice;
			}
			}

		public void POCOToPM(TariffLinesContainersPricePM entityPM, TariffLinesContainersPrice entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffLineId))
            {
					entityPM.TariffLineId = entityPOCO.TariffLineId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SurchargeId))
            {
					entityPM.SurchargeId = entityPOCO.SurchargeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Price1))
            {
					entityPM.Price1 = entityPOCO.Price1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Price2))
            {
					entityPM.Price2 = entityPOCO.Price2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Price3))
            {
					entityPM.Price3 = entityPOCO.Price3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Price4))
            {
					entityPM.Price4 = entityPOCO.Price4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Price5))
            {
					entityPM.Price5 = entityPOCO.Price5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CostPrice))
            {
					entityPM.CostPrice = entityPOCO.CostPrice;
            }

		}

		public void PMToOldPM(TariffLinesContainersPricePM entityPM, TariffLinesContainersPricePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
                oldEntityPM.TariffId = entityPM.TariffId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffLineId))
            {
                oldEntityPM.TariffLineId = entityPM.TariffLineId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SurchargeId))
            {
                oldEntityPM.SurchargeId = entityPM.SurchargeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price1))
            {
                oldEntityPM.Price1 = entityPM.Price1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price2))
            {
                oldEntityPM.Price2 = entityPM.Price2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price3))
            {
                oldEntityPM.Price3 = entityPM.Price3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price4))
            {
                oldEntityPM.Price4 = entityPM.Price4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Price5))
            {
                oldEntityPM.Price5 = entityPM.Price5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CostPrice))
            {
                oldEntityPM.CostPrice = entityPM.CostPrice;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffLinesContainersPricePM entityPM)
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
	 