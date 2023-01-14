
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class DashboardGlobalPresetFilterDataMapping: IMapping<DashboardGlobalPresetFilterPM, DashboardGlobalPresetFilter>,IMappingEncodeBase64NVARCHARFields<DashboardGlobalPresetFilterPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         DisplayName, 
	         DataTypeCode, 
	         IsDisabled, 
	         IsMultiSelect, 
	         JoinedTableName, 
	         Sort, 
	         JoinedTableDisplayField, 
	         CanSearch,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         DisplayName, 
	         DataTypeCode, 
	         IsDisabled, 
	         IsMultiSelect, 
	         JoinedTableName, 
	         Sort, 
	         JoinedTableDisplayField, 
	         CanSearch,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DashboardGlobalPresetFilterPM entityPM, DashboardGlobalPresetFilter entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayName))
            {
				entityPOCO.DisplayName = entityPM.DisplayName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
				entityPOCO.DataTypeCode = entityPM.DataTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDisabled))
            {
				entityPOCO.IsDisabled = entityPM.IsDisabled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiSelect))
            {
				entityPOCO.IsMultiSelect = entityPM.IsMultiSelect;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableName))
            {
				entityPOCO.JoinedTableName = entityPM.JoinedTableName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sort))
            {
				entityPOCO.Sort = entityPM.Sort;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDisplayField))
            {
				entityPOCO.JoinedTableDisplayField = entityPM.JoinedTableDisplayField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanSearch))
            {
				entityPOCO.CanSearch = entityPM.CanSearch;
			}
			}

		public void POCOToPM(DashboardGlobalPresetFilterPM entityPM, DashboardGlobalPresetFilter entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DisplayName))
            {
					entityPM.DisplayName = entityPOCO.DisplayName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DataTypeCode))
            {
					entityPM.DataTypeCode = entityPOCO.DataTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDisabled))
            {
					entityPM.IsDisabled = entityPOCO.IsDisabled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMultiSelect))
            {
					entityPM.IsMultiSelect = entityPOCO.IsMultiSelect;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableName))
            {
					entityPM.JoinedTableName = entityPOCO.JoinedTableName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Sort))
            {
					entityPM.Sort = entityPOCO.Sort;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableDisplayField))
            {
					entityPM.JoinedTableDisplayField = entityPOCO.JoinedTableDisplayField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CanSearch))
            {
					entityPM.CanSearch = entityPOCO.CanSearch;
            }

		}

		public void PMToOldPM(DashboardGlobalPresetFilterPM entityPM, DashboardGlobalPresetFilterPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayName))
            {
                oldEntityPM.DisplayName = entityPM.DisplayName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
                oldEntityPM.DataTypeCode = entityPM.DataTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDisabled))
            {
                oldEntityPM.IsDisabled = entityPM.IsDisabled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiSelect))
            {
                oldEntityPM.IsMultiSelect = entityPM.IsMultiSelect;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableName))
            {
                oldEntityPM.JoinedTableName = entityPM.JoinedTableName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sort))
            {
                oldEntityPM.Sort = entityPM.Sort;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDisplayField))
            {
                oldEntityPM.JoinedTableDisplayField = entityPM.JoinedTableDisplayField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanSearch))
            {
                oldEntityPM.CanSearch = entityPM.CanSearch;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DashboardGlobalPresetFilterPM entityPM)
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
	 