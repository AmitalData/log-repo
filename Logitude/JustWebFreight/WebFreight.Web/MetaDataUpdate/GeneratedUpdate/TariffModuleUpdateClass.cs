
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
		 
	   	TariffUpdateClass  TariffUpdateClass = new TariffUpdateClass();
		TariffLineUpdateClass  TariffLineUpdateClass = new TariffLineUpdateClass();
		TariffLinesContainersPriceUpdateClass  TariffLinesContainersPriceUpdateClass = new TariffLinesContainersPriceUpdateClass();
		TariffSettingUpdateClass  TariffSettingUpdateClass = new TariffSettingUpdateClass();
		TariffSurchargesUpdateUpdateClass  TariffSurchargesUpdateUpdateClass = new TariffSurchargesUpdateUpdateClass();
		TariffSurchargesUpdateMethodUpdateClass  TariffSurchargesUpdateMethodUpdateClass = new TariffSurchargesUpdateMethodUpdateClass();
		TariffTypeUpdateClass  TariffTypeUpdateClass = new TariffTypeUpdateClass();
		TariffVersionUpdateClass  TariffVersionUpdateClass = new TariffVersionUpdateClass();
		TariffVersionAllInChargeUpdateClass  TariffVersionAllInChargeUpdateClass = new TariffVersionAllInChargeUpdateClass();
	
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
   
	   	   TariffUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffLinesContainersPriceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffVersionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   TariffVersionAllInChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   TariffUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffLinesContainersPriceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffVersionUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   TariffVersionAllInChargeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   TariffUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffVersionUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
        }

		public void CreateAllScreens()
        {
   
	   	   TariffUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   TariffUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   TariffUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffVersionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   TariffUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   TariffUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   TariffUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffLinesContainersPriceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSurchargesUpdateUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffSurchargesUpdateMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffVersionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   TariffVersionAllInChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   
	   
	   	   TariffSurchargesUpdateMethodUpdateClass.FillTariffSurchargesUpdateMethod();
	
	   	   TariffTypeUpdateClass.FillTariffType();
	
	   
	   
        }

   	 
	 

   }

}
