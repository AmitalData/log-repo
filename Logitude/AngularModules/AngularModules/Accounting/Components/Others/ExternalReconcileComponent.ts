declare var window: any;
import { IdGeneratorPipe } from './../../../Controls/Pipes/IdGeneratorPipe';
import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ReconcileEventManager } from '../../Utilities/ReconcileEventManager';
import { FullAccountingSettingPM } from '../../EntityPMs/FullAccountingSettingPM';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';


//Entities
import { ExternalReconciliationPM } from '../../EntityPMs/ExternalReconciliationPM';
import { ExternalReconciliationLinePM } from '../../EntityPMs/ExternalReconciliationLinePM';
import { ExternalReconciliationList } from '../../EntityLists/ExternalReconciliationList';
import { LedgerTransactionPM } from '../../EntityPMs/LedgerTransactionPM';
import { LedgerTransactionList } from '../../EntityLists/LedgerTransactionList';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { BankAccountPM } from '../../EntityPMs/BankAccountPM';
import { ReconcileExternalPagePM } from '../../EntityPMs/ReconcileExternalPagePM';
import { ReconcileExternalPageLinePM } from '../../EntityPMs/ReconcileExternalPageLinePM';


//Services
import { ExternalReconciliationPMService } from '../../Services/StandardPMs/ExternalReconciliationPMService';
import { ExternalReconciliationExtendedPMService } from '../../Services/ExtendedPMs/ExternalReconciliationExtendedPMService';
import { LedgerTransactionExtendedListService } from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ExternalReconciliationExtendedListService, ExternalAutoReconcileServiceArgs } from '../../Services/ExtendedLists/ExternalReconciliationExtendedListService';
import { retry } from 'rxjs/operators';
import { ExternalReconciliationOpService } from '../../Services/ExtendedPMs/ExternalReconciliationOpService';
import { BankTransferPaymentArguments } from 'Invoice/DataContracts/BankTransferPaymentArguments';
import { PageLineModel } from '../NewEntity/AddEditRecoExPageComponent';
import { LineModel } from './ReconcileComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { ReconcileExcelDataArgs } from 'Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';




const BankTransferPaymentMethodCode = "BT";
const NewARPaymentWindowWidth = 900;
const NewARPaymentWindowHeight = 570;
const CrossYearConfirmationDialogWidth = 390;
const exportToExcelWindowWidth = 500;
const exportToExcelWindowHeight = 200;
@Component({
    selector: 'ExternalReconcileComponent',
    moduleId: './Accounting/Components/Others/',
    providers: [EntityListService],
    templateUrl: 'ExternalReconcileComponent.html',
})

export class ExternalReconcileComponent extends BaseComponent implements OnInit, AfterViewInit {
    public DataContext: ExternalReconcileComponent = this;
    public ObjectTableName: string;
    public ExtRecoTable: string = "ExternalReconciliation";

    public NewARPaymentTitle = TextCodeTranslator.Translate("ExternalReconciliation.O.BTCreateARPayment");
    private BankTransferDifferenceMessage = TextCodeTranslator.Translate("ExternalReconciliation.O.BankTransferDifferenceMsg");
    private SelectCreditLinesOnlyMessage = TextCodeTranslator.Translate("ExternalReconciliation.O.BTCreditLinesOnly");
    private OnlyBankPagesMessage = TextCodeTranslator.Translate("ExternalReconciliation.O.BTOnlyBankPages");

    public FireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
    public PagePM: ReconcileExternalPagePM;
    public GLAccountPM: GLAccountPM;
    public BankAccountPM: BankAccountPM;
    public ExternalRecoPM: ExternalReconciliationPM;
    public ValidationErrorsList: string[] = [];
    TransactionSelectedLines: ObservableCollection;
    ExtPageSelectedLines: ObservableCollection;
    text_SumOfXRowsSelected: string = TextCodeTranslator.Translate("ReconcileExternalPage.O.SumOfXRowsSelected");
    public isRTL: boolean = false;
    public OperatorsList: any[] = [];
    public DateFilterList: any[] = [];
    IsEntityValid: boolean = true;
    txt_FiltersSelected: string = "";
    LoadGrids: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    ExternalPagesTitle: string;
    CreatedReconciliationsCount: number = 0;
    SessionEvent;
    entityListService: EntityListService = new EntityListService();
    ledgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    _ExternalReconciliationExtendedListService: ExternalReconciliationExtendedListService = new ExternalReconciliationExtendedListService();
    externalReconciliationExtendedPMService: ExternalReconciliationExtendedPMService = new ExternalReconciliationExtendedPMService();
    externalReconciliationPMService: ExternalReconciliationPMService = new ExternalReconciliationPMService();
    _ExternalReconciliationOpService: ExternalReconciliationOpService = new ExternalReconciliationOpService();

