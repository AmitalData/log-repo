
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
   
   public partial class TicketDataMapping: IMapping<TicketPM, Ticket>,IMappingEncodeBase64NVARCHARFields<TicketPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TicketNumber, 
	         CreateDate, 
	         CreatedByContactId, 
	         UpdateDate, 
	         CompanyId, 
	         ContactId, 
	         UpdatedByUserId, 
	         MainClassificationId, 
	         OwnerId, 
	         StageId, 
	         SeverityId, 
	         Subject, 
	         TicketTypeId, 
	         IsCancelled, 
	         IsClosed, 
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
	         SearchFields, 
	         TicketDescription, 
	         NextActivityTypeCode, 
	         NextActivitySubject, 
	         NextActivityDate, 
	         LastCompletedActivityTypeCode, 
	         LastCompletedActivitySubject, 
	         LastCompletedActivityDate, 
	         CCs, 
	         Bcc, 
	         BusinessUnitId, 
	         EmployeeGroupId, 
	         FirstResponseTime, 
	         FirstResponseDue, 
	         FullResolvedTime, 
	         ResolveWithinDue, 
	         GuidId, 
	         OpenEscalation, 
	         InternalUsers, 
	         ClosureDescription, 
	         Closewithoutnotifying, 
	         SecondaryClassificationId, 
	         ShipmentId, 
	         ShipmentNumber, 
	         FirstResolveDate, 
	         Source, 
	         CreatedbyType, 
	         FirstCloseDate, 
	         LastCloseDate, 
	         OpenDate, 
	         OpenPeriodMinutes, 
	         InternalMode, 
	         CustomerContactId, 
	         QuoteId, 
	         QuoteNumber, 
	         SLAId, 
	         EntityType, 
	         SupportMailboxId, 
	         LastCorrespondence,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TicketNumber, 
	         CreateDate, 
	         CreatedByContactId, 
	         UpdateDate, 
	         CompanyId, 
	         ContactId, 
	         UpdatedByUserId, 
	         MainClassificationId, 
	         OwnerId, 
	         StageId, 
	         SeverityId, 
	         Subject, 
	         TicketTypeId, 
	         IsCancelled, 
	         IsClosed, 
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
	         SearchFields, 
	         TicketDescription, 
	         CompanyName, 
	         OwnerName, 
	         StageName, 
	         MainClassificationName, 
	         TypeName, 
	         SeverityName, 
	         ContactName, 
	         ContactPhone, 
	         NextActivityTypeCode, 
	         NextActivitySubject, 
	         NextActivityDate, 
	         LastCompletedActivityTypeCode, 
	         LastCompletedActivitySubject, 
	         LastCompletedActivityDate, 
	         CCs, 
	         Bcc, 
	         CreatedByContactName, 
	         RankCode, 
	         BusinessUnitId, 
	         EmployeeGroupId, 
	         FirstResponseTime, 
	         FirstResponseDue, 
	         FullResolvedTime, 
	         ResolveWithinDue, 
	         ContactEmail, 
	         GuidId, 
	         OpenEscalation, 
	         InternalUsers, 
	         ClosureDescription, 
	         Closewithoutnotifying, 
	         StageCode, 
	         LastCompletedActivityTypeName, 
	         NextActivityTypeName, 
	         SecondaryClassificationId, 
	         SecondaryClassificationName, 
	         ShipmentId, 
	         ShipmentNumber, 
	         UpdatedByUserName, 
	         FirstResolveDate, 
	         TicketHeader, 
	         TicketFooter, 
	         TicketReplyto, 
	         SeverityCode, 
	         TicketFirstResponseTime, 
	         TicketFirstResolveTime, 
	         Source, 
	         CreatedbyType, 
	         SourceName, 
	         CreatedbyTypeName, 
	         FirstCloseDate, 
	         LastCloseDate, 
	         OpenDate, 
	         OpenPeriodMinutes, 
	         IsCreatedFromOutSide, 
	         IsResolveDue, 
	         ResolveColor, 
	         IsResolveExamination, 
	         IsResponseDue, 
	         ResponseColor, 
	         IsResponseExamination, 
	         CompanyTableName, 
	         RankName, 
	         OwnerEmail, 
	         EmployeeGroupName, 
	         InternalMode, 
	         CustomerContactId, 
	         ClassificationManager, 
	         ClassificationNotify, 
	         GroupNotify, 
	         GroupManager, 
	         QuoteId, 
	         QuoteNumber, 
	         ContactTel, 
	         SLAName, 
	         SLAId, 
	         EntityType, 
	         EntityNumber, 
	         SupportMailboxId, 
	         LastCorrespondence,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TicketPM entityPM, Ticket entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketNumber))
            {
				entityPOCO.TicketNumber = entityPM.TicketNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByContactId))
            {
				entityPOCO.CreatedByContactId = entityPM.CreatedByContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyId))
            {
				entityPOCO.CompanyId = entityPM.CompanyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
				entityPOCO.ContactId = entityPM.ContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainClassificationId))
            {
				entityPOCO.MainClassificationId = entityPM.MainClassificationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
				entityPOCO.StageId = entityPM.StageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeverityId))
            {
				entityPOCO.SeverityId = entityPM.SeverityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
				entityPOCO.Subject = entityPM.Subject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketTypeId))
            {
				entityPOCO.TicketTypeId = entityPM.TicketTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketDescription))
            {
				entityPOCO.TicketDescription = entityPM.TicketDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
				entityPOCO.NextActivityTypeCode = entityPM.NextActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
				entityPOCO.NextActivitySubject = entityPM.NextActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
				entityPOCO.NextActivityDate = entityPM.NextActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityTypeCode))
            {
				entityPOCO.LastCompletedActivityTypeCode = entityPM.LastCompletedActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivitySubject))
            {
				entityPOCO.LastCompletedActivitySubject = entityPM.LastCompletedActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityDate))
            {
				entityPOCO.LastCompletedActivityDate = entityPM.LastCompletedActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CCs))
            {
				entityPOCO.CCs = entityPM.CCs;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Bcc))
            {
				entityPOCO.Bcc = entityPM.Bcc;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
				entityPOCO.BusinessUnitId = entityPM.BusinessUnitId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
				entityPOCO.EmployeeGroupId = entityPM.EmployeeGroupId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTime))
            {
				entityPOCO.FirstResponseTime = entityPM.FirstResponseTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseDue))
            {
				entityPOCO.FirstResponseDue = entityPM.FirstResponseDue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullResolvedTime))
            {
				entityPOCO.FullResolvedTime = entityPM.FullResolvedTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinDue))
            {
				entityPOCO.ResolveWithinDue = entityPM.ResolveWithinDue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuidId))
            {
				entityPOCO.GuidId = entityPM.GuidId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenEscalation))
            {
				entityPOCO.OpenEscalation = entityPM.OpenEscalation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalUsers))
            {
				entityPOCO.InternalUsers = entityPM.InternalUsers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosureDescription))
            {
				entityPOCO.ClosureDescription = entityPM.ClosureDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Closewithoutnotifying))
            {
				entityPOCO.Closewithoutnotifying = entityPM.Closewithoutnotifying;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryClassificationId))
            {
				entityPOCO.SecondaryClassificationId = entityPM.SecondaryClassificationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
				entityPOCO.ShipmentNumber = entityPM.ShipmentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResolveDate))
            {
				entityPOCO.FirstResolveDate = entityPM.FirstResolveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Source))
            {
				entityPOCO.Source = entityPM.Source;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedbyType))
            {
				entityPOCO.CreatedbyType = entityPM.CreatedbyType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstCloseDate))
            {
				entityPOCO.FirstCloseDate = entityPM.FirstCloseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCloseDate))
            {
				entityPOCO.LastCloseDate = entityPM.LastCloseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
				entityPOCO.OpenDate = entityPM.OpenDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenPeriodMinutes))
            {
				entityPOCO.OpenPeriodMinutes = entityPM.OpenPeriodMinutes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalMode))
            {
				entityPOCO.InternalMode = entityPM.InternalMode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerContactId))
            {
				entityPOCO.CustomerContactId = entityPM.CustomerContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
				entityPOCO.QuoteId = entityPM.QuoteId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteNumber))
            {
				entityPOCO.QuoteNumber = entityPM.QuoteNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAId))
            {
				entityPOCO.SLAId = entityPM.SLAId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityType))
            {
				entityPOCO.EntityType = entityPM.EntityType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SupportMailboxId))
            {
				entityPOCO.SupportMailboxId = entityPM.SupportMailboxId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCorrespondence))
            {
				entityPOCO.LastCorrespondence = entityPM.LastCorrespondence;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TicketPM entityPM, Ticket entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TicketNumber))
            {
					entityPM.TicketNumber = entityPOCO.TicketNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByContactId))
            {
					entityPM.CreatedByContactId = entityPOCO.CreatedByContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CompanyId))
            {
					entityPM.CompanyId = entityPOCO.CompanyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactId))
            {
					entityPM.ContactId = entityPOCO.ContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainClassificationId))
            {
					entityPM.MainClassificationId = entityPOCO.MainClassificationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StageId))
            {
					entityPM.StageId = entityPOCO.StageId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SeverityId))
            {
					entityPM.SeverityId = entityPOCO.SeverityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Subject))
            {
					entityPM.Subject = entityPOCO.Subject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TicketTypeId))
            {
					entityPM.TicketTypeId = entityPOCO.TicketTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field1))
            {
				entityPM.Field1 = new CustomFieldClass("Field1", "Ticket", entityPOCO.Field1);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field2))
            {
				entityPM.Field2 = new CustomFieldClass("Field2", "Ticket", entityPOCO.Field2);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field3))
            {
				entityPM.Field3 = new CustomFieldClass("Field3", "Ticket", entityPOCO.Field3);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field4))
            {
				entityPM.Field4 = new CustomFieldClass("Field4", "Ticket", entityPOCO.Field4);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field5))
            {
				entityPM.Field5 = new CustomFieldClass("Field5", "Ticket", entityPOCO.Field5);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field6))
            {
				entityPM.Field6 = new CustomFieldClass("Field6", "Ticket", entityPOCO.Field6);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field7))
            {
				entityPM.Field7 = new CustomFieldClass("Field7", "Ticket", entityPOCO.Field7);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field8))
            {
				entityPM.Field8 = new CustomFieldClass("Field8", "Ticket", entityPOCO.Field8);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field9))
            {
				entityPM.Field9 = new CustomFieldClass("Field9", "Ticket", entityPOCO.Field9);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field10))
            {
				entityPM.Field10 = new CustomFieldClass("Field10", "Ticket", entityPOCO.Field10);
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TicketDescription))
            {
					entityPM.TicketDescription = entityPOCO.TicketDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityTypeCode))
            {
					entityPM.NextActivityTypeCode = entityPOCO.NextActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivitySubject))
            {
					entityPM.NextActivitySubject = entityPOCO.NextActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityDate))
            {
					entityPM.NextActivityDate = entityPOCO.NextActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCompletedActivityTypeCode))
            {
					entityPM.LastCompletedActivityTypeCode = entityPOCO.LastCompletedActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCompletedActivitySubject))
            {
					entityPM.LastCompletedActivitySubject = entityPOCO.LastCompletedActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCompletedActivityDate))
            {
					entityPM.LastCompletedActivityDate = entityPOCO.LastCompletedActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CCs))
            {
					entityPM.CCs = entityPOCO.CCs;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Bcc))
            {
					entityPM.Bcc = entityPOCO.Bcc;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessUnitId))
            {
					entityPM.BusinessUnitId = entityPOCO.BusinessUnitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmployeeGroupId))
            {
					entityPM.EmployeeGroupId = entityPOCO.EmployeeGroupId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseTime))
            {
					entityPM.FirstResponseTime = entityPOCO.FirstResponseTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseDue))
            {
					entityPM.FirstResponseDue = entityPOCO.FirstResponseDue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullResolvedTime))
            {
					entityPM.FullResolvedTime = entityPOCO.FullResolvedTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResolveWithinDue))
            {
					entityPM.ResolveWithinDue = entityPOCO.ResolveWithinDue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GuidId))
            {
					entityPM.GuidId = entityPOCO.GuidId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenEscalation))
            {
					entityPM.OpenEscalation = entityPOCO.OpenEscalation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalUsers))
            {
					entityPM.InternalUsers = entityPOCO.InternalUsers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClosureDescription))
            {
					entityPM.ClosureDescription = entityPOCO.ClosureDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Closewithoutnotifying))
            {
					entityPM.Closewithoutnotifying = entityPOCO.Closewithoutnotifying;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondaryClassificationId))
            {
					entityPM.SecondaryClassificationId = entityPOCO.SecondaryClassificationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentNumber))
            {
					entityPM.ShipmentNumber = entityPOCO.ShipmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResolveDate))
            {
					entityPM.FirstResolveDate = entityPOCO.FirstResolveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Source))
            {
					entityPM.Source = entityPOCO.Source;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedbyType))
            {
					entityPM.CreatedbyType = entityPOCO.CreatedbyType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstCloseDate))
            {
					entityPM.FirstCloseDate = entityPOCO.FirstCloseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCloseDate))
            {
					entityPM.LastCloseDate = entityPOCO.LastCloseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenDate))
            {
					entityPM.OpenDate = entityPOCO.OpenDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenPeriodMinutes))
            {
					entityPM.OpenPeriodMinutes = entityPOCO.OpenPeriodMinutes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalMode))
            {
					entityPM.InternalMode = entityPOCO.InternalMode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerContactId))
            {
					entityPM.CustomerContactId = entityPOCO.CustomerContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteId))
            {
					entityPM.QuoteId = entityPOCO.QuoteId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteNumber))
            {
					entityPM.QuoteNumber = entityPOCO.QuoteNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SLAId))
            {
					entityPM.SLAId = entityPOCO.SLAId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityType))
            {
					entityPM.EntityType = entityPOCO.EntityType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SupportMailboxId))
            {
					entityPM.SupportMailboxId = entityPOCO.SupportMailboxId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCorrespondence))
            {
					entityPM.LastCorrespondence = entityPOCO.LastCorrespondence;
            }

		}

		public void PMToOldPM(TicketPM entityPM, TicketPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketNumber))
            {
                oldEntityPM.TicketNumber = entityPM.TicketNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByContactId))
            {
                oldEntityPM.CreatedByContactId = entityPM.CreatedByContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompanyId))
            {
                oldEntityPM.CompanyId = entityPM.CompanyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
                oldEntityPM.ContactId = entityPM.ContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainClassificationId))
            {
                oldEntityPM.MainClassificationId = entityPM.MainClassificationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
                oldEntityPM.StageId = entityPM.StageId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeverityId))
            {
                oldEntityPM.SeverityId = entityPM.SeverityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
                oldEntityPM.Subject = entityPM.Subject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketTypeId))
            {
                oldEntityPM.TicketTypeId = entityPM.TicketTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TicketDescription))
            {
                oldEntityPM.TicketDescription = entityPM.TicketDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
                oldEntityPM.NextActivityTypeCode = entityPM.NextActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
                oldEntityPM.NextActivitySubject = entityPM.NextActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
                oldEntityPM.NextActivityDate = entityPM.NextActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityTypeCode))
            {
                oldEntityPM.LastCompletedActivityTypeCode = entityPM.LastCompletedActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivitySubject))
            {
                oldEntityPM.LastCompletedActivitySubject = entityPM.LastCompletedActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCompletedActivityDate))
            {
                oldEntityPM.LastCompletedActivityDate = entityPM.LastCompletedActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CCs))
            {
                oldEntityPM.CCs = entityPM.CCs;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Bcc))
            {
                oldEntityPM.Bcc = entityPM.Bcc;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
                oldEntityPM.BusinessUnitId = entityPM.BusinessUnitId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
                oldEntityPM.EmployeeGroupId = entityPM.EmployeeGroupId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTime))
            {
                oldEntityPM.FirstResponseTime = entityPM.FirstResponseTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseDue))
            {
                oldEntityPM.FirstResponseDue = entityPM.FirstResponseDue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullResolvedTime))
            {
                oldEntityPM.FullResolvedTime = entityPM.FullResolvedTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinDue))
            {
                oldEntityPM.ResolveWithinDue = entityPM.ResolveWithinDue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuidId))
            {
                oldEntityPM.GuidId = entityPM.GuidId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenEscalation))
            {
                oldEntityPM.OpenEscalation = entityPM.OpenEscalation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalUsers))
            {
                oldEntityPM.InternalUsers = entityPM.InternalUsers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosureDescription))
            {
                oldEntityPM.ClosureDescription = entityPM.ClosureDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Closewithoutnotifying))
            {
                oldEntityPM.Closewithoutnotifying = entityPM.Closewithoutnotifying;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryClassificationId))
            {
                oldEntityPM.SecondaryClassificationId = entityPM.SecondaryClassificationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
                oldEntityPM.ShipmentNumber = entityPM.ShipmentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResolveDate))
            {
                oldEntityPM.FirstResolveDate = entityPM.FirstResolveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Source))
            {
                oldEntityPM.Source = entityPM.Source;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedbyType))
            {
                oldEntityPM.CreatedbyType = entityPM.CreatedbyType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstCloseDate))
            {
                oldEntityPM.FirstCloseDate = entityPM.FirstCloseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCloseDate))
            {
                oldEntityPM.LastCloseDate = entityPM.LastCloseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
                oldEntityPM.OpenDate = entityPM.OpenDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenPeriodMinutes))
            {
                oldEntityPM.OpenPeriodMinutes = entityPM.OpenPeriodMinutes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalMode))
            {
                oldEntityPM.InternalMode = entityPM.InternalMode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerContactId))
            {
                oldEntityPM.CustomerContactId = entityPM.CustomerContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteId))
            {
                oldEntityPM.QuoteId = entityPM.QuoteId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteNumber))
            {
                oldEntityPM.QuoteNumber = entityPM.QuoteNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAId))
            {
                oldEntityPM.SLAId = entityPM.SLAId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityType))
            {
                oldEntityPM.EntityType = entityPM.EntityType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SupportMailboxId))
            {
                oldEntityPM.SupportMailboxId = entityPM.SupportMailboxId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCorrespondence))
            {
                oldEntityPM.LastCorrespondence = entityPM.LastCorrespondence;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TicketPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Subject)) //T4 find type == nText 
            {
                entityPM.Subject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Subject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TicketDescription)) //T4 find type == nText 
            {
                entityPM.TicketDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TicketDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NextActivitySubject)) //T4 find type == nText 
            {
                entityPM.NextActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NextActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastCompletedActivitySubject)) //T4 find type == nText 
            {
                entityPM.LastCompletedActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastCompletedActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ClosureDescription)) //T4 find type == nText 
            {
                entityPM.ClosureDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ClosureDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastCorrespondence)) //T4 find type == nText 
            {
                entityPM.LastCorrespondence = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastCorrespondence));
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
		
		private void BuildSearchFieldsGenerated(TicketPM entityPM, Ticket entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 