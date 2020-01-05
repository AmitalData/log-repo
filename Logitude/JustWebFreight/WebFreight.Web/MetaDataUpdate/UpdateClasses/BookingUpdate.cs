using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.Repositories;
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class BookingUpdate
    {
        private bool isUpdate;
        private IWebFreightContext objectContext;

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

        #region Upgrade Closed Tables
        public void UpgradeClosedTablesForTenantZero()
        {
            isUpdate = true;

            List<GlobalDB> dbList = null;
            using (TransactionScope scop = TransactionFactory.GetNewTransaction(new TimeSpan(0, 5, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                GlobalDBRepository globalDbRep = new GlobalDBRepository();
                dbList = globalDbRep.GetGlobalDBs().ToList();
                scop.Complete();
            }

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }

        private void LoadBaseTablesForConnection(string connectionStr)
        {
            
        }
        #endregion

        #region Upgrade Objects Tenant Zero
        public void LoadUpdateTenantZero(IWebFreightContext context)
        {
            isUpdate = true;
            LoadObjectsTenantZero(context);
        }

        private void LoadObjectsTenantZero(IWebFreightContext context)
        {
            objectContext = context;

            textCodeRepository = new TextCodeRepository(objectContext);
            menuButtonRepository = new MenuButtonRepository(objectContext);
            menuButtonGroupRepository = new MenuButtonGroupRepository(objectContext);

            LoadRolesAndFeatures(0);
            //CreateMenuButtonsForTenant(0);

            this.objectContext.SaveChanges();
        }

        private void CreateAllTablesTips(Dictionary<string, Tip> tips, Dictionary<string, TextCode> textcodes)
        {

        }

        private void CreateMenuButtonsForTenant(int tenant)
        {
            menuButtonGroupQuery = new MenuButtonGroupQuery(menuButtonGroupRepository);

            if (textCodeRepository == null)
            {
                textCodeRepository = new TextCodeRepository(tenant);
            }

            Dictionary<string, TextCode> textCodes = textCodeRepository.GetTextCodesByTenant(tenant).Where(d => d.TextCodeTypeCode == "B").ToDictionary(s => s.Code, a => a);
            Dictionary<string, MenuButton> tenantMenuButtons = menuButtonRepository.GetMenuButtonsByTenant(tenant).ToDictionary(d => d.EventCode + d.MenuButtonGroupId, a => a);
            Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups = menuButtonGroupRepository.GetMenuButtonGroupsByTenant(tenant).ToDictionary(d => d.Name, a => a);

            FeatureQuery featureQuery = new FeatureQuery(tenant);
            List<FeaturePM> features = featureQuery.GetFeaturePMsByTenant(tenant).ToList();


        }

        #endregion

        #region Screens

        public void loadScreens()
        {
            objectContext = WebFreightContext.GetContext(0);
            screenFieldsRepository = new ScreenFieldsRepository(objectContext);
            screensRepository = new ScreensRepository(objectContext);
            Dictionary<string, Screen> tenantScreens = screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

            BuildBookingScreens(tenantScreens, tenantScreenField);

            LoadObjectTableRulesANDFieldsValidations();
        }

        public void BuildBookingScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Booking" && d.Tenant == 0).FirstOrDefault();
            List<ObjectField> entityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).ToList();

            objectContext.SaveChanges();
        }

        #endregion

        #region Rules

        public void LoadObjectTableRulesANDFieldsValidations()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableRuleRepository = new ObjectTableRuleRepository(objectContext);
            objectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(objectContext);
            objectFieldValidationRepository = new ObjectFieldValidationRepository(objectContext);
            ruleConditionFieldRepository = new RuleConditionFieldRepository(objectContext);

            Dictionary<string, ObjectTableRule> TenantObjectTableRule = objectTableRuleRepository.GetObjectTableRules(0).ToDictionary(d => d.RuleCode, a => a);
            Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields = objectTableRuleFieldRepository.GetObjectTableRuleFields(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldCode, a => a);
            Dictionary<string, RuleConditionField> TenantRuleConditionFields = ruleConditionFieldRepository.GetRuleConditionFieldsByTenant(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldId, a => a);
            List<ObjectFieldValidation> TenantObjectFieldValidations = objectFieldValidationRepository.GetObjectFieldValidations(0).ToList();

            CreateObjectFieldValidations(TenantObjectFieldValidations);
        }

        private void CreateObjectFieldValidations(List<ObjectFieldValidation> TenantObjectFieldValidations)
        {
            ObjectTable BookingTable = objectContext.ObjectTables.Where(f => f.Name == "Booking" && f.Tenant == 0).FirstOrDefault();
            ObjectField MasterField = objectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == BookingTable.Id).FirstOrDefault();

            #region Booking ObjectFieldValidations
            ObjectFieldValidation MasterFieldLengthValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = MasterField.Id,
                ValidationOrder = 0,
                Code = "BMLV",
                ValidationExpression = "If(Or(Len(value) = 8,[TransportModeCode] <> \"A\" ),True,False)",
                ErrorMessage = "Master number length must be 8 digits",
            };

            ObjectFieldValidation MasterFieldCheckDigitValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = MasterField.Id,
                ValidationOrder = 1,
                Condition = "If(Len(value) = 8,True,False)",
                ValidationExpression = "If(Or([TransportModeCode] <> \"A\",Mod(Mid(value,1,7),7)= Int(Mid(value,8,1))),True,False)",
                ErrorMessage = "Master check digit is not valid",
                Code = "BMCV",
            };

            objectFieldValidationRepository.Add(MasterFieldLengthValidation);
            objectFieldValidationRepository.Add(MasterFieldCheckDigitValidation);

            #endregion

            objectContext.SaveChanges();
        }
        #endregion

        #region Features
        private void LoadRolesAndFeatures(int tenant)
        {
            ICommonDataContext ObjectContext = CommonDataContext.GetContext(tenant);
            FeatureRepository FeaturesRepository = new FeatureRepository(ObjectContext);
            RoleFeatureRepository RoleFeaturesRepository = new RoleFeatureRepository(ObjectContext);
            TextCodeRepository textCodeRep = new TextCodeRepository(tenant);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            objectTabelQuery = new ObjectTableQuery(objecttableRep);

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(tenant).ToList();
            Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
            List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

            #region ObjectTables
            ObjectTablePM BookingObjectTable = objectTables.Where(d => d.Name == "Booking").FirstOrDefault();
            ObjectTablePM FlightsSchedulesRequestTable = objectTables.Where(d => d.Name == "FlightsSchedulesRequest").FirstOrDefault();
            #endregion

            #region Booking
            Feature BookingFeature_01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.New", NameTextCodeDefaultText = "New Booking" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_02 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_03 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.Edit", NameTextCodeDefaultText = "Edit Booking" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_04 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.PackageFeature", NameTextCodeDefaultText = "Booking Package Feature" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature BookingFeature_Q01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.CreatedBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.CreatedBookings", NameTextCodeDefaultText = " Waiting for Transmission" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q02 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.WatingForResponseBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.WatingForResponseBookings", NameTextCodeDefaultText = "Waiting for Airline Confirmation" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q03 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.ConfirmedBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.ConfirmedBookings", NameTextCodeDefaultText = "Confirmed Without Shipment" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q04 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.RejectedBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.RejectedBookings", NameTextCodeDefaultText = "Errors and Rejections" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q05 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.InProgressBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.InProgressBookings", NameTextCodeDefaultText = "In progress" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q06 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.AllBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.AllBookings", NameTextCodeDefaultText = "All Bookings" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_Q07 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Q.CancelledBookings", FeatureTypeCode = "QUER", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.CancelledBookings", NameTextCodeDefaultText = "Cancelled Bookings" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            
            Feature BookingFeature_A01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BOOKINGSENDRESPONSE", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.SendResponse", NameTextCodeDefaultText = "Send Response", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BookingFeature_A02 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BOOKINGEVENTS", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature BookingFeature_M01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Menu", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.BookingMenu", NameTextCodeDefaultText = "Bookings", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature BookingFeature_C01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Booking.Action.SendToAirlineTenant", Packagable = true, ObjectTableId = BookingObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Booking.Features.SendToAirlineTenant", NameTextCodeDefaultText = "Send to airline tenant", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion

            #region SchedulesRequest
            Feature FlightsSchedulesRequestFeature_01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", ObjectTableId = FlightsSchedulesRequestTable.Id, Tenant = tenant, NameTextCodeCode = "FlightsSchedulesRequest.Features.New", NameTextCodeDefaultText = "New Booking" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature FlightsSchedulesRequestFeature_02 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", ObjectTableId = FlightsSchedulesRequestTable.Id, Tenant = tenant, NameTextCodeCode = "FlightsSchedulesRequest.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature FlightsSchedulesRequestFeature_03 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", ObjectTableId = FlightsSchedulesRequestTable.Id, Tenant = tenant, NameTextCodeCode = "FlightsSchedulesRequest.Features.Edit", NameTextCodeDefaultText = "Edit Booking" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature FlightsSchedulesRequestFeature_04 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", Packagable = true, ObjectTableId = FlightsSchedulesRequestTable.Id, Tenant = tenant, NameTextCodeCode = "FlightsSchedulesRequest.Features.PackageFeature", NameTextCodeDefaultText = "Package Feature" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature FlightsSchedulesRequestFeature_A01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FlightsSchedules.Simulator", Packagable = true, ObjectTableId = FlightsSchedulesRequestTable.Id, Tenant = tenant, NameTextCodeCode = "FlightsSchedulesRequest.Features.SendResponse", NameTextCodeDefaultText = "Send Response", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
        }
        #endregion

        public void LoadMenustables()
        {
            objectContext = WebFreightContext.GetContext(0);
            menusTablesRepository = new MenusTableRepository(objectContext);
            objectTableRepository = new ObjectTableRepository(objectContext);

            FeatureRepository featureRepository = new FeatureRepository(0);
            Dictionary<string, MenusTable> tenantMenusTables = menusTablesRepository.GetMenusTablesByTenant(0).ToDictionary(d => d.Code, a => a);
            List<ObjectTable> tenantObjectTables = objectTableRepository.GetObjectsByTenant(0).ToList();
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();

            menusTablesRepository.SubmitChanges();
        }

        public void LoadObjectTableHelperControls()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableHelperControlsRepository = new ObjectTableHelperControlRepository(objectContext);
            Dictionary<string, ObjectTableHelperControl> TenantHelpers = objectTableHelperControlsRepository.GetObjectTableHelperControlsByTenant(0).ToDictionary(d => d.Code, a => a);

            ObjectTable BookingTable = objectContext.ObjectTables.Where(f => f.Name == "Booking" && f.Tenant == 0).FirstOrDefault();

            objectContext.SaveChanges();
        }

        public void LoadObjectTableTabs()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableTabsRepository = new ObjectTableTabRepository(objectContext);
            textCodeRepository = new TextCodeRepository(objectContext);

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();
            Dictionary<string, ObjectTableTab> TenantObjectTableTabs = objectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);

            ObjectTablePM BookingObjectTable = objectTables.Where(d => d.Name == "Booking").FirstOrDefault();


            objectContext.SaveChanges();
        }

        public void loadQueries()
        {
            objectContext = WebFreightContext.GetContext(0);

            queriesRepository = new QueryRepository(objectContext);
            queryColumnsRepository = new QueryColumnRepository(objectContext);
            advancedQueryFiltersRepository = new AdvancedQueryFilterRepository(objectContext);
            queryGroupRepository = new QueryGroupRepository(objectContext);

            Dictionary<string, Query> tenantQueries = queriesRepository.GetQueriesByTenantSystemLevel(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, QueryColumn> tenantQueryColumns = queryColumnsRepository.GetQueryColumnsByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldId, a => a);
            Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters = advancedQueryFiltersRepository.GetAdvancedQueryFiltersByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldId, a => a);

            FeatureRepository featureRepository = new FeatureRepository(0);
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
           
            #region ObjectTables
            ObjectTable BookingObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Booking" && d.Tenant == 0).FirstOrDefault();
            #endregion

            #region ObjectFields
            List<ObjectField> BookingObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Booking").ToList();
            #endregion

            #region QueryGroups
            QueryGroup BookingQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "BKNG", Name = "Booking" }, queryGroupRepository);
            #endregion

            queryGroupRepository.SubmitChanges();

            #region Booking
            Feature BookingFeature_Created = tenantFeatures.Where(d => d.Code == "Booking.Q.CreatedBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_Waiting = tenantFeatures.Where(d => d.Code == "Booking.Q.WatingForResponseBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_Confirmed = tenantFeatures.Where(d => d.Code == "Booking.Q.ConfirmedBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_Rejected = tenantFeatures.Where(d => d.Code == "Booking.Q.RejectedBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_Progress = tenantFeatures.Where(d => d.Code == "Booking.Q.InProgressBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_All = tenantFeatures.Where(d => d.Code == "Booking.Q.AllBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature BookingFeature_Cancelled = tenantFeatures.Where(d => d.Code == "Booking.Q.CancelledBookings" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            
            Query BookingQueryCreated = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.CreatedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "CreatedBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Created.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryWaiting = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.WatingForResponseBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "WatingForResponse", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Waiting.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryConfirmed = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.ConfirmedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "ConfirmedBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Confirmed.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryRejected = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.RejectedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "RejectedBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Rejected.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryProgress = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.InProgressBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "InProgressBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Progress.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryAll = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.AllBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "AllBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_All.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            Query BookingQueryCancelled = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Booking.Q.CancelledBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, Code = "CancelledBookings", QueryGroupCode = BookingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = BookingObjectTable.Id, QuerySection = "Booking", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = BookingFeature_Cancelled.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", EditWizardName = "Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl" }, queriesRepository, tenantQueries);
            
            #region Created
            QueryColumn BookingQueryCreated_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCreated_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCreated.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryCreatedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryCreated.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "CreatedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region Waiting For Response
            QueryColumn BookingQueryWaiting_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryWaiting_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryWaiting.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryWaitingFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryWaiting.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "WaitingBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region Confirmed
            QueryColumn BookingQueryConfirmed_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryConfirmed_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryConfirmed.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryConfirmedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryConfirmed.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "ConfirmedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region Rejected
            QueryColumn BookingQueryRejected_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryRejected_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryRejected.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryRejectedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryRejected.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "RejectedBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region In Progress
            QueryColumn BookingQueryProgress_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryProgress_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryProgress.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryProgressFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryProgress.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "ProgressBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region All
            QueryColumn BookingQueryAll_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "MainCarriageETD" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingProductName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryAll_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryAll.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Cancelled
            QueryColumn BookingQueryCancelled_Col00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 0, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "TransportModeCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 1, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "DirectionCode" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 2, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingNumber" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 3, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 4, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "Airline" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 5, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 6, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FirstFlight" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 7, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "BookingStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 8, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "SpaceAllocationName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 9, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusName" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn BookingQueryCancelled_Col10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = BookingQueryCancelled.Id, IndexOrder = 10, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "FFRStatusDate" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter BookingQueryCancelledFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { QueryId = BookingQueryCancelled.Id, IsPredefined = true, ObjectFieldId = BookingObjectFields.Where(d => d.FieldName == "CancelledBookings" && d.ObjectTableId == BookingObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #endregion

            objectContext.SaveChanges();
        }

        public void LoadOtherFields(IWebFreightContext context)
        {
            objectContext = context;
            textCodeRepository = new TextCodeRepository(objectContext);

            Dictionary<string, TextCode> textcodes = textCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);

            #region ObjectTables
            ObjectTable BookingObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Booking" && d.Tenant == 0).FirstOrDefault();
            ObjectTable BookingPackageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "BookingPackage" && d.Tenant == 0).FirstOrDefault();
            #endregion

            #region Queries

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.CreatedBookings", DefaultText = "Waiting for Transmission", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.WatingForResponseBookings", DefaultText = "Waiting for Airline Confirmation", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.ConfirmedBookings", DefaultText = "Confirmed Without Shipment", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.RejectedBookings", DefaultText = "Errors and Rejections", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.InProgressBookings", DefaultText = "In Progress", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.AllBookings", DefaultText = "All Bookings", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.Q.CancelledBookings", DefaultText = "Cancelled Bookings", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);
                        
            #endregion

            #region Screens

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Overview.Pisces", DefaultText = "Pieces", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.To", DefaultText = "To", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.MainCarriageLeg1", DefaultText = "Main Carriage Leg1", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.MainCarriageLeg2", DefaultText = "Main Carriage Leg2", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.MainCarriageLeg3", DefaultText = "Main Carriage Leg3", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
			AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.FlightNo", DefaultText = "Flight No", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Routings.Airline", DefaultText = "Airline", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Packages.Details", DefaultText = "Details", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Packages.Summary", DefaultText = "Summary", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.S.Packages.Packages", DefaultText = "Packages", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.S.Packages.Tare", DefaultText = "Tare (%WeightCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.S.Packages.Volume", DefaultText = "Volume (%VolumeCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.S.Packages.Dimensions", DefaultText = "Dimensions (L-W-H) (%UnitCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.S.Packages.Weight", DefaultText = "Gross Weight (%WeightCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.S.Packages.VolumetricWeight", DefaultText = "Volumetric Weight (%WeightCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", }, textCodeRepository, textcodes);
            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.O.Packages.Volume", DefaultText = "Volume (%UnitCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.O.Packages.GrossWeight", DefaultText = "Gross Weight (%UnitCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.O.Packages.Dimensions", DefaultText = "Dimensions (L-W-H) (%UnitCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.O.Packages.VolWeight", DefaultText = "Volumetric Weight (%UnitCode)", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.O.Packages.EditDangerousGoods", DefaultText = "Edit Dangerous Goods", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.B.Packages.DangerouseGoodsDetails", DefaultText = "Details", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", }, textCodeRepository, textcodes);
           
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Booking.M.DeleteThisPackage", DefaultText = "Delete this package?", ObjectTableId = BookingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);
			AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BookingPackage.M.ValueEditedByUser", DefaultText = "Value Edited by User, Double Click to Reset Calculated value", ObjectTableId = BookingPackageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);

            #endregion

            objectContext.SaveChanges();
        }

        public void LoadEventTypes()
        {
            objectContext = WebFreightContext.GetContext(0);
            eventTypesRepository = new EventTypeRepository(objectContext);
            Dictionary<string, EventType> tenantEventTypes = eventTypesRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);

            #region Booking events
            ObjectTablePM bookingObject = ObjectTableQuery.GetObjectTableByCode("Booking", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPBO",
                EnglishName = "Booking Updated",
                Tenant = 0,
                LocalName = "Booking Updated",
                ObjectTableId = bookingObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRBO",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = bookingObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FFR",
                EnglishName = "FFR Message Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "FFR Message Sent",
                ObjectTableId = bookingObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BOKS",
                EnglishName = "Booking Status Updated",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Booking Status Updated",
                ObjectTableId = bookingObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BOKF",
                EnglishName = "Messaging Status Updated",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Messaging Status Updated",
                ObjectTableId = bookingObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            eventTypesRepository.SubmitChanges();
        }
    }
}