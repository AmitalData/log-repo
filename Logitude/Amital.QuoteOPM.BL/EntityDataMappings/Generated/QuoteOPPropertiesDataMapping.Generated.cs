
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
   
   public partial class QuoteOPPropertiesDataMapping: IMapping<QuoteOPPropertiesPM, QuoteOPProperties>,IMappingEncodeBase64NVARCHARFields<QuoteOPPropertiesPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         QuoteID, 
	         IndexOrder, 
	         FromPortId, 
	         ToPortId, 
	         IncotermId, 
	         SpecialServiceID, 
	         MainCarriageCarrierId, 
	         FromAddressId, 
	         FromAddressZipCode, 
	         FromAddressCountryId, 
	         FromAddressCity, 
	         ToAddressId, 
	         ToAddressCity, 
	         ToAddressCountryId, 
	         ToAddressZipCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         QuoteID, 
	         IndexOrder, 
	         FromPortId, 
	         ToPortId, 
	         IncotermId, 
	         SpecialServiceID, 
	         MainCarriageCarrierId, 
	         FromAddressId, 
	         FromAddressZipCode, 
	         FromAddressCountryId, 
	         FromAddressCity, 
	         ToAddressId, 
	         ToAddressCity, 
	         ToAddressCountryId, 
	         ToAddressZipCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPPropertiesPM entityPM, QuoteOPProperties entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteID))
            {
				entityPOCO.QuoteID = entityPM.QuoteID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndexOrder))
            {
				entityPOCO.IndexOrder = entityPM.IndexOrder;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
				entityPOCO.FromPortId = entityPM.FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
				entityPOCO.ToPortId = entityPM.ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
				entityPOCO.IncotermId = entityPM.IncotermId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServiceID))
            {
				entityPOCO.SpecialServiceID = entityPM.SpecialServiceID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
				entityPOCO.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
				entityPOCO.FromAddressId = entityPM.FromAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressZipCode))
            {
				entityPOCO.FromAddressZipCode = entityPM.FromAddressZipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCountryId))
            {
				entityPOCO.FromAddressCountryId = entityPM.FromAddressCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCity))
            {
				entityPOCO.FromAddressCity = entityPM.FromAddressCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
				entityPOCO.ToAddressId = entityPM.ToAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCity))
            {
				entityPOCO.ToAddressCity = entityPM.ToAddressCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCountryId))
            {
				entityPOCO.ToAddressCountryId = entityPM.ToAddressCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressZipCode))
            {
				entityPOCO.ToAddressZipCode = entityPM.ToAddressZipCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(QuoteOPPropertiesPM entityPM, QuoteOPProperties entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteID))
            {
					entityPM.QuoteID = entityPOCO.QuoteID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IndexOrder))
            {
					entityPM.IndexOrder = entityPOCO.IndexOrder;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortId))
            {
					entityPM.FromPortId = entityPOCO.FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortId))
            {
					entityPM.ToPortId = entityPOCO.ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncotermId))
            {
					entityPM.IncotermId = entityPOCO.IncotermId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialServiceID))
            {
					entityPM.SpecialServiceID = entityPOCO.SpecialServiceID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageCarrierId))
            {
					entityPM.MainCarriageCarrierId = entityPOCO.MainCarriageCarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressId))
            {
					entityPM.FromAddressId = entityPOCO.FromAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressZipCode))
            {
					entityPM.FromAddressZipCode = entityPOCO.FromAddressZipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressCountryId))
            {
					entityPM.FromAddressCountryId = entityPOCO.FromAddressCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressCity))
            {
					entityPM.FromAddressCity = entityPOCO.FromAddressCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressId))
            {
					entityPM.ToAddressId = entityPOCO.ToAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressCity))
            {
					entityPM.ToAddressCity = entityPOCO.ToAddressCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressCountryId))
            {
					entityPM.ToAddressCountryId = entityPOCO.ToAddressCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressZipCode))
            {
					entityPM.ToAddressZipCode = entityPOCO.ToAddressZipCode;
            }

		}

		public void PMToOldPM(QuoteOPPropertiesPM entityPM, QuoteOPPropertiesPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteID))
            {
                oldEntityPM.QuoteID = entityPM.QuoteID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndexOrder))
            {
                oldEntityPM.IndexOrder = entityPM.IndexOrder;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
                oldEntityPM.FromPortId = entityPM.FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
                oldEntityPM.ToPortId = entityPM.ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
                oldEntityPM.IncotermId = entityPM.IncotermId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServiceID))
            {
                oldEntityPM.SpecialServiceID = entityPM.SpecialServiceID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
                oldEntityPM.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
                oldEntityPM.FromAddressId = entityPM.FromAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressZipCode))
            {
                oldEntityPM.FromAddressZipCode = entityPM.FromAddressZipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCountryId))
            {
                oldEntityPM.FromAddressCountryId = entityPM.FromAddressCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCity))
            {
                oldEntityPM.FromAddressCity = entityPM.FromAddressCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
                oldEntityPM.ToAddressId = entityPM.ToAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCity))
            {
                oldEntityPM.ToAddressCity = entityPM.ToAddressCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCountryId))
            {
                oldEntityPM.ToAddressCountryId = entityPM.ToAddressCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressZipCode))
            {
                oldEntityPM.ToAddressZipCode = entityPM.ToAddressZipCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPPropertiesPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FromAddressCity)) //T4 find type == nText 
            {
                entityPM.FromAddressCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FromAddressCity));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ToAddressCity)) //T4 find type == nText 
            {
                entityPM.ToAddressCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ToAddressCity));
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
		
		private void BuildSearchFieldsGenerated(QuoteOPPropertiesPM entityPM, QuoteOPProperties entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 