
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
   
   public partial class WidgetMeasureDataMapping: IMapping<WidgetMeasurePM, WidgetMeasure>,IMappingEncodeBase64NVARCHARFields<WidgetMeasurePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         WidgetId, 
	         MeasureCode, 
	         MeasureFieldId, 
	         RenderAs, 
	         YAxisType,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         WidgetId, 
	         MeasureCode, 
	         MeasureFieldId, 
	         RenderAs, 
	         YAxisType,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WidgetMeasurePM entityPM, WidgetMeasure entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WidgetId))
            {
				entityPOCO.WidgetId = entityPM.WidgetId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasureCode))
            {
				entityPOCO.MeasureCode = entityPM.MeasureCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasureFieldId))
            {
				entityPOCO.MeasureFieldId = entityPM.MeasureFieldId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenderAs))
            {
				entityPOCO.RenderAs = entityPM.RenderAs;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.YAxisType))
            {
				entityPOCO.YAxisType = entityPM.YAxisType;
			}
			}

		public void POCOToPM(WidgetMeasurePM entityPM, WidgetMeasure entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WidgetId))
            {
					entityPM.WidgetId = entityPOCO.WidgetId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeasureCode))
            {
					entityPM.MeasureCode = entityPOCO.MeasureCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeasureFieldId))
            {
					entityPM.MeasureFieldId = entityPOCO.MeasureFieldId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RenderAs))
            {
					entityPM.RenderAs = entityPOCO.RenderAs;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.YAxisType))
            {
					entityPM.YAxisType = entityPOCO.YAxisType;
            }

		}

		public void PMToOldPM(WidgetMeasurePM entityPM, WidgetMeasurePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WidgetId))
            {
                oldEntityPM.WidgetId = entityPM.WidgetId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasureCode))
            {
                oldEntityPM.MeasureCode = entityPM.MeasureCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasureFieldId))
            {
                oldEntityPM.MeasureFieldId = entityPM.MeasureFieldId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenderAs))
            {
                oldEntityPM.RenderAs = entityPM.RenderAs;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.YAxisType))
            {
                oldEntityPM.YAxisType = entityPM.YAxisType;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WidgetMeasurePM entityPM)
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
	 