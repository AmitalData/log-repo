
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using WebFreight.Web.QuoteModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate
{
   public class CRMUpdateClass
   {
        public Dictionary<string, ObjectTable> ObjectTables { get; set; }
	    public Dictionary<string, TextCode> TextCodes { get; set; }
	    public ObjectTableRepository ObjectTableRepository { get; set; }
	    public TextCodeRepository TextCodeRepository { get; set; } 
		public Dictionary<string, ObjectField> ObjectFields { get; set; }
	    public ObjectFieldRepository ObjectFieldsRepository { get; set; }
		public IWebFreightContext ObjectContext { get; set; }
		 public ICommonDataContext CommonContext { get; set; }
		public Dictionary<string, Query> Queries {get; set; }
		public Dictionary<string, QueryColumn> QueryColumns {get; set; }
		public QueryRepository queriesRepository { get; set; } 
		public QueryColumnRepository queryColumnsRepository { get; set; } 
		public FeatureRepository FeaturesRepository { get; set; }// = new FeatureRepository(0);
		public Dictionary<string, Feature> TenantFeatures { get; set; }
		public QueryGroupRepository queryGroupRepository{ get; set; }
		public AdvancedQueryFilterRepository advancedQueryFiltersRepository {get;set;}
		public Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters {get; set; }
		public ScreensRepository screensRepository {get;set;}
		public Dictionary<string, Screen> tenantScreens {get; set; }
		public ScreenFieldsRepository screenFieldsRepository {get;set;}
		public Dictionary<string, ScreenField> tenantScreenFields {get; set; }
		public Dictionary<string, ObjectTableTab> TenantObjectTableTabs {get; set; }
		public ObjectTableTabRepository objectTableTabsRepository {get;set;}
	    public Dictionary<string, EventType> tenantEventTypes {get;set;}
		public EventTypeRepository EventTypeRepository {get;set;}
		public MenuButtonRepository menuButtonRepository {get;set;}
		public MenuButtonGroupRepository menuButtonGroupRepository {get;set;} 
		public Dictionary<string, MenuButton> tenantMenuButtons {get;set;}
		public Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups {get;set;}
		public EntityStatusRepository EntityStatusRepository { get; set; }
		public  List<EntityStatus>  AllEntityStatuses { get; set; }
		 
	   	ActivityUpdateClass  ActivityUpdateClass = new ActivityUpdateClass();
		ActivityEmailRecipientUpdateClass  ActivityEmailRecipientUpdateClass = new ActivityEmailRecipientUpdateClass();
		ActivityInviteeUpdateClass  ActivityInviteeUpdateClass = new ActivityInviteeUpdateClass();
		ActivityNoteUpdateClass  ActivityNoteUpdateClass = new ActivityNoteUpdateClass();
		ActivityOwnerHistoryUpdateClass  ActivityOwnerHistoryUpdateClass = new ActivityOwnerHistoryUpdateClass();
		ActivityPriorityUpdateClass  ActivityPriorityUpdateClass = new ActivityPriorityUpdateClass();
		ActivityStatusUpdateClass  ActivityStatusUpdateClass = new ActivityStatusUpdateClass();
		ActivityTimeTypeUpdateClass  ActivityTimeTypeUpdateClass = new ActivityTimeTypeUpdateClass();
		ActivityTypeUpdateClass  ActivityTypeUpdateClass = new ActivityTypeUpdateClass();
		CallTypeUpdateClass  CallTypeUpdateClass = new CallTypeUpdateClass();
		CorrespondenceUpdateClass  CorrespondenceUpdateClass = new CorrespondenceUpdateClass();
		CorrespondencesAttachmentUpdateClass  CorrespondencesAttachmentUpdateClass = new CorrespondencesAttachmentUpdateClass();
		CRMFilterSettingUpdateClass  CRMFilterSettingUpdateClass = new CRMFilterSettingUpdateClass();
		EmployeeGroupUpdateClass  EmployeeGroupUpdateClass = new EmployeeGroupUpdateClass();
		EmployeeGroupLineUpdateClass  EmployeeGroupLineUpdateClass = new EmployeeGroupLineUpdateClass();
		EscalationActionTimeIndicatorUpdateClass  EscalationActionTimeIndicatorUpdateClass = new EscalationActionTimeIndicatorUpdateClass();
		EscalationPreDefinitionUpdateClass  EscalationPreDefinitionUpdateClass = new EscalationPreDefinitionUpdateClass();
		OccasionUpdateClass  OccasionUpdateClass = new OccasionUpdateClass();
		OccasionContactUpdateClass  OccasionContactUpdateClass = new OccasionContactUpdateClass();
		OccasionInviteeUpdateClass  OccasionInviteeUpdateClass = new OccasionInviteeUpdateClass();
		OccasionStatusUpdateClass  OccasionStatusUpdateClass = new OccasionStatusUpdateClass();
		OccasionTypeUpdateClass  OccasionTypeUpdateClass = new OccasionTypeUpdateClass();
		OpportunityUpdateClass  OpportunityUpdateClass = new OpportunityUpdateClass();
		OpportunityAdditionalServiceUpdateClass  OpportunityAdditionalServiceUpdateClass = new OpportunityAdditionalServiceUpdateClass();
		OpportunityClosingReasonUpdateClass  OpportunityClosingReasonUpdateClass = new OpportunityClosingReasonUpdateClass();
		OpportunityCompetitorUpdateClass  OpportunityCompetitorUpdateClass = new OpportunityCompetitorUpdateClass();
		OpportunityCompetitorProductUpdateClass  OpportunityCompetitorProductUpdateClass = new OpportunityCompetitorProductUpdateClass();
		OpportunityProductUpdateClass  OpportunityProductUpdateClass = new OpportunityProductUpdateClass();
		OpportunityProductLocationUpdateClass  OpportunityProductLocationUpdateClass = new OpportunityProductLocationUpdateClass();
		OpportunityStageUpdateClass  OpportunityStageUpdateClass = new OpportunityStageUpdateClass();
		OpportunityTypeUpdateClass  OpportunityTypeUpdateClass = new OpportunityTypeUpdateClass();
		QuestionnaireUpdateClass  QuestionnaireUpdateClass = new QuestionnaireUpdateClass();
		QuestionnaireAnswerUpdateClass  QuestionnaireAnswerUpdateClass = new QuestionnaireAnswerUpdateClass();
		QuestionnaireAnswerLineUpdateClass  QuestionnaireAnswerLineUpdateClass = new QuestionnaireAnswerLineUpdateClass();
		QuestionnaireQuestionUpdateClass  QuestionnaireQuestionUpdateClass = new QuestionnaireQuestionUpdateClass();
		RatingUpdateClass  RatingUpdateClass = new RatingUpdateClass();
		SLAEscalationUpdateClass  SLAEscalationUpdateClass = new SLAEscalationUpdateClass();
		SLAEscalationRecepientUpdateClass  SLAEscalationRecepientUpdateClass = new SLAEscalationRecepientUpdateClass();
		SLAHeaderUpdateClass  SLAHeaderUpdateClass = new SLAHeaderUpdateClass();
		SLALineUpdateClass  SLALineUpdateClass = new SLALineUpdateClass();
		StageUpdateClass  StageUpdateClass = new StageUpdateClass();
		SupportMailboxUpdateClass  SupportMailboxUpdateClass = new SupportMailboxUpdateClass();
		TicketUpdateClass  TicketUpdateClass = new TicketUpdateClass();
		TicketClassificationUpdateClass  TicketClassificationUpdateClass = new TicketClassificationUpdateClass();
		TicketCreatedByTypeUpdateClass  TicketCreatedByTypeUpdateClass = new TicketCreatedByTypeUpdateClass();
		TicketEscalationUpdateClass  TicketEscalationUpdateClass = new TicketEscalationUpdateClass();
		TicketSeverityUpdateClass  TicketSeverityUpdateClass = new TicketSeverityUpdateClass();
		TicketSourceUpdateClass  TicketSourceUpdateClass = new TicketSourceUpdateClass();
		TicketStageUpdateClass  TicketStageUpdateClass = new TicketStageUpdateClass();
		TicketTypeUpdateClass  TicketTypeUpdateClass = new TicketTypeUpdateClass();
		TimeUnitUpdateClass  TimeUnitUpdateClass = new TimeUnitUpdateClass();
	
		public void LoadObjectsTenantZero(IWebFreightContext context)
        {
		    ICommonDataContext commonContext =  CommonDataContext.GetContext(0);
            ObjectContext = context;
			CommonContext = commonContext;
            TextCodeRepository = new TextCodeRepository(ObjectContext);
            ObjectTableRepository = new ObjectTableRepository(ObjectContext);
            ObjectFieldsRepository = new ObjectFieldRepository(ObjectContext);
			queriesRepository = new QueryRepository(ObjectContext);
			queryColumnsRepository = new QueryColumnRepository(ObjectContext);
            queryGroupRepository = new QueryGroupRepository(ObjectContext);
			advancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
			screensRepository = new ScreensRepository(ObjectContext);
			screenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
			objectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);
			EventTypeRepository = new EventTypeRepository(ObjectContext);
			menuButtonRepository = new MenuButtonRepository(ObjectContext);
			menuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
			FeaturesRepository = new FeatureRepository(CommonContext);
			EntityStatusRepository = new EntityStatusRepository(context);

            TextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a, StringComparer.OrdinalIgnoreCase);
            ObjectTables = ObjectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Name, a => a);
            ObjectFields = ObjectFieldsRepository.GetObjectFieldsByTenant(0).ToDictionary(d => d.FieldName + d.ObjectTableId, a => a);
			Queries = queriesRepository.GetQueriesByTenantSystemLevel(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            QueryColumns = queryColumnsRepository.GetQueryColumnsByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldCode, a => a);
			TenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantAdvancedFilters = advancedQueryFiltersRepository.GetAdvancedQueryFiltersByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldCode, a => a);
			tenantScreens = screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantScreenFields = screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenCode + d.ObjectFieldCode);
			TenantObjectTableTabs = objectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);
			tenantEventTypes = EventTypeRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantMenuButtons = menuButtonRepository.GetMenuButtonsByTenant(0).ToDictionary(d => d.EventCode + d.MenuButtonGroupId, a => a);
			tenantMenuButtonGroups = menuButtonGroupRepository.GetMenuButtonGroupsByTenant(0).ToDictionary(d => d.Name, a => a);
			AllEntityStatuses = EntityStatusRepository.GetEntityStatusByTenant(0).ToList();

			CreateAllObjectTables();
		    this.ObjectContext.SaveChanges();
			
			CreateAllObjectFields();
		    this.ObjectContext.SaveChanges();

			CreateAllQueries();
		    this.ObjectContext.SaveChanges();

			CreateAllScreens();
		    this.ObjectContext.SaveChanges();

			CreateAllTabs();
		    this.ObjectContext.SaveChanges();

			CreateAllEventTypes();
		    this.ObjectContext.SaveChanges();

			CreateAllClosedTables();
		    this.ObjectContext.SaveChanges();

			CreateAllFeatures();
			CreateAdditionalTextCodes();
		    this.ObjectContext.SaveChanges();
		    this.CommonContext.SaveChanges();

			CreateAllMenuButtons();
		    this.ObjectContext.SaveChanges();

        }


        public void CreateAllObjectTables()
        {
   
	   	   ActivityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityEmailRecipientUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityInviteeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityNoteUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityOwnerHistoryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityPriorityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityTimeTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ActivityTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   CallTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   CorrespondenceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   CorrespondencesAttachmentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   CRMFilterSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   EmployeeGroupUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   EmployeeGroupLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   EscalationPreDefinitionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OccasionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OccasionContactUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OccasionInviteeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OccasionStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OccasionTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityClosingReasonUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityCompetitorUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityCompetitorProductUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityProductUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityProductLocationUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityStageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OpportunityTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuestionnaireUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuestionnaireAnswerUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuestionnaireQuestionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   RatingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SLAEscalationUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SLAEscalationRecepientUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SLAHeaderUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SLALineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   StageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SupportMailboxUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketClassificationUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketCreatedByTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketEscalationUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketSeverityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketSourceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketStageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TicketTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TimeUnitUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   ActivityUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityEmailRecipientUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityInviteeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityNoteUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityOwnerHistoryUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityPriorityUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityTimeTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ActivityTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   CallTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   CorrespondenceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   CorrespondencesAttachmentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   CRMFilterSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   EmployeeGroupUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   EmployeeGroupLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   EscalationPreDefinitionUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OccasionUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OccasionContactUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OccasionInviteeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OccasionStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OccasionTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityClosingReasonUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityCompetitorUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityCompetitorProductUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityProductUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityProductLocationUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityStageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   OpportunityTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuestionnaireUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuestionnaireAnswerUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuestionnaireQuestionUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   RatingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SLAEscalationUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SLAEscalationRecepientUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SLAHeaderUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SLALineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   StageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SupportMailboxUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketClassificationUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketCreatedByTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketEscalationUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketSeverityUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketSourceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketStageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TicketTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TimeUnitUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   ActivityUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityInviteeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityNoteUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityPriorityUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityTimeTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ActivityTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   CallTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   CorrespondenceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   CRMFilterSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   EmployeeGroupUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   EmployeeGroupLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OccasionUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OccasionContactUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OccasionInviteeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OccasionStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OccasionTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityCompetitorUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityProductUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityProductLocationUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityStageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   OpportunityTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuestionnaireUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   RatingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SLAEscalationUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SLAHeaderUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SLALineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   StageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SupportMailboxUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketClassificationUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketEscalationUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketSeverityUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketSourceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketStageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TicketTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TimeUnitUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
        }

		public void CreateAllScreens()
        {
   
	   	   ActivityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityInviteeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityNoteUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityPriorityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityTimeTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ActivityTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   CallTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   CorrespondenceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   CRMFilterSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   EmployeeGroupUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   EmployeeGroupLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OccasionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OccasionContactUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OccasionInviteeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OccasionStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OccasionTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityCompetitorUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityProductUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityProductLocationUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityStageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OpportunityTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuestionnaireUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   RatingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SLAEscalationUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SLAHeaderUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SLALineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   StageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SupportMailboxUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketClassificationUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketEscalationUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketSeverityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketSourceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketStageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TicketTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TimeUnitUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   ActivityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityInviteeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityNoteUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityPriorityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityTimeTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ActivityTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   CallTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   CorrespondenceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   CRMFilterSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   EmployeeGroupUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   EmployeeGroupLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OccasionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OccasionContactUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OccasionInviteeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OccasionStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OccasionTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityCompetitorUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityProductUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityProductLocationUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityStageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OpportunityTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuestionnaireUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   RatingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SLAEscalationUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SLAHeaderUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SLALineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   StageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SupportMailboxUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketClassificationUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketEscalationUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketSeverityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketSourceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketStageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TicketTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TimeUnitUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   ActivityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityInviteeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityNoteUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityPriorityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityTimeTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ActivityTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   CallTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   CorrespondenceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   CRMFilterSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   EmployeeGroupUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   EmployeeGroupLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OccasionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OccasionContactUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OccasionInviteeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OccasionStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OccasionTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityCompetitorUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityProductUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityProductLocationUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityStageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OpportunityTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuestionnaireUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   RatingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SLAEscalationUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SLAHeaderUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SLALineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   StageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SupportMailboxUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketClassificationUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketEscalationUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketSeverityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketSourceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketStageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TicketTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TimeUnitUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   ActivityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityInviteeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityNoteUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityPriorityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityTimeTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CallTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CorrespondenceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CRMFilterSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EmployeeGroupUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EmployeeGroupLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionContactUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionInviteeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityCompetitorUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityProductUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityProductLocationUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityStageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   RatingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAEscalationUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAHeaderUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLALineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   StageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SupportMailboxUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketClassificationUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketEscalationUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketSeverityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketSourceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketStageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TimeUnitUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   ActivityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityInviteeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityNoteUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityPriorityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityTimeTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ActivityTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CallTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CorrespondenceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CRMFilterSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EmployeeGroupUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EmployeeGroupLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionContactUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionInviteeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OccasionTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityCompetitorUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityProductUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityProductLocationUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityStageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OpportunityTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   RatingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAEscalationUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLAHeaderUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SLALineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   StageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SupportMailboxUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketClassificationUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketEscalationUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketSeverityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketSourceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketStageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TicketTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TimeUnitUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   ActivityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityEmailRecipientUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityInviteeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityNoteUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityOwnerHistoryUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityPriorityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityTimeTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ActivityTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   CallTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   CorrespondenceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   CorrespondencesAttachmentUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   CRMFilterSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   EmployeeGroupUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   EmployeeGroupLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   EscalationActionTimeIndicatorUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   EscalationPreDefinitionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OccasionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OccasionContactUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OccasionInviteeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OccasionStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OccasionTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityAdditionalServiceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityClosingReasonUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityCompetitorUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityCompetitorProductUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityProductUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityProductLocationUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityStageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OpportunityTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuestionnaireUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuestionnaireAnswerUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuestionnaireAnswerLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuestionnaireQuestionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   RatingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SLAEscalationUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SLAEscalationRecepientUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SLAHeaderUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SLALineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   StageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SupportMailboxUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketClassificationUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketCreatedByTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketEscalationUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketSeverityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketSourceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketStageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TicketTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TimeUnitUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   
	   
	   	   ActivityPriorityUpdateClass.FillActivityPriority();
	
	   	   ActivityStatusUpdateClass.FillActivityStatus();
	
	   	   ActivityTimeTypeUpdateClass.FillActivityTimeType();
	
	   	   ActivityTypeUpdateClass.FillActivityType();
	
	   	   CallTypeUpdateClass.FillCallType();
	
	   
	   
	   
	   
	   
	   	   EscalationActionTimeIndicatorUpdateClass.FillEscalationActionTimeIndicator();
	
	   	   EscalationPreDefinitionUpdateClass.FillEscalationPreDefinition();
	
	   
	   
	   
	   	   OccasionStatusUpdateClass.FillOccasionStatus();
	
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   	   TicketCreatedByTypeUpdateClass.FillTicketCreatedByType();
	
	   
	   
	   	   TicketSourceUpdateClass.FillTicketSource();
	
	   
	   
	   	   TimeUnitUpdateClass.FillTimeUnit();
	
        }

   	 
	 

   }

}
