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
using Logitude.CRM.BL.CLoseTable;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class CRMUpdate
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
            CRMContext cRMContext = new CRMContext(DatabaseInitializer.GetConnection(connectionStr));

            ActivityTimeTypeRepository activityTimeTypeRepository = new ActivityTimeTypeRepository(cRMContext);
            AddClosedTables.AddActivityTimeType(new ActivityTimeTypeDetails() { Code = "FR", Name = "Free", SearchFields = "FR,Free" }, activityTimeTypeRepository);
            AddClosedTables.AddActivityTimeType(new ActivityTimeTypeDetails() { Code = "BS", Name = "Busy", SearchFields = "BS,Busy" }, activityTimeTypeRepository);
            AddClosedTables.AddActivityTimeType(new ActivityTimeTypeDetails() { Code = "OF", Name = "Out Of Office", SearchFields = "OF,Out Of Office" }, activityTimeTypeRepository);
            AddClosedTables.AddActivityTimeType(new ActivityTimeTypeDetails() { Code = "TN", Name = "Tentative", SearchFields = "TN,Tentative" }, activityTimeTypeRepository);
            activityTimeTypeRepository.SubmitChanges();

            ActivityTypeRepository activityTypeRepository = new ActivityTypeRepository(cRMContext);
            AddClosedTables.AddActivityType(new ActivityTypeDetails() { Code = "TS", Name = "Task", SearchFields = "TS,Task" }, activityTypeRepository);
            AddClosedTables.AddActivityType(new ActivityTypeDetails() { Code = "CL", Name = "Call", SearchFields = "CL,Call" }, activityTypeRepository);
            AddClosedTables.AddActivityType(new ActivityTypeDetails() { Code = "AP", Name = "Appointment", SearchFields = "AP,Appointment" }, activityTypeRepository);
            AddClosedTables.AddActivityType(new ActivityTypeDetails() { Code = "EI", Name = "Email In", SearchFields = "EI,Email In" }, activityTypeRepository);
            AddClosedTables.AddActivityType(new ActivityTypeDetails() { Code = "EO", Name = "Email Out", SearchFields = "EO,Email Out" }, activityTypeRepository);
            activityTypeRepository.SubmitChanges();

            CallTypeRepository callTypeRepository = new CallTypeRepository(cRMContext);
            AddClosedTables.AddActivityCallType(new ActivityCallTypeDetails() { Code = "I", Name = "Incoming", SearchFields = "I,Incoming" }, callTypeRepository);
            AddClosedTables.AddActivityCallType(new ActivityCallTypeDetails() { Code = "O", Name = "Outgoing", SearchFields = "O,Outgoing" }, callTypeRepository);
            callTypeRepository.SubmitChanges();

            //ActivityPriorityRepository priorityRepository = new ActivityPriorityRepository(cRMContext);
            //AddClosedTables.AddActivityPriority(new ActivityPriorityDetails() { Code = "01", Name = "Low", SearchFields = "1,Low" }, priorityRepository);
            //AddClosedTables.AddActivityPriority(new ActivityPriorityDetails() { Code = "02", Name = "Normal", SearchFields = "1,Normal" }, priorityRepository);
            //AddClosedTables.AddActivityPriority(new ActivityPriorityDetails() { Code = "03", Name = "High", SearchFields = "1,High" }, priorityRepository);
            //priorityRepository.SubmitChanges();

            ActivityStatusRepository activityStatusRepository = new ActivityStatusRepository(cRMContext);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "N", Name = "Not Started", SearchFields = "N,Not Started" }, activityStatusRepository);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "I", Name = "In Progress", SearchFields = "I,In Progress" }, activityStatusRepository);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "C", Name = "Completed", SearchFields = "C,Completed" }, activityStatusRepository);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "W", Name = "Waiting on Someone else", SearchFields = "W,Waiting on Someone else" }, activityStatusRepository);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "D", Name = "Deferred", SearchFields = "D,Deferred" }, activityStatusRepository);
            AddClosedTables.AddActivityStatus(new ActivityStatusDetails() { Code = "X", Name = "Canceled", SearchFields = "X,Canceled" }, activityStatusRepository);
            activityStatusRepository.SubmitChanges();
            
            RatingRepository ratingRepository = new RatingRepository(cRMContext);
            AddClosedTables.AddRating(new RatingDetails() { Code = "C", Name = "Cold", SearchFields = "C,Cold", IndexOrder = 1 }, ratingRepository);
            AddClosedTables.AddRating(new RatingDetails() { Code = "W", Name = "Warm", SearchFields = "W,Warm", IndexOrder = 3 }, ratingRepository);
            AddClosedTables.AddRating(new RatingDetails() { Code = "H", Name = "Hot", SearchFields = "H,Hot", IndexOrder = 4 }, ratingRepository);
            AddClosedTables.AddRating(new RatingDetails() { Code = "N", Name = "Neutral", SearchFields = "N,Neutral", IndexOrder = 2 }, ratingRepository);
            ratingRepository.SubmitChanges();

            WebFreightContext webFreightContext = new WebFreightContext(DatabaseInitializer.GetConnection(connectionStr));
            CategoryTypeRepository categoryTypeRepository = new CategoryTypeRepository(webFreightContext);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "CRM", Name = "CRM" }, categoryTypeRepository);
            categoryTypeRepository.SubmitChanges();

            //OpportunityTypeRepository opportunityTypeRepository = new OpportunityTypeRepository(cRMContext);
            //AddClosedTables.AddOpportunityType(new OpportunityTypeDetails() { Code = "N", Name = "New Business", SearchFields = "N,New Business" }, opportunityTypeRepository);
            //AddClosedTables.AddOpportunityType(new OpportunityTypeDetails() { Code = "R", Name = "Bid", SearchFields = "R,Bid" }, opportunityTypeRepository);
            //AddClosedTables.AddOpportunityType(new OpportunityTypeDetails() { Code = "E", Name = "Expansion", SearchFields = "E,Expansion" }, opportunityTypeRepository);
            //AddClosedTables.AddOpportunityType(new OpportunityTypeDetails() { Code = "T", Name = "Routing Order", SearchFields = "T,Routing Order" }, opportunityTypeRepository);
            //opportunityTypeRepository.SubmitChanges();            
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

            #region Activity

            string activityTableId = objectContext.ObjectTables.Where(f => f.Name == "Activity" && f.Tenant == tenant).FirstOrDefault().Id;

            FeaturePM activityFeature_01 = features.Where(d => d.Code == "MarkAsComplete" && d.ObjectTableId == activityTableId).FirstOrDefault();
            FeaturePM activityFeature_02 = features.Where(d => d.Code == "Cancel" && d.ObjectTableId == activityTableId).FirstOrDefault();
            FeaturePM activityFeature_03 = features.Where(d => d.Code == "ReOpen" && d.ObjectTableId == activityTableId).FirstOrDefault();
            FeaturePM activityFeature_04 = features.Where(d => d.Code == "Copy" && d.ObjectTableId == activityTableId).FirstOrDefault();

            MenuButtonGroup activityMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ActivityEdit",
                Name = "ActivityEditButtonsGroup",
                ObjectTableId = activityTableId,
                Tenant = tenant
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            MenuButton activityMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Activity.B.More",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = activityMenuButtonGroup.Id,
                ObjectTableId = activityTableId,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton activityMenuItem1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Cancel",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Activity.B.Cancel",
                LabelTextCodeDefaultText = "Cancel",
                ObjectTableId = activityTableId,
                Tenant = tenant,
                MenuButtonGroupId = activityMenuButtonGroup.Id,
                ParentMenuButtonId = activityMenuButton1.Id,
                FeatureId = activityFeature_02.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton activityMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "MarkAsComplete",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Activity.B.MarkAsComplete",
                LabelTextCodeDefaultText = "Mark as Complete",
                ObjectTableId = activityTableId,
                Tenant = tenant,
                MenuButtonGroupId = activityMenuButtonGroup.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                FeatureId = activityFeature_01.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton activityMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReOpen",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Activity.B.ReOpen",
                LabelTextCodeDefaultText = "Re-open",
                ObjectTableId = activityTableId,
                Tenant = tenant,
                MenuButtonGroupId = activityMenuButtonGroup.Id,
                ParentMenuButtonId = activityMenuButton1.Id,
                FeatureId = activityFeature_03.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton activityMenuItem4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Copy",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Activity.B.Copy",
                LabelTextCodeDefaultText = "Copy",
                ObjectTableId = activityTableId,
                Tenant = tenant,
                MenuButtonGroupId = activityMenuButtonGroup.Id,
                ParentMenuButtonId = activityMenuButton1.Id,
                FeatureId = activityFeature_04.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Opportunity

            string opportunityTableId = objectContext.ObjectTables.Where(f => f.Name == "Opportunity" && f.Tenant == tenant).FirstOrDefault().Id;
           
            FeaturePM opportunityFeature_01 = features.Where(d => d.Code == "CloseAsWon" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_02 = features.Where(d => d.Code == "CloseAsLost" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_03 = features.Where(d => d.Code == "ReOpen" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_04 = features.Where(d => d.Code == "Copy" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_05 = features.Where(d => d.Code == "Cancel" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_06 = features.Where(d => d.Code == "TenantManagement" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_07 = features.Where(d => d.Code == "Totango" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_08 = features.Where(d => d.Code == "CREATETENANT" && d.ObjectTableId == opportunityTableId).FirstOrDefault();
            FeaturePM opportunityFeature_Edit = features.Where(d => d.Code == "Edit.Opportunity" && d.ObjectTableId == opportunityTableId).FirstOrDefault();

            MenuButtonGroup opportunityMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "OpportunityEdit",
                Name = "OpportunityEditButtonsGroup",
                ObjectTableId = opportunityTableId,
                Tenant = tenant
            }, menuButtonGroupRepository, tenantMenuButtonGroups);
                

            MenuButton opportunityMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Opportunity.B.More",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ObjectTableId = opportunityTableId,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton menuItem0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Cancel",
                Index = 0,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.Cancel",
                LabelTextCodeDefaultText = "Cancel",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ParentMenuButtonId = opportunityMenuButton1.Id,
                FeatureId = opportunityFeature_05.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton menuItem1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReOpen",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.ReOpen",
                LabelTextCodeDefaultText = "Re-open",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ParentMenuButtonId = opportunityMenuButton1.Id,
                FeatureId = opportunityFeature_03.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton menuItem2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Copy",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.Copy",
                LabelTextCodeDefaultText = "Copy",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ParentMenuButtonId = opportunityMenuButton1.Id,
                FeatureId = opportunityFeature_04.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton menuItem3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Edit",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.Edit",
                LabelTextCodeDefaultText = "Edit Opportunity",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ParentMenuButtonId = opportunityMenuButton1.Id,
                FeatureId = opportunityFeature_Edit.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton closeAsWonButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CloseAsWon",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.CloseAsWon",
                LabelTextCodeDefaultText = "Close as Won",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                FeatureId = opportunityFeature_01.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton closeAsLostButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CloseAsLost",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.CloseAsLost",
                LabelTextCodeDefaultText = "Close as Lost",
                ObjectTableId = opportunityTableId,
                Tenant = tenant,
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                MenuButtonType = "button",
                Style = "RedButtonStyle",
                FeatureId = opportunityFeature_02.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton TenantManagementButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OpportunityTenantManagement",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.TenantManagement",
                LabelTextCodeDefaultText = "Manage",
                Tenant = tenant,
                MenuButtonType = "button",
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ObjectTableId = opportunityTableId,
                FeatureId = opportunityFeature_06.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            // 8- Totango
            MenuButton TotangoButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OpportunityTotango",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.Totango",
                LabelTextCodeDefaultText = "Totango",
                Tenant = tenant,
                MenuButtonType = "button",

                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ObjectTableId = opportunityTableId,
                FeatureId = opportunityFeature_07.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            // 8- CreateTenant
            MenuButton CreateTenant = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OpportunityCreateTenant",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "Opportunity.B.CreateTenant",
                LabelTextCodeDefaultText = "Create Tenant",
                Tenant = tenant,
                MenuButtonType = "button",
                MenuButtonGroupId = opportunityMenuButtonGroup.Id,
                ObjectTableId = opportunityTableId,
                FeatureId = opportunityFeature_08.Id,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);


            #endregion

            #region Ticket 

            string ticketTableId = objectContext.ObjectTables.Where(f => f.Name == "Ticket" && f.Tenant == tenant).FirstOrDefault().Id;

            FeaturePM ticketFeature_01 = features.Where(d => d.Code == "Cancel" && d.ObjectTableId == ticketTableId).FirstOrDefault();
            FeaturePM ticketFeature_02 = features.Where(d => d.Code == "Reactivate" && d.ObjectTableId == ticketTableId).FirstOrDefault();
            FeaturePM ticketFeature_03 = features.Where(d => d.Code == "ClosewithoutNotifying" && d.ObjectTableId == ticketTableId).FirstOrDefault();

            MenuButtonGroup ticketMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "TicektEdit",
                Name = "TicketEditButtonsGroup",
                ObjectTableId = ticketTableId,
                Tenant = tenant
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            MenuButton ticketMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Ticket.B.More",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = ticketMenuButtonGroup.Id,
                ObjectTableId = ticketTableId,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton ticketMenuItem1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Cancel",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Ticket.B.Cancel",
                LabelTextCodeDefaultText = "Cancel",
                ObjectTableId = ticketTableId,
                Tenant = tenant,
                MenuButtonGroupId = ticketMenuButtonGroup.Id,
                ParentMenuButtonId = ticketMenuButton1.Id,
                FeatureId = ticketFeature_01.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);


            MenuButton ticketMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Reactivate",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Ticekt.B.Reactivate",
                LabelTextCodeDefaultText = "Reactivate",
                ObjectTableId = ticketTableId,
                Tenant = tenant,
                MenuButtonGroupId = ticketMenuButtonGroup.Id,
                ParentMenuButtonId = ticketMenuButton1.Id,
                FeatureId = ticketFeature_02.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            MenuButton ticketMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ClosewithoutNotifying",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Ticekt.B.ClosewithoutNotifying",
                LabelTextCodeDefaultText = "Close without Notifying",
                ObjectTableId = ticketTableId,
                Tenant = tenant,
                MenuButtonGroupId = ticketMenuButtonGroup.Id,
                ParentMenuButtonId = ticketMenuButton1.Id,
                FeatureId = ticketFeature_02.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion 
        }
        #endregion

        #region Load Screens
        public void loadScreens()
        {
            objectContext = WebFreightContext.GetContext(0);
            screenFieldsRepository = new ScreenFieldsRepository(objectContext);
            screensRepository = new ScreensRepository(objectContext);
            Dictionary<string, Screen> tenantScreens = screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

            BuildActivityScreens(tenantScreens, tenantScreenField);
            BuildOpportunityScreens(tenantScreens, tenantScreenField);
            BuildStageScreens(tenantScreens, tenantScreenField);
            BuildClosingReasonScreens(tenantScreens, tenantScreenField);
            BuildOpportunityTypeScreens(tenantScreens, tenantScreenField);
            BuildTicketTypeScreens(tenantScreens, tenantScreenField);
            BuildTicketSeverityScreens(tenantScreens, tenantScreenField);
            BuildTicketStageScreens(tenantScreens, tenantScreenField);
            BuildTicketClassificationScreens(tenantScreens, tenantScreenField);
            BuildTicketScreens(tenantScreens, tenantScreenField);

            LoadObjectTableRulesANDFieldsValidations();
        }

        public void BuildActivityScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Activity" && d.Tenant == 0).FirstOrDefault();
            List<ObjectField> entityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).ToList();

            ObjectField entityObjectField01 = entityObjectFields.Where(d => d.FieldName == "Subject").FirstOrDefault();
            ObjectField entityObjectField02 = entityObjectFields.Where(d => d.FieldName == "ActivityStatusName").FirstOrDefault();
            ObjectField entityObjectField03 = entityObjectFields.Where(d => d.FieldName == "PriorityName").FirstOrDefault();
            ObjectField entityObjectField04 = entityObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault();

            Screen entityHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Activity.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField entityScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = entityObjectField01.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = entityObjectField02.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = entityObjectField03.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = entityObjectField04.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            #region New Activity Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Activity.AdditionalFields", Name = "Additional Fields", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, Tenant = 0 }, screensRepository, tenantScreens);

            #endregion

            entityObjectTable.HeaderScreenId = entityHeaderScreen.Id;
            objectContext.SaveChanges();
        }

        public void BuildOpportunityScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();
            List<ObjectField> entityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).ToList();

            #region Header Screen
            ObjectField entityObjectField01 = entityObjectFields.Where(d => d.FieldName == "Subject").FirstOrDefault();
            ObjectField entityObjectField02 = entityObjectFields.Where(d => d.FieldName == "OwnerName").FirstOrDefault();
            ObjectField entityObjectField03 = entityObjectFields.Where(d => d.FieldName == "ValueField").FirstOrDefault();
            ObjectField entityObjectField04 = entityObjectFields.Where(d => d.FieldName == "RatingName").FirstOrDefault();
            ObjectField entityObjectField05 = entityObjectFields.Where(d => d.FieldName == "StageDueDate").FirstOrDefault();
            ObjectField entityObjectField06 = entityObjectFields.Where(d => d.FieldName == "StageName").FirstOrDefault();
            ObjectField entityObjectField07 = entityObjectFields.Where(d => d.FieldName == "ContactName").FirstOrDefault();
            ObjectField entityObjectField08 = entityObjectFields.Where(d => d.FieldName == "ContactPhone").FirstOrDefault();

            Screen entityHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Opportunity.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField entityScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = entityObjectField01.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = entityObjectField02.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = entityObjectField03.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = entityObjectField04.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = entityObjectField05.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = entityObjectField06.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = entityObjectField07.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField entityScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = entityObjectField08.Id, ScreenId = entityHeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            entityObjectTable.HeaderScreenId = entityHeaderScreen.Id;
            #endregion






            #region New Opportunity Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Opportunity.AdditionalFields", Name = "Additional Fields", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, Tenant = 0 }, screensRepository, tenantScreens);
            #endregion

            objectContext.SaveChanges();
        }

        public void BuildStageScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable stageObject = objectContext.ObjectTables.Where(d => d.Name == "Stage" && d.Tenant == 0).FirstOrDefault();

            ObjectField stageName = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == stageObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField stageProbability = objectContext.ObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTable.Id == stageObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField stageMaxDays = objectContext.ObjectFields.Where(d => d.FieldName == "MaxDays" && d.ObjectTable.Id == stageObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField stageInActive = objectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == stageObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Stage.HeaderScreen", Name = "Header Screen", ObjectTableId = stageObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = stageName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            stageObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            //Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Stage.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = stageObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, screensRepository, tenantScreens);

            //ScreenField stageNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = stageName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            //ScreenField stageProbabilityScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = stageProbability.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            //ScreenField stageMaxDayscreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = stageMaxDays.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            //ScreenField stageInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = stageInActive.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();
        }

        public void BuildClosingReasonScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable closingReasonObject = objectContext.ObjectTables.Where(d => d.Name == "OpportunityClosingReason" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == closingReasonObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LocalNameField = objectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == closingReasonObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == closingReasonObject.Id && d.Tenant == 0).FirstOrDefault();
            
            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "OpportunityClosingReason.HeaderScreen", Name = "Header Screen", ObjectTableId = closingReasonObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_LocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = LocalNameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            closingReasonObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "OpportunityClosingReason.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = closingReasonObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField LocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = LocalNameField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            
            objectContext.SaveChanges();
        }

        public void BuildOpportunityTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable OpportunityTypeObject = objectContext.ObjectTables.Where(d => d.Name == "OpportunityType" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == OpportunityTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == OpportunityTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "OpportunityType.HeaderScreen", Name = "Header Screen", ObjectTableId = OpportunityTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            OpportunityTypeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "OpportunityType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = OpportunityTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();
        }

        public void BuildTicketTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable TicketTypeObject = objectContext.ObjectTables.Where(d => d.Name == "TicketType" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == TicketTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == TicketTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketType.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            TicketTypeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();

        }

        public void BuildTicketSeverityScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable TicketSeverityObject = objectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == TicketSeverityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == TicketSeverityObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketSeverity.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketSeverityObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            TicketSeverityObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketSeverity.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketSeverityObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();

        }

        public void BuildTicketStageScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable TicketStageObject = objectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == TicketStageObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == TicketStageObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketStage.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketStageObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            TicketStageObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketStage.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketStageObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();

        }

        public void BuildTicketClassificationScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable TicketClassificationObject = objectContext.ObjectTables.Where(d => d.Name == "TicketClassification" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == TicketClassificationObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = objectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == TicketClassificationObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketClassification.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketClassificationObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            TicketClassificationObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            //Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketClassification.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketClassificationObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            //ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            //ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();

        }

        public void BuildTicketScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Ticket" && d.Tenant == 0).FirstOrDefault();
            List<ObjectField> entityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).ToList();

            #region New Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Ticket.AdditionalFields", Name = "Additional Fields", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, Tenant = 0 }, screensRepository, tenantScreens);
            #endregion

            ObjectField SubjectField = objectContext.ObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OwnerField = objectContext.ObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField StageField = objectContext.ObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SeverityField = objectContext.ObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField ContactTelField = objectContext.ObjectFields.Where(d => d.FieldName == "ContactTel" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SLANameField = objectContext.ObjectFields.Where(d => d.FieldName == "SLAName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField MainClassificationField = objectContext.ObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SecondaryClassificationField = objectContext.ObjectFields.Where(d => d.FieldName == "SecondaryClassificationName" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField FirstResponseField = objectContext.ObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ResolveWithinField = objectContext.ObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Ticket.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SubjectField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_OwnerScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = OwnerField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            
            ScreenField Header_StageScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column =2 , ObjectFieldId = StageField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_SeverityScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2 , ObjectFieldId = SeverityField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            ScreenField Header_MainClassificationScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = MainClassificationField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_SecondaryClassificationScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = SecondaryClassificationField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            ScreenField Header_ContactTelScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = ContactTelField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_TelScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = SLANameField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            ScreenField Header_FirstResponseField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = FirstResponseField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField Header_ResolveWithinField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = ResolveWithinField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);


            entityObjectTable.HeaderScreenId = HeaderScreen.Id;

            objectContext.SaveChanges();
        }
        #endregion

        #region Load Rules
        public void LoadObjectTableRulesANDFieldsValidations()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableRuleRepository = new ObjectTableRuleRepository(objectContext);
            objectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(objectContext);
            objectFieldValidationRepository = new ObjectFieldValidationRepository(objectContext);
            ruleConditionFieldRepository = new RuleConditionFieldRepository(objectContext);

            Dictionary<string, ObjectTableRule> TenantObjectTableRule = objectTableRuleRepository.GetObjectTableRules(0).ToDictionary(d => d.RuleCode, a => a);
            Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields = objectTableRuleFieldRepository.GetObjectTableRuleFields(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldId, a => a);
            Dictionary<string, RuleConditionField> TenantRuleConditionFields = ruleConditionFieldRepository.GetRuleConditionFieldsByTenant(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldCode, a => a);
            List<ObjectFieldValidation> TenantObjectFieldValidations = objectFieldValidationRepository.GetObjectFieldValidations(0).ToList();

            CreatelosingReasonRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
        }

        private void CreatelosingReasonRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            ObjectTable closingReasonTable = objectContext.ObjectTables.Where(f => f.Name == "OpportunityClosingReason" && f.Tenant == 0).FirstOrDefault();

            ObjectField addedManuallyField = objectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();
            ObjectField closingReasonNameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();
            ObjectField closingReasonLocalNameField = objectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();

            ObjectTableRule blockNamesRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Closing_BlockNames",
                Name = "Block Closing Reason Names",
                ObjectTableId = closingReasonTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, objectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsBlocked_CondField = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = addedManuallyField.Id, ObjectFieldCode = addedManuallyField.FieldCode, ObjectTableRuleId = blockNamesRule.Id, Operator = "Equals", Value = "False", Tenant = blockNamesRule.Tenant }, ruleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField ClosingReasonNameRuleField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = closingReasonNameField.Id, ObjectTableRuleId = blockNamesRule.Id, SystemLevel = true, Tenant = 0 }, objectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ClosingReasonLocalNameRuleField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = closingReasonLocalNameField.Id, ObjectTableRuleId = blockNamesRule.Id, SystemLevel = true, Tenant = 0 }, objectTableRuleFieldRepository, TenantObjectTableRuleFields);
            
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

            #region ObjectTables
            ObjectTablePM GeneralObjectTable = objectTables.Where(d => d.Name == "General").FirstOrDefault();
            ObjectTablePM activityObjectTable = objectTables.Where(d => d.Name == "Activity").FirstOrDefault();
            ObjectTablePM opportunityObjectTable = objectTables.Where(d => d.Name == "Opportunity").FirstOrDefault();
            ObjectTablePM stageObjectTable = objectTables.Where(d => d.Name == "Stage").FirstOrDefault();
            ObjectTablePM opportunityStageObjectTable = objectTables.Where(d => d.Name == "OpportunityStage").FirstOrDefault();
            ObjectTablePM closingReasonObjectTable = objectTables.Where(d => d.Name == "OpportunityClosingReason").FirstOrDefault();
            ObjectTablePM QuestionnaireQuestionObjectTable = objectTables.Where(d => d.Name == "QuestionnaireQuestion").FirstOrDefault();
            ObjectTablePM QuestionnaireObjectTable = objectTables.Where(d => d.Name == "Questionnaire").FirstOrDefault();
            ObjectTablePM QuestionnaireAnswerObjectTable = objectTables.Where(d => d.Name == "QuestionnaireAnswer").FirstOrDefault();
            ObjectTablePM OpportunityTypeObjectTable = objectTables.Where(d => d.Name == "OpportunityType").FirstOrDefault();
            ObjectTablePM TicketTypeObjectTable = objectTables.Where(d => d.Name == "TicketType").FirstOrDefault();
            ObjectTablePM TicketSeverityObjectTable = objectTables.Where(d => d.Name == "TicketSeverity").FirstOrDefault();
            ObjectTablePM TicketStageObjectTable = objectTables.Where(d => d.Name == "TicketStage").FirstOrDefault();
            ObjectTablePM TicketClassificationObjectTable = objectTables.Where(d => d.Name == "TicketClassification").FirstOrDefault();
            ObjectTablePM TicketObjectTable = objectTables.Where(d => d.Name == "Ticket").FirstOrDefault();
            ObjectTablePM employeeGroupObjectTable = objectTables.Where(d => d.Name == "EmployeeGroup").FirstOrDefault();
            ObjectTablePM SLAHeaderObjectTable = objectTables.Where(d => d.Name == "SLAHeader").FirstOrDefault();
            ObjectTablePM TicketEscalationsObjectTable = objectTables.Where(d => d.Name == "TicketEscalation").FirstOrDefault();
            #endregion

            Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
            List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

            Feature ActivityModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.PackageFeature", NameTextCodeDefaultText = "Activity Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.PackageFeature", NameTextCodeDefaultText = "Opportunity Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #region Activity Features

            Feature activityFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.New", NameTextCodeDefaultText = "New Activity", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", IsBusinessUnitEnabled = true, ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", IsBusinessUnitEnabled = true, ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.Edit", NameTextCodeDefaultText = "Edit Activity", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.MyOpenActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.MyOpenActivities", NameTextCodeDefaultText = "My Open Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.AllOpenActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.AllOpenActivities", NameTextCodeDefaultText = "All Open Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.MyClosedActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.MyClosedActivities", NameTextCodeDefaultText = "My Closed Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.AllClosedActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.AllClosedActivities", NameTextCodeDefaultText = "All Closed Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.AllActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.AllActivities", NameTextCodeDefaultText = "All Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.MeetingsSummary", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.MeetingsSummary", NameTextCodeDefaultText = "Meetings Summary", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Q.CancelledActivities", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.CancelledActivities", NameTextCodeDefaultText = "Cancelled Activities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature activityFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Tab.General", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Activity.Tab.Events", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MarkAsComplete", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.MarkAsComplete", NameTextCodeDefaultText = "Mark as Complete", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReOpen", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.ReOpen", NameTextCodeDefaultText = "Re-Open", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature activityFeature_MB4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Copy", ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.Copy", NameTextCodeDefaultText = "Copy", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature activityFeature11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", Packagable = true, ObjectTableId = activityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Activity.Features.OutlookConnection", NameTextCodeDefaultText = "Outlook Connection", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion

            #region Opportunity Features

            Feature opportunityFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.New", NameTextCodeDefaultText = "New Opportunity", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", IsBusinessUnitEnabled = true, ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", IsBusinessUnitEnabled = true, ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Edit", NameTextCodeDefaultText = "Edit Opportunity", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.MyOpenOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.MyOpenOpportunities", NameTextCodeDefaultText = "My Open Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllOpenOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.AllOpenOpportunities", NameTextCodeDefaultText = "All Open Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.MyClosedOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.MyClosedOpportunities", NameTextCodeDefaultText = "My Closed Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllClosedOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.AllClosedOpportunities", NameTextCodeDefaultText = "All Closed Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.AllOpportunities", NameTextCodeDefaultText = "All Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.OpenByStage", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.OpenByStage", NameTextCodeDefaultText = "Open Opportunities by Stage", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.CancelledOpportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.CancelledOpportunities", NameTextCodeDefaultText = "Cancelled Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature opportunityFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Overview", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Overview", NameTextCodeDefaultText = "Main", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.General", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Products", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Products", NameTextCodeDefaultText = "Products", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.DocsOut", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.DocsIn", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);            
            Feature opportunityFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Events", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature opportunityFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseAsWon", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.CloseAsWon", NameTextCodeDefaultText = "Close as Won", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseAsLost", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.CloseAsLost", NameTextCodeDefaultText = "Close as Lost", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReOpen", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.ReOpen", NameTextCodeDefaultText = "Re-open", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Copy", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Copy", NameTextCodeDefaultText = "Copy", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TenantManagement", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.TenantManagement", NameTextCodeDefaultText = "Tenant Management", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Totango", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.Totango", NameTextCodeDefaultText = "Totango", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATETENANT", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.CreateTenant", NameTextCodeDefaultText = "Create Tenant", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_MB9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Edit.Opportunity", ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.EditOpportunity", NameTextCodeDefaultText = "Edit Opportunity", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityFeature_OC1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", Packagable = true, ObjectTableId = opportunityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Opportunity.Features.OutlookConnection", NameTextCodeDefaultText = "Outlook Connection", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
         
            #endregion

            // Stage Features            
            Feature stageFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.New", NameTextCodeDefaultText = "New Stage", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature stageFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature stageFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.Edit", NameTextCodeDefaultText = "Edit Stage", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature stageFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLSTAGES", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.AllStages", NameTextCodeDefaultText = "All Stages", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature stageFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Stage.Tab.General", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature stageFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Stage.Tab.Events", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GeneralStageFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STAGES", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.Stages", NameTextCodeDefaultText = "Stages", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature StageModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = stageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Stage.Features.PackageFeature", NameTextCodeDefaultText = "Stage Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            // Opportunity Stage Features            
            Feature opportunityStageFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = opportunityStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityStage.Features.New", NameTextCodeDefaultText = "New Opportunity Stage", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityStageFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = opportunityStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityStage.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityStageFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = opportunityStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityStage.Features.Edit", NameTextCodeDefaultText = "Edit Opportunity Stage", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature opportunityStageFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = opportunityStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityStage.Features.PackageFeature", NameTextCodeDefaultText = "Opportunity Stage Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            //Closing Reason
            Feature closingReasonFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.New", NameTextCodeDefaultText = "New Closing Reason", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.Edit", NameTextCodeDefaultText = "Edit Closing Reason", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLCLOSINGREASONS", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.AllClosingReasons", NameTextCodeDefaultText = "All Closing Reasons", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OpportunityClosingReason.Tab.General", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OpportunityClosingReason.Tab.Events", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GeneralClosingReasonFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLOSINGREASONS", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.ClosingReasons", NameTextCodeDefaultText = "Closing Reasons", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature closingReasonModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = closingReasonObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ClosingReason.Features.PackageFeature", NameTextCodeDefaultText = "Closing Reason Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #region  QuestionnaireAnswersFeatures
            Feature QuestionnaireAnswerFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = QuestionnaireAnswerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireAnswer.Features.New", NameTextCodeDefaultText = "New QuestionnaireAnswer", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireAnswerFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = QuestionnaireAnswerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireAnswer.Features.Edit", NameTextCodeDefaultText = "Edit QuestionnaireAnswer", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireAnswerFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = QuestionnaireAnswerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireAnswer.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            
            #endregion

            #region QuestionnaireFeatures
            Feature QuestionnaireFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = QuestionnaireObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Questionnaire.Features.New", NameTextCodeDefaultText = "New Questionnaire", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = QuestionnaireObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Questionnaire.Features.Edit", NameTextCodeDefaultText = "Edit Questionnaire", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = QuestionnaireObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Questionnaire.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Questionnaire.Q.AllQuestionnaires", ObjectTableId = QuestionnaireObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Questionnaire.Features.AllQuestionnaires", NameTextCodeDefaultText = "All Questionnaires", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = QuestionnaireObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Questionnaire.Features.PackageFeature", NameTextCodeDefaultText = "Questionnaires Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GeneralQuestionnairefeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUESTIONNAIRE", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.Questionnaire", NameTextCodeDefaultText = "Questionnaire", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature Questionnairefeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUESTIONNAIREGENERAL", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.QuestionnaireGeneral", NameTextCodeDefaultText = "Questionnaire", FeatureTypeCode = "OTH" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region QuestionnaireQuestionFeatures
            Feature QuestionnaireQuestionFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = QuestionnaireQuestionObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireQuestion.Features.New", NameTextCodeDefaultText = "New QuestionnaireQuestion", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireQuestionFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = QuestionnaireQuestionObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireQuestion.Features.Edit", NameTextCodeDefaultText = "Edit QuestionnaireQuestion", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature QuestionnaireQuestionFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = QuestionnaireQuestionObjectTable.Id, Tenant = tenant, NameTextCodeCode = "QuestionnaireQuestion.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region OpportunityType
            Feature OpportunityTypeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.New", NameTextCodeDefaultText = "New Opportunity Type", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.Edit", NameTextCodeDefaultText = "Edit Opportunity Type", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLOPPORTUNITYTYPES", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.AllOpportunityTypes", NameTextCodeDefaultText = "All Opportunity Types", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OpportunityType.Tab.General", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OpportunityType.Tab.Events", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GeneralOpportunityTypeFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPPORTUNITYTYPES", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.OpportunityTypes", NameTextCodeDefaultText = "Opportunity Types", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature OpportunityTypeModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpportunityType.Features.PackageFeature", NameTextCodeDefaultText = "Opportunity Type Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region Ticket Types 
            Feature TicketTypeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.New", NameTextCodeDefaultText = "New Ticket Type", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.Edit", NameTextCodeDefaultText = "Edit Ticket  Type", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.PackageFeature", NameTextCodeDefaultText = "Ticket Type Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETTYPES", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.TicketTypes", NameTextCodeDefaultText = "Ticket Types", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETTYPES", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.AllTicketTypes", NameTextCodeDefaultText = "All Ticket Types", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketType.Tab.General", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketTypeFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketType.Tab.Events", ObjectTableId = TicketTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketType.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region Ticket Severities
            Feature TicketSeverityFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.New", NameTextCodeDefaultText = "New Ticket Severity", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.Edit", NameTextCodeDefaultText = "Edit Ticket Severity", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.PackageFeature", NameTextCodeDefaultText = "TicketType Type Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETSEVERITIES", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.TicketSeverities", NameTextCodeDefaultText = "Ticket Severities", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETSEVERITIES", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.AllTicketSeverities", NameTextCodeDefaultText = "All Ticket Severities", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketSeverity.Tab.General", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketSeverityFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketSeverity.Tab.Events", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketSeverity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        
            #endregion

            #region Ticket Stage 
            Feature TicketStageFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.New", NameTextCodeDefaultText = "New Ticket Stage", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.Edit", NameTextCodeDefaultText = "Edit Ticket  Stage", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.PackageFeature", NameTextCodeDefaultText = "Ticket Stage Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETSTAGES", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.TicketStages", NameTextCodeDefaultText = "Ticket Stages", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETSTAGES", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.AllTicketStagess", NameTextCodeDefaultText = "All Ticket Stages", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketStage.Tab.General", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketStageFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketStage.Tab.Events", ObjectTableId = TicketStageObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketStage.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        
            #endregion 

            #region Ticket Classification
            Feature TicketClassificationFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.New", NameTextCodeDefaultText = "New Ticket Classification", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.Edit", NameTextCodeDefaultText = "Edit Ticket  Classification", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.PackageFeature", NameTextCodeDefaultText = "Ticket Classification Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETCLASSIFICATION", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.TicketClassifications", NameTextCodeDefaultText = "Ticket Classifications", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETCLASSIFICATIONS", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.AllTicketClassifications", NameTextCodeDefaultText = "All Ticket Classifications", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketClassification.Tab.General", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TicketClassificationFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketClassification.Tab.Events", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketClassification.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion 

            #region Ticket
            Feature ticketFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.New", NameTextCodeDefaultText = "New Ticket", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", IsBusinessUnitEnabled = true, ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", IsBusinessUnitEnabled = true, ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Edit", NameTextCodeDefaultText = "Edit Ticket", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.AllOpenTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.AllOpenTickets", NameTextCodeDefaultText = "All Open Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.AllCancelledTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.AllCancelledTickets", NameTextCodeDefaultText = "All Cancelled Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.PackageFeature", NameTextCodeDefaultText = "Ticket Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.Events", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.General", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Reactivate", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Reactivate", NameTextCodeDefaultText = "Reactivate", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.Overview", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Overview", NameTextCodeDefaultText = "Overview", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.DocsOut", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.DocsIn", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.Main", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Main", NameTextCodeDefaultText = "Main", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.TicketEscalation", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.TicketEscalation", NameTextCodeDefaultText = "Ticket Escalation", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ClosewithoutNotifying", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.ClosewithoutNotifying", NameTextCodeDefaultText = "Close without Notifying", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.UnassignedTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.UnassignedTickets", NameTextCodeDefaultText = "Unassigned Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.AllTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.AllTickets", NameTextCodeDefaultText = "All Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature18 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.SolvedTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SolvedTickets", NameTextCodeDefaultText = "Solved Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature19 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.SLAFailureTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SLAFailures", NameTextCodeDefaultText = "SLA Failures Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature20 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.RecentlyUpdatedTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.RecentlyUpdated", NameTextCodeDefaultText = "Recently Updated Tickets", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature21 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.Audit", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Audit", NameTextCodeDefaultText = "Audit", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature22 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.SolvedSLAFailureTickets", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SolvedSLAFailures", NameTextCodeDefaultText = "Solved with SLA Failures", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature23 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Tab.Communication", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

           

            Feature TicketDashboardFeature_M01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketDashboard.Menu", Packagable = true, ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TicketDashboard.Features.TicketMenu", NameTextCodeDefaultText = "Tickets Dashboard", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature ticketFeature24 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketReply", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.TicketReply", NameTextCodeDefaultText = "Reply", FeatureTypeCode = "ACT",Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature25 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketClosure", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.TicketClosure", NameTextCodeDefaultText = "Closure", FeatureTypeCode = "ACT" , Packagable = true}, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature26 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketActivities", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.TicketActivities", NameTextCodeDefaultText = "Activities", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature27 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketMore", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.TicketMore", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature ticketFeature28 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUTOMATION", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.Automation", NameTextCodeDefaultText = "Automation", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature ticketFeature29 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SaveAsClosed", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SaveAsClosed", NameTextCodeDefaultText = "Save As Closed", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SaveAsOpen", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SaveAsOpen", NameTextCodeDefaultText = "Save As Open", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SaveAsResolved", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.SaveAsResolved", NameTextCodeDefaultText = "Save As Resolved", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ticketFeature32 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OwnerLicenseUpdate", ObjectTableId = TicketObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.OwnerLicenseUpdate", NameTextCodeDefaultText = "Owner License Update", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature ticketsSettingFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketsSetting", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.TicketsSetting", NameTextCodeDefaultText = "Ticket Settings", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion 

            #region Ticket Escalation

            Feature ticketEscalationFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Ticket.Q.AllTicketEscalations", ObjectTableId = TicketEscalationsObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Ticket.Features.AllTicketEscalations", NameTextCodeDefaultText = "Ticket Escalations", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion 

            #region EmployeeGroup
            Feature EmployeeGroupFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EMPLOYEEGROUPS", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.EmployeeGroups", NameTextCodeDefaultText = "Employee Groups", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature EmployeeGroupModuleFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = employeeGroupObjectTable.Id, Tenant = tenant, NameTextCodeCode = "EmployeeGroup.Features.PackageFeature", NameTextCodeDefaultText = "Employee Group Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion 

            #region SLAHeader 
            //Feature SLAHeaderFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = SLAHeaderObjectTable.Id, Tenant = tenant, NameTextCodeCode = "SLAHeader.Features.New", NameTextCodeDefaultText = "New SLA", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature SLAHeaderFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = SLAHeaderObjectTable.Id, Tenant = tenant, NameTextCodeCode = "SLAHeader.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature SLAHeaderFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = SLAHeaderObjectTable.Id, Tenant = tenant, NameTextCodeCode = "SLAHeader.Features.Edit", NameTextCodeDefaultText = "Edit SLA", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature SLAHeaderFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = SLAHeaderObjectTable.Id, Tenant = tenant, NameTextCodeCode = "SLAHeader.Features.PackageFeature", NameTextCodeDefaultText = "SLA Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature SLAHeaderFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SLAHEADER", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.SLAHeader", NameTextCodeDefaultText = "SLA", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion 

            #region Business hours
            //BUSINESSHOUR
            Feature BusinesshourFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUSINESSHOUR", Packagable = true, ObjectTableId = GeneralObjectTable.Id, Tenant = tenant, NameTextCodeCode = "General.Features.BusinessHour", NameTextCodeDefaultText = "Business Hours", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

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

            #region Features
            Feature stageFeature = tenantFeatures.Where(d => d.Code == "STAGES" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature closingFeature = tenantFeatures.Where(d => d.Code == "CLOSINGREASONS" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature QuestionnairesFeature = tenantFeatures.Where(d => d.Code == "QUESTIONNAIRE" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature OpportunityTypeFeature = tenantFeatures.Where(d => d.Code == "OPPORTUNITYTYPES" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature TicketTypeFeature = tenantFeatures.Where(d => d.Code == "TICKETTYPES" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature OccasionTypeFeature = tenantFeatures.Where(d => d.Code == "General.Occasion.OccasionType" && d.FeatureTypeCode == "MENU").FirstOrDefault();

            Feature TicketSeverityFeature = tenantFeatures.Where(d => d.Code == "TICKETSEVERITIES" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature TicketStageFeature = tenantFeatures.Where(d => d.Code == "TICKETSTAGES" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature TicketClassificationFeature = tenantFeatures.Where(d => d.Code == "TICKETCLASSIFICATION" && d.FeatureTypeCode == "MENU").FirstOrDefault();

            Feature EmployeeGroupFeature = tenantFeatures.Where(d => d.Code == "EMPLOYEEGROUPS" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature SLAHeaderFeature = tenantFeatures.Where(d => d.Code == "SLAHEADER" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature BusinessHourFeature = tenantFeatures.Where(d => d.Code == "BUSINESSHOUR" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTQU", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 16, CategoryTypeCode = "Oth", TextCode = "General.MC.Others.Questionnaires", Icon = "BusinessUnits.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Questionnaire").FirstOrDefault().Id, FeatureId = QuestionnairesFeature.Id }, menusTablesRepository, tenantMenusTables);

            //CRM            
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTSG", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 1, CategoryTypeCode = "CRM", TextCode = "General.MC.CRM.Stages", Icon = "Stages.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Stage").FirstOrDefault().Id, FeatureId = stageFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTCS", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 2, CategoryTypeCode = "CRM", TextCode = "General.MC.CRM.ClosingReasons", Icon = "ClosingReasons.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "OpportunityClosingReason").FirstOrDefault().Id, FeatureId = closingFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTOP", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 3, CategoryTypeCode = "CRM", TextCode = "General.MC.CRM.OpportunityTypes", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "OpportunityType").FirstOrDefault().Id, FeatureId = OpportunityTypeFeature.Id }, menusTablesRepository, tenantMenusTables);
            
            //Tickets 
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTTT", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 0, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.TicketType", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TicketType").FirstOrDefault().Id, FeatureId = TicketTypeFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTTS", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 1, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.TicketSeverity", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TicketSeverity").FirstOrDefault().Id, FeatureId = TicketSeverityFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTTG", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 2, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.TicketStage", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TicketStage").FirstOrDefault().Id, FeatureId = TicketStageFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTTC", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 3, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.TicketClassification", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TicketClassification").FirstOrDefault().Id, FeatureId = TicketClassificationFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTSV", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 4, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.SLAHeader", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "SLAHeader").FirstOrDefault().Id, FeatureId = SLAHeaderFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTEG", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 5, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.EmployeeGroups", Icon = "EmployeeGroups.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "EmployeeGroup").FirstOrDefault().Id, FeatureId = EmployeeGroupFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTBH", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 6, CategoryTypeCode = "TKT", TextCode = "General.MC.TKT.BusinessHour", Icon = "BusinessHour.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "BusinessHour").FirstOrDefault().Id, FeatureId = BusinessHourFeature.Id }, menusTablesRepository, tenantMenusTables);

            // Occasion
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTOT", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 0, CategoryTypeCode = "OCS", TextCode = "General.MC.OCS.OccasionType", Icon = "OpportunityTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "OccasionType").FirstOrDefault().Id, FeatureId = OccasionTypeFeature.Id }, menusTablesRepository, tenantMenusTables);

            menusTablesRepository.SubmitChanges(); 
        }

        public void LoadObjectTableHelperControls()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableHelperControlsRepository = new ObjectTableHelperControlRepository(objectContext);

            ObjectTable opportunityTable = objectContext.ObjectTables.Where(f => f.Name == "Opportunity" && f.Tenant == 0).FirstOrDefault();
            ObjectTable activityTable = objectContext.ObjectTables.Where(f => f.Name == "Activity" && f.Tenant == 0).FirstOrDefault();
            ObjectTable ticketTable = objectContext.ObjectTables.Where(f => f.Name == "Ticket" && f.Tenant == 0).FirstOrDefault();

            Dictionary<string, ObjectTableHelperControl> TenantHelpers = objectTableHelperControlsRepository.GetObjectTableHelperControlsByTenant(0).ToDictionary(d => d.Code, a => a);

            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "OPPH", ControlPath = "Logitude.CRM.Views.Helper.OpportunityHelperControl", ObjectTableId = opportunityTable.Id, Tenant = 0 }, objectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "ACTH", ControlPath = "Logitude.CRM.Views.Helper.ActivityHelperControl", ObjectTableId = activityTable.Id, Tenant = 0 }, objectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "TKTH", ControlPath = "Logitude.CRM.Views.Helper.TicketHelperControl", ObjectTableId = ticketTable.Id, Tenant = 0 }, objectTableHelperControlsRepository, TenantHelpers);

            objectContext.SaveChanges();
        }

        public void LoadObjectTableTabs()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableTabsRepository = new ObjectTableTabRepository(objectContext);
            textCodeRepository = new TextCodeRepository(objectContext);
            Dictionary<string, ObjectTableTab> TenantObjectTableTabs = objectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);
     
            #region ObjectTables
            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();

            ObjectTablePM activityObjectTable = objectTables.Where(d => d.Name == "Activity").FirstOrDefault();
            ObjectTablePM opportunityObjectTable = objectTables.Where(d => d.Name == "Opportunity").FirstOrDefault();
            ObjectTablePM stageObjectTable = objectTables.Where(d => d.Name == "Stage").FirstOrDefault();
            ObjectTablePM closingObjectTable = objectTables.Where(d => d.Name == "OpportunityClosingReason").FirstOrDefault();
            ObjectTablePM OpportunityTypeObjectTable = objectTables.Where(d => d.Name == "OpportunityType").FirstOrDefault();
            ObjectTablePM TicketTypeObjectTable = objectTables.Where(d => d.Name == "TicketType").FirstOrDefault();

            ObjectTablePM TicketSeverityObjectTable = objectTables.Where(d => d.Name == "TicketSeverity").FirstOrDefault();
            ObjectTablePM TicketStageObjectTable = objectTables.Where(d => d.Name == "TicketStage").FirstOrDefault();
            ObjectTablePM TicketClassificationObjectTable = objectTables.Where(d => d.Name == "TicketClassification").FirstOrDefault();
            ObjectTablePM TicketObjectTable = objectTables.Where(d => d.Name == "Ticket").FirstOrDefault(); 

            #endregion

            #region TextCodes
            Dictionary<string, TextCode> textcodes = textCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);

            #region Activity TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.TH.General", DefaultText = "General", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.TH.Events", DefaultText = "Events", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);

            #endregion

            #region Opportunity TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Overview", DefaultText = "Main", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.General", DefaultText = "General", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.DocsOut", DefaultText = "Docs Out", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.DocsIn", DefaultText = "Docs In", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Products", DefaultText = "Products", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Events", DefaultText = "Events", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            
            #endregion

            #region Stage TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Stage.TH.General", DefaultText = "General", ObjectTableId = stageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Stage.TH.Events", DefaultText = "Events", ObjectTableId = stageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region OpportunityClosingReason TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityClosingReason.TH.General", DefaultText = "General", ObjectTableId = closingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityClosingReason.TH.Events", DefaultText = "Events", ObjectTableId = closingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region OpportunityType TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityType.TH.General", DefaultText = "General", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityType.TH.Events", DefaultText = "Events", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region TicketType TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketType.TH.General", DefaultText = "General", ObjectTableId = TicketTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketType.TH.Events", DefaultText = "Events", ObjectTableId = TicketTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region TicketSeverity TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.TH.General", DefaultText = "General", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.TH.Events", DefaultText = "Events", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region TicketStage TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.TH.General", DefaultText = "General", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.TH.Events", DefaultText = "Events", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region TicketClassification TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketClassification.TH.General", DefaultText = "General", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketClassification.TH.Events", DefaultText = "Events", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region Ticket TextCodes
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.General", DefaultText = "Details", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.Events", DefaultText = "Events", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.DocsOut", DefaultText = "Docs Out", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.DocsIn", DefaultText = "Docs In", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.Overview", DefaultText = "Overview", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.Main", DefaultText = "Main", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.TicketEscalation", DefaultText = "Ticket Escalations", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.Audit", DefaultText = "Audit", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.TH.Communications", DefaultText = "Communication", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);

            #endregion

            objectContext.SaveChanges();
            #endregion

            #region Features
            FeatureRepository featureRepository = new FeatureRepository(0);
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();

            #region Activity Features
            Feature activityFeature1 = tenantFeatures.Where(d => d.Code == "Activity.Tab.General" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault();
            Feature activityFeature2 = tenantFeatures.Where(d => d.Code == "Activity.Tab.Events" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault();
            #endregion 
            
            #region Opportunity Features
            Feature opportunityFeature1 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.Overview" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();
            Feature opportunityFeature2 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.General" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();            
            Feature opportunityFeature3 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.Products" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();
            Feature opportunityFeature4 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.DocsOut" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();
            Feature opportunityFeature5 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.DocsIn" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();           
            Feature opportunityFeature6 = tenantFeatures.Where(d => d.Code == "Opportunity.Tab.Events" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault();
            
            #endregion 

            #region Stage Features
            Feature stageFeature1 = tenantFeatures.Where(d => d.Code == "Stage.Tab.General" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault();
            Feature stageFeature2 = tenantFeatures.Where(d => d.Code == "Stage.Tab.Events" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault();
            #endregion

            #region OpportunityClosingReason Features
            Feature OpportunityClosingReasonFeature1 = tenantFeatures.Where(d => d.Code == "OpportunityClosingReason.Tab.General" && d.ObjectTableId == closingObjectTable.Id).FirstOrDefault();
            Feature OpportunityClosingReasonFeature2 = tenantFeatures.Where(d => d.Code == "OpportunityClosingReason.Tab.Events" && d.ObjectTableId == closingObjectTable.Id).FirstOrDefault();
            #endregion

            #region OpportunityType Features
            Feature OpportunityTypeFeature1 = tenantFeatures.Where(d => d.Code == "OpportunityType.Tab.General" && d.ObjectTableId == OpportunityTypeObjectTable.Id).FirstOrDefault();
            Feature OpportunityTypeFeature2 = tenantFeatures.Where(d => d.Code == "OpportunityType.Tab.Events" && d.ObjectTableId == OpportunityTypeObjectTable.Id).FirstOrDefault();
            #endregion

            #region TicketType Features
            Feature TicketTypeFeature1 = tenantFeatures.Where(d => d.Code == "TicketType.Tab.General" && d.ObjectTableId == TicketTypeObjectTable.Id).FirstOrDefault();
            Feature TicketTypeFeature2 = tenantFeatures.Where(d => d.Code == "TicketType.Tab.Events" && d.ObjectTableId == TicketTypeObjectTable.Id).FirstOrDefault();
            #endregion

            #region TicketSeverity Features
            Feature TicketSeverityFeature1 = tenantFeatures.Where(d => d.Code == "TicketSeverity.Tab.General" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault();
            Feature TicketSeverityFeature2 = tenantFeatures.Where(d => d.Code == "TicketSeverity.Tab.Events" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault();
            #endregion

            #region TicketStage Features
            Feature TicketStageFeature1 = tenantFeatures.Where(d => d.Code == "TicketStage.Tab.General" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault();
            Feature TicketStageFeature2 = tenantFeatures.Where(d => d.Code == "TicketStage.Tab.Events" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault();
            #endregion

            #region TicketClassification Features
            Feature TicketClassificationFeature1 = tenantFeatures.Where(d => d.Code == "TicketClassification.Tab.General" && d.ObjectTableId == TicketClassificationObjectTable.Id).FirstOrDefault();
            Feature TicketClassificationFeature2 = tenantFeatures.Where(d => d.Code == "TicketClassification.Tab.Events" && d.ObjectTableId == TicketClassificationObjectTable.Id).FirstOrDefault();
            #endregion

            #region Ticket 

            Feature TicketFeature1 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.General" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature2 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.Events" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature3 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.Overview" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature4 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.DocsOut" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature5 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.DocsIn" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature6 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.Main" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature7 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.TicketEscalation" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature8 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.Audit" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            Feature TicketFeature9 = tenantFeatures.Where(d => d.Code == "Ticket.Tab.Communication" && d.ObjectTableId == TicketObjectTable.Id).FirstOrDefault();
            #endregion 

            #endregion

            #region Add Tabs

            #region Activity Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ACGN", FeatureId = activityFeature1.Id, ControlPath = "Logitude.CRM.Views.Tabs.ActivityTabs.ActivityGeneralTabControl", ObjectTableId = activityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 , HtmlComponentUrl = "./CRMModules/CRMActivity/Components/EditTabs/ActivityGeneralTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ACVN", FeatureId = activityFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = activityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);          
            #endregion

            #region Opportunity Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPOV", FeatureId = opportunityFeature1.Id, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.OverviewTabControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.Overview" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0,HtmlComponentUrl= "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityOverviewTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPGN", FeatureId = opportunityFeature2.Id, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.OpportunityGeneralTabControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1,HtmlComponentUrl= "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityGeneralTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);            
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPPR", FeatureId = opportunityFeature3.Id, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.ProductsTabControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.Products" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2,HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityProductsTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPDO", FeatureId = opportunityFeature4.Id, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.DocsOutTabControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.DocsOut" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3,HtmlComponentUrl= "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityDocsOutTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPDI", FeatureId = opportunityFeature5.Id, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.DocsInTabControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 , HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityDocsInTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);           
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPEV", FeatureId = opportunityFeature6.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = opportunityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);            
            #endregion

            #region Stage Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "STGN", FeatureId = stageFeature1.Id, ControlPath = "Logitude.CRM.Views.Tabs.StageTabs.StageGeneralTabControl", HtmlComponentUrl = "./CRMModules/CRMStages/Components/StageGeneralTabComponent", ObjectTableId = stageObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Stage.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "SGEV", FeatureId = stageFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = stageObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Stage.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region OpportunityClosingReason Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CRGN", FeatureId = OpportunityClosingReasonFeature1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = closingObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityClosingReason.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CEEV", FeatureId = OpportunityClosingReasonFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = closingObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityClosingReason.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region OpportunityType Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OTGE", FeatureId = OpportunityTypeFeature1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = OpportunityTypeObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityType.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OTEV", FeatureId = OpportunityTypeFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = OpportunityTypeObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityType.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region TicketType Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TTGE", FeatureId = TicketTypeFeature1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = TicketTypeObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketType.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TTEV", FeatureId = TicketTypeFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketTypeObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketType.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region TicketSeverity Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TSGE", FeatureId = TicketSeverityFeature1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = TicketSeverityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketSeverity.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TSEV", FeatureId = TicketSeverityFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketSeverityObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketSeverity.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region TicketStage Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TAGE", FeatureId = TicketStageFeature1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = TicketStageObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketStage.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TAEV", FeatureId = TicketStageFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketStageObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketStage.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region TicketClassification Tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TCGE", FeatureId = TicketClassificationFeature1.Id, ControlPath = "Logitude.CRM.Views.Tabs.TicketClassification.TicketClassificationGeneralTabControl", ObjectTableId = TicketClassificationObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketClassification.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TCEV", FeatureId = TicketClassificationFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketClassificationObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketClassification.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region Ticket
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIMN", FeatureId = TicketFeature6.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketMainTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.Main" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0,     HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/MainTab/TicketMainTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIOV", FeatureId = TicketFeature3.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketOverviewTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.Overview" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/Overview/TicketOverviewTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIGE", FeatureId = TicketFeature1.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketGeneralTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/Details/TicketDetailsTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIDO", FeatureId = TicketFeature4.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketDocsOutTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.DocsOut" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/DocsOut/TicketDocsOutTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIDI", FeatureId = TicketFeature5.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketDocsInTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/DocsIn/TicketDocsInTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TITE", FeatureId = TicketFeature7.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketEscalationTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.TicketEscalation" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/Escalation/TicketEscalationTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIAU", FeatureId = TicketFeature8.Id, ControlPath = "Logitude.CRM.Views.Tabs.Ticket.TicketAutomationTabControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.Audit" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 6, HtmlComponentUrl = "./CRMModules/CRMTickets/Components/EditTabs/Audit/TicketAuditTabComponent" }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TICM", FeatureId = TicketFeature9.Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.Communications" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TIEV", FeatureId = TicketFeature2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketObjectTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 8, }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion 

            #endregion

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
            ObjectTable activityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Activity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable opportunityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable stageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Stage" && d.Tenant == 0).FirstOrDefault();
            ObjectTable closingObjectTable = objectContext.ObjectTables.Where(d => d.Name == "OpportunityClosingReason" && d.Tenant == 0).FirstOrDefault();
            ObjectTable OpportunityTypeObjectTable = objectContext.ObjectTables.Where(d => d.Name == "OpportunityType" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketTypeObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketType" && d.Tenant == 0).FirstOrDefault();

            ObjectTable TicketSeverityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketStageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketClassificationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketClassification" && d.Tenant == 0).FirstOrDefault();
            ObjectTable ticketObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Ticket" && d.Tenant == 0).FirstOrDefault();
            ObjectTable ticketEscalationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketEscalation" && d.Tenant == 0).FirstOrDefault(); 

            #endregion

            #region ObjectFields
            List<ObjectField> activityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Activity").ToList();
            List<ObjectField> opportunityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Opportunity").ToList();            
            List<ObjectField> stageObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Stage").ToList();
            List<ObjectField> closingObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "OpportunityClosingReason").ToList();
            List<ObjectField> QuestionnaireObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Questionnaire").ToList();
            List<ObjectField> OpportunityTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "OpportunityType").ToList();
            List<ObjectField> TicketTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketType").ToList();

            List<ObjectField> TicketSeverityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketSeverity").ToList();
            List<ObjectField> TicketStageObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketStage").ToList();
            List<ObjectField> TicketClassificationObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketClassification").ToList();

            List<ObjectField> ticketObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Ticket").ToList();
            List<ObjectField> ticketEscalationObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketEscalation").ToList();
            #endregion

            #region QueryGroups
            QueryGroup activityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ACTV", Name = "Activity" }, queryGroupRepository);
            QueryGroup opportunityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APPO", Name = "Opportunity" }, queryGroupRepository);
            QueryGroup stageQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "STAG", Name = "Stage" }, queryGroupRepository);
            QueryGroup closingQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "OPCR", Name = "OpportunityClosingReason" }, queryGroupRepository);
            QueryGroup QuestionnaireQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QUQG", Name = "Questionnaires" }, queryGroupRepository);
            QueryGroup OpportunityTypeQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "OTQG", Name = "OpportunityType" }, queryGroupRepository);
            QueryGroup TicketTypeQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TTQG", Name = "TicketType" }, queryGroupRepository);

            QueryGroup TicketSeverityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TSQG", Name = "TicketSeverity" }, queryGroupRepository);
            QueryGroup TicketStageQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TAQG", Name = "TicketStage" }, queryGroupRepository);
            QueryGroup TicketClassificationQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TCQG", Name = "TicketClassification" }, queryGroupRepository);

            QueryGroup ticketQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TKQG", Name = "Ticket" }, queryGroupRepository);
            QueryGroup ticketEscalationQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TEQG", Name = "TicketEscalation" }, queryGroupRepository);

            #endregion

            queryGroupRepository.SubmitChanges();
            
            #region Features
            Feature myOpenActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.MyOpenActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allOpenActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.AllOpenActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature myClosedActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.MyClosedActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allClosedActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.AllClosedActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.AllActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature meetingsSummaryFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.MeetingsSummary" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature cancelledActivitiesFeature = tenantFeatures.Where(d => d.Code == "Activity.Q.CancelledActivities" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            Feature myOpenOpportunitiesFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.MyOpenOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allOpenOpportunitiesFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.AllOpenOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature myClosedOpportunitiesFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.MyClosedOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allClosedOpportunitiesFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.AllClosedOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allOpportunitiesFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.AllOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature cancelledFeature = tenantFeatures.Where(d => d.Code == "Opportunity.Q.CancelledOpportunities" && d.FeatureTypeCode == "QUER").FirstOrDefault();            
            Feature openByStageFeature = tenantFeatures.Where(d => d.Code == "Opportunity.OpenByStage" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            Feature stageFeature = tenantFeatures.Where(d => d.Code == "ALLSTAGES" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature closingFeature = tenantFeatures.Where(d => d.Code == "ALLCLOSINGREASONS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature OpportunityTypeFeature = tenantFeatures.Where(d => d.Code == "ALLOPPORTUNITYTYPES" && d.FeatureTypeCode == "QUER").FirstOrDefault();
          
            Feature TicketTypeFeature = tenantFeatures.Where(d => d.Code == "ALLTICKETTYPES" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            Feature TicketSeverityFeature = tenantFeatures.Where(d => d.Code == "ALLTICKETSEVERITIES" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature TicketStageFeature = tenantFeatures.Where(d => d.Code == "ALLTICKETSTAGES" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature TicketClassificationFeature = tenantFeatures.Where(d => d.Code == "ALLTICKETCLASSIFICATIONS" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            Feature allOpenTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.AllOpenTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ticketCancelledFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.AllCancelledTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature unassignedTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.UnassignedTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature allTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.AllTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature solvedTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.SolvedTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature sLAFailureTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.SLAFailureTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature recentlyUpdatedTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.RecentlyUpdatedTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature solvedSLAFailureTicketsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.SolvedSLAFailureTickets" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ticketEscalationsFeature = tenantFeatures.Where(d => d.Code == "Ticket.Q.AllTicketEscalations" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            #endregion

            #region Activity
            Query myOpenActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.MyOpenActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "My Open Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = myOpenActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allOpenActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.AllOpenActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "All Open Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 1, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = allOpenActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query myClosedActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.MyClosedActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "My Closed Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 2, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = myClosedActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allClosedActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.AllClosedActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "All Closed Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 3, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = allClosedActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.AllActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "All Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 4, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = allActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query meetingsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.MeetingsSummary" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "Meetings Summary", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 5, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = meetingsSummaryFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query cancelledActivitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Activity.Q.CancelledActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, Code = "Cancelled Activities", QueryGroupCode = activityQueryGroup.Code, IndexOrder = 6, Tenant = 0, ObjectTableId = activityObjectTable.Id, QuerySection = "Activity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = cancelledActivitiesFeature.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);

            QueryColumn myOpenActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter myOpenActivitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "MyOpenActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = myOpenActivitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allOpenActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityStatusName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 5, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenActivitiesQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenActivitiesQuery.Id, IndexOrder = 6, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allOpenActivitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "AllOpenActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allOpenActivitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn myClosedActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter myClosedActivitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "MyClosedActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = myClosedActivitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allClosedActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityStatusName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 5, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedActivitiesQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedActivitiesQuery.Id, IndexOrder = 6, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allClosedActivitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "AllClosedActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allClosedActivitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityStatusName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 5, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allActivitiesQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allActivitiesQuery.Id, IndexOrder = 6, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

            QueryColumn meetingsQueryQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn meetingsQueryQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn meetingsQueryQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn meetingsQueryQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn meetingsQueryQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "CompleteDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn meetingsQueryQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = meetingsQuery.Id, IndexOrder = 5, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "MeetingSummary" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter meetingsQueryQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "MeetingsSummary" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = meetingsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn cancelledActivitiesQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 0, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 1, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityTypeName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 2, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "ActivityStatusName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 3, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 4, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "PriorityName" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 5, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "StartDateTime" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn cancelledActivitiesQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cancelledActivitiesQuery.Id, IndexOrder = 6, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter cancelledActivitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = activityObjectFields.Where(d => d.FieldName == "CancelledActivities" && d.ObjectTableId == activityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = cancelledActivitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            #endregion

            #region Opportunity
            // SpotlightDataTemplate = "OpportunitySpotlightDataTemplate"
            //myOpenOpportunityQuery
            //allOpenOpportunityQuery
            //allOpportunityQuery
            //OpenOpportunitiesByStageQuery

            Query myOpenOpportunityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.MyOpenOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "My Open Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = myOpenOpportunitiesFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allOpenOpportunityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.AllOpenOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "All Open Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 1, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allOpenOpportunitiesFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query myClosedOpportunityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.MyClosedOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "My Closed Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 2, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = myClosedOpportunitiesFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allClosedOpportunityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.AllClosedOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "All Closed Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 3, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allClosedOpportunitiesFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allOpportunityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.AllOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "All Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 4, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allOpportunitiesFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query allCancelledOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.CancelledOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "Cancelled Opportunities", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 4, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = cancelledFeature.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            Query OpenOpportunitiesByStageQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Opportunity.Q.OpenByStage" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, Code = "Open By Stage", QueryGroupCode = opportunityQueryGroup.Code, IndexOrder = 5, Tenant = 0, ObjectTableId = opportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = openByStageFeature.Id, DefaultSortName = "StageDueDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);

            QueryColumn myOpenOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myOpenOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myOpenOpportunityQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter myOpenOpportunityQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "MyOpenOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = myOpenOpportunityQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allOpenOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenOpportunityQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allOpenOpportunityQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "AllOpenOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allOpenOpportunityQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn myClosedOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn myClosedOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = myClosedOpportunityQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter myClosedOpportunityQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "MyClosedOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = myClosedOpportunityQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allClosedOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allClosedOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allClosedOpportunityQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allClosedOpportunityQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "AllClosedOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allClosedOpportunityQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpportunityQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpportunityQuery.Id, IndexOrder = 7, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

            QueryColumn openByStageQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageDueDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns); ;
            QueryColumn openByStageQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "LastStageDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityTypeCode" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 7, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 8, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "NextActivityTypeCode" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn openByStageQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenOpportunitiesByStageQuery.Id, IndexOrder = 9, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "NextActivityDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter openByStageQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "OpenByStageOpp" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = OpenOpportunitiesByStageQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            QueryColumn allCancelledOpportunityQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 0, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 1, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 2, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 3, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 4, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 5, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 6, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledOpportunityQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledOpportunitiesQuery.Id, IndexOrder = 7, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter cancelledOpportunitiesQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = opportunityObjectFields.Where(d => d.FieldName == "CancelledOpportunities" && d.ObjectTableId == opportunityObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allCancelledOpportunitiesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion
                        
            #region Stage
            Query stageQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Stage.Q.AllStages" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault().Id, Code = "All Stages", QueryGroupCode = stageQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = stageObjectTable.Id, QuerySection = "Stage", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = stageFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn stageQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = stageQuery.Id, IndexOrder = 0, ObjectFieldId = stageObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn stageQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = stageQuery.Id, IndexOrder = 1, ObjectFieldId = stageObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn stageQueryColumn03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = stageQuery.Id, IndexOrder = 2, ObjectFieldId = stageObjectFields.Where(d => d.FieldName == "MaxDays" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn stageQueryColumn04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = stageQuery.Id, IndexOrder = 3, ObjectFieldId = stageObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == stageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Closing Reason
            Query closingQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityClosingReason.Q.AllClosingReasons" && d.ObjectTableId == closingObjectTable.Id).FirstOrDefault().Id, Code = "All Closing Reasons", QueryGroupCode = closingQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = closingObjectTable.Id, QuerySection = "OpportunityClosingReason", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = closingFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn closingQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = closingQuery.Id, IndexOrder = 0, ObjectFieldId = closingObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == closingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn closingQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = closingQuery.Id, IndexOrder = 1, ObjectFieldId = closingObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == closingObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Questionnaire
            ObjectTable QuestionnaireObject = objectContext.ObjectTables.Where(d => d.Name == "Questionnaire" && d.Tenant == 0).FirstOrDefault();
            Feature QuestionnairesFeature_01 = tenantFeatures.Where(d => d.Code == "Questionnaire.Q.AllQuestionnaires" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            Query AllQuestionnairesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Questionnaire.Q.AllQuestionnaires" && d.ObjectTableId == QuestionnaireObject.Id).FirstOrDefault().Id, IndexOrder = 6, Code = " All Questionnaires", QueryGroupCode = QuestionnaireQueryGroup.Code, Tenant = 0, ObjectTableId = QuestionnaireObject.Id, QuerySection = "Questionnaire", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuestionnairesFeature_01.Id, EditWizardComponentPath = "./CRMModules/CRMOthers/Components/Questionnaire/AddEditQuestionnaireComponent" }, queriesRepository, tenantQueries);
            QueryColumn AllQuestionnairesColumn1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuestionnairesQuery.Id, IndexOrder = 1, ObjectFieldId = QuestionnaireObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == QuestionnaireObject.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllQuestionnairesColumn2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuestionnairesQuery.Id, IndexOrder = 2, ObjectFieldId = QuestionnaireObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == QuestionnaireObject.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllQuestionnairesColumn3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuestionnairesQuery.Id, IndexOrder = 3, ObjectFieldId = QuestionnaireObjectFields.Where(d => d.FieldName == "VersionNumber" && d.ObjectTableId == QuestionnaireObject.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllQuestionnairesColumn4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuestionnairesQuery.Id, IndexOrder = 4, ObjectFieldId = QuestionnaireObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == QuestionnaireObject.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region OpportunityType
            Query OpportunityTypeQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "OpportunityType.Q.AllOpportunityTypes" && d.ObjectTableId == OpportunityTypeObjectTable.Id).FirstOrDefault().Id, Code = "All Opportunity Types", QueryGroupCode = OpportunityTypeQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = OpportunityTypeObjectTable.Id, QuerySection = "OpportunityType", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityTypeFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn OpportunityTypeQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpportunityTypeQuery.Id, IndexOrder = 0, ObjectFieldId = OpportunityTypeObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == OpportunityTypeObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpportunityTypeQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpportunityTypeQuery.Id, IndexOrder = 1, ObjectFieldId = OpportunityTypeObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == OpportunityTypeObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region TicketType
            Query TicketTypeQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketType.Q.AllTicketTypes" && d.ObjectTableId == TicketTypeObjectTable.Id).FirstOrDefault().Id, Code = "All Ticket Types", QueryGroupCode = TicketTypeQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = TicketTypeObjectTable.Id, QuerySection = "TicketType", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TicketTypeFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn TicketTypeQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketTypeQuery.Id, IndexOrder = 0, ObjectFieldId = TicketTypeObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketTypeObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn TicketTypeQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketTypeQuery.Id, IndexOrder = 1, ObjectFieldId = TicketTypeObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketTypeObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region TicketSeverity
            Query TicketSeverityQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketSeverity.Q.AllTicketSeverities" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().Id, Code = "All Ticket Severities", QueryGroupCode = TicketSeverityQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = TicketSeverityObjectTable.Id, QuerySection = "TicketSeverity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = TicketSeverityFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn TicketSeverityQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketSeverityQuery.Id, IndexOrder = 0, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn TicketSeverityQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketSeverityQuery.Id, IndexOrder = 1, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region TicketStage
            Query TicketStageQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketStage.Q.AllTicketStages" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault().Id, Code = "All Ticket Stages", QueryGroupCode = TicketStageQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = TicketStageObjectTable.Id, QuerySection = "TicketStage", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = TicketStageFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn TicketStageQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketStageQuery.Id, IndexOrder = 0, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn TicketStageQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketStageQuery.Id, IndexOrder = 1, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Ticket Classification
            Query TicketClassificationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "TicketClassification.Q.AllTicketClassifications" && d.ObjectTableId == TicketClassificationObjectTable.Id).FirstOrDefault().Id, Code = "All Ticket Classifications", QueryGroupCode = TicketClassificationQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = TicketClassificationObjectTable.Id, QuerySection = "TicketClassification", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TicketClassificationFeature.Id }, queriesRepository, tenantQueries);
            QueryColumn TicketClassificationQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketClassificationQuery.Id, IndexOrder = 0, ObjectFieldId = TicketClassificationObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketClassificationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn TicketClassificationQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TicketClassificationQuery.Id, IndexOrder = 1, ObjectFieldId = TicketClassificationObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketClassificationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Ticket 
            Query allOpenTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.AllOpenTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "All Open Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allOpenTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn allOpenTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allOpenTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allOpenTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allOpenTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MyAllOpenTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allOpenTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            Query unassignedTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.UnassignedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "Unassigned Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 1, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = unassignedTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn unassignedTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn unassignedTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = unassignedTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter unassignedTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MyUnassignedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = unassignedTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            Query allTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.AllTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "All Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 2, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn allTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            Query solvedTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.SolvedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "Solved Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 3, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = solvedTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn solvedTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter solvedTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MySolvedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = solvedTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            Query solvedSLAFailureTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.SolvedSLAFailureTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "Solved with SLA Failures", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 4, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = solvedSLAFailureTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn solvedSLAFailureTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn solvedSLAFailureTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = solvedSLAFailureTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter solvedSLAFailureTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MySolvedSLATickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = solvedSLAFailureTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);


            Query sLAFailureTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.SLAFailureTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "SLA Failures", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 5, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = sLAFailureTicketsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn sLAFailureTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn sLAFailureTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = sLAFailureTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter sLAFailureTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MySLAFailures" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = sLAFailureTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            Query allCancelledTicketsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.AllCancelledTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "All Cancelled Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 6, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ticketCancelledFeature.Id , DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn allCancelledTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allCancelledTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allCancelledTicketsQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter cancelledTicketsQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "IsCancelled" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = allCancelledTicketsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            Query RecentlyUpdatedTicketQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.RecentlyUpdatedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, Code = "Recently Updated Tickets", QueryGroupCode = ticketQueryGroup.Code, IndexOrder = 7, Tenant = 0, ObjectTableId = ticketObjectTable.Id, QuerySection = "Ticket", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = recentlyUpdatedTicketsFeature.Id, DefaultSortName = "UpdateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn RecentlyUpdatedTicketQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 0, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 1, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 2, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 3, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MainClassificationName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 4, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 5, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 6, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "SeverityName" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 7, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 8, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResponseTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_09 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 9, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "TicketFirstResolveTime" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn RecentlyUpdatedTicketQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RecentlyUpdatedTicketQuery.Id, IndexOrder = 10, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "EntityNumber" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter RecentlyUpdatedTicketQueryFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ticketObjectFields.Where(d => d.FieldName == "MyRecentlyUpdatedTickets" && d.ObjectTableId == ticketObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = RecentlyUpdatedTicketQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

            #endregion 

            #region Ticket Escalation
            Query allTicketEscalationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Ticket.Q.AllTicketEscalations" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, Code = "All Ticket Escalations", QueryGroupCode = ticketEscalationQueryGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = ticketEscalationObjectTable.Id, QuerySection = "TicketEscalation", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ticketEscalationsFeature.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
            QueryColumn allTicketEscalationQueryColumn_00 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 0, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "LineNumber" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 1, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 2, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "EscalationForName" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 3, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "Recepients" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_04 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 4, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_05 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 5, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "IsSLAViolated" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_06 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 6, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_07 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 7, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "IsClose" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allTicketEscalationQueryColumn_08 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allTicketEscalationQuery.Id, IndexOrder = 8, ObjectFieldId = ticketEscalationObjectFields.Where(d => d.FieldName == "CloseDate" && d.ObjectTableId == ticketEscalationObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            #endregion 

            objectContext.SaveChanges();
        }

        public void LoadOtherFields(IWebFreightContext context)
        {
            objectContext = context;
            textCodeRepository = new TextCodeRepository(objectContext);
            
            Dictionary<string, TextCode> textcodes = textCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);

            #region ObjectTables
            ObjectTable activityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Activity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable opportunityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable stageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Stage" && d.Tenant == 0).FirstOrDefault();
            ObjectTable closingObjectTable = objectContext.ObjectTables.Where(d => d.Name == "OpportunityClosingReason" && d.Tenant == 0).FirstOrDefault();
            ObjectTable QuestionnaireTable = objectContext.ObjectTables.Where(f => f.Name == "Questionnaire" && f.Tenant == 0).FirstOrDefault();
            ObjectTable OpportunityTypeObjectTable = objectContext.ObjectTables.Where(d => d.Name == "OpportunityType" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketTypeObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketType" && d.Tenant == 0).FirstOrDefault();

            ObjectTable TicketStageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketSeverityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketClassificationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketClassification" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Ticket" && d.Tenant == 0).FirstOrDefault();
            ObjectTable TicketEscalationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketEscalation" && d.Tenant == 0).FirstOrDefault();
            ObjectTable SLAHeaderObjectTable = objectContext.ObjectTables.Where(d => d.Name == "SLAHeader" && d.Tenant == 0).FirstOrDefault();
            ObjectTable BusinessHourObjectTable = objectContext.ObjectTables.Where(d => d.Name == "BusinessHour" && d.Tenant == 0).FirstOrDefault();
            ObjectTable EmployeeGroupObjectTable = objectContext.ObjectTables.Where(d => d.Name == "EmployeeGroup" && d.Tenant == 0).FirstOrDefault();
            #endregion

            #region TableDescription

            TextCode QuestionnaireTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Questionnaire.O.TableDescription", DefaultText = "Create a customized page for entering  customer details  when creating a new customer from a potential customer.", ObjectTableId = QuestionnaireTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            QuestionnaireTable.DescriptionTextCodeId = QuestionnaireTc.Id;
            QuestionnaireTable.DescriptionTextCodeCode = QuestionnaireTc.Code;

            TextCode StageTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "stage.O.TableDescription", DefaultText = "Define the stages of the sales process for CRM opportunities, including the probability percents.", ObjectTableId = stageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            stageObjectTable.DescriptionTextCodeId = StageTc.Id;
            stageObjectTable.DescriptionTextCodeCode = StageTc.Code;

            TextCode OpportunityClosingReasonTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityClosingReason.O.TableDescription", DefaultText = "Define the reasons for closing  opportunities.", ObjectTableId = closingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            closingObjectTable.DescriptionTextCodeId = OpportunityClosingReasonTc.Id;
            closingObjectTable.DescriptionTextCodeCode = OpportunityClosingReasonTc.Code;

            TextCode TicketTypeTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketType.O.TableDescription", DefaultText = "Define the ticket types to be used when closing tickets.", ObjectTableId = TicketTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            TicketTypeObjectTable.DescriptionTextCodeId = TicketTypeTc.Id;
            TicketTypeObjectTable.DescriptionTextCodeCode = TicketTypeTc.Code;

            TextCode TicketStageTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.O.TableDescription", DefaultText = "Define the stages of the ticket handling process in the system.", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            TicketStageObjectTable.DescriptionTextCodeId = TicketStageTc.Id;
            TicketStageObjectTable.DescriptionTextCodeCode = TicketStageTc.Code;

            TextCode SLAHeaderTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "SLAHeader.O.TableDescription", DefaultText = "Define the service level agreement based on severity and escalation management conditions.", ObjectTableId = SLAHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            SLAHeaderObjectTable.DescriptionTextCodeId = SLAHeaderTc.Id;
            SLAHeaderObjectTable.DescriptionTextCodeCode = SLAHeaderTc.Code;

            TextCode BusinessHourTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BusinessHour.O.TableDescription", DefaultText = "Define business hours and holidays.", ObjectTableId = BusinessHourObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            BusinessHourObjectTable.DescriptionTextCodeId = BusinessHourTc.Id;
            BusinessHourObjectTable.DescriptionTextCodeCode = BusinessHourTc.Code;

            TextCode TicketSeverityTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.O.TableDescription", DefaultText = "Define the levels of severity to be used in ticket processing.", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            TicketSeverityObjectTable.DescriptionTextCodeId = TicketSeverityTc.Id;
            TicketSeverityObjectTable.DescriptionTextCodeCode = TicketSeverityTc.Code;

            TextCode TicketClassificationTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketClassification.O.TableDescription", DefaultText = "Add the categories according to which tickets will be classified, in hierarchy tree pattern. Each branch in the tree can be defined with Employee Group, Default Severity, Manager and Notify users.", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            TicketClassificationObjectTable.DescriptionTextCodeId = TicketClassificationTc.Id;
            TicketClassificationObjectTable.DescriptionTextCodeCode = TicketClassificationTc.Code;

            TextCode EmployeeGroupTc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "EmployeeGroup.O.TableDescription", DefaultText = "Define the employee groups to be used in classifications and the SLA table. Each group can be defined with Manager, Notify users and the list of group members.", ObjectTableId = EmployeeGroupObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            EmployeeGroupObjectTable.DescriptionTextCodeId = EmployeeGroupTc.Id;
            EmployeeGroupObjectTable.DescriptionTextCodeCode = EmployeeGroupTc.Code;
            #endregion

            #region Queries
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.MyOpenActivities", DefaultText = "My Open Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.AllOpenActivities", DefaultText = "All Open Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.MyClosedActivities", DefaultText = "My Closed Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.AllClosedActivities", DefaultText = "All Closed Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.AllActivities", DefaultText = "All Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.MeetingsSummary", DefaultText = "Meetings Summary", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Activity.Q.CancelledActivities", DefaultText = "Cancelled Activities", ObjectTableId = activityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.MyOpenOpportunities", DefaultText = "My Open Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllOpenOpportunities", DefaultText = "All Open Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.MyClosedOpportunities", DefaultText = "My Closed Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllClosedOpportunities", DefaultText = "All Closed Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllOpportunities", DefaultText = "All Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.CancelledOpportunities", DefaultText = "Cancelled Opportunities", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.OpenByStage", DefaultText = "Open Opportunities by Stage", ObjectTableId = opportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Stage.Q.AllStages", DefaultText = "Stages", ObjectTableId = stageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityClosingReason.Q.AllClosingReasons", DefaultText = "Closing Reasons", ObjectTableId = closingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Questionnaire.Q.AllQuestionnaires", DefaultText = "All Questionnaire ", ObjectTableId = QuestionnaireTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "OpportunityType.Q.AllOpportunityTypes", DefaultText = "Opportunity Types", ObjectTableId = OpportunityTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketType.Q.AllTicketTypes", DefaultText = "Ticket Types", ObjectTableId = TicketTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.Q.AllTicketSeverities", DefaultText = "Ticket Severities", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.Q.AllTicketStages", DefaultText = "Ticket Stages", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketClassification.Q.AllTicketClassifications", DefaultText = "Ticket Classifications", ObjectTableId = TicketClassificationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.AllOpenTickets", DefaultText = "All Open Tickets", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.AllCancelledTickets", DefaultText = "Cancelled Tickets", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.UnassignedTickets", DefaultText = "Unassigned Tickets", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.AllTickets", DefaultText = "All Tickets", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.SolvedTickets", DefaultText = "All Solved\\Closed Tickets", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.SLAFailureTickets", DefaultText = "SLA Open Failures", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.RecentlyUpdatedTickets", DefaultText = "Recently Updated", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.SolvedSLAFailureTickets", DefaultText = "SLA Solved\\Closed Failures", ObjectTableId = TicketObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Ticket.Q.AllTicketEscalations", DefaultText = "Ticket Escalations", ObjectTableId = TicketEscalationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            #endregion

            objectContext.SaveChanges();
        }
        
        public void LoadEventTypes()
        {
            objectContext = WebFreightContext.GetContext(0);            
            eventTypesRepository = new EventTypeRepository(objectContext);            
            Dictionary<string, EventType> tenantEventTypes = eventTypesRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);

            #region Opportunity events
            ObjectTablePM opportunityObject = ObjectTableQuery.GetObjectTableByCode("Opportunity", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CROP",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPOP",
                EnglishName = "Opportunity Updated",
                Tenant = 0,
                LocalName = "Opportunity Updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OEMO",
                EnglishName = "Opportunity email out sent",
                Tenant = 0,
                LocalName = "Email out sent",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ROOP",
                EnglishName = "Opportunity re-opened",
                Tenant = 0,
                LocalName = "Opportunity re-opened",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "COOP",
                EnglishName = "Copied",
                Tenant = 0,
                LocalName = "Copied",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CLOP",
                EnglishName = "Closed As Lost",
                Tenant = 0,
                LocalName = "Closed As Lost",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CCOP",
                EnglishName = "Closed As Lost To Competitor",
                Tenant = 0,
                LocalName = "Closed As Lost To Competitor",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CWOP",
                EnglishName = "Closed As Won",
                Tenant = 0,
                LocalName = "Closed As Won",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CAOP",
                EnglishName = "Cancelled",
                Tenant = 0,
                LocalName = "Cancelled",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PROP",
                EnglishName = "Products Updated",
                Tenant = 0,
                LocalName = "Products Updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CMOP",
                EnglishName = "Competitors Updated",
                Tenant = 0,
                LocalName = "Competitors Updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ADOP",
                EnglishName = "Additional Services Updated",
                Tenant = 0,
                LocalName = "Additional Services Updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTOP",
                EnglishName = "Quotes Updated",
                Tenant = 0,
                LocalName = "Quotes Updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OACR",
                EnglishName = "Activity Created",
                Tenant = 0,
                LocalName = "Activity Created",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OACM",
                EnglishName = "Activity Completed",
                Tenant = 0,
                LocalName = "Activity Completed",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OARP",
                EnglishName = "Activity Reopened",
                Tenant = 0,
                LocalName = "Activity Reopened",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPSG",
                EnglishName = "Stage Due Date updated",
                Tenant = 0,
                LocalName = "Stage Due Date updated",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPFO",
                EnglishName = "Opportunity Followed",
                Tenant = 0,
                LocalName = "Opportunity Followed",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPST",
                EnglishName = "Stage Changed",
                Tenant = 0,
                LocalName = "Stage Changed",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPMO",
                Tenant = 0,
                EnglishName = "Email out added",
                LocalName = "Email out added",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPMI",
                Tenant = 0,
                EnglishName = "Email in added",
                LocalName = "Email in added",
                ObjectTableId = opportunityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            #endregion

            #region Activity events

            ObjectTablePM activityObject = ObjectTableQuery.GetObjectTableByCode("Activity", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAV",
                EnglishName = "Activity Updated",
                Tenant = 0,
                LocalName = "Activity Updated",
                ObjectTableId = activityObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAV",
                EnglishName = "Activity Created",
                Tenant = 0,
                LocalName = "Activity Created",
                ObjectTableId = activityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "COAV",
                EnglishName = "Completed",
                Tenant = 0,
                LocalName = "Completed",
                ObjectTableId = activityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ROAV",
                EnglishName = "Reopened",
                Tenant = 0,
                LocalName = "Reopened",
                ObjectTableId = activityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CAAV",
                EnglishName = "Cancelled",
                Tenant = 0,
                LocalName = "Cancelled",
                ObjectTableId = activityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "COAC",
                EnglishName = "Copied",
                Tenant = 0,
                LocalName = "Copied",
                ObjectTableId = activityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            #endregion

            #region Stage
            ObjectTablePM StageObject = ObjectTableQuery.GetObjectTableByCode("Stage", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPSG",
                EnglishName = "Stage Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Stage Updated",
                ObjectTableId = StageObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRSG",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = StageObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            #region OpportunityClosingReason
            ObjectTablePM OpportunityClosingReasonObject = ObjectTableQuery.GetObjectTableByCode("OpportunityClosingReason", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCL",
                EnglishName = "Closing Reason Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Closing Reason Updated",
                ObjectTableId = OpportunityClosingReasonObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCL",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = OpportunityClosingReasonObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            #region OpportunityType
            ObjectTablePM OpportunityTypeObject = ObjectTableQuery.GetObjectTableByCode("OpportunityType", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPOT",
                EnglishName = "Opportunity Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Opportunity Type Updated",
                ObjectTableId = OpportunityTypeObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CROT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = OpportunityTypeObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            #region Ticket Type 
            ObjectTablePM ticketTypeObject = ObjectTableQuery.GetObjectTableByCode("TicketType", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTT",
                EnglishName = "Ticket Type Updated",
                Tenant = 0,
                LocalName = "Ticket Type Updated",
                ObjectTableId = ticketTypeObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTT",
                EnglishName = "Ticket Type Created",
                Tenant = 0,
                LocalName = "Ticket Type Created",
                ObjectTableId = ticketTypeObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACTT",
                EnglishName = "Ticket Type Activated",
                Tenant = 0,
                LocalName = "Ticket Type Activated",
                ObjectTableId = ticketTypeObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "IATT",
                EnglishName = "Ticket Type Inactivated",
                Tenant = 0,
                LocalName = "Ticket Type Inactivated",
                ObjectTableId = ticketTypeObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion 

            #region Ticket Stage
            ObjectTablePM ticketStageObject = ObjectTableQuery.GetObjectTableByCode("TicketStage", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTS",
                EnglishName = "Ticket Stage Updated",
                Tenant = 0,
                LocalName = "Ticket Stage Updated",
                ObjectTableId = ticketStageObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTS",
                EnglishName = "Ticket Stage Created",
                Tenant = 0,
                LocalName = "Ticket Stage Created",
                ObjectTableId = ticketStageObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACTS",
                EnglishName = "Ticket Stage Activated",
                Tenant = 0,
                LocalName = "Ticket Stage Activated",
                ObjectTableId = ticketStageObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "IATS",
                EnglishName = "Ticket Stage Inactivated",
                Tenant = 0,
                LocalName = "Ticket Stage Inactivated",
                ObjectTableId = ticketStageObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion 

            #region Ticket Classification
            ObjectTablePM ticketClassificationObject = ObjectTableQuery.GetObjectTableByCode("TicketClassification", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTC",
                EnglishName = "Ticket Classification Updated",
                Tenant = 0,
                LocalName = "Ticket Classification Updated",
                ObjectTableId = ticketClassificationObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTC",
                EnglishName = "Ticket Classification Created",
                Tenant = 0,
                LocalName = "Ticket Classification Created",
                ObjectTableId = ticketClassificationObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACTC",
                EnglishName = "Ticket Classification Activated",
                Tenant = 0,
                LocalName = "Ticket Classification Activated",
                ObjectTableId = ticketClassificationObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "IATC",
                EnglishName = "Ticket Classification Inactivated",
                Tenant = 0,
                LocalName = "Ticket Classification Inactivated",
                ObjectTableId = ticketClassificationObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion 

            #region Ticket Severity
            ObjectTablePM ticketSeverityObject = ObjectTableQuery.GetObjectTableByCode("TicketSeverity", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTE",
                EnglishName = "Ticket Severity Updated",
                Tenant = 0,
                LocalName = "Ticket Severity Updated",
                ObjectTableId = ticketSeverityObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTE",
                EnglishName = "Ticket Severity Created",
                Tenant = 0,
                LocalName = "Ticket Severity Created",
                ObjectTableId = ticketSeverityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACTE",
                EnglishName = "Ticket Severity Activated",
                Tenant = 0,
                LocalName = "Ticket Severity Activated",
                ObjectTableId = ticketSeverityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "IATE",
                EnglishName = "Ticket Severity Inactivated",
                Tenant = 0,
                LocalName = "Ticket Severity Inactivated",
                ObjectTableId = ticketSeverityObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);
            #endregion 

            #region Ticket
            ObjectTablePM ticketObject = ObjectTableQuery.GetObjectTableByCode("Ticket", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTK",
                EnglishName = "Ticket Updated",
                Tenant = 0,
                LocalName = "Ticket Updated",
                ObjectTableId = ticketObject.Id,
                ShortView = false,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTK",
                EnglishName = "Ticket Created",
                Tenant = 0,
                LocalName = "Ticket Created",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CATK",
                EnglishName = "Ticket Cancelled",
                Tenant = 0,
                LocalName = "Ticket Cancelled",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RATK",
                EnglishName = "Ticket Reactivated",
                Tenant = 0,
                LocalName = "Ticket Reactivated",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CLTK",
                EnglishName = "Ticket Closed",
                Tenant = 0,
                LocalName = "Ticket Closed",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ESTK",
                EnglishName = "Escalation Sent",
                Tenant = 0,
                LocalName = "Escalation Sent",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SCTK",
                EnglishName = "Ticket Stage Changed",
                Tenant = 0,
                LocalName = "Ticket Stage Changed",
                ObjectTableId = ticketObject.Id,
                ShortView = true,
            }, eventTypesRepository, tenantEventTypes);

            #endregion 

            eventTypesRepository.SubmitChanges();
        }

        public void LoadOpportunityClosingReasons()
        {
            OpportunityClosingReasonRepository repository = new OpportunityClosingReasonRepository(0);
            Dictionary<string, OpportunityClosingReason> tenantClosingReasons = repository.GetAll(0).ToDictionary(d => d.Code, a => a);

            AddClosingReasons.AddClosingReason(new ClosingReasonDetails() { Code = "CA", Name = "Cancelled", LocalName = "Cancelled", AddedManually = false, SearchFields = "Cancelled,Cancelled", IsClosedLost = true }, repository, tenantClosingReasons);
            AddClosingReasons.AddClosingReason(new ClosingReasonDetails() { Code = "LC", Name = "Lost To Competition", LocalName = "Lost To Competition", AddedManually = false, SearchFields = "Lost To Competition,Lost To Competition", IsClosedLost = true }, repository, tenantClosingReasons);
            AddClosingReasons.AddClosingReason(new ClosingReasonDetails() { Code = "LO", Name = "Lost", LocalName = "Lost", AddedManually = false, SearchFields = "Lost,Lost", IsClosedLost = true }, repository, tenantClosingReasons);
            AddClosingReasons.AddClosingReason(new ClosingReasonDetails() { Code = "WN", Name = "Won", LocalName = "Won", AddedManually = false, SearchFields = "Won,Won", IsClosedLost = false }, repository, tenantClosingReasons);

            repository.SubmitChanges();
        }

        public void LoadOpportunityTypes()
        {
            OpportunityTypeRepository repository = new OpportunityTypeRepository(0);
            Dictionary<string, OpportunityType> tenantOpportunityTypes = repository.GetAll(0).ToDictionary(d => d.Code, a => a);

            AddOpportunityTypes.AddOpportunityType(new OpportunityTypeDetails() { Code = "N", Name = "New Business", SearchFields = "N,New Business" }, repository, tenantOpportunityTypes);
            AddOpportunityTypes.AddOpportunityType(new OpportunityTypeDetails() { Code = "R", Name = "Bid", SearchFields = "R,Bid" }, repository, tenantOpportunityTypes);
            AddOpportunityTypes.AddOpportunityType(new OpportunityTypeDetails() { Code = "E", Name = "Expansion", SearchFields = "E,Expansion" }, repository, tenantOpportunityTypes);
            AddOpportunityTypes.AddOpportunityType(new OpportunityTypeDetails() { Code = "T", Name = "Routing Order", SearchFields = "T,Routing Order" }, repository, tenantOpportunityTypes);

            repository.SubmitChanges();
        }
    }
}