import { GLAccountsPageComponent } from './Components/Workspaces/GLAccounts/GLAccountsPageComponent';
import { AccountingNoteComponent } from './Components/Others/AccountingNoteComponent';
import { IntegrityCheckTabComponent } from './Components/EditTabs/AccountingIntegrityCheck/IntegrityCheckTabComponent';
import {AccountingWorkspaceComponent} from './Components/Workspaces/AccountingWorkspaceComponent';
import {MainPageComponent} from './Components/Workspaces/Main/MainPageComponent';
import {JournalPageComponent} from './Components/Workspaces/Journal/JournalPageComponent';
import {ReceivablePageComponent} from './Components/Workspaces/Receivable/ReceivablePageComponent';
import {PayablePageComponent} from './Components/Workspaces/Payable/PayablePageComponent';
import {BanksPageComponent} from './Components/Workspaces/Banks/BanksPageComponent';
import { MiscPageComponent } from './Components/Workspaces/Misc/MiscPageComponent';
import { InterestPageComponent } from './Components/Workspaces/Interest/InterestPageComponent';
import { BatchInvoicesComponent } from './Components/Others/BatchInvoicesComponent';
import { BatchPrintComponent } from './Components/Others/BatchPrintComponent';
import { BtatchPrintWarningComponent } from './Components/Others/BtatchPrintWarningComponent';
import { BtatchPrintConfirmComponent } from './Components/Others/BtatchPrintConfirmComponent';
import { InvoiceDateForBatchInvoicesComponent } from './Components/Others/InvoiceDateForBatchInvoicesComponent';
import { InterestInvoiceAutoCreditComponent } from './Components/Others/InterestInvoiceAutoCreditComponent';

import {NewGLAccountComponent} from './Components/NewEntity/NewGLAccountComponent';
import {NewChartOfAccountComponent} from './Components/NewEntity/NewChartOfAccountComponent';
import {NewCashBookComponent} from './Components/NewEntity/NewCashBookComponent';
import {NewBankCodeComponent} from './Components/NewEntity/NewBankCodeComponent';
import {NewBankAccountComponent} from './Components/NewEntity/NewBankAccountComponent';
import {NewBankDepositComponent} from './Components/NewEntity/NewBankDepositComponent';

import {NewCategory1Component} from './Components/Maintenance/NewCategory1Component';
import {NewCategory2Component} from './Components/Maintenance/NewCategory2Component';
import {NewCategory3Component} from './Components/Maintenance/NewCategory3Component';
import {NewCategory4Component} from './Components/Maintenance/NewCategory4Component';
import {NewCategory5Component} from './Components/Maintenance/NewCategory5Component';
import {AutoRecoMethodComponent} from './Components/Maintenance/AutoRecoMethodComponent';
import {NewRevaluationComponent} from './Components/NewEntity/NewRevaluationComponent';
import {AddEditRecoExPageComponent} from './Components/NewEntity/AddEditRecoExPageComponent';
import {NewConnectedGLAccountComponent} from './Components/EditTabs/GLAccount/NewConnectedGLAccountComponent';
import {NewPaymentChequeComponent} from './Components/NewEntity/NewPaymentChequeComponent';
import {NewTaxWithholdingAssessingOfficeComponent} from './Components/Maintenance/NewTaxWithholdingAssessingOfficeComponent';
import { FullAccountingSettingsComponent } from './Components/Maintenance/FullAccountingSettingsComponent';
import { FullAccountingAddControlComponent } from './Components/Maintenance/FullAccountingAddControlComponent';

