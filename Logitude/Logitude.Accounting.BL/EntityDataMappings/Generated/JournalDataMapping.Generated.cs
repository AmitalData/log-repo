
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
   
   public partial class JournalDataMapping: IMapping<JournalPM, Journal>,IMappingEncodeBase64NVARCHARFields<JournalPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         JournalNumber, 
	         CreateDate, 
	         AccountingDate, 
	         TypeCode, 
	         StatusCode, 
	         CreatedByUserId, 
	         AccountingEntityCode, 
	         AccountingEntityId, 
	         ExternalNo, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         ApproveDate, 
	         ApprovedByUserId, 
	         SearchFields, 
	         AccountingEntityReference, 
	         OriginalJournalId, 
	         VoidedByUserId, 
	         VoidDate, 
	         IsVoided, 
	         VoidedByJournalId, 
	         ExternalSystem, 
	         QueueId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         JournalNumber, 
	         CreateDate, 
	         AccountingDate, 
	         TypeCode, 
	         StatusCode, 
	         CreatedByUserId, 
	         AccountingEntityCode, 
	         AccountingEntityId, 
	         ExternalNo, 
	         TypeName, 
	         StatusName, 
	         CreatedByUserName, 
	         AccountingEntityName, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         ApproveDate, 
	         ApprovedByUserId, 
	         UpdatedByUserName, 
	         ApprovedByUserName, 
	         SearchFields, 
	         AccountingEntityReference, 
	         OriginalJournalId, 
	         VoidedByUserId, 
	         VoidDate, 
	         OriginalJournalName, 
	         VoidedByUserName, 
	         IsVoided, 
	         VoidedByJournalId, 
	         ExternalSystem, 
	         QueueId, 
	         StatusLocalName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(JournalPM entityPM, Journal entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JournalNumber))
            {
				entityPOCO.JournalNumber = entityPM.JournalNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingDate))
            {
				entityPOCO.AccountingDate = entityPM.AccountingDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityCode))
            {
				entityPOCO.AccountingEntityCode = entityPM.AccountingEntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityId))
            {
				entityPOCO.AccountingEntityId = entityPM.AccountingEntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalNo))
            {
				entityPOCO.ExternalNo = entityPM.ExternalNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
				entityPOCO.ApproveDate = entityPM.ApproveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
				entityPOCO.ApprovedByUserId = entityPM.ApprovedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityReference))
            {
				entityPOCO.AccountingEntityReference = entityPM.AccountingEntityReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalJournalId))
            {
				entityPOCO.OriginalJournalId = entityPM.OriginalJournalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidedByUserId))
            {
				entityPOCO.VoidedByUserId = entityPM.VoidedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidDate))
            {
				entityPOCO.VoidDate = entityPM.VoidDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVoided))
            {
				entityPOCO.IsVoided = entityPM.IsVoided;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidedByJournalId))
            {
				entityPOCO.VoidedByJournalId = entityPM.VoidedByJournalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalSystem))
            {
				entityPOCO.ExternalSystem = entityPM.ExternalSystem;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueId))
            {
				entityPOCO.QueueId = entityPM.QueueId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(JournalPM entityPM, Journal entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JournalNumber))
            {
					entityPM.JournalNumber = entityPOCO.JournalNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingDate))
            {
					entityPM.AccountingDate = entityPOCO.AccountingDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingEntityCode))
            {
					entityPM.AccountingEntityCode = entityPOCO.AccountingEntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingEntityId))
            {
					entityPM.AccountingEntityId = entityPOCO.AccountingEntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalNo))
            {
					entityPM.ExternalNo = entityPOCO.ExternalNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApproveDate))
            {
					entityPM.ApproveDate = entityPOCO.ApproveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovedByUserId))
            {
					entityPM.ApprovedByUserId = entityPOCO.ApprovedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingEntityReference))
            {
					entityPM.AccountingEntityReference = entityPOCO.AccountingEntityReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalJournalId))
            {
					entityPM.OriginalJournalId = entityPOCO.OriginalJournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VoidedByUserId))
            {
					entityPM.VoidedByUserId = entityPOCO.VoidedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VoidDate))
            {
					entityPM.VoidDate = entityPOCO.VoidDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsVoided))
            {
					entityPM.IsVoided = entityPOCO.IsVoided;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VoidedByJournalId))
            {
					entityPM.VoidedByJournalId = entityPOCO.VoidedByJournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalSystem))
            {
					entityPM.ExternalSystem = entityPOCO.ExternalSystem;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QueueId))
            {
					entityPM.QueueId = entityPOCO.QueueId;
            }

		}

		public void PMToOldPM(JournalPM entityPM, JournalPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JournalNumber))
            {
                oldEntityPM.JournalNumber = entityPM.JournalNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingDate))
            {
                oldEntityPM.AccountingDate = entityPM.AccountingDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityCode))
            {
                oldEntityPM.AccountingEntityCode = entityPM.AccountingEntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityId))
            {
                oldEntityPM.AccountingEntityId = entityPM.AccountingEntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalNo))
            {
                oldEntityPM.ExternalNo = entityPM.ExternalNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
                oldEntityPM.ApproveDate = entityPM.ApproveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
                oldEntityPM.ApprovedByUserId = entityPM.ApprovedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityReference))
            {
                oldEntityPM.AccountingEntityReference = entityPM.AccountingEntityReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalJournalId))
            {
                oldEntityPM.OriginalJournalId = entityPM.OriginalJournalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidedByUserId))
            {
                oldEntityPM.VoidedByUserId = entityPM.VoidedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidDate))
            {
                oldEntityPM.VoidDate = entityPM.VoidDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVoided))
            {
                oldEntityPM.IsVoided = entityPM.IsVoided;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoidedByJournalId))
            {
                oldEntityPM.VoidedByJournalId = entityPM.VoidedByJournalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalSystem))
            {
                oldEntityPM.ExternalSystem = entityPM.ExternalSystem;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueId))
            {
                oldEntityPM.QueueId = entityPM.QueueId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(JournalPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExternalSystem)) //T4 find type == nText 
            {
                entityPM.ExternalSystem = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExternalSystem));
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
		
		private void BuildSearchFieldsGenerated(JournalPM entityPM, Journal entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 