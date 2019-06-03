using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate
{
    public class AccountingUpdate
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
        public CounterDefinitionRepository CounterDefinitionRepository;
        private EventTypeRepository eventTypesRepository;
        #endregion

        private ObjectTableQuery objectTabelQuery;
        private MenuButtonGroupQuery menuButtonGroupQuery;


        //  ________________________________
        // |                                |
        // |          CLOSED TABLES         |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V

        #region Upgrade Closed Tables


        public void LoadBaseTablesForDataBases()
        {
            GlobalDBRepository globalDbRep = new GlobalDBRepository();
            List<GlobalDB> dbList = globalDbRep.GetGlobalDBs().ToList();

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }
       
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

        // ------ Closed Tables Data -------- //
        private void LoadBaseTablesForConnection(string connectionStr)
        {
            AccountingContext accountingContext = new AccountingContext(DatabaseInitializer.GetConnection(connectionStr));

            // Journal Status
            JournalStatusTypeRepository journalStatusTypeRepository = new JournalStatusTypeRepository(accountingContext);
            AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "0", EnglishName = "Draft", LocalName = "פתוח" }, journalStatusTypeRepository);
            AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "1", EnglishName = "Waiting for Approval", LocalName = "מחכה לאישור" }, journalStatusTypeRepository);
            AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "2", EnglishName = "Approved", LocalName = "מאושר" }, journalStatusTypeRepository);
            AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "3", EnglishName = "Voided", LocalName = "מבוטל" }, journalStatusTypeRepository);
            AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "4", EnglishName = "Failed", LocalName = "נכשל" }, journalStatusTypeRepository);
            journalStatusTypeRepository.SubmitChanges();

            // JournalType
            JournalTypeRepository journalTypeRepository = new JournalTypeRepository(accountingContext);
            AddClosedTables.AddJournalType(new JournalTypeDetails() { JournalTypeID = "0", EnglishName = "Regular", LocalName = "רגיל" }, journalTypeRepository);
            AddClosedTables.AddJournalType(new JournalTypeDetails() { JournalTypeID = "1", EnglishName = "Template ", LocalName = "תבנית" }, journalTypeRepository);

            journalTypeRepository.SubmitChanges();

            // ChartOfAccountsTypes
            ChartOfAccountsTypeRepository chartOfAccountsTypeRepository = new ChartOfAccountsTypeRepository(accountingContext);
            //AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "0", EnglishName = "????", LocalName = "????" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "1", EnglishName = "Revenues", LocalName = "הכנסות" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "2", EnglishName = "Expenses", LocalName = "הוצאות" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "3", EnglishName = "Customers", LocalName = "לקוחות" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "4", EnglishName = "Vendors", LocalName = "ספקים" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "5", EnglishName = "Banks", LocalName = "בנקים" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "6", EnglishName = "Workers", LocalName = "עובדים" }, chartOfAccountsTypeRepository);
            AddClosedTables.AddChartOfAccountsType(new ChartOfAccountsTypeDetails() { Code = "7", EnglishName = "Debtors And Creditors", LocalName = "חו\"זים" }, chartOfAccountsTypeRepository);
            chartOfAccountsTypeRepository.SubmitChanges();

            // GLAccountTypes
            GLAccountTypeRepository glAccountTypeRepository = new GLAccountTypeRepository(accountingContext);
            AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "1", EnglishName = "Card", LocalName = "כרטיס" }, glAccountTypeRepository);
            AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "2", EnglishName = "Client", LocalName = "לקוח" }, glAccountTypeRepository);
            AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "3", EnglishName = "Vendor", LocalName = "ספק" }, glAccountTypeRepository);
            AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "4", EnglishName = "Job", LocalName = "ג'וב" }, glAccountTypeRepository);
            AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "5", EnglishName = "File", LocalName = "תיק" }, glAccountTypeRepository);
            glAccountTypeRepository.SubmitChanges();

            // RevenueExpenseTypes
            RevenueExpenseTypeRepository revenueExpenseRepository = new RevenueExpenseTypeRepository(accountingContext);
            AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "1", EnglishName = "Revenue", LocalName = "הכנסות" }, revenueExpenseRepository);
            AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "2", EnglishName = "Expense", LocalName = "הוצאות" }, revenueExpenseRepository);
            AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "3", EnglishName = "Other", LocalName = "אחר" }, revenueExpenseRepository);
            revenueExpenseRepository.SubmitChanges();

            // JournalActionTypes
            JournalActionTypeRepository journalActionTypeRepository = new JournalActionTypeRepository(accountingContext);
            AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "1", JournalActionTypeID = "1", Tenant = 0, EnglishName = "Credit", LocalName = "זכות" }, journalActionTypeRepository);
            AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "2", JournalActionTypeID = "2", Tenant = 0, EnglishName = "Debit ", LocalName = "חובה" }, journalActionTypeRepository);
            AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "3", JournalActionTypeID = "3", Tenant = 0, EnglishName = "Debit And Credit", LocalName = "חובה+זכות" }, journalActionTypeRepository);
            AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "4", JournalActionTypeID = "4", Tenant = 0, EnglishName = "Debit, Credit And Vat deduction ", LocalName = "חובה + זכות + חילוץ מעמ" }, journalActionTypeRepository);

            journalActionTypeRepository.SubmitChanges();

            // ReconcileMethods
            ReconcileMethodRepository reconcileMethodRepository = new ReconcileMethodRepository(accountingContext);
            AddClosedTables.AddReconcileMethod(new ReconcileMethodDetails() { Code = "0", EnglishName = "Local Currency", LocalName = "מטבע מקומי" }, reconcileMethodRepository);
            AddClosedTables.AddReconcileMethod(new ReconcileMethodDetails() { Code = "1", EnglishName = "Foreign Currency", LocalName = "מטבע חוץ" }, reconcileMethodRepository);
            reconcileMethodRepository.SubmitChanges();

            // AutomaticReconciles
            AutomaticReconcileRepository automaticReconcileRepository = new AutomaticReconcileRepository(accountingContext);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "1", EnglishName = "Open Amount", LocalName = "סכום פתוח" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "2", EnglishName = "Reference Date", LocalName = "תאריך אסמכתא" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "3", EnglishName = "Due Date", LocalName = "תאריך ערך" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "4", EnglishName = "Accounting Date", LocalName = "תאריך חשבונאי" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "5", EnglishName = "Reference 1", LocalName = "אסמכתא 1" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "6", EnglishName = "Reference 2", LocalName = "אסמכתא 2" }, automaticReconcileRepository);
            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "7", EnglishName = "Reference 3", LocalName = "אסמכתא 3" }, automaticReconcileRepository);
            automaticReconcileRepository.SubmitChanges();

            // PeriodTypes
            PeriodTypeRepository periodTypeRepository = new PeriodTypeRepository(accountingContext);
            AddClosedTables.AddPeriodType(new PeriodTypeDetails() { Code = "1", EnglishName = "Accounting", LocalName = "חשבונאות" }, periodTypeRepository);
            AddClosedTables.AddPeriodType(new PeriodTypeDetails() { Code = "2", EnglishName = "Invoice", LocalName = "חשבונית" }, periodTypeRepository);
            periodTypeRepository.SubmitChanges();

            // AccountingEntities
            AccountingEntityRepository accountingEntityRepository = new AccountingEntityRepository(accountingContext);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "1", EnglishName = "Journal", LocalName = "פקודת יומן" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "2", EnglishName = "ARInvoice", LocalName = "חשבונית לקוח" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "3", EnglishName = "ARPayment", LocalName = "קבלה לקוח" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "4", EnglishName = "APInvoice", LocalName = "חשבונית ספק" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "5", EnglishName = "APPayment", LocalName = "קבלה ספק" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "6", EnglishName = "Cheque Deposit", LocalName = "הפקדת המחאות" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "7", EnglishName = "Cash Deposit", LocalName = "הפקדת מזומן" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "8", EnglishName = "Revaluation", LocalName = "שערוך" }, accountingEntityRepository);
            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "9", EnglishName = "Payment Cheque", LocalName = "מערכת המחאות" }, accountingEntityRepository);

            AddClosedTables.AddAccountingEntity(new AccountingEntityDetails() { Code = "10", EnglishName = "Reconciliation", LocalName = "התאמה" }, accountingEntityRepository);


            accountingEntityRepository.SubmitChanges();

        }

        #endregion



        //  ________________________________
        // |                                |
        // |           MENU BUTTON          |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V

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
            //CreateMenuButtonsForTenant(0); // islam: this should be generated in each table update class

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


            string journalTableId = objectContext.ObjectTables.Where(f => f.Name == "Journal" && f.Tenant == tenant).FirstOrDefault().Id;
            string cashbookTableId = objectContext.ObjectTables.Where(f => f.Name == "CashBook" && f.Tenant == tenant).FirstOrDefault().Id;
            string glAccountTableId = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == tenant).FirstOrDefault().Id;
            string paymentChequeTableId = objectContext.ObjectTables.Where(f => f.Name == "PaymentCheque" && f.Tenant == tenant).FirstOrDefault().Id;
            string reconciliationTableId = objectContext.ObjectTables.Where(f => f.Name == "Reconciliation" && f.Tenant == tenant).FirstOrDefault().Id;
            string externalReconciliationTableId = objectContext.ObjectTables.Where(f => f.Name == "ExternalReconciliation" && f.Tenant == tenant).FirstOrDefault().Id;

            #region Journal Buttons

            FeaturePM journalFeature_More = features.Where(d => d.Code == "MOREJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            FeaturePM journalFeature_Save = features.Where(d => d.Code == "SAVEJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            FeaturePM journalFeature_SaveAsDraft = features.Where(d => d.Code == "SAVEASDRAFTJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            FeaturePM journalFeature_Approve = features.Where(d => d.Code == "APPROVEJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            FeaturePM journalFeature_Print = features.Where(d => d.Code == "PRINTJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();


            MenuButtonGroup journalMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "JournalEdit",
                Name = "JournalEditButtonsGroup",
                ObjectTableId = journalTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region Journal SaveAsDraftButton Button
            MenuButton JournalSaveAsDraftButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "JournalSaveAsDraft",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalSaveAsDraft",
                LabelTextCodeDefaultText = "Save As Draft",
                LocalDefaultText = "שמור כטיוטא",
                ObjectTableId = journalTableId,
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                //   ParentMenuButtonId = actionButton.Id,
                FeatureId = journalFeature_SaveAsDraft.Id,
                MenuButtonType = "button",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region JournalSave Button
            MenuButton JournalSaveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "JournalSave",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalSave",
                LabelTextCodeDefaultText = "Waiting For Approval",
                LocalDefaultText = "שמור",
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ObjectTableId = journalTableId,
                FeatureId = journalFeature_Save.Id,
                MenuButtonType = "button",
                Width = 150,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #region JournalApproval Button
            MenuButton JournalApprovalButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "JournalApprove",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalApprove",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אשר פקודת יומן",
                ObjectTableId = journalTableId,
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = journalFeature_Approve.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                Width = 150,
                // ControlPath = "Logitude.Customs.CustomsControls.SendOptionsControl",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion


            // More List
            #region More Button

            MenuButton JournalMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalMore",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "יותר",
                ObjectTableId = journalTableId,
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = journalFeature_More.Id,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Journal VoidButton Button

            MenuButton JournalVoidButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "JournalVoid",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalVoid",
                LabelTextCodeDefaultText = "Void",
                LocalDefaultText = "ביטול",
                ObjectTableId = journalTableId,
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ParentMenuButtonId = JournalMoreButton.Id,
                FeatureId = journalFeature_SaveAsDraft.Id,
                MenuButtonType = "menuitem",         
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Journal Print Button

            MenuButton JournalPrintButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "JournalPrint",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "Journal.B.JournalPrint",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפס",
                ObjectTableId = journalTableId,
                Tenant = tenant,
                MenuButtonGroupId = journalMenuButtonGroup.Id,
                ParentMenuButtonId = JournalMoreButton.Id,
                FeatureId = journalFeature_Print.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #endregion

            #region BankDeposit Buttons

            string bankDepositTableId = objectContext.ObjectTables.Where(f => f.Name == "BankDeposit" && f.Tenant == tenant).FirstOrDefault().Id;

            FeaturePM bankDepositFeature_Print = features.Where(d => d.Code == "BANKDEPOSITPRINT" && d.ObjectTableId == bankDepositTableId).FirstOrDefault();
            FeaturePM BankDeposit_Approve = features.Where(d => d.Code == "BANKDEPOSITAPRV" && d.ObjectTableId == bankDepositTableId).FirstOrDefault();
            
            MenuButtonGroup bankDepositMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "BankDepositEdit",
                Name = "BankDepositEditButtonsGroup",
                ObjectTableId = bankDepositTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region More Button
            FeaturePM feature_More = features.Where(d => d.Code == "MOREBNKDPST" && d.ObjectTableId == bankDepositTableId).FirstOrDefault();

            MenuButton BankDepositMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "BankDeposit.B.More",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "יותר",
                ObjectTableId = bankDepositTableId,
                Tenant = tenant,
                MenuButtonGroupId = bankDepositMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = feature_More.Id,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region BankDepositPrint Button
            MenuButton BankDepositPrintButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "BankDepositPrint",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "BankDeposit.B.BankDepositPrint",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפסה",
                ObjectTableId = bankDepositTableId,
                Tenant = tenant,
                MenuButtonGroupId = bankDepositMenuButtonGroup.Id,
                ParentMenuButtonId = BankDepositMoreButton.Id,
                FeatureId = bankDepositFeature_Print.Id,
                MenuButtonType = "menuitem",
                Style = "Ordinary",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #region Approve Button
            MenuButton BankDepositApprovalButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "BankDepositApprove",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "BankDeposit.B.Approve",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "לאשר",
                ObjectTableId = bankDepositTableId,
                Tenant = tenant,
                MenuButtonGroupId = bankDepositMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = BankDeposit_Approve.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                Width = 120,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion




            #region Cancel Button
            FeaturePM bnkdpFeature_cancel = features.Where(d => d.Code == "CancelDeposit" && d.ObjectTableId == bankDepositTableId).FirstOrDefault();

            MenuButton CancelDepositButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelDeposit",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "BankDeposit.B.Cancel",
                LabelTextCodeDefaultText = "Cancel deposit",
                LocalDefaultText = "ביטול הפקדה",
                ObjectTableId = bankDepositTableId,
                Tenant = tenant,
                MenuButtonGroupId = bankDepositMenuButtonGroup.Id,
                ParentMenuButtonId = BankDepositMoreButton.Id,
                FeatureId = bnkdpFeature_cancel.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion


            #endregion

            #region CashBook

            FeaturePM cashBookFeature_More = features.Where(d => d.Code == "MORECASHBOOK" && d.ObjectTableId == cashbookTableId).FirstOrDefault();
            FeaturePM cashBookFeature_InActivate = features.Where(d => d.Code == "INACITVE" && d.ObjectTableId == cashbookTableId).FirstOrDefault();
            FeaturePM cashBookFeature_Deposite = features.Where(d => d.Code == "DPSTCASHBOOK" && d.ObjectTableId == cashbookTableId).FirstOrDefault();


            MenuButtonGroup cashBookMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "CasBookEdit",
                Name = "CasBookEditButtonsGroup",
                ObjectTableId = cashbookTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region More Button

            MenuButton CashbookMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "CashBook.B.More",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "יותר",
                ObjectTableId = cashbookTableId,
                Tenant = tenant,
                MenuButtonGroupId = cashBookMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = cashBookFeature_More.Id,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Inactivate Button

            MenuButton InactiveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CashBookInactive",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "CashBook.B.Inactive",
                LabelTextCodeDefaultText = "Inactive",
                LocalDefaultText = "חסימה",
                ObjectTableId = cashbookTableId,
                Tenant = tenant,
                MenuButtonGroupId = cashBookMenuButtonGroup.Id,
                ParentMenuButtonId = CashbookMoreButton.Id,
                FeatureId = cashBookFeature_InActivate.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion  

            #region Deposit Button
            MenuButton DepositeButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CashBookDeposite",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "CashBook.B.Deposite",
                LabelTextCodeDefaultText = "Deposit",
                LocalDefaultText = "הפקדה",
                ObjectTableId = cashbookTableId,
                Tenant = tenant,
                MenuButtonGroupId = cashBookMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = cashBookFeature_Deposite.Id,
                MenuButtonType = "button",
                Width = 110,
                Style = "ApproveButtonStyle",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #endregion

            #region GLAccount

            FeaturePM glAccountFeature_More = features.Where(d => d.Code == "MOREGLACCOUNT" && d.ObjectTableId == glAccountTableId).FirstOrDefault();
            FeaturePM glAccountFeature_InActivate = features.Where(d => d.Code == "INACITVE" && d.ObjectTableId == glAccountTableId).FirstOrDefault();
            FeaturePM glAccountFeature_PrintCardIndex = features.Where(d => d.Code == "PRINTCARDINDEX" && d.ObjectTableId == glAccountTableId).FirstOrDefault();
            FeaturePM glAccountFeature_Reconcile = features.Where(d => d.Code == "RECOCILE" && d.ObjectTableId == glAccountTableId).FirstOrDefault();


            MenuButtonGroup glAccountMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "GLAEdit",
                Name = "GLAEditButtonsGroup",
                ObjectTableId = glAccountTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region More Button

            MenuButton GLAccountMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "GLAccount.B.More",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "יותר",
                ObjectTableId = glAccountTableId,
                Tenant = tenant,
                MenuButtonGroupId = glAccountMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = glAccountFeature_More.Id,
                MenuButtonType = "dropdownbutton",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Inactivate Button

            MenuButton GLAInactiveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "GLAccountInactive",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "GLAccount.B.Inactive",
                LabelTextCodeDefaultText = "Inactive",
                LocalDefaultText = "חסימה",
                ObjectTableId = glAccountTableId,
                Tenant = tenant,
                MenuButtonGroupId = glAccountMenuButtonGroup.Id,
                ParentMenuButtonId = GLAccountMoreButton.Id,
                FeatureId = glAccountFeature_InActivate.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion



            #region Print Card Index Button

            MenuButton GLAPrintCardIndexButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "GLAccountPrintCardIndex",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "GLAccount.B.PrintCardIndex",
                LabelTextCodeDefaultText = "Print Card Index",
                LocalDefaultText = "הדפסת כרטסת",
                ObjectTableId = glAccountTableId,
                Tenant = tenant,
                MenuButtonGroupId = glAccountMenuButtonGroup.Id,
                ParentMenuButtonId = GLAccountMoreButton.Id,
                FeatureId = glAccountFeature_PrintCardIndex.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Reconcile Button
            MenuButton GLAccountReconcileButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Reconcile",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "GLAccount.B.Reconcile",
                LabelTextCodeDefaultText = "Reconcile",
                LocalDefaultText = "התאם",
                ObjectTableId = glAccountTableId,
                Tenant = tenant,
                MenuButtonGroupId = glAccountMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = glAccountFeature_Reconcile.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                Width = 120,
                // ControlPath = "Logitude.Customs.CustomsControls.SendOptionsControl",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion


            #endregion


            #region PaymentCheque

            FeaturePM paymentChequeFeature_SaveAsDraft = features.Where(d => d.Code == "SAVEASDRAFTT" && d.ObjectTableId == paymentChequeTableId).FirstOrDefault();
            FeaturePM paymentChequeFeature_Approve = features.Where(d => d.Code == "APPROVEE" && d.ObjectTableId == paymentChequeTableId).FirstOrDefault();
            FeaturePM paymentChequeFeature_More = features.Where(d => d.Code == "MOREE" && d.ObjectTableId == paymentChequeTableId).FirstOrDefault();
            FeaturePM paymentChequeFeature_Print = features.Where(d => d.Code == "PRINTCHEQUEE" && d.ObjectTableId == paymentChequeTableId).FirstOrDefault();
            FeaturePM paymentChequeFeature_Cancel = features.Where(d => d.Code == "CANCELCHEQUEE" && d.ObjectTableId == paymentChequeTableId).FirstOrDefault();


            MenuButtonGroup paymentChequeMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "PaymentChequeEdit",
                Name = "PaymentChequeButtonsGroup",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region More Button

            MenuButton PaymentChequeMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "More",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "PaymentCheque.B.More",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "נוספים",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
                MenuButtonGroupId = paymentChequeMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = paymentChequeFeature_More.Id,
                MenuButtonType = "dropdownbutton",
                Width = 150,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Print Button

            MenuButton PaymentChequePrintButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "PrintCheque",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "PaymentCheque.B.PaymentChequePrint",
                LabelTextCodeDefaultText = "Print Cheque",
                LocalDefaultText = "הדפסת המחאה",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
                MenuButtonGroupId = paymentChequeMenuButtonGroup.Id,
                ParentMenuButtonId = PaymentChequeMoreButton.Id,
                FeatureId = paymentChequeFeature_Print.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Cancel Button

            MenuButton PaymentChequeCancelButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelCheque",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "PaymentCheque.B.PaymentChequeCancel",
                LabelTextCodeDefaultText = "Cancel Cheque",
                LocalDefaultText = "ביטול המחאה",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
                MenuButtonGroupId = paymentChequeMenuButtonGroup.Id,
                ParentMenuButtonId = PaymentChequeMoreButton.Id,
                FeatureId = paymentChequeFeature_Cancel.Id,
                MenuButtonType = "menuitem",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion


            #region  SaveAsDraftButton Button
            MenuButton SaveAsDraftButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SaveAsDraft",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "PaymentCheque.B.PaymentChequeSaveAsDraft",
                LabelTextCodeDefaultText = "Save As Draft",
                LocalDefaultText = "שמור כטיוטה",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
                MenuButtonGroupId = paymentChequeMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                //   ParentMenuButtonId = actionButton.Id,
                FeatureId = paymentChequeFeature_SaveAsDraft.Id,
                MenuButtonType = "button",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);

            #endregion

            #region Approve Button
            MenuButton ApproveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Approve",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "PaymentCheque.B.PaymentChequeApprove",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אישור",
                ObjectTableId = paymentChequeTableId,
                Tenant = tenant,
                MenuButtonGroupId = paymentChequeMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = paymentChequeFeature_Approve.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                Width = 150,
                // ControlPath = "Logitude.Customs.CustomsControls.SendOptionsControl",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion


            #endregion

            #region BankAccount Buttons

            string bankAccountTableId = objectContext.ObjectTables.Where(f => f.Name == "BankAccount" && f.Tenant == tenant).FirstOrDefault().Id;

            FeaturePM bankAccountFeature_Reconcile = features.Where(d => d.Code == "BankAccountReconcileMenuButton" && d.ObjectTableId == bankAccountTableId).FirstOrDefault();

            MenuButtonGroup bankAccountMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "BankAccountEdit",
                Name = "BankAccountEditButtonsGroup",
                ObjectTableId = bankAccountTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region Reconcile Button
            MenuButton ReconcileButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "BankAccountReconcile",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "BankAccount.B.Reconcile",
                LabelTextCodeDefaultText = "Reconcile",
                LocalDefaultText = "התאם",
                ObjectTableId = bankAccountTableId,
                Tenant = tenant,
                MenuButtonGroupId = bankAccountMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = bankAccountFeature_Reconcile.Id,
                MenuButtonType = "button",
                Width = 110,
                //Style = "ApproveButtonStyle",
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #endregion

            #region Reconciliation Buttons


            FeaturePM reconciliationFeature_Reconcile = features.Where(d => d.Code == "ReconciliationCancelMenuButton" && d.ObjectTableId == reconciliationTableId).FirstOrDefault();

            MenuButtonGroup reconciliationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ReconciliationEdit",
                Name = "ReconciliationEditButtonsGroup",
                ObjectTableId = reconciliationTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region Reconcile Button
            MenuButton CancelReconcileButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelReco",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "Reconciliation.B.CancelReco",
                LabelTextCodeDefaultText = "Cancel Reconciltiation",
                LocalDefaultText = " ביטול התאמה",
                ObjectTableId = reconciliationTableId,
                Tenant = tenant,
                MenuButtonGroupId = reconciliationMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = reconciliationFeature_Reconcile.Id,
                MenuButtonType = "button",
                Width = 135,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #endregion

            #region External Reconciliation Buttons


            FeaturePM externalReconciliationFeature_Reconcile = features.Where(d => d.Code == "ExtReconciliationCancelMenuButton" && d.ObjectTableId == externalReconciliationTableId).FirstOrDefault();

            MenuButtonGroup externalReconciliationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ExternalReconciliationEdit",
                Name = "ExtReconciliationEditButtonsGroup",
                ObjectTableId = externalReconciliationTableId,
                Tenant = tenant,
            }, menuButtonGroupRepository, tenantMenuButtonGroups);

            #region Reconcile Button
            MenuButton CancelExReconcileButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelExtReco",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "ExternalReconciliation.B.CancelExtReco",
                LabelTextCodeDefaultText = "Cancel Reconciltiation",
                LocalDefaultText = "ביטול התאמה",
                ObjectTableId = externalReconciliationTableId,
                Tenant = tenant,
                MenuButtonGroupId = externalReconciliationMenuButtonGroup.Id,
                ParentMenuButtonId = null,
                FeatureId = externalReconciliationFeature_Reconcile.Id,
                MenuButtonType = "button",
                Width = 135,
            }, menuButtonRepository, tenantMenuButtons, textCodeRepository, textCodes);
            #endregion

            #endregion

        }



        #endregion




        //  ________________________________
        // |                                |
        // |             FEATURES           |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V

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
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "General" && f.Tenant == 0).FirstOrDefault();

            ObjectTablePM journalActionTypeObjectTable = objectTables.Where(d => d.Name == "JournalActionType").FirstOrDefault();
            ObjectTablePM chartOfAccountObjectTable = objectTables.Where(d => d.Name == "ChartOfAccount").FirstOrDefault();
            ObjectTablePM journalObjectTable = objectTables.Where(d => d.Name == "Journal").FirstOrDefault();
            ObjectTablePM gLAccountObjectTable = objectTables.Where(d => d.Name == "GLAccount").FirstOrDefault();
            ObjectTablePM accountingPeriodObjectTable = objectTables.Where(d => d.Name == "AccountingPeriod").FirstOrDefault();
            ObjectTablePM automaticReconcileMethodObjectTable = objectTables.Where(d => d.Name == "AutomaticReconcileMethod").FirstOrDefault();
            ObjectTablePM reconciliationObjectTable = objectTables.Where(d => d.Name == "Reconciliation").FirstOrDefault();
            ObjectTablePM category1ObjectTable = objectTables.Where(d => d.Name == "Category1").FirstOrDefault();
            ObjectTablePM category2ObjectTable = objectTables.Where(d => d.Name == "Category2").FirstOrDefault();
            ObjectTablePM category3ObjectTable = objectTables.Where(d => d.Name == "Category3").FirstOrDefault();
            ObjectTablePM category4ObjectTable = objectTables.Where(d => d.Name == "Category4").FirstOrDefault();
            ObjectTablePM category5ObjectTable = objectTables.Where(d => d.Name == "Category5").FirstOrDefault();
            ObjectTablePM bankCodeObjectTable = objectTables.Where(d => d.Name == "BankCode").FirstOrDefault();
            ObjectTablePM bankAccountObjectTable = objectTables.Where(d => d.Name == "BankAccount").FirstOrDefault();
            ObjectTablePM bankDepositObjectTable = objectTables.Where(d => d.Name == "BankDeposit").FirstOrDefault();
            ObjectTablePM revaluationObjectTable = objectTables.Where(d => d.Name == "Revaluation").FirstOrDefault();

            ObjectTablePM cashbookObjectTable = objectTables.Where(d => d.Name == "CashBook").FirstOrDefault();
            ObjectTablePM PaymentChequeObjectTable = objectTables.Where(d => d.Name == "PaymentCheque").FirstOrDefault();
            ObjectTablePM TaxWithholdingAssessOffice = objectTables.Where(d => d.Name == "TaxWithholdingAssessOffice").FirstOrDefault();
            ObjectTablePM externalReconciliationObjectTable = objectTables.Where(d => d.Name == "ExternalReconciliation").FirstOrDefault();
            ObjectTablePM TaxReportObjectTable = objectTables.Where(d => d.Name == "TaxReport").FirstOrDefault();
            ObjectTablePM TaxDeductionReportObjectTable = objectTables.Where(d => d.Name == "TaxDeductionReport").FirstOrDefault();
            ObjectTablePM AccountingIntegrityCheckObjectTable = objectTables.Where(d => d.Name == "AccountingIntegrityCheck").FirstOrDefault();
            ObjectTablePM OpenFormatReportObjectTable = objectTables.Where(d => d.Name == "OpenFormatReport").FirstOrDefault();

            #endregion

            Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
            List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

            #region features

            #region JournalActionType
            Feature JournalActionTypeFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.PackageFeature", NameTextCodeDefaultText = "Journal Action Type Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNALACTIONTYPES", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.JournalActionTypes", NameTextCodeDefaultText = "Journal Action Types", FeatureTypeCode = "QUER", FullLocalDefaultText = "קוד פעולה של פקודה", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.New", NameTextCodeDefaultText = "New Journal Action Type", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalActionTypeFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.Edit", NameTextCodeDefaultText = "Edit Journal Action Type", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature JournalActionTypeFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNALACTIONTYPESMENU", ObjectTableId = journalActionTypeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "JournalActionType.Features.JournalActionTypesMenu", NameTextCodeDefaultText = "Journal Action Types", FeatureTypeCode = "MENU", FullLocalDefaultText = "קוד פעולה של פקודה", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region ChartOfAccount
            Feature ChartOfAccountFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.PackageFeature", NameTextCodeDefaultText = "Chart of Accounts Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHARTOFACCOUNTS", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.ChartOfAccounts", NameTextCodeDefaultText = "Chart of Accounts", FeatureTypeCode = "QUER", FullLocalDefaultText = "חשבונות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.New", NameTextCodeDefaultText = "New Chart of Accounts", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.Edit", NameTextCodeDefaultText = "Edit Chart of Accounts", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChartOfAccountFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHARTOFACCOUNTSMENU", ObjectTableId = chartOfAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ChartOfAccount.Features.ChartOfAccountsMenu", NameTextCodeDefaultText = "Chart of Accounts", FeatureTypeCode = "MENU", FullLocalDefaultText = "חשבונות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region GLAccount
            Feature GLAccountFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.PackageFeature", NameTextCodeDefaultText = "General Ledger Accounts Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature GLAccountFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.GLAccounts", NameTextCodeDefaultText = "General Ledger Accounts", FeatureTypeCode = "QUER", FullLocalDefaultText = "לוח חשבונות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.New", NameTextCodeDefaultText = "New General Ledger Account", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Edit", NameTextCodeDefaultText = "Edit General Ledger Account", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature GLAccountFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLACCOUNTSMENU", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.GLAccountsMenu", NameTextCodeDefaultText = "General Ledger Accounts", FeatureTypeCode = "MENU", FullLocalDefaultText = "לוח חשבונות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLIENTGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Clients", NameTextCodeDefaultText = "All Customers Accounts", FeatureTypeCode = "QUER", FullLocalDefaultText = "לקוחות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature GLAccountFeature9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLIENTGLACCOUNTSMENU", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.ClientGLAccountsMenu", NameTextCodeDefaultText = "Client Accounts", FeatureTypeCode = "MENU", FullLocalDefaultText = "לקוחות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRANSACTIONS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Transactions", NameTextCodeDefaultText = "Transactions", FeatureTypeCode = "AREA", FullLocalDefaultText = "תנועות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RECONCILE", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Reconcile", NameTextCodeDefaultText = "Reconcile", FeatureTypeCode = "AREA", FullLocalDefaultText = "התאמה", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGERECONCILIATIONS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.ManageReconciliations", NameTextCodeDefaultText = "Manage Reconc.", FeatureTypeCode = "AREA", FullLocalDefaultText = "ניהול התאמות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Vendors", NameTextCodeDefaultText = "Vendor Accounts", FeatureTypeCode = "QUER", FullLocalDefaultText = "ספקים", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature GLAccountFeature14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORGLACCOUNTSMENU", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.VendorGLAccountsMenu", NameTextCodeDefaultText = "Vendor Accounts", FeatureTypeCode = "MENU", FullLocalDefaultText = "ספקים", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.AllGLAccounts", NameTextCodeDefaultText = "All GL Accounts", FeatureTypeCode = "QUER", FullLocalDefaultText = "לוח חשבונות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        

            Feature GLAccountFeature16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREGLACCOUNT", Packagable = true, ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.More", NameTextCodeDefaultText = "More Buttons", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACITVE", Packagable = true, ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Inactive", NameTextCodeDefaultText = "Inactive", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature171 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTCARDINDEX", Packagable = true, ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.PrintCardIndex", NameTextCodeDefaultText = "Print Card Index", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RECOCILE", Packagable = true, ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Reconcile", NameTextCodeDefaultText = "Reconcile", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            
            // Main GLAccounts
            Feature GLAccountFeature18 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVEGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.ACTIVEGLACCOUNTS", NameTextCodeDefaultText = "Active GL Account", FeatureTypeCode = "QUER", FullLocalDefaultText = "Active GL Account", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature19 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACTIVEGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.INACTIVEGLACCOUNTS", NameTextCodeDefaultText = "Inactive GL Account", FeatureTypeCode = "QUER", FullLocalDefaultText = "Inactive GL Account", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature20 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENFILESGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.OPENFILESGLACCOUNTS", NameTextCodeDefaultText = "Open Files", FeatureTypeCode = "QUER", FullLocalDefaultText = "Open Files", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature21 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLOSEDFILESGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.CLOSEDFILESGLACCOUNTS", NameTextCodeDefaultText = "Closed Files", FeatureTypeCode = "QUER", FullLocalDefaultText = "Closed Files", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature211 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLFILESGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.ALLFILESGLACCOUNTS", NameTextCodeDefaultText = "All Files", FeatureTypeCode = "QUER", FullLocalDefaultText = "All Files", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature212 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLJOBSGLACCOUNTS", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.ALLJOBSGLACCOUNTS", NameTextCodeDefaultText = "All Jobs", FeatureTypeCode = "QUER", FullLocalDefaultText = "All Jobs", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


            // Customer GLAccounts
            Feature GLAccountFeature22 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "collectorsGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.collectorsGLA", NameTextCodeDefaultText = "My Customers (As Colectors)", FeatureTypeCode = "QUER", FullLocalDefaultText = "My Customers (As Colectors)", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature23 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "debetorsGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.debetorsGLA", NameTextCodeDefaultText = "Debetors Customers", FeatureTypeCode = "QUER", FullLocalDefaultText = "Debetors Customers", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature24 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "activeCustomersGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.activeCustomersGLA", NameTextCodeDefaultText = "Active Customers", FeatureTypeCode = "QUER", FullLocalDefaultText = "Active Customers", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature25 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "inactiveCustomersGla", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.inactiveCustomersGla", NameTextCodeDefaultText = "Inactive Customers", FeatureTypeCode = "QUER", FullLocalDefaultText = "Inactive Customers", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            // Vendor GLAccounts
            //Feature GLAccountFeature26 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "collectorsGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.collectorsGLA", NameTextCodeDefaultText = "My Customers (As Colectors)", FeatureTypeCode = "QUER", FullLocalDefaultText = "My Customers (As Colectors)", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature GLAccountFeature27 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "debetorsGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.debetorsGLA", NameTextCodeDefaultText = "Debetors Customers", FeatureTypeCode = "QUER", FullLocalDefaultText = "Debetors Customers", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature28 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "activeVendorsGLA", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.activeVendorsGLA", NameTextCodeDefaultText = "Active Vendors", FeatureTypeCode = "QUER", FullLocalDefaultText = "Active Vendors", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature29 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "inactiveVendorsGla", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.inactiveVendorsGla", NameTextCodeDefaultText = "Inactive Vendors", FeatureTypeCode = "QUER", FullLocalDefaultText = "Inactive Vendors", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDITIONAL", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Additional", NameTextCodeDefaultText = "Additional Data", FeatureTypeCode = "AREA", FullLocalDefaultText = "נתונים נוספים", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature GLAccountFeature31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TAX", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GLAccount.Features.Tax", NameTextCodeDefaultText = "Tax Withholding", FeatureTypeCode = "AREA", FullLocalDefaultText = "ניכוי מס במקור", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


            #endregion

            #region Journal
            Feature JournalFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.PackageFeature", NameTextCodeDefaultText = "Journal Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature JournalFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.New", NameTextCodeDefaultText = "New Journal", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Edit", NameTextCodeDefaultText = "Edit Journal", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            //Feature JournalFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNALMENU", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.JournalMenu", NameTextCodeDefaultText = "Journal", FeatureTypeCode = "MENU", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEJOURNAL", Packagable = true, ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Savejournal", NameTextCodeDefaultText = "Save Journal", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREJOURNAL", Packagable = true, ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Morejournal", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEASDRAFTJOURNAL", Packagable = true, ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.SaveAsDraftJournal", NameTextCodeDefaultText = "Save As Draft Journal", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVEJOURNAL", Packagable = true, ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.ApproveJournal", NameTextCodeDefaultText = "Approve Journal", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature20 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTJOURNAL", Packagable = true, ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.JournalPrint", NameTextCodeDefaultText = "Print Journal Button", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature JournalFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNAL", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.Journal", NameTextCodeDefaultText = "All Journals", FeatureTypeCode = "QUER", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature19 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DraftJournal", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.DraftJournal", NameTextCodeDefaultText = "Draft Journals", FeatureTypeCode = "QUER", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SavedJournal", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.SavedJournal", NameTextCodeDefaultText = "Saved Journals", FeatureTypeCode = "QUER", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ApprovedJournal", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.ApprovedJournal", NameTextCodeDefaultText = "Approved Journals", FeatureTypeCode = "QUER", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature JournalFeature15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExternalJournals", ObjectTableId = journalObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Journal.Features.ExternalJournals", NameTextCodeDefaultText = "External Journals", FeatureTypeCode = "QUER", FullLocalDefaultText = "פקודת יומן", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);




            //FeaturePM journalFeature_Save = features.Where(d => d.Code == "SAVEJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            //FeaturePM journalFeature_SaveAsDraft = features.Where(d => d.Code == "SAVEASDRAFTJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            //FeaturePM journalFeature_Approve = features.Where(d => d.Code == "APPROVEJOURNAL" && d.ObjectTableId == journalTableId).FirstOrDefault();
            
            #endregion

            #region AccountingPeriod
            Feature AccountingPeriodFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.PackageFeature", NameTextCodeDefaultText = "Accounting Periods Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGPERIODS", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.AccountingPeriods", NameTextCodeDefaultText = "Accounting Periods", FeatureTypeCode = "QUER", FullLocalDefaultText = "תקופות חשבונאיות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.New", NameTextCodeDefaultText = "New Accounting Period", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.Edit", NameTextCodeDefaultText = "Edit Accounting Period", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingPeriodFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGPERIODSMENU", ObjectTableId = accountingPeriodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingPeriod.Features.AccountingPeriodsMenu", NameTextCodeDefaultText = "Accounting Periods", FeatureTypeCode = "MENU", FullLocalDefaultText = "תקופות חשבונאיות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region AutomaticReconcileMethod
            Feature AutomaticReconcileMethodFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.PackageFeature", NameTextCodeDefaultText = "Automatic Reconcile Method Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUTORECOMETHODS", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.AutomaticReconcileMethods", NameTextCodeDefaultText = "Automatic Reconcile Methods", FeatureTypeCode = "QUER", FullLocalDefaultText = "שיטות התאמה אוטומטית", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.New", NameTextCodeDefaultText = "New Automatic Reconcile Method", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.Edit", NameTextCodeDefaultText = "Edit Automatic Reconcile Method", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AutomaticReconcileMethodFeature7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUTORECOMETHODSMENU", ObjectTableId = automaticReconcileMethodObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AutomaticReconcileMethod.Features.AutomaticReconcileMethodsMenu", NameTextCodeDefaultText = "Automatic Reconcile Methods", FeatureTypeCode = "MENU", FullLocalDefaultText = "שיטות התאמה אוטומטית", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region Reconciliation
            Feature ReconciliationFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RECONCILERECO", Packagable = true, ObjectTableId = reconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Reconciliation.Features.Reconcile", NameTextCodeDefaultText = "Reconcile", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ReconciliationFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEASDRAFTRECO", Packagable = true, ObjectTableId = reconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Reconciliation.Features.SaveAsDraft", NameTextCodeDefaultText = "Save As Draft", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ReconciliationFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADJUSTRECO", Packagable = true, ObjectTableId = reconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Reconciliation.Features.Adjust", NameTextCodeDefaultText = "Adjust", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature reconciliationFeature_CancelReconcile = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReconciliationCancelMenuButton", Packagable = true, ObjectTableId = reconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Reconciliation.Features.CancelReconcileMenuButton", NameTextCodeDefaultText = "Cancel Reconciltiation", FullLocalDefaultText = "ביטול התאמה", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region Categories
            Feature category1Feature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { ObjectTableId = category1ObjectTable.Id, Tenant = tenant, FeatureTypeCode = "MENU", Packagable = true, Code = "Category1.Features.Category1", NameTextCodeCode = "Category1.Features.Category1", NameTextCodeDefaultText = "Category 1" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature category2Feature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { ObjectTableId = category2ObjectTable.Id, Tenant = tenant, FeatureTypeCode = "MENU", Packagable = true, Code = "Category2.Features.Category2", NameTextCodeCode = "Category2.Features.Category2", NameTextCodeDefaultText = "Category 2" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature category3Feature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { ObjectTableId = category3ObjectTable.Id, Tenant = tenant, FeatureTypeCode = "MENU", Packagable = true, Code = "Category3.Features.Category3", NameTextCodeCode = "Category3.Features.Category3", NameTextCodeDefaultText = "Category 3" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature category4Feature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { ObjectTableId = category4ObjectTable.Id, Tenant = tenant, FeatureTypeCode = "MENU", Packagable = true, Code = "Category4.Features.Category4", NameTextCodeCode = "Category4.Features.Category4", NameTextCodeDefaultText = "Category 4" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature category5Feature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { ObjectTableId = category5ObjectTable.Id, Tenant = tenant, FeatureTypeCode = "MENU", Packagable = true, Code = "Category5.Features.Category5", NameTextCodeCode = "Category5.Features.Category5", NameTextCodeDefaultText = "Category 5" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region BankCodes

            Feature BankCodeFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = bankCodeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankCode.Features.New", NameTextCodeDefaultText = "New Bank Code", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankCodeFeatureMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKCODEMENU", ObjectTableId = bankCodeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankCode.Features.BankCodeMenu", NameTextCodeDefaultText = "Bank Codes", FeatureTypeCode = "MENU", FullLocalDefaultText = "קודי בנקים", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region BankDeposit
            Feature BankDepositFeatureMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKDEPOSITMENU", ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.BankDepositMenu", NameTextCodeDefaultText = "Bank Deposit", FeatureTypeCode = "MENU", FullLocalDefaultText = "הפקדות", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankDepositFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKDEPOSITPRINT", Packagable = true, ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.BankDepositPrint", NameTextCodeDefaultText = "Print Bank Deposit", FullLocalDefaultText = "הדפסת הפקדה לבנק", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankDepositFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKDEPOSITAPRV", Packagable = true, ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.BankDepositApprove", NameTextCodeDefaultText = "Approve Deposit", FullLocalDefaultText = "Approve", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankDepositFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.Details", NameTextCodeDefaultText = "Details Tab", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature CashBankDepositFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBankDeposit", ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.cashdepo", NameTextCodeDefaultText = "Cash Deposit", FeatureTypeCode = "QUER", FullLocalDefaultText = "Cash Deposit", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ChequeBankDepositFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ChequeBankDeposit", ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.chequedepo", NameTextCodeDefaultText = "Cheque Deposit", FeatureTypeCode = "QUER", FullLocalDefaultText = "Cheque Deposit", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TodayBankDepositFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TodayBankDeposit", ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.todaydepo", NameTextCodeDefaultText = "Today Deposit", FeatureTypeCode = "QUER", FullLocalDefaultText = "Today Deposit", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature BankDepositFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREBNKDPST", Packagable = true, ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.More", NameTextCodeDefaultText = "More Buttons", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankDepositFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelDeposit", Packagable = true, ObjectTableId = bankDepositObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankDeposit.Features.CancelDeposit", NameTextCodeDefaultText = "Cancel Deposit", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion

            #region Revaluation
            Feature YearTransferFeatureMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "YEARTRANSFERMENU", ObjectTableId = gLAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "YearTransfer.Features.YearTransferMenu", NameTextCodeDefaultText = "YearTransfer", FeatureTypeCode = "MENU", FullLocalDefaultText = "מעבר שנה", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion
              
            #region BankAccounts

            Feature BankAccountFeatureMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKACCOUNTMENU", ObjectTableId = bankAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankAccount.Features.BankAccountMenu", NameTextCodeDefaultText = "Bank Accounts", FeatureTypeCode = "MENU", FullLocalDefaultText = "חשבונות בנק", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature BankAccountReconcileMenuButtonFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BankAccountReconcileMenuButton", Packagable = true, ObjectTableId = bankAccountObjectTable.Id, Tenant = tenant, NameTextCodeCode = "BankAccount.Features.ReconcileMenuButton", NameTextCodeDefaultText = "Reconcile Button", FullLocalDefaultText = "התאם", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


            #endregion


            #region Revaluation
            Feature RevaluationFeatureMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REVALUATIONMENU", ObjectTableId = revaluationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Revaluation.Features.RevaluationMenu", NameTextCodeDefaultText = "Revaluations", FeatureTypeCode = "MENU", FullLocalDefaultText = "שערוכים", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature RevaluationFeature0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = revaluationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Revaluation.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature RevaluationFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = revaluationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Revaluation.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            
            #endregion


            #region CashBook
            Feature CashbookFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashbookMenuFeature", Packagable = true, ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.CashbookMenuFeature", NameTextCodeDefaultText = "Cashbook", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature CashbookFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature CashbookFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MNGDEPO", ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.MNGDEPO", NameTextCodeDefaultText = "Manage Depo.", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature CashbookFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MORECASHBOOK", Packagable = true, ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.More", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature CashbookFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACITVE", Packagable = true, ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.Inactive", NameTextCodeDefaultText = "Inactive", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature CashbookFeature6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DPSTCASHBOOK", Packagable = true, ObjectTableId = cashbookObjectTable.Id, Tenant = tenant, NameTextCodeCode = "CashBook.Features.Deposite", NameTextCodeDefaultText = "Deposite", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion

            #region PaymentCheque
            Feature PaymentChequeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEASDRAFTT", Packagable = true, ObjectTableId = PaymentChequeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PaymentCheque.Features.PCSaveAsDraft", NameTextCodeDefaultText = "Save as draft", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature PaymentChequeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVEE", Packagable = true, ObjectTableId = PaymentChequeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PaymentCheque.Features.PCApprove", NameTextCodeDefaultText = "Approve", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature PaymentChequeFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREE", Packagable = true, ObjectTableId = PaymentChequeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PaymentCheque.Features.PCMore", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT"}, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            Feature PaymentChequeFeature4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTCHEQUEE", Packagable = true, ObjectTableId = PaymentChequeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PaymentCheque.Features.PCPrint", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature PaymentChequeFeature5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELCHEQUEE", Packagable = true, ObjectTableId = PaymentChequeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PaymentCheque.Features.PCCancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region TaxWithholdingOfficesMenuFeatures
            Feature TaxWithholdingOfficesMenuFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TaxOfficesMenuFeature", Packagable = true, ObjectTableId = TaxWithholdingAssessOffice.Id, Tenant = tenant, NameTextCodeCode = "TaxWithholdingAssessOffice.Features.Menu", NameTextCodeDefaultText = "TaxWithholdingAssessingOffice", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TaxWithholdingOfficesFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = TaxWithholdingAssessOffice.Id, Tenant = tenant, NameTextCodeCode = "TaxWithholdingAssessOffice.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TaxWithholdingOfficesFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = TaxWithholdingAssessOffice.Id, Tenant = tenant, NameTextCodeCode = "TaxWithholdingAssessOffice.Features.New", NameTextCodeDefaultText = "New Tax Withholding Assessing Office", FeatureTypeCode = "NEW", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature TaxWithholdingOfficesFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = TaxWithholdingAssessOffice.Id, Tenant = tenant, NameTextCodeCode = "TaxWithholdingAssessOffice.Features.Edit", NameTextCodeDefaultText = "Edit Tax Withholding Assessing Office", FeatureTypeCode = "UPDT", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

            #endregion

            #region ExternalReconciliation
            Feature ExternalReconciliationFeature_CancelReconcile = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExtReconciliationCancelMenuButton", Packagable = true, ObjectTableId = externalReconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ExternalReconciliation.Features.CancelReconcileMenuButton", NameTextCodeDefaultText = "Cancel Reconciltiation", FullLocalDefaultText = "ביטול התאמה", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature ExternalReconciliationFeature_GenerateTestRecords = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExtRecoGenerateTestRecords", Packagable = true, ObjectTableId = externalReconciliationObjectTable.Id, Tenant = tenant, NameTextCodeCode = "ExternalReconciliation.Features.ExtRecoGenerateTestRecords", NameTextCodeDefaultText = "Generate Test Records", FullLocalDefaultText = "Generate Test Records", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region TaxReport
            Feature TaxReportFeature_Menu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TaxReport.Features.Menu", Packagable = true, ObjectTableId = TaxReportObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TaxReport.Features.Menu", NameTextCodeDefaultText = "Tax Report", FullLocalDefaultText = "", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion
            //  
            #region TaxDeductionReport
            Feature TaxDeductionReportFeature_Menu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TaxDeductionReport.Features.Menu", Packagable = true, ObjectTableId = TaxDeductionReportObjectTable.Id, Tenant = tenant, NameTextCodeCode = "TaxDeductionReport.Features.Menu", NameTextCodeDefaultText = "Tax Deduction Report", FullLocalDefaultText = "", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            Feature AccountingIntegrityCheckFeature_Menu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AccountingIntegrityCheck.Features.Menu", Packagable = true, ObjectTableId = AccountingIntegrityCheckObjectTable.Id, Tenant = tenant, NameTextCodeCode = "AccountingIntegrityCheck.Features.Menu", NameTextCodeDefaultText = "Accounting Integrity Checks", FullLocalDefaultText = "", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #region OpenFormatReport
            Feature OpenFormatReportFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TestingMode", Packagable = true, ObjectTableId = OpenFormatReportObjectTable.Id, Tenant = tenant, NameTextCodeCode = "OpenFormatReport.Features.TestingMode", NameTextCodeDefaultText = "Testing Mode", FeatureTypeCode = "ACT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            #endregion

            #region FullAccountingTabs
            Feature AccountingMainTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCMAIN", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.Main", NameTextCodeDefaultText = "Main Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingCustomersTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCCustomers", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.Customers", NameTextCodeDefaultText = "Customers Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingVendorsTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCVendors", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.Vendors", NameTextCodeDefaultText = "Vendors Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingBanksTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCBanks", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.Banks", NameTextCodeDefaultText = "Banks Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingGLAccountsTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCGLAccounts", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.GLAccounts", NameTextCodeDefaultText = "GLAccounts Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            Feature AccountingMiscTabFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCMisc", Packagable = true, ObjectTableId = objectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.Features.Misc", NameTextCodeDefaultText = "Misc Tab", FeatureTypeCode = "MENU" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


            #endregion
            #endregion

            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
        }
        #endregion




        //  ________________________________
        // |                                |
        // |            TEXT CODES          |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V

        #region LoadTextCodes

        public void LoadOtherFields(IWebFreightContext context)
        {
            objectContext = context;
            textCodeRepository = new TextCodeRepository(objectContext);

            Dictionary<string, TextCode> textcodes = textCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);
            LoadTextCodes_General(textcodes);
            LoadTextCodes_JournalActionType(textcodes);
            LoadTextCodes_ChartOfAccounts(textcodes);
            LoadTextCodes_Journal(textcodes); 
            LoadTextCodes_BankCode(textcodes);     
            LoadTextCodes_BankAccount(textcodes);
            LoadTextCodes_BankDeposit(textcodes);
            LoadTextCodes_CashBook(textcodes);
            LoadTextCodes_Revaluation(textcodes);
            LoadTextCodes_JournalLine(textcodes); 
            LoadTextCodes_GLAccounts(textcodes);
            LoadTextCodes_AccountingPeriods(textcodes);
            LoadTextCodes_Catgories(textcodes);
            LoadTextCodes_YearTransfer(textcodes);
            LoadTextCodes_AutomaticReconcileMethod(textcodes);
            LoadTextCodes_ReconcileExternalPage(textcodes);
            #region ObjectTable
            ObjectTable ChartOfAccountTable = objectContext.ObjectTables.Where(f => f.Name == "ChartOfAccount" && f.Tenant == 0).FirstOrDefault();
            ObjectTable JournalTable = objectContext.ObjectTables.Where(f => f.Name == "Journal" && f.Tenant == 0).FirstOrDefault();
            ObjectTable GLAccountTable = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == 0).FirstOrDefault();
            ObjectTable AccountingPeriodTable = objectContext.ObjectTables.Where(f => f.Name == "AccountingPeriod" && f.Tenant == 0).FirstOrDefault();
            ObjectTable JournalActionTypeTable = objectContext.ObjectTables.Where(f => f.Name == "JournalActionType" && f.Tenant == 0).FirstOrDefault();
            ObjectTable AutomaticReconcileMethodTable = objectContext.ObjectTables.Where(f => f.Name == "AutomaticReconcileMethod" && f.Tenant == 0).FirstOrDefault();
            ObjectTable CashBookTable = objectContext.ObjectTables.Where(f => f.Name == "CashBook" && f.Tenant == 0).FirstOrDefault();
            ObjectTable RevaluationTable = objectContext.ObjectTables.Where(f => f.Name == "Revaluation" && f.Tenant == 0).FirstOrDefault();
            ObjectTable ReconcileExternalPageTable = objectContext.ObjectTables.Where(f => f.Name == "ReconcileExternalPage" && f.Tenant == 0).FirstOrDefault();

            #endregion

            #region Tabs Headers


            #region ChartOfAccount
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccount.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = ChartOfAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccount.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = ChartOfAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion


            #region YearTransfer
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion


            #region Journal
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = JournalTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = JournalTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.Details", DefaultText = "Details", LocalDefaultText = "פרטים", ObjectTableId = JournalTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion




            #region GLAccount
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.New", DefaultText = "New", LocalDefaultText = "חדש", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Transactions", DefaultText = "Transactions", LocalDefaultText = "תנועות", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Reconcile", DefaultText = "Reconcile", LocalDefaultText = "התאמה", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.ManageReconciliations", DefaultText = "Manage Reconc.", LocalDefaultText = "ניהול התאמות", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Additional", DefaultText = "Additional Data", LocalDefaultText = "נתונים נוספים", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Tax", DefaultText = "Tax withholding", LocalDefaultText = "ניכוי מס במקור", ObjectTableId = GLAccountTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);

            
            #endregion


            #region AccountingPeriod
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.New", DefaultText = "New", LocalDefaultText = "חדש", ObjectTableId = AccountingPeriodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = AccountingPeriodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = AccountingPeriodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion


            #region JournalActionType
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalActionType.TH.New", DefaultText = "New", LocalDefaultText = "חדש", ObjectTableId = JournalActionTypeTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalActionType.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = JournalActionTypeTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalActionType.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = JournalActionTypeTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion


            #region AutomaticReconcileMethod
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.TH.New", DefaultText = "New", LocalDefaultText = "חדש", ObjectTableId = AutomaticReconcileMethodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = AutomaticReconcileMethodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = AutomaticReconcileMethodTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region CashBook
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.Details", DefaultText = "Details", LocalDefaultText = "פרטים", ObjectTableId = CashBookTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.ManageDepo", DefaultText = "Manage Depo.", LocalDefaultText = "ניהול הפקדות", ObjectTableId = CashBookTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region BankDeposit
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.TH.Details", DefaultText = "Details", LocalDefaultText = "פרטים", ObjectTableId = CashBookTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            #region Revaluation
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = RevaluationTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.TH.Details", DefaultText = "Details", LocalDefaultText = "פרטים", ObjectTableId = RevaluationTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, textCodeRepository, textcodes);
            #endregion

            objectContext.SaveChanges();
            
            
            #endregion
        
        }
        

        #region LoadTextCodes_General
        private void LoadTextCodes_General(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "General" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.SaveAndNew", DefaultText = "Save And New", LocalDefaultText = "שמירה וחדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Add", DefaultText = "Add", LocalDefaultText = "הוסף", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.OK", DefaultText = "Ok", LocalDefaultText = "אישור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Cancel", DefaultText = "Cancel", LocalDefaultText = "ביטול", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Close", DefaultText = "Close", LocalDefaultText = "סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Browse", DefaultText = "Browse", LocalDefaultText = "אישור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Search", DefaultText = "Search", LocalDefaultText = "חפש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Edit", DefaultText = "Edit", LocalDefaultText = "ערוכה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AddDocumentVersion", DefaultText = "Add Version", LocalDefaultText = "גרסה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Reconcile", DefaultText = "Reconcile", LocalDefaultText = "התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.SaveAsDraft", DefaultText = "Save as Draft", LocalDefaultText = "שמור טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Adjust", DefaultText = "Adjust", LocalDefaultText = "אישור הפרשים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AutomaticReconcile", DefaultText = "AutomaticReconcile", LocalDefaultText = "התאמה אוטומטית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.View", DefaultText = "View", LocalDefaultText = "צפייה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.CancelReconciliation", DefaultText = "Cancel Reconciliation", LocalDefaultText = "בטל התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
             AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.InvoiceNumber", DefaultText = "The Invoice Number", LocalDefaultText = "מספר חשבונית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O"  }, textCodeRepository, textcodes);
             AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AlreadyExist", DefaultText = "already exist in another invoice", LocalDefaultText = " כבר קיים בחשבונית אחרת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O" }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RefDate", DefaultText = "Ref. Date", LocalDefaultText = "תאריך אסמכתא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNo", DefaultText = "Journal No.", LocalDefaultText = "מספר פקודה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TransactionNo", DefaultText = "Transaction No.", LocalDefaultText = "מספר תנועה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingDate", DefaultText = "Accounting Date", LocalDefaultText = "תאריך חשבונאי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OriginalAmount", DefaultText = "Original Amount", LocalDefaultText = "סכום מקורי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenAmount", DefaultText = "Open Amount", LocalDefaultText = "סכום פתוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AmountToReconcile", DefaultText = "Amount to Reconcile", LocalDefaultText = "סכום להתאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconciliationAmount", DefaultText = "Reconciliation Amount", LocalDefaultText = "סכום התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconciliationNo", DefaultText = "Reconciliation No.", LocalDefaultText = "התאמה מס'", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref1", DefaultText = "Ref. 1", LocalDefaultText = "אסמכתא 1", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref2", DefaultText = "Ref. 2", LocalDefaultText = "אסמכתא 2", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref3", DefaultText = "Ref. 3", LocalDefaultText = "אסמכתא 3", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Notes", DefaultText = "Notes", LocalDefaultText = "הערות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Totals", DefaultText = "Totals", LocalDefaultText = "סך הכל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Details", DefaultText = "Details", LocalDefaultText = "פרטים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreateDate", DefaultText = "Create Date", LocalDefaultText = "תאריך יצירה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Currency", DefaultText = "Currency", LocalDefaultText = "מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Source", DefaultText = "Source", LocalDefaultText = "מקור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            
            
            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewEntity", DefaultText = "New %Entity", LocalDefaultText = "%Entity חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Close", DefaultText = "Close", LocalDefaultText = "סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Loading", DefaultText = "Loading ....", LocalDefaultText = "טוען ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Sending", DefaultText = "Sending ....", LocalDefaultText = "שולח ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveColumns", DefaultText = "Add/Remove columns", LocalDefaultText = "הוסף/מחק עמודות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoFiltersHaveBeenSet", DefaultText = "No filters have been set", LocalDefaultText = "לא הוגדרו חיתוכים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExportToExcel", DefaultText = "Export to excel ", LocalDefaultText = "Excel הורד לאקסל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Saving", DefaultText = "Saving ....", LocalDefaultText = "שמירה ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PlsWait", DefaultText = "Please wait a moment ....", LocalDefaultText = "נא להמתין רגע ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EditMetaData", DefaultText = "Edit Meta Data", LocalDefaultText = "עריכת מטה דאטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.IsRequired", DefaultText = "Is Required", LocalDefaultText = "הוא נדרש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FieldForTableIsRequired", DefaultText = "%FieldName in %TableName %EntityReference is Required", LocalDefaultText = "%FieldName ב- %TableName %EntityReference הוא חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ObjectTables", DefaultText = "Object Tables", LocalDefaultText = "טבלאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RequiredFields", DefaultText = "Required Fields", LocalDefaultText = "שדות חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WrongEntityName", DefaultText = "Entity name is wrong", LocalDefaultText = "שם הישות שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveRequiredFields", DefaultText = "Add / Remove Required Fields", LocalDefaultText = "הוספה / הסרה של שדות חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SelectObjectTable", DefaultText = "You must select an ObjectTable", LocalDefaultText = "עליך לבחור בלוח אובייקט", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ViewCode", DefaultText = "View Code", LocalDefaultText = "תצוגת קוד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Existed", DefaultText = "Existed", LocalDefaultText = "קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.All", DefaultText = "All", LocalDefaultText = "הכל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FullAccounting", DefaultText = "Full Accounting", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OldValue", DefaultText = "Old value: ", LocalDefaultText = ", ערך קודם: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewValue", DefaultText = ", New value: ", LocalDefaultText = ", ערך חדש: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueLong", DefaultText = "Value too long", LocalDefaultText = "ערך ארוך מדי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueShort", DefaultText = "Value too short", LocalDefaultText = "ערך קצר מדי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueWrong", DefaultText = "Wrong value", LocalDefaultText = "ערך שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.UncompletedRecoQuestion", DefaultText = "There is an uncompleted reconciliation, do you want to complete it?", LocalDefaultText = "קיימת טיוטת התאמה, האם ברצונך להשלים אותה?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.UncompletedReconciliation", DefaultText = "Uncompleted Reconciliation", LocalDefaultText = "טיוטת התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SavingAsDraft", DefaultText = "Saving as a draft", LocalDefaultText = "שמירת טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoLinesChosen", DefaultText = "No lines were chosen", LocalDefaultText = "לא סומנה אף שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalMustZero", DefaultText = "Total amount must be zero", LocalDefaultText = "סך הכל צריל להיות אפס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconcileError", DefaultText = "Reconcile Error", LocalDefaultText = "שגיאת התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PermissionError", DefaultText = "Permission Error", LocalDefaultText = "שגיאת הרשאה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoPermission", DefaultText = "You don't have permission to perform this action", LocalDefaultText = "אין לך הרשאה לבצע את הפעולה הזאת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticRecoQuestion", DefaultText = "Any draft reconciliation will be deleted, do you want to proceed?", LocalDefaultText = "טיוטת התאמה תימחק, האם ברצונך להמשיך?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticReconciliation", DefaultText = "Automatic Reconciliation", LocalDefaultText = "התאמה אוטומטית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelRecoQuestion", DefaultText = "The reconciliation will be cancelled and deleted, do you want to proceed?", LocalDefaultText = "ההתאמה תבוטל ותימחק, האם ברצונך להמשיך?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelReconciliation", DefaultText = "Cancel Reconciliation", LocalDefaultText = "ביטול התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconcileSearchHint", DefaultText = "Search Journal No./References", LocalDefaultText = "חפש לפי מס' פקודה/אסמכתאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EnterYear", DefaultText = "Please enter a year", LocalDefaultText = "נא להקליד שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ConnectedToGLA", DefaultText = "Can’t change the chart of account, there are GL Accounts connected to it.", LocalDefaultText = "לא ניתן לשנות סוג קבוצת מאזן, ישנם כרטיסים המחוברים לקבוצה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RevaluationDate", DefaultText = "Revaluation Date", LocalDefaultText = "תאריך שערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountForRevaluation", DefaultText = "Revaluation's GL Account:", LocalDefaultText = "כרטיס הפרשי שער", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountsforrevaluation", DefaultText = "GLAccounts for revaluation:", LocalDefaultText = "כרטיסים לשערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DefaultGLAccount", DefaultText = "Default GL Account", LocalDefaultText = "כרטיסי ברירת מחדל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfAccount", DefaultText = "Chart Of Account:", LocalDefaultText = "קבוצת מאז", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Account", DefaultText = "Account:", LocalDefaultText = "חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Journals", DefaultText = "Journals", LocalDefaultText = "פקודת יומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalLines", DefaultText = "Journal Lines", LocalDefaultText = "שורות פקודת יומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RevaluationAccountReq", DefaultText = "The field Revaluation’s GL Account is reqired", LocalDefaultText = " שדה כרטיסים לשערוך הינו שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChooseGLAccount", DefaultText = "You must choose GL Accounts for revaluation", LocalDefaultText = "חובה לבחור כרטיסים לשערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Main", DefaultText = "Main", LocalDefaultText = "ראשי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Receivables", DefaultText = "Receivables", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Payables", DefaultText = "Payables", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Banks", DefaultText = "Banks", LocalDefaultText = "בנקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Misc", DefaultText = "Misc", LocalDefaultText = "שונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.MainQueries", DefaultText = "Main Queries", LocalDefaultText = "שאילתות ראשיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentGLAccounts", DefaultText = "Recent GL Accounts", LocalDefaultText = "כרטיסים אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccount", DefaultText = "GL Account", LocalDefaultText = "כרטיסים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            // AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Journals", DefaultText = "Journals", LocalDefaultText = "פקודות יומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.MiscQueries", DefaultText = "Misc Queries", LocalDefaultText = "שאילתות שונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewOpenFormatReport", DefaultText = "New Open Format Report", LocalDefaultText = "דוח מבנה אחיד חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.User", DefaultText = "User", LocalDefaultText = "משתמש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountManager", DefaultText = "Account Manager", LocalDefaultText = "מנהל חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Search", DefaultText = "Search", LocalDefaultText = "חיפוש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.New", DefaultText = "New", LocalDefaultText = "חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountNoName", DefaultText = "Account No. / Name", LocalDefaultText = "מספר חשבון / שם חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CustomersQueries", DefaultText = "Customers Queries", LocalDefaultText = "שאילתות לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentCustomers", DefaultText = "Recent Customers", LocalDefaultText = "לקוחות אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AgingGraph", DefaultText = "Aging Graph", LocalDefaultText = "גרף גיול", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Top10Debtors", DefaultText = "Top 10 Debtors", LocalDefaultText = "10 החייבים ביותר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TimeRange", DefaultText = "Time Range", LocalDefaultText = "טווח זמן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LastXMonth", DefaultText = "Last #number Month", LocalDefaultText = "#number חודשים אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BalanceDue", DefaultText = "Balance Due", LocalDefaultText = "יתרה חייבת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingBalance", DefaultText = "Accounting Balance", LocalDefaultText = "יתרה חשבונאית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewInvoice", DefaultText = "New Invoice", LocalDefaultText = "חשבונית חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewCreditNote", DefaultText = "New Credit Note", LocalDefaultText = "זיכוי חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Collector", DefaultText = "Collector", LocalDefaultText = "גובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Salesman", DefaultText = "Salesman", LocalDefaultText = "איש מכירות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Balanceinlocalcurrency", DefaultText = "Balance in local currency", LocalDefaultText = "יתרה ב", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.dueBalance", DefaultText = "Due balance", LocalDefaultText = " יתרה חייבת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewAccount", DefaultText = "New Account", LocalDefaultText = "חשבון חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Customers", DefaultText = "Customers", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GeneralInvoice", DefaultText = "General Invoice", LocalDefaultText = "חשבוניות לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Payments", DefaultText = "Payments", LocalDefaultText = "קבלות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DraftPayments", DefaultText = "Draft Payments", LocalDefaultText = "קבלות בסטטוס טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenPayments", DefaultText = "Open Payments", LocalDefaultText = "קבלות בסטטוס מאושר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllPayments", DefaultText = "All Payments", LocalDefaultText = "כל הקבלות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ApprovedPayments", DefaultText = "Approved Payments", LocalDefaultText = "קבלות מאושרות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ARInvoices", DefaultText = "AR Invoices", LocalDefaultText = "חשבוניות לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ARPayments", DefaultText = "AR Payments", LocalDefaultText = "קבלות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.APInvoices", DefaultText = "AP Invoices", LocalDefaultText = "חשבוניות ספק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.APPayments", DefaultText = "AP Payments", LocalDefaultText = "תשלום לספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DraftInvoices", DefaultText = "Draft Invoices", LocalDefaultText = "חשבוניות בסטטוס טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ApprovedInvoices", DefaultText = "Approved Invoices", LocalDefaultText = "חשבוניות מאושרות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ApprovedPayments", DefaultText = "Approved Payments", LocalDefaultText = "תשלומים מאושרים ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ClosedMonth", DefaultText = "Closed Month", LocalDefaultText = "חודש סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.VendorsQueries", DefaultText = "Vendors Queries", LocalDefaultText = "שאילתות ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentVendors", DefaultText = "Recent Vendors", LocalDefaultText = "ספקים אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Vendors", DefaultText = "Vendors", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            // Transaction tab
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Transactions", DefaultText = "Transactions", LocalDefaultText = "תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconcile", DefaultText = "Reconcile", LocalDefaultText = "התאם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreateDatefrom", DefaultText = "Create date from", LocalDefaultText = "תאריך יצירה מ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Datefrom", DefaultText = "Date from", LocalDefaultText = "מתאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.To", DefaultText = "To", LocalDefaultText = "עד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNoRef", DefaultText = "Journal No. / Ref.", LocalDefaultText = "מספר פקודת יומן/ אסמכתא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithAttachedAccounts", DefaultText = "With Attached Accounts", LocalDefaultText = "לכלול כרטיסים מקושרים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalInLocalCurrency", DefaultText = "Total In Local Currency", LocalDefaultText = "סה”כ במטבע מקומי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalInCurrencies", DefaultText = "Total In Currencies", LocalDefaultText = "סך הכל במטבעות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenAmount", DefaultText = "Open Amount", LocalDefaultText = "יתרת פתיחה לתקופה המוצגת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithSplittedbycurrency", DefaultText = "With Splitted by currency GL Accounts", LocalDefaultText = "לכלול כרטיסי פיצול לפי מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenBalanceByCurrency", DefaultText = "Open Balance By Currency", LocalDefaultText = "מאזן פתוח לפי מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankQueries", DefaultText = "Bank Queries", LocalDefaultText = "שאילתות בנקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentDeposits", DefaultText = "Recent Deposits", LocalDefaultText = "הפקדות אחרונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashbookStatus", DefaultText = "Cashbook Status", LocalDefaultText = "סטטוס קופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Deposits", DefaultText = "Deposits", LocalDefaultText = "הפקדות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.Q.AllDeposits", DefaultText = "All Deposits", LocalDefaultText = "כל ההפקדות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cashbooks", DefaultText = "Cashbooks", LocalDefaultText = "קופות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cash", DefaultText = "Cash", LocalDefaultText = "מזומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cheque", DefaultText = "Cheque", LocalDefaultText = "המחאה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.postdated", DefaultText = "Postdated", LocalDefaultText = "דחוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllCashbook", DefaultText = "All Cashbook", LocalDefaultText = "כל הקופות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankAccounts", DefaultText = "Bank Accounts", LocalDefaultText = "חשבונות בנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllBankAccounts", DefaultText = "All Bank Accounts", LocalDefaultText = "כל חשבונות הבנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Date", DefaultText = "Date", LocalDefaultText = "תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentCheques", DefaultText = "Payment Cheques", LocalDefaultText = "בדיקות תשלום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllPaymentCheques", DefaultText = "All Payment Cheques", LocalDefaultText = "כל המחאות התשלום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BanksQuery", DefaultText = "Banks Query", LocalDefaultText = "שאילתות בנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositNoChequeNo", DefaultText = "Deposit No. / Cheque No.", LocalDefaultText = "מספר הפקדה / מספר המחאה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositDetails", DefaultText = "Deposit Details", LocalDefaultText = "פרטי הפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewDeposit", DefaultText = "New Deposit", LocalDefaultText = "הפקדה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashCheques", DefaultText = "Cash Cheques", LocalDefaultText = "המחאות מזומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.postdatedCheques", DefaultText = "Postdated Cheques", LocalDefaultText = "המחאות דחויות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashbookTotal", DefaultText = "Cashbook Total", LocalDefaultText = "סך הכל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositAmount", DefaultText = "Deposit Amount", LocalDefaultText = "סכום הפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noChequestodeposit", DefaultText = "There is no Cheques to deposit", LocalDefaultText = "אין פיקדון להפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.InCashbook", DefaultText = "In Cashbook", LocalDefaultText = "בתוך הספר במזומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.InBank", DefaultText = "In Bank", LocalDefaultText = "בבנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChequeNoBankAccount", DefaultText = "Cheque No. / Bank Account", LocalDefaultText = "בדוק מספר / חשבון בנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelApproval", DefaultText = "Cancel Approval", LocalDefaultText = "ביטול אישור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Saveasdraft", DefaultText = "Save as draft", LocalDefaultText = "שמור כטיוטא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Approve", DefaultText = "Approve", LocalDefaultText = "לאשר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewCashbook", DefaultText = "New Cashbook", LocalDefaultText = "קופה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewBankAccounts", DefaultText = "New Bank Account", LocalDefaultText = "חשבון בנק חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewJournal", DefaultText = "New Journal", LocalDefaultText = "פקודת יומן חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Credit", DefaultText = "Credit", LocalDefaultText = "זכות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Debit", DefaultText = "Debit", LocalDefaultText = "חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExchangeRatesetbysystem", DefaultText = "The Exchange Rate set by system", LocalDefaultText = "שער החליפין נקבע על ידי המערכת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExchangeRatesetbyuser", DefaultText = "The Exchange Rate set by user", LocalDefaultText = "שער החליפין שנקבע על ידי המשתמש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reference", DefaultText = "Reference", LocalDefaultText = "אסמכתא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.References", DefaultText = "References", LocalDefaultText = "אסמכתאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticReconcile", DefaultText = "Automatic Reconcile", LocalDefaultText = "התאם אוטומטית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Accordingto", DefaultText = "According to", LocalDefaultText = "לפי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Difference", DefaultText = "Difference", LocalDefaultText = "הפרש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Adjust", DefaultText = "Adjust", LocalDefaultText = "התאם ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Equals", DefaultText = "Equals", LocalDefaultText = "שווים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NotEqual", DefaultText = "Not Equal", LocalDefaultText = "לא שווה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LargerThan", DefaultText = "Larger Than", LocalDefaultText = "גדול מ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LessThan", DefaultText = "Less Than", LocalDefaultText = "פחות מ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LessThanOrEqual", DefaultText = "Less Than Or Equal", LocalDefaultText = "פחות מ או שווה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GreaterThanOrEqual", DefaultText = "Greater Than Or Equal", LocalDefaultText = "גדול או שווה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OriginalAmount", DefaultText = "Original Amount", LocalDefaultText = "סכום מקורי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconciled", DefaultText = "Reconciled", LocalDefaultText = "התואם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconciliation", DefaultText = "Reconciliation", LocalDefaultText = "התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.wascreatedsuccessfully", DefaultText = "was created successfully", LocalDefaultText = "נוצרה בהצלחה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.cashbookDetails", DefaultText = "Cashbook details", LocalDefaultText = "כללי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Amount", DefaultText = "Amount", LocalDefaultText = "סכום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoChequesintheCashbook", DefaultText = "No Cheques in the Cashbook", LocalDefaultText = "אין צ'קים בקופות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewPage", DefaultText = "New Bank Page", LocalDefaultText = "דף בנק חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentCurrencies", DefaultText = "The payment currency does not match to the pay to GL Account Currency ", LocalDefaultText = "המטבע של התשלום לא תואם למטבע כרטיס הנה”ח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentChequeExist", DefaultText = "There is a payment cheque with the same number", LocalDefaultText = "קיימת המחאה עם מספר זהה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoChequeCounter", DefaultText = "The cheque counter did not defined for the chosen bank ", LocalDefaultText = "מונה המחאות לא הוגדר עבור חשבון הבנק הנבחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentCheques", DefaultText = "Payment Cheques", LocalDefaultText = "המחאות תשלום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllPaymentCheques", DefaultText =  "All Payment Cheques", LocalDefaultText = "כל המחאות התשלום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Line", DefaultText = "in line", LocalDefaultText = "שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ManageReconciliation", DefaultText = "Manage reconciliations", LocalDefaultText = "ניהול התאמות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ZeroDeposit", DefaultText = "The deposit amount must be bigger than zero", LocalDefaultText = "הסכום להפקדה חייב להיות גדול מאפס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.minusDepositNotAllowed", DefaultText = "Cannot deposit a minus value", LocalDefaultText = "Cannot deposit minus value", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllFieldsRequired", DefaultText = "All Fields Required", LocalDefaultText = "כל השדות נדרשים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Currencydifferent", DefaultText = "The currency of the bank account and the cashbook is different", LocalDefaultText = "המטבע של חשבון הבנק והקופה שונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noCashICashbook", DefaultText = "There are no cash in the cashbook", LocalDefaultText = "אין מזומנים בקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noChequesinCashbook", DefaultText = "There are no cheques in the cashbook", LocalDefaultText = "אין המחאות בקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNotValid", DefaultText = "Journal is not valid", LocalDefaultText = "פקודת יומן לא תקפה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CurrentCurrency", DefaultText = "Current currency is", LocalDefaultText = "נבחר מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ButAccountCurrencyDifferent", DefaultText = "But account currency is different", LocalDefaultText = "ומטבע הכרטיס הוא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountIs", DefaultText = "Account is ", LocalDefaultText = "כרטיס ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountcurnotmatchcashbookcur", DefaultText = "The GL Account currency does not match the cashbook currency", LocalDefaultText = "המטבע של הכרטיס הנבחר לא תואם למטבע הקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ThereOpenTransaction", DefaultText = "There are open transactions for the GL Account. can’t make it single currency GL Account", LocalDefaultText = "לכרטיס ישנם תנועות פתוחת. אי אפשר להפוך אותו לכרטיס חד מטבעי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ThereTransactions4GLAwithexistingCurrency", DefaultText = "Can’t change currency. There are transactions for this GL Account with the existing currency", LocalDefaultText = "אי אפשר לשנות את המטבע. לכרטיס יש תנועות במטבע הנוכחי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.selectAtLeast1Linetodeposit", DefaultText = "Please select at least one cashbook line to deposit it", LocalDefaultText = "יש לבחור לפחות המחאה אחת להפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Selected", DefaultText = "Selected", LocalDefaultText = "סה''כ סכום נבחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancellationReason", DefaultText = "Cancellation Reason", LocalDefaultText = "סיבת ביטול", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLABalanceNotEqual0", DefaultText = "Can’t inactivate the GL Account. GL Account’s balance is not equal to zero", LocalDefaultText = "לא ניתן לחסום את הכרטיס . יתרת כרטיס שונה מאפס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseActionCode", DefaultText = "You must choose action code for line", LocalDefaultText = "יש לבחור קוד פעולה עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseCreditAccount", DefaultText = "You must choose credit account for line", LocalDefaultText = "יש לבחור כרטיס זכות עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseDebitAccount", DefaultText = "You must choose debit account for line", LocalDefaultText = "יש לבחור כרטיס חובה עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseCurrency", DefaultText = "You must choose currency for line", LocalDefaultText = "יש לבחור מטבע עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AmountIsMissing", DefaultText = "Amount is missing for line", LocalDefaultText = "סכום חסר בשורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseRefDate", DefaultText = "You should choose ref. date for line", LocalDefaultText = "יש לבחור תאריך אסמכתא עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseDueDate", DefaultText = "You should choose due date for line", LocalDefaultText = "יש לבחור תאריך ערך עבור שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreditDebitAccountMustSameCurrency", DefaultText = "Credit and debit account must be the same currency", LocalDefaultText = "כרטיס זכות וכרטיס חובה חייבים להיות באותו מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalDebitMustEqualTotalCredit", DefaultText = "Total debit amount must be equal to total credit amount. There is a difference of", LocalDefaultText = "“סה“כ חובה צריך להיות שווה לסה“כ זכות, קיים הפרש של", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoAutoRecoMethodDefined4GLAccount", DefaultText = "No automatic reconcile method defined for this GL Account", LocalDefaultText = "לא הוגדר לכרטיס שיטת התאמה אוטומטית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NotransactionsSelected", DefaultText = "No transactions selected", LocalDefaultText = "לא נבחרו תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferenceMustEqual0", DefaultText = "The difference must be equal to zero", LocalDefaultText = "ההפרש חייב להיות שווה לאפס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PrepareTransactions", DefaultText = "Preparing Transactions", LocalDefaultText = "מכין תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewConnectedGLAccount", DefaultText = "New Connected GLAccount", LocalDefaultText = "כרטיס מקושר חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentAmount", DefaultText = "Payment Amount:", LocalDefaultText = "סכום לתשלום:", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewGeneralInvoice", DefaultText = "New General Invoice", LocalDefaultText = "חשבונית כללית חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.False", DefaultText = "False", LocalDefaultText = "שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankAccountDifferentCurrencies", DefaultText = "The payment currency should be similar to bank gl account currency", LocalDefaultText = "מטבע התשלום חייב להיות זהה למטבע הכרטיס של הבנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Disconnect", DefaultText = "Disconnect", LocalDefaultText = "ניתוק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentAmounts", DefaultText = "Total amount of lines must be equal to foreign amount", LocalDefaultText = "סהכ סכום השורות חייב להיות שווה לסכום ההמחאה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewPaymentCheque", DefaultText = "New Payment Cheque", LocalDefaultText = "המחאה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Added", DefaultText = "Splitted by currency GLAccount has been added-", LocalDefaultText = "נוסף כרטיס פיצול לפני מטבע-", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Deactivated", DefaultText = "The Splitted GLAccount- was deactivated", LocalDefaultText = "כרטיס הפיצול- נחסם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChildAdded", DefaultText = "The GLAccount- was added as a child ", LocalDefaultText = "לכרטיס - נוסף כרטיס בן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Disconnected", DefaultText = "The Child GLAccount- was disconnected ", LocalDefaultText = "כרטיס בן - נותק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Thereischashbookwithcurrencytypebranch", DefaultText = "There is chashbook with the chosen currency. type & branch", LocalDefaultText = "קיימת קופה עם המטבע והסוג והסניף הנבחרים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ToDateMustGreaterFromDate", DefaultText = "To date must be greater than from date ", LocalDefaultText = "עד תאריך חייב להיות גדול מתאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FromDateMustSmallerToDate", DefaultText = "From date must be smaller than to date ", LocalDefaultText = "מתאריך חייב להיות קטן מ- עד תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Areyousuredeleteline", DefaultText = "Are you sure to delete this line", LocalDefaultText = "האם אתה בטוח שאתה רוצה למחוק את השורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SourceJournal", DefaultText = "Source Journal", LocalDefaultText = "פקודת מקור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", DefaultText = "The GL Account is already connected to a Bank Account", LocalDefaultText = "הכרטיס הנבחר מקושר לחשבון בנק אחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Activated", DefaultText = "The Splitted GLAccount- was activated", LocalDefaultText = "כרטיס הפיצול- מוּפעָל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentBankCurrency", DefaultText = "Bank GLAccount currency differs from Local currency. Can’t create new payment cheque with currency that differs from local currency", LocalDefaultText = "מטבע הכרטיס של הבנק שונה ממטבע מקומי. לא ניתן ליצור המחאה חדשה עם מטבע שונה ממטבע מקומי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ToDate", DefaultText = "To Date", LocalDefaultText = "לתאריך:", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Level", DefaultText = "Level:", LocalDefaultText = "רמה:", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfaccountType", DefaultText = "Chart of account type", LocalDefaultText = "סוג קבוצת מאזן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfaccountFilter", DefaultText = "Chart of account", LocalDefaultText = "קבוצת מאזן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccount", DefaultText = "GLAccount", LocalDefaultText = "כרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithZeroBalance", DefaultText = "GLAccount with balance equal to zero", LocalDefaultText = "כלול כרטיסים ללא תנועות עם יתרה 0", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Run", DefaultText = "Run", LocalDefaultText = "הרץ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FutureDate", DefaultText = "Future Date", LocalDefaultText = "תאריך עתיד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PostdatedChequeRedemption", DefaultText = "Postdated Cheque Redemption", LocalDefaultText = "פרעון שיק דחוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithTransaction", DefaultText = "With Transaction", LocalDefaultText = "עם תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithoutTransaction", DefaultText = "Without Transaction", LocalDefaultText = "בלי תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FilterBy", DefaultText = "Filter by:", LocalDefaultText = "סנן לפי:", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BuildingDocument", DefaultText = "Building document....", LocalDefaultText = "....טוען מסמך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExternalReconcile", DefaultText = "External Reconcile", LocalDefaultText = "התאמות חיצוניות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelReconciltiation", DefaultText = "Cancel Reconciltiation", LocalDefaultText = "ביטול התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.IsCancelled", DefaultText = "Cancelled", LocalDefaultText = "מבוטלת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WantToCancelCurrentReconciliation", DefaultText = "Are you sure you want to cancel the current reconciliation?", LocalDefaultText = "האם אתה בטוח שאתה רוצה לבטל את ההתאמה הנוכחית?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SplittedAccountMsgCredit", DefaultText = "There is a splitted GL Accounts for the chosen multi currency Credit Account. The transactions will be registered in the splitted by currency GL Account", LocalDefaultText = "לכרטיס זה מוגדר פיצול מטבעות . התנועה תרשם על הכרטיס שמתאים למטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SplittedAccountMsgDebit", DefaultText = "There is a splitted GL Accounts for the chosen multi currency Debit Account. The transactions will be registered in the splitted by currency GL Account", LocalDefaultText = "לכרטיס זה מוגדר פיצול מטבעות . התנועה תרשם על הכרטיס שמתאים למטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CantVoidJouranlItDidntTurnedToTransactions", DefaultText = "Can't void the Jouranl. It did not turned to transactions", LocalDefaultText = "לא ניתן לבטל את פקודת היומן משום שהיא לא הפכה לתנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.RefDateAndAmountAreRequired", DefaultText = "Ref. Date and Amount are required", LocalDefaultText = "תאריך אסמכתא וסכום הינם שדות חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoAutomaticReconcileFoundByThisMethod", DefaultText = "No automatic reconcile found by this method. Please change method or select lines manually", LocalDefaultText = "לא נמצאה התאמה עבור שיטת ההתאמה שנבחרה. יש לשנות שיטת התאמה או להתאים ידנית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoReconcile", DefaultText = "No Reconcile", LocalDefaultText = "לא נמצא תוצאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AutomaticReconcileWillClearAllSelectedLines", DefaultText = "Automatic Reconcile will clear all selected lines. continue?", LocalDefaultText = "השורות הנבחרות ימחקו. האם להמשיך?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.SelectTwoTransactionAtLeast", DefaultText = "Please select at least two transactions in order to create a new external reconciliation", LocalDefaultText = "חובה לבחור לפחות שתי תנועות ע''מ ליצור התאמה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxLineTitle", DefaultText = "New tax deduction period", LocalDefaultText = "תקופת ניכוי חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MustBeLess", DefaultText = "From date must be less than to date", LocalDefaultText = "מ-תאריך חייב להיות קטן מ-עד תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MustBeLarger", DefaultText = "To date must be larger than from date", LocalDefaultText = "עד תאריך חייב להיות גדול מ-תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxDeduction", DefaultText = "Tax deduction details", LocalDefaultText = "פירוט ניכויים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.WithholdingBlocked", DefaultText = "The Vendor does not have a certificate according to the 1000 System", LocalDefaultText = "לספק לא קיים אישור על פי מערכת 1000", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.WithholdingLineDisabled", DefaultText = "A new line was entered with the same date by the 1000 System", LocalDefaultText = "נקלטה שורה חדשה עם תאריך זהה על ידי מערכת 1000", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReferenceDateIsRequired", DefaultText = "Reference date is required", LocalDefaultText = "שדה תאריך הוא שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AmountIsRequired", DefaultText = "Amount is required", LocalDefaultText = "שדה סכום הוא חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.EditTaxPeriod", DefaultText = "Edit tax deduction period", LocalDefaultText = "עריכת תקופת ניכוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BalanceOfCashbookUnequalZeroCantBlocked", DefaultText = "The balance of the cashbook is unequal to zero, can’t be blocked", LocalDefaultText = "יתרת הקופה שונה מאפס, לא ניתן לחסום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoCashInCashbook", DefaultText = "There are no cash in the cashbook", LocalDefaultText = "אין מזומנים בקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoChequesInCashbook", DefaultText = "There are no cheques in the cashbook", LocalDefaultText = "אין המחאות בקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FutureYear", DefaultText = "Future Year!", LocalDefaultText = "!שנה עתידית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoExchangeRateForLocalCurrency", DefaultText = "There is no exchange rate definition for local currency", LocalDefaultText = "חסרה הגדרת שער המרה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.PeriodLinesIsNotCreated", DefaultText = "The period lines for %Year is not created yet, do you want it to be created?", LocalDefaultText = "לא נוצרו עדיין רשומות לשנת %Year , האם לצור؟", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Create", DefaultText = "Create", LocalDefaultText = "יצירה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MonthNotEndedCantClosed", DefaultText = "The month is not ended, can’t be closed", LocalDefaultText = "החודש לא הסתיים, לא ניתן לסגור אותו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.LineActivated", DefaultText = "Line Activated", LocalDefaultText = "שורה הופעלה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.LineDeactivated", DefaultText = "Line - deactivated", LocalDefaultText = "שורה מספר - נחסמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReconcileAccordingTo", DefaultText = "Reconcile according to", LocalDefaultText = "התאמה לפי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FiltersSelected", DefaultText = "Filters selected", LocalDefaultText = "מסננים נבחרו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Last7days", DefaultText = "Last 7 days", LocalDefaultText = "7 ימים אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Lastmonth", DefaultText = "Last month", LocalDefaultText = "חודש אחרון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Last3months", DefaultText = "Last 3 months", LocalDefaultText = "3 חודשים אחרונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Lastyear", DefaultText = "Last year", LocalDefaultText = "שנה אחרונה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FromDate", DefaultText = "From Date", LocalDefaultText = "מתאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoReconciliationFound", DefaultText = "There is no reconciliation found", LocalDefaultText = "לא נמצאו התאמות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ThereRTransactions4GLAccountCantUpdated", DefaultText = "There are transactions for the current GL Account, therefore , it can’t be updated", LocalDefaultText = "ישנם תנועות בכרטיס הנה”ח המקושר, ולכן לא ניתן לעדכן את הכרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails()
            {
                Code = "Accounting.O.NewReconcileWithAdjusment",
                DefaultText = "The difference must be equal to zero, In this case, new reconcile with adjusment will be created",
                LocalDefaultText = "ההפרש חייב להיות שווה לאפס, במקרה כזה תיווצר התאמה עם תיקון שיוצר פקודת יומן חדשה",
                ObjectTableId = objectTable.Id,
                Tenant = 0,
                TextCodeTypeCode = "O",
            }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.OutOfDeposit", DefaultText = "Out of Deposit", LocalDefaultText = "הוצאה מהפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.OutOfDepositMSG", DefaultText = "The selected cheque will be out of deposite and returned to cashbook", LocalDefaultText = "ההמחאות שנבחרו יוצאו מההפקדה ויוחזרו לקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MissingDefaultPercentage", DefaultText = "Missing default tax withholding percentage in accounting settings", LocalDefaultText = "חסרה הגדרת מערכת לאחוז ניכוי מס במקור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FullAccountingSettings", DefaultText = "Full Accounting Settings", LocalDefaultText = "הגדרות הנהלת חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ControlGLAccounts", DefaultText = "Control GL Accounts", LocalDefaultText = "כרטיסים מרכזים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Custom", DefaultText = "Custom", LocalDefaultText = "מותאם אישית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Cashbook", DefaultText = "Cashbook", LocalDefaultText = "קופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Customer", DefaultText = "Customer", LocalDefaultText = "לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AccountingPeriods", DefaultText = "Accounting Periods", LocalDefaultText = "תקופות חשבונאיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BackFromAutoRecoMSG", DefaultText = "#number transactions to reconcile was found", LocalDefaultText = "נמצאו #number תנועות להתאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BackFromAutoRecoBACK", DefaultText = "Back", LocalDefaultText = "חזור להתאמה ידנית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AreconcileOfXTransactionCreated", DefaultText = "A reconcile of #Number transaction was created", LocalDefaultText = "בוצעה התאמה עבור #Number תנועו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Nolineswereenteredonbank", DefaultText = "No lines were entered on the bank page", LocalDefaultText = "לא הוזנו שורות בדף הבנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DetailedControlAccountForCustomer", DefaultText = "Detailed for customer", LocalDefaultText = "פירוט לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DetailedControlAccountForVendor", DefaultText = "Detailed for vendor", LocalDefaultText = "פירוט ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CurrencyDetailed", DefaultText = "Currency detailed", LocalDefaultText = "פירוט לפי מטבעות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxableTransactions", DefaultText = "Taxable Transactions", LocalDefaultText = "עסקאות חייבות במע”מ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ExemptTransactions", DefaultText = "Exempt Transactions", LocalDefaultText = "עסקאות פטורות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AllTransactions", DefaultText = "All Transactions", LocalDefaultText = "כל העסקאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.InputsEquipments", DefaultText = "Inputs - Equipments", LocalDefaultText = "תשומות - ציוד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.InputsOther", DefaultText = "Inputs - Other", LocalDefaultText = "תשומות - אחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Included", DefaultText = "Included", LocalDefaultText = "לכלול", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Line", DefaultText = "Line", LocalDefaultText = "שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CurrencyDetailed", DefaultText = "Currency detailed", LocalDefaultText = "פירוט לפי מטבעות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReportExist", DefaultText = "There is already report for the chosen month", LocalDefaultText = "ישנו דוח לחודש הנבחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.HigherMonthReport", DefaultText = "There’s a report with a higher month", LocalDefaultText = "קיים כבר דוח עם חודש גבוה יותר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReportWithClosedMonth", DefaultText = "Report month Month is not closed , please check Accounting Periods", LocalDefaultText = "לא ניתן להפיק דוח מע\" על חודש שעדיין לא נסגר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WantToCancelTaxReport", DefaultText = "Are you sure you want to cancel this report?", LocalDefaultText = "האם אתה בטוח שאתה רוצה לבטל דוח זה؟", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
		    AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Noautorecofoundbymethodchangemethod", DefaultText = "No automatic reconcile found by this method, Please change method or select lines manually", LocalDefaultText = "לא נמצאו תנועות להתאמה, יש לשנות שיטת התאמה או להתאים ידנית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.EditLine", DefaultText = "Edit Line Number", LocalDefaultText = "ערוך שורה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxReportErrorMsg", DefaultText = "Cannot approve report, there are #Number errors", LocalDefaultText = "קיימות #Number שגיאות - יש לתקנם לפני שידור הדוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Inactive", DefaultText = "Inactive", LocalDefaultText = "חסום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CantIncludeTwoOrMorePayment", DefaultText = "Can’t include more than one payment in the same reconciliation", LocalDefaultText = "לא ניתן לכלול יותר מקבלה אחת באותה התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ClientActivationMSG", DefaultText = "Client is not yet activated in accounting system", LocalDefaultText = "הלקוח עדיין לא הופעל במערכת הנהלת חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ClientActivationBTN", DefaultText = "Activate", LocalDefaultText = "הפעל כעת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MultiRecoCreated", DefaultText = "Reconciliations created successfully", LocalDefaultText = "התאמות בוצעו בהצלחה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.RedeemedChequeMSG", DefaultText = "The cheque is redeemed and can’t be out of deposit, cancel the external reconcilaition in order to return the cheque to the cashbook", LocalDefaultText = "לא ניתן להוציא את ההמחאה מההפקדה משום שהיא בסטטוס נפרע, יש לבטל את ההתאמה החיצונית ע”מ להחזיר את ההמחאה לקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.IntegrityChecks", DefaultText = "Accounting Integrity Checks", LocalDefaultText = "Accounting Integrity Checks", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CustomersNotes", DefaultText = "Customer's Notes", LocalDefaultText = "הערות לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NewAccountingNote", DefaultText = "New Accounting Note", LocalDefaultText = "הזן הערה חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CompletedReportExist", DefaultText = "Can’t create tax report for the the chosen month, there is no tax report for the previous month", LocalDefaultText = "לא ניתן להפיק דוח מעמ לחודש הנבחר משום שלא הופק דוח מעמ לחודש הקודם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NotCompletedReportExist", DefaultText = "Can’t create tax report for the chosen month, the report of the previous month is not completed", LocalDefaultText = "לא ניתן להפיק דוח לחודש הנבחר, הדוח של החודש הקודם לא הושלם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.InvoiceNumber", DefaultText = "Invoice Number", LocalDefaultText = "מספר חשבונית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.InvoiceAmount", DefaultText = "Invoice Amount", LocalDefaultText = "סכום חשבונית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.Duedate", DefaultText = "Due date", LocalDefaultText = "תאריך ערך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.Status", DefaultText = "Status", LocalDefaultText = "סטטוס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.ReconciliationNumber", DefaultText = "Reco. Number", LocalDefaultText = "מספר התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.Open", DefaultText = "Open", LocalDefaultText = "פתוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.Closed", DefaultText = "Closed", LocalDefaultText = "סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.partiallyOpened", DefaultText = "Partially open", LocalDefaultText = "פתוח חלקית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.selectedinvoicesishigherthanpayamount", DefaultText = "The amount of the selected invoices is higher than the payment amount", LocalDefaultText = "סכום החשבוניות שנבחרו גבוה מסכום הקבלה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.ReconciledAmount", DefaultText = "Reconciled Amount", LocalDefaultText = "סכום שהותאם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.invoiceAmount2reconcileMSG", DefaultText = "The amount to reconcile in the invoice is higher than the invoice open amount", LocalDefaultText = "הסכום להתאמה גדול מהסכום הפתוח בחשבונית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.paymentAmount2reconcileMSG", DefaultText = "The amount to reconcile in the invoices is higher than the payment open amount", LocalDefaultText = "הסכום להתאמה גדול מהסכום הפתוח בקבלה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.PaymentAmount", DefaultText = "Payment amount", LocalDefaultText = "סכום קבלה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.ReconciledAmount", DefaultText = "Reconciled amount", LocalDefaultText = "סכום שהותאם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.Amount2Reconcile", DefaultText = "Amount to reconcile", LocalDefaultText = "סכום להתאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.xxxxxx", DefaultText = "xxxxxxxx", LocalDefaultText = "yyyyyyyy", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ARP.xxxxxx", DefaultText = "xxxxxxxx", LocalDefaultText = "yyyyyyyy", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DocumentTypeNotFound", DefaultText = "Document type X not found", LocalDefaultText = "סוג מסמך X לא נמצא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DeductionFileNumberNotFound", DefaultText = "Deduction file number not found", LocalDefaultText = "לא הוקלד מספר תיק ניכויים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Rebuild", DefaultText = "Rebuild", LocalDefaultText = "בניה מחדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Building", DefaultText = "Building...", LocalDefaultText = "...בניין", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AuthoritiesReports", DefaultText = "Authorities Reports", LocalDefaultText = "דוחות לרשויות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.856Report", DefaultText = "856 Report", LocalDefaultText = "דוח 856", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenFormat", DefaultText = "Open Format", LocalDefaultText = "דוח מבנה אחיד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails()
            {
                Code = "Accounting.General.O.Generate1000",
                DefaultText = "Generate a vendor file for the system 1000",
                LocalDefaultText = "הפקת קובץ ספקים למערכת 1000",
                ObjectTableId = objectTable.Id,
                Tenant = 0,
                TextCodeTypeCode = "O",
            }, textCodeRepository, textcodes);
            
            AddTextCodes.AddTextCode(new TextCodeDetails()
            {
                Code = "Accounting.General.O.Receiving1000",
                DefaultText = "Receiving file withholding tax system 1000",
                LocalDefaultText = "קליטת קובץ ניכוי מס מערכת 1000",
                ObjectTableId = objectTable.Id,
                Tenant = 0,
                TextCodeTypeCode = "O",
            }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingPeriods", DefaultText = "Accounting Periods", LocalDefaultText = "תקופות חשבונאיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingPeriodsDefinition", DefaultText = "Accounting Periods Definition", LocalDefaultText = "הגדרת תקופות חשבונאיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Fix", DefaultText = "Fix", LocalDefaultText = "תקן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.GLAccounts", DefaultText = "GL Accounts", LocalDefaultText = "כרטיסים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ShipmentsAndMasters", DefaultText = "Shipments & Masters", LocalDefaultText = "תיקים וגו’בים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.xxxxxx", DefaultText = "xxxxxxxx", LocalDefaultText = "yyyyyyyy", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.YearTransfer", DefaultText = "Year Transfer", LocalDefaultText = "העברת שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TaxReport", DefaultText = "Tax Report", LocalDefaultText = "דוח מעמ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.APPaymentDueDate", DefaultText = "Due Date", LocalDefaultText = "תאריך פרעון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LoadBankPage", DefaultText = "Load Bank Page", LocalDefaultText = "טען דפי בנק מקובץ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Customers", DefaultText = "Customers", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Vendors", DefaultText = "Vendors", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);



            #region MainMenu


            #endregion
            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_JournalActionType
        private void LoadTextCodes_JournalActionType(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "JournalActionType" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.AccountingTables", DefaultText = "Accounting Tables", LocalDefaultText = "טבלאות הנהלת חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.JournalActionTypes", DefaultText = "Journal Actions", LocalDefaultText = "סוגי פעולה של פקודות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalActionType.Q.JournalActionType", DefaultText = "Journal Action Types", LocalDefaultText = "סוגי פעולה של פקודות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalActionType.O.CodeAlreadyExists", DefaultText = "Existing code", LocalDefaultText = "הקוד קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            
            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_ChartOfAccounts
        private void LoadTextCodes_ChartOfAccounts(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "ChartOfAccount" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.ChartOfAccounts", DefaultText = "Chart of Accounts", LocalDefaultText = "לוח חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.Q.ChartOfAccounts", DefaultText = "Chart of Accounts", LocalDefaultText = "לוח חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.O.CodeAlreadyExists", DefaultText = "The Code is used by another Chart Of Account", LocalDefaultText = "הקוד קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.O.CannotBeItself", DefaultText = "Cannot be a parent of itself", LocalDefaultText = "קבוצת מאזן לא יכולה לשמש קוד אב של עצמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.O.ParentDoesNotExist", DefaultText = "Parent does not exist", LocalDefaultText = "קוד אב לא קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.O.WrongParentType", DefaultText = "The type of parent chart of account differs from this chart of account", LocalDefaultText = "סוג קבוצת מאזן שונה מסוג של קוד אב", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ChartOfAccounts.O.ParentIsChild", DefaultText = "Can't connect to this account since it's already defined as a  child for the current account", LocalDefaultText = "לא ניתן לקשר כרטיס זה כאב מכיוון שהוא מוגדר כבר כבן לכרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            
            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_GLAccounts
        private void LoadTextCodes_GLAccounts(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.GLAccounts", DefaultText = "General Ledger Accounts", LocalDefaultText = "חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Clients", DefaultText = "Client Accounts", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.GLAccounts", DefaultText = "General Ledger Accounts", LocalDefaultText = "חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Clients", DefaultText = "All Customers", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Multi", DefaultText = "Multi", LocalDefaultText = "רב מטבעי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Active", DefaultText = "Active", LocalDefaultText = "פעיל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Inactive", DefaultText = "Inactive", LocalDefaultText = "חסום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.DisplayNumberAlreadyExists", DefaultText = "Existing number - choose another one", LocalDefaultText = "מספר קיים - יש לבחור במספר אחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.InternalNumberAlreadyExists", DefaultText = "The Internal Number exists with another GL Account", LocalDefaultText = "המספר הפנימי קיים בחשבון אחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ClientMultiAlreadyExists", DefaultText = "A multi-currency account already exists for the Client - please choose a currrency", LocalDefaultText = "ללקוח קיים כבר חשבון רב-מטבעי - אנא בחר מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ClientCurrencyAlreadyExists", DefaultText = "Client account already exists in ", LocalDefaultText = "ללקוח קיים כבר חשבון במטבע ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.VendorMultiAlreadyExists", DefaultText = "A multi-currency account already exists for the Vendor - please choose a currrency", LocalDefaultText = "לספק קיים כבר חשבון רב-מטבעי - אנא בחר מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.VendorCurrencyAlreadyExists", DefaultText = "Vendor account already exists in ", LocalDefaultText = "ללקוח קיים כבר חשבון במטבע ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.LedgerTransactionExists", DefaultText = "Ledger account transactions exist", LocalDefaultText = "לכרטיס יש תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.LedgerTransactionManyExist", DefaultText = "This account has transactions with different currencies", LocalDefaultText = "לכרטיס ישנם תנועות במטבעות שונים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ParentAccountNotFound", DefaultText = "Parent account not found", LocalDefaultText = "לא נמצא חשבון אב", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CustomerAccountNotFound", DefaultText = "Customer account not found", LocalDefaultText = "לא נמצא חשבון לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ChartOfAccountsNotFound", DefaultText = "Chart of accounts not found", LocalDefaultText = "לא נמצא לוח חשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CardRevExpOnly", DefaultText = "A Card may ber Revenue or Expense only", LocalDefaultText = "כרטיס יכול להיות הוצאות או הכנסות בלבד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.RevExpOther", DefaultText = "Revenue or Expense permitted for a Card only", LocalDefaultText = "שדה סוג חשבון נדרש רק עבור כרטיסי הכנסות או הוצאות"/*"הוצאות והכנסות מותר לכרטיסים בלבד"*/, ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.WrongParentType", DefaultText = "Type of the Chart of Accounts differs from this GL Account", LocalDefaultText = "סוג קבוצת מאזן שונה מסוג של חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CustomerAccountMissing", DefaultText = "Customer Account is missing", LocalDefaultText = "חסר חשבון לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ControlAccountNotDefined", DefaultText = "The Account is defined as a Control Account but is not connected to the Full Accounting Settings", LocalDefaultText = "החשבון מוגדר כמרכז אבל חסר בהגדרות הנהלת החשבונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ControlAccountMissing", DefaultText = "Control Account is missing", LocalDefaultText = "חסר חשבון מרכז", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ControlAccountNotFound", DefaultText = "Control Account is missing", LocalDefaultText = "חשבון מרכז לא נמצא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.WrongControlChartType", DefaultText = "Control Account's Chart of Accounts Type differs from this GL Account", LocalDefaultText = "סוג קבוצת מאזן של המרכז שונה מזה של חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AccountTypeCodeMissing", DefaultText = "Account Type Code is missing", LocalDefaultText = "חסר קוד של סוג חשבון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.WrongCustomerAccountType", DefaultText = "Wrong Customer Account Type", LocalDefaultText = "סוג חשבון לקוח שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CurrencyOrMulti", DefaultText = "Currency or Multi is a must", LocalDefaultText = "מטבע או רב מטבעי - חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AccountIsaMulti", DefaultText = "The account is defined as multi currency account", LocalDefaultText = "החשבון מוגדר כחשבון רב-מטבעי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ControlMulti", DefaultText = "Control account must be a multi currency account, would you like to make it a multi currency account?", LocalDefaultText = "חשבון מרכז חייב להיות רב מטבעי, האם ברצונך להפוך אותו לחשבון רב מטבעי?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AmountToReconcileTooBig", DefaultText = "Amount to reconcile is too big", LocalDefaultText = "סכום ההתאמה גדול מדי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AmountToReconcileWrongSign", DefaultText = "Amount to reconcile has wrong sign", LocalDefaultText = "סכום ההתאמה בסימן שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ReconcileSearch", DefaultText = "Search by:", LocalDefaultText = "חפש לפי:", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ReconcileSearchHelpText", DefaultText = " Journal\n Accounting Entity Ref.\n Ref.1\n Ref.2\n Ref.3", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "H", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.YearTransferSucceed", DefaultText = " Year Transfer Succeed", LocalDefaultText = "מעבר שנה הסתיים בהצלחה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "H", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.RevaluationSucceed", DefaultText = " Revaluation Succeed", LocalDefaultText = "שערוך הסתיים בהצלחה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "H", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.MultiCurrencyForSplitted", DefaultText = "The GL Account must be defined as multi currency in order to create splitted GL Accounts by currency", LocalDefaultText = "הכרטיס חייב להיות מוגדר כרב מטבעי ע”מ ליצור כרטיסים מפוצלים לפי מטבעות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CurrencyExists", DefaultText = "This customer has GL Account with the chosen currency. choose another one", LocalDefaultText = "ללקוח קיים כרטיס במטבע הנבחר. נא לבחור מטבע אחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ConnectedGLAccounts", DefaultText = "Connected GL Accounts", LocalDefaultText = "כרטיסים מקושרים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.SplittedByCurrency", DefaultText = "Splitted by Currency", LocalDefaultText = "פיצול לפי מטבע", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Children", DefaultText = "Children GL Accounts", LocalDefaultText = "כרטיסי בנים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Choose", DefaultText = "Choose", LocalDefaultText = "בחר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Add", DefaultText = "Add", LocalDefaultText = "הוסף", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.NotValidDate", DefaultText = "Can’t enter two identical periods", LocalDefaultText = "לא ניתן להזין שתי תקופות חופפות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AccountingBalance", DefaultText = "Accounting Balance", LocalDefaultText = "יתרה חשבונאית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.DispalyTransactions", DefaultText = "Dispaly transactions", LocalDefaultText = "הצג תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.TotalDue", DefaultText = "Total Due", LocalDefaultText = "יתרה לגביה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.NextDueDate", DefaultText = "Next due date", LocalDefaultText = "תאריך פרעון הבא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.OpenTransactions", DefaultText = "Open Transactions", LocalDefaultText = "תנועות פתוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Reconcile", DefaultText = "Reconcile", LocalDefaultText = "בצע התאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.LastTransactions", DefaultText = "Last Transactions", LocalDefaultText = "תנועות אחרונות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Date", DefaultText = "Date", LocalDefaultText = "תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Collecting", DefaultText = "Collecting", LocalDefaultText = "גביה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.Accounting", DefaultText = "Accounting", LocalDefaultText = "חשבונאית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.MustBeMultiCurrency", DefaultText = "Parent GLAccount must be multi currency", LocalDefaultText = "כרטיס אב חייב להיות רב מטבעי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.O.CheckInvoiceDate", DefaultText = "Invoice Date can't be bigger than the Accounting Date", LocalDefaultText = "לא ניתן להקליד תאריך אסמכתא מאוחר מהתאריך החשבונאי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);




            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.WrongOpenMonth", DefaultText = "Wrong open month", LocalDefaultText = "חודש פתוח שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.WrongClosedMonth", DefaultText = "Wrong closed month", LocalDefaultText = "חודש סגור שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.FutureMonthForbidden", DefaultText = "Future month is not allowed", LocalDefaultText = "לא ניתן להגדיר חודש עתידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.ClosedAfterOpen", DefaultText = "Closed month later than open month is not allowed", LocalDefaultText = "לא ניתן להגדיר חודש סגור אחרי חודש פתוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.therearejournalsdidnottranslated", DefaultText = "Can’t close this month. there are journals that did not translated into transactions for this month", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.O.ClosedMonth", DefaultText = "Closed month", LocalDefaultText = "חודש סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Transactions.F.WithAttachAccount", DefaultText = "With Attach Account", LocalDefaultText = "הצג חשבונות קשורים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Transactions.F.WithoutAdjustedTransactions", DefaultText = "Without adjusted Transactions", LocalDefaultText = "ללא תנועות מותאמות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            
            //    AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Clients", DefaultText = "Client Accounts", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Clients", DefaultText = "Client Accounts", LocalDefaultText = "לקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Vendors", DefaultText = "Vendor Accounts", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Vendors", DefaultText = "Vendor Accounts", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Suppliers", DefaultText = "Suppliers", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Suppliers", DefaultText = "Suppliers", LocalDefaultText = "ספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.NewClient", DefaultText = "New Client Account", LocalDefaultText = "לקוח חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.NewVendor", DefaultText = "New Vendor Account", LocalDefaultText = "ספק חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.F.DisplayNumberLabel", DefaultText = "Account No.: ", LocalDefaultText = "מספר חשבון: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.F.EnglishNameLabel", DefaultText = "Account Name: ", LocalDefaultText = "שם חשבון: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.F.CurrencyCodeLabel", DefaultText = "Currency: ", LocalDefaultText = "מטבע: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.F.ReconcileMethodNameLabel", DefaultText = "Reconcile Method: ", LocalDefaultText = "שיטת התאמה: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.F.InternalNumberLabel", DefaultText = "System No.: ", LocalDefaultText = "מספר מערכת: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.FutureDateForbidden", DefaultText = "Future date is not allowed", LocalDefaultText = "לא ניתן להגדיר תאריך עתידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.Revaluation", DefaultText = "Revaluation", LocalDefaultText = "שערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.RevaluationError", DefaultText = "Revaluation error: ", LocalDefaultText = "שגיאת התאמה: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.RateNotFound", DefaultText = " exchange rate not found for ", LocalDefaultText = " שער המרה לא נמצא לתאריך ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.DiffAccountNotDefined", DefaultText = "Difference account not defined", LocalDefaultText = "לא מודגר חשבון להפרשים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.DataMissing", DefaultText = "Chart of accounts or an account or 'Revaluation Enabled' is missing", LocalDefaultText = "חסרים: קבוצת מאזן או חשבון או 'מאופשר שערוך'", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluations.Q.OpenRevaluations", DefaultText = "Other open revaluations exist", LocalDefaultText = "קיימים שערוכים אחרים פתוחים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journals.O.ExternalNoAlreadyExists_1", DefaultText = "Journal with External Number ", LocalDefaultText = "פקודת יומן עם מספר חיצוני ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journals.O.ExternalNoAlreadyExists_2", DefaultText = " from ", LocalDefaultText = " מ- ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journals.O.ExternalNoAlreadyExists_3", DefaultText = " exists already", LocalDefaultText = "כבר קיימת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journals.O.ExternalNoAlreadyExists_4", DefaultText = " as a Journal No. ", LocalDefaultText = " כפקודת יומן מספר ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.LocalCurrencyErr", DefaultText = "The reconcile method for multi currency GLAaccount must be local currency", LocalDefaultText = "שיטת ההתאמה עבור כרטיסים רב מטבעיים היא במטבע מקומי בלבד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.GLAccountIsControl", DefaultText = "This GL Account is defined as control account", LocalDefaultText = "כרטיס זה מוגדר ככרטיס מרכז", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.RequiredFields", DefaultText = "Fill the required fields", LocalDefaultText = "נא למלא שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AgingDetails", DefaultText = "Aging Details", LocalDefaultText = "נתוני גיול בש”ח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ChartOfAccountCantChangedGLAhaveTrans", DefaultText = "The chart of account can’t be changed, the GL account have transactions", LocalDefaultText = "לא ניתן לשנות את קבוצת המאזן לכרטיס שיש בו תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.GLAParentValidation1", DefaultText = "GLAccount and its parent must be same chart of account type", LocalDefaultText = "סוג קבוצת מאזן עבור הכרטיס וכרטיס האב שמקושר אליו חייב להיות זהה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.GLAParentValidation2", DefaultText = "GLAccount and its parent must be same chart of account", LocalDefaultText = "קבוצת מאזן עבור הכרטיס ווכרטיס האב שמקושר אליו חייבת להיות זהה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.ReconcileMethodcantUpdated", DefaultText = "The reconcile method can’t be updated, the GLAccount has transations", LocalDefaultText = "לא ניתן לעדכן שיטת התאמה, נרשמו תנועות על הכרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.filter_accounting", DefaultText = "Accounting", LocalDefaultText = "חשבונאי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.filter_reference", DefaultText = "Document", LocalDefaultText = "אסמכתא", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.filter_due", DefaultText = "Due", LocalDefaultText = "פרעון", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CreditDetails", DefaultText = "Credit Details", LocalDefaultText = "נתוני אשראי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CreditLimit", DefaultText = "Credit Limit", LocalDefaultText = "מסגרת אשראי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.AccountingBalance", DefaultText = "Accounting Balance", LocalDefaultText = "יתרה חשבונאית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.CardIndex", DefaultText = "Card Index", LocalDefaultText = "הצג תנועות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.FutureChequesToday", DefaultText = "Open Cheques", LocalDefaultText = "המחאות שלא נפרעו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.DisplayChequelist", DefaultText = "Display Cheque list", LocalDefaultText = " הצג רשימת המחאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.FutureCheques", DefaultText = "Future Open Cheques", LocalDefaultText = "המחאות עתידיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.TotalOpenShipments", DefaultText = "Open Shipments", LocalDefaultText = " תיקים פתוחים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.fromfieldrequired", DefaultText = "From date field is required", LocalDefaultText = "מתאריך שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.tofieldrequired", DefaultText = "To data field is required", LocalDefaultText = "עד תאריך שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.O.xxxx", DefaultText = "xxxxx", LocalDefaultText = "yyyyy", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            // ..... GLAccount Queries ....

            //  Main query
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllGLAccounts", DefaultText = "All GL Accounts", LocalDefaultText = "כל הכרטיסים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveGLAccounts", DefaultText = "Active GL Account", LocalDefaultText = "כרטיסים פעילים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InActiveGLAccounts", DefaultText = "Inactive GL Account", LocalDefaultText = "כרטיסים לא פעילים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.OpenFiles", DefaultText = "Open Files", LocalDefaultText = "תקים פתוחים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ClosedFiles", DefaultText = "Closed Files", LocalDefaultText = "תיקים סגורים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllFiles", DefaultText = "All Files", LocalDefaultText = "כל התיקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllJobs", DefaultText = "All Masters", LocalDefaultText = "כל הג’ובים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            //  Customers query
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllCustomers", DefaultText = "All Customers", LocalDefaultText = "כל הלקוחות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Collectors", DefaultText = "My Customers (As Collectors)", LocalDefaultText = "לקוחות שלי (כגובה)", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.debetors", DefaultText = "Debtors Customers", LocalDefaultText = "לקוחות חייבים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveCustomers", DefaultText = "Active Customers", LocalDefaultText = "לקוחות פעילים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InactiveCustomers", DefaultText = "Blocked Customers", LocalDefaultText = "לקוחות חסומים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            //  Vendors query
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Collectors", DefaultText = "My Customers (As Collectors)", LocalDefaultText = "My Customers (As Collectors)", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.debetors", DefaultText = "Debtors Customers", LocalDefaultText = "Debtors Customers", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveVendors", DefaultText = "Active Vendors", LocalDefaultText = "ספקים פעילים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InactiveVendors", DefaultText = "Inactive Vendors", LocalDefaultText = "ספקים חסומים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllVendors", DefaultText = "All Vendors", LocalDefaultText = "כל הספקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);


            // Reconciliations
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.CreatedByUser", DefaultText = "Created by User", LocalDefaultText = "נוצר על ידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.ReconciliationNumber", DefaultText = "Reconciliation No.", LocalDefaultText = "התאמה מספר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.CreateDate", DefaultText = "Create Date", LocalDefaultText = "תאריך פתיחה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.ManageReconciliations", DefaultText = "Manage Reconciliations", LocalDefaultText = "ניהול התאמות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.FromDateLabel", DefaultText = "From Date: ", LocalDefaultText = "מתאריך: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.F.ToDateLabel", DefaultText = "To: ", LocalDefaultText = "עד: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.Q.OnlyFirstRecords", DefaultText = "Only the first ", LocalDefaultText = "מוצגות רק ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.Q.OnlyFirstRecords_Ending", DefaultText = " records are shown. Please refine your search to get less results.", LocalDefaultText = " רשומות ראשונות. נא לחדד את החיפוש שלך כדי לקבל פחות תוצאות.", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.Q.SearchWarning", DefaultText = "Search Warning", LocalDefaultText = "אזהרה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.Q.reconciliationwassavedas", DefaultText = "The reconciliation was successfully saved as draft, and it will be dispalyed next time entering the the screen.", LocalDefaultText = "ההתאמה נשמרה כטיוטה והיא תוצג ברגע שנכנסים למסך התאמות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.Q.ThereisUncompletedReconciliation", DefaultText = "There is uncompleted reconciliation, do you want to complete it?", LocalDefaultText = "קיימת התאמה שלא הושלמה, האם תרצה להשלים אותה?", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.O.AmountMustBSmaller2OpenAmount", DefaultText = "The amount must be smaller or equal to open amount", LocalDefaultText = "הסכום חייב להיות קטן או שווה לסכום הפתוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Reconciliations.O.ErrorsInSelectedLines", DefaultText = "There are errors in the selected reconciliation lines", LocalDefaultText = "קיימת הודעת שגיאה בשורת ההתאמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            // Postdated cheque redemption
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Cheques.Q.ChequeNotDeposited", DefaultText = "Cheque has not been deposited", LocalDefaultText = "המחאה לא הופקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            // Aging report
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.AgingForDate", DefaultText = "Aging For Date", LocalDefaultText = "גיול לתאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.Customer", DefaultText = "Customer", LocalDefaultText = "לקוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.NumberMonthsBackwards", DefaultText = "Number of months backwards", LocalDefaultText = "מספר חודשים אחורנית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.CurrenciesDetailed", DefaultText = "Currencies Detailed", LocalDefaultText = "פירוט מטבעות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.RunReport", DefaultText = "Run Report", LocalDefaultText = "הרץ דוח", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgingReport.O.FutureDate", DefaultText = "Future date", LocalDefaultText = "תאריך עתידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            // GLAccount Transactions Report
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLTransactionReport.O.GLAccountNo", DefaultText = "GL Account No.", LocalDefaultText = "מספר כרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLTransactionReport.O.GLAccountZrequierd", DefaultText = "GL Account field is requierd", LocalDefaultText = "חובה למלא את השדה מספר כרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLTransactionReport.O.WithClosedTransactions", DefaultText = "With Closed Transactions", LocalDefaultText = "כלול תנועות סגורות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);






            #region Screens

            AddTextCodes.AddTextCode(new TextCodeDetails() { TextCodeTypeCode = "S", Code = "GLAccounts.S.Transactions.Transactions", DefaultText = "Transactions", ObjectTableId = objectTable.Id, Tenant = 0 }, textCodeRepository, textcodes);

            #endregion

        }
         #endregion


        #region LoadTextCodes_AccountingPeriods
        private void LoadTextCodes_AccountingPeriods(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "AccountingPeriod" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.AccountingPeriods", DefaultText = "Accounting Periods", LocalDefaultText = "תקופות חשבונאיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.Q.AccountingPeriods", DefaultText = "Accounting Periods", LocalDefaultText = "תקופות חשבונאיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.Q.AccountingPeriodMng", DefaultText = "Accounting Period", LocalDefaultText = "ניהול תקופה חשבונאית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.F.Year", DefaultText = "Year", LocalDefaultText = "שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.F.YearLabel", DefaultText = "Year: ", LocalDefaultText = "שנה: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
        }
        #endregion


        #region Catgories
        private void LoadTextCodes_Catgories(Dictionary<string, TextCode> textcodes)
        {
            //Category1
            ObjectTable category1ObjectTable = objectContext.ObjectTables.Where(f => f.Name == "Category1" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Category1", DefaultText = "Category 1", LocalDefaultText = "Category 1", ObjectTableId = category1ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //Category2
            ObjectTable category2ObjectTable = objectContext.ObjectTables.Where(f => f.Name == "Category2" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Category2", DefaultText = "Category 1", LocalDefaultText = "Category 1", ObjectTableId = category2ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //Category3
            ObjectTable category3ObjectTable = objectContext.ObjectTables.Where(f => f.Name == "Category3" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Category3", DefaultText = "Category 1", LocalDefaultText = "Category 1", ObjectTableId = category3ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //Category4
            ObjectTable category4ObjectTable = objectContext.ObjectTables.Where(f => f.Name == "Category4" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Category4", DefaultText = "Category 1", LocalDefaultText = "Category 1", ObjectTableId = category4ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            //Category5
            ObjectTable category5ObjectTable = objectContext.ObjectTables.Where(f => f.Name == "Category5" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Category5", DefaultText = "Category 1", LocalDefaultText = "Category 1", ObjectTableId = category5ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);


         }
        #endregion


        #region LoadTextCodes_YearTransfer
        private void LoadTextCodes_YearTransfer(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.YearTransfer", DefaultText = "Year Transfer", LocalDefaultText = "מעבר שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.Q.YearTransfer", DefaultText = "Year Transfer", LocalDefaultText = "מעבר שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.F.Year", DefaultText = "Year", LocalDefaultText = "שנה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.F.YearLabel", DefaultText = "Year: ", LocalDefaultText = "שנה: ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "YearTransfer.O.RevenueExpenseType", DefaultText = "A year transfer account is undefined or is not configured correctly.", LocalDefaultText = "חשבון להעברת שנה אינו מוגדר או אינו מוגדר תקין.", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
        }
        #endregion



        #region LoadTextCodes_System1000
        private void LoadTextCodes_System1000(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == 0).FirstOrDefault();
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.System1000", DefaultText = "System 1000", LocalDefaultText = "מערכת 1000", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "System1000.Q.System1000", DefaultText = "System 1000", LocalDefaultText = "מערכת 1000", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "System1000.O.DeductionFileNumber", DefaultText = "Deduction File Number is undefined.", LocalDefaultText = "מספר תיק ניכויים אינו מוגדר.", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
        }
        #endregion


        #region LoadTextCodes_AutomaticReconcileMethod
        private void LoadTextCodes_AutomaticReconcileMethod(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "AutomaticReconcileMethod" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.AutomaticReconcileMethods", DefaultText = "Automatic Reconcile Methods", LocalDefaultText = "התאמות אוטומטיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.Q.AutomaticReconcileMethod", DefaultText = "Automatic Reconcile Methods", LocalDefaultText = "שיטות התאמה אוטומטית", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.O.CodeAlreadyExists", DefaultText = "Existing code", LocalDefaultText = "הקוד קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AutomaticReconcileMethod.O.uniqueMethods", DefaultText = "There is Reconcile Method with the same Automatic Reconcile", LocalDefaultText = "There is reconcile method with the same automatic reconcile", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);


            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_Journal
        private void LoadTextCodes_Journal(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "Journal" && f.Tenant == 0).FirstOrDefault();
          

            
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Journal", DefaultText = "Journal", LocalDefaultText = "פקודת יומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.Journal", DefaultText = "All Journal", LocalDefaultText = "כל פקודות היומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.ApprovedJournal", DefaultText = "Approved Journals", LocalDefaultText = "פקודות יומן מאושרות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.DraftJournal", DefaultText = "Draft Journals", LocalDefaultText = "פקודות יומן בסטטוס טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.SavedJournal", DefaultText = "Waiting for Approval Journals", LocalDefaultText = "פקודות יומן מחכות לאישור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.ExternalJournals", DefaultText = "External Journals", LocalDefaultText = " פקודות יומן חיצוניות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.AutoCreatedJournals", DefaultText = "Auto Created Journals", LocalDefaultText = " פקודות יומן אוטומטיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.TheAccountingDayMustBeInRange", DefaultText = "The accounting day must be in the range of the accounting month.", LocalDefaultText = "היום החשבונאי שהוקלד אינו קיים בטווח ימי החודש החשבונאי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.AccDay", DefaultText = "Acc. Day", LocalDefaultText = "יום חשבונאי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            

            #region Screens

            AddTextCodes.AddTextCode(new TextCodeDetails() { TextCodeTypeCode = "S", Code = "Journal.S.Details.Details", DefaultText = "Details", ObjectTableId = objectTable.Id, Tenant = 0 }, textCodeRepository, textcodes);

            #endregion



            #region Column Headers
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Line", DefaultText = "Line", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.ActionCode", DefaultText = "Action Code", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DocumentDate", DefaultText = "Document Date", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DueDate", DefaultText = "Due Date", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.CreditAccountName", DefaultText = "C.Account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DebitAccountName", DefaultText = "D.Account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Currency", DefaultText = "Currency", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.LocalAmount", DefaultText = "Amount (%InvoiceCurrencyCode)", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.ForeignAmount", DefaultText = "F.Amount", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference1", DefaultText = "Ref.1", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference2", DefaultText = "Ref.2", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference3", DefaultText = "Ref.3", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Notes", DefaultText = "Notes", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);


               #endregion


            #region Messages
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ForeignAmountNotZero", DefaultText = "Foreign amount is empty", LocalDefaultText ="סכום במטח הינו חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.LocalAmountNotZero", DefaultText = "Local amount is empty", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",
                LocalDefaultText="סכום במטבע מקומי הינו חובה "
            }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.YouShouldHaveOneLineAtLeast", DefaultText = "There must be at least one journal line", LocalDefaultText= "חובה להזין לפחות שורת פקודת יומן אחת", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.CurrenyNotMatched", DefaultText = "Account Currncy does not equal to selected currecy code", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", LocalDefaultText ="מטבע הכרטיס לא תואם את המטבע הנבחר"}, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.JournalAmountNotMatched", DefaultText = "Journal credit amount does not match the debit amount", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", LocalDefaultText = "סכום חובה שונה מסכום זכות "}, textCodeRepository, textcodes);
          //  AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.JournalLinesAmountNotZero", DefaultText = "Invoice line amount field must not be zero", ObjectTableId = objectTableId, Tenant = 0, TextCodeTypeCode = "M", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ExchangeRateEmpty", DefaultText = "Exchange rate is not defined", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",
                LocalDefaultText="לא הוגדר שער המרה"
            }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeCredit", DefaultText = "Please select a credit account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",
                LocalDefaultText ="נא לבחור כרטיס זכות"}, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.JLAccountingDateMustWithinJournalMonth",
                LocalDefaultText="תאריך בשורה חייב להיות בטווח של החודש החשבונאי של פ היומן",
                DefaultText = "Jornal Line Accounting Date must be within Accounting month of Journal", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.FutureDateForbidden", DefaultText = "Future date is not allowed", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", LocalDefaultText= "לא ניתן  להקליד תאריך עתידי "}, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.AccountIsBlocked", DefaultText = "GL Account (%name) is inactive", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M", LocalDefaultText = "כרטיס (%name) חסום" }, textCodeRepository, textcodes);
            


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeDebit", DefaultText = "Please select a debit account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText= "אנא בחר כרטיס חובה" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeCreditAndCredit", DefaultText = "Please select a credit and a debit account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="אנא בחר כרטיס זכות וכרטיס חובה" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeNotMatched", DefaultText = "Action Code does not Matched the account you picked", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="שגיאה" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCode", DefaultText = "Please Select Action Code", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="אנא בחר ציין את את סוג השורה  חובה/זכות" }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DocumentDateBiggerDueDate", DefaultText = "Document date must be earlier then due date ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="התאריך החשבונאי חייב להיות מוקדם מתאריך האסמכתא" }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DueDateMustgreaterthancurrent", DefaultText = "Due Date ,Must be Equal or greater than current date ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="תאריך הפרעון צריך להיות גדול או שווה מהתאריך הנוכחי" }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.Journal.currencydoesnotexist", DefaultText = "Currency does not exist", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="המטבע לא קיים" }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ControlAccountIdIsMust", DefaultText = "Account Which is not a card must Control Account definition", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="חשבון שאיננו כרטיס תפעולי חייב להיות כרטיס מרכז" }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ControlAccountIdIsNotMatch", DefaultText = "Control Account Is Not Match", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="חשבון מרכז איננו תואם" }, textCodeRepository, textcodes);



            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DueDateIsMust", DefaultText = "Due Date Is Must", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "M",LocalDefaultText="תאריך פרעון הינו חובה" }, textCodeRepository, textcodes);


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.FAMltiExchangerateNELA",
                                                            DefaultText = "Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})",
                                                            LocalDefaultText = "סכום במטבע מקומי חייב להיות שווה לסכום במטבע זר כפול שער המרה",
                                                            ObjectTableId = objectTable.Id,
                                                            Tenant = 0,
                                                            TextCodeTypeCode = "M",
            }, textCodeRepository, textcodes);



            AddTextCodes.AddTextCode(new TextCodeDetails()
            {
                Code = "Journal.M.AllDateMustInit",
                DefaultText = "All dates variable must initialize",
                ObjectTableId = objectTable.Id,
                Tenant = 0,
                TextCodeTypeCode = "M",
                LocalDefaultText = "כל התאריכים חייבים אתחול"
            }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.YouShouldSelectTwoTransactions", DefaultText = "You should select at lease two transactions in order to create new reconcile", LocalDefaultText= "יש לבחור לפחות שתי תנועות על מנת ליצור התאמה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);


            #endregion


            #region Other
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CodeShort", DefaultText = "Journal Code Too Short", LocalDefaultText = "פרט מכס קצר מידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CodeLong", DefaultText = "Journal Code Too Long", LocalDefaultText = "פרט המכס ארוך מדי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CorrectDigit", DefaultText = "Check digit is incorrect ,the correct digit is ", LocalDefaultText = " ספרת הביקורת שגויה , הספרה הנכונה היא ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            #endregion

            objectContext.SaveChanges();


        }
        #endregion


        #region LoadTextCodes_JournalLine
        private void LoadTextCodes_JournalLine(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "JournalLine" && f.Tenant == 0).FirstOrDefault();


            #region Other
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalLine.O.AccountCurrency", DefaultText = "Journal Code Too Short", LocalDefaultText = "פרט מכס קצר מידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalLine.O.CodeLong", DefaultText = "Journal Code Too Long", LocalDefaultText = "פרט המכס ארוך מדי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "JournalLine.O.CorrectDigit", DefaultText = "Check digit is incorrect ,the correct digit is ", LocalDefaultText = " ספרת הביקורת שגויה , הספרה הנכונה היא ", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            #endregion

            objectContext.SaveChanges();


        }
         #endregion
 
        
        #region LoadTextCodes_BankCode
        private void LoadTextCodes_BankCode(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "BankCode" && f.Tenant == 0).FirstOrDefault();



            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.BankCodes", DefaultText = "Bank Codes", LocalDefaultText = "קודי בנקים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
           


            #region Screens
       
        #endregion



            #region Column Headers

            #endregion


            #region Messages

            #endregion


            #region Other

            #endregion

            objectContext.SaveChanges();

      
        }

        #endregion


        #region LoadTextCodes_CashBook
        private void LoadTextCodes_CashBook(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "CashBook" && f.Tenant == 0).FirstOrDefault();

            // Filters
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.B.All", DefaultText = "All", LocalDefaultText = "הכל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.B.CashCheques", DefaultText = "Cash Cheques", LocalDefaultText = "המחאות מזומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.B.PostdatedCheques", DefaultText = "Postdated Cheques", LocalDefaultText = "המחאות דחויות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_BankDeposit
        private void LoadTextCodes_BankDeposit(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "BankDeposit" && f.Tenant == 0).FirstOrDefault();



            #region Menu Deposit 

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.BankDeposits", DefaultText = "Bank Deposits", LocalDefaultText = "הפקדות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            #endregion

            #region Deposit.Details Tab

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = " Deposit.Details.O.Details", DefaultText = "Details", LocalDefaultText = "פרטי הפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = " Deposit.General.O.Details", DefaultText = "General", LocalDefaultText = "הגדרות הפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Deposit.Details.B.CancelDeposit", DefaultText = "Cancel Deposit", LocalDefaultText = "ביטול הפקדה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            #endregion

            #region Column Headers
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.Line", DefaultText = "Line", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.DepositId", DefaultText = "Deposit", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.ARPaymentChequeId", DefaultText = "ARPayment Cheque", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.DueDate", DefaultText = "Due Date", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.IsOutOfDeposit", DefaultText = "Out Of Deposit", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.OutOfDepositeDate", DefaultText = "Out Of Deposite Date", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.Currency", DefaultText = "Currency", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.LocalAmount", DefaultText = "Amount (%InvoiceCurrencyCode)", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.ForeignAmount", DefaultText = "Foreign Amount", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.ChequeNumber", DefaultText = "Cheque No.", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.Bank", DefaultText = "Bank", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.BankAccountNumber", DefaultText = "Bank Account", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.Notes", DefaultText = "Notes", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.Branch", DefaultText = "Branch", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.CH.ARPayment", DefaultText = "ARPayment", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            
            #endregion

            // BankDeposit
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.Today", DefaultText = "Today's Deposits", LocalDefaultText = "הפקדות מהיום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.cash", DefaultText = "Cash Deposit", LocalDefaultText = "הפקדות מזומן", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.chequeDeposit", DefaultText = "Cheque Deposit", LocalDefaultText = "הפקדות המחאות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.O.DepositAmountmustbelessthanCashbook", DefaultText = "Deposit amount should be less than or equal cashbook total", LocalDefaultText = "סכום ההפקדה צריך להיות קטן או שווה לסכום בקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.O.DepositCancelChequeMSG", DefaultText = "The deposit can’t be cancelled, at least one one cheques have been redeemed, you should cancel the external reconciliation in order to return the cheque to the cashbook", LocalDefaultText = "לא ניתן לבטל את ההפקדה, משום שהיא מכילה לפחות המחאה אחת שנפרעה, יש לבטל את ההתאמה החיצונית ע”מ להחזיר את ההמחאה לקופה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);




            objectContext.SaveChanges();
        }
         #endregion


        #region LoadTextCodes_Revaluation
        private void LoadTextCodes_Revaluation(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "Revaluation" && f.Tenant == 0).FirstOrDefault();



            #region Menu Deposit

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Revaluations", DefaultText = "Revaluations", LocalDefaultText = "שערוכים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            #endregion

            #region Deposit.Details Tab

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = " Revaluation.Details.O.Details", DefaultText = "Details", LocalDefaultText = "פרטי שערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = " Revaluation.General.O.Details", DefaultText = "General", LocalDefaultText = "הגדרות שערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.Details.B.CancelRevaluation", DefaultText = "Cancel Revaluation", LocalDefaultText = "ביטול שערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            #endregion


            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.CashBooks", DefaultText = "Cash Books", LocalDefaultText = "שערוכים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.O.GLAccountForRevaluation", DefaultText = "You must choose GL Accounts for revaluation", LocalDefaultText = "חובה לבחור כרטיסים לשערוך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.O.ChartAccountForRevaluation", DefaultText = "You must choose Chart of account for revaluation", LocalDefaultText = "שדה קבוצת מאזן הוא שדה חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.O.FutureDateIsNotAllowed", DefaultText = "Future Date!", LocalDefaultText = "!תאריך עתידי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);



            #region Column Headers
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.CH.Id", DefaultText = "Revaluation Id", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.CH.RevaluationNumber", DefaultText = "Revaluation Number", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.CH.RevaluationDate", DefaultText = "Revaluation Date", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Revaluation.CH.RevaluationEnabled", DefaultText = "Revaluation Enabled", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", }, textCodeRepository, textcodes);

            #endregion

            objectContext.SaveChanges();
        }
        #endregion


        #region LoadTextCodes_BankAccount
        private void LoadTextCodes_BankAccount(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "BankAccount" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.BankAccounts", DefaultText = "Bank Accounts", LocalDefaultText = "חשבונות בנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankAccounts.O.CurrencyGLAccountAndDefferredMustSame", DefaultText = "The currency of the GL Account and the defferred GL Account must be the same", LocalDefaultText = "המטבע של הכרטיס צריך להיות זהה למטבע של כרטיס הדחויים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankAccounts.O.CancelPage", DefaultText = "Cancel Page", LocalDefaultText = "ביטול דף בנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankAccounts.O.CantCancelItsNotLastApproved", DefaultText = "Can’t cancel the bank page. It’s not the last approved page", LocalDefaultText = "לא ניתן לבטל דף בנק זה משום שהוא אינו הדף המאושר האחרו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankAccounts.O.CanCancelItsTransactionsReconciled", DefaultText = "Can’t cancel the bank page. It’s transactions have been reconciled", LocalDefaultText = "לא ניתן לבטל דף בנק זה משום שהתנועות שלו הותאמו כבר", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);

            objectContext.SaveChanges();



        }

        #endregion


        #region LoadTextCodes_ReconcileExternalPage
        private void LoadTextCodes_ReconcileExternalPage(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = objectContext.ObjectTables.Where(f => f.Name == "ReconcileExternalPage" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.PreviuosBankPage", DefaultText = "Previuos page", LocalDefaultText = "דף קודם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.CantNewBankPageDraft", DefaultText = "Can’t create new bank page, The previous page is not approved", LocalDefaultText = "לא ניתן ליצור דף בנק חדש, הדף הקודם עדיין בסטטוס טיוטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate", DefaultText = "From date should be bigger than the previous bank page to date", LocalDefaultText = "שדה מתאריך חייב להיות גדול מתאריך סגירה של דף קודם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.ToDateShouldBiggerFromDate", DefaultText = "To date should bigger/equal to from date", LocalDefaultText = " עד תאריך חייב להיות גדול/שווה מ-תאריך", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.PageApprovedCantCancel", DefaultText = "The bank page is approved and can’t be cancelled", LocalDefaultText = "דף הבנק בסטטוס מאושר ולא ניתן לבטל אותו", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance", DefaultText = "The start balance of the bank page should be equal the close balance of the pervious page", LocalDefaultText = "יתרת פתיחה של הדף חייבת להיות שווה ליתרת סגירה של דף קודם", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.RefDateShouldBiggerOrSmaller", DefaultText = "The reference date should be bigger or equal than start date and smaller or equal than end date", LocalDefaultText = "תאריך האסמכתא חייב להיות גדול/שווה לתאריך התחלה של הדף וקטן/שווה לתאריך סיום של הדף", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.StartBalanceNotEqualEndBalance", DefaultText = "Start balance + lines doesn't equal to End Balance", LocalDefaultText = "סכום יתרת פתיחה + שורות לא תואם ליתרת סגירה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.Transactions", DefaultText = "Transactions", LocalDefaultText = "תנועות הכרטיס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.BankAccountTransactions", DefaultText = "Bank Account Transactions", LocalDefaultText = "תנועות הבנק", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.SumOfXRowsSelected", DefaultText = "Total of %Number rows selected", LocalDefaultText = "סה”כ %Number שורות נבחרות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.Difference", DefaultText = "Difference", LocalDefaultText = "הפרש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReconcileExternalPage.O.Sum", DefaultText = "Sum", LocalDefaultText = "סכום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, textCodeRepository, textcodes);



            objectContext.SaveChanges();
        }

        #endregion

        #endregion




        //  ________________________________
        // |                                |
        // |            QUERIES             |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V

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
  
          

            
            #region ObjectFieldsLists
            List<ObjectField> journalActionTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "JournalActionType").ToList();
            List<ObjectField> chartOfAccountsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ChartOfAccount").ToList();
            List<ObjectField> journalObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Journal").ToList();
            List<ObjectField> gLAccountsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "GLAccount").ToList();
            List<ObjectField> accountingPeriodsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AccountingPeriod").ToList();
            List<ObjectField> automaticReconcileMethodObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AutomaticReconcileMethod").ToList();
            List<ObjectField> bankdepositsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "BankDeposit").ToList();
            #region closed tables
            List<ObjectField> journalStatusTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "JournalStatusType").ToList();
            List<ObjectField> journalTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "JournalType").ToList();
            List<ObjectField> automaticReconcileObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AutomaticReconcile").ToList();
            #endregion

            #endregion




            #region ObjectTables
            ObjectTable journalActionTypeObject = objectContext.ObjectTables.Where(d => d.Name == "JournalActionType" && d.Tenant == 0).FirstOrDefault();
            ObjectTable chartOfAccountObject = objectContext.ObjectTables.Where(d => d.Name == "ChartOfAccount" && d.Tenant == 0).FirstOrDefault();
            ObjectTable journalObject = objectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();
            ObjectTable gLAccountObject = objectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();
            ObjectTable accountingPeriodObject = objectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault();
            ObjectTable automaticReconcileMethodObject = objectContext.ObjectTables.Where(d => d.Name == "AutomaticReconcileMethod" && d.Tenant == 0).FirstOrDefault();
            ObjectTable bankdepositObject = objectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault();

            #region closed Tables
            ObjectTable journalStatusTypeObject = objectContext.ObjectTables.Where(d => d.Name == "JournalStatusType" && d.Tenant == 0).FirstOrDefault();
            ObjectTable journalTypeObject = objectContext.ObjectTables.Where(d => d.Name == "JournalType" && d.Tenant == 0).FirstOrDefault();
            ObjectTable automaticReconcileObject = objectContext.ObjectTables.Where(d => d.Name == "AutomaticReconcile" && d.Tenant == 0).FirstOrDefault();
            #endregion
 
            #endregion




            #region QueryGroups
            QueryGroup journalActionTypeGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "JACT", Name = "JournalActionType" }, queryGroupRepository);
            QueryGroup chartOfAccountsGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CHAC", Name = "ChartOfAccount" }, queryGroupRepository);
            QueryGroup journalGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "JNAC", Name = "Journal" }, queryGroupRepository);
            QueryGroup gLAccountsGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "GLAC", Name = "GLAccount" }, queryGroupRepository);
            QueryGroup accountingPeriodsGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ACPR", Name = "AccountingPeriod" }, queryGroupRepository);
            QueryGroup clientGLAccountsGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CLAC", Name = "ClientGLAccount" }, queryGroupRepository);
            QueryGroup vendorGLAccountsGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "VNAC", Name = "VendorGLAccount" }, queryGroupRepository);
            QueryGroup automaticReconcileMethodGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ARCM", Name = "AutomaticReconcileMethod" }, queryGroupRepository);
            QueryGroup yearTransferGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "YTRN", Name = "YearTransfer" }, queryGroupRepository);
            //QueryGroup bankDepositGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "BNKD", Name = "BankDeposit" }, queryGroupRepository);



            #region closed tables
            QueryGroup journalStatusTypeGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "JSTT", Name = "JournalStatusType" }, queryGroupRepository);
            QueryGroup journalTypeGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "JNLT", Name = "JournalType" }, queryGroupRepository);
            QueryGroup automaticReconcileGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "AURC", Name = "AutomaticReconcile" }, queryGroupRepository);
            queryGroupRepository.SubmitChanges();
            #endregion

            #endregion

            queryGroupRepository.SubmitChanges();




            #region Features

            #region JournalActionType features
            Feature journalActionTypeFeature = tenantFeatures.Where(d => d.Code == "JOURNALACTIONTYPES" && d.FeatureTypeCode == "QUER").FirstOrDefault(); 
            #endregion

            #region ChartOfAccount features
            Feature chartOfAccountFeature = tenantFeatures.Where(d => d.Code == "CHARTOFACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            #endregion

            #region Journal features
            Feature journalFeature = tenantFeatures.Where(d => d.Code == "JOURNAL" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ExternalJournalsFeature = tenantFeatures.Where(d => d.Code == "ExternalJournals" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ApprovedJournalFeature = tenantFeatures.Where(d => d.Code == "ApprovedJournal" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature DraftJournalFeature = tenantFeatures.Where(d => d.Code == "DraftJournal" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature SavedJournalFeature = tenantFeatures.Where(d => d.Code == "SavedJournal" && d.FeatureTypeCode == "QUER").FirstOrDefault();
          
            #endregion


            #region GLAccount features
            Feature allgLAccountFeature = tenantFeatures.Where(d => d.Code == "ALLGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            //Feature gLAccountFeature = tenantFeatures.Where(d => d.Code == "GLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature clientGLAccountFeature = tenantFeatures.Where(d => d.Code == "CLIENTGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature vendorGLAccountFeature = tenantFeatures.Where(d => d.Code == "VENDORGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature accountingPeriodFeature = tenantFeatures.Where(d => d.Code == "ACCOUNTINGPERIODS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature yearTransferFeature = tenantFeatures.Where(d => d.Code == "YEARTRANSFER" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            // Main GLAccount
            Feature activeGLAccountFeature = tenantFeatures.Where(d => d.Code == "ACTIVEGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature inactiveGLAccountFeature = tenantFeatures.Where(d => d.Code == "INACTIVEGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature OpenFilesFeature = tenantFeatures.Where(d => d.Code == "OPENFILESGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ClosedFilesFeature = tenantFeatures.Where(d => d.Code == "CLOSEDFILESGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature AllFilesFeature = tenantFeatures.Where(d => d.Code == "ALLFILESGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature AllJobsFeature = tenantFeatures.Where(d => d.Code == "ALLJOBSGLACCOUNTS" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            // Customers GLAccount
            Feature collectorsFeature = tenantFeatures.Where(d => d.Code == "collectorsGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature debetorsFeature = tenantFeatures.Where(d => d.Code == "debetorsGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature activeCustomerFeature = tenantFeatures.Where(d => d.Code == "activeCustomersGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature inactiveCustomerFeature = tenantFeatures.Where(d => d.Code == "inactiveCustomersGla" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            // Vendors GLAccount
            //Feature collectorsFeature = tenantFeatures.Where(d => d.Code == "collectorsGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            //Feature debetorsFeature = tenantFeatures.Where(d => d.Code == "debetorsGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature activeVendorFeature = tenantFeatures.Where(d => d.Code == "activeVendorsGLA" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature inactiveVendorFeature = tenantFeatures.Where(d => d.Code == "inactiveVendorsGla" && d.FeatureTypeCode == "QUER").FirstOrDefault();

            #endregion

            #region BankDeposit features
            Feature TodayBankDepositFeature = tenantFeatures.Where(d => d.Code == "TodayBankDeposit" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature CashBankDepositFeature = tenantFeatures.Where(d => d.Code == "CashBankDeposit" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            Feature ChequeBankDepositFeature = tenantFeatures.Where(d => d.Code == "ChequeBankDeposit" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            #endregion

            #region AutomaticReconcileMethod features
            Feature automaticReconcileMethodFeature = tenantFeatures.Where(d => d.Code == "AUTORECOMETHODS" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            #endregion

       

            #region closed tables
            #endregion

            #endregion


            #region JournalActionType queries
            Query journalActionTypes = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "JournalActionType.Q.JournalActionType" && d.ObjectTableId == journalActionTypeObject.Id).FirstOrDefault().Id, Code = "Journal Action Types", QueryGroupCode = journalActionTypeGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalActionTypeObject.Id, QuerySection = "JournalActionType", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = journalActionTypeFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn journalActionTypes1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = journalActionTypes.Id, IndexOrder = 0, ObjectFieldId = journalActionTypeObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == journalActionTypeObject.Id).FirstOrDefault().Id, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn journalActionTypes2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = journalActionTypes.Id, IndexOrder = 1, ObjectFieldId = journalActionTypeObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == journalActionTypeObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn journalActionTypes3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = journalActionTypes.Id, IndexOrder = 2, ObjectFieldId = journalActionTypeObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == journalActionTypeObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn journalActionTypes4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = journalActionTypes.Id, IndexOrder = 3, ObjectFieldId = journalActionTypeObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == journalActionTypeObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            #endregion


            #region ChartOfAccounts queries
            Query chartOfAccounts = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "ChartOfAccounts.Q.ChartOfAccounts" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, Code = "Chart of Accounts", QueryGroupCode = chartOfAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = chartOfAccountObject.Id, QuerySection = "ChartOfAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = chartOfAccountFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn chartOfAccounts1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 0, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn chartOfAccounts2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 1, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn chartOfAccounts3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 2, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn chartOfAccounts4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 5, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn chartOfAccounts5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 3, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "ParentName" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn chartOfAccounts6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chartOfAccounts.Id, IndexOrder = 4, ObjectFieldId = chartOfAccountsObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == chartOfAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region Journal queries

            #region AllJournal
            Query Journal = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.Q.Journal" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, Code = "All Journals", QueryGroupCode = journalGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalObject.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = journalFeature.Id, DefaultSortDirection= "Descending", DefaultSortName="CreateDate" }, queriesRepository, tenantQueries);

            QueryColumn Journal1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 0, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 1, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 2, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 3, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 4, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 6, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn Journal7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Journal.Id, IndexOrder = 5, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region ApprovedJournals
            Query ApprovedJournals = AddQueries.AddQuery(new QueryDetails() { DefaultSortDirection = "Descending", DefaultSortName = "CreateDate", NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.Q.ApprovedJournal" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, Code = "Approved Journals", QueryGroupCode = journalGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalObject.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ApprovedJournalFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn ClosedJournal1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 0, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 1, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 2, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 3, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 4, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 6, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedJournal7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournals.Id, IndexOrder = 5, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            

            AdvancedQueryFilter JournalQueryFilter_01 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusCode").FirstOrDefault().Id, PredefinedValue = "2", QueryId = ApprovedJournals.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            //  AdvancedQueryFilter UnpaidInvoicePredefinedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = InvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == InvoiceObject.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = aRInvoiceQuery_UnPaid.Id, Tenant = 0 }, AdvancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region DraftJournals
            Query DraftJournals = AddQueries.AddQuery(new QueryDetails() { DefaultSortDirection = "Descending", DefaultSortName = "CreateDate", NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.Q.DraftJournal" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, Code = "Draft Journals", QueryGroupCode = journalGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalObject.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DraftJournalFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn OpenedJournal1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 0, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 1, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 2, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 3, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 4, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 6, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenedJournal7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournals.Id, IndexOrder = 5, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            
            AdvancedQueryFilter JournalQueryFilter_02 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusCode").FirstOrDefault().Id, PredefinedValue = "0", QueryId = DraftJournals.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            // Waiting for approved
            #region SavedJournals
            Query SavedJournals = AddQueries.AddQuery(new QueryDetails() { DefaultSortDirection = "Descending", DefaultSortName = "CreateDate", NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.Q.SavedJournal" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, Code = "Saved Journals", QueryGroupCode = journalGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalObject.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = SavedJournalFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn SavedJournal1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 0, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 1, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 2, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 3, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 4, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 5, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn SavedJournal7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournals.Id, IndexOrder = 6, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            
            AdvancedQueryFilter JournalQueryFilter_03 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusCode").FirstOrDefault().Id, PredefinedValue = "1", QueryId = SavedJournals.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region ExternalJournals
            Query ExternalJournals = AddQueries.AddQuery(new QueryDetails() { DefaultSortDirection = "Descending", DefaultSortName = "CreateDate", NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.Q.ExternalJournals" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, Code = "External Journals", QueryGroupCode = journalGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = journalObject.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ExternalJournalsFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn ExternalJournal1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 0, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 1, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 2, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 3, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 4, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "TypeName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 5, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ExternalJournal7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournals.Id, IndexOrder = 6, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == journalObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

            AdvancedQueryFilter JournalQueryFilter_04 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = journalObjectFields.Where(d => d.FieldName == "ExternalSystem").FirstOrDefault().Id, PredefinedValue = "1",Operator="IsNotNull", QueryId = ExternalJournals.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #endregion

            #region AutomaticReconcileMethod queries
            Query automaticReconcileMethods = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AutomaticReconcileMethod.Q.AutomaticReconcileMethod" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, Code = "Automatic Reconcile Methods", QueryGroupCode = automaticReconcileMethodGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = automaticReconcileMethodObject.Id, QuerySection = "AutomaticReconcileMethod", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = automaticReconcileMethodFeature.Id }, queriesRepository, tenantQueries);

            QueryColumn automaticReconcileMethods1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = automaticReconcileMethods.Id, IndexOrder = 0, ObjectFieldId = automaticReconcileMethodObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn automaticReconcileMethods2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = automaticReconcileMethods.Id, IndexOrder = 1, ObjectFieldId = automaticReconcileMethodObjectFields.Where(d => d.FieldName == "AutomaticReconcileName1" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn automaticReconcileMethods3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = automaticReconcileMethods.Id, IndexOrder = 2, ObjectFieldId = automaticReconcileMethodObjectFields.Where(d => d.FieldName == "AutomaticReconcileName2" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn automaticReconcileMethods4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = automaticReconcileMethods.Id, IndexOrder = 3, ObjectFieldId = automaticReconcileMethodObjectFields.Where(d => d.FieldName == "AutomaticReconcileName3" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn automaticReconcileMethods5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = automaticReconcileMethods.Id, IndexOrder = 4, ObjectFieldId = automaticReconcileMethodObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == automaticReconcileMethodObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            #endregion

            #region GLAccounts queries

            #region Customers Workspace

            #region All Customers
            //Query clientGLAccounts = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.Clients" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "All Customers", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = clientGLAccountFeature.Id, Perspective = "GLAccountRecievable" }, queriesRepository, tenantQueries);

            //QueryColumn clientGLAccounts1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn clientGLAccounts7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = clientGLAccounts.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            //AdvancedQueryFilter clientGLAccountPredefinedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "2", QueryId = clientGLAccounts.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region collectors
            Query collectors = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.Collectors" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "MyCustomersAsCollectors", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = collectorsFeature.Id, Perspective = "GLAccountRecievable" }, queriesRepository, tenantQueries);

            QueryColumn collectors1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors45 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn collectors7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = collectors.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter collectorsGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "2", QueryId = collectors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            //AdvancedQueryFilter collectorsGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "IsNotNull", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CollectorId" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = null, QueryId = collectors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region debitors
            Query debetors = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.debetors" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "DebetorsCustomers", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = debetorsFeature.Id, Perspective = "GLAccountRecievable" }, queriesRepository, tenantQueries);

            QueryColumn debetors1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors45 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn debetors7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = debetors.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter debetorsGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "2", QueryId = debetors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter debetorsGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "LargerThan", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "0", QueryId = debetors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region active customers
            Query activeCustomers = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.ActiveCustomers" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "ActiveCustomersGLAccounts", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = activeCustomerFeature.Id, Perspective = "GLAccountRecievable" }, queriesRepository, tenantQueries);

            QueryColumn activeCustomers1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers45 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeCustomers6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeCustomers.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter activeCustomersGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "2", QueryId = activeCustomers.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter activeCustomersGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "false", QueryId = activeCustomers.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region inactive customers
            Query inactiveCustomers = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.InactiveCustomers" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "InactiveCustomersGLAccount", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = inactiveCustomerFeature.Id, Perspective = "GLAccountRecievable" }, queriesRepository, tenantQueries);

            QueryColumn inactiveCustomers1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers45 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveCustomers6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveCustomers.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter inactiveCustomersGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "2", QueryId = inactiveCustomers.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter inactiveCustomersGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = inactiveCustomers.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #endregion

            #region Main Workspace

            // 1- Active GLAccounts: AccountType.GLAccounts=1 && Inactive.GLAccounts= Flase
            // 2- Inactive GLAccounts: AccountType.GLAccounts=1 && Inactive.GLAccounts= True
            // 3- All GLAccounts: AccountType.GLAccounts=1
            // 4- Open Files: AccountType.GLAccounts=5 && BalanceInLocalCurrency <> 0(the OPC for Files will be dimmed)
            // 5- Closed Files: AccountType.GLAccounts=5 && BalanceInLocalCurrency = 0(the OPC for Files will be dimmed)
            // 6- All files: AccountType.GLAccounts=5(the OPC for Files will be dimmed)
            // 7- All Jobs: AccountType.GLAccounts=4(The OPC for jobs will be dimmed)

            #region active glaccount
            Query activeGLAccount = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.ActiveGLAccounts" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "ActiveGLAccounts", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = activeGLAccountFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn activeGLAccount1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeGLAccount9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeGLAccount.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter activeGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "1", QueryId = activeGLAccount.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter activeGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "false", QueryId = activeGLAccount.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region inactive glaccount
            Query inactiveGLAccount = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.InActiveGLAccounts" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "InactiveGLAccounts", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = inactiveGLAccountFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn inactiveGLAccount1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveGLAccount9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveGLAccount.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter inactiveGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "1", QueryId = inactiveGLAccount.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter inactiveGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = inactiveGLAccount.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region All GLAccount
            Query allGLAccounts = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.AllGLAccounts" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "All GLAccounts", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 3, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = allgLAccountFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn allgLAccounts1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allgLAccounts9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allGLAccounts.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter allGLAccountsPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "1", QueryId = allGLAccounts.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region Open Files
            Query OpenFiles = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.OpenFiles" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "OpenFiles", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = OpenFilesFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn OpenFiles1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn OpenFiles9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFiles.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter OpenFilesPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "5", QueryId = OpenFiles.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter OpenFilesPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "NotEqual", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "0", QueryId = OpenFiles.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region Closed Files
            Query ClosedFiles = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.ClosedFiles" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "ClosedFiles", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ClosedFilesFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn ClosedFiles1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles6= AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn ClosedFiles9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFiles.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter ClosedFilesPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "5", QueryId = ClosedFiles.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter ClosedFilesPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "0", QueryId = ClosedFiles.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region All Files
            Query AllFiles = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.AllFiles" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "AllFiles", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AllFilesFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn AllFiles1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllFiles9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFiles.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter AllFilesPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "5", QueryId = AllFiles.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region All Jobs
            Query AllJobs = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.AllJobs" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "AllJobs", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AllJobsFeature.Id, Perspective = "GLAccountMain" }, queriesRepository, tenantQueries);

            QueryColumn AllJobs1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 4, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn AllJobs9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobs.Id, IndexOrder = 8, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter AllJobsPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "4", QueryId = AllJobs.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #endregion

            #region vendors Workspace
            #region all vendors
            Query vendorGLAccounts = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.Vendors" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "Vendor Accounts", QueryGroupCode = vendorGLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = vendorGLAccountFeature.Id, Perspective="GLAccountPayables"}, queriesRepository, tenantQueries);

            QueryColumn allVendors1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn allVendors7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = vendorGLAccounts.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter vendorGLAccountPredefinedFilter = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "3", QueryId = vendorGLAccounts.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region active vendors
            Query activeVendors = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.ActiveVendors" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "ActiveVendorsGLAccounts", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = activeVendorFeature.Id, Perspective = "GLAccountPayables" }, queriesRepository, tenantQueries);

            QueryColumn activeVendors1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn activeVendors7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = activeVendors.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter activeVendorsGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "3", QueryId = activeVendors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter activeVendorsGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "false", QueryId = activeVendors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion

            #region inactive vendors
            Query inactiveVendors = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccounts.Q.InactiveVendors" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, Code = "InactiveVendorsGLAccount", QueryGroupCode = gLAccountsGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = gLAccountObject.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = inactiveVendorFeature.Id, Perspective = "GLAccountPayables" }, queriesRepository, tenantQueries);

            QueryColumn inactiveVendors1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 0, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 2, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 1, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 3, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 5, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 6, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);
            QueryColumn inactiveVendors7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = inactiveVendors.Id, IndexOrder = 7, ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);
            AdvancedQueryFilter inactiveVendorsGLAccountPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "3", QueryId = inactiveVendors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            AdvancedQueryFilter inactiveVendorsGLAccountPredefinedFilter2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = gLAccountsObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == gLAccountObject.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = inactiveVendors.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            #endregion
            
            #endregion

            

            #endregion

            #region Bank Deposit queries

           // #region Today Deposit
            //DateTime now = DateTime.Now;
            //DateTime startOfDay = now.Date;
            //DateTime endOfDay = startOfDay.AddDays(1);

            // filtering will be in client side

         ///*   Query todayDeposit = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "BankDeposit.Q.Today" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, Code = "TodayDeposits", QueryGroupCode = bankDepositGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = bankdepositObject.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TodayBankDepositFeature.Id }, queriesRepository, tenantQueries);

         //   QueryColumn todayDeposit1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 0, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 1, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 2, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 3, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 4, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 5, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn todayDeposit7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = todayDeposit.Id, IndexOrder = 6, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   AdvancedQueryFilter todayDepositPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Between", ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, PredefinedValue = "#today", PredefinedValue2 = "#today", QueryId = todayDeposit.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
         //   #endregion

         //   #region Cash Deposit
         //   Query cashDeposit = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "BankDeposit.Q.cash" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, Code = "cashDeposits", QueryGroupCode = bankDepositGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = bankdepositObject.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CashBankDepositFeature.Id }, queriesRepository, tenantQueries);*/

         //   QueryColumn cashDeposit1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 0, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 1, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 2, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 3, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 4, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 6, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
         //   QueryColumn cashDeposit6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDeposit.Id, IndexOrder = 7, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
         //   AdvancedQueryFilter cashDepositPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, PredefinedValue = "true", QueryId = cashDeposit.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
         //   #endregion

            //#region cheque Deposit
            //Query chequeDeposit = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "BankDeposit.Q.chequeDeposit" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, Code = "chequeDeposit", QueryGroupCode = bankDepositGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = bankdepositObject.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ChequeBankDepositFeature.Id }, queriesRepository, tenantQueries);

            //QueryColumn chequeDeposit1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 0, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 1, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 2, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 3, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 4, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 5, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);
            //QueryColumn chequeDeposit7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDeposit.Id, IndexOrder = 6, ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
            //AdvancedQueryFilter chequeDepositPredefinedFilter1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, Operator = "Equals", ObjectFieldId = bankdepositsObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == bankdepositObject.Id).FirstOrDefault().Id, PredefinedValue = "false", QueryId = chequeDeposit.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);
            //#endregion

            #endregion


          
            objectContext.SaveChanges();
        }




        //  ________________________________
        // |                                |
        // |             SCREENS            |
        // |________________________________|
        //                 |||
        //                 |||
        //                VVVVV
        //                 VVV
        //                  V


        #region Screens

        public void loadScreens()
        {
            objectContext = WebFreightContext.GetContext(0);
            screenFieldsRepository = new ScreenFieldsRepository(objectContext);
            screensRepository = new ScreensRepository(objectContext);
            Dictionary<string, Screen> tenantScreens = screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

            BuildChartOfAccountScreen(tenantScreens, tenantScreenField);
            BuildGLAccountScreen(tenantScreens, tenantScreenField);
            BuildJournalScreens(tenantScreens, tenantScreenField);


            LoadObjectTableRulesANDFieldsValidations();
      


        }
        
        #region BuildChartOfAccountScreen
        private void BuildChartOfAccountScreen(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable chartOfAccountObject = objectContext.ObjectTables.Where(d => d.Name == "ChartOfAccount" && d.Tenant == 0).FirstOrDefault();

            ObjectField codeField = objectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == chartOfAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField englishNameField = objectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == chartOfAccountObject.Id && d.Tenant == 0).FirstOrDefault();


            #region Header Screen
            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ChartOfAccount.HeaderScreen", Name = "Header Screen", ObjectTableId = chartOfAccountObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);


            ScreenField screenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = codeField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            ScreenField screenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = englishNameField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);

            chartOfAccountObject.HeaderScreenId = headerScreen.Id;
            #endregion

        }
        #endregion



        #region BuildGLAccountScreen
        private void BuildGLAccountScreen(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable gLAccountObject = objectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();

            ObjectField displayNumberField = objectContext.ObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField englishNameField = objectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField localNameField = objectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField currencyNameField = objectContext.ObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField reconcileMethodField = objectContext.ObjectFields.Where(d => d.FieldName == "ReconcileMethodName" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField activeStatusNameField = objectContext.ObjectFields.Where(d => d.FieldName == "ActiveStatusName" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField internalNumberField = objectContext.ObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == gLAccountObject.Id && d.Tenant == 0).FirstOrDefault();


            #region Header Screen
            // → moved to generated tool
            //Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "GLAccount.HeaderScreen", Name = "Header Screen", ObjectTableId = gLAccountObject.Id, NumberOfColumns = 6, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);


            //ScreenField screenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = displayNumberField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            //ScreenField screenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = localNameField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            //ScreenField screenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = currencyNameField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            //ScreenField screenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = reconcileMethodField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            //ScreenField screenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = activeStatusNameField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);
            //ScreenField screenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 5, Row = 0, ObjectFieldId = internalNumberField.Id, ScreenId = headerScreen.Id, Tenant = 0 }, screenFieldsRepository, tenantScreenFields);

            //// ↓↓↓↓
            //gLAccountObject.HeaderScreenId = headerScreen.Id;
            #endregion

        }
        #endregion


        #region BuildJournalScreens
        public void BuildJournalScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable JournalObject = objectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();

            ObjectField JournalCreateDate = objectContext.ObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalCreateBy = objectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalCreateByName = objectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalUpdateDate = objectContext.ObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalUpdateBy = objectContext.ObjectFields.Where(d => d.FieldName == "UpdatedByUserId" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalApproveDate = objectContext.ObjectFields.Where(d => d.FieldName == "ApproveDate" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalApprovedBy = objectContext.ObjectFields.Where(d => d.FieldName == "ApprovedByUserId" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalNumber = objectContext.ObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalStatus = objectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalOriginal = objectContext.ObjectFields.Where(d => d.FieldName == "OriginalJournalName" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalAccountingEntityReference = objectContext.ObjectFields.Where(d => d.FieldName == "AccountingEntityReference" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalAccountingEntityName = objectContext.ObjectFields.Where(d => d.FieldName == "AccountingEntityName" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalexNumberField = objectContext.ObjectFields.Where(d => d.FieldName == "ExternalNo" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField JournalexSystemField = objectContext.ObjectFields.Where(d => d.FieldName == "ExternalSystem" && d.ObjectTableId == JournalObject.Id && d.Tenant == 0).FirstOrDefault(); 

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Journal.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = JournalObject.Id, NumberOfColumns = 3, NumberOfRows = 3 }, screensRepository, tenantScreens);
          

            //general tab
            ScreenField JournalCreateDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = JournalCreateDate.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0,}, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalCreatedByScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = JournalCreateBy.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalUpdateDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = JournalUpdateDate.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalUpdatedByScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = JournalUpdateBy.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalApproveDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = JournalApproveDate.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalApprovedByScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = JournalApprovedBy.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            
            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Journal.HeaderScreen", Name = "Header Screen", ObjectTableId = JournalObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);

            //col1
            ScreenField JournalNumberHeaderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = JournalNumber.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalCreatedDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = JournalCreateDate.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            //col2
            ScreenField JournalStatusHeaderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = JournalStatus.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalcreatedbyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = JournalCreateByName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            //col3
            ScreenField JournalOriginalHeaderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = JournalOriginal.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalapprovedateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = JournalApproveDate.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            //col4
            ScreenField JournalReferenceScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = JournalAccountingEntityReference.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalJournalAccountingEntityNameFild = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = JournalAccountingEntityName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            //col5
            ScreenField JournalexNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = JournalexNumberField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField JournalexSystemScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = JournalexSystemField.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);


            JournalObject.HeaderScreenId = HeaderScreen.Id;

            objectContext.SaveChanges();
        }
        #endregion


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
            Dictionary<string, RuleConditionField> TenantRuleConditionFields = ruleConditionFieldRepository.GetRuleConditionFieldsByTenant(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldId, a => a);
            List<ObjectFieldValidation> TenantObjectFieldValidations = objectFieldValidationRepository.GetObjectFieldValidations(0).ToList();

            // load rules
        }
        #endregion


        public void LoadObjectTableTabs()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableTabsRepository = new ObjectTableTabRepository(objectContext);
            textCodeRepository = new TextCodeRepository(objectContext);

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();
            Dictionary<string, ObjectTableTab> TenantObjectTableTabs = objectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);
            FeatureRepository featureRepository = new FeatureRepository(0);
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();


            #region ObjectTables
            ObjectTable ChartOfAccountTable = objectContext.ObjectTables.Where(f => f.Name == "ChartOfAccount" && f.Tenant == 0).FirstOrDefault();
            ObjectTable JournalTable = objectContext.ObjectTables.Where(f => f.Name == "Journal" && f.Tenant == 0).FirstOrDefault();
            ObjectTable GLAccountTable = objectContext.ObjectTables.Where(f => f.Name == "GLAccount" && f.Tenant == 0).FirstOrDefault();
            ObjectTable AccountingPeriodTable = objectContext.ObjectTables.Where(f => f.Name == "AccountingPeriod" && f.Tenant == 0).FirstOrDefault();
            ObjectTable AutomaticReconcileMethodTable = objectContext.ObjectTables.Where(f => f.Name == "AutomaticReconcileMethod" && f.Tenant == 0).FirstOrDefault();
            ObjectTable JournalActionTypeTable = objectContext.ObjectTables.Where(f => f.Name == "JournalActionType" && f.Tenant == 0).FirstOrDefault();

            ObjectTable Category1Table = objectContext.ObjectTables.Where(f => f.Name == "Category1" && f.Tenant == 0).FirstOrDefault();
            ObjectTable Category2Table = objectContext.ObjectTables.Where(f => f.Name == "Category2" && f.Tenant == 0).FirstOrDefault();
            ObjectTable Category3Table = objectContext.ObjectTables.Where(f => f.Name == "Category3" && f.Tenant == 0).FirstOrDefault();
            ObjectTable Category4Table = objectContext.ObjectTables.Where(f => f.Name == "Category4" && f.Tenant == 0).FirstOrDefault();
            ObjectTable Category5Table = objectContext.ObjectTables.Where(f => f.Name == "Category5" && f.Tenant == 0).FirstOrDefault();

            ObjectTable CashBookTable = objectContext.ObjectTables.Where(f => f.Name == "CashBook" && f.Tenant == 0).FirstOrDefault();
            ObjectTable BankDepositTable = objectContext.ObjectTables.Where(f => f.Name == "BankDeposit" && f.Tenant == 0).FirstOrDefault();
            ObjectTable RevaluationTable = objectContext.ObjectTables.Where(f => f.Name == "Revaluation" && f.Tenant == 0).FirstOrDefault();

            #endregion

            //#region ChartOfAccount
            //Feature ChartOfAccountGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == ChartOfAccountTable.Id).FirstOrDefault();
            //Feature ChartOfAccountEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == ChartOfAccountTable.Id).FirstOrDefault();

            ////tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = ChartOfAccountGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.CHART.ChartOfAccountGeneralTabControl", ObjectTableId = ChartOfAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "ChartOfAccount.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "CAGC", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = ChartOfAccountEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ChartOfAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "ChartOfAccount.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "CAEV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            //#endregion



            #region Journal
            // Feature JournalGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == JournalTable.Id).FirstOrDefault();
            Feature JournalEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == JournalTable.Id).FirstOrDefault();
            Feature JournalDETAILSFeature = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == JournalTable.Id).FirstOrDefault();

            //tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = JournalGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.JRNL.JournalGeneralTabControl", ObjectTableId = JournalTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "JNGC", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = JournalEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = JournalTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "JNEV", Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentName = "JournalDetailsTabComponent", HtmlComponentUrl = "./Accounting/Components/EditTabs/Journal/JournalDetailsTabComponent", FeatureId = JournalDETAILSFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.JRNL.JournalDetailsTabControl", ObjectTableId = JournalTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Journal.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Code = "JNDT", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);


            
            
            
            
            #endregion






            #region GLAccount
            //Feature GLAccountNEWFeature = tenantFeatures.Where(d => d.Code == "NEW" && d.ObjectTableId == ChartOfAccountTable.Id).FirstOrDefault();
            Feature GLAccountGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountTRANSACTIONSFeature = tenantFeatures.Where(d => d.Code == "TRANSACTIONS" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountRECONCILEFeature = tenantFeatures.Where(d => d.Code == "RECONCILE" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountMANAGERECONCILIATIONSFeature = tenantFeatures.Where(d => d.Code == "MANAGERECONCILIATIONS" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountAdditionalSFeature = tenantFeatures.Where(d => d.Code == "ADDITIONAL" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
            Feature GLAccountTaxWithholdingFeature = tenantFeatures.Where(d => d.Code == "TAX" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();

            //tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = GLAccountNEWFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.NewGLAccountControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.New" && d.Tenant == 0).FirstOrDefault().Id, Code = "GAAN", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountTransactionsTabComponent", HtmlComponentName = "GLAccountTransactionsTabComponent", FeatureId = GLAccountTRANSACTIONSFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.GLAccountTransactionsTabControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.Transactions" && d.Tenant == 0).FirstOrDefault().Id, Code = "GATR", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountGeneralTabComponent", HtmlComponentName = "GLAccountGeneralTabComponent", FeatureId = GLAccountGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.GLAccountGeneralTabControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "GAGC", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/ManageReconciliationsTabComponent", HtmlComponentName = "ManageReconciliationsTabComponent", FeatureId = GLAccountMANAGERECONCILIATIONSFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.RECO.GLAccountManageReconciliationsTabControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.ManageReconciliations" && d.Tenant == 0).FirstOrDefault().Id, Code = "GAMR", Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = GLAccountAdditionalSFeature.Id, ControlPath = "", HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountAdditionalDataTabComponent", HtmlComponentName = "GLAccountAdditionalDataTabComponent", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.Additional" && d.Tenant == 0).FirstOrDefault().Id, Code = "GAAD", Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = GLAccountTaxWithholdingFeature.Id, ControlPath = "", HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountTaxWithholdingTabComponent", HtmlComponentName = "GLAccountTaxWithholdingTabComponent", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.Tax" && d.Tenant == 0).FirstOrDefault().Id, Code = "GLTX", Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);

            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = GLAccountEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl",  ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "GLAccount.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "GAEV", Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
         
            
            #endregion
            
            
            #region AccountingPeriod
            //Feature AccountingPeriodNEWFeature = tenantFeatures.Where(d => d.Code == "NEW" && d.ObjectTableId == ChartOfAccountTable.Id).FirstOrDefault();
            Feature AccountingPeriodGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == AccountingPeriodTable.Id).FirstOrDefault();
            Feature AccountingPeriodEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == AccountingPeriodTable.Id).FirstOrDefault();

            //tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = AccountingPeriodNEWFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.NewAccountingPeriodControl", ObjectTableId = AccountingPeriodTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AccountingPeriod.TH.New" && d.Tenant == 0).FirstOrDefault().Id, Code = "APRN", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = AccountingPeriodGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.AccountingPeriodGeneralTabControl", ObjectTableId = AccountingPeriodTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AccountingPeriod.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "APGC", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = AccountingPeriodEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AccountingPeriodTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AccountingPeriod.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "APEV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion
            
          
            #region YearTransfer
            //Feature YearTransferGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();
          //  Feature YearTransferEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == GLAccountTable.Id).FirstOrDefault();

            //tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = YearTransferGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.YearTransferControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "YearTransfer.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "YTRN", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
         //   AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = YearTransferEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = GLAccountTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "YearTransfer.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "APEV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion
            
         
            #region AutomaticReconcileMethod
            Feature automaticReconcileMethodGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == AutomaticReconcileMethodTable.Id).FirstOrDefault();
            Feature automaticReconcileMethodEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == AutomaticReconcileMethodTable.Id).FirstOrDefault();

            //tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = automaticReconcileMethodGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.RECO.AutomaticReconcileMethodGeneralTabControl", ObjectTableId = AutomaticReconcileMethodTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AutomaticReconcileMethod.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "ARGC", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = automaticReconcileMethodEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AutomaticReconcileMethodTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "AutomaticReconcileMethod.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "AREV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region JournalActionType
            Feature journalActionTypeGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == JournalActionTypeTable.Id).FirstOrDefault();
            Feature journalActionTypeEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == JournalActionTypeTable.Id).FirstOrDefault();

            //tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = journalActionTypeGENERALFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.JRNL.JournalActionTypeGeneralTabControl", ObjectTableId = JournalActionTypeTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "JournalActionType.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "JAGC", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = journalActionTypeEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = JournalActionTypeTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "JournalActionType.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "JAEV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            #endregion

            #region CashBook
            Feature CashBookDetailsFeature = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == CashBookTable.Id).FirstOrDefault();
            Feature CashBookManageDepoFeature = tenantFeatures.Where(d => d.Code == "MNGDEPO" && d.ObjectTableId == CashBookTable.Id).FirstOrDefault();

            //tabs
            // general and event tabs are in lxml
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentName = "CashBookDetailsTabComponent", HtmlComponentUrl = "./Accounting/Components/EditTabs/CashBook/CashBookDetailsTabComponent", FeatureId = CashBookDetailsFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.CSHB.CashBookDetailsTabComponent", ObjectTableId = CashBookTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "CashBook.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Code = "CBDT", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentName = "CashBookManageDepoTabComponent", HtmlComponentUrl = "./Accounting/Components/EditTabs/CashBook/CashBookManageDepoTabComponent", FeatureId = CashBookManageDepoFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.CSHB.CashBookManageDepoTabComponent", ObjectTableId = CashBookTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "CashBook.TH.ManageDepo" && d.Tenant == 0).FirstOrDefault().Id, Code = "CBMD", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);






            #endregion

            #region BankDeposit
            //Feature BankDepositDetailsFeature = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == BankDepositTable.Id).FirstOrDefault();

            //tabs
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentName = "BankDepositDetailsTabComponent", HtmlComponentUrl = "./Accounting/Components/EditTabs/BankDeposit/BankDepositDetailsTabComponent", FeatureId = BankDepositDetailsFeature.Id, ControlPath = "Logitude.Accounting.Views.Tabs.BNKD.BankDepositDetailsTabComponent", ObjectTableId = BankDepositTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "BankDeposit.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Code = "BDDT", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);






            #endregion

            #region Revaluation
    
            Feature RevaluationEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == RevaluationTable.Id).FirstOrDefault();
            //Feature RevaluationDETAILSFeature = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == RevaluationTable.Id).FirstOrDefault();

            //tabs
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = RevaluationEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = RevaluationTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Revaluation.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "RVEV", Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
            //AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { HtmlComponentName = "RevaluationDetailsComponent", HtmlComponentUrl = "./Accounting/Components/EditTabs/Revaluation/RevaluationDetailsComponent", FeatureId = RevaluationDETAILSFeature.Id, ControlPath = "", ObjectTableId = RevaluationTable.Id, TabNameTextCodeId = objectContext.TextCodes.Where(d => d.Code == "Revaluation.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Code = "RVDT", Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);

            #endregion

            objectContext.SaveChanges();
        }

        public void LoadObjectTableHelperControls()
        {
            objectContext = WebFreightContext.GetContext(0);
            objectTableHelperControlsRepository = new ObjectTableHelperControlRepository(objectContext);
            Dictionary<string, ObjectTableHelperControl> TenantHelpers = objectTableHelperControlsRepository.GetObjectTableHelperControlsByTenant(0).ToDictionary(d => d.Code, a => a);

            
        }


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

            #region FULL ACCOUNTING
            Feature fullAccountingMenuFeature = tenantFeatures.Where(d => d.Code == "FULLACCOUNTING" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region JournalActionType features
            Feature journalActionTypeMenuFeature = tenantFeatures.Where(d => d.Code == "JOURNALACTIONTYPESMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region ChartOfAccounts features
            Feature chartOfAccountsMenuFeature = tenantFeatures.Where(d => d.Code == "CHARTOFACCOUNTSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region Journal features
            //Feature journalMenuFeature = tenantFeatures.Where(d => d.Code == "JOURNALMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion


            #region GLAccount features
            Feature gLAccountsMenuFeature = tenantFeatures.Where(d => d.Code == "GLACCOUNTSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature clientGLAccountsMenuFeature = tenantFeatures.Where(d => d.Code == "CLIENTGLACCOUNTSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature vendorGLAccountsMenuFeature = tenantFeatures.Where(d => d.Code == "VENDORGLACCOUNTSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region AccountingPeriod features
            Feature accountingPeriodsMenuFeature = tenantFeatures.Where(d => d.Code == "ACCOUNTINGPERIODSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region AutomaticReconcileMethod features
            Feature automaticReconcileMethodMenuFeature = tenantFeatures.Where(d => d.Code == "AUTORECOMETHODSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion


            #region BankCodes features
            Feature BankCodeMenuFeature = tenantFeatures.Where(d => d.Code == "BANKCODEMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region BankAccounts features
            Feature BankAccountMenuFeature = tenantFeatures.Where(d => d.Code == "BANKACCOUNTMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion


            #region BankDeposits features
            Feature BankDepositMenuFeature = tenantFeatures.Where(d => d.Code == "BANKDEPOSITMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion


            #region Revaluation features
            Feature RevaluationMenuFeature = tenantFeatures.Where(d => d.Code == "REVALUATIONMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region YearTransfer features
            Feature YearTransferMenuFeature = tenantFeatures.Where(d => d.Code == "YEARTRANSFERSMENU" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region closed tables
            #endregion

            #region Categories
            Feature category1MenuFeature = tenantFeatures.Where(d => d.Code == "Category1.Features.Category1" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature category2MenuFeature = tenantFeatures.Where(d => d.Code == "Category2.Features.Category2" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature category3MenuFeature = tenantFeatures.Where(d => d.Code == "Category3.Features.Category3" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature category4MenuFeature = tenantFeatures.Where(d => d.Code == "Category4.Features.Category4" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            Feature category5MenuFeature = tenantFeatures.Where(d => d.Code == "Category5.Features.Category5" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region CashBook Features
            Feature CashBookMenuFeature = tenantFeatures.Where(d => d.Code == "CashbookMenuFeature" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region TaxWithholdingAssessingOffices  Features
            Feature TaxWithholdingOfficesMenuFeature = tenantFeatures.Where(d => d.Code == "TaxOfficesMenuFeature" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region TaxReport  Features
            Feature TaxReportMenuFeature = tenantFeatures.Where(d => d.Code == "TaxReport.Features.Menu" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            #region TaxDeductionReport  Features
            Feature TaxDeductionReportMenuFeature = tenantFeatures.Where(d => d.Code == "TaxDeductionReport.Features.Menu" && d.FeatureTypeCode == "MENU").FirstOrDefault();
            #endregion

            Feature AccountingIntegrityCheckMenuFeature = tenantFeatures.Where(d => d.Code == "AccountingIntegrityCheck.Features.Menu" && d.FeatureTypeCode == "MENU").FirstOrDefault();


            #endregion

            #region Menus

            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MFAC", Tenant = 0, MenuTypeCode = "Main", IndexOfOrder = 10, CategoryTypeCode = null, TextCode = "General.MH.FullAccounting", Icon = "AccountingPath", FeatureId = fullAccountingMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            //   AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "CSDC", Tenant = 0, MenuTypeCode = "Main", IndexOfOrder = 10, CategoryTypeCode = null, TextCode = "General.MH.Declarations", Icon = "CustomersPath", FeatureId = customFeature.Id, ObjectTableId = tenantObjectTables.Where(o => o.Name == "Customs.Declaration").FirstOrDefault().Id }, MenusTablesRepository, tenantMenusTables);
            //  AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "CSRS", Tenant = 0, MenuTypeCode = "Main", IndexOfOrder = 11, CategoryTypeCode = null, TextCode = "General.MH.CustomsRequestsSheets", Icon = "ReportsPath", FeatureId = customFeature.Id, }, MenusTablesRepository, tenantMenusTables);


            #endregion

            #region Maintenance



            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTJA", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 0, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.JournalActionTypes", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "JournalActionType").FirstOrDefault().Id, FeatureId = journalActionTypeMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTCA", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 1, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.ChartOfAccounts", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "ChartOfAccount").FirstOrDefault().Id, FeatureId = chartOfAccountsMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTGA", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 3, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.GLAccounts", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "GLAccount").FirstOrDefault().Id, FeatureId = gLAccountsMenuFeature.Id }, menusTablesRepository, tenantMenusTables);


            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTJN", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 2, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Journal", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Journal").FirstOrDefault().Id, FeatureId = journalMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "ACAR", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 6, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.AutomaticReconcileMethods", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "AutomaticReconcileMethod").FirstOrDefault().Id, FeatureId = automaticReconcileMethodMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTGC", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 7, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Clients", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "GLAccount").FirstOrDefault().Id, FeatureId = clientGLAccountsMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTGV", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 8, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Vendors", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "GLAccount").FirstOrDefault().Id, FeatureId = vendorGLAccountsMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            // Categories
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTC1", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 8, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Category1", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Category1").FirstOrDefault().Id, FeatureId = category1MenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTC2", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 9, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Category2", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Category2").FirstOrDefault().Id, FeatureId = category2MenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTC3", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 10, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Category3", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Category3").FirstOrDefault().Id, FeatureId = category3MenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTC4", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 11, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Category4", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Category4").FirstOrDefault().Id, FeatureId = category4MenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTC5", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 12, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Category5", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Category5").FirstOrDefault().Id, FeatureId = category5MenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTBC", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 9, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.BankCodes", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "BankCode").FirstOrDefault().Id, FeatureId = BankCodeMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTBA", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 10, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.BankAccounts", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "BankAccount").FirstOrDefault().Id, FeatureId = BankAccountMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTBD", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 11, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.BankDeposits", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "BankDeposit").FirstOrDefault().Id, FeatureId = BankDepositMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
           // AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTRV", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 12, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.Revaluations", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Revaluation").FirstOrDefault().Id, FeatureId = RevaluationMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MCSH", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 13, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.CashBooks", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "CashBook").FirstOrDefault().Id, FeatureId = CashBookMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTTX", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 14, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.TaxWithholding", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TaxWithholdingAssessOffice").FirstOrDefault().Id, FeatureId = TaxWithholdingOfficesMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "TXRP", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 15, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.TaxReport", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TaxReport").FirstOrDefault().Id, FeatureId = TaxReportMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            //AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "TXDR", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 16, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.TaxDeductionReport", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "TaxDeductionReport").FirstOrDefault().Id, FeatureId = TaxDeductionReportMenuFeature.Id }, menusTablesRepository, tenantMenusTables);
            AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "AICH", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 17, CategoryTypeCode = "ACC", TextCode = "General.MC.ACC.IntegrityChecks", Icon = "Money_64.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "AccountingIntegrityCheck").FirstOrDefault().Id, FeatureId = AccountingIntegrityCheckMenuFeature.Id }, menusTablesRepository, tenantMenusTables);

            #endregion




            menusTablesRepository.SubmitChanges();
        }

        public void LoadEventTypes()
        {
            objectContext = WebFreightContext.GetContext(0);
            eventTypesRepository = new EventTypeRepository(objectContext);
            Dictionary<string, EventType> tenantEventTypes = eventTypesRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);

            #region GLAccount events
            ObjectTablePM gLAccountObject = ObjectTableQuery.GetObjectTableByCode("GLAccount", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                LocalName = "חדש",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACH",
                EnglishName = "Account Number Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שינוי מספר חשבון",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "NCH",
                EnglishName = "Account Name Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שינוי שם חשבון",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CHCH",
                EnglishName = "Chart of Accounts Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שינוי קבוצת מאזן",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BLK",
                EnglishName = "Account Deactivated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "החשבון נחסם",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UBLK",
                EnglishName = "Account Activated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "החשבון הופעל",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ADD",
                EnglishName = "Splitted GLAccount added",
                Tenant = 0,
                AddedManually = false,
                LocalName = "נוסף כרטיס פיצול לפני מטבע",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INGL",
                EnglishName = "Splitted GLAccount deactivated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "כרטיס הפיצול נחסם",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DISC",
                EnglishName = "Child GLAccount was disconnected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "כרטיס בן נותק",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CHID",
                EnglishName = "Child GLAccount was added",
                Tenant = 0,
                AddedManually = false,
                LocalName = "לכרטיס נוסף כרטיס בן",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SGAC",
                EnglishName = "Splitted GLAccount was activated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "כרטיס הפיצול מוּפעָל",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "LIAC",
                EnglishName = "Line activated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שורה הופעלה",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DETV",
                EnglishName = "Line deactivated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שורה מספר נחסמה",
                ObjectTableId = gLAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            #region ChartOfAccount events
            ObjectTablePM chartOfAccountObject = ObjectTableQuery.GetObjectTableByCode("ChartOfAccount", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUPD",
                EnglishName = "Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שינוי",
                ObjectTableId = chartOfAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CCR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                LocalName = "חדש",
                ObjectTableId = chartOfAccountObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            #endregion
         
            #region Journal events
            ObjectTablePM JournalObject = ObjectTableQuery.GetObjectTableByCode("Journal", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JCR",
                EnglishName = "Journal Created",
                Tenant = 0,
                AddedManually = false,
                LocalName = "פתיחת פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JUP",
                EnglishName = "Journal Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "עדכון פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JCL",
                EnglishName = "Journal Closed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "סגור פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JAP",
                EnglishName = "Journal Approved",
                Tenant = 0,
                AddedManually = false,
                LocalName = "אישור פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JVD",
                EnglishName = "Journal Voided",
                Tenant = 0,
                AddedManually = false,
                LocalName = "בטלה פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "JSV",
                EnglishName = "Journal Waiting For Approval",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שמירת פקודת יומן",
                ObjectTableId = JournalObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            #endregion

            #region AccountingPeriod events
            ObjectTablePM accountingPeriodObject = ObjectTableQuery.GetObjectTableByCode("AccountingPeriod", 0);
           
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PUPD",
                EnglishName = "Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "שינוי",
                ObjectTableId = accountingPeriodObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PCR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                LocalName = "חדש",
                ObjectTableId = accountingPeriodObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
             
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PDL",
                EnglishName = "Deleted",
                Tenant = 0,
                AddedManually = false,
                LocalName = "נמחק",
                ObjectTableId = accountingPeriodObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);
            #endregion

            #region GLAccountWithholdingTax events
            ObjectTablePM gLAccountWithholdingTax = ObjectTableQuery.GetObjectTableByCode("GLAccountWithholdingTax", 0);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "WBLK",
                EnglishName = "The Vendor does not have a certificate according to the 1000 System",
                Tenant = 0,
                AddedManually = false,
                LocalName = "לספק לא קיים אישור על פי מערכת 1000",
                ObjectTableId = gLAccountWithholdingTax.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "WLDA",
                EnglishName = "A new line was entered with the same date by the 1000 System",
                Tenant = 0,
                AddedManually = false,
                LocalName = "נקלטה שורה חדשה עם תאריך זהה על ידי מערכת 1000",
                ObjectTableId = gLAccountWithholdingTax.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, eventTypesRepository, tenantEventTypes);


            #endregion


            eventTypesRepository.SubmitChanges();
        }


        #region fillClosedtables
        #region  TaxWithholdingAssessingOffice
        public void FillTaxWithholdingAssessOffice()
        {

            TaxWithholdingAssessOfficeRepository taxWithholdingAssessOfficeRepository = new TaxWithholdingAssessOfficeRepository(0);
            Dictionary<string, TaxWithholdingAssessOffice> TenantTaxWithholdingAssessOffices = taxWithholdingAssessOfficeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);

            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() {Id=IdCounter.GetNumber("TaxWithholdingAssessOffice",0), Code = "01", Name = "Tiberias", LocalName = "טבריה", Tenant=0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "02", Name = "Afula", LocalName = "עפולה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "04", Name = "Zefat", LocalName = "צפת", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "05", Name = "Nazareth", LocalName = "נצרת", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "07", Name = "Acre", LocalName = "עכו", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "10", Name = "Haifa", LocalName = "חיפה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "17", Name = "Hedera", LocalName = "חדרה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "39", Name = "Gush Dan", LocalName = "גוש דן", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "21", Name = "Netania", LocalName = "נתניה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "23", Name = "Kfar Saba", LocalName = "כפר סבא", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "24", Name = "Pettah Tekva", LocalName = "פתח תקווה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "25", Name = "Ramla", LocalName = "רמלה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "26", Name = "Rehovaot", LocalName = "רחובות", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "31", Name = "Tel Aviv 1", LocalName = "תל אביב 1", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "38", Name = "Tel Aviv 3", LocalName = "תל אביב 3", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "34", Name = "Tel Aviv 4", LocalName = "תל אביב 4", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "30", Name = "Tel Aviv 5", LocalName = "תל אביב 5", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "32", Name = "Holon", LocalName = "חולון", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "37", Name = "Big factories", LocalName = "מפעלים גדולים", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "51", Name = "Ashkleon", LocalName = "אשקלון", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "52", Name = "Ber Sheva", LocalName = "באר שבע", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "50", Name = "Eilat", LocalName = "אילת", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "48", Name = "autonomy", LocalName = "אוטונומיה", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "41", Name = "Jerusalem 1", LocalName = "ירושלים 1", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "45", Name = "Jerusalem 2", LocalName = "ירושלים 2", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
            AddClosedTables.AddTaxWithholdingAssessingOfficeMode(new TaxWithholdingAssessOfficeDetails() { Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", 0), Code = "43", Name = "Jerusalem 3+", LocalName = "ירושלים 3", Tenant = 0 }, taxWithholdingAssessOfficeRepository);
        

            taxWithholdingAssessOfficeRepository.SubmitChanges();

        }

        #endregion


        #region AccountingCompanyType


        public void FillAccountingCompanyType()
        {

            AccountingCompanyTypeRepository accountingCompanyTypeRepository = new AccountingCompanyTypeRepository(0);
            Dictionary<string, AccountingCompanyType> TenantAccountingCompanyTypes = accountingCompanyTypeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);

            AddClosedTables.AddAccountingCompanyType(new AccountingCompanyTypeDetails() { Id = IdCounter.GetNumber("AccountingCompanyType", 0), Code = "1", EnglishName = "Self-employed", LocalName = "עצמאי", Tenant = 0 }, accountingCompanyTypeRepository);
            AddClosedTables.AddAccountingCompanyType(new AccountingCompanyTypeDetails() { Id = IdCounter.GetNumber("AccountingCompanyType", 0), Code = "2", EnglishName = "Company", LocalName = "חברה", Tenant = 0 }, accountingCompanyTypeRepository);
            AddClosedTables.AddAccountingCompanyType(new AccountingCompanyTypeDetails() { Id = IdCounter.GetNumber("AccountingCompanyType", 0), Code = "3", EnglishName = "Authorized Dealer", LocalName = "עוסק מורשה", Tenant = 0 }, accountingCompanyTypeRepository);


           accountingCompanyTypeRepository.SubmitChanges();

        }

        #endregion


        #region WithholdingTaxDeductionType


        public void FillWithholdingTaxDeductionTypes()
        {

            WithholdingTaxDeductionTypeRepository withholdingTaxDeductionTypeRepository = new WithholdingTaxDeductionTypeRepository(0);
            Dictionary<string, WithholdingTaxDeductionType> TenantWithholdingTaxDeductionTypes = withholdingTaxDeductionTypeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);

            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "01", EnglishName = "Interest", LocalName = "ריבית", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "02", EnglishName = "Insurance Commision", LocalName = "עמלת ביטוח", Tenant = 0 }, withholdingTaxDeductionTypeRepository);

            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "03", EnglishName = "Wage", LocalName = "שכר", Tenant = 0 }, withholdingTaxDeductionTypeRepository);

            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "05", EnglishName = "Services", LocalName = "שירותים", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "06", EnglishName = "Construction Payment", LocalName = "תשלומי בניה", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "07", EnglishName = "Payment for foreigner(Deduction by the businesss)", LocalName = "תשלום לתושב זר(נוכה ע\"י העסק)", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "08", EnglishName = "Payment for foreigner(Deduction by the bank)", LocalName = "תשלום לתושב זר(נוכה ע\"י הבנק)", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "11", EnglishName = "Illegal fund payment", LocalName = "תשלום שלא כדין מקופת גמל", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "12", EnglishName = "Refud", LocalName = "החזר תשלום למעביד מקופת גמל לפיצויים", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "13", EnglishName = "", LocalName = "תשלומים בעד שכיורת מקרקעין שניתן לתבוע כהוצאה", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "14", EnglishName = "", LocalName = "תשלום מקרן השתלמות לעצמאי", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "15", EnglishName = "Payout Payment or profit derived from gambling", LocalName = "תשלומים מהשתכרות או רווח שמקורם בהימורים", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "18", EnglishName = "Dividend Payment", LocalName = "תשלום דיבידנד", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "19", EnglishName = "", LocalName = "רווח הון מפדיון מניות/אופציות", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "20", EnglishName = "", LocalName = "סעיף מיוחד לביטוח לאומי", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "21", EnglishName = "", LocalName = "הכנסה מהפקת חשמל במסלול פטור", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
            AddClosedTables.AddWithholdingTaxDeductionType(new WithholdingTaxDeductionTypeDetails() { Id = IdCounter.GetNumber("WithholdingTaxDeductionType", 0), Code = "22", EnglishName = "", LocalName = "הכנסה מהפקת חשמל במסלול מס מופחת", Tenant = 0 }, withholdingTaxDeductionTypeRepository);
        



            withholdingTaxDeductionTypeRepository.SubmitChanges();

        }

        #endregion

        #endregion


     

        public void CreateCounters(int tenant)
        {
            //objectContext = WebFreightContext.GetContext(tenant);
            //CounterRepository CounterRepository = new CounterRepository(objectContext);
            //CounterDefinitionRepository = new CounterDefinitionRepository(objectContext);
            //List<Counter> zeroCounters = CounterRepository.GetCounters(0).ToList();
            //ObjectTable taxDeductionReportObject = objectContext.ObjectTables.Where(d => d.Name == "TaxDeductionReport" && d.Tenant == 0).FirstOrDefault();
            //#region Tax Deduction Report Counters

            //if (!zeroCounters.Where(c => c.Code == "TXDC" && c.Tenant == 0).Any())
            //{
            //    Counter taxDeductionReportCounter = new Counter()
            //    {
            //        Id = IdCounter.GetNumber("Counter", 0).ToString(),
            //        ObjectTableId = taxDeductionReportObject.Id,
            //        Code = "TXDC",
            //        Tenant = 0,
            //        Name = "Tax Deduction Report",
            //    };

            //    CounterDefinition taxDeductionReportCounter_CounterDef = new CounterDefinition()
            //    {
            //        Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
            //        CounterId = taxDeductionReportCounter.Id,
            //        Tenant = 0,
            //        StartNumber = 1000,
            //        Parameter1 = "TX",
            //    };

            //    CounterRepository.Add(taxDeductionReportCounter);
            //    CounterDefinitionRepository.Add(taxDeductionReportCounter_CounterDef);

            //}
            //#endregion

            //this.objectContext.SaveChanges();
        }
    }
}
 