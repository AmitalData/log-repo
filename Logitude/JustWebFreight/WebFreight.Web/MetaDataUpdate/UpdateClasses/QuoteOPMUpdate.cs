
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
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.ShipmentsModel.EntityUpdateClasses;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class QuoteOPMUpdate
    {


		public Dictionary<string, ObjectTable> ObjectTables { get; set; }
		public Dictionary<string, TextCode> TextCodes { get; set; }
		public ObjectTableRepository ObjectTableRepository { get; set; }
		public TextCodeRepository TextCodeRepository { get; set; }
		public Dictionary<string, ObjectField> ObjectFields { get; set; }
		public ObjectFieldRepository ObjectFieldsRepository { get; set; }
		public IWebFreightContext ObjectContext { get; set; }
		public ICommonDataContext CommonContext { get; set; }
		public Dictionary<string, Query> Queries { get; set; }
		public Dictionary<string, QueryColumn> QueryColumns { get; set; }
		public QueryRepository queriesRepository { get; set; }
		public QueryColumnRepository queryColumnsRepository { get; set; }
		public FeatureRepository FeaturesRepository { get; set; }// = new FeatureRepository(0);
		public Dictionary<string, Feature> TenantFeatures { get; set; }
		public QueryGroupRepository queryGroupRepository { get; set; }
		public AdvancedQueryFilterRepository advancedQueryFiltersRepository { get; set; }
		public Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters { get; set; }
		public ScreensRepository screensRepository { get; set; }
		public Dictionary<string, Screen> tenantScreens { get; set; }
		public ScreenFieldsRepository screenFieldsRepository { get; set; }
		public Dictionary<string, ScreenField> tenantScreenFields { get; set; }
		public Dictionary<string, ObjectTableTab> TenantObjectTableTabs { get; set; }
		public ObjectTableTabRepository objectTableTabsRepository { get; set; }
		public Dictionary<string, EventType> tenantEventTypes { get; set; }
		public EventTypeRepository EventTypeRepository { get; set; }
		public MenuButtonRepository menuButtonRepository { get; set; }
		public MenuButtonGroupRepository menuButtonGroupRepository { get; set; }
		public Dictionary<string, MenuButton> tenantMenuButtons { get; set; }
		public Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups { get; set; }
		public EntityStatusRepository EntityStatusRepository { get; set; }
		public List<EntityStatus> AllEntityStatuses { get; set; }
		Dictionary<string, QueryGroup> tenantQueryGroups { get; set; }

		SpecialServicesTypeUpdateClass SpecialServicesTypeUpdateClass = new SpecialServicesTypeUpdateClass();
		public void LoadObjectsTenantZero(IWebFreightContext context)
		{
			ObjectContext = context;
			TextCodeRepository = new TextCodeRepository(ObjectContext);
			Dictionary<string, TextCode> textcodes = TextCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);
			ObjectTable GeneralTable = ObjectContext.ObjectTables.Where(f => f.Name == "General" && f.Tenant == 0).FirstOrDefault();
			AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.QuotesOP", DefaultText = "Quotes", LocalDefaultText = "הצעות מחיר", ObjectTableId = GeneralTable.Id, Tenant = 0, TextCodeTypeCode = "MH", }, TextCodeRepository, textcodes);
			return;// quit SpecialServicesType
			ICommonDataContext commonContext = CommonDataContext.GetContext(0);
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
			tenantQueryGroups = queryGroupRepository.GetQueryGroups().ToDictionary(d => d.Code, a => a);
			ManulayUpdate();
		}

        void ManulayUpdate()
        {
            if (MetadataUpdateUtility.IsChangedMetadataTable("SpecialServicesType", ObjectTables, SpecialServicesTypeUpdateClass.HashString))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    MetadataUpdateUtility.DeleteAllTableMetadata("SpecialServicesType");
                    SpecialServicesTypeUpdateClass.AddObjectTable(ObjectTables, TextCodes, ObjectTableRepository, TextCodeRepository);
                    this.ObjectContext.SaveChanges();
                    List<ObjectField> addedFields = new List<ObjectField>();
                    List<TextCode> addedTextCodes = new List<TextCode>();
                    SpecialServicesTypeUpdateClass.AddObjectFields(ObjectFields, ObjectTables, TextCodes, ObjectFieldsRepository, TextCodeRepository, addedFields, addedTextCodes);
                    SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
                    SqlBulkInsert.BulkInsert("ObjectFields", addedFields);
                    //this.ObjectContext.TextCodes.AddRange(addedTextCodes);
                    //this.ObjectContext.ObjectFields.AddRange(addedFields);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableQueries(Queries, QueryColumns, ObjectTables, TextCodes, queryGroupRepository, queriesRepository, queryColumnsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, advancedQueryFiltersRepository, tenantAdvancedFilters, tenantQueryGroups);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableScreens(tenantScreens, tenantScreenFields, screensRepository, screenFieldsRepository, ObjectContext);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableTabs(TenantObjectTableTabs, TextCodes, objectTableTabsRepository, TextCodeRepository, FeaturesRepository, TenantFeatures, ObjectContext);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableEventTypes(tenantEventTypes, EventTypeRepository, ObjectContext, AllEntityStatuses);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableFeatures(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableTextCodes(TextCodeRepository, FeaturesRepository, TenantFeatures, TextCodes, ObjectContext);
                    //this.ObjectContext.SaveChanges();
                    SpecialServicesTypeUpdateClass.AddTableMenuButtons(tenantMenuButtons, tenantMenuButtonGroups, TextCodes, TextCodeRepository, FeaturesRepository, menuButtonRepository, TenantFeatures, menuButtonGroupRepository, ObjectContext);
                    this.ObjectContext.SaveChanges();
                    scope.Complete();
                }
            }
        }
    }
	}