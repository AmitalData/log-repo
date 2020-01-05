import { IntegrityCheckStatusListService } from './Services/StandardLists/IntegrityCheckStatusListService';
import { AccountingNoteExtendedListService } from './Services/ExtendedLists/AccountingNoteExtendedListService';
//#region import services
import { AccountingIntegrityCheckListService } from './Services/StandardLists/AccountingIntegrityCheckListService';
import { AccountingEntityListService } from './Services/StandardLists/AccountingEntityListService';
import { AccountingPeriodListService } from './Services/StandardLists/AccountingPeriodListService';
import { AutomaticReconcileListService } from './Services/StandardLists/AutomaticReconcileListService';
import { AutomaticReconcileMethodListService } from './Services/StandardLists/AutomaticReconcileMethodListService';
import { Category1ListService } from './Services/StandardLists/Category1ListService';
import { Category2ListService } from './Services/StandardLists/Category2ListService';
import { Category3ListService } from './Services/StandardLists/Category3ListService';
import { Category4ListService } from './Services/StandardLists/Category4ListService';
import { Category5ListService } from './Services/StandardLists/Category5ListService';
import { ChartOfAccountListService } from './Services/StandardLists/ChartOfAccountListService';
import { ChartOfAccountsTypeListService } from './Services/StandardLists/ChartOfAccountsTypeListService';
import { FullAccountingSettingListService } from './Services/StandardLists/FullAccountingSettingListService';
import { GLAccountBalanceByYearListService } from './Services/StandardLists/GLAccountBalanceByYearListService';
import { GLAccountListService } from './Services/StandardLists/GLAccountListService';
import { GLAccountTotalByMonthListService } from './Services/StandardLists/GLAccountTotalByMonthListService';
import { GLAccountTypeListService } from './Services/StandardLists/GLAccountTypeListService';
import { JournalActionTypeListService } from './Services/StandardLists/JournalActionTypeListService';
import { JournalListService } from './Services/StandardLists/JournalListService';
import { JournalStatusTypeListService } from './Services/StandardLists/JournalStatusTypeListService';
import { JournalTypeListService } from './Services/StandardLists/JournalTypeListService';
import { LedgerTransactionListService } from './Services/StandardLists/LedgerTransactionListService';
import { PeriodTypeListService } from './Services/StandardLists/PeriodTypeListService';
import { ReconcileCurrencyTypeListService } from './Services/StandardLists/ReconcileCurrencyTypeListService';
import { ReconcileMethodListService } from './Services/StandardLists/ReconcileMethodListService';
import { ReconciliationListService } from './Services/StandardLists/ReconciliationListService';
import { ReconciliationLineListService } from './Services/StandardLists/ReconciliationLineListService';
import { CashBookLineListService } from './Services/StandardLists/CashBookLineListService';
import { RevenueExpenseTypeListService } from './Services/StandardLists/RevenueExpenseTypeListService';
import { TestEntityListService } from './Services/StandardLists/TestEntityListService';
import { TaxReportListService } from './Services/StandardLists/TaxReportListService';
import { TaxReportLineExtendedListService } from './Services/ExtendedLists/TaxReportLineExtendedListService';

