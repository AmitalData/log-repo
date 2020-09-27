
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
   
   public partial class TicketEscalationDataMapping: IMapping<TicketEscalationPM, TicketEscalation>,IMappingEncodeBase64NVARCHARFields<TicketEscalationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         TicketId, 
	         LineNumber, 
	         EscalationFor, 
	         Recepients, 
	         IsClose, 
	         IsSLAViolated, 
	         DueDate, 
	         CloseDate, 
	         UpdateDate, 
	         SearchFields,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         TicketId, 
	         LineNumber, 
	         EscalationFor, 
	         Recepients, 
	         IsClose, 
	         IsSLAViolated, 
	         DueDate, 
	         CloseDate, 
	         UpdateDate, 
	         EscalationForName, 
	         SearchFields,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TicketEscalationPM entityPM, TicketEscalation entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketId))
            {
				entityPOCO.TicketId = entityPM.TicketId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationFor))
            {
				entityPOCO.EscalationFor = entityPM.EscalationFor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Recepients))
            {
				entityPOCO.Recepients = entityPM.Recepients;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClose))
            {
				entityPOCO.IsClose = entityPM.IsClose;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSLAViolated))
            {
				entityPOCO.IsSLAViolated = entityPM.IsSLAViolated;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
				entityPOCO.DueDate = entityPM.DueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseDate))
            {
				entityPOCO.CloseDate = entityPM.CloseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TicketEscalationPM entityPM, TicketEscalation entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TicketId))
            {
					entityPM.TicketId = entityPOCO.TicketId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationFor))
            {
					entityPM.EscalationFor = entityPOCO.EscalationFor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Recepients))
            {
					entityPM.Recepients = entityPOCO.Recepients;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClose))
            {
					entityPM.IsClose = entityPOCO.IsClose;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSLAViolated))
            {
					entityPM.IsSLAViolated = entityPOCO.IsSLAViolated;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DueDate))
            {
					entityPM.DueDate = entityPOCO.DueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CloseDate))
            {
					entityPM.CloseDate = entityPOCO.CloseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

		}

		public void PMToOldPM(TicketEscalationPM entityPM, TicketEscalationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketId))
            {
                oldEntityPM.TicketId = entityPM.TicketId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationFor))
            {
                oldEntityPM.EscalationFor = entityPM.EscalationFor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Recepients))
            {
                oldEntityPM.Recepients = entityPM.Recepients;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClose))
            {
                oldEntityPM.IsClose = entityPM.IsClose;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSLAViolated))
            {
                oldEntityPM.IsSLAViolated = entityPM.IsSLAViolated;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
                oldEntityPM.DueDate = entityPM.DueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseDate))
            {
                oldEntityPM.CloseDate = entityPM.CloseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TicketEscalationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
		
		private void BuildSearchFieldsGenerated(TicketEscalationPM entityPM, TicketEscalation entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 