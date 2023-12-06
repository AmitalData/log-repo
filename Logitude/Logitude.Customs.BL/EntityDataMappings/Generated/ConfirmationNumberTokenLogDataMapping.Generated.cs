
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
   
   public partial class ConfirmationNumberTokenLogDataMapping: IMapping<ConfirmationNumberTokenLogPM, ConfirmationNumberTokenLog>,IMappingEncodeBase64NVARCHARFields<ConfirmationNumberTokenLogPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         CreateDate, 
	         Tenant, 
	         CompanyIdInvoiceProducer, 
	         CompanyIdInvoiceRecipient, 
	         InvoiceNumber, 
	         CallType, 
	         CommunicationType, 
	         CommunicationLogId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         CreateDate, 
	         Tenant, 
	         CompanyIdInvoiceProducer, 
	         CompanyIdInvoiceRecipient, 
	         InvoiceNumber, 
	         CallType, 
	         CommunicationType, 
	         CommunicationLogId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyIdInvoiceProducer))
            {
				entityPOCO.CompanyIdInvoiceProducer = entityPM.CompanyIdInvoiceProducer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyIdInvoiceRecipient))
            {
				entityPOCO.CompanyIdInvoiceRecipient = entityPM.CompanyIdInvoiceRecipient;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
				entityPOCO.InvoiceNumber = entityPM.InvoiceNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallType))
            {
				entityPOCO.CallType = entityPM.CallType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationType))
            {
				entityPOCO.CommunicationType = entityPM.CommunicationType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
				entityPOCO.CommunicationLogId = entityPM.CommunicationLogId;
			}
			}

		public void POCOToPM(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CompanyIdInvoiceProducer))
            {
					entityPM.CompanyIdInvoiceProducer = entityPOCO.CompanyIdInvoiceProducer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CompanyIdInvoiceRecipient))
            {
					entityPM.CompanyIdInvoiceRecipient = entityPOCO.CompanyIdInvoiceRecipient;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceNumber))
            {
					entityPM.InvoiceNumber = entityPOCO.InvoiceNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallType))
            {
					entityPM.CallType = entityPOCO.CallType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationType))
            {
					entityPM.CommunicationType = entityPOCO.CommunicationType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationLogId))
            {
					entityPM.CommunicationLogId = entityPOCO.CommunicationLogId;
            }

		}

		public void PMToOldPM(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLogPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyIdInvoiceProducer))
            {
                oldEntityPM.CompanyIdInvoiceProducer = entityPM.CompanyIdInvoiceProducer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyIdInvoiceRecipient))
            {
                oldEntityPM.CompanyIdInvoiceRecipient = entityPM.CompanyIdInvoiceRecipient;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
                oldEntityPM.InvoiceNumber = entityPM.InvoiceNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallType))
            {
                oldEntityPM.CallType = entityPM.CallType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationType))
            {
                oldEntityPM.CommunicationType = entityPM.CommunicationType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
                oldEntityPM.CommunicationLogId = entityPM.CommunicationLogId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConfirmationNumberTokenLogPM entityPM)
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
	 