    CreateBankTransferButtonFeatureEnabled = false;
    showBankTransferAlert = false;
    createdPaymentNumber;
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent= new LogitudeGridExportToExcelComponent();
    constructor(private CD: ChangeDetectorRef) {
        super();
        this.isRTL = SessionLocator.TenantPM.LayoutDirection === 'rtl';

        this.ExternalRecoPM = new ExternalReconciliationPM();
        this.ExternalRecoPM.Tenant = SessionLocator.Tenant;

        this.TransactionSelectedLines = new ObservableCollection([]);
        this.ExtPageSelectedLines = new ObservableCollection([]);

        var filters = new ApiQueryFilters();

        this.txt_FiltersSelected = TextCodeTranslator.Translate("Accounting.O.FiltersSelected");

        this.OperatorsList =
            [{ EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
            { EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
            { EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
            { EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
            { EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
            { EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
            ];
        this.DateFilterList =
            [
                { EnglishName: 'Last 7 days', LocalName: TextCodeTranslator.Translate("Accounting.O.Last7days") },
                { EnglishName: 'Last month', LocalName: TextCodeTranslator.Translate("Accounting.O.Lastmonth") },
                { EnglishName: 'Last 3 months', LocalName: TextCodeTranslator.Translate("Accounting.O.Last3months") },
                { EnglishName: 'Last year', LocalName: TextCodeTranslator.Translate("Accounting.O.Lastyear") },
                { EnglishName: 'Custom', LocalName: TextCodeTranslator.Translate("Accounting.O.Custom") },
            ];


        //#endregion

        this.GetDefaultValues();

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.BankAccountPM = args.BankAccountPM;
            this.EntityPM = args.EntityPM;
            this.ObjectTableName = args.ObjectTableName;
            this.openAmountCurrency = args.openAmountCurrency;
            this.CurrentSession.TransferAccountId = this.BankAccountPM?.TransferGLAcccountId;

            this.SetTitles();
            this.ResetFilters();
        }
    }
    OpenLedgerTransactionInternalNote(line: any) {

        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("LedgerTransaction.F.InternalNote");
        logWindow.WindowArgs = { ledgerTransaction: line };
        logWindow.Show('./Accounting/Components/Others/LedgerTransactionInternalNotesComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.detectChanges();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    SetTitles() {
        switch (this.ObjectTableName) {
            case 'GLAccount': {
                this.ExternalPagesTitle = TextCodeTranslator.Translate('GLAccount.TH.ExternalTransactions');

                break;
            }
            case 'BankAccount': {
                this.ExternalPagesTitle = TextCodeTranslator.Translate('ReconcileExternalPage.O.BankAccountTransactions');

                break;
            }

            default: {
                this.ExternalPagesTitle = TextCodeTranslator.Translate('ReconcileExternalPage.O.BankAccountTransactions');

                break;
            }
        }
    }

    ngOnInit() {
        this.TransactionBuildColumns();
        this.ExtPageBuildColumns();
        this.ReloadScreen();
        this.ExtPageReloadScreen();
        this.GetFeatures();
        this.Listen();

    }
    ngAfterViewInit() {
        var t = setTimeout(() => {
            this.LoadGrids = true;
        }, 100);
    }
    private GetFeatures() {
        this.CreateBankTransferButtonFeatureEnabled = !!FeatureLocator.IsFeatureGrantedByCode("ExtRecoCreateBankTransferPY");
    }
    Listen() {
        this.isAllSelected = false;
        this.isAllSelectedExt = false;

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s && s.Name == "BankTransferARPaymentCreated") {
                this.createdPaymentNumber = s.PaymentNumber;
                this.showBankTransferAlert = true;
            }
        });
    }

    GetBankTransferAlertMessage() {
        return TextCodeTranslator.Translate('ExternalReconciliation.O.BTbackgroundCreationMSG').replace('#number', this.createdPaymentNumber);
    }

    //#region Properties

    public get EnableCreateBankTransferPaymentButton(): boolean {
        return this.CreateBankTransferButtonFeatureEnabled
            && this.ExtPageSelectedLines.Length > 0
            && this.ObjectTableName != 'GLAccount';
    }

    //#endregion

    //#region Search Fields
    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);

                this.ReloadScreen();
                this.ExtPageReloadScreen();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.ReloadScreen();
            this.ExtPageReloadScreen();
        }
    }
    OpenAmountTextChanged(num) {
        if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.SelectedOperator)) {

            this.timerToken = setTimeout(() => {
                if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.SelectedOperator)) {

                    var OpenAmountFilterOperator = this.SelectedOperator.EnglishName.replace(/ /g, ''); // remove white spaces
                    if (OpenAmountFilterOperator == "Equals") {
                        this.openAmountFilter = new FilterItem("ForeignAmount", num, -1 * num, null, OpenAmountFilterOperator, false, false, false, "number", false);
                    }
                    else if (OpenAmountFilterOperator == "LessThan") {
                        num = Math.abs(num);
                        this.openAmountFilter = new FilterItem("ForeignAmount", -1 * --num, +num, null, "Between", false, false, false, "number", false);
                    }
                    else if (OpenAmountFilterOperator == "LessThanOrEqual") {
                        num = Math.abs(num);
                        this.openAmountFilter = new FilterItem("ForeignAmount", -1 * num, +num, null, "Between", false, false, false, "number", false);
                    }
                    else {
                        this.openAmountFilter = new FilterItem("ForeignAmount", Math.abs(num), null, null, OpenAmountFilterOperator, false, false, false, "number", false);
                    }
                    this.ReloadScreen();
                    this.ExtPageReloadScreen();
                } else {
                    this.openAmountFilter = null;

                    this.ReloadScreen();
                    this.ExtPageReloadScreen();
                }
            }, 700);

        } else {
            this.timerToken = setTimeout(() => {

                this.openAmountFilter = null;

                this.ReloadScreen();
                this.ExtPageReloadScreen();
            }, 700);
        }
    }
    //#endregion

    //#region Buttons Handlers
    ReconcileButtonClicked() {
        if (Math.abs(this.totalDifference) > 0.001)
            this.MakeAdjustment();
        else
            this.MakeReconciliation();
    }

    private MakeAdjustment() {
        const hasCrossYearLines = this.CheckIfHasCrossYearLines();

        if (hasCrossYearLines)
            return this.ShowCrossYearConfirmationDialogForAdjust();


        this.AdjustReconcile();

    }

    private CheckAdjustLedgerTransactionsOnly() {
        if (this.ExtPageSelectedLines.Length == 0 && this.TransactionSelectedLines.Length > 0) {
            this.ValidationErrorsList = [TextCodeTranslator.Translate("ExternalReconciliation.O.CantAdjustLedgersOnly")];
        }
    }

    private MakeReconciliation() {
        var errors: string[] = this.ValidateReconciliation();



        if (errors.length == 0) {
            var reconciliation = this.CreateReconciliation();

            const hasCrossYearLines = this.CheckIfHasCrossYearLines();

            if (hasCrossYearLines) {
                this.ShowCrossYearConfirmationDialog(reconciliation);
            } else {
                this.SubmitChanges(reconciliation);
            }

        }
        else
            this.ValidationErrorsList = errors;
    }

    private ShowCrossYearConfirmationDialog(reconciliation: ExternalReconciliationPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = CrossYearConfirmationDialogWidth;
        confirmWindow.Show(TextCodeTranslator.Translate("ExternalReconciliation.O.CrossYearConfirmMsg"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes)
                this.SubmitChanges(reconciliation);
        });
    }

    private ShowCrossYearConfirmationDialogForAdjust() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = CrossYearConfirmationDialogWidth;
        confirmWindow.Show(TextCodeTranslator.Translate("ExternalReconciliation.O.CrossYearConfirmMsg"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.AdjustReconcile();
            }
        });

    }

    private AdjustReconcile() {
        if (this.ExtPageSelectedLines.Length >= 1 && this.TransactionSelectedLines.Length >= 0) {
            this.AdjustBankFeeWithNewJournalScreen();
        }
        this.CheckAdjustLedgerTransactionsOnly();
    }

    private CheckIfHasCrossYearLines() {
        const ledgerYears = this.GetLedgerTransactionsYearsCount();
        const pageLinesYears = this.GetExternalPagesLinesYearsCount();

        const bothLinesSelected = this.ExtPageSelectedLines.Length > 0 && this.TransactionSelectedLines.Length > 0;
        const ledgerHasDifferentYears = ledgerYears > 1;
        const pageLinesHasDifferentYears = pageLinesYears > 1;
        const hasSingleDifferentYears = this.CheckIfHasSingleDifferentYears(ledgerYears, pageLinesYears);

        return ledgerHasDifferentYears
            || (bothLinesSelected && pageLinesHasDifferentYears)
            || (hasSingleDifferentYears);
    }

    private CheckIfHasSingleDifferentYears(ledgerYears: number, pageLinesYears: number) {
        let transactionsGroupedByYears = this.GetTransactionsYears();
        let pageLinesGroupedByYears = this.GetExternalPageLinesYears();

        const hasSingleDifferentYears =
            ledgerYears == 1 && pageLinesYears == 1
            && Object.keys(transactionsGroupedByYears)[0] != Object.keys(pageLinesGroupedByYears)[0];

        return hasSingleDifferentYears;
    }

    private GetLedgerTransactionsYearsCount() {
        let linesGroupedByYears = this.GetTransactionsYears();

        return this.countObjectKeys(linesGroupedByYears);
    }

    private GetTransactionsYears() {
        return this.TransactionSelectedLines.Collection
            .filter(e => e.LedgerTransactionPM.AccountId != this.BankAccountPM.TransferGLAcccountId)
            .reduce((result, current: TransactionLineModel) => {

                const year = new Date(current.LedgerTransactionPM.AccountingDate).getFullYear();
                result[year] = (result[year] || 0) + 1;
                return result;
            }, Object.create(null));
    }

    private countObjectKeys(object: any) {
        var groupCount = 0;
        for (const key in object)
            groupCount++;
        return groupCount;
    }

    private GetExternalPagesLinesYearsCount() {
        let pageLinesGroupedByYears = this.GetExternalPageLinesYears();

        return this.countObjectKeys(pageLinesGroupedByYears);
    }

    private GetExternalPageLinesYears() {
        return this.ExtPageSelectedLines.Collection
            .reduce((result, current: PageLineModel) => {
                const year = new Date(current.ReferenceDate).getFullYear();
                result[year] = (result[year] || 0) + 1;
                return result;
            }, Object.create(null));
    }

    private ValidateReconciliation() {
        var errors: string[] = [];

        this.BlockAdjustLedgerTransactionOnly(errors);
        this.CheckEmptyReconcile(errors);
        return errors;
    }

    private CheckEmptyReconcile(errors: string[]) {
        if (this.ExtPageSelectedLines.Length == 0 && this.TransactionSelectedLines.Length == 0)
            errors.push(TextCodeTranslator.Translate("Accounting.O.SelectTwoTransactionAtLeast"));
    }

    private BlockAdjustLedgerTransactionOnly(errors: string[]) {
        if (Math.abs(this.totalDifference) > 0.001 && this.ExtPageSelectedLines.Length == 0 && this.TransactionSelectedLines.Length > 0) {
            errors.push(TextCodeTranslator.Translate("ExternalReconciliation.O.CantAdjustLedgersOnly"));
        }
    }
    GetFirstXLedgerForReconciliationByParam() {
        this.ValidationErrorsList = [];
        var filters = this.GetAPIFilters();

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
        this.ledgerTransactionExtendedListService.GetFirstXLedgerForReconciliationByParams(this.getGLAccountId(), filters).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            var first100Transactions = mm.Result;
            if (!mm.HasError) {
                if (!AppTool.IsNullOrEmpty(first100Transactions)) {
                    this.FuncTransactionSelectedLine(first100Transactions);
                    this.CalculateTotals();
                }
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    GetFirstXLedgerForExtPageReconciliationByParam() {
        this.ValidationErrorsList = [];
        var filters = this.GetAPIFiltersExt();
        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
        this.ledgerTransactionExtendedListService.GetFirstXLedgerForExtReconciliationByParam("ReconcileExternalPage", objectTable.Id, this.EntityPM.Id, filters).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            var first100Transactions = mm.Result;
            if (!mm.HasError) {
                if (!AppTool.IsNullOrEmpty(first100Transactions)) {
                    this.FuncExtPageSelectedLines(first100Transactions);
                    this.CalculateExtPageTotals();
                }
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    public GetAPIFilters() {
        var filters = new ApiQueryFilters;
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }

        filters.PageSize = 2000;
        filters.PageIndex = 1;
        filters.GetAll = true;
        filters.GetCount = true;


        filters.addAdditionalFilter("IsExternalReconcile", false, null, null, "Equals", false, false, false, "Boolean");
        // filters.addAdditionalFilter("DueDate", "#today", null, null, "LessThan", false, false, false, "Date"); // value will be override in server, to avoid edging problem!
        
        if (this.ObjectTableName == "BankAccount" && this.filterSelectedValue != 'filter_Bank')
            filters.addAdditionalFilter("DUMMY_TransferAccountId", this.BankAccountPM.TransferGLAcccountId, null, null, "Equals", false, false, false, "String");

        if (!this.showInProgessLines) {
            filters.addAdditionalFilter("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean");
            filters.addAdditionalFilter("InProgressExternalReconcile", false, null, null, "Equals", false, false, false, "boolean");

        }
        filters.SortBy = this.TransactionDataSource.sortingCol;
        filters.SortDirection = this.TransactionDataSource.sortingDir;
        return filters;
    }


    public GetAPIFiltersExt() {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            var refDateFilter = new FilterItem("ReferenceDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            filters.AdditionalFilters.push(refDateFilter);
        }
         if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            var amountFilter = new FilterItem("Amount2Filter", this.openAmountFilter.FieldValue, this.openAmountFilter.FieldValue2, null, this.openAmountFilter.Operator, true, false, true, "number", false);
            filters.AdditionalFilters.push(amountFilter);
        }
        filters.PageSize = 2000;
        filters.PageIndex = 1;
        filters.GetAll = true;
        filters.GetCount = true;
        filters.SortBy = this.ExtPageDataSource.sortingCol;
        filters.SortDirection = this.ExtPageDataSource.sortingDir;
       
        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (!this.showInProgessLines) {
            filters.addAdditionalFilter("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean");
            filters.addAdditionalFilter("InProgressExternalReconcile", false, null, null, "Equals", false, false, false, "boolean");
        }
        return filters;
    }
    public ChangeCheckBoxesState: EventEmitter<any> = new EventEmitter();
    public ChangeCheckBoxesStateExt: EventEmitter<any> = new EventEmitter();

    public FuncTransactionSelectedLine(result: any) {
        this.TransactionSelectedLines.Clear();
        let emittedArray = result.map((res: ReconcileExternalPageLinePM) => ({ rowData: res, IsChecked: true, RowIndex: -1, ById: true })); //result.map(res=>(new LineModel(res,this,-1)));//[];
        let selectedLines = result.map(res => (new TransactionLineModel(res, this, -1)));
        this.TransactionSelectedLines.InsertCollection(selectedLines);
        this.ChangeCheckBoxesState.emit(emittedArray);
    }
    public FuncExtPageSelectedLines(result: any) {
        this.ExtPageSelectedLines.Clear();
        let emittedArray = result.map((res: ReconcileExternalPageLinePM) => ({ rowData: res, IsChecked: true, RowIndex: -1, ById: true })); //result.map(res=>(new LineModel(res,this,-1)));//[];
        let selectedLines = result.map(res => (new ExtPageLineModel(res, this, -1)));
        this.ExtPageSelectedLines.InsertCollection(selectedLines);
        this.ChangeCheckBoxesStateExt.emit(emittedArray);
    }

    SaveAsDraftButton() {

        //if (this.SelectedLines.Length > 0) {

        //    // 1- prepare transactions
        //    var transactionsList = [];
        //    this.SelectedLines.Collection.forEach((lineModel: LineModel) => {
        //        var transaction = lineModel.LedgerTransactionPM;
        //        //transaction.Mark = !transaction.Mark; // the service will take this misson

        //        transactionsList.push(transaction);
        //    });

        //    // 2- call the service
        //    this.CurrentSession.StartBusyIndicatorSaving();
        //    this._ReconciliationExtendedPMService.delsertDraftLedgerTransaction(transactionsList).subscribe((serviceResponse: ServiceResponse) => {
        //        console.log("_ReconciliationExtendedPMService.delsertDraftLedgerTransaction", serviceResponse);
        //        this.CurrentSession.StopBusyIndicator();

        //        var result = serviceResponse.Result;

        //        var msg = new MessageWindow();
        //        msg.ShowSuccessIcon = true;
        //        msg.Width = 400;
        //        msg.Show(TextCodeTranslator.Translate("Reconciliations.Q.reconciliationwassavedas"));
        //        msg.WindowClosed.subscribe((event: any) => {
        //            this.CancelButtonClicked();
        //        });

        //    });

        //} else {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected")); //"No transactions selected!"
        //}
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ExternalReco");
    }

    RefreshButtonClicked() {
        this.ReloadScreen();
        this.ExtPageReloadScreen();
        if (this.IsAutoReconcile) {
            this.AutoReco();
        }
    }
    AdjustBankFeeWithNewJournalScreen(): void {
        //throw new Error("Method not implemented.");
        if (this.ExtPageSelectedLines.Length < 1) {
            console.error("(this.ExtPageSelectedLines.Length != 1)")
            this.ValidationErrorsList.push("to adjust bank fees, select one or more row External page line ");
            return;
        }
        if (this.ExtPageSelectedLines.Length > 1 && this.TransactionSelectedLines.Length > 0) {
            this.ValidationErrorsList.push("to adjust bank fees with Transaction select only one page line  ");
            return;
        }
        let LedgerTransactionIdList: string[] = [];
        this.TransactionSelectedLines.Collection.forEach(r => {
            let myTransactionLineModel: TransactionLineModel = r;
            LedgerTransactionIdList.push(myTransactionLineModel.LedgerTransactionPM.Id)

        });
        let myExtPageLineModel: ExtPageLineModel = this.ExtPageSelectedLines.Collection[0];
        let ReconcileExternalPageLinePMList: ReconcileExternalPageLinePM[] = [];
        this.ExtPageSelectedLines.Collection.forEach(r /*: ExtPageLineModel*/ => {
            let a: ReconcileExternalPageLinePM = r.PageLinePM;
            ReconcileExternalPageLinePMList.push(a);
        });



        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 390;
        confirmWindow.Show(TextCodeTranslator.Translate("Accounting.O.NewReconcileWithAdjusment"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.entityResourceService.getEntityResourceByTableName("Journal").subscribe(response => {
                    this.CurrentSession.entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(response => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 400;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.B.Adjust");

                        //logitudeWindow.WindowArgs = { "ExtPageSelectedLine": myExtPageLineModel.PageLinePM, "LedgerTransactionIdList": LedgerTransactionIdList, "BankAccountPMId": this.BankAccountPM.Id };
                        logitudeWindow.WindowArgs = {
                            "ReconcileExternalPageLinePMList": ReconcileExternalPageLinePMList,
                            "LedgerTransactionIdList": LedgerTransactionIdList,
                            "BankAccountPMId": this.BankAccountPM.Id,
                            TotalDifference: this.totalDifference,
                            TotalDifferenceCurrency: this.openAmountCurrency,
                            OrignalDifference: this.orignalDifference
                        };

                        logitudeWindow.Show('./Accounting/Components/Others/ExtReconcileAdjustBankFeeComponent');
                        logitudeWindow.WindowClosed
                            .subscribe(($event: any) => {
                                this.ExtPageSelectedLines.Clear();
                                this.TransactionSelectedLines.Clear();
                                this.RefreshButtonClicked();
                            });
                    });
                });

            }
        });




    }
    //#endregion

    //#region [A] Transactions Data Source

    public TransactionFireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
    public TransactionColumnsReady: EventEmitter<any> = new EventEmitter();
    public TransactionMarkIsChecked: EventEmitter<any> = new EventEmitter();

    @Output() TransactionMenuHeaderchangeevent = new EventEmitter();
    @Output() TransactiononQueryChangeEvent = new EventEmitter();
    dateFilter: FilterItem;
    currencyFilter: FilterItem;
    searchFieldFilter: FilterItem;
    openAmountFilter: FilterItem;
    public TransactionsQueryColumns: QueryColumnPM[] = [];
    public TransactionsColumns: any[] = null;
    TransactionBuildColumns() {
        this.TransactionsColumns = [];
        this.TransactionsColumns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '60px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.TransactionsColumns.push({
        //    FieldName: 'GroupHash',
        //    DataTypeCode: 'String',
        //    Display: '#',
        //    Styles: { width: '30px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'AccountingDate'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccountingDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate")));

        //this.TransactionsColumns.push({
        //    FieldName: 'DocumentDate',
        //    DataTypeCode: 'DateTime',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"), //'Ref. Date',
        //    Styles: { width: '100px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        //this.TransactionsColumns.push({
        //    FieldName: 'DueDate',
        //    DataTypeCode: 'DateTime',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"), // 'Due Date',
        //    Styles: { width: '100px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Source"), // 'Source',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'Source'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Source",'String',TextCodeTranslator.Translate("LedgerTransaction.F.Source")));

        //this.TransactionsColumns.push({ // Check ReconcileMethodCode.GLAccounts:
        //    FieldName: 'OriginalAmount',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("Accounting.General.O.OriginalAmount") + ' (' + this.originalAmountCurrency + ')',
        //    Styles: { width: '150px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true,
        //,
        // ServerSideSortable: true,
        // SortByName: 'Source'
        // });
        //this.TransactionsColumns.push({
        //    FieldName: 'OpenAmount',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + ' (' + this.openAmountCurrency + ')',
        //    Styles: { width: '120px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //,
        //     ServerSideSortable: true,
        //     SortByName: 'Source'
        // });
        this.TransactionsColumns.push({
            FieldName: 'ForeignAmount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmount") + ' (' + this.openAmountCurrency + ')',
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'ForeignAmount'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ForeignAmount", 'String', TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmount") + ' (' + this.openAmountCurrency + ')'));

        this.TransactionsColumns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"), // 'Ref. 1',
            Styles: { width: '80px' },
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'Reference1'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference1",'String',TextCodeTranslator.Translate("LedgerTransaction.F.Reference1")));

        this.TransactionsColumns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"), // 'Ref. 2',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Reference2'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference2",'String',TextCodeTranslator.Translate("LedgerTransaction.F.Reference2")));

        this.TransactionsColumns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"), // 'Ref. 3',
            Styles: { width: '80px' },
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'Reference3'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference3",'String',TextCodeTranslator.Translate("LedgerTransaction.F.Reference3")));

        //this.TransactionsColumns.push({
        //    FieldName: 'JournalNumber',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"), // 'Journal No.',
        //    Styles: { width: '80px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //,
        //     ServerSideSortable: true,
        //     SortByName: 'Source'
        // });

        this.TransactionsColumns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Notes"), // 'Notes',
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'Notes'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Notes",'String',TextCodeTranslator.Translate("LedgerTransaction.F.Notes")));

        this.TransactionsColumns.push({
            FieldName: 'InternalNote',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.InternalNote"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsInternalNotesTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsInternalNotesTemplate',
            IsCustomTemplate: true
            ,
            ServerSideSortable: true,
            SortByName: 'InternalNote'
        });
        this.TransactionsQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InternalNote",'String',TextCodeTranslator.Translate("LedgerTransaction.F.InternalNote")));

        ReconcileEventManager.CheckBoxChecked.subscribe(($event) => {
            if (!AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;
                var oneTime = $event.oneTime;
                console.log("---->> Row Selected: ", rowId, row, isChecked);

                if (isChecked) {
                    this.PushLine(row, RowIndex);
                } else {
                    this.PopLine(rowId);
                    this.TransactionFireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });

                }


            }
        });
    }

    onRowSelected($event) {
        if ($event) {
            const row = $event.rowData;
            const rowId = row.Id;
            const RowIndex = $event.rowIndex;
            const index = this.TransactionSelectedLines.Collection.findIndex(c => c.Id == row.Id);
            if (index < 0) {

                this.PushLine(row, RowIndex);
                this.TransactionFireCheckBoxChecked.emit({ rowData: row, IsChecked: true, RowIndex: RowIndex });
            } else {
                this.PopLine(rowId);
                this.TransactionFireCheckBoxChecked.emit({ rowData: row, IsChecked: false, RowIndex: RowIndex, ById: true });
            }
        }
    }

    onExternalPageRowSelected($event) {
        if ($event) {
            const row = $event.rowData;
            const rowId = row.Id;
            const RowIndex = $event.rowIndex;
            const index = this.ExtPageSelectedLines.Collection.findIndex(c => c.Id == row.Id);
            if (index < 0) {
                this.ExtPagePushLine(row, RowIndex);
                this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: true, RowIndex: RowIndex });
            } else {
                this.ExtPagePopLine(rowId);
                this.ExtPageFireCheckBoxChecked.emit({ rowData: row, IsChecked: false, RowIndex: RowIndex, ById: true });
            }
        }
    }



    TransactiononDataLoaded() {

        //this.TransactionsCheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue: true, IsAutoRecClicked: this.IsAutoRecClicked});
        this.TransactionMarkIsChecked.emit({ SelectedLines: this.TransactionSelectedLines });

    }
    TransactionDataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "DocumentDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    transCount: number;
    onCountReadyTrans(count) {
        this.transCount = count;
    }
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        //else {
        //    return;
        //}
        //if (this.currencyFilter) {
        //    filters.AdditionalFilters.push(this.currencyFilter);
        //}
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }

        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.PageSize = take;
        filters.PageIndex = skip + 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;


        filters.addAdditionalFilter("IsExternalReconcile", false, null, null, "Equals", false, false, false, "Boolean");
        // filters.addAdditionalFilter("SourceTypeCode", "5,9", null, null, "InList", false, false, false, "String");
        filters.addAdditionalFilter("DueDate", "#today", null, null, "LessThan", false, false, false, "Date"); // value will be override in server, to avoid edging problem!
        
        if (this.ObjectTableName == "BankAccount" && this.filterSelectedValue != 'filter_Bank')
            filters.addAdditionalFilter("DUMMY_TransferAccountId", this.BankAccountPM.TransferGLAcccountId, null, null, "Equals", false, false, false, "String");

        if (!this.showInProgessLines) {
            filters.addAdditionalFilter("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean");
            filters.addAdditionalFilter("InProgressExternalReconcile", false, null, null, "Equals", false, false, false, "boolean");

        }
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        const glaccountId = this.filterSelectedValue == 'filter_Transfer' ? null : this.getGLAccountId();

        return this.entityListService.getReconciliationsByFilter("LedgerTransaction", glaccountId, filters);
    }

    private getGLAccountId() {
        var glaccountId;
        if (this.ObjectTableName == "GLAccount")
            glaccountId = this.EntityPM.Id;
        else if (this.ObjectTableName == "BankAccount")
            glaccountId = this.BankAccountPM.GLAccountId;
        return glaccountId;
    }
    private GetGLAccountCurrency() {
        var currency;
        if (this.ObjectTableName == "GLAccount")
            currency = this.EntityPM.CurrencyId;
        else if (this.ObjectTableName == "BankAccount")
            currency = this.BankAccountPM.GLAccountCurrencyId;
        return currency;
    }
    OnSortInvoked(event) {
        // this.TransactionSelectedLines = new ObservableCollection([]);
        // this.ExtPageSelectedLines = new ObservableCollection([]);
    }

    // selectedTransferTransactionsCount: number = 0;


    public get selectedTransferTransactionsCount(): number {
        var count = 0;
        if (this.TransactionSelectedLines.Length > 0) {

            var transferTransactions = this.TransactionSelectedLines.Collection
                .filter((d: TransactionLineModel) => d.LedgerTransactionPM.AccountId == this.BankAccountPM.TransferGLAcccountId);

            if (transferTransactions)
                count = transferTransactions.length;
        }
        return count;
    }


    PushLine(row, RowIndex) {
        
        var index = this.TransactionSelectedLines.Collection.findIndex(c => c.Id == row.Id);
        if (index < 0) { // DNE


            var ledger = new TransactionLineModel(row, this, RowIndex);

            var sameBankAccountWithTransfer = this.BankAccountPM ? (this.BankAccountPM.GLAccountId == this.BankAccountPM.TransferGLAcccountId) : false;

            var isTransferTransaction = ledger.LedgerTransactionPM.AccountId == this.BankAccountPM.TransferGLAcccountId;
            // if (isTransferTransaction && !sameBankAccountWithTransfer &&  this.selectedTransferTransactionsCount >= 1 && this.ExtPageSelectedLines.Length > 1)
            // {
            //     this.ValidationErrorsList = [TextCodeTranslator.Translate("ExternalReconciliation.O.OnlyOneTransferTransactionCanReconciledWithOnePageLine")];
            //     // this.ValidationErrorsList = [TextCodeTranslator.Translate("ExternalReconciliation.O.CantReconcileTwoTransfer")];
            //     this.TransactionFireCheckBoxChecked.emit({ rowData: ledger.LedgerTransactionPM, IsChecked: false, RowIndex: RowIndex, ById: true });
            // }
            // else if (isTransferTransaction && !sameBankAccountWithTransfer && this.selectedTransferTransactionsCount == 0 && this.ExtPageSelectedLines.Length > 1)
            // {
            //     // this.ValidationErrorsList = ["When transfer transaction selected, only one line should be marked on the external page with the deferred check amount."];
            //     this.ValidationErrorsList = [TextCodeTranslator.Translate("ExternalReconciliation.O.OnlyOneTransferTransactionCanReconciledWithOnePageLine")];
            //     this.TransactionFireCheckBoxChecked.emit({ rowData: ledger.LedgerTransactionPM, IsChecked: false, RowIndex: RowIndex, ById: true });
            // }
            // else
            {
                this.TransactionSelectedLines.Insert(ledger);
                this.CalculateTotals();
                this.TransactionFireCheckBoxChecked.emit({ rowData: row, IsChecked: true, RowIndex: RowIndex });
                this.ValidationErrorsList = [];
            }

        }
    }

    PopLine(id, specialCase = false) {
        //Automatic reconcile
        if (this.IsAutoReconcile) {
            // 1- find id of opposit line
            var transactionRow = this.TransactionSelectedLines.Collection.find(d => d.Id == id);
            var transactionMatchedRows = this.TransactionSelectedLines.Collection.filter(d => d.GroupHash == transactionRow.GroupHash) || [];
            var oppositLines = this.ExtPageSelectedLines.Collection.filter(d => d.GroupHash == transactionRow.GroupHash) || [];

            // 2- popline
            if (oppositLines.length == 1 && transactionMatchedRows.length == 1)
                if (!specialCase) this.ExtPagePopLine(oppositLines[0].Id, true);
        }
        //

        this.TransactionSelectedLines.Remove(this.TransactionSelectedLines.Collection.find(c => c.Id == id));
        this.CalculateTotals();


    }



    CheckBoxValueChanged(Row) {
        this.PopLine(Row.LedgerTransactionPM.Id);
        this.TransactionFireCheckBoxChecked.emit({ rowData: Row.LedgerTransactionPM, IsChecked: false, RowIndex: Row.RowIndex, ById: true });
    }

    CalculateTotals() {
        
        this.accountTransactionsTotal = 0;
        var total = 0;
        for (let line of this.TransactionSelectedLines.Collection) {
            //total += +line.OpenAmount;
            // total += +line.ForeignAmount;

            if (line.IsCredit)
                total -= +line.ForeignAmount;
            else
                total += +line.ForeignAmount;

        }
        this.accountTransactionsTotal = total;
        var def = (this.extPageTransactionsTotal + this.accountTransactionsTotal)
        this.totalDifference = def < 0 ? def * -1 : def;
        this.orignalDifference = def;
    }

    ReloadScreen() {
        this.TransactiononQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        // this.TransactionSelectedLines.Clear();
        this.CalculateTotals();
    }
    ExtPageReloadScreen() {
        this.ExtPageonQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        /// this.ExtPageSelectedLines.Clear();
        this.CalculateExtPageTotals();

    }
    //#endregion

    //#region [B] external pages Data Source

    public ExtPageFireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
    public ExtPageColumnsReady: EventEmitter<any> = new EventEmitter();
    public ExtPageMarkIsChecked: EventEmitter<any> = new EventEmitter();

    @Output() ExtPageMenuHeaderchangeevent = new EventEmitter();
    @Output() ExtPageonQueryChangeEvent = new EventEmitter();
    ExtPage_dateFilter: FilterItem;
    ExtPage_currencyFilter: FilterItem;
    ExtPage_searchFieldFilter: FilterItem;
    ExtPage_openAmountFilter: FilterItem;

    public ExtPageColumns: any[] = null;
    ExportTransactions() {
        this.ValidationErrorsList = [];
        if (this.TransactionSelectedLines.Length == 0)
            this.ValidationErrorsList = [TextCodeTranslator.Translate("Reconciliation.O.NoLinesSelected")];
        else {
            let lines = this.TransactionSelectedLines.Collection.map((d: LineModel) => d.LedgerTransactionPM);
            this.ShowExportToExcelWindow(this.TransactionsQueryColumns, TextCodeTranslator.Translate('ReconcileExternalPage.O.Transactions'), lines, "DraftReconciliation");
        }
    }
    ExportExternalTransactions() {
        this.ValidationErrorsList = [];
        if (this.ExtPageSelectedLines.Length == 0)
            this.ValidationErrorsList = [TextCodeTranslator.Translate("Reconciliation.O.NoLinesSelected")];
        else {
            let lines = this.ExtPageSelectedLines.Collection.map((d: ExtPageLineModel) => d.PageLinePM);
            this.ShowExportToExcelWindow(this.ExtPageQueryColumns, this.ExternalPagesTitle, lines, "DraftReconciliationExt");
        }
    }
    private ShowExportToExcelWindow(queryColumns, title, lines, queryType) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = exportToExcelWindowWidth;
        logitudeWindow.Height = exportToExcelWindowHeight;
        logitudeWindow.WindowArgs = this.GetExportToExcelWindowArgs(queryColumns, title, lines, queryType);
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
    }
    private GetExportToExcelWindowArgs(queryColumns, title, lines, queryType) {
        var args: ReconcileExcelDataArgs = new ReconcileExcelDataArgs();
        args.QueryColumns = queryColumns;
        args.Tenant = SessionLocator.Tenant;
        args.Data = lines;
        args.Title = title;
        var windowArgs: any = {};
        windowArgs.ReconcileExcelDataArgs = args;
        windowArgs.tenant = SessionLocator.Tenant;
        windowArgs.ObjectTableName = this.ObjectTableName;
        windowArgs.QueryName = this.ObjectTableName;
        windowArgs.QueryType = queryType;
        return windowArgs;
    }

    public ExtPageQueryColumns: QueryColumnPM[] = [];
    ExtPageBuildColumns() {
        this.ExtPageColumns = [];
        this.ExtPageColumns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });

        //this.BankColumns.push({
        //    FieldName: 'GroupHash',
        //    DataTypeCode: 'String',
        //    Display: '#',
        //    Styles: { width: '30px' },
        //    HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
        //    IsCustomTemplate: true
        //});
        this.ExtPageColumns.push({
            FieldName: 'ReferenceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ReconcileExternalPageLine.F.ReferenceDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ReferenceDate'
        });
        this.ExtPageQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ReferenceDate",'DateTime',TextCodeTranslator.Translate("ReconcileExternalPageLine.F.ReferenceDate")));

        this.ExtPageColumns.push({
            FieldName: 'Amount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount") + ' (' + this.openAmountCurrency + ')',
            Styles: { width: '120px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Amount'
        });
        this.ExtPageQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Amount",'String',TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount")));

        this.ExtPageColumns.push({
            FieldName: 'Reference',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Reference"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Reference'
        });
        this.ExtPageQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference",'String',TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Reference")));

        this.ExtPageColumns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Notes"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Notes'
        });
        this.ExtPageQueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Notes",'String',TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Notes")));

        ReconcileEventManager.ExtPageCheckBoxChecked.subscribe(($event) => {
            if (!AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;
                var oneTime = $event.oneTime;
                console.log("---->> ExtPage Row Selected: ", rowId, row, isChecked);

                if (isChecked) {
                    this.ExtPagePushLine(row, RowIndex);
                } else {
                    this.ExtPagePopLine(rowId);
                    this.ExtPageFireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });
                }


            }
        });

    }
    ExtPageonDataLoaded() {

        //this.CheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue: true, IsAutoRecClicked: this.IsAutoRecClicked});
        this.ExtPageMarkIsChecked.emit({ SelectedLines: this.ExtPageSelectedLines });

    }
    ExtPageDataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "ReferenceDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getExtPageRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    extPageCount: number;
    onCountReadyExtPage(count) {
        this.extPageCount = count;
    }

    getExtPageRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            var refDateFilter = new FilterItem("ReferenceDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            filters.AdditionalFilters.push(refDateFilter);
        }
        //else {
        //    return;
        //}
        //if (this.currencyFilter) {
        //    filters.AdditionalFilters.push(this.currencyFilter);
        //}
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            var amountFilter = new FilterItem("Amount2Filter", this.openAmountFilter.FieldValue, this.openAmountFilter.FieldValue2, null, this.openAmountFilter.Operator, true, false, true, "number", false);
            filters.AdditionalFilters.push(amountFilter);
        }


        filters.PageSize = take;
        filters.PageIndex = skip + 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;


        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (!this.showInProgessLines) {
            filters.addAdditionalFilter("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean");
            filters.addAdditionalFilter("InProgressExternalReconcile", false, null, null, "Equals", false, false, false, "boolean");
        }

        return this.entityListService.getExternalReoncilioationsByFilter("ReconcileExternalPage", objectTable.Id, this.EntityPM.Id, filters);
    }

    ExtPagePushLine(row, RowIndex) {
        var index = this.ExtPageSelectedLines.Collection.findIndex(c => c.Id == row.Id);
        if (index < 0) { // DNE
            row.AmountToReconcile = row.CreditAmount != 0 ? row.CreditAmount : row.DebitAmount;

            var r = new ExtPageLineModel(row, this, RowIndex);

            var sameBankAccountWithTransfer = this.BankAccountPM ? (this.BankAccountPM.GLAccountId == this.BankAccountPM.TransferGLAcccountId) : false;


            //     if (this.selectedTransferTransactionsCount >= 1 && this.ExtPageSelectedLines.Length >= 1 && !sameBankAccountWithTransfer) {
            //     this.ValidationErrorsList = [TextCodeTranslator.Translate("ExternalReconciliation.O.OnlyOneTransferTransactionCanReconciledWithOnePageLine")];
            //     // this.ValidationErrorsList = ["When transfer transaction selected, only one line should be marked on the external page with the deferred check amount"];
            //     this.ExtPageFireCheckBoxChecked.emit({ rowData: r.PageLinePM, IsChecked: false, RowIndex: Number(RowIndex), ById: true });
            // }
            // else
            {
                this.ExtPageSelectedLines.Insert(r);
                this.CalculateExtPageTotals();
                this.ExtPageFireCheckBoxChecked.emit({ rowData: row, IsChecked: true, RowIndex: RowIndex });
                this.ValidationErrorsList = [];
            }

        }
    }

    ExtPagePopLine(id, specialCase = false) {

        //Automatic reconcile
        if (this.IsAutoReconcile) {
            // 1- find id of opposit line
            var pageLineRow = this.ExtPageSelectedLines.Collection.find(d => d.Id == id);
            var pageLineMatchedRows = this.ExtPageSelectedLines.Collection.filter(d => d.GroupHash == pageLineRow.GroupHash) || [];
            var oppositLines = this.TransactionSelectedLines.Collection.filter(d => d.GroupHash == pageLineRow.GroupHash) || [];


            // 2- popline
            if (oppositLines.length == 1 && pageLineMatchedRows.length == 1)
                if (!specialCase) this.PopLine(oppositLines[0].Id, true);
        }
        //

        this.ExtPageSelectedLines.Remove(this.ExtPageSelectedLines.Collection.find(c => c.Id == id));
        this.CalculateExtPageTotals();
    }

    ExtPageCheckBoxValueChanged(Row) {
        this.ExtPagePopLine(Row.PageLinePM.Id);
        this.ExtPageFireCheckBoxChecked.emit({ rowData: Row.PageLinePM, IsChecked: false, RowIndex: Row.RowIndex });
    }


    //#endregion

    //#region Totals Work
    accountTransactionsTotal: number = 0;
    extPageTransactionsTotal: number = 0;
    totalDifference: number = 0;
    orignalDifference: number = 0;
    CalculateExtPageTotals() {
        this.extPageTransactionsTotal = 0;
        var total = 0;
        for (let line of this.ExtPageSelectedLines.Collection) {
            if (line.IsCredit)
                total -= +line.Amount;
            else
                total += +line.Amount;

        }
        this.extPageTransactionsTotal = total;
        var def = (this.extPageTransactionsTotal + this.accountTransactionsTotal)
        this.totalDifference = def < 0 ? def * -1 : def;
        this.orignalDifference = def;
    }
    //#endregion

    //#region Automatic Reconcile + Filters
    isFiltersSelected: boolean = false;
    isFiltersVisible: boolean = false;
    IsAutoReconcile: boolean = false;

    AmountCheckBoxChecked: boolean = false;
    ReferenceCheckBoxChecked: boolean = false;
    ReferenceDateCheckBoxChecked: boolean = false;


    private showInProgessLines: boolean = false;
    public get ShowInProgessLines(): boolean {
        return this.showInProgessLines;
    }
    public set ShowInProgessLines(v: boolean) {
        this.showInProgessLines = v;

        this.RefreshButtonClicked();

    }

    public _isAllSelected: boolean;
    public get isAllSelected(): boolean {
        return this._isAllSelected;
    }
    public set isAllSelected(v: boolean) {
        this._isAllSelected = v;

        if (v) {

            this.GetFirstXLedgerForReconciliationByParam();
        }
        else {

            this.ReloadScreen();
            this.TransactionSelectedLines.Clear();
            this.CalculateTotals();
        }
    }
    public _isAllSelectedExt: boolean;
    public get isAllSelectedExt(): boolean {
        return this._isAllSelectedExt;
    }
    public set isAllSelectedExt(v: boolean) {
        this._isAllSelectedExt = v;

        if (v) {

            this.GetFirstXLedgerForExtPageReconciliationByParam();
        }
        else {

            this.ExtPageReloadScreen();
            this.ExtPageSelectedLines.Clear();
            this.CalculateExtPageTotals();
        }
    }
    AutoReco() {

        this.filterSelectedValue = 'filter_All';

        this.TransactionSelectedLines.Clear();
        this.ExtPageSelectedLines.Clear();

        console.log("[AUTO RECO] ", this.AmountCheckBoxChecked, this.ReferenceCheckBoxChecked, this.ReferenceDateCheckBoxChecked);

        this.IsAutoReconcile = true;

        //this.IsAutoRecClicked = true;
        //if (this.SelectedLines.Length > 0) {
        //    // Show prompt
        //    var confirmWindow = new ConfirmWindow();
        //    confirmWindow.Width = 390;
        //    confirmWindow.Show("Automatic Reconcile will clear all selected lines, continue?"); // "קיימות תנועות שנבחרו , הםם להמשיך בהתםמה םוטומטית ?"

        //    confirmWindow.WindowClosed.subscribe((event: any) => {
        //        if (confirmWindow.Yes) {
        //            this.SelectedLines.Clear();// = [];

        //            this.RunAutomaticReconcile();

        //        } else if (confirmWindow.No) {

        //        }
        //    });
        //} else {
        //    this.RunAutomaticReconcile();
        //}

        this.RunAutomaticReconcile();

    }

    RunAutomaticReconcile() {

        // Reload screen
        this.ReloadScreen();
        this.ExtPageReloadScreen();

        var t = setTimeout(() => {

            this.ValidationErrorsList = [];

            //#region filters
            var filters = new ApiQueryFilters;
            if (this.dateFilter) {
                filters.AdditionalFilters.push(this.dateFilter);
            }
            if (this.searchFieldFilter) {
                filters.AdditionalFilters.push(this.searchFieldFilter);
            }
            if (this.openAmountFilter) {
                filters.AdditionalFilters.push(this.openAmountFilter);
            }

            filters.PageSize = 30;
            filters.PageIndex = 0;
            filters.GetAll = true;
            filters.GetCount = true;


            filters.addAdditionalFilter("IsExternalReconcile", false, null, null, "Equals", false, false, false, "Boolean");
            // filters.addAdditionalFilter("SourceTypeCode", "5,9", null, null, "InList", false, false, false, "String");
            filters.addAdditionalFilter("DueDate", "#today", null, null, "LessThan", false, false, false, "Date"); // value will be override in server, to avoid edging problem!
            
            if (this.ObjectTableName == "BankAccount")
                filters.addAdditionalFilter("DUMMY_TransferAccountId", this.BankAccountPM.TransferGLAcccountId, null, null, "Equals", false, false, false, "String");

            if (!this.showInProgessLines) {
                filters.addAdditionalFilter("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean");
                filters.addAdditionalFilter("InProgressExternalReconcile", false, null, null, "Equals", false, false, false, "boolean");

            }

            //#endregion

            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."



            var serviceArgs = this.CreateExternalAutoReconcileServiceArgs(filters);

            this._ExternalReconciliationExtendedListService.getExternalAutomaticReconcilationsByFilter(serviceArgs)
                .subscribe((myResult: ServiceResponse) => {

                    var mm: ServiceResponse = myResult;
                    var result = mm.Result;
                    if (!mm.HasError) {

                        if (!AppTool.IsNullOrEmpty(result)) {

                            if (result) {
                                if (result.Count > 0) {

                                    //fill result

                                    //...ledger lines
                                    var lineModelList = [];
                                    var transactionLines = result.transactionLines;
                                    transactionLines.forEach(line => {
                                        var newLineModel = new TransactionLineModel(line, this, -1);
                                        lineModelList.push(newLineModel);
                                    });
                                    this.TransactionSelectedLines.InsertCollection(lineModelList, true);
                                    this.transCount = transactionLines.length;
                                    //.


                                    //...ExtPage lines
                                    var extPagelineModelList = [];
                                    var pageLines = result.pageLines;
                                    pageLines.forEach(line => {
                                        line.AmountToReconcile = line.Amount;
                                        var newLineModel = new ExtPageLineModel(line, this, -1);
                                        extPagelineModelList.push(newLineModel);
                                    });
                                    this.ExtPageSelectedLines.InsertCollection(extPagelineModelList, true);
                                    this.extPageCount = pageLines.length;
                                    //.

                                    this.CalculateTotals();
                                    this.CalculateExtPageTotals();
                                }
                                else {
                                    this.ShowEmptyAutoReco();
                                }
                            }
                        }
                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                    this.CurrentSession.StopBusyIndicator();
                });

        }, 200);

    }

    private CreateExternalAutoReconcileServiceArgs(filters: ApiQueryFilters) {
        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        var serviceArgs = new ExternalAutoReconcileServiceArgs();
        serviceArgs.amountReconcile = this.AmountCheckBoxChecked;
        serviceArgs.referenceReconcile = this.ReferenceCheckBoxChecked;
        serviceArgs.refDateReconcile = this.ReferenceDateCheckBoxChecked;
        serviceArgs.objectTableId = objectTable.Id;
        serviceArgs.entityId = this.EntityPM.Id;

        if (this.ObjectTableName == "BankAccount")
            serviceArgs.glAccountId = this.EntityPM.GLAccountId;
        else if (this.ObjectTableName == "GLAccount")
            serviceArgs.glAccountId = this.EntityPM.Id;

        serviceArgs.filters = filters;
        return serviceArgs;
    }

    FilterButtonClicked() {
        this.isFiltersVisible = !this.isFiltersVisible;
    }

    ChangeDate() {

        if (this.SelectedDateOperator) {
            //var TommorowDate = DateTool.AddDays((new Date()), 1);
            var TommorowDate = new Date();
            TommorowDate.setHours(23, 59, 59, 59);
            var TodayDate = new Date();
            TodayDate.setUTCHours(0, 0, 0, 0);
            var YesterdayDate = DateTool.AddDays((new Date()), -1);
            YesterdayDate.setUTCHours(0, 0, 0, 0);
            var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThreeMonthDate = DateTool.AddDays((new Date()), -90);
            LastThreeMonthDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
            var LastYearFromDate = DateTool.AddDays((new Date()), -365);
            LastYearFromDate.setUTCHours(0, 0, 0, 0);
            var LastYearToDate = DateTool.AddDays((new Date()), 1);
            LastYearToDate.setUTCHours(0, 0, 0, 0);

            this.UIProperties.SetEnabled("FromDate", this.ExtRecoTable, false);
            this.UIProperties.SetEnabled("ToDate", this.ExtRecoTable, false);

            switch (this.SelectedDateOperator.EnglishName) {
                case "Today":
                    {
                        this.FromDate = TodayDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Yesterday":
                    {
                        this.FromDate = YesterdayDate;
                        this.ToDate = TodayDate;
                        break;
                    }
                case "Last 7 days":
                    {
                        this.FromDate = LastSevenDaysDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Last month":
                    {
                        this.FromDate = LastThirtyDaysDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Last 3 months":
                    {
                        this.FromDate = LastThreeMonthDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Current year":
                    {
                        this.FromDate = CurrentYearFromDate;
                        this.ToDate = CurrentYearToDate;
                        break;
                    }
                case "Last year":
                    {
                        this.FromDate = LastYearFromDate;
                        this.ToDate = LastYearToDate;
                        break;
                    }
                case "Custom":
                    {
                        this.FromDate = null;
                        this.ToDate = null;
                        this.UIProperties.SetEnabled("FromDate", this.ExtRecoTable, true);
                        this.UIProperties.SetEnabled("ToDate", this.ExtRecoTable, true);
                        this.CD.detectChanges();
                        break;
                    }
            }
        } else {
            this.FromDate = null;
            this.ToDate = null;
        }
        this.ReloadScreen();
        this.ExtPageReloadScreen();
    }

    //Back
    AutoRecoBackButtonClicked() {
        this.IsAutoReconcile = false;

        this.ReloadScreen();
        this.ExtPageReloadScreen();
    }
    autoRecoCount: number;
    GetAutoRecoBackMSG(): string {
        this.autoRecoCount = this.TransactionSelectedLines.Length;
        var msg = TextCodeTranslator.Translate("Accounting.O.BackFromAutoRecoMSG");
        msg = msg.replace("#number", this.autoRecoCount.toString());
        return msg;
    }

    //operators
    private selectedOperator: any;
    get SelectedOperator() { return this.selectedOperator; }
    set SelectedOperator(value: any) {
        if (this.selectedOperator != value) {
            this.selectedOperator = value;

            this.OpenAmountTextChanged(this.openAmount);
            this.FiltersChanged();
        }
    }


    openAmount: number;
    get OpenAmount() { return this.openAmount; }
    set OpenAmount(value: number) {
        if (this.openAmount != value) {
            this.openAmount = value;

            this.isFiltersSelected = (!AppTool.IsNullOrEmpty(this.OpenAmount) || !AppTool.IsNullOrEmpty(this.FromDate));
            this.FiltersChanged();
        }
    }

    private selecteddateOperator: any;
    get SelectedDateOperator() { return this.selecteddateOperator; }
    set SelectedDateOperator(value: any) {
        if (this.selecteddateOperator != value) {
            this.selecteddateOperator = value;

            this.ChangeDate();
            this.FiltersChanged();
        }
    }

    fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;

            //Filters Selection
            this.isFiltersSelected = (!AppTool.IsNullOrEmpty(this.OpenAmount) || !AppTool.IsNullOrEmpty(this.FromDate));

            //Date Filter
            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("DocumentDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.ReloadScreen();
                this.ExtPageReloadScreen();
            } else {
                this.dateFilter = null;
                this.ReloadScreen();
                this.ExtPageReloadScreen();
            }
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;

            //Filters Selection
            this.isFiltersSelected = (!AppTool.IsNullOrEmpty(this.OpenAmount) || !AppTool.IsNullOrEmpty(this.FromDate));

            //Date Filter
            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("DocumentDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.ReloadScreen();
                this.ExtPageReloadScreen();
            } else {
                this.dateFilter = null;
                this.ReloadScreen();
                this.ExtPageReloadScreen();
            }
        }
    }

    FiltersChanged() {
        var t = setTimeout(() => {
            if (this.IsAutoReconcile) {
                this.AutoReco();
            }
        }, 700);
    }

    //#endregion

    CreateReconciliation() {
        var newReconciliation: ExternalReconciliationPM = this.InitNewReconciliation();

        this.AddLines(newReconciliation);

        return newReconciliation;
    }
    private AddLines(newEntity: ExternalReconciliationPM) {
        newEntity.ExternalReconciliationLines = [];
        var lineNumber = 1;

        for (var i = 0; i < this.TransactionSelectedLines.Length; i++) {
            var newLine: ExternalReconciliationLinePM = this.CreateLedgerLine(i, newEntity, lineNumber);

            newEntity.AddExternalReconciliationLine(newLine);

            lineNumber++;
        }

        for (var i = 0; i < this.ExtPageSelectedLines.Length; i++) {
            var newLine: ExternalReconciliationLinePM = this.CreateExternalTransactionLine(i, newLine, newEntity, lineNumber);
            newEntity.AddExternalReconciliationLine(newLine);

            lineNumber++;
        }
    }

    private CreateExternalTransactionLine(i: number, newLine: ExternalReconciliationLinePM, newEntity: ExternalReconciliationPM, lineNumber: number) {
        let selectedTransaction = this.ExtPageSelectedLines.Collection[i];
        var newLine: ExternalReconciliationLinePM = new ExternalReconciliationLinePM(newEntity);

        newLine.ChangeSetOp = "1";
        newLine.ReconciliationId = newEntity.Id;
        newLine.Tenant = newEntity.Tenant;
        newLine.Line = lineNumber;
        newLine.GroupNumber = selectedTransaction.GroupHash ? selectedTransaction.GroupHash : 1;
        newLine.ExternalPageLineId = selectedTransaction.Id;
        newLine.LedgerTransactionId = null;
        return newLine;
    }

    private CreateLedgerLine(i: number, newEntity: ExternalReconciliationPM, lineNumber: number) {
        let selectedTransaction: TransactionLineModel = this.TransactionSelectedLines.Collection[i];
        var newLine: ExternalReconciliationLinePM = new ExternalReconciliationLinePM(newEntity);

        newLine.ChangeSetOp = "1";
        newLine.ReconciliationId = newEntity.Id;
        newLine.Tenant = newEntity.Tenant;
        newLine.Line = lineNumber;
        newLine.GroupNumber = selectedTransaction.GroupHash ? selectedTransaction.GroupHash : 1;
        newLine.LedgerTransactionId = selectedTransaction.Id;
        newLine.ExternalPageLineId = null;
        newLine.LedgerGLAccountId = selectedTransaction.LedgerTransactionPM.AccountId;
        return newLine;
    }

    private InitNewReconciliation() {
        var newEntity: ExternalReconciliationPM = new ExternalReconciliationPM();

        newEntity.Id = "new";
        newEntity.GLAccountId = this.getGLAccountId();
        newEntity.BankAccountId = this.BankAccountPM.Id;
        newEntity.Tenant = this.BankAccountPM.Tenant;
        newEntity.CreateDate = new Date();
        newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
        return newEntity;
    }

    SubmitChanges(entity) {

        this.CurrentSession.StartBusyIndicatorSaving();
        this._ExternalReconciliationOpService.insert(entity).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            var mm: ServiceResponse = myResult;
            var entity = mm.Result?.CreatedExternalReconciliation;
            if (!mm.HasError) {
                this.CreatedReconciliationsCount = mm.Result.CreatedReconciliationsCount;
                this.ExternalRecoPM = entity;
                this.ShowSuccessAlert();
                this.TransactionSelectedLines.Clear();
                this.ExtPageSelectedLines.Clear();
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    OpenSource(id: string, type: string) {

        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = AccountingEntityHelper.getEntityObjectTableName(type);

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName,
                    BackButtonLabel: 'GLAccount'
                });
            });

    }
    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    OpenReco() {
        if (!AppTool.IsNullOrEmpty(this.ExternalRecoPM.Id)) {
            this.showAlert = false;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.ExternalRecoPM.Id, ObjectTableName: 'ExternalReconciliation' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.CurrentSession.CloseCurrentWindow();
                    });
                });

        }
    }

    showAlert: boolean = false;
    ShowSuccessAlert() {
        if (this.IsAutoReconcile) {
            this.IsAutoReconcile = false;
            this.showAlert = true;
            this.timerToken = setTimeout(() => {
                this.showAlert = false;
            }, 7000); // 7 sec
            this.AutoRecoBackButtonClicked();
        }
        else {
            this.showAlert = true;
            this.timerToken = setTimeout(() => {
                this.showAlert = false;
            }, 5000); // 5 sec
            this.ReloadScreen();
            this.ExtPageReloadScreen();
        }


    }
    ShowEmptyAutoReco() {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 400;
        messageWindow.Height = 150;
        messageWindow.RTL = this.isRTL;
        messageWindow.Title = " ";
        messageWindow.Show(TextCodeTranslator.Translate("Accounting.O.Noautorecofoundbymethodchangemethod"));
    }
    CloseAlert() {
        this.showAlert = false;
    }

    openAmountCurrency: string = "";
    originalAmountCurrency: string = "";

    getTotalText(gridName: string) {
        var number = 0;
        if (gridName == 'glaccount') {
            number = this.TransactionSelectedLines.Length;
        }
        if (gridName == 'ExtPage') {
            number = this.ExtPageSelectedLines.Length;
        }
        var text = this.text_SumOfXRowsSelected;
        text = text.replace('%Number', number.toString());
        return text;

    }

    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();
    GetDefaultValues() {
        this.entityListService.getSingle(SessionLocator.Tenant + "", "FullAccountingSetting").then((res: any) => {
            res.subscribe(myResponse => {
                if (myResponse != null) {
                    var res = myResponse.Result;
                    this.FullAccountingSetting = res;

                    this.GetAutoRecoMethod();

                    switch (this.FullAccountingSetting.ExternalReconciliationDefault) {
                        case '1': { // Amount
                            this.AmountCheckBoxChecked = true;
                            this.ReferenceDateCheckBoxChecked = false;
                            this.ReferenceCheckBoxChecked = false;
                            break;
                        }
                        case '2': { // Reference
                            this.AmountCheckBoxChecked = false;
                            this.ReferenceDateCheckBoxChecked = false;
                            this.ReferenceCheckBoxChecked = true;
                            break;
                        }
                        case '3': { // Reference Date + Reference
                            this.AmountCheckBoxChecked = false;
                            this.ReferenceDateCheckBoxChecked = true;
                            this.ReferenceCheckBoxChecked = true;
                            break;
                        }
                        case '4': { // Amount + Reference + Reference Date
                            this.AmountCheckBoxChecked = true;
                            this.ReferenceDateCheckBoxChecked = true;
                            this.ReferenceCheckBoxChecked = true;
                            break;
                        }

                        default:
                            break;
                    }

                }
            })
        });
    }
    AutoRecoMethod: any;
    GetAutoRecoMethod() {
        if (this.FullAccountingSetting.ExternalReconciliationDefault) {
            this.entityListService.getSingle(this.FullAccountingSetting.ExternalReconciliationDefault + "", "AutomaticExternalRconcilMthod").then((res: any) => {
                res.subscribe(myResponse => {
                    if (myResponse != null) {
                        var res = myResponse.Result;
                        this.AutoRecoMethod = res;
                        if (this.AutoRecoMethod) {
                            switch (this.AutoRecoMethod.Code) {
                                case '1': { // Amount
                                    this.AmountCheckBoxChecked = true;
                                    this.ReferenceDateCheckBoxChecked = false;
                                    this.ReferenceCheckBoxChecked = false;
                                    break;
                                }
                                case '2': { // Reference
                                    this.AmountCheckBoxChecked = false;
                                    this.ReferenceDateCheckBoxChecked = false;
                                    this.ReferenceCheckBoxChecked = true;
                                    break;
                                }
                                case '3': { // Reference Date + Reference
                                    this.AmountCheckBoxChecked = false;
                                    this.ReferenceDateCheckBoxChecked = true;
                                    this.ReferenceCheckBoxChecked = true;
                                    break;
                                }
                                case '4': { // Amount + Reference + Reference Date
                                    this.AmountCheckBoxChecked = true;
                                    this.ReferenceDateCheckBoxChecked = true;
                                    this.ReferenceCheckBoxChecked = true;
                                    break;
                                }

                                default:
                                    break;
                            }

                        }

                    }
                })
            });
        }
    }

    ResetFilters() {

        // Date
        this.SelectedDateOperator = this.DateFilterList[4];
        this.FromDate = null;
        this.ToDate = null;

        // Amount
        this.OpenAmount = null;
        this.SelectedOperator = this.OperatorsList[0];
        this.OpenAmountTextChanged(null);
    }

    reapeatCount: number = 1;

    //#region TEST PURPOSE
    IsGenerateButtonVisible: boolean = false;
    IsGeneratePasswordVisible: boolean = false;
    GenerateTestLines(txt: string) {
        this.CurrentSession.StartBusyIndicator("Generate test lines... " + "(" + this.reapeatCount + "/" + 100 + ")");
        this._ExternalReconciliationExtendedListService.getGenerateTestRecordsForExternalReco(this.BankAccountPM.Id, this.BankAccountPM.GLAccountId, txt).subscribe((myResult: ServiceResponse) => {

            var mm: ServiceResponse = myResult;
            var result = mm.Result;
            if (!mm.HasError) {

                if (!AppTool.IsNullOrEmpty(result)) {

                    if (this.reapeatCount == 100) {
                        var msg = new MessageWindow();
                        this.CurrentSession.StopBusyIndicator();

                        msg.Show("Test lines generated successfully :) ");
                        this.ReloadScreen();
                        this.ExtPageReloadScreen();
                    }
                    else {
                        this.reapeatCount++;
                        this.GenerateTestLines(txt);
                    }



                }
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    labelCount = 0;
    LabelClicked() {
        var feature = FeatureLocator.IsFeatureGrantedByCode("ExtRecoGenerateTestRecords");
        if (feature) {
            this.labelCount++;
            if (this.labelCount == 5) {
                this.IsGeneratePasswordVisible = true;
            }
        }
    }
    GeneratePWD: string;
    PasswordOkButtonClicked() {
        if (this.GeneratePWD == "extrecopwd") {
            this.IsGeneratePasswordVisible = false;
            this.IsGenerateButtonVisible = true;
        } else {
            this.GeneratePWD = "";
        }
    }
    //#endregion

    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }

    CreatePaymentButtonClicked() {

        this.ValidationErrorsList = [];

        this.ValidateSelectedPageLinesTotals();
        this.ValidateSelectedLines();

        if (this.ValidationErrorsList.length == 0) {
            this.ShowNewBankTransferARPayment();
        }
    }


    private ValidateSelectedPageLinesTotals() {
        var hasDebitLines = this.ExtPageSelectedLines.Collection.find(line => line.PageLinePM.DebitAmount != 0);
        var hasNegativeCreditLines = this.ExtPageSelectedLines.Collection.find(line => line.PageLinePM.CreditAmount < 0);
        if (hasDebitLines || hasNegativeCreditLines) {
            this.ValidationErrorsList.push(this.SelectCreditLinesOnlyMessage);
        }
    }
    private ValidateSelectedLines() {
        if (this.TransactionSelectedLines.Length > 0) {
            this.ValidationErrorsList.push(this.OnlyBankPagesMessage);
        }
    }
    ShowNewBankTransferARPayment() {
        this.showBankTransferAlert = false;
        var logWindow = new LogitudeWindow();
        logWindow.Title = this.NewARPaymentTitle;
        logWindow.Width = NewARPaymentWindowWidth;
        logWindow.Height = NewARPaymentWindowHeight;
        this.SetNewBankTransferWindowArguments(logWindow);

        logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ReloadScreen();
            this.ExtPageReloadScreen();
            this.TransactionSelectedLines = new ObservableCollection([]);
            this.ExtPageSelectedLines = new ObservableCollection([]);

            setTimeout(() => {
                this.RefreshButtonClicked();
            }, 1500);
        });

    }

    private SetNewBankTransferWindowArguments(logWindow: LogitudeWindow) {

        let bankTransferPaymentArguments = new BankTransferPaymentArguments();
        bankTransferPaymentArguments.BankAccountId = this.BankAccountPM?.Id;
        bankTransferPaymentArguments.CurrencyId = this.GetGLAccountCurrency();
        bankTransferPaymentArguments.PaymentAmount = this.GetSelectedExternalPageLinesCreditTotal();
        const singleBankPageLineSelected = this.ExtPageSelectedLines.Length == 1;
        if (singleBankPageLineSelected) {
            bankTransferPaymentArguments.ValueDate = this.ExtPageSelectedLines.Collection[0].ReferenceDate;
            bankTransferPaymentArguments.RegisterDate = this.ExtPageSelectedLines.Collection[0].ReferenceDate;
            bankTransferPaymentArguments.PaymentReference = this.ExtPageSelectedLines.Collection[0].Reference;
        }

        logWindow.WindowArgs = {
            AccountingPaymentMethodCode: BankTransferPaymentMethodCode,
            ExteranlPageLinesIds: this.GetSelectedPageLinesIds(),
            BankTransferPaymentArguments: bankTransferPaymentArguments
        };
    }

    private GetSelectedPageLinesIds() {
        return this.ExtPageSelectedLines.Collection.map(line => line.Id).join(',');
    }

    private GetSelectedExternalPageLinesCreditTotal() {
        var externalPagesTotal = 0;
        this.ExtPageSelectedLines.Collection.forEach((line: ExtPageLineModel) => {
            externalPagesTotal += line.Amount;
        });
        return externalPagesTotal;
    }


    //#region Filter Methods
    public filterSelectedValue: string = 'filter_All';
    FilterItemClicked(itemValue: string) {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterChanged();
        }
    }
    FilterChanged() {
        this.RefreshButtonClicked();
    }
    //#endregion


}


