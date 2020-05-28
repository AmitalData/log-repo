using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.EntityPOCOs;

namespace Logitude.CRM.Data
{

    public interface ICRMContext : IContext
    {
   
       	 IDbSet<Activity> Activities { get; }
		 IDbSet<ActivityEmailRecipient> ActivityEmailRecipients { get; }
		 IDbSet<ActivityInvitee> ActivityInvitees { get; }
		 IDbSet<ActivityNote> ActivityNotes { get; }
		 IDbSet<ActivityOwnerHistory> ActivityOwnerHistories { get; }
		 IDbSet<ActivityPriority> ActivityPriorities { get; }
		 IDbSet<ActivityStatus> ActivityStatus { get; }
		 IDbSet<ActivityTimeType> ActivityTimeTypes { get; }
		 IDbSet<ActivityType> ActivityTypes { get; }
		 IDbSet<CallType> CallTypes { get; }
		 IDbSet<Correspondence> Correspondences { get; }
		 IDbSet<CorrespondencesAttachment> CorrespondencesAttachments { get; }
		 IDbSet<CRMFilterSetting> CRMFilterSettings { get; }
		 IDbSet<EmployeeGroup> EmployeeGroups { get; }
		 IDbSet<EmployeeGroupLine> EmployeeGroupLines { get; }
		 IDbSet<EscalationActionTimeIndicator> EscalationActionTimeIndicators { get; }
		 IDbSet<EscalationPreDefinition> EscalationPreDefinitions { get; }
		 IDbSet<Occasion> Occasions { get; }
		 IDbSet<OccasionInvitee> OccasionInvitees { get; }
		 IDbSet<OccasionStatus> OccasionStatuses { get; }
		 IDbSet<OccasionType> OccasionTypes { get; }
		 IDbSet<Opportunity> Opportunities { get; }
		 IDbSet<OpportunityAdditionalService> OpportunityAdditionalServices { get; }
		 IDbSet<OpportunityClosingReason> OpportunityClosingReasons { get; }
		 IDbSet<OpportunityCompetitor> OpportunityCompetitors { get; }
		 IDbSet<OpportunityCompetitorProduct> OpportunityCompetitorProducts { get; }
		 IDbSet<OpportunityProduct> OpportunityProducts { get; }
		 IDbSet<OpportunityProductLocation> OpportunityProductLocations { get; }
		 IDbSet<OpportunityStage> OpportunityStages { get; }
		 IDbSet<OpportunityType> OpportunityTypes { get; }
		 IDbSet<Questionnaire> Questionnaires { get; }
		 IDbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; }
		 IDbSet<QuestionnaireAnswerLine> QuestionnaireAnswerLines { get; }
		 IDbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; }
		 IDbSet<Rating> Ratings { get; }
		 IDbSet<SLAEscalation> SLAEscalations { get; }
		 IDbSet<SLAEscalationRecepient> SLAEscalationRecepients { get; }
		 IDbSet<SLAHeader> SLAHeaders { get; }
		 IDbSet<SLALine> SLALines { get; }
		 IDbSet<Stage> Stages { get; }
		 IDbSet<SupportMailbox> SupportMailboxes { get; }
		 IDbSet<Ticket> Tickets { get; }
		 IDbSet<TicketClassification> TicketClassifications { get; }
		 IDbSet<TicketCreatedByType> TicketCreatedByTypes { get; }
		 IDbSet<TicketEscalation> TicketEscalations { get; }
		 IDbSet<TicketSeverity> TicketSeverities { get; }
		 IDbSet<TicketSource> TicketSources { get; }
		 IDbSet<TicketStage> TicketStages { get; }
		 IDbSet<TicketType> TicketTypes { get; }
		 IDbSet<TimeUnit> TimeUnits { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}