
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
   
   public partial class CustomsRequiredFieldDataMapping: IMapping<CustomsRequiredFieldPM, CustomsRequiredField>,IMappingEncodeBase64NVARCHARFields<CustomsRequiredFieldPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ObjectTableId, 
	         ObjectfieldId, 
	         ObjectfieldCode, 
	         IsImport, 
	         IsExport,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ObjectTableId, 
	         ObjectfieldId, 
	         ObjectFieldName, 
	         ObjectfieldCode, 
	         IsImport, 
	         IsExport,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsRequiredFieldPM entityPM, CustomsRequiredField entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectfieldId))
            {
				entityPOCO.ObjectfieldId = entityPM.ObjectfieldId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectfieldCode))
            {
				entityPOCO.ObjectfieldCode = entityPM.ObjectfieldCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
				entityPOCO.IsImport = entityPM.IsImport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
				entityPOCO.IsExport = entityPM.IsExport;
			}
			}

		public void POCOToPM(CustomsRequiredFieldPM entityPM, CustomsRequiredField entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectfieldId))
            {
					entityPM.ObjectfieldId = entityPOCO.ObjectfieldId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectfieldCode))
            {
					entityPM.ObjectfieldCode = entityPOCO.ObjectfieldCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImport))
            {
					entityPM.IsImport = entityPOCO.IsImport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExport))
            {
					entityPM.IsExport = entityPOCO.IsExport;
            }

		}

		public void PMToOldPM(CustomsRequiredFieldPM entityPM, CustomsRequiredFieldPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectfieldId))
            {
                oldEntityPM.ObjectfieldId = entityPM.ObjectfieldId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectfieldCode))
            {
                oldEntityPM.ObjectfieldCode = entityPM.ObjectfieldCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
                oldEntityPM.IsImport = entityPM.IsImport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
                oldEntityPM.IsExport = entityPM.IsExport;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsRequiredFieldPM entityPM)
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
	 