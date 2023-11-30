
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
   
   public partial class ActivityDataMapping: IMapping<ActivityPM, Activity>,IMappingEncodeBase64NVARCHARFields<ActivityPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ActivityTypeCode, 
	         Subject, 
	         DueDate, 
	         PriorityCode, 
	         OwnerId, 
	         StartDateTime, 
	         EndDateTime, 
	         Description, 
	         CallTypeCode, 
	         CallPurpose, 
	         CallDetails, 
	         CallResult, 
	         PhoneNumber, 
	         Duration, 
	         Notes, 
	         CreatedByUserId, 
	         CreateDate, 
	         ActivityStatusCode, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         BranchId, 
	         AllDayEvent, 
	         ActivityTimeTypeCode, 
	         Location, 
	         SearchFields, 
	         IsLeftVoiceMail, 
	         OutlookId, 
	         NeedSynchronization, 
	         IsOpen, 
	         MeetingSummary, 
	         CustomerId, 
	         OpportunityId, 
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
	         CompleteDate, 
	         BusinessUnitId, 
	         SenderEmail, 
	         CommunicationLogId, 
	         SenderContactId, 
	         CallWithId, 
	         ConcurrencyGUID, 
	         QuoteId, 
	         TicketId, 
	         SortingDate, 
	         SortingBy, 
	         SendReceiveDate, 
	         ActivityWith, 
	         DescriptionRightToLeft, 
	         MeetingSummaryRightToLeft, 
	         From, 
	         To, 
	         Cc, 
	         DueDateOffset, 
	         DueDateDateField, 
	         BusinessProcessQueueId, 
	         TeamId, 
	         ShipmentId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ActivityTypeCode, 
	         Subject, 
	         DueDate, 
	         PriorityCode, 
	         OwnerId, 
	         StartDateTime, 
	         EndDateTime, 
	         Description, 
	         CallTypeCode, 
	         CallPurpose, 
	         CallDetails, 
	         CallResult, 
	         PhoneNumber, 
	         Duration, 
	         Notes, 
	         CreatedByUserId, 
	         CreateDate, 
	         ActivityStatusCode, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         BranchId, 
	         AllDayEvent, 
	         ActivityTimeTypeCode, 
	         Location, 
	         ActivityTypeName, 
	         SearchFields, 
	         ActivityStatusName, 
	         OwnerName, 
	         PriorityName, 
	         IsLeftVoiceMail, 
	         OutlookId, 
	         NeedSynchronization, 
	         IsMarkedCompleted, 
	         IsOpen, 
	         MeetingSummary, 
	         CustomerId, 
	         OpportunityId, 
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
	         CompleteDate, 
	         OpportunitySubject, 
	         BusinessUnitId, 
	         SenderEmail, 
	         CommunicationLogId, 
	         CustomerName, 
	         SenderContactId, 
	         CallWithId, 
	         ConcurrencyGUID, 
	         CreatedByUserName, 
	         QuoteId, 
	         QuoteNumber, 
	         TicketId, 
	         SortingDate, 
	         SortingBy, 
	         SendReceiveDate, 
	         ActivityWith, 
	         DescriptionRightToLeft, 
	         MeetingSummaryRightToLeft, 
	         SenderContactName, 
	         OriginalActivitySubject, 
	         IsHybrid, 
	         IsCustomerBlockedBusinessUnit, 
	         IsCopy, 
	         LastModified, 
	         DontSetNeedSynchronization, 
	         ActivityTypePathCode, 
	         ObjectTableName, 
	         From, 
	         To, 
	         Cc, 
	         ArchiveDate, 
	         DueDateOffset, 
	         DueDateDateField, 
	         BusinessProcessQueueId, 
	         TeamId, 
	         ShipmentId, 
	         LeadSourceId, 
	         LeadSourceName, 
	         CustomerCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ActivityPM entityPM, Activity entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTypeCode))
            {
				entityPOCO.ActivityTypeCode = entityPM.ActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
				entityPOCO.Subject = entityPM.Subject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
				entityPOCO.DueDate = entityPM.DueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriorityCode))
            {
				entityPOCO.PriorityCode = entityPM.PriorityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDateTime))
            {
				entityPOCO.StartDateTime = entityPM.StartDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDateTime))
            {
				entityPOCO.EndDateTime = entityPM.EndDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallTypeCode))
            {
				entityPOCO.CallTypeCode = entityPM.CallTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallPurpose))
            {
				entityPOCO.CallPurpose = entityPM.CallPurpose;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallDetails))
            {
				entityPOCO.CallDetails = entityPM.CallDetails;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallResult))
            {
				entityPOCO.CallResult = entityPM.CallResult;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PhoneNumber))
            {
				entityPOCO.PhoneNumber = entityPM.PhoneNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
				entityPOCO.Duration = entityPM.Duration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityStatusCode))
            {
				entityPOCO.ActivityStatusCode = entityPM.ActivityStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchId))
            {
				entityPOCO.BranchId = entityPM.BranchId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllDayEvent))
            {
				entityPOCO.AllDayEvent = entityPM.AllDayEvent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTimeTypeCode))
            {
				entityPOCO.ActivityTimeTypeCode = entityPM.ActivityTimeTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
				entityPOCO.Location = entityPM.Location;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLeftVoiceMail))
            {
				entityPOCO.IsLeftVoiceMail = entityPM.IsLeftVoiceMail;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutlookId))
            {
				entityPOCO.OutlookId = entityPM.OutlookId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedSynchronization))
            {
				entityPOCO.NeedSynchronization = entityPM.NeedSynchronization;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOpen))
            {
				entityPOCO.IsOpen = entityPM.IsOpen;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeetingSummary))
            {
				entityPOCO.MeetingSummary = entityPM.MeetingSummary;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
				entityPOCO.OpportunityId = entityPM.OpportunityId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompleteDate))
            {
				entityPOCO.CompleteDate = entityPM.CompleteDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
				entityPOCO.BusinessUnitId = entityPM.BusinessUnitId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderEmail))
            {
				entityPOCO.SenderEmail = entityPM.SenderEmail;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
				entityPOCO.CommunicationLogId = entityPM.CommunicationLogId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderContactId))
            {
				entityPOCO.SenderContactId = entityPM.SenderContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallWithId))
            {
				entityPOCO.CallWithId = entityPM.CallWithId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
				entityPOCO.QuoteId = entityPM.QuoteId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketId))
            {
				entityPOCO.TicketId = entityPM.TicketId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortingDate))
            {
				entityPOCO.SortingDate = entityPM.SortingDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortingBy))
            {
				entityPOCO.SortingBy = entityPM.SortingBy;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SendReceiveDate))
            {
				entityPOCO.SendReceiveDate = entityPM.SendReceiveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityWith))
            {
				entityPOCO.ActivityWith = entityPM.ActivityWith;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionRightToLeft))
            {
				entityPOCO.DescriptionRightToLeft = entityPM.DescriptionRightToLeft;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeetingSummaryRightToLeft))
            {
				entityPOCO.MeetingSummaryRightToLeft = entityPM.MeetingSummaryRightToLeft;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.From))
            {
				entityPOCO.From = entityPM.From;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.To))
            {
				entityPOCO.To = entityPM.To;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Cc))
            {
				entityPOCO.Cc = entityPM.Cc;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDateOffset))
            {
				entityPOCO.DueDateOffset = entityPM.DueDateOffset;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDateDateField))
            {
				entityPOCO.DueDateDateField = entityPM.DueDateDateField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessProcessQueueId))
            {
				entityPOCO.BusinessProcessQueueId = entityPM.BusinessProcessQueueId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamId))
            {
				entityPOCO.TeamId = entityPM.TeamId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ActivityPM entityPM, Activity entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityTypeCode))
            {
					entityPM.ActivityTypeCode = entityPOCO.ActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Subject))
            {
					entityPM.Subject = entityPOCO.Subject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DueDate))
            {
					entityPM.DueDate = entityPOCO.DueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PriorityCode))
            {
					entityPM.PriorityCode = entityPOCO.PriorityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDateTime))
            {
					entityPM.StartDateTime = entityPOCO.StartDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDateTime))
            {
					entityPM.EndDateTime = entityPOCO.EndDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallTypeCode))
            {
					entityPM.CallTypeCode = entityPOCO.CallTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallPurpose))
            {
					entityPM.CallPurpose = entityPOCO.CallPurpose;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallDetails))
            {
					entityPM.CallDetails = entityPOCO.CallDetails;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallResult))
            {
					entityPM.CallResult = entityPOCO.CallResult;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PhoneNumber))
            {
					entityPM.PhoneNumber = entityPOCO.PhoneNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Duration))
            {
					entityPM.Duration = entityPOCO.Duration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityStatusCode))
            {
					entityPM.ActivityStatusCode = entityPOCO.ActivityStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchId))
            {
					entityPM.BranchId = entityPOCO.BranchId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AllDayEvent))
            {
					entityPM.AllDayEvent = entityPOCO.AllDayEvent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityTimeTypeCode))
            {
					entityPM.ActivityTimeTypeCode = entityPOCO.ActivityTimeTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Location))
            {
					entityPM.Location = entityPOCO.Location;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsLeftVoiceMail))
            {
					entityPM.IsLeftVoiceMail = entityPOCO.IsLeftVoiceMail;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutlookId))
            {
					entityPM.OutlookId = entityPOCO.OutlookId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NeedSynchronization))
            {
					entityPM.NeedSynchronization = entityPOCO.NeedSynchronization;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsOpen))
            {
					entityPM.IsOpen = entityPOCO.IsOpen;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeetingSummary))
            {
					entityPM.MeetingSummary = entityPOCO.MeetingSummary;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field1))
            {
				entityPM.Field1 = new CustomFieldClass("Field1", "Activity", entityPOCO.Field1);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field2))
            {
				entityPM.Field2 = new CustomFieldClass("Field2", "Activity", entityPOCO.Field2);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field3))
            {
				entityPM.Field3 = new CustomFieldClass("Field3", "Activity", entityPOCO.Field3);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field4))
            {
				entityPM.Field4 = new CustomFieldClass("Field4", "Activity", entityPOCO.Field4);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field5))
            {
				entityPM.Field5 = new CustomFieldClass("Field5", "Activity", entityPOCO.Field5);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field6))
            {
				entityPM.Field6 = new CustomFieldClass("Field6", "Activity", entityPOCO.Field6);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field7))
            {
				entityPM.Field7 = new CustomFieldClass("Field7", "Activity", entityPOCO.Field7);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field8))
            {
				entityPM.Field8 = new CustomFieldClass("Field8", "Activity", entityPOCO.Field8);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field9))
            {
				entityPM.Field9 = new CustomFieldClass("Field9", "Activity", entityPOCO.Field9);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field10))
            {
				entityPM.Field10 = new CustomFieldClass("Field10", "Activity", entityPOCO.Field10);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CompleteDate))
            {
					entityPM.CompleteDate = entityPOCO.CompleteDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessUnitId))
            {
					entityPM.BusinessUnitId = entityPOCO.BusinessUnitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SenderEmail))
            {
					entityPM.SenderEmail = entityPOCO.SenderEmail;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationLogId))
            {
					entityPM.CommunicationLogId = entityPOCO.CommunicationLogId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SenderContactId))
            {
					entityPM.SenderContactId = entityPOCO.SenderContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CallWithId))
            {
					entityPM.CallWithId = entityPOCO.CallWithId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteId))
            {
					entityPM.QuoteId = entityPOCO.QuoteId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TicketId))
            {
					entityPM.TicketId = entityPOCO.TicketId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SortingDate))
            {
					entityPM.SortingDate = entityPOCO.SortingDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SortingBy))
            {
					entityPM.SortingBy = entityPOCO.SortingBy;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SendReceiveDate))
            {
					entityPM.SendReceiveDate = entityPOCO.SendReceiveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityWith))
            {
					entityPM.ActivityWith = entityPOCO.ActivityWith;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionRightToLeft))
            {
					entityPM.DescriptionRightToLeft = entityPOCO.DescriptionRightToLeft;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeetingSummaryRightToLeft))
            {
					entityPM.MeetingSummaryRightToLeft = entityPOCO.MeetingSummaryRightToLeft;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.From))
            {
					entityPM.From = entityPOCO.From;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.To))
            {
					entityPM.To = entityPOCO.To;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Cc))
            {
					entityPM.Cc = entityPOCO.Cc;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DueDateOffset))
            {
					entityPM.DueDateOffset = entityPOCO.DueDateOffset;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DueDateDateField))
            {
					entityPM.DueDateDateField = entityPOCO.DueDateDateField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessProcessQueueId))
            {
					entityPM.BusinessProcessQueueId = entityPOCO.BusinessProcessQueueId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TeamId))
            {
					entityPM.TeamId = entityPOCO.TeamId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

		}

		public void PMToOldPM(ActivityPM entityPM, ActivityPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTypeCode))
            {
                oldEntityPM.ActivityTypeCode = entityPM.ActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
                oldEntityPM.Subject = entityPM.Subject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
                oldEntityPM.DueDate = entityPM.DueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriorityCode))
            {
                oldEntityPM.PriorityCode = entityPM.PriorityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDateTime))
            {
                oldEntityPM.StartDateTime = entityPM.StartDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDateTime))
            {
                oldEntityPM.EndDateTime = entityPM.EndDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallTypeCode))
            {
                oldEntityPM.CallTypeCode = entityPM.CallTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallPurpose))
            {
                oldEntityPM.CallPurpose = entityPM.CallPurpose;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallDetails))
            {
                oldEntityPM.CallDetails = entityPM.CallDetails;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallResult))
            {
                oldEntityPM.CallResult = entityPM.CallResult;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PhoneNumber))
            {
                oldEntityPM.PhoneNumber = entityPM.PhoneNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
                oldEntityPM.Duration = entityPM.Duration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityStatusCode))
            {
                oldEntityPM.ActivityStatusCode = entityPM.ActivityStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchId))
            {
                oldEntityPM.BranchId = entityPM.BranchId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllDayEvent))
            {
                oldEntityPM.AllDayEvent = entityPM.AllDayEvent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTimeTypeCode))
            {
                oldEntityPM.ActivityTimeTypeCode = entityPM.ActivityTimeTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
                oldEntityPM.Location = entityPM.Location;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLeftVoiceMail))
            {
                oldEntityPM.IsLeftVoiceMail = entityPM.IsLeftVoiceMail;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutlookId))
            {
                oldEntityPM.OutlookId = entityPM.OutlookId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedSynchronization))
            {
                oldEntityPM.NeedSynchronization = entityPM.NeedSynchronization;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOpen))
            {
                oldEntityPM.IsOpen = entityPM.IsOpen;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeetingSummary))
            {
                oldEntityPM.MeetingSummary = entityPM.MeetingSummary;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
                oldEntityPM.OpportunityId = entityPM.OpportunityId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompleteDate))
            {
                oldEntityPM.CompleteDate = entityPM.CompleteDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
                oldEntityPM.BusinessUnitId = entityPM.BusinessUnitId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderEmail))
            {
                oldEntityPM.SenderEmail = entityPM.SenderEmail;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
                oldEntityPM.CommunicationLogId = entityPM.CommunicationLogId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderContactId))
            {
                oldEntityPM.SenderContactId = entityPM.SenderContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CallWithId))
            {
                oldEntityPM.CallWithId = entityPM.CallWithId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
                oldEntityPM.QuoteId = entityPM.QuoteId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketId))
            {
                oldEntityPM.TicketId = entityPM.TicketId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortingDate))
            {
                oldEntityPM.SortingDate = entityPM.SortingDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortingBy))
            {
                oldEntityPM.SortingBy = entityPM.SortingBy;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SendReceiveDate))
            {
                oldEntityPM.SendReceiveDate = entityPM.SendReceiveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityWith))
            {
                oldEntityPM.ActivityWith = entityPM.ActivityWith;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionRightToLeft))
            {
                oldEntityPM.DescriptionRightToLeft = entityPM.DescriptionRightToLeft;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeetingSummaryRightToLeft))
            {
                oldEntityPM.MeetingSummaryRightToLeft = entityPM.MeetingSummaryRightToLeft;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.From))
            {
                oldEntityPM.From = entityPM.From;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.To))
            {
                oldEntityPM.To = entityPM.To;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Cc))
            {
                oldEntityPM.Cc = entityPM.Cc;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDateOffset))
            {
                oldEntityPM.DueDateOffset = entityPM.DueDateOffset;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDateDateField))
            {
                oldEntityPM.DueDateDateField = entityPM.DueDateDateField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessProcessQueueId))
            {
                oldEntityPM.BusinessProcessQueueId = entityPM.BusinessProcessQueueId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamId))
            {
                oldEntityPM.TeamId = entityPM.TeamId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ActivityPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Subject)) //T4 find type == nText 
            {
                entityPM.Subject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Subject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CallPurpose)) //T4 find type == nText 
            {
                entityPM.CallPurpose = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CallPurpose));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CallDetails)) //T4 find type == nText 
            {
                entityPM.CallDetails = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CallDetails));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CallResult)) //T4 find type == nText 
            {
                entityPM.CallResult = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CallResult));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Location)) //T4 find type == nText 
            {
                entityPM.Location = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Location));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.MeetingSummary)) //T4 find type == nText 
            {
                entityPM.MeetingSummary = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.MeetingSummary));
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
		
		private void BuildSearchFieldsGenerated(ActivityPM entityPM, Activity entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 