
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
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.QuoteModel.EntityUpdateClasses;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.QuoteModel
{
   public class QuoteModelUpdateClass
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
		 
	   	BorderTypeUpdateClass  BorderTypeUpdateClass = new BorderTypeUpdateClass();
		MarkUpTypeUpdateClass  MarkUpTypeUpdateClass = new MarkUpTypeUpdateClass();
		QuoteUpdateClass  QuoteUpdateClass = new QuoteUpdateClass();
		QuoteChargeUpdateClass  QuoteChargeUpdateClass = new QuoteChargeUpdateClass();
		QuoteClosingReasonUpdateClass  QuoteClosingReasonUpdateClass = new QuoteClosingReasonUpdateClass();
		QuoteCostChargeUpdateClass  QuoteCostChargeUpdateClass = new QuoteCostChargeUpdateClass();
		QuoteCustomerTypeUpdateClass  QuoteCustomerTypeUpdateClass = new QuoteCustomerTypeUpdateClass();
		QuotePackageUpdateClass  QuotePackageUpdateClass = new QuotePackageUpdateClass();
		QuotePriceStepsUpdateClass  QuotePriceStepsUpdateClass = new QuotePriceStepsUpdateClass();
		QuoteRatingUpdateClass  QuoteRatingUpdateClass = new QuoteRatingUpdateClass();
		QuoteSaleChargeUpdateClass  QuoteSaleChargeUpdateClass = new QuoteSaleChargeUpdateClass();
		QuoteSettingUpdateClass  QuoteSettingUpdateClass = new QuoteSettingUpdateClass();
		QuoteStageUpdateClass  QuoteStageUpdateClass = new QuoteStageUpdateClass();
		QuoteTemplateUpdateClass  QuoteTemplateUpdateClass = new QuoteTemplateUpdateClass();
		QuoteTemplateSectionUpdateClass  QuoteTemplateSectionUpdateClass = new QuoteTemplateSectionUpdateClass();
		QuoteTemplateSettingUpdateClass  QuoteTemplateSettingUpdateClass = new QuoteTemplateSettingUpdateClass();
		QuoteTemplateTableDesignUpdateClass  QuoteTemplateTableDesignUpdateClass = new QuoteTemplateTableDesignUpdateClass();
		QuoteTemplateTextCodeUpdateClass  QuoteTemplateTextCodeUpdateClass = new QuoteTemplateTextCodeUpdateClass();
		QuoteTemplateTextDesignUpdateClass  QuoteTemplateTextDesignUpdateClass = new QuoteTemplateTextDesignUpdateClass();
		QuoteTotalVATUpdateClass  QuoteTotalVATUpdateClass = new QuoteTotalVATUpdateClass();
		QuoteTypeUpdateClass  QuoteTypeUpdateClass = new QuoteTypeUpdateClass();
		QuoteVATsTotalUpdateClass  QuoteVATsTotalUpdateClass = new QuoteVATsTotalUpdateClass();
	
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


        public void CreateAllObjectTables()
        {
   
	   	   BorderTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   MarkUpTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteClosingReasonUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteCostChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteCustomerTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuotePackageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuotePriceStepsUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteRatingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteSaleChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteStageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateSectionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteVATsTotalUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   BorderTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   MarkUpTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteChargeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteClosingReasonUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteCostChargeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteCustomerTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuotePackageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuotePriceStepsUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteRatingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteSaleChargeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteStageUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateSectionUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTotalVATUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   QuoteVATsTotalUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   BorderTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   MarkUpTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteChargeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteClosingReasonUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteCostChargeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuotePackageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuotePriceStepsUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteRatingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteSaleChargeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteStageUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTotalVATUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   QuoteVATsTotalUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
        }

		public void CreateAllScreens()
        {
   
	   	   BorderTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   MarkUpTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteClosingReasonUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteCostChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuotePackageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuotePriceStepsUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteRatingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteSaleChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteStageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTotalVATUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteVATsTotalUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   BorderTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   MarkUpTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteClosingReasonUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteCostChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuotePackageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuotePriceStepsUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteRatingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteSaleChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteStageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteVATsTotalUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   BorderTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   MarkUpTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteClosingReasonUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteCostChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuotePackageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuotePriceStepsUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteRatingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteSaleChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteStageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteVATsTotalUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   BorderTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MarkUpTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteClosingReasonUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteCostChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuotePackageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuotePriceStepsUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteRatingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteSaleChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteStageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTotalVATUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteVATsTotalUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   BorderTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MarkUpTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteClosingReasonUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteCostChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuotePackageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuotePriceStepsUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteRatingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteSaleChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteStageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteVATsTotalUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   BorderTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   MarkUpTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteClosingReasonUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteCostChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteCustomerTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuotePackageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuotePriceStepsUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteRatingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteSaleChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteStageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateSectionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateTableDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateTextCodeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTemplateTextDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteVATsTotalUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   	   MarkUpTypeUpdateClass.FillMarkUpType();
	
	   
	   
	   	   QuoteClosingReasonUpdateClass.FillQuoteClosingReason();
	
	   
	   	   QuoteCustomerTypeUpdateClass.FillQuoteCustomerType();
	
	   
	   
	   	   QuoteRatingUpdateClass.FillQuoteRating();
	
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   	   QuoteTypeUpdateClass.FillQuoteType();
	
	   
        }

   	 
	 

   }

}