import { BankCodeListService } from './Services/StandardLists/BankCodeListService';
import { BankAccountListService } from './Services/StandardLists/BankAccountListService';
import { BankDepositListService } from './Services/StandardLists/BankDepositListService';
import { CashBookListService } from './Services/StandardLists/CashBookListService';
import { CashBookTypeListService } from './Services/StandardLists/CashBookTypeListService';
import { RevaluationListService } from './Services/StandardLists/RevaluationListService';
import { AccountingCompanyTypeListService } from './Services/StandardLists/AccountingCompanyTypeListService';
import { WithholdingTaxDeductionTypeListService } from './Services/StandardLists/WithholdingTaxDeductionTypeListService';
import { GLAccountWithholdingTaxListService } from './Services/StandardLists/GLAccountWithholdingTaxListService';
import { ExternalReconciliationListService } from './Services/StandardLists/ExternalReconciliationListService';
import { TaxReportLineTransmitStatusListService } from './Services/StandardLists/TaxReportLineTransmitStatusListService';
import { TaxReportStatusListService } from './Services/StandardLists/TaxReportStatusListService';
import { TaxReportLineTypeListService } from './Services/StandardLists/TaxReportLineTypeListService';
import { TaxReportLineStatusListService } from './Services/StandardLists/TaxReportLineStatusListService';
import { VatReportStatusListService } from './Services/StandardLists/VatReportStatusListService';
import { GLAccountMoreDataListService } from './Services/StandardLists/GLAccountMoreDataListService';
import { TaxDeductionReportListService } from './Services/StandardLists/TaxDeductionReportListService';
import { OpenFormatReportListService } from './Services/StandardLists/OpenFormatReportListService';
//import { OpenFormatDateTypeListService } from './Services/StandardLists/OpenFormatDateTypeListService';
import { TaxDeductionReportStatusListService } from './Services/StandardLists/TaxDeductionReportStatusListService';
import { TaxReportPMService } from './Services/StandardPMs/TaxReportPMService';
import { AccountingPeriodPMService } from './Services/StandardPMs/AccountingPeriodPMService';
import { AutomaticReconcileMethodPMService } from './Services/StandardPMs/AutomaticReconcileMethodPMService';
import { Category1PMService } from './Services/StandardPMs/Category1PMService';
import { Category2PMService } from './Services/StandardPMs/Category2PMService';
import { Category3PMService } from './Services/StandardPMs/Category3PMService';
import { Category4PMService } from './Services/StandardPMs/Category4PMService';
import { Category5PMService } from './Services/StandardPMs/Category5PMService';
import { ChartOfAccountPMService } from './Services/StandardPMs/ChartOfAccountPMService';
import { FullAccountingSettingPMService } from './Services/StandardPMs/FullAccountingSettingPMService';
import { GLAccountBalanceByYearPMService } from './Services/StandardPMs/GLAccountBalanceByYearPMService';
import { GLAccountPMService } from './Services/StandardPMs/GLAccountPMService';
import { GLAccountTotalByMonthPMService } from './Services/StandardPMs/GLAccountTotalByMonthPMService';
import { JournalActionTypePMService } from './Services/StandardPMs/JournalActionTypePMService';
import { JournalPMService } from './Services/StandardPMs/JournalPMService';
import { LedgerTransactionPMService } from './Services/StandardPMs/LedgerTransactionPMService';
import { ReconciliationPMService } from './Services/StandardPMs/ReconciliationPMService';
import { TestEntityPMService } from './Services/StandardPMs/TestEntityPMService';
import { PaymentChequePMService } from './Services/StandardPMs/PaymentChequePMService';
import { ARPaymentChequePMService } from './Services/StandardPMs/ARPaymentChequePMService';
import { BankAccountPMService } from './Services/StandardPMs/BankAccountPMService';
import { BankCodePMService } from './Services/StandardPMs/BankCodePMService';
import { BankDepositPMService } from './Services/StandardPMs/BankDepositPMService';
import { CashBookPMService } from './Services/StandardPMs/CashBookPMService';
import { RevaluationPMService } from './Services/StandardPMs/RevaluationPMService';
import { TaxWithholdingAssessOfficePMService } from './Services/StandardPMs/TaxWithholdingAssessOfficePMService';
import { AccountingCompanyTypePMService } from './Services/StandardPMs/AccountingCompanyTypePMService';
import { WithholdingTaxDeductionTypePMService } from './Services/StandardPMs/WithholdingTaxDeductionTypePMService';
import { ExternalReconciliationPMService } from './Services/StandardPMs/ExternalReconciliationPMService';
import { GLAccountMoreDataPMService } from './Services/StandardPMs/GLAccountMoreDataPMService';
import { TaxDeductionReportPMService } from './Services/StandardPMs/TaxDeductionReportPMService';
import { OpenFormatReportPMService } from './Services/StandardPMs/OpenFormatReportPMService';
import { InterestReportLinePMService } from './Services/StandardPMs/InterestReportLinePMService';
import { InterestReportPMService } from './Services/StandardPMs/InterestReportPMService';

//#endregion
import { AccountingPeriodExtendedListService } from './Services/ExtendedLists/AccountingPeriodExtendedListService';
import { AccountingPeriodExtendedPMService } from './Services/ExtendedPMs/AccountingPeriodExtendedPMService';
import { GLAccountExtendedListService } from './Services/ExtendedLists/GLAccountExtendedListService';
import { JournalExtendedListService } from './Services/ExtendedLists/JournalExtendedListService';
import { LedgerTransactionExtendedListService } from './Services/ExtendedLists/LedgerTransactionExtendedListService';
import { BankDepositExtendedListService } from './Services/ExtendedLists/BankDepositExtendedListService';



