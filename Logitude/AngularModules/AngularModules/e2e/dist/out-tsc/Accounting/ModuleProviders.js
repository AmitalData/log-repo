"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var IntegrityCheckStatusListService_1 = require("./Services/StandardLists/IntegrityCheckStatusListService");
var AccountingNoteExtendedListService_1 = require("./Services/ExtendedLists/AccountingNoteExtendedListService");
//#region import services
var AccountingIntegrityCheckListService_1 = require("./Services/StandardLists/AccountingIntegrityCheckListService");
var AccountingEntityListService_1 = require("./Services/StandardLists/AccountingEntityListService");
var AccountingPeriodListService_1 = require("./Services/StandardLists/AccountingPeriodListService");
var AutomaticReconcileListService_1 = require("./Services/StandardLists/AutomaticReconcileListService");
var AutomaticReconcileMethodListService_1 = require("./Services/StandardLists/AutomaticReconcileMethodListService");
var Category1ListService_1 = require("./Services/StandardLists/Category1ListService");
var Category2ListService_1 = require("./Services/StandardLists/Category2ListService");
var Category3ListService_1 = require("./Services/StandardLists/Category3ListService");
var Category4ListService_1 = require("./Services/StandardLists/Category4ListService");
var Category5ListService_1 = require("./Services/StandardLists/Category5ListService");
var ChartOfAccountListService_1 = require("./Services/StandardLists/ChartOfAccountListService");
var ChartOfAccountsTypeListService_1 = require("./Services/StandardLists/ChartOfAccountsTypeListService");
var FullAccountingSettingListService_1 = require("./Services/StandardLists/FullAccountingSettingListService");
var GLAccountBalanceByYearListService_1 = require("./Services/StandardLists/GLAccountBalanceByYearListService");
var GLAccountListService_1 = require("./Services/StandardLists/GLAccountListService");
var GLAccountTotalByMonthListService_1 = require("./Services/StandardLists/GLAccountTotalByMonthListService");
var GLAccountTypeListService_1 = require("./Services/StandardLists/GLAccountTypeListService");
var JournalActionTypeListService_1 = require("./Services/StandardLists/JournalActionTypeListService");
var JournalListService_1 = require("./Services/StandardLists/JournalListService");
var JournalStatusTypeListService_1 = require("./Services/StandardLists/JournalStatusTypeListService");
var JournalTypeListService_1 = require("./Services/StandardLists/JournalTypeListService");
var LedgerTransactionListService_1 = require("./Services/StandardLists/LedgerTransactionListService");
var PeriodTypeListService_1 = require("./Services/StandardLists/PeriodTypeListService");
var ReconcileCurrencyTypeListService_1 = require("./Services/StandardLists/ReconcileCurrencyTypeListService");
var ReconcileMethodListService_1 = require("./Services/StandardLists/ReconcileMethodListService");
var ReconciliationListService_1 = require("./Services/StandardLists/ReconciliationListService");
var RevenueExpenseTypeListService_1 = require("./Services/StandardLists/RevenueExpenseTypeListService");
var TestEntityListService_1 = require("./Services/StandardLists/TestEntityListService");
var TaxReportListService_1 = require("./Services/StandardLists/TaxReportListService");
var TaxReportLineExtendedListService_1 = require("./Services/ExtendedLists/TaxReportLineExtendedListService");
var BankCodeListService_1 = require("./Services/StandardLists/BankCodeListService");
var BankAccountListService_1 = require("./Services/StandardLists/BankAccountListService");
var BankDepositListService_1 = require("./Services/StandardLists/BankDepositListService");
var CashBookListService_1 = require("./Services/StandardLists/CashBookListService");
var CashBookTypeListService_1 = require("./Services/StandardLists/CashBookTypeListService");
var RevaluationListService_1 = require("./Services/StandardLists/RevaluationListService");
var AccountingCompanyTypeListService_1 = require("./Services/StandardLists/AccountingCompanyTypeListService");
var WithholdingTaxDeductionTypeListService_1 = require("./Services/StandardLists/WithholdingTaxDeductionTypeListService");
var GLAccountWithholdingTaxListService_1 = require("./Services/StandardLists/GLAccountWithholdingTaxListService");
var ExternalReconciliationListService_1 = require("./Services/StandardLists/ExternalReconciliationListService");
var TaxReportLineTransmitStatusListService_1 = require("./Services/StandardLists/TaxReportLineTransmitStatusListService");
var TaxReportStatusListService_1 = require("./Services/StandardLists/TaxReportStatusListService");
var TaxReportLineTypeListService_1 = require("./Services/StandardLists/TaxReportLineTypeListService");
var TaxReportLineStatusListService_1 = require("./Services/StandardLists/TaxReportLineStatusListService");
var VatReportStatusListService_1 = require("./Services/StandardLists/VatReportStatusListService");
var GLAccountMoreDataListService_1 = require("./Services/StandardLists/GLAccountMoreDataListService");
var TaxDeductionReportListService_1 = require("./Services/StandardLists/TaxDeductionReportListService");
var OpenFormatReportListService_1 = require("./Services/StandardLists/OpenFormatReportListService");
//import { OpenFormatDateTypeListService } from './Services/StandardLists/OpenFormatDateTypeListService';
var TaxDeductionReportStatusListService_1 = require("./Services/StandardLists/TaxDeductionReportStatusListService");
var TaxReportPMService_1 = require("./Services/StandardPMs/TaxReportPMService");
var AccountingPeriodPMService_1 = require("./Services/StandardPMs/AccountingPeriodPMService");
var AutomaticReconcileMethodPMService_1 = require("./Services/StandardPMs/AutomaticReconcileMethodPMService");
var Category1PMService_1 = require("./Services/StandardPMs/Category1PMService");
var Category2PMService_1 = require("./Services/StandardPMs/Category2PMService");
var Category3PMService_1 = require("./Services/StandardPMs/Category3PMService");
var Category4PMService_1 = require("./Services/StandardPMs/Category4PMService");
var Category5PMService_1 = require("./Services/StandardPMs/Category5PMService");
var ChartOfAccountPMService_1 = require("./Services/StandardPMs/ChartOfAccountPMService");
var FullAccountingSettingPMService_1 = require("./Services/StandardPMs/FullAccountingSettingPMService");
var GLAccountBalanceByYearPMService_1 = require("./Services/StandardPMs/GLAccountBalanceByYearPMService");
var GLAccountPMService_1 = require("./Services/StandardPMs/GLAccountPMService");
var GLAccountTotalByMonthPMService_1 = require("./Services/StandardPMs/GLAccountTotalByMonthPMService");
var JournalActionTypePMService_1 = require("./Services/StandardPMs/JournalActionTypePMService");
var JournalPMService_1 = require("./Services/StandardPMs/JournalPMService");
var LedgerTransactionPMService_1 = require("./Services/StandardPMs/LedgerTransactionPMService");
var ReconciliationPMService_1 = require("./Services/StandardPMs/ReconciliationPMService");
var TestEntityPMService_1 = require("./Services/StandardPMs/TestEntityPMService");
var PaymentChequePMService_1 = require("./Services/StandardPMs/PaymentChequePMService");
var ARPaymentChequePMService_1 = require("./Services/StandardPMs/ARPaymentChequePMService");
var BankAccountPMService_1 = require("./Services/StandardPMs/BankAccountPMService");
var BankCodePMService_1 = require("./Services/StandardPMs/BankCodePMService");
var BankDepositPMService_1 = require("./Services/StandardPMs/BankDepositPMService");
var CashBookPMService_1 = require("./Services/StandardPMs/CashBookPMService");
var RevaluationPMService_1 = require("./Services/StandardPMs/RevaluationPMService");
var TaxWithholdingAssessOfficePMService_1 = require("./Services/StandardPMs/TaxWithholdingAssessOfficePMService");
var AccountingCompanyTypePMService_1 = require("./Services/StandardPMs/AccountingCompanyTypePMService");
var WithholdingTaxDeductionTypePMService_1 = require("./Services/StandardPMs/WithholdingTaxDeductionTypePMService");
var ExternalReconciliationPMService_1 = require("./Services/StandardPMs/ExternalReconciliationPMService");
var GLAccountMoreDataPMService_1 = require("./Services/StandardPMs/GLAccountMoreDataPMService");
var TaxDeductionReportPMService_1 = require("./Services/StandardPMs/TaxDeductionReportPMService");
var OpenFormatReportPMService_1 = require("./Services/StandardPMs/OpenFormatReportPMService");
//#endregion
var AccountingPeriodExtendedListService_1 = require("./Services/ExtendedLists/AccountingPeriodExtendedListService");
var AccountingPeriodExtendedPMService_1 = require("./Services/ExtendedPMs/AccountingPeriodExtendedPMService");
var GLAccountExtendedListService_1 = require("./Services/ExtendedLists/GLAccountExtendedListService");
var JournalExtendedListService_1 = require("./Services/ExtendedLists/JournalExtendedListService");
var LedgerTransactionExtendedListService_1 = require("./Services/ExtendedLists/LedgerTransactionExtendedListService");
var BankDepositExtendedListService_1 = require("./Services/ExtendedLists/BankDepositExtendedListService");
var ReconciliationExtendedPMService_1 = require("./Services/ExtendedPMs/ReconciliationExtendedPMService");
var JournalOpService_1 = require("./Services/Others/JournalOpService");
var BankAccountExtendedListService_1 = require("./Services/ExtendedLists/BankAccountExtendedListService");
var ReconcileExternalPageListService_1 = require("./Services/StandardLists/ReconcileExternalPageListService");
var BankDepositExtendedPMService_1 = require("./Services/ExtendedPMs/BankDepositExtendedPMService");
var AccountingOpService_1 = require("./Services/Others/AccountingOpService");
var PaymentChequeListService_1 = require("./Services/StandardLists/PaymentChequeListService");
var TaxWithholdingAssessOfficeListService_1 = require("./Services/StandardLists/TaxWithholdingAssessOfficeListService");
var ExternalReconciliationExtendedPMService_1 = require("./Services/ExtendedPMs/ExternalReconciliationExtendedPMService");
var ReconcileExternalPageExtendedListService_1 = require("./Services/ExtendedLists/ReconcileExternalPageExtendedListService");
var ExternalReconciliationExtendedListService_1 = require("./Services/ExtendedLists/ExternalReconciliationExtendedListService");
var AutomaticExternalReconcileMethodListService_1 = require("./Services/StandardLists/AutomaticExternalReconcileMethodListService");
var TaxReportExtendedPMService_1 = require("./Services/ExtendedPMs/TaxReportExtendedPMService");
var OpenFormatReportStatusListService_1 = require("./Services/StandardLists/OpenFormatReportStatusListService");
var JournalMenuButtonsHandler_1 = require("./Components/MenuButtons/JournalMenuButtonsHandler");
var GLAccountMenuButtonsHandler_1 = require("./Components/MenuButtons/GLAccountMenuButtonsHandler");
var CashBookMenuButtonsHandler_1 = require("./Components/MenuButtons/CashBookMenuButtonsHandler");
var BankDepositMenuButtonsHandler_1 = require("./Components/MenuButtons/BankDepositMenuButtonsHandler");
var PaymentChequeMenuButtonsHandler_1 = require("./Components/MenuButtons/PaymentChequeMenuButtonsHandler");
var BankAccountMenuButtonsHandler_1 = require("./Components/MenuButtons/BankAccountMenuButtonsHandler");
var ReconciliationMenuButtonsHandler_1 = require("./Components/MenuButtons/ReconciliationMenuButtonsHandler");
var ExternalReconciliationMenuButtonsHandler_1 = require("./Components/MenuButtons/ExternalReconciliationMenuButtonsHandler");
var TaxReportMenuButtonsHandler_1 = require("./Components/MenuButtons/TaxReportMenuButtonsHandler");
var TaxDeductionReportMenuButtonsHandler_1 = require("./Components/MenuButtons/TaxDeductionReportMenuButtonsHandler");
var OpenFormatReportMenuButtonsHandler_1 = require("./Components/MenuButtons/OpenFormatReportMenuButtonsHandler");
var AccountingIntegrityCheckPMService_1 = require("./Services/StandardPMs/AccountingIntegrityCheckPMService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            //#region standerd services
            case "AccountingEntityListService": {
                myResult = new AccountingEntityListService_1.AccountingEntityListService();
                break;
            }
            case "AccountingPeriodListService": {
                myResult = new AccountingPeriodListService_1.AccountingPeriodListService();
                break;
            }
            case "AutomaticReconcileListService": {
                myResult = new AutomaticReconcileListService_1.AutomaticReconcileListService();
                break;
            }
            case "AutomaticReconcileMethodListService": {
                myResult = new AutomaticReconcileMethodListService_1.AutomaticReconcileMethodListService();
                break;
            }
            case "Category1ListService": {
                myResult = new Category1ListService_1.Category1ListService();
                break;
            }
            case "Category2ListService": {
                myResult = new Category2ListService_1.Category2ListService();
                break;
            }
            case "Category3ListService": {
                myResult = new Category3ListService_1.Category3ListService();
                break;
            }
            case "Category4ListService": {
                myResult = new Category4ListService_1.Category4ListService();
                break;
            }
            case "Category5ListService": {
                myResult = new Category5ListService_1.Category5ListService();
                break;
            }
            case "ChartOfAccountListService": {
                myResult = new ChartOfAccountListService_1.ChartOfAccountListService();
                break;
            }
            case "ChartOfAccountsTypeListService": {
                myResult = new ChartOfAccountsTypeListService_1.ChartOfAccountsTypeListService();
                break;
            }
            case "FullAccountingSettingListService": {
                myResult = new FullAccountingSettingListService_1.FullAccountingSettingListService();
                break;
            }
            case "GLAccountBalanceByYearListService": {
                myResult = new GLAccountBalanceByYearListService_1.GLAccountBalanceByYearListService();
                break;
            }
            case "GLAccountListService": {
                myResult = new GLAccountListService_1.GLAccountListService();
                break;
            }
            case "GLAccountTotalByMonthListService": {
                myResult = new GLAccountTotalByMonthListService_1.GLAccountTotalByMonthListService();
                break;
            }
            case "TaxReportLineExtendedListService": {
                myResult = new TaxReportLineExtendedListService_1.TaxReportLineExtendedListService();
                break;
            }
            case "GLAccountTypeListService": {
                myResult = new GLAccountTypeListService_1.GLAccountTypeListService();
                break;
            }
            case "JournalActionTypeListService": {
                myResult = new JournalActionTypeListService_1.JournalActionTypeListService();
                break;
            }
            case "JournalListService": {
                myResult = new JournalListService_1.JournalListService();
                break;
            }
            case "JournalStatusTypeListService": {
                myResult = new JournalStatusTypeListService_1.JournalStatusTypeListService();
                break;
            }
            case "JournalTypeListService": {
                myResult = new JournalTypeListService_1.JournalTypeListService();
                break;
            }
            case "LedgerTransactionListService": {
                myResult = new LedgerTransactionListService_1.LedgerTransactionListService();
                break;
            }
            case "PeriodTypeListService": {
                myResult = new PeriodTypeListService_1.PeriodTypeListService();
                break;
            }
            case "ReconcileCurrencyTypeListService": {
                myResult = new ReconcileCurrencyTypeListService_1.ReconcileCurrencyTypeListService();
                break;
            }
            case "ReconcileMethodListService": {
                myResult = new ReconcileMethodListService_1.ReconcileMethodListService();
                break;
            }
            case "ReconciliationListService": {
                myResult = new ReconciliationListService_1.ReconciliationListService();
                break;
            }
            case "RevenueExpenseTypeListService": {
                myResult = new RevenueExpenseTypeListService_1.RevenueExpenseTypeListService();
                break;
            }
            case "TestEntityListService": {
                myResult = new TestEntityListService_1.TestEntityListService();
                break;
            }
            case "TestEntityListService": {
                myResult = new TestEntityListService_1.TestEntityListService();
                break;
            }
            case "BankAccountListService": {
                myResult = new BankAccountListService_1.BankAccountListService();
                break;
            }
            case "BankCodeListService": {
                myResult = new BankCodeListService_1.BankCodeListService();
                break;
            }
            case "BankDepositListService": {
                myResult = new BankDepositListService_1.BankDepositListService();
                break;
            }
            case "CashBookListService": {
                myResult = new CashBookListService_1.CashBookListService();
                break;
            }
            case "CashBookTypeListService": {
                myResult = new CashBookTypeListService_1.CashBookTypeListService();
                break;
            }
            case "RevaluationListService": {
                myResult = new RevaluationListService_1.RevaluationListService();
                break;
            }
            case "PaymentChequeListService": {
                myResult = new PaymentChequeListService_1.PaymentChequeListService();
                break;
            }
            case "AccountingCompanyTypeListService": {
                myResult = new AccountingCompanyTypeListService_1.AccountingCompanyTypeListService();
                break;
            }
            case "WithholdingTaxDeductionTypeListService": {
                myResult = new WithholdingTaxDeductionTypeListService_1.WithholdingTaxDeductionTypeListService();
                break;
            }
            case "GLAccountWithholdingTaxListService": {
                myResult = new GLAccountWithholdingTaxListService_1.GLAccountWithholdingTaxListService();
                break;
            }
            case "AutomaticExternalRconcilMthodListService": {
                myResult = new AutomaticExternalReconcileMethodListService_1.AutomaticExternalRconcilMthodListService();
                break;
            }
            case "ExternalReconciliationListService": {
                myResult = new ExternalReconciliationListService_1.ExternalReconciliationListService();
                break;
            }
            case "TaxReportListService": {
                myResult = new TaxReportListService_1.TaxReportListService();
                break;
            }
            case "TaxDeductionReportListService": {
                myResult = new TaxDeductionReportListService_1.TaxDeductionReportListService();
                break;
            }
            case "OpenFormatReportListService": {
                myResult = new OpenFormatReportListService_1.OpenFormatReportListService();
                break;
            }
            case "TaxDeductionReportPMService": {
                myResult = new TaxDeductionReportPMService_1.TaxDeductionReportPMService();
                break;
            }
            case "OpenFormatReportPMService": {
                myResult = new OpenFormatReportPMService_1.OpenFormatReportPMService();
                break;
            }
            case "AccountingPeriodPMService": {
                myResult = new AccountingPeriodPMService_1.AccountingPeriodPMService();
                break;
            }
            case "AutomaticReconcileMethodPMService": {
                myResult = new AutomaticReconcileMethodPMService_1.AutomaticReconcileMethodPMService();
                break;
            }
            case "Category1PMService": {
                myResult = new Category1PMService_1.Category1PMService();
                break;
            }
            case "Category2PMService": {
                myResult = new Category2PMService_1.Category2PMService();
                break;
            }
            case "Category3PMService": {
                myResult = new Category3PMService_1.Category3PMService();
                break;
            }
            case "Category4PMService": {
                myResult = new Category4PMService_1.Category4PMService();
                break;
            }
            case "Category5PMService": {
                myResult = new Category5PMService_1.Category5PMService();
                break;
            }
            case "ChartOfAccountPMService": {
                myResult = new ChartOfAccountPMService_1.ChartOfAccountPMService();
                break;
            }
            case "FullAccountingSettingPMService": {
                myResult = new FullAccountingSettingPMService_1.FullAccountingSettingPMService();
                break;
            }
            case "GLAccountBalanceByYearPMService": {
                myResult = new GLAccountBalanceByYearPMService_1.GLAccountBalanceByYearPMService();
                break;
            }
            case "GLAccountPMService": {
                myResult = new GLAccountPMService_1.GLAccountPMService();
                break;
            }
            case "GLAccountTotalByMonthPMService": {
                myResult = new GLAccountTotalByMonthPMService_1.GLAccountTotalByMonthPMService();
                break;
            }
            case "JournalActionTypePMService": {
                myResult = new JournalActionTypePMService_1.JournalActionTypePMService();
                break;
            }
            case "JournalPMService": {
                myResult = new JournalPMService_1.JournalPMService();
                break;
            }
            case "LedgerTransactionPMService": {
                myResult = new LedgerTransactionPMService_1.LedgerTransactionPMService();
                break;
            }
            case "ReconciliationPMService": {
                myResult = new ReconciliationPMService_1.ReconciliationPMService();
                break;
            }
            case "TestEntityPMService": {
                myResult = new TestEntityPMService_1.TestEntityPMService();
                break;
            }
            case "PaymentChequePMService": {
                myResult = new PaymentChequePMService_1.PaymentChequePMService();
                break;
            }
            case "ARPaymentChequePMService": {
                myResult = new ARPaymentChequePMService_1.ARPaymentChequePMService();
                break;
            }
            case "BankAccountPMService": {
                myResult = new BankAccountPMService_1.BankAccountPMService();
                break;
            }
            case "BankCodePMService": {
                myResult = new BankCodePMService_1.BankCodePMService();
                break;
            }
            case "BankDepositPMService": {
                myResult = new BankDepositPMService_1.BankDepositPMService();
                break;
            }
            case "CashBookPMService": {
                myResult = new CashBookPMService_1.CashBookPMService();
                break;
            }
            case "RevaluationPMService": {
                myResult = new RevaluationPMService_1.RevaluationPMService();
                break;
            }
            case "TaxWithholdingAssessOfficePMService": {
                myResult = new TaxWithholdingAssessOfficePMService_1.TaxWithholdingAssessOfficePMService();
                break;
            }
            case "AccountingCompanyTypePMService": {
                myResult = new AccountingCompanyTypePMService_1.AccountingCompanyTypePMService();
                break;
            }
            case "WithholdingTaxDeductionTypePMService": {
                myResult = new WithholdingTaxDeductionTypePMService_1.WithholdingTaxDeductionTypePMService();
                break;
            }
            case "ExternalReconciliationPMService": {
                myResult = new ExternalReconciliationPMService_1.ExternalReconciliationPMService();
                break;
            }
            case "TaxReportPMService": {
                myResult = new TaxReportPMService_1.TaxReportPMService();
                break;
            }
            case "TaxReportLineTransmitStatusListService": {
                myResult = new TaxReportLineTransmitStatusListService_1.TaxReportLineTransmitStatusListService();
                break;
            }
            case "GLAccountMoreDataPMService": {
                myResult = new GLAccountMoreDataPMService_1.GLAccountMoreDataPMService();
                break;
            }
            case "TaxReportStatusListService": {
                myResult = new TaxReportStatusListService_1.TaxReportStatusListService();
                break;
            }
            case "TaxReportLineTypeListService": {
                myResult = new TaxReportLineTypeListService_1.TaxReportLineTypeListService();
                break;
            }
            case "TaxReportLineStatusListService": {
                myResult = new TaxReportLineStatusListService_1.TaxReportLineStatusListService();
                break;
            }
            case "VatReportStatusListService": {
                myResult = new VatReportStatusListService_1.VatReportStatusListService();
                break;
            }
            case "GLAccountMoreDataListService": {
                myResult = new GLAccountMoreDataListService_1.GLAccountMoreDataListService();
                break;
            }
            // case "OpenFormatDateTypeListService": { myResult = new OpenFormatDateTypeListService(); break; }
            case "TaxDeductionReportStatusListService": {
                myResult = new TaxDeductionReportStatusListService_1.TaxDeductionReportStatusListService();
                break;
            }
            case "OpenFormatReportStatusListService": {
                myResult = new OpenFormatReportStatusListService_1.OpenFormatReportStatusListService();
                break;
            }
            //#endregion
            //Extend Services
            case "AccountingPeriodExtendedListService": {
                myResult = new AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService();
                break;
            }
            case "AccountingPeriodExtendedPMService": {
                myResult = new AccountingPeriodExtendedPMService_1.AccountingPeriodExtendedPMService();
                break;
            }
            case "GLAccountExtendedListService": {
                myResult = new GLAccountExtendedListService_1.GLAccountExtendedListService();
                break;
            }
            case "JournalExtendedListService": {
                myResult = new JournalExtendedListService_1.JournalExtendedListService();
                break;
            }
            case "LedgerTransactionExtendedListService": {
                myResult = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
                break;
            }
            case "BankDepositExtendedListService": {
                myResult = new BankDepositExtendedListService_1.BankDepositExtendedListService();
                break;
            }
            case "BankAccountExtendedListService": {
                myResult = new BankAccountExtendedListService_1.BankAccountExtendedListService();
                break;
            }
            case "ReconciliationExtendedPMService": {
                myResult = new ReconciliationExtendedPMService_1.ReconciliationExtendedPMService();
                break;
            }
            case "JournalOpService": {
                myResult = new JournalOpService_1.JournalOpService();
                break;
            }
            case "ReconcileExternalPageListService": {
                myResult = new ReconcileExternalPageListService_1.ReconcileExternalPageListService();
                break;
            }
            case "TaxWithholdingAssessOfficeListService": {
                myResult = new TaxWithholdingAssessOfficeListService_1.TaxWithholdingAssessOfficeListService();
                break;
            }
            case "ExternalReconciliationExtendedPMService": {
                myResult = new ExternalReconciliationExtendedPMService_1.ExternalReconciliationExtendedPMService();
                break;
            }
            case "ReconcileExternalPageExtendedListService": {
                myResult = new ReconcileExternalPageExtendedListService_1.ReconcileExternalPageExtendedListService();
                break;
            }
            case "ExternalReconciliationExtendedListService": {
                myResult = new ExternalReconciliationExtendedListService_1.ExternalReconciliationExtendedListService();
                break;
            }
            case "BankDepositExtendedPMService": {
                myResult = new BankDepositExtendedPMService_1.BankDepositExtendedPMService();
                break;
            }
            case "TaxReportExtendedPMService": {
                myResult = new TaxReportExtendedPMService_1.TaxReportExtendedPMService();
                break;
            }
            case "AccountingIntegrityCheckListService": {
                myResult = new AccountingIntegrityCheckListService_1.AccountingIntegrityCheckListService;
                break;
            }
            case "AccountingIntegrityCheckPMService": {
                myResult = new AccountingIntegrityCheckPMService_1.AccountingIntegrityCheckPMService;
                break;
            }
            case "AccountingNoteExtendedListService": {
                myResult = new AccountingNoteExtendedListService_1.AccountingNoteExtendedListService;
                break;
            }
            case "IntegrityCheckStatusListService": {
                myResult = new IntegrityCheckStatusListService_1.IntegrityCheckStatusListService;
                break;
            }
            case "GLAccountOpService": {
                myResult = new AccountingOpService_1.AccountingOpService();
                break;
            }
            //Menu Buttons
            case "JournalMenuButtonsHandler": {
                myResult = new JournalMenuButtonsHandler_1.JournalMenuButtonsHandler;
                break;
            }
            case "GLAccountMenuButtonsHandler": {
                myResult = new GLAccountMenuButtonsHandler_1.GLAccountMenuButtonsHandler;
                break;
            }
            case "CashBookMenuButtonsHandler": {
                myResult = new CashBookMenuButtonsHandler_1.CashBookMenuButtonsHandler;
                break;
            }
            case "BankDepositMenuButtonsHandler": {
                myResult = new BankDepositMenuButtonsHandler_1.BankDepositMenuButtonsHandler;
                break;
            }
            case "PaymentChequeMenuButtonsHandler": {
                myResult = new PaymentChequeMenuButtonsHandler_1.PaymentChequeMenuButtonsHandler;
                break;
            }
            case "BankAccountMenuButtonsHandler": {
                myResult = new BankAccountMenuButtonsHandler_1.BankAccountMenuButtonsHandler;
                break;
            }
            case "ReconciliationMenuButtonsHandler": {
                myResult = new ReconciliationMenuButtonsHandler_1.ReconciliationMenuButtonsHandler;
                break;
            }
            case "ExternalReconciliationMenuButtonsHandler": {
                myResult = new ExternalReconciliationMenuButtonsHandler_1.ExternalReconciliationMenuButtonsHandler;
                break;
            }
            case "TaxReportMenuButtonsHandler": {
                myResult = new TaxReportMenuButtonsHandler_1.TaxReportMenuButtonsHandler;
                break;
            }
            case "TaxDeductionReportMenuButtonsHandler": {
                myResult = new TaxDeductionReportMenuButtonsHandler_1.TaxDeductionReportMenuButtonsHandler;
                break;
            }
            case "OpenFormatReportMenuButtonsHandler": {
                myResult = new OpenFormatReportMenuButtonsHandler_1.OpenFormatReportMenuButtonsHandler;
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map