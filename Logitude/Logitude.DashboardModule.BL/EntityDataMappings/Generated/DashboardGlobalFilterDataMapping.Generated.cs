
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
   
   public partial class DashboardGlobalFilterDataMapping: IMapping<DashboardGlobalFilterPM, DashboardGlobalFilter>,IMappingEncodeBase64NVARCHARFields<DashboardGlobalFilterPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DashboardId, 
	         IsCommonFilter, 
	         CommonFilterField, 
	         DataSetId, 
	         DataSetFieldId, 
	         FilterOperator, 
	         DataTypeCode, 
	         LineNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DashboardId, 
	         IsCommonFilter, 
	         CommonFilterField, 
	         DataSetId, 
	         DataSetFieldId, 
	         FilterOperator, 
	         DataTypeCode, 
	         LineNumber, 
	         JoinedTableName, 
	         FieldCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DashboardGlobalFilterPM entityPM, DashboardGlobalFilter entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
				entityPOCO.DashboardId = entityPM.DashboardId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCommonFilter))
            {
				entityPOCO.IsCommonFilter = entityPM.IsCommonFilter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommonFilterField))
            {
				entityPOCO.CommonFilterField = entityPM.CommonFilterField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataSetId))
            {
				entityPOCO.DataSetId = entityPM.DataSetId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataSetFieldId))
            {
				entityPOCO.DataSetFieldId = entityPM.DataSetFieldId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterOperator))
            {
				entityPOCO.FilterOperator = entityPM.FilterOperator;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
				entityPOCO.DataTypeCode = entityPM.DataTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			}

		public void POCOToPM(DashboardGlobalFilterPM entityPM, DashboardGlobalFilter entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DashboardId))
            {
					entityPM.DashboardId = entityPOCO.DashboardId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCommonFilter))
            {
					entityPM.IsCommonFilter = entityPOCO.IsCommonFilter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommonFilterField))
            {
					entityPM.CommonFilterField = entityPOCO.CommonFilterField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DataSetId))
            {
					entityPM.DataSetId = entityPOCO.DataSetId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DataSetFieldId))
            {
					entityPM.DataSetFieldId = entityPOCO.DataSetFieldId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FilterOperator))
            {
					entityPM.FilterOperator = entityPOCO.FilterOperator;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DataTypeCode))
            {
					entityPM.DataTypeCode = entityPOCO.DataTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

		}

		public void PMToOldPM(DashboardGlobalFilterPM entityPM, DashboardGlobalFilterPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
                oldEntityPM.DashboardId = entityPM.DashboardId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCommonFilter))
            {
                oldEntityPM.IsCommonFilter = entityPM.IsCommonFilter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommonFilterField))
            {
                oldEntityPM.CommonFilterField = entityPM.CommonFilterField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataSetId))
            {
                oldEntityPM.DataSetId = entityPM.DataSetId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataSetFieldId))
            {
                oldEntityPM.DataSetFieldId = entityPM.DataSetFieldId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterOperator))
            {
                oldEntityPM.FilterOperator = entityPM.FilterOperator;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
                oldEntityPM.DataTypeCode = entityPM.DataTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DashboardGlobalFilterPM entityPM)
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
	 