import {AccountingPeriodsComponent} from './Components/Maintenance/AccountingPeriodsComponent';
import {EditAccountingPeriodComponent} from './Components/Maintenance/EditAccountingPeriodComponent';
import {AccountingPeriodEventComponent} from './Components/Maintenance/AccountingPeriodEventComponent';
import {YearTransferComponent} from './Components/Maintenance/YearTransferComponent';
import { NewTaxReportComponent } from './Components/NewEntity/NewTaxReportComponent';
import { NewOpenFormatReportComponent } from './Components/NewEntity/NewOpenFormatReportComponent';
import { AccountingLoadTestComponent } from './Components/Maintenance/AccountingLoadTestComponent';
import { LoadRecoExPageComponent } from './Components/NewEntity/LoadRecoExPageComponent';
import { Generate1000Component } from './Components/Maintenance/Generate1000Component';
import { Receiving1000Component } from './Components/Maintenance/Receiving1000Component';
import { AccountingFunctionalTestComponent } from './Components/Maintenance/AccountingFunctionalTestComponent';
import { ManageExternalReconciliationTabComponent } from './Components/Others/ReconcileExternalPage/ManageExternalReconciliationTabComponent';
import { ExternalPagesTabComponent } from './Components/Others/ReconcileExternalPage/ExternalPagesTabComponent';

import {GLAccountGeneralTabComponent} from './Components/EditTabs/GLAccount/GLAccountGeneralTabComponent';
import {GLAccountTransactionsTabComponent} from './Components/EditTabs/GLAccount/GLAccountTransactionsTabComponent';
import {ManageReconciliationsTabComponent} from './Components/EditTabs/GLAccount/ManageReconciliationsTabComponent';
import {JournalDetailsTabComponent} from './Components/EditTabs/Journal/JournalDetailsTabComponent';
import {CashBookDetailsTabComponent} from './Components/EditTabs/CashBook/CashBookDetailsTabComponent';
import {CashBookManageDepoTabComponent} from './Components/EditTabs/CashBook/CashBookManageDepoTabComponent';
import {ChartOfAccountGeneralTabComponent} from './Components/EditTabs/ChartOfAccount/ChartOfAccountGeneralTabComponent';
import {BankDepositDetailsTabComponent} from './Components/EditTabs/BankDeposit/BankDepositDetailsTabComponent';
import {BankAccountGeneralTabComponent} from './Components/EditTabs/BankAccount/BankAccountGeneralTabComponent';
import {ReconciliationDetailsTabComponent} from './Components/EditTabs/Reconciliation/ReconciliationDetailsTabComponent';
import {RevaluationDetailsComponent} from './Components/EditTabs/Revaluation/RevaluationDetailsComponent';
import {GLAccountTaxWithholdingTabComponent} from './Components/EditTabs/GLAccount/GLAccountTaxWithholdingTabComponent';
import {BankCodeGeneralTabComponent} from './Components/EditTabs/BankCode/BankCodeGeneralTabComponent';
import {ExternalRecoDetailsTabComponent} from './Components/EditTabs/ExternalReconciliation/ExternalRecoDetailsTabComponent';
import { OpenFormatReportLogTabComponent } from './Components/EditTabs/OpenFormatReport/OpenFormatReportLogTabComponent';
import { TaxDeductionReportGeneralTabComponent } from './Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabComponent';
import { BankPageEventsComponent } from './Components/EditTabs/BankAccount/BankPageEventsComponent';

import {GlAccountLedgerTransactionsListTemplate} from './Components/ListTemplates/GlAccountLedgerTransactionsListTemplate';
import {ReconcileExternalPageListTemplate} from './Components/ListTemplates/ReconcileExternalPageListTemplate';
import {ReconcileExternalPageLineListTemplate} from './Components/ListTemplates/ReconcileExternalPageLineListTemplate';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {ManageReconciliationListTemplate} from './Components/ListTemplates/ManageReconciliationListTemplate';
import {TaxReportListTemplate} from './Components/ListTemplates/TaxReportListTemplate';
import { ReconciliationLineListTemplate } from './Components/ListTemplates/ReconciliationLineListTemplate';
import {InterestReportListTemplate} from './Components/ListTemplates/InterestReportListTemplate'
import {InterestInvoiceListTemplate} from './Components/ListTemplates/InterestInvoiceListTemplate'
import { InterestBasesTypeDetailsTabComponent } from './Components/EditTabs/Interest/DetailsTab/InterestBasesTypeDetailsTabComponent';
import {PrintedListHeaderTemplate} from './Components/ListTemplates/PrintedListHeaderTemplate'


