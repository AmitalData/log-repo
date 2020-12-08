
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsAirlineDataMapping: IMapping<CustomsAirlinePM, CustomsAirline>,IMappingEncodeBase64NVARCHARFields<CustomsAirlinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AirlineCode, 
	         LocalName, 
	         EnglishName, 
	         InActive, 
	         SearchFields, 
	         AirlinePrefix, 
	         ICAO, 
	         UnloadPortCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AirlineCode, 
	         LocalName, 
	         EnglishName, 
	         InActive, 
	         SearchFields, 
	         AirlinePrefix, 
	         ICAO, 
	         UnloadPortCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsAirlinePM entityPM, CustomsAirline entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlineCode))
            {
				entityPOCO.AirlineCode = entityPM.AirlineCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
				entityPOCO.InActive = entityPM.InActive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlinePrefix))
            {
				entityPOCO.AirlinePrefix = entityPM.AirlinePrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ICAO))
            {
				entityPOCO.ICAO = entityPM.ICAO;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadPortCode))
            {
				entityPOCO.UnloadPortCode = entityPM.UnloadPortCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CustomsAirlinePM entityPM, CustomsAirline entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirlineCode))
            {
					entityPM.AirlineCode = entityPOCO.AirlineCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InActive))
            {
					entityPM.InActive = entityPOCO.InActive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirlinePrefix))
            {
					entityPM.AirlinePrefix = entityPOCO.AirlinePrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ICAO))
            {
					entityPM.ICAO = entityPOCO.ICAO;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnloadPortCode))
            {
					entityPM.UnloadPortCode = entityPOCO.UnloadPortCode;
            }

		}

		public void PMToOldPM(CustomsAirlinePM entityPM, CustomsAirlinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlineCode))
            {
                oldEntityPM.AirlineCode = entityPM.AirlineCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
                oldEntityPM.InActive = entityPM.InActive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlinePrefix))
            {
                oldEntityPM.AirlinePrefix = entityPM.AirlinePrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ICAO))
            {
                oldEntityPM.ICAO = entityPM.ICAO;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadPortCode))
            {
                oldEntityPM.UnloadPortCode = entityPM.UnloadPortCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsAirlinePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
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
		
		private void BuildSearchFieldsGenerated(CustomsAirlinePM entityPM, CustomsAirline entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AirlineCode);
           
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
           
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
           
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AirlinePrefix);
           
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ICAO);
           
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 