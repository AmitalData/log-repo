
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
   
   public partial class CustomsDocumentMetaDataValueDataMapping: IMapping<CustomsDocumentMetaDataValuePM, CustomsDocumentMetaDataValue>,IMappingEncodeBase64NVARCHARFields<CustomsDocumentMetaDataValuePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CustomsDocumentId, 
	         Tenant, 
	         MetaDataTypeCode, 
	         MetaDataValue,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CustomsDocumentId, 
	         Tenant, 
	         MetaDataTypeCode, 
	         MetaDataValue, 
	         MetaDataTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsDocumentMetaDataValuePM entityPM, CustomsDocumentMetaDataValue entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MetaDataValue))
            {
				entityPOCO.MetaDataValue = entityPM.MetaDataValue;
			}
			}

		public void POCOToPM(CustomsDocumentMetaDataValuePM entityPM, CustomsDocumentMetaDataValue entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsDocumentId))
            {
					entityPM.CustomsDocumentId = entityPOCO.CustomsDocumentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MetaDataTypeCode))
            {
					entityPM.MetaDataTypeCode = entityPOCO.MetaDataTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MetaDataValue))
            {
					entityPM.MetaDataValue = entityPOCO.MetaDataValue;
            }

		}

		public void PMToOldPM(CustomsDocumentMetaDataValuePM entityPM, CustomsDocumentMetaDataValuePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MetaDataValue))
            {
                oldEntityPM.MetaDataValue = entityPM.MetaDataValue;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsDocumentMetaDataValuePM entityPM)
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
	 