import { ReconciliationExtendedPMService } from './Services/ExtendedPMs/ReconciliationExtendedPMService';
import { CashBookExtendedPMService } from './Services/ExtendedPMs/CashBookExtendedPMService';
import { JournalOpService } from './Services/Others/JournalOpService';
import { BankAccountExtendedListService } from './Services/ExtendedLists/BankAccountExtendedListService';
import { ReconcileExternalPageListService } from './Services/StandardLists/ReconcileExternalPageListService';
import { BankDepositExtendedPMService } from './Services/ExtendedPMs/BankDepositExtendedPMService';
import { AccountingOpService } from './Services/Others/AccountingOpService';

import { PaymentChequeListService } from './Services/StandardLists/PaymentChequeListService';
import { TaxWithholdingAssessOfficeListService } from './Services/StandardLists/TaxWithholdingAssessOfficeListService';
import { ExternalReconciliationExtendedPMService } from './Services/ExtendedPMs/ExternalReconciliationExtendedPMService';
import { ReconcileExternalPageExtendedListService } from './Services/ExtendedLists/ReconcileExternalPageExtendedListService';
import { ExternalReconciliationExtendedListService } from './Services/ExtendedLists/ExternalReconciliationExtendedListService';
import { AutomaticExternalRconcilMthodListService } from './Services/StandardLists/AutomaticExternalReconcileMethodListService';
import { TaxReportExtendedPMService } from './Services/ExtendedPMs/TaxReportExtendedPMService';
import { OpenFormatReportStatusListService } from './Services/StandardLists/OpenFormatReportStatusListService';

