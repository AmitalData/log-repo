
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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class TaskExtendedDataMapping: IMapping<TaskExtendedPM, TaskExtended>,IMappingEncodeBase64NVARCHARFields<TaskExtendedPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Fields, 
	         ToDoConditions, 
	         DoneConditions, 
	         Description,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Fields, 
	         ToDoConditions, 
	         DoneConditions, 
	         Description,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TaskExtendedPM entityPM, TaskExtended entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Fields))
            {
				entityPOCO.Fields = entityPM.Fields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDoConditions))
            {
				entityPOCO.ToDoConditions = entityPM.ToDoConditions;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DoneConditions))
            {
				entityPOCO.DoneConditions = entityPM.DoneConditions;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			}

		public void POCOToPM(TaskExtendedPM entityPM, TaskExtended entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Fields))
            {
					entityPM.Fields = entityPOCO.Fields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToDoConditions))
            {
					entityPM.ToDoConditions = entityPOCO.ToDoConditions;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DoneConditions))
            {
					entityPM.DoneConditions = entityPOCO.DoneConditions;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

		}

		public void PMToOldPM(TaskExtendedPM entityPM, TaskExtendedPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Fields))
            {
                oldEntityPM.Fields = entityPM.Fields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDoConditions))
            {
                oldEntityPM.ToDoConditions = entityPM.ToDoConditions;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DoneConditions))
            {
                oldEntityPM.DoneConditions = entityPM.DoneConditions;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TaskExtendedPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Fields)) //T4 find type == nText 
            {
                entityPM.Fields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Fields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ToDoConditions)) //T4 find type == nText 
            {
                entityPM.ToDoConditions = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ToDoConditions));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DoneConditions)) //T4 find type == nText 
            {
                entityPM.DoneConditions = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DoneConditions));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
	 