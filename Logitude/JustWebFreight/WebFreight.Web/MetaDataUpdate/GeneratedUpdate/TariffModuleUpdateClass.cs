
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
   public class TariffModuleUpdateClass
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
		 
	   	TariffUpdateClass  TariffUpdateClass = new TariffUpdateClass();
		TariffLineUpdateClass  TariffLineUpdateClass = new TariffLineUpdateClass();
		TariffLinesContainersPriceUpdateClass  TariffLinesContainersPriceUpdateClass = new TariffLinesContainersPriceUpdateClass();
		TariffProductUpdateClass  TariffProductUpdateClass = new TariffProductUpdateClass();
		TariffSettingUpdateClass  TariffSettingUpdateClass = new TariffSettingUpdateClass();
		TariffSurchargesUpdateUpdateClass  TariffSurchargesUpdateUpdateClass = new TariffSurchargesUpdateUpdateClass();
		TariffSurchargesUpdateMethodUpdateClass  TariffSurchargesUpdateMethodUpdateClass = new TariffSurchargesUpdateMethodUpdateClass();
		TariffTypeUpdateClass  TariffTypeUpdateClass = new TariffTypeUpdateClass();
		TariffVersionUpdateClass  TariffVersionUpdateClass = new TariffVersionUpdateClass();
		TariffVersionAllInChargeUpdateClass  TariffVersionAllInChargeUpdateClass = new TariffVersionAllInChargeUpdateClass();
		TariffVersionUploadedExcelUpdateClass  TariffVersionUploadedExcelUpdateClass = new TariffVersionUploadedExcelUpdateClass();
	
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
 			TablesHashStrings.Add("Tariff",  TariffUpdateClass.HashString);
			TablesHashStrings.Add("TariffLine",  TariffLineUpdateClass.HashString);
			TablesHashStrings.Add("TariffLinesContainersPrice",  TariffLinesContainersPriceUpdateClass.HashString);
			TablesHashStrings.Add("TariffProduct",  TariffProductUpdateClass.HashString);
			TablesHashStrings.Add("TariffSetting",  TariffSettingUpdateClass.HashString);
			TablesHashStrings.Add("TariffSurchargesUpdate",  TariffSurchargesUpdateUpdateClass.HashString);
			TablesHashStrings.Add("TariffSurchargesUpdateMethod",  TariffSurchargesUpdateMethodUpdateClass.HashString);
			TablesHashStrings.Add("TariffType",  TariffTypeUpdateClass.HashString);
			TablesHashStrings.Add("TariffVersion",  TariffVersionUpdateClass.HashString);
			TablesHashStrings.Add("TariffVersionAllInCharge",  TariffVersionAllInChargeUpdateClass.HashString);
			TablesHashStrings.Add("TariffVersionUploadedExcel",  TariffVersionUploadedExcelUpdateClass.HashString);
			return TablesHashStrings;
        }
        public void CreateAllObjectTablesMetadata()
        {
   
			if(MetadataUpdateUtility.IsChangedMetadataTable("Tariff", ObjectTables, TariffUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("Tariff");
					TariffUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffLine", ObjectTables, TariffLineUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffLine");
					TariffLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffLineUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLineUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffLinesContainersPrice", ObjectTables, TariffLinesContainersPriceUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffLinesContainersPrice");
					TariffLinesContainersPriceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffLinesContainersPriceUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffLinesContainersPriceUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffProduct", ObjectTables, TariffProductUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffProduct");
					TariffProductUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffProductUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffProductUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffSetting", ObjectTables, TariffSettingUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffSetting");
					TariffSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffSurchargesUpdate", ObjectTables, TariffSurchargesUpdateUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffSurchargesUpdate");
					TariffSurchargesUpdateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffSurchargesUpdateUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffSurchargesUpdateMethod", ObjectTables, TariffSurchargesUpdateMethodUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffSurchargesUpdateMethod");
					TariffSurchargesUpdateMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffSurchargesUpdateMethodUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffSurchargesUpdateMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				TariffSurchargesUpdateMethodUpdateClass.FillTariffSurchargesUpdateMethod();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffType", ObjectTables, TariffTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffType");
					TariffTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				TariffTypeUpdateClass.FillTariffType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffVersion", ObjectTables, TariffVersionUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffVersion");
					TariffVersionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffVersionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffVersionAllInCharge", ObjectTables, TariffVersionAllInChargeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffVersionAllInCharge");
					TariffVersionAllInChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffVersionAllInChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionAllInChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("TariffVersionUploadedExcel", ObjectTables, TariffVersionUploadedExcelUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("TariffVersionUploadedExcel");
					TariffVersionUploadedExcelUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					TariffVersionUploadedExcelUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					TariffVersionUploadedExcelUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

        }
   

        public void CreateAllObjectTables()
        {
   
	   	   TariffUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffLinesContainersPriceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffProductUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffVersionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffVersionAllInChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   //TariffUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffLineUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffLinesContainersPriceUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffProductUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffSurchargesUpdateUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffSurchargesUpdateMethodUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffVersionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffVersionAllInChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //TariffVersionUploadedExcelUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   TariffUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffLineUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffProductUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffSettingUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffVersionUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
        }

		public void CreateAllScreens()
        {
   
	   	   TariffUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffProductUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   TariffUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffProductUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   TariffUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffProductUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffVersionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   TariffUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffProductUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   TariffUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffProductUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   TariffUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffProductUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffVersionUploadedExcelUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   
	   
	   
	   	   TariffSurchargesUpdateMethodUpdateClass.FillTariffSurchargesUpdateMethod();
	
	   	   TariffTypeUpdateClass.FillTariffType();
	
	   
	   
	   
        }
 	 
	 

   }

}
