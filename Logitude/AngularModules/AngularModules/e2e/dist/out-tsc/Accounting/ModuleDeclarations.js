"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GLAccountsPageComponent_1 = require("./Components/Workspaces/GLAccounts/GLAccountsPageComponent");
var AccountingNoteComponent_1 = require("./Components/Others/AccountingNoteComponent");
var IntegrityCheckTabComponent_1 = require("./Components/EditTabs/AccountingIntegrityCheck/IntegrityCheckTabComponent");
var AccountingWorkspaceComponent_1 = require("./Components/Workspaces/AccountingWorkspaceComponent");
var MainPageComponent_1 = require("./Components/Workspaces/Main/MainPageComponent");
var JournalPageComponent_1 = require("./Components/Workspaces/Journal/JournalPageComponent");
var ReceivablePageComponent_1 = require("./Components/Workspaces/Receivable/ReceivablePageComponent");
var PayablePageComponent_1 = require("./Components/Workspaces/Payable/PayablePageComponent");
var BanksPageComponent_1 = require("./Components/Workspaces/Banks/BanksPageComponent");
var MiscPageComponent_1 = require("./Components/Workspaces/Misc/MiscPageComponent");
var NewGLAccountComponent_1 = require("./Components/NewEntity/NewGLAccountComponent");
var NewChartOfAccountComponent_1 = require("./Components/NewEntity/NewChartOfAccountComponent");
var NewCashBookComponent_1 = require("./Components/NewEntity/NewCashBookComponent");
var NewBankCodeComponent_1 = require("./Components/NewEntity/NewBankCodeComponent");
var NewBankAccountComponent_1 = require("./Components/NewEntity/NewBankAccountComponent");
var NewBankDepositComponent_1 = require("./Components/NewEntity/NewBankDepositComponent");
var NewCategory1Component_1 = require("./Components/Maintenance/NewCategory1Component");
var NewCategory2Component_1 = require("./Components/Maintenance/NewCategory2Component");
var NewCategory3Component_1 = require("./Components/Maintenance/NewCategory3Component");
var NewCategory4Component_1 = require("./Components/Maintenance/NewCategory4Component");
var NewCategory5Component_1 = require("./Components/Maintenance/NewCategory5Component");
var AutoRecoMethodComponent_1 = require("./Components/Maintenance/AutoRecoMethodComponent");
var NewRevaluationComponent_1 = require("./Components/NewEntity/NewRevaluationComponent");
var AddEditRecoExPageComponent_1 = require("./Components/NewEntity/AddEditRecoExPageComponent");
var NewConnectedGLAccountComponent_1 = require("./Components/EditTabs/GLAccount/NewConnectedGLAccountComponent");
var NewPaymentChequeComponent_1 = require("./Components/NewEntity/NewPaymentChequeComponent");
var NewTaxWithholdingAssessingOfficeComponent_1 = require("./Components/Maintenance/NewTaxWithholdingAssessingOfficeComponent");
var FullAccountingSettingsComponent_1 = require("./Components/Maintenance/FullAccountingSettingsComponent");
var AccountingPeriodsComponent_1 = require("./Components/Maintenance/AccountingPeriodsComponent");
var EditAccountingPeriodComponent_1 = require("./Components/Maintenance/EditAccountingPeriodComponent");
var AccountingPeriodEventComponent_1 = require("./Components/Maintenance/AccountingPeriodEventComponent");
var YearTransferComponent_1 = require("./Components/Maintenance/YearTransferComponent");
var NewTaxReportComponent_1 = require("./Components/NewEntity/NewTaxReportComponent");
var NewOpenFormatReportComponent_1 = require("./Components/NewEntity/NewOpenFormatReportComponent");
var AccountingLoadTestComponent_1 = require("./Components/Maintenance/AccountingLoadTestComponent");
var LoadRecoExPageComponent_1 = require("./Components/NewEntity/LoadRecoExPageComponent");
var Generate1000Component_1 = require("./Components/Maintenance/Generate1000Component");
var Receiving1000Component_1 = require("./Components/Maintenance/Receiving1000Component");
var AccountingFunctionalTestComponent_1 = require("./Components/Maintenance/AccountingFunctionalTestComponent");
var GLAccountGeneralTabComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountGeneralTabComponent");
var GLAccountTransactionsTabComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountTransactionsTabComponent");
var ManageReconciliationsTabComponent_1 = require("./Components/EditTabs/GLAccount/ManageReconciliationsTabComponent");
var JournalDetailsTabComponent_1 = require("./Components/EditTabs/Journal/JournalDetailsTabComponent");
var CashBookDetailsTabComponent_1 = require("./Components/EditTabs/CashBook/CashBookDetailsTabComponent");
var CashBookManageDepoTabComponent_1 = require("./Components/EditTabs/CashBook/CashBookManageDepoTabComponent");
var ChartOfAccountGeneralTabComponent_1 = require("./Components/EditTabs/ChartOfAccount/ChartOfAccountGeneralTabComponent");
var BankDepositDetailsTabComponent_1 = require("./Components/EditTabs/BankDeposit/BankDepositDetailsTabComponent");
var BankAccountGeneralTabComponent_1 = require("./Components/EditTabs/BankAccount/BankAccountGeneralTabComponent");
var BankPagesTabComponent_1 = require("./Components/EditTabs/BankAccount/BankPagesTabComponent");
var ManageRecoTabComponent_1 = require("./Components/EditTabs/BankAccount/ManageRecoTabComponent");
var ReconciliationDetailsTabComponent_1 = require("./Components/EditTabs/Reconciliation/ReconciliationDetailsTabComponent");
var RevaluationDetailsComponent_1 = require("./Components/EditTabs/Revaluation/RevaluationDetailsComponent");
var GLAccountTaxWithholdingTabComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountTaxWithholdingTabComponent");
var BankCodeGeneralTabComponent_1 = require("./Components/EditTabs/BankCode/BankCodeGeneralTabComponent");
var ExternalRecoDetailsTabComponent_1 = require("./Components/EditTabs/ExternalReconciliation/ExternalRecoDetailsTabComponent");
var OpenFormatReportLogTabComponent_1 = require("./Components/EditTabs/OpenFormatReport/OpenFormatReportLogTabComponent");
var TaxDeductionReportGeneralTabComponent_1 = require("./Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabComponent");
var BankPageEventsComponent_1 = require("./Components/EditTabs/BankAccount/BankPageEventsComponent");
var GlAccountLedgerTransactionsListTemplate_1 = require("./Components/ListTemplates/GlAccountLedgerTransactionsListTemplate");
var ReconcileExternalPageListTemplate_1 = require("./Components/ListTemplates/ReconcileExternalPageListTemplate");
var ReconcileExternalPageLineListTemplate_1 = require("./Components/ListTemplates/ReconcileExternalPageLineListTemplate");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var ManageReconciliationListTemplate_1 = require("./Components/ListTemplates/ManageReconciliationListTemplate");
var TaxReportListTemplate_1 = require("./Components/ListTemplates/TaxReportListTemplate");
var ReconcileComponent_1 = require("./Components/Others/ReconcileComponent");
var ReconciledMessage_1 = require("./Components/Others/ReconciledMessage");
var OutOfDepositMessage_1 = require("./Components/Others/OutOfDepositMessage");
var Aging4CustomerChartWindowComponent_1 = require("./Components/Others/Aging4CustomerChartWindowComponent");
var GLAccountAdditionalDataTabComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountAdditionalDataTabComponent");
var GLAccountSearchWindowComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountSearchWindowComponent");
var PaymentChequeGeneralTabComponent_1 = require("./Components/EditTabs/PaymentCheque/PaymentChequeGeneralTabComponent");
var CancelChequeComponent_1 = require("./Components/Others/CancelChequeComponent");
var DropdownButtonComponent_1 = require("./Components/Others/DropdownButtonComponent");
var JournalReconcileComponent_1 = require("./Components/Others/JournalReconcileComponent");
var ExternalReconcileComponent_1 = require("./Components/Others/ExternalReconcileComponent");
var AddEditTaxWithholdingLineComponent_1 = require("./Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent");
var TaxReportDetailsTabComponent_1 = require("./Components/EditTabs/TaxReport/TaxReportDetailsTabComponent");
var AccountingFlatFileDownloadComponent_1 = require("./Components/Others/AccountingFlatFileDownloadComponent");
var GLAccountOverviewComponent_1 = require("./Components/EditTabs/GLAccount/GLAccountOverviewComponent");
var TaxDeductionReportLogTabComponent_1 = require("./Components/EditTabs/TaxDeductionReport/TaxDeductionReportLogTabComponent");
var NewTaxDeductionReportComponent_1 = require("./Components/NewEntity/NewTaxDeductionReportComponent");
// Short Titles
var GLAccountShortTitleComponent_1 = require("./Components/ShortTitles/GLAccountShortTitleComponent");
var BankAccountShortTitleComponent_1 = require("./Components/ShortTitles/BankAccountShortTitleComponent");
var BankDepositShortTitleComponent_1 = require("./Components/ShortTitles/BankDepositShortTitleComponent");
var PaymentChequeShortTitleComponent_1 = require("./Components/ShortTiTles/PaymentChequeShortTitleComponent");
var ReconciliationShortTitleComponent_1 = require("./Components/ShortTiTles/ReconciliationShortTitleComponent");
var ExternalReconciliationShortTitleComponent_1 = require("./Components/ShortTiTles/ExternalReconciliationShortTitleComponent");
var TaxReportShortTitleComponent_1 = require("./Components/ShortTiTles/TaxReportShortTitleComponent");
var EditTaxReportLineComponent_1 = require("./Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent");
var NewIntegrityCheckComponent_1 = require("./Components/NewEntity/NewIntegrityCheckComponent");
exports.Components = [
    //Workspaces
    AccountingWorkspaceComponent_1.AccountingWorkspaceComponent,
    MainPageComponent_1.MainPageComponent,
    GLAccountsPageComponent_1.GLAccountsPageComponent,
    JournalPageComponent_1.JournalPageComponent,
    ReceivablePageComponent_1.ReceivablePageComponent,
    PayablePageComponent_1.PayablePageComponent,
    BanksPageComponent_1.BanksPageComponent,
    MiscPageComponent_1.MiscPageComponent,
    //New Entites
    NewGLAccountComponent_1.NewGLAccountComponent,
    NewChartOfAccountComponent_1.NewChartOfAccountComponent,
    NewCashBookComponent_1.NewCashBookComponent,
    NewBankCodeComponent_1.NewBankCodeComponent,
    NewBankAccountComponent_1.NewBankAccountComponent,
    NewBankDepositComponent_1.NewBankDepositComponent,
    AutoRecoMethodComponent_1.AutoRecoMethodComponent,
    NewRevaluationComponent_1.NewRevaluationComponent,
    AddEditRecoExPageComponent_1.AddEditRecoExPageComponent,
    NewConnectedGLAccountComponent_1.NewConnectedGLAccountComponent,
    NewPaymentChequeComponent_1.NewPaymentChequeComponent,
    NewOpenFormatReportComponent_1.NewOpenFormatReportComponent,
    LoadRecoExPageComponent_1.LoadRecoExPageComponent,
    //Maintenance
    NewCategory1Component_1.NewCategory1Component,
    NewCategory2Component_1.NewCategory2Component,
    NewCategory3Component_1.NewCategory3Component,
    NewCategory4Component_1.NewCategory4Component,
    NewCategory5Component_1.NewCategory5Component,
    FullAccountingSettingsComponent_1.FullAccountingSettingsComponent,
    AccountingPeriodsComponent_1.AccountingPeriodsComponent,
    EditAccountingPeriodComponent_1.EditAccountingPeriodComponent,
    AccountingPeriodEventComponent_1.AccountingPeriodEventComponent,
    NewTaxWithholdingAssessingOfficeComponent_1.NewTaxWithholdingAssessingOfficeComponent,
    YearTransferComponent_1.YearTransferComponent,
    AccountingLoadTestComponent_1.AccountingLoadTestComponent,
    Generate1000Component_1.Generate1000Component,
    Receiving1000Component_1.Receiving1000Component,
    AccountingFunctionalTestComponent_1.AccountingFunctionalTestComponent,
    //Edit Tabs
    GLAccountGeneralTabComponent_1.GLAccountGeneralTabComponent,
    GLAccountOverviewComponent_1.GLAccountOverviewComponent,
    GLAccountTransactionsTabComponent_1.GLAccountTransactionsTabComponent,
    ManageReconciliationsTabComponent_1.ManageReconciliationsTabComponent,
    JournalDetailsTabComponent_1.JournalDetailsTabComponent,
    CashBookDetailsTabComponent_1.CashBookDetailsTabComponent,
    CashBookManageDepoTabComponent_1.CashBookManageDepoTabComponent,
    ChartOfAccountGeneralTabComponent_1.ChartOfAccountGeneralTabComponent,
    BankDepositDetailsTabComponent_1.BankDepositDetailsTabComponent,
    BankAccountGeneralTabComponent_1.BankAccountGeneralTabComponent,
    BankPagesTabComponent_1.BankPagesTabComponent,
    ManageRecoTabComponent_1.ManageRecoTabComponent,
    ReconciliationDetailsTabComponent_1.ReconciliationDetailsTabComponent,
    RevaluationDetailsComponent_1.RevaluationDetailsComponent,
    ExternalRecoDetailsTabComponent_1.ExternalRecoDetailsTabComponent,
    GLAccountAdditionalDataTabComponent_1.GLAccountAdditionalDataTabComponent,
    PaymentChequeGeneralTabComponent_1.PaymentChequeGeneralTabComponent,
    TaxReportDetailsTabComponent_1.TaxReportDetailsTabComponent,
    GLAccountTaxWithholdingTabComponent_1.GLAccountTaxWithholdingTabComponent,
    BankCodeGeneralTabComponent_1.BankCodeGeneralTabComponent,
    EditTaxReportLineComponent_1.EditTaxReportLineComponent,
    TaxDeductionReportLogTabComponent_1.TaxDeductionReportLogTabComponent,
    OpenFormatReportLogTabComponent_1.OpenFormatReportLogTabComponent,
    TaxDeductionReportGeneralTabComponent_1.TaxDeductionReportGeneralTabComponent,
    BankPageEventsComponent_1.BankPageEventsComponent,
    //Templates
    GlAccountLedgerTransactionsListTemplate_1.GlAccountLedgerTransactionsListTemplate,
    ReconcileExternalPageListTemplate_1.ReconcileExternalPageListTemplate,
    ReconcileExternalPageLineListTemplate_1.ReconcileExternalPageLineListTemplate,
    FieldTemplateComponent_1.FieldTemplateComponent,
    ManageReconciliationListTemplate_1.ManageReconciliationListTemplate,
    TaxReportListTemplate_1.TaxReportListTemplate,
    //Others
    ReconcileComponent_1.ReconcileComponent,
    ReconciledMessage_1.ReconciledMessage,
    OutOfDepositMessage_1.OutOfDepositMessage,
    Aging4CustomerChartWindowComponent_1.Aging4CustomerChartWindowComponent,
    PaymentChequeShortTitleComponent_1.PaymentChequeShortTitleComponent,
    ReconciliationShortTitleComponent_1.ReconciliationShortTitleComponent,
    ExternalReconciliationShortTitleComponent_1.ExternalReconciliationShortTitleComponent,
    TaxReportShortTitleComponent_1.TaxReportShortTitleComponent,
    GLAccountShortTitleComponent_1.GLAccountShortTitleComponent,
    BankAccountShortTitleComponent_1.BankAccountShortTitleComponent,
    BankDepositShortTitleComponent_1.BankDepositShortTitleComponent,
    GLAccountSearchWindowComponent_1.GLAccountSearchWindowComponent,
    CancelChequeComponent_1.CancelChequeComponent,
    DropdownButtonComponent_1.DropdownButtonComponent,
    JournalReconcileComponent_1.JournalReconcileComponent,
    ExternalReconcileComponent_1.ExternalReconcileComponent,
    AddEditTaxWithholdingLineComponent_1.AddEditTaxWithholdingLineComponent,
    NewTaxReportComponent_1.NewTaxReportComponent,
    AccountingFlatFileDownloadComponent_1.AccountingFlatFileDownloadComponent,
    NewTaxDeductionReportComponent_1.NewTaxDeductionReportComponent,
    IntegrityCheckTabComponent_1.IntegrityCheckTabComponent,
    AccountingNoteComponent_1.AccountingNoteComponent,
    NewIntegrityCheckComponent_1.NewIntegrityCheckComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            //Workspaces
            case "AccountingWorkspaceComponent": {
                myResult = AccountingWorkspaceComponent_1.AccountingWorkspaceComponent;
                break;
            }
            case "MainPageComponent": {
                myResult = MainPageComponent_1.MainPageComponent;
                break;
            }
            case "GLAccountsPageComponent": {
                myResult = GLAccountsPageComponent_1.GLAccountsPageComponent;
                break;
            }
            case "JournalPageComponent": {
                myResult = JournalPageComponent_1.JournalPageComponent;
                break;
            }
            case "ReceivablePageComponent": {
                myResult = ReceivablePageComponent_1.ReceivablePageComponent;
                break;
            }
            case "PayablePageComponent": {
                myResult = PayablePageComponent_1.PayablePageComponent;
                break;
            }
            case "BanksPageComponent": {
                myResult = BanksPageComponent_1.BanksPageComponent;
                break;
            }
            case "MiscPageComponent": {
                myResult = MiscPageComponent_1.MiscPageComponent;
                break;
            }
            //New Entites
            case "NewGLAccountComponent": {
                myResult = NewGLAccountComponent_1.NewGLAccountComponent;
                break;
            }
            case "NewChartOfAccountComponent": {
                myResult = NewChartOfAccountComponent_1.NewChartOfAccountComponent;
                break;
            }
            case "NewCashBookComponent": {
                myResult = NewCashBookComponent_1.NewCashBookComponent;
                break;
            }
            case "NewBankCodeComponent": {
                myResult = NewBankCodeComponent_1.NewBankCodeComponent;
                break;
            }
            case "NewBankAccountComponent": {
                myResult = NewBankAccountComponent_1.NewBankAccountComponent;
                break;
            }
            case "NewBankDepositComponent": {
                myResult = NewBankDepositComponent_1.NewBankDepositComponent;
                break;
            }
            case "AutoRecoMethodComponent": {
                myResult = AutoRecoMethodComponent_1.AutoRecoMethodComponent;
                break;
            }
            case "NewRevaluationComponent": {
                myResult = NewRevaluationComponent_1.NewRevaluationComponent;
                break;
            }
            case "AddEditRecoExPageComponent": {
                myResult = AddEditRecoExPageComponent_1.AddEditRecoExPageComponent;
                break;
            }
            case "NewConnectedGLAccountComponent": {
                myResult = NewConnectedGLAccountComponent_1.NewConnectedGLAccountComponent;
                break;
            }
            case "NewPaymentChequeComponent": {
                myResult = NewPaymentChequeComponent_1.NewPaymentChequeComponent;
                break;
            }
            case "NewTaxReportComponent": {
                myResult = NewTaxReportComponent_1.NewTaxReportComponent;
                break;
            }
            case "NewTaxDeductionReportComponent": {
                myResult = NewTaxDeductionReportComponent_1.NewTaxDeductionReportComponent;
                break;
            }
            case "NewOpenFormatReportComponent": {
                myResult = NewOpenFormatReportComponent_1.NewOpenFormatReportComponent;
                break;
            }
            case "LoadRecoExPageComponent": {
                myResult = LoadRecoExPageComponent_1.LoadRecoExPageComponent;
                break;
            }
            //Maintenance
            case "NewCategory1Component": {
                myResult = NewCategory1Component_1.NewCategory1Component;
                break;
            }
            case "NewCategory2Component": {
                myResult = NewCategory2Component_1.NewCategory2Component;
                break;
            }
            case "NewCategory3Component": {
                myResult = NewCategory3Component_1.NewCategory3Component;
                break;
            }
            case "NewCategory4Component": {
                myResult = NewCategory4Component_1.NewCategory4Component;
                break;
            }
            case "NewCategory5Component": {
                myResult = NewCategory5Component_1.NewCategory5Component;
                break;
            }
            case "FullAccountingSettingsComponent": {
                myResult = FullAccountingSettingsComponent_1.FullAccountingSettingsComponent;
                break;
            }
            case "YearTransferComponent": {
                myResult = YearTransferComponent_1.YearTransferComponent;
                break;
            }
            case "AccountingPeriodsComponent": {
                myResult = AccountingPeriodsComponent_1.AccountingPeriodsComponent;
                break;
            }
            case "EditAccountingPeriodComponent": {
                myResult = EditAccountingPeriodComponent_1.EditAccountingPeriodComponent;
                break;
            }
            case "AccountingPeriodEventComponent": {
                myResult = AccountingPeriodEventComponent_1.AccountingPeriodEventComponent;
                break;
            }
            case "NewTaxWithholdingAssessingOfficeComponent": {
                myResult = NewTaxWithholdingAssessingOfficeComponent_1.NewTaxWithholdingAssessingOfficeComponent;
                break;
            }
            case "AccountingLoadTestComponent": {
                myResult = AccountingLoadTestComponent_1.AccountingLoadTestComponent;
                break;
            }
            case "Generate1000Component": {
                myResult = Generate1000Component_1.Generate1000Component;
                break;
            }
            case "Receiving1000Component": {
                myResult = Receiving1000Component_1.Receiving1000Component;
                break;
            }
            case "AccountingFunctionalTestComponent": {
                myResult = AccountingFunctionalTestComponent_1.AccountingFunctionalTestComponent;
                break;
            }
            //Edit Tabs
            case "GLAccountGeneralTabComponent": {
                myResult = GLAccountGeneralTabComponent_1.GLAccountGeneralTabComponent;
                break;
            }
            case "GLAccountOverviewComponent": {
                myResult = GLAccountOverviewComponent_1.GLAccountOverviewComponent;
                break;
            }
            case "GLAccountTransactionsTabComponent": {
                myResult = GLAccountTransactionsTabComponent_1.GLAccountTransactionsTabComponent;
                break;
            }
            case "ManageReconciliationsTabComponent": {
                myResult = ManageReconciliationsTabComponent_1.ManageReconciliationsTabComponent;
                break;
            }
            case "JournalDetailsTabComponent": {
                myResult = JournalDetailsTabComponent_1.JournalDetailsTabComponent;
                break;
            }
            case "CashBookDetailsTabComponent": {
                myResult = CashBookDetailsTabComponent_1.CashBookDetailsTabComponent;
                break;
            }
            case "CashBookManageDepoTabComponent": {
                myResult = CashBookManageDepoTabComponent_1.CashBookManageDepoTabComponent;
                break;
            }
            case "ChartOfAccountGeneralTabComponent": {
                myResult = ChartOfAccountGeneralTabComponent_1.ChartOfAccountGeneralTabComponent;
                break;
            }
            case "BankDepositDetailsTabComponent": {
                myResult = BankDepositDetailsTabComponent_1.BankDepositDetailsTabComponent;
                break;
            }
            case "BankAccountGeneralTabComponent": {
                myResult = BankAccountGeneralTabComponent_1.BankAccountGeneralTabComponent;
                break;
            }
            case "BankPagesTabComponent": {
                myResult = BankPagesTabComponent_1.BankPagesTabComponent;
                break;
            }
            case "ManageRecoTabComponent": {
                myResult = ManageRecoTabComponent_1.ManageRecoTabComponent;
                break;
            }
            case "ReconciliationDetailsTabComponent": {
                myResult = ReconciliationDetailsTabComponent_1.ReconciliationDetailsTabComponent;
                break;
            }
            case "RevaluationDetailsComponent": {
                myResult = RevaluationDetailsComponent_1.RevaluationDetailsComponent;
                break;
            }
            case "ExternalRecoDetailsTabComponent": {
                myResult = ExternalRecoDetailsTabComponent_1.ExternalRecoDetailsTabComponent;
                break;
            }
            case "GLAccountAdditionalDataTabComponent": {
                myResult = GLAccountAdditionalDataTabComponent_1.GLAccountAdditionalDataTabComponent;
                break;
            }
            case "PaymentChequeGeneralTabComponent": {
                myResult = PaymentChequeGeneralTabComponent_1.PaymentChequeGeneralTabComponent;
                break;
            }
            case "TaxReportDetailsTabComponent": {
                myResult = TaxReportDetailsTabComponent_1.TaxReportDetailsTabComponent;
                break;
            }
            case "GLAccountTaxWithholdingTabComponent": {
                myResult = GLAccountTaxWithholdingTabComponent_1.GLAccountTaxWithholdingTabComponent;
                break;
            }
            case "BankCodeGeneralTabComponent": {
                myResult = BankCodeGeneralTabComponent_1.BankCodeGeneralTabComponent;
                break;
            }
            case "EditTaxReportLineComponent": {
                myResult = EditTaxReportLineComponent_1.EditTaxReportLineComponent;
                break;
            }
            case "TaxDeductionReportLogTabComponent": {
                myResult = TaxDeductionReportLogTabComponent_1.TaxDeductionReportLogTabComponent;
                break;
            }
            case "OpenFormatReportLogTabComponent": {
                myResult = OpenFormatReportLogTabComponent_1.OpenFormatReportLogTabComponent;
                break;
            }
            case "TaxDeductionReportGeneralTabComponent": {
                myResult = TaxDeductionReportGeneralTabComponent_1.TaxDeductionReportGeneralTabComponent;
                break;
            }
            case "BankPageEventsComponent": {
                myResult = BankPageEventsComponent_1.BankPageEventsComponent;
                break;
            }
            //Templates
            case "GlAccountLedgerTransactionsListTemplate": {
                myResult = GlAccountLedgerTransactionsListTemplate_1.GlAccountLedgerTransactionsListTemplate;
                break;
            }
            case "ReconcileExternalPageListTemplate": {
                myResult = ReconcileExternalPageListTemplate_1.ReconcileExternalPageListTemplate;
                break;
            }
            case "ReconcileExternalPageLineListTemplate": {
                myResult = ReconcileExternalPageLineListTemplate_1.ReconcileExternalPageLineListTemplate;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "ManageReconciliationListTemplate": {
                myResult = ManageReconciliationListTemplate_1.ManageReconciliationListTemplate;
                break;
            }
            case "TaxReportListTemplate": {
                myResult = TaxReportListTemplate_1.TaxReportListTemplate;
                break;
            }
            //Others
            case "ReconcileComponent": {
                myResult = ReconcileComponent_1.ReconcileComponent;
                break;
            }
            case "ReconciledMessage": {
                myResult = ReconciledMessage_1.ReconciledMessage;
                break;
            }
            case "OutOfDepositMessage": {
                myResult = OutOfDepositMessage_1.OutOfDepositMessage;
                break;
            }
            case "Aging4CustomerChartWindowComponent": {
                myResult = Aging4CustomerChartWindowComponent_1.Aging4CustomerChartWindowComponent;
                break;
            }
            case "PaymentChequeShortTitleComponent": {
                myResult = PaymentChequeShortTitleComponent_1.PaymentChequeShortTitleComponent;
                break;
            }
            case "GLAccountShortTitleComponent": {
                myResult = GLAccountShortTitleComponent_1.GLAccountShortTitleComponent;
                break;
            }
            case "BankAccountShortTitleComponent": {
                myResult = BankAccountShortTitleComponent_1.BankAccountShortTitleComponent;
                break;
            }
            case "BankDepositShortTitleComponent": {
                myResult = BankDepositShortTitleComponent_1.BankDepositShortTitleComponent;
                break;
            }
            case "ReconciliationShortTitleComponent": {
                myResult = ReconciliationShortTitleComponent_1.ReconciliationShortTitleComponent;
                break;
            }
            case "ExternalReconciliationShortTitleComponent": {
                myResult = ExternalReconciliationShortTitleComponent_1.ExternalReconciliationShortTitleComponent;
                break;
            }
            case "TaxReportShortTitleComponent": {
                myResult = TaxReportShortTitleComponent_1.TaxReportShortTitleComponent;
                break;
            }
            case "GLAccountSearchWindowComponent": {
                myResult = GLAccountSearchWindowComponent_1.GLAccountSearchWindowComponent;
                break;
            }
            case "CancelChequeComponent": {
                myResult = CancelChequeComponent_1.CancelChequeComponent;
                break;
            }
            case "DropdownButtonComponent": {
                myResult = DropdownButtonComponent_1.DropdownButtonComponent;
                break;
            }
            case "JournalReconcileComponent": {
                myResult = JournalReconcileComponent_1.JournalReconcileComponent;
                break;
            }
            case "ExternalReconcileComponent": {
                myResult = ExternalReconcileComponent_1.ExternalReconcileComponent;
                break;
            }
            case "AddEditTaxWithholdingLineComponent": {
                myResult = AddEditTaxWithholdingLineComponent_1.AddEditTaxWithholdingLineComponent;
                break;
            }
            case "AccountingFlatFileDownloadComponent": {
                myResult = AccountingFlatFileDownloadComponent_1.AccountingFlatFileDownloadComponent;
                break;
            }
            case "IntegrityCheckTabComponent": {
                myResult = IntegrityCheckTabComponent_1.IntegrityCheckTabComponent;
                break;
            }
            case "AccountingNoteComponent": {
                myResult = AccountingNoteComponent_1.AccountingNoteComponent;
                break;
            }
            case "NewIntegrityCheckComponent": {
                myResult = NewIntegrityCheckComponent_1.NewIntegrityCheckComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map