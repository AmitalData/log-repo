
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
   public class WarehouseLibUpdateClass
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
		 
	   	WarehouseEntryUpdateClass  WarehouseEntryUpdateClass = new WarehouseEntryUpdateClass();
		WarehouseEntryPackageUpdateClass  WarehouseEntryPackageUpdateClass = new WarehouseEntryPackageUpdateClass();
		WarehouseEntryPackagesReleaseUpdateClass  WarehouseEntryPackagesReleaseUpdateClass = new WarehouseEntryPackagesReleaseUpdateClass();
		WarehouseEntryStatusUpdateClass  WarehouseEntryStatusUpdateClass = new WarehouseEntryStatusUpdateClass();
		WarehouseReleaseUpdateClass  WarehouseReleaseUpdateClass = new WarehouseReleaseUpdateClass();
		WarehouseReleasePackageUpdateClass  WarehouseReleasePackageUpdateClass = new WarehouseReleasePackageUpdateClass();
		WarehouseReleaseStatusUpdateClass  WarehouseReleaseStatusUpdateClass = new WarehouseReleaseStatusUpdateClass();
	
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
   
	   	   WarehouseEntryUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseEntryPackageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseEntryStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseReleaseUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseReleasePackageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   WarehouseReleaseStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   WarehouseEntryUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseEntryPackageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseEntryStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseReleaseUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseReleasePackageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   WarehouseReleaseStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseReleaseUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
        }

		public void CreateAllScreens()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseReleaseUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseReleaseUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseReleaseUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleaseUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleaseUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   WarehouseEntryUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseEntryPackageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseEntryPackagesReleaseUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseEntryStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseReleaseUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseReleasePackageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   WarehouseReleaseStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   	   WarehouseEntryStatusUpdateClass.FillWarehouseEntryStatus();
	
	   
	   
	   	   WarehouseReleaseStatusUpdateClass.FillWarehouseReleaseStatus();
	
        }

   	 
	 

   }

}
