
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
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel.EntityUpdateClasses;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel
{
   public class InvoiceModelUpdateClass
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
		 
	   	AccountUpdateClass  AccountUpdateClass = new AccountUpdateClass();
		AccountingPaymentMethodUpdateClass  AccountingPaymentMethodUpdateClass = new AccountingPaymentMethodUpdateClass();
		AccountingSystemsSettingUpdateClass  AccountingSystemsSettingUpdateClass = new AccountingSystemsSettingUpdateClass();
		AccountingSystemsSyncStatusUpdateClass  AccountingSystemsSyncStatusUpdateClass = new AccountingSystemsSyncStatusUpdateClass();
		AccountingTransferHeaderUpdateClass  AccountingTransferHeaderUpdateClass = new AccountingTransferHeaderUpdateClass();
		AccountingTransferLineUpdateClass  AccountingTransferLineUpdateClass = new AccountingTransferLineUpdateClass();
		AccountingTransferTypeUpdateClass  AccountingTransferTypeUpdateClass = new AccountingTransferTypeUpdateClass();
		AccountTypeUpdateClass  AccountTypeUpdateClass = new AccountTypeUpdateClass();
		APInvoiceUpdateClass  APInvoiceUpdateClass = new APInvoiceUpdateClass();
		APInvoiceLineUpdateClass  APInvoiceLineUpdateClass = new APInvoiceLineUpdateClass();
		APInvoicePaymentUpdateClass  APInvoicePaymentUpdateClass = new APInvoicePaymentUpdateClass();
		APInvoiceStatusUpdateClass  APInvoiceStatusUpdateClass = new APInvoiceStatusUpdateClass();
		APInvoiceTotalVATUpdateClass  APInvoiceTotalVATUpdateClass = new APInvoiceTotalVATUpdateClass();
		APInvoiceTransferStatusUpdateClass  APInvoiceTransferStatusUpdateClass = new APInvoiceTransferStatusUpdateClass();
		APInvoiceTypeUpdateClass  APInvoiceTypeUpdateClass = new APInvoiceTypeUpdateClass();
		APPaymentUpdateClass  APPaymentUpdateClass = new APPaymentUpdateClass();
		APPaymentMethodUpdateClass  APPaymentMethodUpdateClass = new APPaymentMethodUpdateClass();
		APPaymentStatusUpdateClass  APPaymentStatusUpdateClass = new APPaymentStatusUpdateClass();
		APPaymentTransferStatusUpdateClass  APPaymentTransferStatusUpdateClass = new APPaymentTransferStatusUpdateClass();
		ARInvoiceUpdateClass  ARInvoiceUpdateClass = new ARInvoiceUpdateClass();
		ARInvoiceLineUpdateClass  ARInvoiceLineUpdateClass = new ARInvoiceLineUpdateClass();
		ARInvoicePaymentUpdateClass  ARInvoicePaymentUpdateClass = new ARInvoicePaymentUpdateClass();
		ARInvoiceStatusUpdateClass  ARInvoiceStatusUpdateClass = new ARInvoiceStatusUpdateClass();
		ARInvoiceStockUpdateClass  ARInvoiceStockUpdateClass = new ARInvoiceStockUpdateClass();
		ARInvoiceStockLineUpdateClass  ARInvoiceStockLineUpdateClass = new ARInvoiceStockLineUpdateClass();
		ARInvoiceStocksStatusUpdateClass  ARInvoiceStocksStatusUpdateClass = new ARInvoiceStocksStatusUpdateClass();
		ARInvoiceTotalVATUpdateClass  ARInvoiceTotalVATUpdateClass = new ARInvoiceTotalVATUpdateClass();
		ARInvoiceTransferStatusUpdateClass  ARInvoiceTransferStatusUpdateClass = new ARInvoiceTransferStatusUpdateClass();
		ARInvoiceTypeUpdateClass  ARInvoiceTypeUpdateClass = new ARInvoiceTypeUpdateClass();
		ARPaymentUpdateClass  ARPaymentUpdateClass = new ARPaymentUpdateClass();
		ARPaymentChequeReplicaUpdateClass  ARPaymentChequeReplicaUpdateClass = new ARPaymentChequeReplicaUpdateClass();
		ARPaymentStatusUpdateClass  ARPaymentStatusUpdateClass = new ARPaymentStatusUpdateClass();
		ARPaymentTransferStatusUpdateClass  ARPaymentTransferStatusUpdateClass = new ARPaymentTransferStatusUpdateClass();
		BankAccountLiteUpdateClass  BankAccountLiteUpdateClass = new BankAccountLiteUpdateClass();
		CreditCardTypeUpdateClass  CreditCardTypeUpdateClass = new CreditCardTypeUpdateClass();
		ExternalSystemsTablesCodeUpdateClass  ExternalSystemsTablesCodeUpdateClass = new ExternalSystemsTablesCodeUpdateClass();
		SATInterfaceUpdateClass  SATInterfaceUpdateClass = new SATInterfaceUpdateClass();
		SATInterfaceSettingUpdateClass  SATInterfaceSettingUpdateClass = new SATInterfaceSettingUpdateClass();
		SATInvoiceStatusUpdateClass  SATInvoiceStatusUpdateClass = new SATInvoiceStatusUpdateClass();
		SATPaymentMethodUpdateClass  SATPaymentMethodUpdateClass = new SATPaymentMethodUpdateClass();
		SATTransferStatusUpdateClass  SATTransferStatusUpdateClass = new SATTransferStatusUpdateClass();
	
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
 			TablesHashStrings.Add("Account",  AccountUpdateClass.HashString);
			TablesHashStrings.Add("AccountingPaymentMethod",  AccountingPaymentMethodUpdateClass.HashString);
			TablesHashStrings.Add("AccountingSystemsSetting",  AccountingSystemsSettingUpdateClass.HashString);
			TablesHashStrings.Add("AccountingSystemsSyncStatus",  AccountingSystemsSyncStatusUpdateClass.HashString);
			TablesHashStrings.Add("AccountingTransferHeader",  AccountingTransferHeaderUpdateClass.HashString);
			TablesHashStrings.Add("AccountingTransferLine",  AccountingTransferLineUpdateClass.HashString);
			TablesHashStrings.Add("AccountingTransferType",  AccountingTransferTypeUpdateClass.HashString);
			TablesHashStrings.Add("AccountType",  AccountTypeUpdateClass.HashString);
			TablesHashStrings.Add("APInvoice",  APInvoiceUpdateClass.HashString);
			TablesHashStrings.Add("APInvoiceLine",  APInvoiceLineUpdateClass.HashString);
			TablesHashStrings.Add("APInvoicePayment",  APInvoicePaymentUpdateClass.HashString);
			TablesHashStrings.Add("APInvoiceStatus",  APInvoiceStatusUpdateClass.HashString);
			TablesHashStrings.Add("APInvoiceTotalVAT",  APInvoiceTotalVATUpdateClass.HashString);
			TablesHashStrings.Add("APInvoiceTransferStatus",  APInvoiceTransferStatusUpdateClass.HashString);
			TablesHashStrings.Add("APInvoiceType",  APInvoiceTypeUpdateClass.HashString);
			TablesHashStrings.Add("APPayment",  APPaymentUpdateClass.HashString);
			TablesHashStrings.Add("APPaymentMethod",  APPaymentMethodUpdateClass.HashString);
			TablesHashStrings.Add("APPaymentStatus",  APPaymentStatusUpdateClass.HashString);
			TablesHashStrings.Add("APPaymentTransferStatus",  APPaymentTransferStatusUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoice",  ARInvoiceUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceLine",  ARInvoiceLineUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoicePayment",  ARInvoicePaymentUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceStatus",  ARInvoiceStatusUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceStock",  ARInvoiceStockUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceStockLine",  ARInvoiceStockLineUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceStocksStatus",  ARInvoiceStocksStatusUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceTotalVAT",  ARInvoiceTotalVATUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceTransferStatus",  ARInvoiceTransferStatusUpdateClass.HashString);
			TablesHashStrings.Add("ARInvoiceType",  ARInvoiceTypeUpdateClass.HashString);
			TablesHashStrings.Add("ARPayment",  ARPaymentUpdateClass.HashString);
			TablesHashStrings.Add("ARPaymentChequeReplica",  ARPaymentChequeReplicaUpdateClass.HashString);
			TablesHashStrings.Add("ARPaymentStatus",  ARPaymentStatusUpdateClass.HashString);
			TablesHashStrings.Add("ARPaymentTransferStatus",  ARPaymentTransferStatusUpdateClass.HashString);
			TablesHashStrings.Add("BankAccountLite",  BankAccountLiteUpdateClass.HashString);
			TablesHashStrings.Add("CreditCardType",  CreditCardTypeUpdateClass.HashString);
			TablesHashStrings.Add("ExternalSystemsTablesCode",  ExternalSystemsTablesCodeUpdateClass.HashString);
			TablesHashStrings.Add("SATInterface",  SATInterfaceUpdateClass.HashString);
			TablesHashStrings.Add("SATInterfaceSetting",  SATInterfaceSettingUpdateClass.HashString);
			TablesHashStrings.Add("SATInvoiceStatus",  SATInvoiceStatusUpdateClass.HashString);
			TablesHashStrings.Add("SATPaymentMethod",  SATPaymentMethodUpdateClass.HashString);
			TablesHashStrings.Add("SATTransferStatus",  SATTransferStatusUpdateClass.HashString);
			return TablesHashStrings;
        }
        public void CreateAllObjectTablesMetadata()
        {
   
			if(MetadataUpdateUtility.IsChangedMetadataTable("Account", ObjectTables, AccountUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("Account");
				AccountUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingPaymentMethod", ObjectTables, AccountingPaymentMethodUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingPaymentMethod");
				AccountingPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingSystemsSetting", ObjectTables, AccountingSystemsSettingUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingSystemsSetting");
				AccountingSystemsSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingSystemsSyncStatus", ObjectTables, AccountingSystemsSyncStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingSystemsSyncStatus");
				AccountingSystemsSyncStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingSystemsSyncStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingTransferHeader", ObjectTables, AccountingTransferHeaderUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingTransferHeader");
				AccountingTransferHeaderUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferHeaderUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingTransferLine", ObjectTables, AccountingTransferLineUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingTransferLine");
				AccountingTransferLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferLineUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountingTransferType", ObjectTables, AccountingTransferTypeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountingTransferType");
				AccountingTransferTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountingTransferTypeUpdateClass.FillAccountingTransferType();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("AccountType", ObjectTables, AccountTypeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("AccountType");
				AccountTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				AccountTypeUpdateClass.FillAccountType();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoice", ObjectTables, APInvoiceUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoice");
				APInvoiceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoiceLine", ObjectTables, APInvoiceLineUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoiceLine");
				APInvoiceLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceLineUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoicePayment", ObjectTables, APInvoicePaymentUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoicePayment");
				APInvoicePaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoicePaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoiceStatus", ObjectTables, APInvoiceStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoiceStatus");
				APInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceStatusUpdateClass.FillAPInvoiceStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoiceTotalVAT", ObjectTables, APInvoiceTotalVATUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoiceTotalVAT");
				APInvoiceTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoiceTransferStatus", ObjectTables, APInvoiceTransferStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoiceTransferStatus");
				APInvoiceTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTransferStatusUpdateClass.FillAPInvoiceTransferStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APInvoiceType", ObjectTables, APInvoiceTypeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APInvoiceType");
				APInvoiceTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APInvoiceTypeUpdateClass.FillAPInvoiceType();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APPayment", ObjectTables, APPaymentUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APPayment");
				APPaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APPaymentMethod", ObjectTables, APPaymentMethodUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APPaymentMethod");
				APPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APPaymentStatus", ObjectTables, APPaymentStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APPaymentStatus");
				APPaymentStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentStatusUpdateClass.FillAPPaymentStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("APPaymentTransferStatus", ObjectTables, APPaymentTransferStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("APPaymentTransferStatus");
				APPaymentTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				APPaymentTransferStatusUpdateClass.FillAPPaymentTransferStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoice", ObjectTables, ARInvoiceUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoice");
				ARInvoiceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceLine", ObjectTables, ARInvoiceLineUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceLine");
				ARInvoiceLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceLineUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoicePayment", ObjectTables, ARInvoicePaymentUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoicePayment");
				ARInvoicePaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoicePaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceStatus", ObjectTables, ARInvoiceStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceStatus");
				ARInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStatusUpdateClass.FillARInvoiceStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceStock", ObjectTables, ARInvoiceStockUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceStock");
				ARInvoiceStockUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceStockLine", ObjectTables, ARInvoiceStockLineUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceStockLine");
				ARInvoiceStockLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStockLineUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceStocksStatus", ObjectTables, ARInvoiceStocksStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceStocksStatus");
				ARInvoiceStocksStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceStocksStatusUpdateClass.FillARInvoiceStocksStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceTotalVAT", ObjectTables, ARInvoiceTotalVATUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceTotalVAT");
				ARInvoiceTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceTransferStatus", ObjectTables, ARInvoiceTransferStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceTransferStatus");
				ARInvoiceTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTransferStatusUpdateClass.FillARInvoiceTransferStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARInvoiceType", ObjectTables, ARInvoiceTypeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARInvoiceType");
				ARInvoiceTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARInvoiceTypeUpdateClass.FillARInvoiceType();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARPayment", ObjectTables, ARPaymentUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARPayment");
				ARPaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARPaymentChequeReplica", ObjectTables, ARPaymentChequeReplicaUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARPaymentChequeReplica");
				ARPaymentChequeReplicaUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentChequeReplicaUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARPaymentStatus", ObjectTables, ARPaymentStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARPaymentStatus");
				ARPaymentStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentStatusUpdateClass.FillARPaymentStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ARPaymentTransferStatus", ObjectTables, ARPaymentTransferStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ARPaymentTransferStatus");
				ARPaymentTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ARPaymentTransferStatusUpdateClass.FillARPaymentTransferStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("BankAccountLite", ObjectTables, BankAccountLiteUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("BankAccountLite");
				BankAccountLiteUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				BankAccountLiteUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("CreditCardType", ObjectTables, CreditCardTypeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("CreditCardType");
				CreditCardTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				CreditCardTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("ExternalSystemsTablesCode", ObjectTables, ExternalSystemsTablesCodeUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("ExternalSystemsTablesCode");
				ExternalSystemsTablesCodeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				ExternalSystemsTablesCodeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("SATInterface", ObjectTables, SATInterfaceUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("SATInterface");
				SATInterfaceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceUpdateClass.FillSATInterface();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("SATInterfaceSetting", ObjectTables, SATInterfaceSettingUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("SATInterfaceSetting");
				SATInterfaceSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInterfaceSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("SATInvoiceStatus", ObjectTables, SATInvoiceStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("SATInvoiceStatus");
				SATInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATInvoiceStatusUpdateClass.FillSATInvoiceStatus();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("SATPaymentMethod", ObjectTables, SATPaymentMethodUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("SATPaymentMethod");
				SATPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATPaymentMethodUpdateClass.FillSATPaymentMethod();
			}

			if(MetadataUpdateUtility.IsChangedMetadataTable("SATTransferStatus", ObjectTables, SATTransferStatusUpdateClass.HashString))
			{
				MetadataUpdateUtility.DeleteAllTableMetadata("SATTransferStatus");
				SATTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository, TextCodeRepository);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableQueries(Queries, QueryColumns, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
				this.ObjectContext.SaveChanges();
				SATTransferStatusUpdateClass.FillSATTransferStatus();
			}

        }
   

        public void CreateAllObjectTables()
        {
   
	   	   AccountUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingSystemsSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingTransferHeaderUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingTransferLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountingTransferTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   AccountTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoicePaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APInvoiceTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APPaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APPaymentStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   APPaymentTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoicePaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceStockUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceStockLineUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceTotalVATUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARInvoiceTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARPaymentUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARPaymentStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ARPaymentTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   BankAccountLiteUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   CreditCardTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SATInterfaceUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SATInterfaceSettingUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SATInvoiceStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SATPaymentMethodUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
	   	   SATTransferStatusUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository,TextCodeRepository);
	
        }
   

        public void CreateAllObjectFields()
        {
   
	   	   AccountUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingSystemsSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingTransferHeaderUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingTransferLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountingTransferTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   AccountTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoicePaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceTotalVATUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APInvoiceTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APPaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APPaymentStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   APPaymentTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoicePaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceStockUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceStockLineUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceTotalVATUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARInvoiceTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARPaymentUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARPaymentStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ARPaymentTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   BankAccountLiteUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   CreditCardTypeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SATInterfaceUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SATInterfaceSettingUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SATInvoiceStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SATPaymentMethodUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
	   	   SATTransferStatusUpdateClass.AddObjectFields(ObjectFields, TextCodes, ObjectFieldsRepository,TextCodeRepository);
	
        }

		public void CreateAllQueries()
        {
   
	   	   AccountUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingTransferLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountingTransferTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   AccountTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoicePaymentUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APInvoiceTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APPaymentUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APPaymentMethodUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APPaymentStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoicePaymentUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceStockUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARInvoiceTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARPaymentUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARPaymentStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   BankAccountLiteUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   CreditCardTypeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SATInterfaceUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SATInterfaceSettingUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SATInvoiceStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SATPaymentMethodUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
	   	   SATTransferStatusUpdateClass.AddTableQueries(Queries,QueryColumns, TextCodes,queryGroupRepository,queriesRepository,queryColumnsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,advancedQueryFiltersRepository,tenantAdvancedFilters);
	
        }

		public void CreateAllScreens()
        {
   
	   	   AccountUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingTransferLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountingTransferTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   AccountTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoicePaymentUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APInvoiceTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APPaymentUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APPaymentMethodUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APPaymentStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoicePaymentUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceStockUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARInvoiceTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARPaymentUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARPaymentStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   BankAccountLiteUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   CreditCardTypeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SATInterfaceUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SATInterfaceSettingUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SATInvoiceStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SATPaymentMethodUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
	   	   SATTransferStatusUpdateClass.AddTableScreens(tenantScreens,tenantScreenFields,screensRepository,screenFieldsRepository,ObjectContext);
	
        }

		public void CreateAllTabs()
        {
   
	   	   AccountUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingTransferLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountingTransferTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   AccountTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoicePaymentUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APInvoiceTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APPaymentUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APPaymentStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoicePaymentUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceStockUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARInvoiceTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARPaymentUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARPaymentStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   BankAccountLiteUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   CreditCardTypeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SATInterfaceUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SATInterfaceSettingUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SATInvoiceStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SATPaymentMethodUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
	   	   SATTransferStatusUpdateClass.AddTableTabs(TenantObjectTableTabs,TextCodes,objectTableTabsRepository,TextCodeRepository,FeaturesRepository,TenantFeatures,ObjectContext);
	
        }

		public void CreateAllEventTypes()
        {
   
	   	   AccountUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingTransferLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountingTransferTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   AccountTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoicePaymentUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APInvoiceTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APPaymentUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APPaymentStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoicePaymentUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceStockUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARInvoiceTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARPaymentUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARPaymentStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   BankAccountLiteUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   CreditCardTypeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SATInterfaceUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SATInterfaceSettingUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SATInvoiceStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SATPaymentMethodUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
	   	   SATTransferStatusUpdateClass.AddTableEventTypes(tenantEventTypes,EventTypeRepository,ObjectContext,AllEntityStatuses);
	
        }

		public void CreateAllFeatures()
        {
   
	   	   AccountUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoicePaymentUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoicePaymentUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStockUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   BankAccountLiteUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CreditCardTypeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInterfaceUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInterfaceSettingUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInvoiceStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATPaymentMethodUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATTransferStatusUpdateClass.AddTableFeatures(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAdditionalTextCodes()
        {
   
	   	   AccountUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountingTransferTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   AccountTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoicePaymentUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APInvoiceTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoicePaymentUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStockUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARInvoiceTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   BankAccountLiteUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   CreditCardTypeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInterfaceUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInterfaceSettingUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATInvoiceStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATPaymentMethodUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
	   	   SATTransferStatusUpdateClass.AddTableTextCodes(TextCodeRepository,FeaturesRepository,TenantFeatures,TextCodes,ObjectContext);
	
        }
		public void CreateAllMenuButtons()
        {
   
	   	   AccountUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingSystemsSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingSystemsSyncStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingTransferHeaderUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingTransferLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountingTransferTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   AccountTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoicePaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APInvoiceTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APPaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APPaymentStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   APPaymentTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoicePaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceStockUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceStockLineUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceStocksStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceTotalVATUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARInvoiceTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARPaymentUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARPaymentChequeReplicaUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARPaymentStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ARPaymentTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   BankAccountLiteUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   CreditCardTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   ExternalSystemsTablesCodeUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SATInterfaceUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SATInterfaceSettingUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SATInvoiceStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SATPaymentMethodUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
	   	   SATTransferStatusUpdateClass.AddTableMenuButtons(tenantMenuButtons,tenantMenuButtonGroups,TextCodes,TextCodeRepository,FeaturesRepository,menuButtonRepository,TenantFeatures,menuButtonGroupRepository ,ObjectContext);
	
        }

		public void CreateAllClosedTables()
        {
   
	   
	   
	   
	   
	   
	   
	   	   AccountingTransferTypeUpdateClass.FillAccountingTransferType();
	
	   	   AccountTypeUpdateClass.FillAccountType();
	
	   
	   
	   
	   	   APInvoiceStatusUpdateClass.FillAPInvoiceStatus();
	
	   
	   	   APInvoiceTransferStatusUpdateClass.FillAPInvoiceTransferStatus();
	
	   	   APInvoiceTypeUpdateClass.FillAPInvoiceType();
	
	   
	   
	   	   APPaymentStatusUpdateClass.FillAPPaymentStatus();
	
	   	   APPaymentTransferStatusUpdateClass.FillAPPaymentTransferStatus();
	
	   
	   
	   
	   	   ARInvoiceStatusUpdateClass.FillARInvoiceStatus();
	
	   
	   
	   	   ARInvoiceStocksStatusUpdateClass.FillARInvoiceStocksStatus();
	
	   
	   	   ARInvoiceTransferStatusUpdateClass.FillARInvoiceTransferStatus();
	
	   	   ARInvoiceTypeUpdateClass.FillARInvoiceType();
	
	   
	   
	   	   ARPaymentStatusUpdateClass.FillARPaymentStatus();
	
	   	   ARPaymentTransferStatusUpdateClass.FillARPaymentTransferStatus();
	
	   
	   
	   
	   	   SATInterfaceUpdateClass.FillSATInterface();
	
	   
	   	   SATInvoiceStatusUpdateClass.FillSATInvoiceStatus();
	
	   	   SATPaymentMethodUpdateClass.FillSATPaymentMethod();
	
	   	   SATTransferStatusUpdateClass.FillSATTransferStatus();
	
        }
 	 
	 

   }

}
