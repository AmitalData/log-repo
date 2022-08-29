
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
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate
{
   public class DashboardModuleUpdateClass
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
		 
	   	AnalyticsFactsFieldsMetaDataUpdateClass  AnalyticsFactsFieldsMetaDataUpdateClass = new AnalyticsFactsFieldsMetaDataUpdateClass();
		AnalyticsFactsMetaDataUpdateClass  AnalyticsFactsMetaDataUpdateClass = new AnalyticsFactsMetaDataUpdateClass();
		DashboardUpdateClass  DashboardUpdateClass = new DashboardUpdateClass();
		MeasureTypeUpdateClass  MeasureTypeUpdateClass = new MeasureTypeUpdateClass();
		WidgetUpdateClass  WidgetUpdateClass = new WidgetUpdateClass();
		WidgetMeasureUpdateClass  WidgetMeasureUpdateClass = new WidgetMeasureUpdateClass();
		WidgetTypeUpdateClass  WidgetTypeUpdateClass = new WidgetTypeUpdateClass();
	
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
			FeaturesRepository = new FeatureRepository(CommonContext);
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
 			TablesHashStrings.Add("AnalyticsFactsFieldsMetaData",  AnalyticsFactsFieldsMetaDataUpdateClass.HashString);
			TablesHashStrings.Add("AnalyticsFactsMetaData",  AnalyticsFactsMetaDataUpdateClass.HashString);
			TablesHashStrings.Add("Dashboard",  DashboardUpdateClass.HashString);
			TablesHashStrings.Add("MeasureType",  MeasureTypeUpdateClass.HashString);
			TablesHashStrings.Add("Widget",  WidgetUpdateClass.HashString);
			TablesHashStrings.Add("WidgetMeasure",  WidgetMeasureUpdateClass.HashString);
			TablesHashStrings.Add("WidgetType",  WidgetTypeUpdateClass.HashString);
			return TablesHashStrings;
        }
        public void CreateAllObjectTablesMetadata()
        {
   
			if(MetadataUpdateUtility.IsChangedMetadataTable("AnalyticsFactsFieldsMetaData", ObjectTables, AnalyticsFactsFieldsMetaDataUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("AnalyticsFactsFieldsMetaData");
					AnalyticsFactsFieldsMetaDataUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsFieldsMetaDataUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AnalyticsFactsMetaData", ObjectTables, AnalyticsFactsMetaDataUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("AnalyticsFactsMetaData");
					AnalyticsFactsMetaDataUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					AnalyticsFactsMetaDataUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					AnalyticsFactsMetaDataUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("Dashboard", ObjectTables, DashboardUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Dashboard");
					DashboardUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					DashboardUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					DashboardUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("MeasureType", ObjectTables, MeasureTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("MeasureType");
					MeasureTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					MeasureTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MeasureTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				MeasureTypeUpdateClass.FillMeasureType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("Widget", ObjectTables, WidgetUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Widget");
					WidgetUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WidgetUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WidgetMeasure", ObjectTables, WidgetMeasureUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WidgetMeasure");
					WidgetMeasureUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WidgetMeasureUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetMeasureUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("WidgetType", ObjectTables, WidgetTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("WidgetType");
					WidgetTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					WidgetTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					WidgetTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				WidgetTypeUpdateClass.FillWidgetType();

 
			}

        }
   

        public void CreateAllObjectTables()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   DashboardUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   MeasureTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WidgetUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WidgetMeasureUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WidgetTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   //AnalyticsFactsFieldsMetaDataUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //AnalyticsFactsMetaDataUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //DashboardUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //MeasureTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WidgetUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WidgetMeasureUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //WidgetTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   DashboardUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   MeasureTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WidgetUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WidgetMeasureUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   WidgetTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
        }

		public void CreateAllScreens()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   DashboardUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   MeasureTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WidgetUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WidgetMeasureUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WidgetTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   DashboardUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   MeasureTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WidgetUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WidgetMeasureUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WidgetTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   DashboardUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   MeasureTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WidgetUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WidgetMeasureUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WidgetTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   DashboardUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MeasureTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetMeasureUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   DashboardUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MeasureTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetMeasureUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WidgetTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   AnalyticsFactsFieldsMetaDataUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AnalyticsFactsMetaDataUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   DashboardUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   MeasureTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WidgetUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WidgetMeasureUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WidgetTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   	   MeasureTypeUpdateClass.FillMeasureType();
	
	   
	   
	   	   WidgetTypeUpdateClass.FillWidgetType();
	
        }
 	 
	 

   }

}
