
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate
{
   public class WorkflowUpdateClass
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
		Dictionary<string, QueryGroup> tenantQueryGroups {get; set; }
		 
	   	ExpressionUpdateClass  ExpressionUpdateClass = new ExpressionUpdateClass();
		ExpressionCategoryUpdateClass  ExpressionCategoryUpdateClass = new ExpressionCategoryUpdateClass();
		OperatorUpdateClass  OperatorUpdateClass = new OperatorUpdateClass();
		OperatorCategoryUpdateClass  OperatorCategoryUpdateClass = new OperatorCategoryUpdateClass();
		ServiceProviderSubscriptionUpdateClass  ServiceProviderSubscriptionUpdateClass = new ServiceProviderSubscriptionUpdateClass();
		TaskUpdateClass  TaskUpdateClass = new TaskUpdateClass();
		TaskExtendedUpdateClass  TaskExtendedUpdateClass = new TaskExtendedUpdateClass();
		TaskPriorityUpdateClass  TaskPriorityUpdateClass = new TaskPriorityUpdateClass();
		TaskStatusUpdateClass  TaskStatusUpdateClass = new TaskStatusUpdateClass();
		TaskTypeUpdateClass  TaskTypeUpdateClass = new TaskTypeUpdateClass();
		WorkFlowUpdateClass  WorkFlowUpdateClass = new WorkFlowUpdateClass();
		WorkFlowInstanceUpdateClass  WorkFlowInstanceUpdateClass = new WorkFlowInstanceUpdateClass();
		WorkFlowInstanceActivityUpdateClass  WorkFlowInstanceActivityUpdateClass = new WorkFlowInstanceActivityUpdateClass();
		WorkFlowInstanceActivityStatusUpdateClass  WorkFlowInstanceActivityStatusUpdateClass = new WorkFlowInstanceActivityStatusUpdateClass();
		WorkFlowInstanceStatusUpdateClass  WorkFlowInstanceStatusUpdateClass = new WorkFlowInstanceStatusUpdateClass();
		WorkFlowInstanceVariableUpdateClass  WorkFlowInstanceVariableUpdateClass = new WorkFlowInstanceVariableUpdateClass();
		WorkFlowStatusUpdateClass  WorkFlowStatusUpdateClass = new WorkFlowStatusUpdateClass();
		WorkFlowTriggerTypeUpdateClass  WorkFlowTriggerTypeUpdateClass = new WorkFlowTriggerTypeUpdateClass();
		WorkFlowVersionUpdateClass  WorkFlowVersionUpdateClass = new WorkFlowVersionUpdateClass();
		WorkFlowVersionStatusUpdateClass  WorkFlowVersionStatusUpdateClass = new WorkFlowVersionStatusUpdateClass();
	
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
			FeaturesRepository = new FeatureRepository(GlobalContext.GetContext());
			EntityStatusRepository = new EntityStatusRepository(context);

            TextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a, StringComparer.OrdinalIgnoreCase);
            ObjectTables = ObjectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Name, a => a);
            ObjectFields = ObjectFieldsRepository.GetObjectFieldsByTenant(0).ToDictionary(d => d.FieldName + d.ObjectTableId, a => a);
			Queries = queriesRepository.GetQueriesByTenantSystemLevel(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            QueryColumns = queryColumnsRepository.GetQueryColumnsByTenant(0).ToDictionary(d => d.QueryCode + d.ObjectFieldCode, a => a);
			TenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantAdvancedFilters = advancedQueryFiltersRepository.GetAdvancedQueryFiltersByTenant(0).ToDictionary(d => d.QueryCode + d.ObjectFieldCode, a => a);
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

		public void LoadObjectTablesMetadata(IWebFreightContext context, bool runPostDeleteProcedure)
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
			FeaturesRepository = new FeatureRepository(GlobalContext.GetContext());
			EntityStatusRepository = new EntityStatusRepository(context);

            ObjectTables = ObjectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Name, a => a);
            ObjectTable generalTable = ObjectTables["General"];
            TextCodes = TextCodeRepository.GetTextCodesByTenant(0).Where(t => t.ObjectTableId == generalTable.Id).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a, StringComparer.OrdinalIgnoreCase);
			TenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).Where(t => t.ObjectTableId == generalTable.Id).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			ObjectFields = new Dictionary<string, ObjectField>();//ObjectFieldsRepository.GetObjectFieldsByTenant(0).ToDictionary(d => d.FieldName + d.ObjectTableId, a => a);
			Queries = new Dictionary<string, Query>();//queriesRepository.GetQueriesByTenantSystemLevel(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			QueryColumns = new Dictionary<string, QueryColumn>();//queryColumnsRepository.GetQueryColumnsByTenant(0).ToDictionary(d => d.QueryCode + d.ObjectFieldCode, a => a);
			tenantAdvancedFilters = new Dictionary<string, AdvancedQueryFilter>();//advancedQueryFiltersRepository.GetAdvancedQueryFiltersByTenant(0).ToDictionary(d => d.QueryCode + d.ObjectFieldCode, a => a);
			tenantScreens = new Dictionary<string, Screen>();//screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantScreenFields = new Dictionary<string, ScreenField>();//screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenCode + d.ObjectFieldCode);
			TenantObjectTableTabs = new Dictionary<string, ObjectTableTab>();//objectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);
			tenantEventTypes = EventTypeRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
			tenantMenuButtons = new Dictionary<string, MenuButton>();//menuButtonRepository.GetMenuButtonsByTenant(0).ToDictionary(d => d.EventCode + d.MenuButtonGroupId, a => a);
			tenantMenuButtonGroups = menuButtonGroupRepository.GetMenuButtonGroupsByTenant(0).ToDictionary(d => d.Name, a => a);
			AllEntityStatuses = EntityStatusRepository.GetEntityStatusByTenant(0).ToList();
			tenantQueryGroups = queryGroupRepository.GetQueryGroups().ToDictionary(d => d.Code, a => a);	

			 MetadataUpdateUtility.RunPreDeleteProcedure();

			 CreateAllObjectTablesMetadata();
			 
 
			 this.ObjectContext.SaveChanges();
			 this.CommonContext.SaveChanges();
			 if(runPostDeleteProcedure)
			 {
				MetadataUpdateUtility.RunPostDeleteProcedure();
			 }
			//CreateAllObjectTables();
		    //this.ObjectContext.SaveChanges();
			//
			//CreateAllObjectFields();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllQueries();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllScreens();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllTabs();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllEventTypes();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllClosedTables();
		    //this.ObjectContext.SaveChanges();
