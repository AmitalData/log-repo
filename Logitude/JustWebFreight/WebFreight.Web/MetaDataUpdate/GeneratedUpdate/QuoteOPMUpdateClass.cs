
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
   public class QuoteOPMUpdateClass
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
		 
	   	BorderOPTypeUpdateClass  BorderOPTypeUpdateClass = new BorderOPTypeUpdateClass();
		MarkUpOPTypeUpdateClass  MarkUpOPTypeUpdateClass = new MarkUpOPTypeUpdateClass();
		OPSpecialServicesTypeUpdateClass  OPSpecialServicesTypeUpdateClass = new OPSpecialServicesTypeUpdateClass();
		QuoteOPUpdateClass  QuoteOPUpdateClass = new QuoteOPUpdateClass();
		QuoteOPChargeUpdateClass  QuoteOPChargeUpdateClass = new QuoteOPChargeUpdateClass();
		QuoteOPClosingReasonUpdateClass  QuoteOPClosingReasonUpdateClass = new QuoteOPClosingReasonUpdateClass();
		QuoteOPComputedFieldUpdateClass  QuoteOPComputedFieldUpdateClass = new QuoteOPComputedFieldUpdateClass();
		QuoteOPCostChargeUpdateClass  QuoteOPCostChargeUpdateClass = new QuoteOPCostChargeUpdateClass();
		QuoteOPCustomerTypeUpdateClass  QuoteOPCustomerTypeUpdateClass = new QuoteOPCustomerTypeUpdateClass();
		QuoteOPPackageUpdateClass  QuoteOPPackageUpdateClass = new QuoteOPPackageUpdateClass();
		QuoteOPPriceStepsUpdateClass  QuoteOPPriceStepsUpdateClass = new QuoteOPPriceStepsUpdateClass();
		QuoteOPRatingUpdateClass  QuoteOPRatingUpdateClass = new QuoteOPRatingUpdateClass();
		QuoteOPSaleChargeUpdateClass  QuoteOPSaleChargeUpdateClass = new QuoteOPSaleChargeUpdateClass();
		QuoteOPSalesTotalUpdateClass  QuoteOPSalesTotalUpdateClass = new QuoteOPSalesTotalUpdateClass();
		QuoteOPSettingUpdateClass  QuoteOPSettingUpdateClass = new QuoteOPSettingUpdateClass();
		QuoteOPStageUpdateClass  QuoteOPStageUpdateClass = new QuoteOPStageUpdateClass();
		QuoteOPTemplateUpdateClass  QuoteOPTemplateUpdateClass = new QuoteOPTemplateUpdateClass();
		QuoteOPTemplateSectionUpdateClass  QuoteOPTemplateSectionUpdateClass = new QuoteOPTemplateSectionUpdateClass();
		QuoteOPTemplateSectionTypeUpdateClass  QuoteOPTemplateSectionTypeUpdateClass = new QuoteOPTemplateSectionTypeUpdateClass();
		QuoteOPTemplateSettingUpdateClass  QuoteOPTemplateSettingUpdateClass = new QuoteOPTemplateSettingUpdateClass();
		QuoteOPTemplateTableDesignUpdateClass  QuoteOPTemplateTableDesignUpdateClass = new QuoteOPTemplateTableDesignUpdateClass();
		QuoteOPTemplateTextCodeUpdateClass  QuoteOPTemplateTextCodeUpdateClass = new QuoteOPTemplateTextCodeUpdateClass();
		QuoteOPTemplateTextDesignUpdateClass  QuoteOPTemplateTextDesignUpdateClass = new QuoteOPTemplateTextDesignUpdateClass();
		QuoteOPTotalVATUpdateClass  QuoteOPTotalVATUpdateClass = new QuoteOPTotalVATUpdateClass();
		QuoteOPTypeUpdateClass  QuoteOPTypeUpdateClass = new QuoteOPTypeUpdateClass();
		QuoteOPVATsTotalUpdateClass  QuoteOPVATsTotalUpdateClass = new QuoteOPVATsTotalUpdateClass();
	
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
 			TablesHashStrings.Add("BorderOPType",  BorderOPTypeUpdateClass.HashString);
			TablesHashStrings.Add("MarkUpOPType",  MarkUpOPTypeUpdateClass.HashString);
			TablesHashStrings.Add("OPSpecialServicesType",  OPSpecialServicesTypeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOP",  QuoteOPUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPCharge",  QuoteOPChargeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPClosingReason",  QuoteOPClosingReasonUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPComputedField",  QuoteOPComputedFieldUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPCostCharge",  QuoteOPCostChargeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPCustomerType",  QuoteOPCustomerTypeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPPackage",  QuoteOPPackageUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPPriceSteps",  QuoteOPPriceStepsUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPRating",  QuoteOPRatingUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPSaleCharge",  QuoteOPSaleChargeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPSalesTotal",  QuoteOPSalesTotalUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPSetting",  QuoteOPSettingUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPStage",  QuoteOPStageUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplate",  QuoteOPTemplateUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateSection",  QuoteOPTemplateSectionUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateSectionType",  QuoteOPTemplateSectionTypeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateSetting",  QuoteOPTemplateSettingUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateTableDesign",  QuoteOPTemplateTableDesignUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateTextCode",  QuoteOPTemplateTextCodeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTemplateTextDesign",  QuoteOPTemplateTextDesignUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPTotalVAT",  QuoteOPTotalVATUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPType",  QuoteOPTypeUpdateClass.HashString);
			TablesHashStrings.Add("QuoteOPVATsTotal",  QuoteOPVATsTotalUpdateClass.HashString);
			return TablesHashStrings;
        }
        public void CreateAllObjectTablesMetadata()
        {
   
			if(MetadataUpdateUtility.IsChangedMetadataTable("BorderOPType", ObjectTables, BorderOPTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("BorderOPType");
					BorderOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					BorderOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					BorderOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("MarkUpOPType", ObjectTables, MarkUpOPTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("MarkUpOPType");
					MarkUpOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					MarkUpOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					MarkUpOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				MarkUpOPTypeUpdateClass.FillMarkUpOPType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("OPSpecialServicesType", ObjectTables, OPSpecialServicesTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("OPSpecialServicesType");
					OPSpecialServicesTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					OPSpecialServicesTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					OPSpecialServicesTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOP", ObjectTables, QuoteOPUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOP");
					QuoteOPUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPCharge", ObjectTables, QuoteOPChargeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPCharge");
					QuoteOPChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPClosingReason", ObjectTables, QuoteOPClosingReasonUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPClosingReason");
					QuoteOPClosingReasonUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPClosingReasonUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPClosingReasonUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPComputedField", ObjectTables, QuoteOPComputedFieldUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPComputedField");
					QuoteOPComputedFieldUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPComputedFieldUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPComputedFieldUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPCostCharge", ObjectTables, QuoteOPCostChargeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPCostCharge");
					QuoteOPCostChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPCostChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCostChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPCustomerType", ObjectTables, QuoteOPCustomerTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPCustomerType");
					QuoteOPCustomerTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPCustomerTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPCustomerTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				QuoteOPCustomerTypeUpdateClass.FillQuoteOPCustomerType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPPackage", ObjectTables, QuoteOPPackageUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPPackage");
					QuoteOPPackageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPPackageUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPackageUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPPriceSteps", ObjectTables, QuoteOPPriceStepsUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPPriceSteps");
					QuoteOPPriceStepsUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPPriceStepsUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPPriceStepsUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPRating", ObjectTables, QuoteOPRatingUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPRating");
					QuoteOPRatingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPRatingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPRatingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				QuoteOPRatingUpdateClass.FillQuoteOPRating();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPSaleCharge", ObjectTables, QuoteOPSaleChargeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPSaleCharge");
					QuoteOPSaleChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPSaleChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSaleChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPSalesTotal", ObjectTables, QuoteOPSalesTotalUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPSalesTotal");
					QuoteOPSalesTotalUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPSalesTotalUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSalesTotalUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPSetting", ObjectTables, QuoteOPSettingUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPSetting");
					QuoteOPSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPStage", ObjectTables, QuoteOPStageUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPStage");
					QuoteOPStageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPStageUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPStageUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplate", ObjectTables, QuoteOPTemplateUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplate");
					QuoteOPTemplateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateSection", ObjectTables, QuoteOPTemplateSectionUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateSection");
					QuoteOPTemplateSectionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateSectionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateSectionType", ObjectTables, QuoteOPTemplateSectionTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateSectionType");
					QuoteOPTemplateSectionTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateSectionTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSectionTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateSetting", ObjectTables, QuoteOPTemplateSettingUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateSetting");
					QuoteOPTemplateSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateTableDesign", ObjectTables, QuoteOPTemplateTableDesignUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateTableDesign");
					QuoteOPTemplateTableDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateTableDesignUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTableDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateTextCode", ObjectTables, QuoteOPTemplateTextCodeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateTextCode");
					QuoteOPTemplateTextCodeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateTextCodeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextCodeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTemplateTextDesign", ObjectTables, QuoteOPTemplateTextDesignUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTemplateTextDesign");
					QuoteOPTemplateTextDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTemplateTextDesignUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTemplateTextDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPTotalVAT", ObjectTables, QuoteOPTotalVATUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPTotalVAT");
					QuoteOPTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTotalVATUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPType", ObjectTables, QuoteOPTypeUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPType");
					QuoteOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
 
				QuoteOPTypeUpdateClass.FillQuoteOPType();

 
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("QuoteOPVATsTotal", ObjectTables, QuoteOPVATsTotalUpdateClass.HashString))
			{
				using (TransactionScope scope = TransactionFactory.GetNewTransaction())
				{				
					MetadataUpdateUtility.DeleteAllTableMetadata("QuoteOPVATsTotal");
					QuoteOPVATsTotalUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
					this.ObjectContext.SaveChanges();
					List<ObjectField> addedFields = new List<ObjectField>();
					List<TextCode> addedTextCodes = new List<TextCode>();
					QuoteOPVATsTotalUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
					SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
					SqlBulkInsert.BulkInsert("ObjectFields", addedFields);					
					//this.ObjectContext.TextCodes.AddRange(addedTextCodes);
					//this.ObjectContext.ObjectFields.AddRange(addedFields);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters,tenantQueryGroups);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
					//this.ObjectContext.SaveChanges();
					QuoteOPVATsTotalUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
					this.ObjectContext.SaveChanges();
					scope.Complete();
				}
			}

        }
   

        public void CreateAllObjectTables()
        {
   
	   	   BorderOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   MarkUpOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   OPSpecialServicesTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPClosingReasonUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPComputedFieldUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPCostChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPPackageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPPriceStepsUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPRatingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPSaleChargeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPSalesTotalUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPStageUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   QuoteOPVATsTotalUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   //BorderOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //MarkUpOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //OPSpecialServicesTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPClosingReasonUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPComputedFieldUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPCostChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPCustomerTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPPackageUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPPriceStepsUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPRatingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPSaleChargeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPSalesTotalUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPStageUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateSectionUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateSectionTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateSettingUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateTableDesignUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateTextCodeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTemplateTextDesignUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTotalVATUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   //QuoteOPVATsTotalUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   MarkUpOPTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPChargeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPPackageUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPRatingUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPSettingUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPStageUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPTypeUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableQueries(Queries,QueryColumns, ObjectTables, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters,tenantQueryGroups);
	
        }

		public void CreateAllScreens()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   MarkUpOPTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPPackageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPRatingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPStageUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   MarkUpOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPPackageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPRatingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPStageUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   MarkUpOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPPackageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPRatingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPStageUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MarkUpOPTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPPackageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPRatingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPStageUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   MarkUpOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPPackageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPRatingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPStageUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   BorderOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   MarkUpOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   OPSpecialServicesTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPClosingReasonUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPComputedFieldUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPCostChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPCustomerTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPPackageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPPriceStepsUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPRatingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPSaleChargeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPSalesTotalUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPStageUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateSectionUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateSectionTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateTableDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateTextCodeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTemplateTextDesignUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   QuoteOPVATsTotalUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   	   MarkUpOPTypeUpdateClass.FillMarkUpOPType();
	
	   
	   
	   
	   
	   
	   
	   	   QuoteOPCustomerTypeUpdateClass.FillQuoteOPCustomerType();
	
	   
	   
	   	   QuoteOPRatingUpdateClass.FillQuoteOPRating();
	
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   
	   	   QuoteOPTypeUpdateClass.FillQuoteOPType();
	
	   
        }
 	 
	 

   }

}