import { CopyJournalComponent } from './Components/Others/CopyJournalComponent';
import {ReconcileComponent} from './Components/Others/ReconcileComponent';
import {ReconciledMessage} from './Components/Others/ReconciledMessage';
import {OutOfDepositMessage} from './Components/Others/OutOfDepositMessage';
import {Aging4CustomerChartWindowComponent} from './Components/Others/Aging4CustomerChartWindowComponent';
import {GLAccountAdditionalDataTabComponent} from './Components/EditTabs/GLAccount/GLAccountAdditionalDataTabComponent';
import {GLAccountSearchWindowComponent} from './Components/EditTabs/GLAccount/GLAccountSearchWindowComponent';
import {PaymentChequeGeneralTabComponent} from './Components/EditTabs/PaymentCheque/PaymentChequeGeneralTabComponent';
import {CancelChequeComponent} from './Components/Others/CancelChequeComponent';
import {DropdownButtonComponent} from './Components/Others/DropdownButtonComponent';
import {JournalReconcileComponent} from './Components/Others/JournalReconcileComponent';
import {ExternalReconcileComponent} from './Components/Others/ExternalReconcileComponent';
import {AddEditTaxWithholdingLineComponent} from './Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent';
import {TaxReportDetailsTabComponent} from './Components/EditTabs/TaxReport/TaxReportDetailsTabComponent';
import {AccountingFlatFileDownloadComponent} from './Components/Others/AccountingFlatFileDownloadComponent';
import { GLAccountOverviewComponent } from './Components/EditTabs/GLAccount/GLAccountOverviewComponent';
import { TaxDeductionReportLogTabComponent } from './Components/EditTabs/TaxDeductionReport/TaxDeductionReportLogTabComponent';
import { NewTaxDeductionReportComponent } from './Components/NewEntity/NewTaxDeductionReportComponent';
import { AddEditInterestBasesPeriodComponent } from './Components/EditTabs/Interest/DetailsTab/AddEditInterestBasesPeriod/AddEditInterestBasesPeriodComponent';
import { ExtReconcileAdjustBankFeeComponent } from './Components/Others/ExtReconcileAdjustBankFeeComponent';
import { CreateInterestReportsForCustomersComponent } from './Components/Others/CreateInterestReportsForCustomersComponent';
import { AddEditCalculatedChartsOfAccountComponent } from './Components/EditTabs/UserDefinedReport/AddEditCalculatedChartsOfAccount/AddEditCalculatedChartsOfAccountComponent';

// Short Titles
import {GLAccountShortTitleComponent} from './Components/ShortTitles/GLAccountShortTitleComponent';
import {BankAccountShortTitleComponent} from './Components/ShortTitles/BankAccountShortTitleComponent';
import {BankDepositShortTitleComponent} from './Components/ShortTitles/BankDepositShortTitleComponent';
import {PaymentChequeShortTitleComponent} from './Components/ShortTiTles/PaymentChequeShortTitleComponent';
import {ReconciliationShortTitleComponent} from './Components/ShortTiTles/ReconciliationShortTitleComponent';
import {ExternalReconciliationShortTitleComponent} from './Components/ShortTiTles/ExternalReconciliationShortTitleComponent';
import { TaxReportShortTitleComponent } from './Components/ShortTiTles/TaxReportShortTitleComponent';
import { EditTaxReportLineComponent } from './Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent';
import { NewIntegrityCheckComponent } from './Components/NewEntity/NewIntegrityCheckComponent';
import { CashBookLineListTemplate } from './Components/ListTemplates/CashBookLineListTemplate';
import { InterestBasesTypeShortTitleComponent } from './Components/ShortTiTles/InterestBasesTypeShortTitleComponent';
import { GLAccountInterestComponent } from './Components/EditTabs/GLAccount/GLAccountInterestComponent';
import { NewInterestReportComponent } from './Components/NewEntity/NewInterestReportComponent';
import { NewUserDefinedReportComponent } from './Components/NewEntity/NewUserDefinedReportComponent';