class TransactionLineModel extends BaseComponent {
    public LedgerTransactionPM: LedgerTransactionPM = null;
    public ObjectTableName = "LedgerTransaction";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;

    constructor(
        private ledgerTransaction: LedgerTransactionPM,
        private parent: ExternalReconcileComponent,
        private myRowIndex: number
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.LedgerTransactionPM = ledgerTransaction;
        //this.OriginalAmount = parent.CalculateOriginalAmount(this);
        //if (!this.AmountToReconcile)
        //    this.AmountToReconcile = this.ledgerTransaction.OpenAmount;
        this.RowIndex = myRowIndex;

        //this.OddEven = this.ColorMe();


        //#region Set Icons


        this.IconCode = AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);

        //#endregion
    }

    get GroupHash() { return this.LedgerTransactionPM.GroupHash };


    get IsCredit() {
        return this.ledgerTransaction.ForeignAmountCredit != 0;
    }

    // Properties
    public IconCode: string;

    //originalAmount: number;
    //get OriginalAmount() { return this.parent.CalculateOriginalAmount(this); }
    //set OriginalAmount(value: number) {
    //    if (this.originalAmount != value) {
    //        this.originalAmount = this.parent.CalculateOriginalAmount(this);
    //    }
    //}

    //get AmountToReconcile() { return this.LedgerTransactionPM.AmountToReconcile; }
    //set AmountToReconcile(value: number) {
    //    if (this.LedgerTransactionPM.AmountToReconcile != value) {
    //        this.LedgerTransactionPM.AmountToReconcile = value;

    //        //WI26522
    //        if (Math.abs(value) > Math.abs(this.OpenAmount)) {
    //            this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
    //            this.parent.IsEntityValid = false;
    //        } else {
    //            this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
    //            this.parent.IsEntityValid = true;
    //        }

    //        this.parent.CalculateTotals();
    //    }

    //}

    OddEven: boolean;


    //#region Other Properties
    get Id() { return this.LedgerTransactionPM.Id; }
    get Tenant() { return this.LedgerTransactionPM.Tenant; }
    get AccountingDate() { return this.LedgerTransactionPM.AccountingDate; }
    get DocumentDate() { return this.LedgerTransactionPM.DocumentDate; }
    get JournalNumber() { return this.LedgerTransactionPM.JournalNumber; }
    get Source() { return this.LedgerTransactionPM.Source; }
    get SourceType() { return this.LedgerTransactionPM.SourceType; }
    get SourceId() { return this.LedgerTransactionPM.SourceId; }
    get DueDate() { return this.LedgerTransactionPM.DueDate; }
    get LocalAmountCredit() { return this.LedgerTransactionPM.LocalAmountCredit; }
    get LocalAmountDebit() { return this.LedgerTransactionPM.LocalAmountDebit; }
    get ForeignAmountCredit() { return this.LedgerTransactionPM.ForeignAmountCredit; }
    get ForeignAmountDebit() { return this.LedgerTransactionPM.ForeignAmountDebit; }
    get ForeignAmount() { return this.LedgerTransactionPM.ForeignAmount; }
    get OpenAmount() { return this.LedgerTransactionPM.OpenAmount; }
    get OpenAmountCurrencyCode() { return this.LedgerTransactionPM.OpenAmountCurrencyCode; }
    get OpenAmountCurrencySign() { return this.LedgerTransactionPM.OpenAmountCurrencySign; }
    get CurrencyId() { return this.LedgerTransactionPM.CurrencyId; }
    get Reference1() { return this.LedgerTransactionPM.Reference1; }
    get Reference2() { return this.LedgerTransactionPM.Reference2; }
    get Reference3() { return this.LedgerTransactionPM.Reference3; }
    get Notes() { return this.LedgerTransactionPM.Notes; }
    //get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
    get OpenAmountCurrencyId() { return this.LedgerTransactionPM.OpenAmountCurrencyId; }
    get SourceTypeCode() { return this.LedgerTransactionPM.SourceTypeCode; }
    get SourceNumber() { return this.LedgerTransactionPM.SourceNumber; }

    //#endregion


    //#region Row Coloring

    ColorMe() {
        //if (AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
        //    this.parent.lastGroupNumber = this.GroupHash;

        //if (this.parent.lastGroupNumber == this.GroupHash) {
        //    return this.parent.lastColorOperation == true;
        //} else {
        //    this.parent.lastGroupNumber = this.GroupHash;
        //    this.parent.lastColorOperation = !this.parent.lastColorOperation;
        //    return this.parent.lastColorOperation == true;
        //}
    }
    //#endregion

    CalculatOriginalCurruncy() {
        //
        // [i] copied from list template
        //

        if (!AppTool.IsNullOrEmpty(ReconcileEventManager.GLAccountReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (ReconcileEventManager.GLAccountReconcileMethodCode == "0") { // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;

            } else if (ReconcileEventManager.GLAccountReconcileMethodCode == "1") { // 1-foreign currency

                // foreign
                return this.ledgerTransaction.CurrencySign;

            }

        }
    }


}


class ExtPageLineModel extends BaseComponent {
    public PageLinePM: ReconcileExternalPageLinePM = null;
    public ObjectTableName = "ReconcileExternalPageLine";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;

    constructor(
        private pageLine: ReconcileExternalPageLinePM,
        private parent: ExternalReconcileComponent,
        private myRowIndex: number
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.PageLinePM = pageLine;
        this.RowIndex = myRowIndex;

    }

    get IsCredit() {
        return this.pageLine.CreditAmount != 0;
    }
    get GroupHash() { return this.pageLine.GroupHash };

    //#region Properties
    get Id() { return this.PageLinePM.Id; }
    get Amount() { return this.PageLinePM.Amount; }
    get Reference() { return this.PageLinePM.Reference; }
    get ReferenceDate() { return this.PageLinePM.ReferenceDate; }
    get Notes() { return this.PageLinePM.Notes; }
    get LineNumber() { return this.PageLinePM.LineNumber; }

    //#endregion


}

