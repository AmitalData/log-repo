
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class TicketClassificationDataMapping: IMapping<TicketClassificationPM, TicketClassification>,IMappingEncodeBase64NVARCHARFields<TicketClassificationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         SearchFields, 
	         Inactive, 
	         ParentId, 
	         DefaultSeverityId, 
	         EmployeeGroupId, 
	         ManagerUserId, 
	         EscalationNotify,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         SearchFields, 
	         Inactive, 
	         ParentId, 
	         ParentName, 
	         DefaultSeverityId, 
	         EmployeeGroupId, 
	         ManagerUserId, 
	         EscalationNotify, 
	         ManagerUserEmail,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TicketClassificationPM entityPM, TicketClassification entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentId))
            {
				entityPOCO.ParentId = entityPM.ParentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultSeverityId))
            {
				entityPOCO.DefaultSeverityId = entityPM.DefaultSeverityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
				entityPOCO.EmployeeGroupId = entityPM.EmployeeGroupId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManagerUserId))
            {
				entityPOCO.ManagerUserId = entityPM.ManagerUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationNotify))
            {
				entityPOCO.EscalationNotify = entityPM.EscalationNotify;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TicketClassificationPM entityPM, TicketClassification entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentId))
            {
					entityPM.ParentId = entityPOCO.ParentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultSeverityId))
            {
					entityPM.DefaultSeverityId = entityPOCO.DefaultSeverityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmployeeGroupId))
            {
					entityPM.EmployeeGroupId = entityPOCO.EmployeeGroupId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManagerUserId))
            {
					entityPM.ManagerUserId = entityPOCO.ManagerUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationNotify))
            {
					entityPM.EscalationNotify = entityPOCO.EscalationNotify;
            }

		}

		public void PMToOldPM(TicketClassificationPM entityPM, TicketClassificationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentId))
            {
                oldEntityPM.ParentId = entityPM.ParentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultSeverityId))
            {
                oldEntityPM.DefaultSeverityId = entityPM.DefaultSeverityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
                oldEntityPM.EmployeeGroupId = entityPM.EmployeeGroupId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManagerUserId))
            {
                oldEntityPM.ManagerUserId = entityPM.ManagerUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationNotify))
            {
                oldEntityPM.EscalationNotify = entityPM.EscalationNotify;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TicketClassificationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(TicketClassificationPM entityPM, TicketClassification entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 