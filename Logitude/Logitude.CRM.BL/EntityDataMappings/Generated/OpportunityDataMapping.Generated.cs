
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
   
   public partial class OpportunityDataMapping: IMapping<OpportunityPM, Opportunity>,IMappingEncodeBase64NVARCHARFields<OpportunityPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         OwnerId, 
	         Subject, 
	         CustomerId, 
	         LeadSourceId, 
	         ContactId, 
	         EstimatedClosingDate, 
	         StageId, 
	         Probability, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         RatingCode, 
	         IsClosed, 
	         ActualClosingDate, 
	         ClosingDescription, 
	         SearchFields, 
	         NumberOfShipments, 
	         ValueField, 
	         LastStageDate, 
	         LastStageIdBeforeClosure, 
	         Field1, 
	         Field2, 
	         Field3, 
	         Field4, 
	         Field5, 
	         Field6, 
	         Field7, 
	         Field8, 
	         Field9, 
	         Field10, 
	         LastCompletedActivityDate, 
	         LeadDescription, 
	         LastCompletedActivityTypeCode, 
	         LastActivitySubject, 
	         NextActivityDate, 
	         NextActivityTypeCode, 
	         NextActivitySubject, 
	         Notes, 
	         StageDueDate, 
	         BusinessUnitId, 
	         LeadUserId, 
	         LeadPartnerId, 
	         AgentId, 
	         ForeignClientId, 
	         ConcurrencyGUID, 
	         ClosingReasonId, 
	         IsCancelled, 
	         OpportunityTypeId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         OwnerId, 
	         Subject, 
	         CustomerId, 
	         LeadSourceId, 
	         ContactId, 
	         EstimatedClosingDate, 
	         StageId, 
	         Probability, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         RatingCode, 
	         IsClosed, 
	         ActualClosingDate, 
	         ClosingDescription, 
	         CustomerName, 
	         SearchFields, 
	         OwnerName, 
	         StageName, 
	         RatingName, 
	         ClosedToCompetitorId, 
	         NumberOfShipments, 
	         ValueField, 
	         LastStageDate, 
	         LastStageIdBeforeClosure, 
	         Field1, 
	         Field2, 
	         Field3, 
	         Field4, 
	         Field5, 
	         Field6, 
	         Field7, 
	         Field8, 
	         Field9, 
	         Field10, 
	         LastCompletedActivityDate, 
	         LeadDescription, 
	         LastCompletedActivityTypeCode, 
	         LastActivitySubject, 
	         NextActivityDate, 
	         NextActivityTypeCode, 
	         NextActivitySubject, 
	         Notes, 
	         StageDueDate, 
	         BusinessUnitId, 
	         StageProbability, 
	         RatingIndexOrder, 
	         LeadUserId, 
	         LeadPartnerId, 
	         AgentId, 
	         ForeignClientId, 
	         ContactName, 
	         ContactPhone, 
	         ConcurrencyGUID, 
	         ClosingReasonId, 
	         IsCancelled, 
	         ClosingReasonName, 
	         LeadSourceName, 
	         OpportunityTypeId, 
	         OpportunityTypeName, 
	         LeadPartnerName, 
	         IsClosedLost, 
	         CustomerRankCode, 
	         CustomerRankName, 
	         PostToFollowersAsWon, 
	         ClosingReasonCode, 
	         CustomerExternalId, 
	         IsCopy, 
	         CopyFromEntityId, 
	         IsCustomerBlockedBusinessUnit,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OpportunityPM entityPM, Opportunity entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
				entityPOCO.Subject = entityPM.Subject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadSourceId))
            {
				entityPOCO.LeadSourceId = entityPM.LeadSourceId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
				entityPOCO.ContactId = entityPM.ContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedClosingDate))
            {
				entityPOCO.EstimatedClosingDate = entityPM.EstimatedClosingDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
				entityPOCO.StageId = entityPM.StageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Probability))
            {
				entityPOCO.Probability = entityPM.Probability;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RatingCode))
            {
				entityPOCO.RatingCode = entityPM.RatingCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualClosingDate))
            {
				entityPOCO.ActualClosingDate = entityPM.ActualClosingDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosingDescription))
            {
				entityPOCO.ClosingDescription = entityPM.ClosingDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
				entityPOCO.NumberOfShipments = entityPM.NumberOfShipments;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueField))
            {
				entityPOCO.ValueField = entityPM.ValueField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageDate))
            {
				entityPOCO.LastStageDate = entityPM.LastStageDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageIdBeforeClosure))
            {
				entityPOCO.LastStageIdBeforeClosure = entityPM.LastStageIdBeforeClosure;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field1))
            {
				entityPOCO.Field1 = entityPM.Field1!= null ? entityPM.Field1.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field2))
            {
				entityPOCO.Field2 = entityPM.Field2!= null ? entityPM.Field2.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field3))
            {
				entityPOCO.Field3 = entityPM.Field3!= null ? entityPM.Field3.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field4))
            {
				entityPOCO.Field4 = entityPM.Field4!= null ? entityPM.Field4.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field5))
            {
				entityPOCO.Field5 = entityPM.Field5!= null ? entityPM.Field5.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field6))
            {
				entityPOCO.Field6 = entityPM.Field6!= null ? entityPM.Field6.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field7))
            {
				entityPOCO.Field7 = entityPM.Field7!= null ? entityPM.Field7.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field8))
            {
				entityPOCO.Field8 = entityPM.Field8!= null ? entityPM.Field8.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field9))
            {
				entityPOCO.Field9 = entityPM.Field9!= null ? entityPM.Field9.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field10))
            {
				entityPOCO.Field10 = entityPM.Field10!= null ? entityPM.Field10.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityDate))
            {
				entityPOCO.LastCompletedActivityDate = entityPM.LastCompletedActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadDescription))
            {
				entityPOCO.LeadDescription = entityPM.LeadDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityTypeCode))
            {
				entityPOCO.LastCompletedActivityTypeCode = entityPM.LastCompletedActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivitySubject))
            {
				entityPOCO.LastActivitySubject = entityPM.LastActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
				entityPOCO.NextActivityDate = entityPM.NextActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
				entityPOCO.NextActivityTypeCode = entityPM.NextActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
				entityPOCO.NextActivitySubject = entityPM.NextActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageDueDate))
            {
				entityPOCO.StageDueDate = entityPM.StageDueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
				entityPOCO.BusinessUnitId = entityPM.BusinessUnitId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadUserId))
            {
				entityPOCO.LeadUserId = entityPM.LeadUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadPartnerId))
            {
				entityPOCO.LeadPartnerId = entityPM.LeadPartnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
				entityPOCO.AgentId = entityPM.AgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignClientId))
            {
				entityPOCO.ForeignClientId = entityPM.ForeignClientId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosingReasonId))
            {
				entityPOCO.ClosingReasonId = entityPM.ClosingReasonId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityTypeId))
            {
				entityPOCO.OpportunityTypeId = entityPM.OpportunityTypeId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(OpportunityPM entityPM, Opportunity entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Subject))
            {
					entityPM.Subject = entityPOCO.Subject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadSourceId))
            {
					entityPM.LeadSourceId = entityPOCO.LeadSourceId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactId))
            {
					entityPM.ContactId = entityPOCO.ContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedClosingDate))
            {
					entityPM.EstimatedClosingDate = entityPOCO.EstimatedClosingDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StageId))
            {
					entityPM.StageId = entityPOCO.StageId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Probability))
            {
					entityPM.Probability = entityPOCO.Probability;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RatingCode))
            {
					entityPM.RatingCode = entityPOCO.RatingCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualClosingDate))
            {
					entityPM.ActualClosingDate = entityPOCO.ActualClosingDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClosingDescription))
            {
					entityPM.ClosingDescription = entityPOCO.ClosingDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfShipments))
            {
					entityPM.NumberOfShipments = entityPOCO.NumberOfShipments;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueField))
            {
					entityPM.ValueField = entityPOCO.ValueField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStageDate))
            {
					entityPM.LastStageDate = entityPOCO.LastStageDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStageIdBeforeClosure))
            {
					entityPM.LastStageIdBeforeClosure = entityPOCO.LastStageIdBeforeClosure;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field1))
            {
				entityPM.Field1 = new CustomFieldClass("Field1", "Opportunity", entityPOCO.Field1);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field2))
            {
				entityPM.Field2 = new CustomFieldClass("Field2", "Opportunity", entityPOCO.Field2);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field3))
            {
				entityPM.Field3 = new CustomFieldClass("Field3", "Opportunity", entityPOCO.Field3);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field4))
            {
				entityPM.Field4 = new CustomFieldClass("Field4", "Opportunity", entityPOCO.Field4);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field5))
            {
				entityPM.Field5 = new CustomFieldClass("Field5", "Opportunity", entityPOCO.Field5);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field6))
            {
				entityPM.Field6 = new CustomFieldClass("Field6", "Opportunity", entityPOCO.Field6);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field7))
            {
				entityPM.Field7 = new CustomFieldClass("Field7", "Opportunity", entityPOCO.Field7);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field8))
            {
				entityPM.Field8 = new CustomFieldClass("Field8", "Opportunity", entityPOCO.Field8);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field9))
            {
				entityPM.Field9 = new CustomFieldClass("Field9", "Opportunity", entityPOCO.Field9);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field10))
            {
				entityPM.Field10 = new CustomFieldClass("Field10", "Opportunity", entityPOCO.Field10);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCompletedActivityDate))
            {
					entityPM.LastCompletedActivityDate = entityPOCO.LastCompletedActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadDescription))
            {
					entityPM.LeadDescription = entityPOCO.LeadDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCompletedActivityTypeCode))
            {
					entityPM.LastCompletedActivityTypeCode = entityPOCO.LastCompletedActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastActivitySubject))
            {
					entityPM.LastActivitySubject = entityPOCO.LastActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityDate))
            {
					entityPM.NextActivityDate = entityPOCO.NextActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityTypeCode))
            {
					entityPM.NextActivityTypeCode = entityPOCO.NextActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivitySubject))
            {
					entityPM.NextActivitySubject = entityPOCO.NextActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StageDueDate))
            {
					entityPM.StageDueDate = entityPOCO.StageDueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessUnitId))
            {
					entityPM.BusinessUnitId = entityPOCO.BusinessUnitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadUserId))
            {
					entityPM.LeadUserId = entityPOCO.LeadUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadPartnerId))
            {
					entityPM.LeadPartnerId = entityPOCO.LeadPartnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentId))
            {
					entityPM.AgentId = entityPOCO.AgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignClientId))
            {
					entityPM.ForeignClientId = entityPOCO.ForeignClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClosingReasonId))
            {
					entityPM.ClosingReasonId = entityPOCO.ClosingReasonId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityTypeId))
            {
					entityPM.OpportunityTypeId = entityPOCO.OpportunityTypeId;
            }

		}

		public void PMToOldPM(OpportunityPM entityPM, OpportunityPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
                oldEntityPM.Subject = entityPM.Subject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadSourceId))
            {
                oldEntityPM.LeadSourceId = entityPM.LeadSourceId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
                oldEntityPM.ContactId = entityPM.ContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedClosingDate))
            {
                oldEntityPM.EstimatedClosingDate = entityPM.EstimatedClosingDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
                oldEntityPM.StageId = entityPM.StageId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Probability))
            {
                oldEntityPM.Probability = entityPM.Probability;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RatingCode))
            {
                oldEntityPM.RatingCode = entityPM.RatingCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualClosingDate))
            {
                oldEntityPM.ActualClosingDate = entityPM.ActualClosingDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosingDescription))
            {
                oldEntityPM.ClosingDescription = entityPM.ClosingDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
                oldEntityPM.NumberOfShipments = entityPM.NumberOfShipments;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueField))
            {
                oldEntityPM.ValueField = entityPM.ValueField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageDate))
            {
                oldEntityPM.LastStageDate = entityPM.LastStageDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageIdBeforeClosure))
            {
                oldEntityPM.LastStageIdBeforeClosure = entityPM.LastStageIdBeforeClosure;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field1))
            {
                oldEntityPM.Field1 = entityPM.Field1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field2))
            {
                oldEntityPM.Field2 = entityPM.Field2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field3))
            {
                oldEntityPM.Field3 = entityPM.Field3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field4))
            {
                oldEntityPM.Field4 = entityPM.Field4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field5))
            {
                oldEntityPM.Field5 = entityPM.Field5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field6))
            {
                oldEntityPM.Field6 = entityPM.Field6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field7))
            {
                oldEntityPM.Field7 = entityPM.Field7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field8))
            {
                oldEntityPM.Field8 = entityPM.Field8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field9))
            {
                oldEntityPM.Field9 = entityPM.Field9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field10))
            {
                oldEntityPM.Field10 = entityPM.Field10;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityDate))
            {
                oldEntityPM.LastCompletedActivityDate = entityPM.LastCompletedActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadDescription))
            {
                oldEntityPM.LeadDescription = entityPM.LeadDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityTypeCode))
            {
                oldEntityPM.LastCompletedActivityTypeCode = entityPM.LastCompletedActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivitySubject))
            {
                oldEntityPM.LastActivitySubject = entityPM.LastActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
                oldEntityPM.NextActivityDate = entityPM.NextActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
                oldEntityPM.NextActivityTypeCode = entityPM.NextActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
                oldEntityPM.NextActivitySubject = entityPM.NextActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageDueDate))
            {
                oldEntityPM.StageDueDate = entityPM.StageDueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
                oldEntityPM.BusinessUnitId = entityPM.BusinessUnitId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadUserId))
            {
                oldEntityPM.LeadUserId = entityPM.LeadUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadPartnerId))
            {
                oldEntityPM.LeadPartnerId = entityPM.LeadPartnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
                oldEntityPM.AgentId = entityPM.AgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignClientId))
            {
                oldEntityPM.ForeignClientId = entityPM.ForeignClientId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosingReasonId))
            {
                oldEntityPM.ClosingReasonId = entityPM.ClosingReasonId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityTypeId))
            {
                oldEntityPM.OpportunityTypeId = entityPM.OpportunityTypeId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OpportunityPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Subject)) //T4 find type == nText 
            {
                entityPM.Subject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Subject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ClosingDescription)) //T4 find type == nText 
            {
                entityPM.ClosingDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ClosingDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LeadDescription)) //T4 find type == nText 
            {
                entityPM.LeadDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LeadDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastActivitySubject)) //T4 find type == nText 
            {
                entityPM.LastActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NextActivitySubject)) //T4 find type == nText 
            {
                entityPM.NextActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NextActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
		
		private void BuildSearchFieldsGenerated(OpportunityPM entityPM, Opportunity entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 