//
			//CreateAllFeatures();
			//CreateAdditionalTextCodes();
		    //this.ObjectContext.SaveChanges();
		    //this.CommonContext.SaveChanges();
//
			//CreateAllMenuButtons();
		    //this.ObjectContext.SaveChanges();
//
        }

		private static Dictionary<string,string> TablesHashStrings { get; set; }		 
	    public static Dictionary<string, string> GetAllTablesHashStrings()
        {
			if (TablesHashStrings != null)
				return TablesHashStrings;

			TablesHashStrings = new Dictionary<string, string>();
 			TablesHashStrings.Add("Expression",  ExpressionUpdateClass.HashString);
			TablesHashStrings.Add("ExpressionCategory",  ExpressionCategoryUpdateClass.HashString);
			TablesHashStrings.Add("Operator",  OperatorUpdateClass.HashString);
			TablesHashStrings.Add("OperatorCategory",  OperatorCategoryUpdateClass.HashString);
			TablesHashStrings.Add("ServiceProviderSubscription",  ServiceProviderSubscriptionUpdateClass.HashString);
			TablesHashStrings.Add("Task",  TaskUpdateClass.HashString);
			TablesHashStrings.Add("TaskExtended",  TaskExtendedUpdateClass.HashString);
			TablesHashStrings.Add("TaskPriority",  TaskPriorityUpdateClass.HashString);
			TablesHashStrings.Add("TaskStatus",  TaskStatusUpdateClass.HashString);
			TablesHashStrings.Add("TaskType",  TaskTypeUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlow",  WorkFlowUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowInstance",  WorkFlowInstanceUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowInstanceActivity",  WorkFlowInstanceActivityUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowInstanceActivityStatus",  WorkFlowInstanceActivityStatusUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowInstanceStatus",  WorkFlowInstanceStatusUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowInstanceVariable",  WorkFlowInstanceVariableUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowStatus",  WorkFlowStatusUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowTriggerType",  WorkFlowTriggerTypeUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowVersion",  WorkFlowVersionUpdateClass.HashString);
			TablesHashStrings.Add("WorkFlowVersionStatus",  WorkFlowVersionStatusUpdateClass.HashString);
			return TablesHashStrings;
        }
        public void CreateAllObjectTablesMetadata()
        {
   
			if(MetadataUpdateUtility.IsChangedMetadataTable("Expression", ObjectTables, ExpressionUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Expression");
					ExpressionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					ExpressionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				ExpressionUpdateClass.FillExpression();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ExpressionCategory", ObjectTables, ExpressionCategoryUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("ExpressionCategory");
					ExpressionCategoryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					ExpressionCategoryUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ExpressionCategoryUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				ExpressionCategoryUpdateClass.FillExpressionCategory();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("Operator", ObjectTables, OperatorUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Operator");
					OperatorUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					OperatorUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				OperatorUpdateClass.FillOperator();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("OperatorCategory", ObjectTables, OperatorCategoryUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("OperatorCategory");
					OperatorCategoryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					OperatorCategoryUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OperatorCategoryUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				OperatorCategoryUpdateClass.FillOperatorCategory();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ServiceProviderSubscription", ObjectTables, ServiceProviderSubscriptionUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("ServiceProviderSubscription");
					ServiceProviderSubscriptionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					ServiceProviderSubscriptionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					ServiceProviderSubscriptionUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("Task", ObjectTables, TaskUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Task");
					TaskUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TaskUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TaskExtended", ObjectTables, TaskExtendedUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TaskExtended");
					TaskExtendedUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TaskExtendedUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskExtendedUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TaskPriority", ObjectTables, TaskPriorityUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TaskPriority");
					TaskPriorityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TaskPriorityUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskPriorityUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TaskStatus", ObjectTables, TaskStatusUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TaskStatus");
					TaskStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TaskStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TaskType", ObjectTables, TaskTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TaskType");
					TaskTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TaskTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TaskTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlow", ObjectTables, WorkFlowUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlow");
					WorkFlowUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowInstance", ObjectTables, WorkFlowInstanceUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowInstance");
					WorkFlowInstanceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowInstanceUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowInstanceActivity", ObjectTables, WorkFlowInstanceActivityUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowInstanceActivity");
					WorkFlowInstanceActivityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowInstanceActivityUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowInstanceActivityStatus", ObjectTables, WorkFlowInstanceActivityStatusUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowInstanceActivityStatus");
					WorkFlowInstanceActivityStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowInstanceActivityStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceActivityStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WorkFlowInstanceActivityStatusUpdateClass.FillWorkFlowInstanceActivityStatus();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowInstanceStatus", ObjectTables, WorkFlowInstanceStatusUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowInstanceStatus");
					WorkFlowInstanceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowInstanceStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WorkFlowInstanceStatusUpdateClass.FillWorkFlowInstanceStatus();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowInstanceVariable", ObjectTables, WorkFlowInstanceVariableUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowInstanceVariable");
					WorkFlowInstanceVariableUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowInstanceVariableUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowInstanceVariableUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowStatus", ObjectTables, WorkFlowStatusUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowStatus");
					WorkFlowStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WorkFlowStatusUpdateClass.FillWorkFlowStatus();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowTriggerType", ObjectTables, WorkFlowTriggerTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowTriggerType");
					WorkFlowTriggerTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowTriggerTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowTriggerTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WorkFlowTriggerTypeUpdateClass.FillWorkFlowTriggerType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowVersion", ObjectTables, WorkFlowVersionUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowVersion");
					WorkFlowVersionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowVersionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WorkFlowVersionStatus", ObjectTables, WorkFlowVersionStatusUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WorkFlowVersionStatus");
					WorkFlowVersionStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WorkFlowVersionStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WorkFlowVersionStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WorkFlowVersionStatusUpdateClass.FillWorkFlowVersionStatus();

 
			}

        }
   

        public void CreateAllObjectTables()
        {
   
	   	   ExpressionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ExpressionCategoryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OperatorUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OperatorCategoryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TaskUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TaskExtendedUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TaskPriorityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TaskStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TaskTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowInstanceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowVersionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WorkFlowVersionStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   //ExpressionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //ExpressionCategoryUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //OperatorUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //OperatorCategoryUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //ServiceProviderSubscriptionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TaskUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TaskExtendedUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TaskPriorityUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TaskStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TaskTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowInstanceUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowInstanceActivityUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowInstanceActivityStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowInstanceStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowInstanceVariableUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowTriggerTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowVersionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WorkFlowVersionStatusUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   ExpressionUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   ExpressionCategoryUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   OperatorUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   OperatorCategoryUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TaskUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TaskExtendedUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TaskPriorityUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TaskStatusUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TaskTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowInstanceUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowStatusUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowVersionUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
        }

		public void CreateAllScreens()
        {
   
	   	   ExpressionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ExpressionCategoryUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OperatorUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OperatorCategoryUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TaskUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TaskExtendedUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TaskPriorityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TaskStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TaskTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowInstanceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowVersionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   ExpressionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ExpressionCategoryUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OperatorUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OperatorCategoryUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TaskUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TaskExtendedUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TaskPriorityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TaskStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TaskTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowInstanceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowVersionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   ExpressionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ExpressionCategoryUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OperatorUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OperatorCategoryUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TaskUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TaskExtendedUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TaskPriorityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TaskStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TaskTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowInstanceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowVersionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   ExpressionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ExpressionCategoryUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OperatorUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OperatorCategoryUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskExtendedUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskPriorityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowVersionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   ExpressionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ExpressionCategoryUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OperatorUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OperatorCategoryUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskExtendedUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskPriorityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TaskTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowVersionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   ExpressionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ExpressionCategoryUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OperatorUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OperatorCategoryUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ServiceProviderSubscriptionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TaskUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TaskExtendedUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TaskPriorityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TaskStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TaskTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowInstanceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowInstanceActivityUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowInstanceActivityStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowInstanceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowInstanceVariableUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowTriggerTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowVersionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WorkFlowVersionStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   	   ExpressionUpdateClass.FillExpression();
	
	   	   ExpressionCategoryUpdateClass.FillExpressionCategory();
	
	   	   OperatorUpdateClass.FillOperator();
	
	   	   OperatorCategoryUpdateClass.FillOperatorCategory();
	
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   	   WorkFlowInstanceActivityStatusUpdateClass.FillWorkFlowInstanceActivityStatus();
	
	   	   WorkFlowInstanceStatusUpdateClass.FillWorkFlowInstanceStatus();
	
	   
	   	   WorkFlowStatusUpdateClass.FillWorkFlowStatus();
	
	   	   WorkFlowTriggerTypeUpdateClass.FillWorkFlowTriggerType();
	
	   
	   	   WorkFlowVersionStatusUpdateClass.FillWorkFlowVersionStatus();
	
        }
 	 
	 

   }

}
