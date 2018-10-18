using Logitude.CRM.Data;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.BL.CommonDataModel.EntityLists;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class WarehouseUpdate
    {
        private bool isUpdate;
        private IWebFreightContext objectContext;
        private ObjectTablePM warehouseEntryObjectTable;
        private ObjectTablePM warehouseReleaseObjectTable;


        #region Repositories
        private ObjectTableRepository objectTableRepository;
        private TextCodeRepository textCodeRepository;
        private ScreensRepository screensRepository;
        private ScreenFieldsRepository screenFieldsRepository;

        private QueryRepository queriesRepository;
        private QueryColumnRepository queryColumnsRepository;
        private AdvancedQueryFilterRepository advancedQueryFiltersRepository;

        private ObjectTableTabRepository objectTableTabsRepository;
        private ObjectTableHelperControlRepository objectTableHelperControlsRepository;

        private MenusTableRepository menusTablesRepository;
        private MenuButtonRepository menuButtonRepository;
        private MenuButtonGroupRepository menuButtonGroupRepository;

        private ObjectFieldValidationRepository objectFieldValidationRepository;
        private QueryGroupRepository queryGroupRepository;
        private ObjectTableRuleRepository objectTableRuleRepository;
        private ObjectTableRuleFieldRepository objectTableRuleFieldRepository;
        private RuleConditionFieldRepository ruleConditionFieldRepository;

        private EventTypeRepository eventTypesRepository;
        #endregion

        #region Queries
        private ObjectTableQuery objectTabelQuery;
        private MenuButtonGroupQuery menuButtonGroupQuery;
        #endregion


        #region Features
        public void LoadRolesAndFeatures(int tenant)
        {
            ICommonDataContext ObjectContext = CommonDataContext.GetContext(tenant);
            FeatureRepository FeaturesRepository = new FeatureRepository(ObjectContext);
            RoleFeatureRepository RoleFeaturesRepository = new RoleFeatureRepository(ObjectContext);
            TextCodeRepository textCodeRep = new TextCodeRepository(tenant);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            objectTabelQuery = new ObjectTableQuery(objecttableRep);

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(tenant).ToList();

            #region ObjectTables
            warehouseEntryObjectTable = objectTables.Where(d => d.Name == "WarehouseEntry").FirstOrDefault();
            warehouseReleaseObjectTable = objectTables.Where(d => d.Name == "WarehouseRelease").FirstOrDefault();
            ObjectTablePM GeneralObjectTable = objectTables.Where(d => d.Name == "General").FirstOrDefault();

            #endregion

            Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
            List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

            Feature WarehouseReasonFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WAREHOUSEMANAGEMENT", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.WarehouseManagement", NameTextCodeDefaultText = "Warehouse Management", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
        }
        #endregion

        public void LoadMenustables()
        {

        }

        public void LoadObjectTableHelperControls()
        {

        }
        public void LoadObjectTableTabs()
        {

        }
        public void loadQueries()
        {

        }

        public void CreateTableCounters()
        {
            objectContext = WebFreightContext.GetContext(0);
            CounterRepository counterRepository = new CounterRepository(objectContext);
            CounterDefinitionRepository counterDefinitionRepository = new CounterDefinitionRepository(objectContext);
            List<Counter> counters = counterRepository.All().ToList();

            TenantQuery tenantQuery = new TenantQuery(0);
            List<TenantList> tenantList = tenantQuery.GetTenantLists();

            foreach (TenantList tenant in tenantList)
            {
                #region Warehouse Entries Counters

                if (!counters.Where(c => c.Code == "WAEC" && c.Tenant == tenant.Id).Any())
                {
                    if (warehouseEntryObjectTable != null)
                    {
                        Counter warehouseEntriesCounter = new Counter()
                        {
                            Id = IdCounter.GetNumber("Counter", tenant.Id).ToString(),
                            ObjectTableId = warehouseEntryObjectTable.Id,
                            Code = "WAEC",
                            Tenant = tenant.Id,
                            Name = "Warehouse Entries",

                        };

                        CounterDefinition warehouseEntry_Counter = new CounterDefinition()
                        {
                            Id = IdCounter.GetNumber("CounterDefinition", tenant.Id).ToString(),
                            CounterId = warehouseEntriesCounter.Id,
                            Tenant = tenant.Id,
                            StartNumber = 1000,

                        };

                        counterRepository.Add(warehouseEntriesCounter);
                        counterDefinitionRepository.Add(warehouseEntry_Counter);
                    }

                }

                #endregion

                #region Warehouse Entries Counters


                if (!counters.Where(c => c.Code == "WARC" && c.Tenant == tenant.Id).Any())
                {
                    if (warehouseReleaseObjectTable != null)
                    {
                        Counter warehouseReleaseCounter = new Counter()
                        {
                            Id = IdCounter.GetNumber("Counter", tenant.Id).ToString(),
                            ObjectTableId = warehouseReleaseObjectTable.Id,
                            Code = "WARC",
                            Tenant = tenant.Id,
                            Name = "Warehouse Releases",

                        };

                        CounterDefinition warehouseRelease_Counter = new CounterDefinition()
                        {
                            Id = IdCounter.GetNumber("CounterDefinition", tenant.Id).ToString(),
                            CounterId = warehouseReleaseCounter.Id,
                            Tenant = tenant.Id,
                            StartNumber = 1000,

                        };

                        counterRepository.Add(warehouseReleaseCounter);
                        counterDefinitionRepository.Add(warehouseRelease_Counter);
                    }

                }

                #endregion

            }

            objectContext.SaveChanges();
        }

        public void LoadOtherFields(IWebFreightContext context)
        {
            objectContext = context;
            textCodeRepository = new TextCodeRepository(objectContext);

            Dictionary<string, TextCode> textcodes = textCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);
            string objectTableId = warehouseEntryObjectTable.Id;

            /*Partners*/
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.B.Partners.AddShipper", DefaultText = "Add Shipper", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "B", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.B.Partners.AddConsignee", DefaultText = "Add Consignee", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "B", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.B.Partners.AddPartners", DefaultText = "Add Partners", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "B", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.AddShipper", DefaultText = "Add Shipper", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.AddConsignee", DefaultText = "Add Consignee", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.Name", DefaultText = "Name", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.Address", DefaultText = "Address", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.Reference1", DefaultText = "Reference1", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.Reference2", DefaultText = "Reference2", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.MyCustomer", DefaultText = "My Customer", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.EditShipper", DefaultText = "Edit Shipper", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.EditConsignee", DefaultText = "Edit Consignee", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.S.Partners.Partners", DefaultText = "Partners", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "WarehouseEntry.M.DeleteThisPartner", DefaultText = "Delete This Partner?", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);

            

            objectContext.SaveChanges();
        }


    }
}