
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
   
   public partial class CountryCurrencyDataMapping: IMapping<CountryCurrencyPM, CountryCurrency>,IMappingEncodeBase64NVARCHARFields<CountryCurrencyPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CountryId, 
	         Tenant, 
	         LineNumber, 
	         Currency,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CountryId, 
	         Tenant, 
	         LineNumber, 
	         Currency, 
	         CurrencyTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CountryCurrencyPM entityPM, CountryCurrency entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			}

		public void POCOToPM(CountryCurrencyPM entityPM, CountryCurrency entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryId))
            {
					entityPM.CountryId = entityPOCO.CountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Currency))
            {
					entityPM.Currency = entityPOCO.Currency;
            }

		}

		public void PMToOldPM(CountryCurrencyPM entityPM, CountryCurrencyPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CountryCurrencyPM entityPM)
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
	 