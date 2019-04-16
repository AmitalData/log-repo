
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
	         OriginPortId, 
	         DestinationPortId,
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
	         OriginPortId, 
	         DestinationPortId,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
				entityPOCO.OriginPortId = entityPM.OriginPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
				entityPOCO.DestinationPortId = entityPM.DestinationPortId;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginPortId))
            {
					entityPM.OriginPortId = entityPOCO.OriginPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationPortId))
            {
					entityPM.DestinationPortId = entityPOCO.DestinationPortId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
                oldEntityPM.OriginPortId = entityPM.OriginPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
                oldEntityPM.DestinationPortId = entityPM.DestinationPortId;
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
	 