import { InterestBasesTypeMenuButtonsHandler } from './Components/MenuButtons/InterestBasesTypeMenuButtonsHandler';
import { JournalMenuButtonsHandler } from './Components/MenuButtons/JournalMenuButtonsHandler';
import { GLAccountMenuButtonsHandler } from './Components/MenuButtons/GLAccountMenuButtonsHandler';
import { CashBookMenuButtonsHandler } from './Components/MenuButtons/CashBookMenuButtonsHandler';
import { BankDepositMenuButtonsHandler } from './Components/MenuButtons/BankDepositMenuButtonsHandler';
import { PaymentChequeMenuButtonsHandler } from './Components/MenuButtons/PaymentChequeMenuButtonsHandler';
import { BankAccountMenuButtonsHandler } from './Components/MenuButtons/BankAccountMenuButtonsHandler';
import { ReconciliationMenuButtonsHandler } from './Components/MenuButtons/ReconciliationMenuButtonsHandler';
import { ExternalReconciliationMenuButtonsHandler } from './Components/MenuButtons/ExternalReconciliationMenuButtonsHandler';
import { TaxReportMenuButtonsHandler } from './Components/MenuButtons/TaxReportMenuButtonsHandler';
import { TaxDeductionReportMenuButtonsHandler } from './Components/MenuButtons/TaxDeductionReportMenuButtonsHandler';
import { OpenFormatReportMenuButtonsHandler } from './Components/MenuButtons/OpenFormatReportMenuButtonsHandler';
import { AccountingIntegrityCheckPMService } from './Services/StandardPMs/AccountingIntegrityCheckPMService';
import { ExternalPageAdditionalDataListService } from './Services/StandardLists/ExternalPageAdditionalDataListService';
import { ExternalPageAdditionalDataPMService } from './Services/StandardPMs/ExternalPageAdditionalDataPMService';
import { InterestBasesPeriodListService } from './Services/StandardLists/InterestBasesPeriodListService';
import { InterestBasesTypeListService } from './Services/StandardLists/InterestBasesTypeListService';
import { InterestBasesPeriodPMService } from './Services/StandardPMs/InterestBasesPeriodPMService';
import { InterestBasesTypePMService } from './Services/StandardPMs/InterestBasesTypePMService';
import { InterestReportListService } from './Services/StandardLists/InterestReportListService';
import { InterestReportLineListService } from './Services/StandardLists/InterestReportLineListService';
import { InterestReportStatuseListService } from './Services/StandardLists/InterestReportStatuseListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            //#region standerd services
            case "AccountingEntityListService": { myResult = new AccountingEntityListService(); break; }
            case "AccountingPeriodListService": { myResult = new AccountingPeriodListService(); break; }
            case "AutomaticReconcileListService": { myResult = new AutomaticReconcileListService(); break; }
            case "AutomaticReconcileMethodListService": { myResult = new AutomaticReconcileMethodListService(); break; }
            case "Category1ListService": { myResult = new Category1ListService(); break; }
            case "Category2ListService": { myResult = new Category2ListService(); break; }
            case "Category3ListService": { myResult = new Category3ListService(); break; }
            case "Category4ListService": { myResult = new Category4ListService(); break; }
            case "Category5ListService": { myResult = new Category5ListService(); break; }
            case "ChartOfAccountListService": { myResult = new ChartOfAccountListService(); break; }
            case "ChartOfAccountsTypeListService": { myResult = new ChartOfAccountsTypeListService(); break; }
            case "FullAccountingSettingListService": { myResult = new FullAccountingSettingListService(); break; }
            case "GLAccountBalanceByYearListService": { myResult = new GLAccountBalanceByYearListService(); break; }
            case "GLAccountListService": { myResult = new GLAccountListService(); break; }
            case "GLAccountTotalByMonthListService": { myResult = new GLAccountTotalByMonthListService(); break; }
            case "TaxReportLineExtendedListService": { myResult = new TaxReportLineExtendedListService(); break; }
            case "GLAccountTypeListService": { myResult = new GLAccountTypeListService(); break; }
            case "JournalActionTypeListService": { myResult = new JournalActionTypeListService(); break; }
            case "JournalListService": { myResult = new JournalListService(); break; }
            case "JournalStatusTypeListService": { myResult = new JournalStatusTypeListService(); break; }
            case "JournalTypeListService": { myResult = new JournalTypeListService(); break; }
            case "LedgerTransactionListService": { myResult = new LedgerTransactionListService(); break; }
            case "PeriodTypeListService": { myResult = new PeriodTypeListService(); break; }
            case "ReconcileCurrencyTypeListService": { myResult = new ReconcileCurrencyTypeListService(); break; }
            case "ReconcileMethodListService": { myResult = new ReconcileMethodListService(); break; }
            case "ReconciliationListService": { myResult = new ReconciliationListService(); break; }
            case "ReconciliationLineListService": { myResult = new ReconciliationLineListService(); break; }
            case "CashBookLineListService": { myResult = new CashBookLineListService(); break; }
            case "RevenueExpenseTypeListService": { myResult = new RevenueExpenseTypeListService(); break; }
            case "TestEntityListService": { myResult = new TestEntityListService(); break; }
            case "TestEntityListService": { myResult = new TestEntityListService(); break; }

            case "BankAccountListService": { myResult = new BankAccountListService(); break; }
            case "BankCodeListService": { myResult = new BankCodeListService(); break; }
            case "BankDepositListService": { myResult = new BankDepositListService(); break; }
            case "CashBookListService": { myResult = new CashBookListService(); break; }
            case "CashBookTypeListService": { myResult = new CashBookTypeListService(); break; }
            case "RevaluationListService": { myResult = new RevaluationListService(); break; }

            case "PaymentChequeListService": { myResult = new PaymentChequeListService(); break; }
            case "AccountingCompanyTypeListService": { myResult = new AccountingCompanyTypeListService(); break; }
            case "WithholdingTaxDeductionTypeListService": { myResult = new WithholdingTaxDeductionTypeListService(); break; }
            case "GLAccountWithholdingTaxListService": { myResult = new GLAccountWithholdingTaxListService(); break; }
            case "AutomaticExternalRconcilMthodListService": { myResult = new AutomaticExternalRconcilMthodListService(); break; }
            case "ExternalReconciliationListService": { myResult = new ExternalReconciliationListService(); break; }
            case "TaxReportListService": { myResult = new TaxReportListService(); break; }
            case "TaxDeductionReportListService": { myResult = new TaxDeductionReportListService(); break; }
            case "OpenFormatReportListService": { myResult = new OpenFormatReportListService(); break; }
            case "InterestReportListService": { myResult = new InterestReportListService(); break; }
            case "InterestReportLineListService": { myResult = new InterestReportLineListService(); break; }

            case "TaxDeductionReportPMService": { myResult = new TaxDeductionReportPMService(); break; }
            case "OpenFormatReportPMService": { myResult = new OpenFormatReportPMService(); break; }
            case "ExternalPageAdditionalDataPMService": { myResult = new ExternalPageAdditionalDataPMService(); break; }

            case "AccountingPeriodPMService": { myResult = new AccountingPeriodPMService(); break; }
            case "AutomaticReconcileMethodPMService": { myResult = new AutomaticReconcileMethodPMService(); break; }
            case "Category1PMService": { myResult = new Category1PMService(); break; }
            case "Category2PMService": { myResult = new Category2PMService(); break; }
            case "Category3PMService": { myResult = new Category3PMService(); break; }
            case "Category4PMService": { myResult = new Category4PMService(); break; }
            case "Category5PMService": { myResult = new Category5PMService(); break; }
            case "ChartOfAccountPMService": { myResult = new ChartOfAccountPMService(); break; }
            case "FullAccountingSettingPMService": { myResult = new FullAccountingSettingPMService(); break; }
            case "GLAccountBalanceByYearPMService": { myResult = new GLAccountBalanceByYearPMService(); break; }
            case "GLAccountPMService": { myResult = new GLAccountPMService(); break; }
            case "GLAccountTotalByMonthPMService": { myResult = new GLAccountTotalByMonthPMService(); break; }
            case "JournalActionTypePMService": { myResult = new JournalActionTypePMService(); break; }
            case "JournalPMService": { myResult = new JournalPMService(); break; }
            case "LedgerTransactionPMService": { myResult = new LedgerTransactionPMService(); break; }
            case "ReconciliationPMService": { myResult = new ReconciliationPMService(); break; }
            case "TestEntityPMService": { myResult = new TestEntityPMService(); break; }
            case "PaymentChequePMService": { myResult = new PaymentChequePMService(); break; }
            case "ARPaymentChequePMService": { myResult = new ARPaymentChequePMService(); break; }
            case "BankAccountPMService": { myResult = new BankAccountPMService(); break; }
            case "BankCodePMService": { myResult = new BankCodePMService(); break; }
            case "BankDepositPMService": { myResult = new BankDepositPMService(); break; }
            case "CashBookPMService": { myResult = new CashBookPMService(); break; }
            case "RevaluationPMService": { myResult = new RevaluationPMService(); break; }
            case "TaxWithholdingAssessOfficePMService": { myResult = new TaxWithholdingAssessOfficePMService(); break; }
            case "InterestReportPMService": { myResult = new InterestReportPMService(); break; }
            case "InterestReportLinePMService": { myResult = new InterestReportLinePMService(); break; }

            case "AccountingCompanyTypePMService": { myResult = new AccountingCompanyTypePMService(); break; }
            case "WithholdingTaxDeductionTypePMService": { myResult = new WithholdingTaxDeductionTypePMService(); break; }
            case "ExternalReconciliationPMService": { myResult = new ExternalReconciliationPMService(); break; }
            case "TaxReportPMService": { myResult = new TaxReportPMService(); break; }
            case "TaxReportLineTransmitStatusListService": { myResult = new TaxReportLineTransmitStatusListService(); break; }
            case "GLAccountMoreDataPMService": { myResult = new GLAccountMoreDataPMService(); break; }
            case "InterestReportStatuseListService": { myResult = new InterestReportStatuseListService(); break; }

            case "TaxReportStatusListService": { myResult = new TaxReportStatusListService(); break; }
            case "TaxReportLineTypeListService": { myResult = new TaxReportLineTypeListService(); break; }
            case "TaxReportLineStatusListService": { myResult = new TaxReportLineStatusListService(); break; }
            case "VatReportStatusListService": { myResult = new VatReportStatusListService(); break; }
            case "GLAccountMoreDataListService": { myResult = new GLAccountMoreDataListService(); break; }
            // case "OpenFormatDateTypeListService": { myResult = new OpenFormatDateTypeListService(); break; }
            case "TaxDeductionReportStatusListService": { myResult = new TaxDeductionReportStatusListService(); break; }
            case "OpenFormatReportStatusListService": { myResult = new OpenFormatReportStatusListService(); break; }
            case "ExternalPageAdditionalDataListService": { myResult = new ExternalPageAdditionalDataListService(); break; }
            case "InterestBasesPeriodListService": { myResult = new InterestBasesPeriodListService(); break; }
            case "InterestBasesTypeListService": { myResult = new InterestBasesTypeListService(); break; }
            case "InterestBasesPeriodPMService": { myResult = new InterestBasesPeriodPMService(); break; }
            case "InterestBasesTypePMService": { myResult = new InterestBasesTypePMService(); break; }

            //#endregion

            //Extend Services
            case "AccountingPeriodExtendedListService": { myResult = new AccountingPeriodExtendedListService(); break; }
            case "AccountingPeriodExtendedPMService": { myResult = new AccountingPeriodExtendedPMService(); break; }
            case "GLAccountExtendedListService": { myResult = new GLAccountExtendedListService(); break; }
            case "JournalExtendedListService": { myResult = new JournalExtendedListService(); break; }
            case "LedgerTransactionExtendedListService": { myResult = new LedgerTransactionExtendedListService(); break; }
            case "BankDepositExtendedListService": { myResult = new BankDepositExtendedListService(); break; }
            case "BankAccountExtendedListService": { myResult = new BankAccountExtendedListService(); break; }
            case "ReconciliationExtendedPMService": { myResult = new ReconciliationExtendedPMService(); break; }
            case "CashBookExtendedPMService": { myResult = new CashBookExtendedPMService(); break; }
            case "JournalOpService": { myResult = new JournalOpService(); break; }
            case "ReconcileExternalPageListService": { myResult = new ReconcileExternalPageListService(); break; }
            case "TaxWithholdingAssessOfficeListService": { myResult = new TaxWithholdingAssessOfficeListService(); break; }
            case "ExternalReconciliationExtendedPMService": { myResult = new ExternalReconciliationExtendedPMService(); break; }
            case "ReconcileExternalPageExtendedListService": { myResult = new ReconcileExternalPageExtendedListService(); break; }
            case "ExternalReconciliationExtendedListService": { myResult = new ExternalReconciliationExtendedListService(); break; }
            case "BankDepositExtendedPMService": { myResult = new BankDepositExtendedPMService(); break; }
            case "TaxReportExtendedPMService": { myResult = new TaxReportExtendedPMService(); break; }
            case "AccountingIntegrityCheckListService": { myResult = new AccountingIntegrityCheckListService; break; }
            case "AccountingIntegrityCheckPMService": { myResult = new AccountingIntegrityCheckPMService; break; }
            case "AccountingNoteExtendedListService": { myResult = new AccountingNoteExtendedListService; break; }
            case "IntegrityCheckStatusListService": { myResult = new IntegrityCheckStatusListService; break; }
            case "GLAccountOpService": { myResult = new AccountingOpService(); break; }

            //Menu Buttons
            case "JournalMenuButtonsHandler": { myResult = new JournalMenuButtonsHandler; break; }
            case "GLAccountMenuButtonsHandler": { myResult = new GLAccountMenuButtonsHandler; break; }
            case "CashBookMenuButtonsHandler": { myResult = new CashBookMenuButtonsHandler; break; }
            case "BankDepositMenuButtonsHandler": { myResult = new BankDepositMenuButtonsHandler; break; }
            case "InterestBasesTypeMenuButtonsHandler": { myResult = new InterestBasesTypeMenuButtonsHandler; break; }
            case "PaymentChequeMenuButtonsHandler": {
                myResult = new PaymentChequeMenuButtonsHandler; break
            }
            case "BankAccountMenuButtonsHandler": { myResult = new BankAccountMenuButtonsHandler; break; }
            case "ReconciliationMenuButtonsHandler": { myResult = new ReconciliationMenuButtonsHandler; break; }
            case "ExternalReconciliationMenuButtonsHandler": { myResult = new ExternalReconciliationMenuButtonsHandler; break; }
            case "TaxReportMenuButtonsHandler": { myResult = new TaxReportMenuButtonsHandler; break; }
            case "TaxDeductionReportMenuButtonsHandler": { myResult = new TaxDeductionReportMenuButtonsHandler; break; }
            case "OpenFormatReportMenuButtonsHandler": { myResult = new OpenFormatReportMenuButtonsHandler; break; }

        }

        return myResult;
    }
}
