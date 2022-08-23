
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class WidgetDataMapping: IMapping<WidgetPM, Widget>,IMappingEncodeBase64NVARCHARFields<WidgetPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Title, 
	         GroupBy, 
	         DashboardId, 
	         StartPotistion, 
	         EndPosition, 
	         TypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Title, 
	         GroupBy, 
	         DashboardId, 
	         StartPotistion, 
	         EndPosition, 
	         TypeCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WidgetPM entityPM, Widget entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupBy))
            {
				entityPOCO.GroupBy = entityPM.GroupBy;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
				entityPOCO.DashboardId = entityPM.DashboardId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartPotistion))
            {
				entityPOCO.StartPotistion = entityPM.StartPotistion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndPosition))
            {
				entityPOCO.EndPosition = entityPM.EndPosition;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
			}
			}

		public void POCOToPM(WidgetPM entityPM, Widget entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupBy))
            {
					entityPM.GroupBy = entityPOCO.GroupBy;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DashboardId))
            {
					entityPM.DashboardId = entityPOCO.DashboardId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartPotistion))
            {
					entityPM.StartPotistion = entityPOCO.StartPotistion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndPosition))
            {
					entityPM.EndPosition = entityPOCO.EndPosition;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
            }

		}

		public void PMToOldPM(WidgetPM entityPM, WidgetPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupBy))
            {
                oldEntityPM.GroupBy = entityPM.GroupBy;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
                oldEntityPM.DashboardId = entityPM.DashboardId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartPotistion))
            {
                oldEntityPM.StartPotistion = entityPM.StartPotistion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndPosition))
            {
                oldEntityPM.EndPosition = entityPM.EndPosition;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WidgetPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Title)) //T4 find type == nText 
            {
                entityPM.Title = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Title));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StartPotistion)) //T4 find type == nText 
            {
                entityPM.StartPotistion = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StartPotistion));
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
	 