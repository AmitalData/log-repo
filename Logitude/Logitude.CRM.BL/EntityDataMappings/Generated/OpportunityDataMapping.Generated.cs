
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
	         Field11, 
	         Field12, 
	         Field13, 
	         Field14, 
	         Field15, 
	         Field16, 
	         Field17, 
	         Field18, 
	         Field19, 
	         Field20, 
	         Field21, 
	         Field22, 
	         Field23, 
	         Field24, 
	         Field25, 
	         Field26, 
	         Field27, 
	         Field28, 
	         Field29, 
	         Field30, 
	         Field31, 
	         Field32, 
	         Field33, 
	         Field34, 
	         Field35, 
	         Field36, 
	         Field37, 
	         Field38, 
	         Field39, 
	         Field40, 
	         NumberOfConnectedQuotes, 
	         ClientId, 
	         LeadOrigin, 
	         Campaign,
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
	         Field11, 
	         Field12, 
	         Field13, 
	         Field14, 
	         Field15, 
	         Field16, 
	         Field17, 
	         Field18, 
	         Field19, 
	         Field20, 
	         Field21, 
	         Field22, 
	         Field23, 
	         Field24, 
	         Field25, 
	         Field26, 
	         Field27, 
	         Field28, 
	         Field29, 
	         Field30, 
	         Field31, 
	         Field32, 
	         Field33, 
	         Field34, 
	         Field35, 
	         Field36, 
	         Field37, 
	         Field38, 
	         Field39, 
	         Field40, 
	         NumberOfConnectedQuotes, 
	         UserName, 
	         ClientId, 
	         LeadOrigin, 
	         Campaign,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field11))
            {
				entityPOCO.Field11 = entityPM.Field11!= null ? entityPM.Field11.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field12))
            {
				entityPOCO.Field12 = entityPM.Field12!= null ? entityPM.Field12.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field13))
            {
				entityPOCO.Field13 = entityPM.Field13!= null ? entityPM.Field13.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field14))
            {
				entityPOCO.Field14 = entityPM.Field14!= null ? entityPM.Field14.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field15))
            {
				entityPOCO.Field15 = entityPM.Field15!= null ? entityPM.Field15.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field16))
            {
				entityPOCO.Field16 = entityPM.Field16!= null ? entityPM.Field16.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field17))
            {
				entityPOCO.Field17 = entityPM.Field17!= null ? entityPM.Field17.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field18))
            {
				entityPOCO.Field18 = entityPM.Field18!= null ? entityPM.Field18.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field19))
            {
				entityPOCO.Field19 = entityPM.Field19!= null ? entityPM.Field19.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field20))
            {
				entityPOCO.Field20 = entityPM.Field20!= null ? entityPM.Field20.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field21))
            {
				entityPOCO.Field21 = entityPM.Field21!= null ? entityPM.Field21.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field22))
            {
				entityPOCO.Field22 = entityPM.Field22!= null ? entityPM.Field22.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field23))
            {
				entityPOCO.Field23 = entityPM.Field23!= null ? entityPM.Field23.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field24))
            {
				entityPOCO.Field24 = entityPM.Field24!= null ? entityPM.Field24.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field25))
            {
				entityPOCO.Field25 = entityPM.Field25!= null ? entityPM.Field25.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field26))
            {
				entityPOCO.Field26 = entityPM.Field26!= null ? entityPM.Field26.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field27))
            {
				entityPOCO.Field27 = entityPM.Field27!= null ? entityPM.Field27.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field28))
            {
				entityPOCO.Field28 = entityPM.Field28!= null ? entityPM.Field28.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field29))
            {
				entityPOCO.Field29 = entityPM.Field29!= null ? entityPM.Field29.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field30))
            {
				entityPOCO.Field30 = entityPM.Field30!= null ? entityPM.Field30.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field31))
            {
				entityPOCO.Field31 = entityPM.Field31!= null ? entityPM.Field31.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field32))
            {
				entityPOCO.Field32 = entityPM.Field32!= null ? entityPM.Field32.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field33))
            {
				entityPOCO.Field33 = entityPM.Field33!= null ? entityPM.Field33.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field34))
            {
				entityPOCO.Field34 = entityPM.Field34!= null ? entityPM.Field34.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field35))
            {
				entityPOCO.Field35 = entityPM.Field35!= null ? entityPM.Field35.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field36))
            {
				entityPOCO.Field36 = entityPM.Field36!= null ? entityPM.Field36.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field37))
            {
				entityPOCO.Field37 = entityPM.Field37!= null ? entityPM.Field37.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field38))
            {
				entityPOCO.Field38 = entityPM.Field38!= null ? entityPM.Field38.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field39))
            {
				entityPOCO.Field39 = entityPM.Field39!= null ? entityPM.Field39.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field40))
            {
				entityPOCO.Field40 = entityPM.Field40!= null ? entityPM.Field40.Value : null;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfConnectedQuotes))
            {
				entityPOCO.NumberOfConnectedQuotes = entityPM.NumberOfConnectedQuotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
				entityPOCO.ClientId = entityPM.ClientId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadOrigin))
            {
				entityPOCO.LeadOrigin = entityPM.LeadOrigin;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Campaign))
            {
				entityPOCO.Campaign = entityPM.Campaign;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field11))
            {
				entityPM.Field11 = new CustomFieldClass("Field11", "Opportunity", entityPOCO.Field11);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field12))
            {
				entityPM.Field12 = new CustomFieldClass("Field12", "Opportunity", entityPOCO.Field12);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field13))
            {
				entityPM.Field13 = new CustomFieldClass("Field13", "Opportunity", entityPOCO.Field13);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field14))
            {
				entityPM.Field14 = new CustomFieldClass("Field14", "Opportunity", entityPOCO.Field14);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field15))
            {
				entityPM.Field15 = new CustomFieldClass("Field15", "Opportunity", entityPOCO.Field15);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field16))
            {
				entityPM.Field16 = new CustomFieldClass("Field16", "Opportunity", entityPOCO.Field16);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field17))
            {
				entityPM.Field17 = new CustomFieldClass("Field17", "Opportunity", entityPOCO.Field17);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field18))
            {
				entityPM.Field18 = new CustomFieldClass("Field18", "Opportunity", entityPOCO.Field18);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field19))
            {
				entityPM.Field19 = new CustomFieldClass("Field19", "Opportunity", entityPOCO.Field19);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field20))
            {
				entityPM.Field20 = new CustomFieldClass("Field20", "Opportunity", entityPOCO.Field20);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field21))
            {
				entityPM.Field21 = new CustomFieldClass("Field21", "Opportunity", entityPOCO.Field21);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field22))
            {
				entityPM.Field22 = new CustomFieldClass("Field22", "Opportunity", entityPOCO.Field22);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field23))
            {
				entityPM.Field23 = new CustomFieldClass("Field23", "Opportunity", entityPOCO.Field23);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field24))
            {
				entityPM.Field24 = new CustomFieldClass("Field24", "Opportunity", entityPOCO.Field24);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field25))
            {
				entityPM.Field25 = new CustomFieldClass("Field25", "Opportunity", entityPOCO.Field25);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field26))
            {
				entityPM.Field26 = new CustomFieldClass("Field26", "Opportunity", entityPOCO.Field26);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field27))
            {
				entityPM.Field27 = new CustomFieldClass("Field27", "Opportunity", entityPOCO.Field27);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field28))
            {
				entityPM.Field28 = new CustomFieldClass("Field28", "Opportunity", entityPOCO.Field28);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field29))
            {
				entityPM.Field29 = new CustomFieldClass("Field29", "Opportunity", entityPOCO.Field29);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field30))
            {
				entityPM.Field30 = new CustomFieldClass("Field30", "Opportunity", entityPOCO.Field30);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field31))
            {
				entityPM.Field31 = new CustomFieldClass("Field31", "Opportunity", entityPOCO.Field31);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field32))
            {
				entityPM.Field32 = new CustomFieldClass("Field32", "Opportunity", entityPOCO.Field32);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field33))
            {
				entityPM.Field33 = new CustomFieldClass("Field33", "Opportunity", entityPOCO.Field33);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field34))
            {
				entityPM.Field34 = new CustomFieldClass("Field34", "Opportunity", entityPOCO.Field34);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field35))
            {
				entityPM.Field35 = new CustomFieldClass("Field35", "Opportunity", entityPOCO.Field35);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field36))
            {
				entityPM.Field36 = new CustomFieldClass("Field36", "Opportunity", entityPOCO.Field36);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field37))
            {
				entityPM.Field37 = new CustomFieldClass("Field37", "Opportunity", entityPOCO.Field37);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field38))
            {
				entityPM.Field38 = new CustomFieldClass("Field38", "Opportunity", entityPOCO.Field38);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field39))
            {
				entityPM.Field39 = new CustomFieldClass("Field39", "Opportunity", entityPOCO.Field39);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field40))
            {
				entityPM.Field40 = new CustomFieldClass("Field40", "Opportunity", entityPOCO.Field40);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfConnectedQuotes))
            {
					entityPM.NumberOfConnectedQuotes = entityPOCO.NumberOfConnectedQuotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadOrigin))
            {
					entityPM.LeadOrigin = entityPOCO.LeadOrigin;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Campaign))
            {
					entityPM.Campaign = entityPOCO.Campaign;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field11))
            {
                oldEntityPM.Field11 = entityPM.Field11;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field12))
            {
                oldEntityPM.Field12 = entityPM.Field12;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field13))
            {
                oldEntityPM.Field13 = entityPM.Field13;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field14))
            {
                oldEntityPM.Field14 = entityPM.Field14;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field15))
            {
                oldEntityPM.Field15 = entityPM.Field15;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field16))
            {
                oldEntityPM.Field16 = entityPM.Field16;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field17))
            {
                oldEntityPM.Field17 = entityPM.Field17;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field18))
            {
                oldEntityPM.Field18 = entityPM.Field18;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field19))
            {
                oldEntityPM.Field19 = entityPM.Field19;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field20))
            {
                oldEntityPM.Field20 = entityPM.Field20;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field21))
            {
                oldEntityPM.Field21 = entityPM.Field21;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field22))
            {
                oldEntityPM.Field22 = entityPM.Field22;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field23))
            {
                oldEntityPM.Field23 = entityPM.Field23;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field24))
            {
                oldEntityPM.Field24 = entityPM.Field24;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field25))
            {
                oldEntityPM.Field25 = entityPM.Field25;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field26))
            {
                oldEntityPM.Field26 = entityPM.Field26;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field27))
            {
                oldEntityPM.Field27 = entityPM.Field27;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field28))
            {
                oldEntityPM.Field28 = entityPM.Field28;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field29))
            {
                oldEntityPM.Field29 = entityPM.Field29;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field30))
            {
                oldEntityPM.Field30 = entityPM.Field30;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field31))
            {
                oldEntityPM.Field31 = entityPM.Field31;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field32))
            {
                oldEntityPM.Field32 = entityPM.Field32;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field33))
            {
                oldEntityPM.Field33 = entityPM.Field33;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field34))
            {
                oldEntityPM.Field34 = entityPM.Field34;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field35))
            {
                oldEntityPM.Field35 = entityPM.Field35;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field36))
            {
                oldEntityPM.Field36 = entityPM.Field36;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field37))
            {
                oldEntityPM.Field37 = entityPM.Field37;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field38))
            {
                oldEntityPM.Field38 = entityPM.Field38;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field39))
            {
                oldEntityPM.Field39 = entityPM.Field39;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field40))
            {
                oldEntityPM.Field40 = entityPM.Field40;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfConnectedQuotes))
            {
                oldEntityPM.NumberOfConnectedQuotes = entityPM.NumberOfConnectedQuotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
                oldEntityPM.ClientId = entityPM.ClientId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadOrigin))
            {
                oldEntityPM.LeadOrigin = entityPM.LeadOrigin;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Campaign))
            {
                oldEntityPM.Campaign = entityPM.Campaign;
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
	 