import { InterestReportGeneralTabComponent } from './Components/EditTabs/InterestReport/GeneralTab/InterestReportGeneralTabComponent';
import { InterestReportLineByDateDetailsComponent } from './Components/EditTabs/InterestReport/GeneralTab/InterestReportLineByDateDetails/InterestReportLineByDateDetailsComponent';
import { InterestReportLinesByDateListTemplate } from './Components/ListTemplates/InterestReportLinesByDateListTemplate';
import { InterestReportShortTitleComponent } from './Components/ShortTiTles/InterestReportShortTitleComponent';
import { UserDefinedReportShortTitleComponent } from './Components/ShortTiTles/UserDefinedReportShortTitleComponent';
import { ConnectWithGLAccountComponent } from './Components/EditTabs/GLAccount/ConnectWithGLAccountComponent';
import { InterestReportEditOpenBalanceComponent } from './Components/EditTabs/InterestReport/GeneralTab/InterestReportEditOpenBalance/InterestReportEditOpenBalanceComponent';

import { CargoTrackingServiceComponent } from './Components/Others/CargoTrackingService/CargoTrackingServiceComponent';
import { CargoTrackingBuildShipmentComponent } from './Components/Others/CargoTrackingService/CargoTrackingBuildShipmentComponent';
import { CargoTrackingIncrementalStatistics } from './Components/Others/CargoTrackingService/CargoTrackingIncrementalStatistics';
import { CargoTrackingIncrementalStatListTemplate } from './Components/ListTemplates/CargoTrackingIncrementalStatListTemplate';

import { InterestReportEditCalculationDateComponent } from './Components/EditTabs/InterestReport/GeneralTab/InterestReportEditCalculationDate/InterestReportEditCalculationDateComponent';
import { UserDefinedReportGeneralTabComponent } from './Components/EditTabs/UserDefinedReport/UserDefinedReportGeneralTabComponent';
import { AccountingMainTesterComponent } from './Components/Maintenance/Tester/AccountingMainTesterComponent';

import { UpdateJournalLineComponent } from './Components/EditTabs/Journal/UpdateJournalLineComponent';

//import { UpdateJournalLineNoteComponent } from './Components/EditTabs/Journal/UpdateJournalLineNoteComponent';
import { JournalCSVLoadComponent } from './Components/NewEntity/JournalCSVLoadComponent';



//import { CashBookGeneralTabComponent } from './Components/EditTabs/CashBook/CashBookGeneralTabComponent';




