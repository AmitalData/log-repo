
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
   
   public partial class ClientItemDataMapping: IMapping<ClientItemPM, ClientItem>,IMappingEncodeBase64NVARCHARFields<ClientItemPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         ItemDescription, 
	         ClassificationCode, 
	         ItemCode, 
	         OriginCountryCode, 
	         ClientCode, 
	         Id,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         ItemDescription, 
	         ClassificationCode, 
	         ItemCode, 
	         OriginCountryCode, 
	         OriginCountryName, 
	         ClientCode, 
	         Id,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientItemPM entityPM, ClientItem entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemDescription))
            {
				entityPOCO.ItemDescription = entityPM.ItemDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationCode))
            {
				entityPOCO.ClassificationCode = entityPM.ClassificationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
				entityPOCO.OriginCountryCode = entityPM.OriginCountryCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ClientItemPM entityPM, ClientItem entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemDescription))
            {
					entityPM.ItemDescription = entityPOCO.ItemDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassificationCode))
            {
					entityPM.ClassificationCode = entityPOCO.ClassificationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemCode))
            {
					entityPM.ItemCode = entityPOCO.ItemCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginCountryCode))
            {
					entityPM.OriginCountryCode = entityPOCO.OriginCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientCode))
            {
					entityPM.ClientCode = entityPOCO.ClientCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

		}

		public void PMToOldPM(ClientItemPM entityPM, ClientItemPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemDescription))
            {
                oldEntityPM.ItemDescription = entityPM.ItemDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationCode))
            {
                oldEntityPM.ClassificationCode = entityPM.ClassificationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
                oldEntityPM.OriginCountryCode = entityPM.OriginCountryCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientItemPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ItemDescription)) //T4 find type == nText 
            {
                entityPM.ItemDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ItemDescription));
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
		
		private void BuildSearchFieldsGenerated(ClientItemPM entityPM, ClientItem entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 