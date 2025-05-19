
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class CustomerDebtNotificationDataMapping: IMapping<CustomerDebtNotificationPM, CustomerDebtNotification>,IMappingEncodeBase64NVARCHARFields<CustomerDebtNotificationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InActive, 
	         TypesDebts, 
	         DebtLevel, 
	         DebtLevelAmount, 
	         TasksSchedulerId, 
	         PaymentNotes, 
	         AccountId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InActive, 
	         TypesDebts, 
	         DebtLevel, 
	         DebtLevelAmount, 
	         TasksSchedulerId, 
	         PaymentNotes, 
	         AccountId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomerDebtNotificationPM entityPM, CustomerDebtNotification entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
				entityPOCO.InActive = entityPM.InActive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypesDebts))
            {
				entityPOCO.TypesDebts = entityPM.TypesDebts;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtLevel))
            {
				entityPOCO.DebtLevel = entityPM.DebtLevel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtLevelAmount))
            {
				entityPOCO.DebtLevelAmount = entityPM.DebtLevelAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TasksSchedulerId))
            {
				entityPOCO.TasksSchedulerId = entityPM.TasksSchedulerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentNotes))
            {
				entityPOCO.PaymentNotes = entityPM.PaymentNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountId))
            {
				entityPOCO.AccountId = entityPM.AccountId;
			}
			}

		public void POCOToPM(CustomerDebtNotificationPM entityPM, CustomerDebtNotification entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InActive))
            {
					entityPM.InActive = entityPOCO.InActive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypesDebts))
            {
					entityPM.TypesDebts = entityPOCO.TypesDebts;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DebtLevel))
            {
					entityPM.DebtLevel = entityPOCO.DebtLevel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DebtLevelAmount))
            {
					entityPM.DebtLevelAmount = entityPOCO.DebtLevelAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TasksSchedulerId))
            {
					entityPM.TasksSchedulerId = entityPOCO.TasksSchedulerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentNotes))
            {
					entityPM.PaymentNotes = entityPOCO.PaymentNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountId))
            {
					entityPM.AccountId = entityPOCO.AccountId;
            }

		}

		public void PMToOldPM(CustomerDebtNotificationPM entityPM, CustomerDebtNotificationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
                oldEntityPM.InActive = entityPM.InActive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypesDebts))
            {
                oldEntityPM.TypesDebts = entityPM.TypesDebts;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtLevel))
            {
                oldEntityPM.DebtLevel = entityPM.DebtLevel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtLevelAmount))
            {
                oldEntityPM.DebtLevelAmount = entityPM.DebtLevelAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TasksSchedulerId))
            {
                oldEntityPM.TasksSchedulerId = entityPM.TasksSchedulerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentNotes))
            {
                oldEntityPM.PaymentNotes = entityPM.PaymentNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountId))
            {
                oldEntityPM.AccountId = entityPM.AccountId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomerDebtNotificationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.DebtLevel)) //T4 find type == nText 
            {
                entityPM.DebtLevel = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DebtLevel));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PaymentNotes)) //T4 find type == nText 
            {
                entityPM.PaymentNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PaymentNotes));
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
	 