export const Components =
    [
        //Workspaces
        AccountingWorkspaceComponent,
        MainPageComponent,
        GLAccountsPageComponent,
        JournalPageComponent,
        ReceivablePageComponent,
        PayablePageComponent,
        BanksPageComponent,
        MiscPageComponent,
        InterestPageComponent,
        BatchInvoicesComponent,
        BatchPrintComponent, 
        //New Entites
        NewGLAccountComponent,
        NewChartOfAccountComponent,
        NewCashBookComponent,
        NewBankCodeComponent,
        NewBankAccountComponent,
        NewBankDepositComponent,
        AutoRecoMethodComponent,
        NewRevaluationComponent,
        AddEditRecoExPageComponent,
        NewConnectedGLAccountComponent,
        ConnectWithGLAccountComponent,
        NewPaymentChequeComponent,
        NewOpenFormatReportComponent,
        LoadRecoExPageComponent,
        NewUserDefinedReportComponent,
        JournalCSVLoadComponent,
        
        //Maintenance
        NewCategory1Component,
        NewCategory2Component,
        NewCategory3Component,
        NewCategory4Component,
        NewCategory5Component,
        FullAccountingSettingsComponent,
        FullAccountingAddControlComponent,
        AccountingPeriodsComponent,
        EditAccountingPeriodComponent,
        AccountingPeriodEventComponent,
        NewTaxWithholdingAssessingOfficeComponent,
        YearTransferComponent,
        AccountingLoadTestComponent,
        AccountingMainTesterComponent,
        Generate1000Component,
        Receiving1000Component,
        AccountingFunctionalTestComponent,

        //Edit Tabs
        GLAccountGeneralTabComponent,
        //CashBookGeneralTabComponent,
        GLAccountOverviewComponent,
        GLAccountTransactionsTabComponent,
        ManageReconciliationsTabComponent,
        JournalDetailsTabComponent,
        UpdateJournalLineComponent,
        CashBookDetailsTabComponent,
        CashBookManageDepoTabComponent,
        ChartOfAccountGeneralTabComponent,
        BankDepositDetailsTabComponent,
        BankAccountGeneralTabComponent,
        ReconciliationDetailsTabComponent,
        RevaluationDetailsComponent,
        ExternalRecoDetailsTabComponent,
        GLAccountAdditionalDataTabComponent,
        PaymentChequeGeneralTabComponent,
        TaxReportDetailsTabComponent,
        GLAccountTaxWithholdingTabComponent,
        BankCodeGeneralTabComponent,
        EditTaxReportLineComponent,
        TaxDeductionReportLogTabComponent,
        OpenFormatReportLogTabComponent,
        TaxDeductionReportGeneralTabComponent,
        BankPageEventsComponent,
        InterestBasesTypeDetailsTabComponent,
        ManageExternalReconciliationTabComponent,
        ExternalPagesTabComponent,
        GLAccountInterestComponent,
        InterestInvoiceAutoCreditComponent,
        UserDefinedReportGeneralTabComponent,
        //Templates
        GlAccountLedgerTransactionsListTemplate,
        ReconcileExternalPageListTemplate,
        ReconcileExternalPageLineListTemplate,
        FieldTemplateComponent,
        ManageReconciliationListTemplate,
        TaxReportListTemplate,
        ReconciliationLineListTemplate,
        CashBookLineListTemplate,
        InterestReportListTemplate,
        InterestInvoiceListTemplate,
        PrintedListHeaderTemplate,
        //Others
        CopyJournalComponent,
        ReconcileComponent,
        ReconciledMessage,
        OutOfDepositMessage,
        Aging4CustomerChartWindowComponent,
        PaymentChequeShortTitleComponent,
        ReconciliationShortTitleComponent,
        ExternalReconciliationShortTitleComponent,
        TaxReportShortTitleComponent,
        GLAccountShortTitleComponent,
        BankAccountShortTitleComponent,
        BankDepositShortTitleComponent,
        GLAccountSearchWindowComponent,
        CancelChequeComponent,
        DropdownButtonComponent,
        JournalReconcileComponent,
        ExternalReconcileComponent,
        AddEditTaxWithholdingLineComponent,
        AddEditCalculatedChartsOfAccountComponent,
        NewTaxReportComponent,
        AccountingFlatFileDownloadComponent,
        NewTaxDeductionReportComponent,
        IntegrityCheckTabComponent,
        AccountingNoteComponent,
        NewIntegrityCheckComponent,
        AddEditInterestBasesPeriodComponent,
        InterestBasesTypeShortTitleComponent,
        ExtReconcileAdjustBankFeeComponent,
        NewInterestReportComponent,
        CargoTrackingServiceComponent,
        InterestReportGeneralTabComponent,
        InterestReportLineByDateDetailsComponent,
        InterestReportLinesByDateListTemplate,
        InterestReportShortTitleComponent,
        InterestReportEditOpenBalanceComponent,
        CargoTrackingBuildShipmentComponent,
        CargoTrackingIncrementalStatistics,
        CreateInterestReportsForCustomersComponent,



        CargoTrackingIncrementalStatListTemplate,

     
        InterestReportEditCalculationDateComponent,

      

        InterestReportEditOpenBalanceComponent,
        BtatchPrintWarningComponent,
        BtatchPrintConfirmComponent,
        InvoiceDateForBatchInvoicesComponent,
        UserDefinedReportShortTitleComponent


    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {

            //Workspaces
            case "AccountingWorkspaceComponent": { myResult = AccountingWorkspaceComponent; break; }
            case "MainPageComponent": { myResult = MainPageComponent; break; }
            case "GLAccountsPageComponent": { myResult = GLAccountsPageComponent; break; }
            case "JournalPageComponent": { myResult = JournalPageComponent; break; }
            case "ReceivablePageComponent": { myResult = ReceivablePageComponent; break; }
            case "PayablePageComponent": { myResult = PayablePageComponent; break; }
            case "BanksPageComponent": { myResult = BanksPageComponent; break; }
            case "MiscPageComponent": { myResult = MiscPageComponent; break; }
            case "InterestPageComponent":{ myResult = InterestPageComponent; break;}
            //New Entites
            case "NewGLAccountComponent": { myResult = NewGLAccountComponent; break; }
            case "NewChartOfAccountComponent": { myResult = NewChartOfAccountComponent; break; }
            case "NewCashBookComponent": { myResult = NewCashBookComponent; break; }
            case "NewBankCodeComponent": { myResult = NewBankCodeComponent; break; }
            case "NewBankAccountComponent": { myResult = NewBankAccountComponent; break; }
            case "NewBankDepositComponent": { myResult = NewBankDepositComponent; break; }
            case "AutoRecoMethodComponent": { myResult = AutoRecoMethodComponent; break; }
            case "NewRevaluationComponent": { myResult = NewRevaluationComponent; break; }
            case "AddEditRecoExPageComponent": { myResult = AddEditRecoExPageComponent; break; }
            case "ConnectWithGLAccountComponent": { myResult = ConnectWithGLAccountComponent; break; }
            case "NewConnectedGLAccountComponent": { myResult = NewConnectedGLAccountComponent; break; }
            case "NewPaymentChequeComponent": { myResult = NewPaymentChequeComponent; break; }
            case "NewTaxReportComponent": { myResult = NewTaxReportComponent; break; }
            case "NewTaxDeductionReportComponent": { myResult = NewTaxDeductionReportComponent; break; }
            case "NewOpenFormatReportComponent": { myResult = NewOpenFormatReportComponent; break; }
            case "LoadRecoExPageComponent": { myResult = LoadRecoExPageComponent; break; }
            case "JournalCSVLoadComponent": { myResult = JournalCSVLoadComponent; break; }
            case "NewInterestReportComponent": { myResult = NewInterestReportComponent; break; }
            case "NewUserDefinedReportComponent": { myResult = NewUserDefinedReportComponent; break; }

            case "CreateInterestReportsForCustomersComponent": { myResult = CreateInterestReportsForCustomersComponent; break; }

            //Maintenance
            case "NewCategory1Component": { myResult = NewCategory1Component; break; }
            case "NewCategory2Component": { myResult = NewCategory2Component; break; }
            case "NewCategory3Component": { myResult = NewCategory3Component; break; }
            case "NewCategory4Component": { myResult = NewCategory4Component; break; }
            case "NewCategory5Component": { myResult = NewCategory5Component; break; }
            case "FullAccountingSettingsComponent": { myResult = FullAccountingSettingsComponent; break; }
            case "FullAccountingAddControlComponent": { myResult = FullAccountingAddControlComponent; break; }
            case "YearTransferComponent": { myResult = YearTransferComponent; break; }
            case "AccountingPeriodsComponent": { myResult = AccountingPeriodsComponent; break; }
            case "EditAccountingPeriodComponent": { myResult = EditAccountingPeriodComponent; break; }
            case "AccountingPeriodEventComponent": { myResult = AccountingPeriodEventComponent; break; }
            case "NewTaxWithholdingAssessingOfficeComponent": { myResult = NewTaxWithholdingAssessingOfficeComponent; break; }
            case "AccountingLoadTestComponent": { myResult = AccountingLoadTestComponent; break; }
            case "AccountingMainTesterComponent": { myResult = AccountingMainTesterComponent; break; }
            case "Generate1000Component": { myResult = Generate1000Component; break; }
            case "Receiving1000Component": { myResult = Receiving1000Component; break; }
            case "AccountingFunctionalTestComponent": { myResult = AccountingFunctionalTestComponent; break; }

            //Edit Tabs
            case "GLAccountGeneralTabComponent": { myResult = GLAccountGeneralTabComponent; break; }
            //case "CashBookGeneralTabComponent": { myResult = CashBookGeneralTabComponent; break; }
            case "GLAccountOverviewComponent": { myResult = GLAccountOverviewComponent; break; }
            case "GLAccountTransactionsTabComponent": { myResult = GLAccountTransactionsTabComponent; break; }
            case "ManageReconciliationsTabComponent": { myResult = ManageReconciliationsTabComponent; break; }
            case "JournalDetailsTabComponent": { myResult = JournalDetailsTabComponent; break; }
            case "UpdateJournalLineComponent": { myResult = UpdateJournalLineComponent; break; }
            case "CashBookDetailsTabComponent": { myResult = CashBookDetailsTabComponent; break; }
            case "CashBookManageDepoTabComponent": { myResult = CashBookManageDepoTabComponent; break; }
            case "ChartOfAccountGeneralTabComponent": { myResult = ChartOfAccountGeneralTabComponent; break; }
            case "BankDepositDetailsTabComponent": { myResult = BankDepositDetailsTabComponent; break; }
            case "BankAccountGeneralTabComponent": { myResult = BankAccountGeneralTabComponent; break; }
            case "ReconciliationDetailsTabComponent": { myResult = ReconciliationDetailsTabComponent; break; }
            case "RevaluationDetailsComponent": { myResult = RevaluationDetailsComponent; break; }
            case "ExternalRecoDetailsTabComponent": { myResult = ExternalRecoDetailsTabComponent; break; }
            case "GLAccountAdditionalDataTabComponent": { myResult = GLAccountAdditionalDataTabComponent; break; }
            case "PaymentChequeGeneralTabComponent": { myResult = PaymentChequeGeneralTabComponent; break; }
            case "TaxReportDetailsTabComponent": { myResult = TaxReportDetailsTabComponent; break; }
            case "GLAccountTaxWithholdingTabComponent": { myResult = GLAccountTaxWithholdingTabComponent; break; }
            case "BankCodeGeneralTabComponent": { myResult = BankCodeGeneralTabComponent; break; }
            case "EditTaxReportLineComponent": { myResult = EditTaxReportLineComponent; break; }
            case "TaxDeductionReportLogTabComponent": { myResult = TaxDeductionReportLogTabComponent; break; }
            case "OpenFormatReportLogTabComponent": { myResult = OpenFormatReportLogTabComponent; break; }
            case "TaxDeductionReportGeneralTabComponent": { myResult = TaxDeductionReportGeneralTabComponent; break; }
            case "BankPageEventsComponent": { myResult = BankPageEventsComponent; break; }
            case "InterestBasesTypeDetailsTabComponent": { myResult = InterestBasesTypeDetailsTabComponent; break;}
            case "ExternalPagesTabComponent": { myResult = ExternalPagesTabComponent; break; }
            case "GLAccountInterestComponent": { myResult = GLAccountInterestComponent; break; }
            case "InterestReportGeneralTabComponent": { myResult = InterestReportGeneralTabComponent; break; }
            case "UserDefinedReportGeneralTabComponent": { myResult = UserDefinedReportGeneralTabComponent; break; }

             //Templates
            case "GlAccountLedgerTransactionsListTemplate": { myResult = GlAccountLedgerTransactionsListTemplate; break; }
            case "ReconcileExternalPageListTemplate": { myResult = ReconcileExternalPageListTemplate; break; }
            case "ReconcileExternalPageLineListTemplate": { myResult = ReconcileExternalPageLineListTemplate; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "ManageReconciliationListTemplate": { myResult = ManageReconciliationListTemplate; break; }
            case "TaxReportListTemplate": { myResult = TaxReportListTemplate; break; }
            case "ReconciliationLineListTemplate": { myResult = ReconciliationLineListTemplate; break; }
            case "CashBookLineListTemplate": { myResult = CashBookLineListTemplate; break; }
            case "InterestReportLinesByDateListTemplate": { myResult = InterestReportLinesByDateListTemplate; break; }
            case "InterestReportListTemplate": { myResult = InterestReportListTemplate; break;}
            case "CargoTrackingIncrementalStatListTemplate": { myResult = CargoTrackingIncrementalStatListTemplate; break;}
            case "InterestInvoiceListTemplate" :{myResult =InterestInvoiceListTemplate; break;}
            case "PrintedListHeaderTemplate" :{myResult =PrintedListHeaderTemplate; break;}


            //Others
            case "CopyJournalComponent": { myResult = CopyJournalComponent; break;}
            case "UserDefinedReportShortTitleComponent" :{myResult =UserDefinedReportShortTitleComponent; break;}
            case "CargoTrackingBuildShipmentComponent" :{myResult =CargoTrackingBuildShipmentComponent; break;}
            case "CargoTrackingIncrementalStatistics" :{myResult =CargoTrackingIncrementalStatistics; break;}
            case "CargoTrackingServiceComponent": { myResult = CargoTrackingServiceComponent; break; }
            case "BtatchPrintConfirmComponent": { myResult = BtatchPrintConfirmComponent; break; }
            case "ReconcileComponent": { myResult = ReconcileComponent; break; }
            case "ReconciledMessage": { myResult = ReconciledMessage; break; }
            case "OutOfDepositMessage": { myResult = OutOfDepositMessage; break; }
            case "Aging4CustomerChartWindowComponent": { myResult = Aging4CustomerChartWindowComponent; break; }
            case "PaymentChequeShortTitleComponent": { myResult = PaymentChequeShortTitleComponent; break; }
            case "GLAccountShortTitleComponent": { myResult = GLAccountShortTitleComponent; break; }
            case "BankAccountShortTitleComponent": { myResult = BankAccountShortTitleComponent; break; }
            case "BankDepositShortTitleComponent": { myResult = BankDepositShortTitleComponent; break; }
            case "ReconciliationShortTitleComponent": { myResult = ReconciliationShortTitleComponent; break; }
            case "ExternalReconciliationShortTitleComponent": { myResult = ExternalReconciliationShortTitleComponent; break; }
            case "TaxReportShortTitleComponent": { myResult = TaxReportShortTitleComponent; break; }
            case "InterestReportLineByDateDetailsComponent": { myResult = InterestReportLineByDateDetailsComponent; break; }
            case "InterestReportEditOpenBalanceComponent": { myResult = InterestReportEditOpenBalanceComponent; break; }
            case "InterestReportEditCalculationDateComponent": { myResult = InterestReportEditCalculationDateComponent; break; }
            case "AddEditCalculatedChartsOfAccountComponent": { myResult = AddEditCalculatedChartsOfAccountComponent; break; }
            case "GLAccountSearchWindowComponent": {
                myResult = GLAccountSearchWindowComponent; break;
            }
            case "CancelChequeComponent": { myResult = CancelChequeComponent; break; }
            case "DropdownButtonComponent": { myResult = DropdownButtonComponent; break; }
            case "JournalReconcileComponent": { myResult = JournalReconcileComponent; break; }
            case "ExternalReconcileComponent": { myResult = ExternalReconcileComponent; break; }
            case "AddEditTaxWithholdingLineComponent": { myResult = AddEditTaxWithholdingLineComponent; break; }
            case "AccountingFlatFileDownloadComponent": { myResult = AccountingFlatFileDownloadComponent; break; }
            case "IntegrityCheckTabComponent": { myResult = IntegrityCheckTabComponent; break; }
            case "AccountingNoteComponent": { myResult = AccountingNoteComponent; break; }
            case "NewIntegrityCheckComponent": { myResult = NewIntegrityCheckComponent; break; }
            case "AddEditInterestBasesPeriodComponent": { myResult = AddEditInterestBasesPeriodComponent; break; }
            case "InterestBasesTypeShortTitleComponent": { myResult = InterestBasesTypeShortTitleComponent; break; }
            case "ExtReconcileAdjustBankFeeComponent": { myResult = ExtReconcileAdjustBankFeeComponent; break; }
            case "ManageExternalReconciliationTabComponent": { myResult = ManageExternalReconciliationTabComponent; break;}
            case "InterestReportShortTitleComponent": { myResult = InterestReportShortTitleComponent; break;}
            case "BatchInvoicesComponent" :{myResult =BatchInvoicesComponent; break;}
            case "BatchPrintComponent" :{myResult =BatchPrintComponent; break;}
            case "InvoiceDateForBatchInvoicesComponent" :{myResult =InvoiceDateForBatchInvoicesComponent; break;}
            case "BtatchPrintWarningComponent": { myResult = BtatchPrintWarningComponent; break; }
            case "InterestInvoiceAutoCreditComponent": { myResult = InterestInvoiceAutoCreditComponent; break; }
            

        }

        return myResult;
    }
}

