import { FullAccountingSettingPM } from './../../EntityPMs/FullAccountingSettingPM';
import { FullAccountingSettingPMService } from './../../Services/StandardPMs/FullAccountingSettingPMService';
import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { ReconciliationPM } from '../../EntityPMs/ReconciliationPM';

import { AutomaticReconcileMethodList } from '../../EntityLists/AutomaticReconcileMethodList';
import { LedgerTransactionPM } from '../../EntityPMs/LedgerTransactionPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ReconcileEventManager } from '../../Utilities/ReconcileEventManager';
import { ReconcileExcelDataArgs, ReconciliationExtendedPMService } from '../../Services/ExtendedPMs/ReconciliationExtendedPMService';
import { LedgerTransactionExtendedListService } from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { RecoCallback } from '../../DataContracts/RecoCallback';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { APPaymentPM } from 'Invoice/EntityPMs/APPaymentPM';

import { delay, expand, takeLast } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { GLAccountExtendedListService } from 'Accounting/Services/ExtendedLists/GLAccountExtendedListService';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
import { GLAccountExtendedPMService } from 'Accounting/Services/ExtendedPMs/GLAccountExtendedPMService';
import { JournalExtendedPMService } from 'Accounting/Services/ExtendedPMs/JournalExtendedPMService';

export class LineModel extends BaseComponent {
    public LedgerTransactionPM: LedgerTransactionPM = null;
    public ObjectTableName = "LedgerTransaction";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;
    public Title: string = '';

    IsAccountingActivated: boolean = false;
    constructor(
        public ledgerTransaction: LedgerTransactionPM,
        public parent: ReconcileComponent,
        public myRowIndex: number
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.LedgerTransactionPM = ledgerTransaction;
        this.OriginalAmount = parent.CalculateOriginalAmount(this);
        if (!this.AmountToReconcile)
            this.AmountToReconcile = this.ledgerTransaction.OpenAmount;
        this.RowIndex = myRowIndex;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;


        this.OddEven = this.ColorMe();


        //#region Set Icons

        this.IconCode = AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);

        //#endregion
    }

    get GroupHash() { return this.LedgerTransactionPM.GroupHash };

    isLineValid: boolean = true;

    // Properties
    public IconCode: string;

    originalAmount: number;
    get OriginalAmount() { return this.parent.CalculateOriginalAmount(this); }
    set OriginalAmount(value: number) {
        if (this.originalAmount != value) {
            this.originalAmount = this.parent.CalculateOriginalAmount(this);
        }
    }

    get AmountToReconcile() { return this.LedgerTransactionPM.AmountToReconcile; }
    set AmountToReconcile(value: number) {
        if (this.LedgerTransactionPM.AmountToReconcile != value) {
            this.LedgerTransactionPM.AmountToReconcile = value;

            if (value == 0 && (this.OpenAmount < 0 || this.OpenAmount > 0))
            {
                this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.ZeroNotAllowed"));
                this.parent.IsEntityValid = false;
                this.isLineValid = false;
            }
            else if (this.OpenAmount < 0) { // debit
                if (value < this.OpenAmount || value > 0) {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                    this.parent.IsEntityValid = false;
                    this.isLineValid = false;
                } else {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                    this.parent.IsEntityValid = true;
                    this.isLineValid = true;

                }
            } else { // credit
                if (value > this.OpenAmount || value < 0) {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                    this.parent.IsEntityValid = false;
                    this.isLineValid = false;

                } else {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                    this.parent.IsEntityValid = true;
                    this.isLineValid = true;
                }
            }

            //WI26522
            if (this.isLineValid && this.parent.IsEntityValid) {
                if (Math.abs(value) > Math.abs(this.OpenAmount)) {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                    this.parent.IsEntityValid = false;
                    this.isLineValid = false;

                } else {
                    this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                    this.parent.IsEntityValid = true;
                    this.isLineValid = true;
                }
            }
            //}

            this.parent.CalculateTotals();
        }

    }
    AutomaticallyFillAmountToReconciledblclick(logCellTemplate: any, AmountToReconcileTextBox: any) {
        if (this.IsAccountingActivated && this.LedgerTransactionPM.AmountToReconcile == null) {
            if (this.parent.OriginalDifference != 0 && this.OpenAmount != 0) {

                if (Math.abs(this.parent.OriginalDifference) < Math.abs(this.OpenAmount)) {
                    if (Math.sign(this.parent.OriginalDifference) == Math.sign(this.OpenAmount)) {
                        this.LedgerTransactionPM.AmountToReconcile = this.parent.OriginalDifference;
                    }
                    else {
                        this.LedgerTransactionPM.AmountToReconcile = -1 * this.parent.OriginalDifference;
                    }

                }
                else {
                    this.LedgerTransactionPM.AmountToReconcile = this.OpenAmount;
                }
            }
            else {
                this.LedgerTransactionPM.AmountToReconcile = 0;
            }
            AmountToReconcileTextBox.ForceDisabled = true;
            AmountToReconcileTextBox.IsDisabled = true;
            this.parent.CalculateTotals();
        }

    }

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
    get OpenAmount() { return this.LedgerTransactionPM.OpenAmount; }
    get OpenAmountCurrencyCode() { return this.LedgerTransactionPM.OpenAmountCurrencyCode; }
    get OpenAmountCurrencySign() { return this.LedgerTransactionPM.OpenAmountCurrencySign; }
    get CurrencyId() { return this.LedgerTransactionPM.CurrencyId; }
    get Reference1() { return this.LedgerTransactionPM.Reference1; }
    get Reference2() { return this.LedgerTransactionPM.Reference2; }
    get Reference3() { return this.LedgerTransactionPM.Reference3; }
    get Notes() { return this.LedgerTransactionPM.Notes; }
    get InternalNote() { return this.LedgerTransactionPM.InternalNote; }
    get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
    get OpenAmountCurrencyId() { return this.LedgerTransactionPM.OpenAmountCurrencyId; }
    get SourceTypeCode() { return this.LedgerTransactionPM.SourceTypeCode; }
    get SourceNumber() { return this.LedgerTransactionPM.SourceNumber; }
    get GroupNumber() { return this.LedgerTransactionPM.GroupHash; }

    //#endregion

    //#region Row Coloring

    ColorMe() {
        if (AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
            this.parent.lastGroupNumber = this.GroupHash;

        if (this.parent.lastGroupNumber == this.GroupHash) {
            return this.parent.lastColorOperation == true;
        } else {
            this.parent.lastGroupNumber = this.GroupHash;
            this.parent.lastColorOperation = !this.parent.lastColorOperation;
            return this.parent.lastColorOperation == true;
        }
    }
    //#endregion

    CalculateOriginalCurrency() {
        //
        // [i] copied from list template
        //

        if (!AppTool.IsNullOrEmpty(this.parent.GLAccountPM.ReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.parent.GLAccountPM.ReconcileMethodCode == "0") { // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;

            } else if (this.parent.GLAccountPM.ReconcileMethodCode == "1") { // 1-foreign currency

                // foreign
                return this.ledgerTransaction.CurrencySign;

            }

        }
    }


}

const exportToExcelWindowWidth = 500;
const exportToExcelWindowHeight = 200;
@Component({
    selector: 'ReconcileComponent',
    moduleId: './Accounting/Components/Others/',
    providers: [EntityListService],
    templateUrl: 'ReconcileComponent.html',
    styleUrls: ['ReconcileComponent.css']
})

export class ReconcileComponent extends BaseComponent implements OnInit, OnDestroy {

    public EntityPM: LedgerTransactionPM;
    public GLAccountPM: GLAccountPM;
    public RecoPM: ReconciliationPM;
    public DataContext: ReconcileComponent = this;
    public ObjectTableName: string = "LedgerTransaction"; //Reconciliation
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public FireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
    public ColumnsReady: EventEmitter<any> = new EventEmitter();
    public MarkIsChecked: EventEmitter<any> = new EventEmitter();
    public IsAutoReconcile: boolean = false;
    public recoCallback: RecoCallback;
    public lastGroupNumber: number;
    public lastColorOperation: boolean = false;
    public ChangeCheckBoxesState: EventEmitter<any> = new EventEmitter();
    public filterAgrs: ApiQueryFilters;
    public isRTL: boolean = false;
    public Operators: any[] = [];
    public IsEntityValid: boolean = true;
    public CurrentSession = SessionLocator.SelectedSession;
    public LogitudeGridExportToExcelComponent: LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();
    public SelectedLines: ObservableCollection = new ObservableCollection([]);
    public _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    public _ReconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
    public fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();
    public entityListService: EntityListService = new EntityListService();
    public IsComponentDestroyed: boolean = false;
    public IsMultiWithReconcileMethodCodeEqualOne = false;
    public isMark: boolean = false;
    public autoReconil: boolean = false;
    public firstMark: boolean = true;
    public yelloMessage: string = '';
    public failedJournalsInReconcileProcess: boolean = false;
    SessionEvent;
    showInternalReconcileAPPaymentAlert = false;
    createdPaymentNumber;
    Title: string = '';
    public NumberOfselectedlines = 0;
    public NumberOfFilteredlines = 0;
    _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    _GLAccountExtendedPMService: GLAccountExtendedPMService = new GLAccountExtendedPMService();

    isFullAccounting: boolean = SessionLocator.TenantPM.AccountingActivated;
    CurrencyFilters: ApiQueryFilters = new ApiQueryFilters();
    revalOnForeignReco: boolean = false;
    revalJrnlRef1: string = '';


    constructor(public CD: ChangeDetectorRef) {
        super();

        this.InitComponent();
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(ReconcileEventManager.CheckBoxChecked);
        ReconcileEventManager.CheckBoxChecked = new EventEmitter();
        this.IsComponentDestroyed = true;
    }

    public InitComponent() {
        this.SetComponentRTL();

        this.GetTenant();

        this.InitEntity();

        this.InitFilters();


    }
    now: Date = null;
    private Mark() {
        
        if (this.GLAccountPM.MarkDate) {
            this.now = new Date()
            this.now.setHours(this.now.getHours() - 1);
            this.isMark = new Date(this.GLAccountPM.MarkDate) > this.now;
            this.yelloMessage =this.isMark? TextCodeTranslator.Translate("Reconciliations.O.isMatched"):'';
        }


    }
    public RefreshEntity() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.UpdateIsMark();
    }

    UpdateIsMark() {
        this._GLAccountExtendedPMService.SetGLAccountIsMark(this.GLAccountPM?.Id).subscribe((myResult: any) => {
            if (!myResult.HasError) {
                this.now = new Date()
                this.now.setHours(this.now.getHours() - 1);
                this.GLAccountPM.MarkDate = myResult?.Result?.wasNull ? myResult?.Result?.MarkDate : this.GLAccountPM.MarkDate;
                this.isMark = !myResult?.Result?.wasNull ;
                this.yelloMessage =this.isMark ? TextCodeTranslator.Translate("Reconciliations.O.isMatched"):'' ;
                this.CurrentSession.StopBusyIndicator();
            }
           

        });
    }
    UndoMark() {
        this.GLAccountPM.MarkDate = null;
        this.isMark = false;
        this.yelloMessage = '';
        this._GLAccountExtendedPMService.UndoMark(this.GLAccountPM?.Id).subscribe((myResult: any) => {
            
        });
    }
    windowArgs;
    SetWindowArgs(args: any) {

        if (args != null) {
            this.windowArgs = args;

            this.GLAccountPM = args.GLAccountPM;
            ReconcileEventManager.SetGLAccountReconcileMethodCode(this.GLAccountPM.ReconcileMethodCode);

            if (!AppTool.IsNullOrEmpty(args.IsMultiWithReconcileMethodCodeEqualOne)) {
                this.IsMultiWithReconcileMethodCodeEqualOne = args.IsMultiWithReconcileMethodCodeEqualOne;
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Reconciliation.O.WarningMultiRecoOne"));
                this.IsCheckBoxEnabled = false;
                this.GetTransactionsCurrencies();
                GLAccountSecurityLevelService.IsCheckBoxEnabledParameter = false;
            }
            if (!AppTool.IsNullOrEmpty(this.GLAccountPM.CurrencyId)) {
                this.CurrencyId = this.GLAccountPM.CurrencyId;
            }
            if (this.GLAccountPM.AutomaticReconcileId)
                this.AutomaticReconcileId = this.GLAccountPM.AutomaticReconcileId;
            else {
                this.SetDefaultReconcileMethodFromAccountingSettings();
            }
            this.SetUIProperty();
            this.openAmountCurrency = args.openAmountCurrency;
            this.originalAmountCurrency = args.originalAmountCurrency;

            this.CheckIfThereIsDraftReconcile();
            this.DisableDates();
            this.Mark()
            this.GetFailedJournalsInReconcileProcess();
        }

        this.SetTitle();
        if (this.isFullAccounting)
            this.SetNumberOfFilteredlines(args.GLAccountPM.id);

    }
    SetNumberOfFilteredlines(id) {
        this._GLAccountExtendedListService = new GLAccountExtendedListService();
        this._GLAccountExtendedListService.GetAccountOpenTransactionsCount(id).subscribe((myResult: any) => {
            console.log("GetAccountOpenTransactionsCount", myResult);
            var result: ServiceResponse = myResult;
            if (!result.HasError) {
                this.NumberOfFilteredlines = result.Result;
            }
            else { }
        });
    }
    OpenLedgerTransactionInternalNote(line: any) {

        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("ARInvoice.F.InternalNotes");
        logWindow.WindowArgs = { ledgerTransaction: line };
        logWindow.Show('./Accounting/Components/Others/LedgerTransactionInternalNotesComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.detectChanges();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    openTransactionLabel;
    accountName;
    displayNumber;
    paymentTermName;

    GetTransactionsCurrencies() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._LedgerTransactionExtendedListService.GetTransactionsCurrencies(this.GLAccountPM?.Id, false, false)
            .subscribe((serviceResponse: ServiceResponse) => {
                if (serviceResponse.Result) {
                    var result = serviceResponse.Result;
                    console.log("[GetTransactionsCurrencies]", result);
                    var currenciesIds: string[] = result;

                    this.CurrencyFilters = new ApiQueryFilters();
                    this.CurrencyFilters.addAdditionalFilter("Id", currenciesIds.join(','), null, null, "InListExact", false, false, false, "string", false, true);

                    this.CurrentSession.StopBusyIndicator();
                }
            });
    }

    SetTitle() {
        this.openTransactionLabel = TextCodeTranslator.Translate('Reconciliation.O.OpenTransactions');

        const showLocals = !SessionLocator.LoggedUserPM.DontShowLocalLabels;
        this.accountName = showLocals ? this.GLAccountPM.LocalName : (this.GLAccountPM.EnglishName || this.GLAccountPM.LocalName);
        this.displayNumber = this.GLAccountPM.DisplayNumber;
        // let title = `${openTransactionLabel} - ${this.GLAccountPM.DisplayNumber} - ${name}`;

        if (this.windowArgs.PaymentTermName)
            this.paymentTermName = this.windowArgs.PaymentTermName;

        // this.Title = title;
    }

    public GetTenant() {
        this.TenantPM = SessionLocator.TenantPM;
    }

    public SetComponentRTL() {
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    public InitEntity() {
        this.EntityPM = new LedgerTransactionPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.CurrencyId = this.EntityPM.CurrencyId;
        this.CurrentSession.entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => { 
            this.CurrentSession.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { ; });
         });
       

    }

    public InitFilters() {
        this.BuildAmountFiltersOperatorsList();
        this.InitDateFilter();
    }

    public BuildAmountFiltersOperatorsList() {
        this.Operators =
            [{ EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
            { EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
            { EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
            { EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
            { EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
            { EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
            ];
        this.openAmountSelectedOperator = this.Operators[0];
        this.foreignAmountSelectedOperator = this.Operators[0];
    }

    public InitDateFilter() {
        this.DateFilterPresetsList =
            [
                { EnglishName: 'Last 7 days', LocalName: TextCodeTranslator.Translate("Accounting.O.Last7days") },
                { EnglishName: 'Last month', LocalName: TextCodeTranslator.Translate("Accounting.O.Lastmonth") },
                { EnglishName: 'Last 3 months', LocalName: TextCodeTranslator.Translate("Accounting.O.Last3months") },
                { EnglishName: 'Last year', LocalName: TextCodeTranslator.Translate("Accounting.O.Lastyear") },
                { EnglishName: 'Custom', LocalName: TextCodeTranslator.Translate("Accounting.O.Custom") },
            ];
        this.DateTypes =
            [
                { FieldName: 'AccountingDate', LocalName: TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate") },
                { FieldName: 'DocumentDate', LocalName: TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate") },
                { FieldName: 'DueDate', LocalName: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate") },
            ];
        this.SelectedDateType = this.DateTypes[0];
    }

    public ExportToExcelClick() {
        this.ValidationErrorsList = [];
        if (this.SelectedLines.Length == 0)
            this.ValidationErrorsList = [TextCodeTranslator.Translate("Reconciliation.O.NoLinesSelected")];
        else
            this.ShowExportToExcelWindow();
    }

    private ShowExportToExcelWindow() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = exportToExcelWindowWidth;
        logitudeWindow.Height = exportToExcelWindowHeight;
        logitudeWindow.WindowArgs = this.GetExportToExcelWindowArgs();
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
    }

    private GetExportToExcelWindowArgs() {
        var args: ReconcileExcelDataArgs = new ReconcileExcelDataArgs();
        args.QueryColumns = this.QueryColumns;
        args.Tenant = SessionLocator.Tenant;
        args.Data = this.SelectedLines.Collection.map((d: LineModel) => d.LedgerTransactionPM);

        var windowArgs: any = {};
        windowArgs.ReconcileExcelDataArgs = args;
        windowArgs.tenant = SessionLocator.Tenant;
        windowArgs.ObjectTableName = this.ObjectTableName;
        windowArgs.QueryName = this.ObjectTableName;
        windowArgs.QueryType = "DraftReconciliation";
        return windowArgs;
    }

    public AddAccountIdFilterForFilterAgrs() {
        var AccountFilter = new FilterItem("AccountId", this.GLAccountPM.Id, null, null, "Equals", false, false, false, "string", false);
        this.filterAgrs.AdditionalFilters.push(AccountFilter);
    }

    public AddIsReconciledFiltersForFilterAgrs() {
        var IsReconciledFilter = new FilterItem("IsReconciled", false, null, null, "Equals", false, false, false, "boolean", false);
        var IsExternalReconcileFilter = new FilterItem("IsExternalReconcile", false, null, null, "Equals", false, false, false, "boolean", false);
        var InReconcileProgressFilter = new FilterItem("InReconcileProgress", false, null, null, "Equals", false, false, false, "boolean", false);
        this.filterAgrs.AdditionalFilters.push(IsReconciledFilter);
        this.filterAgrs.AdditionalFilters.push(IsExternalReconcileFilter);
        this.filterAgrs.AdditionalFilters.push(InReconcileProgressFilter);
    }

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
    SetUIProperty() {
        if (this.GLAccountPM.IsMultiCurrency) {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        } else {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
        if (this.IsMultiWithReconcileMethodCodeEqualOne && this.CurrencyId == null) {
            this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
            if (this.SelectedLines.Length > 0) {
                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            }
        }
    }

    ngOnInit() {
        this.InitTextCodes();
        this.BuildColumns();
        this.Listen();
        //this.ColumnsReady.emit("");
    }

    OpenAmountTextCode;
    OriginalAmountTextCode;

    InitTextCodes() {
        if (this.IsMultiWithReconcileMethodCodeEqualOne) {
            this.OpenAmountTextCode = TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount");
            this.OriginalAmountTextCode = TextCodeTranslator.Translate("Accounting.General.O.OriginalAmount");
        } else {
            this.OpenAmountTextCode = TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + ' (' + (this.GLAccountPM.IsMultiCurrency ? this.TenantPM.CurrencyCode : this.openAmountCurrency) + ')';
            this.OriginalAmountTextCode = TextCodeTranslator.Translate("Accounting.General.O.OriginalAmount") + ' (' + (this.GLAccountPM.IsMultiCurrency ? 'multi' : this.originalAmountCurrency) + ')';
        }
    }
    ngAfterViewInit() {
        this.RaiseEvent();
    }

    Listen() {
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s && s.Name == "InternalReconcileAPPaymentCreated") {
                this.createdPaymentNumber = s.PaymentNumber;
                this.showInternalReconcileAPPaymentAlert = true;
                // this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
                this.isAllSelected = false;
                setTimeout(() => {
                    this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
                }, 500);
            }

        });
    }

    RaiseEvent() {
        if (this.IsMultiWithReconcileMethodCodeEqualOne) {
            GLAccountSecurityLevelService.IsMultiWithReconcileMethodCodeEqualOneParameter = true;
        }
    }

    SetDefaultReconcileMethodFromAccountingSettings() {
        this.fullAccountingSettingPMService.get(SessionLocator.TenantPM.Id.toString()).subscribe((myResult: any) => {
            var myResponse: ServiceResponse = myResult;
            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {

                var res = myResponse.Result;
                var fullAccountingSetting: FullAccountingSettingPM = res;

                if (fullAccountingSetting) {
                    this.AutomaticReconcileId = fullAccountingSetting.AutomaticReconcileMethodId;
                }

            }

        });
    }


    //#region Properties


    public _isAllSelected: boolean;
    public get isAllSelected(): boolean {
        return this._isAllSelected;
    }
    public set isAllSelected(v: boolean) {
        this._isAllSelected = v;
        if (v) {
           
            if (this.firstMark) {
                this.firstMark = false;
                this.UpdateIsMark();
            }
            this.GetFirstXLedgerForReconciliationByParam();
            if (this.isFullAccounting) {
                this.NumberOfselectedlines = this.DataSource.rowCount;
                this.NumberOfFilteredlines = this.DataSource.rowCount;
            }

        } else {
            if (this.isFullAccounting)
                this.NumberOfselectedlines = 0;
            this.ReloadScreen();
            this.SelectedLines.Clear();
            this.CalculateTotals();
        }
    }

    private PopAllLineWhenCurrencyIdNull() {
        if (this.IsMultiWithReconcileMethodCodeEqualOne) {
            this.SelectedLines.Clear();
            this.CalculateTotals();
            this._isAllSelected = false;
            if (this.isFullAccounting) {
                this.NumberOfFilteredlines = this.DataSource.rowCount;
                this.NumberOfselectedlines = this.SelectedLines.Length;
            }
        }
    }

    public currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.currencyFilter = new FilterItem("CurrencyId", value, null, null, "Equals", false, false, false, "string", false);
                if (this.IsMultiWithReconcileMethodCodeEqualOne && this.CurrencyId != null) {
                    this.PopAllLineWhenCurrencyIdNull();
                    this.ValidationErrorsList = [];
                    this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, false);
                    GLAccountSecurityLevelService.IsCheckBoxEnabledParameter = true;
                    GLAccountSecurityLevelService.IsCheckBoxEnabled.emit({});
                    this.IsCheckBoxEnabled = true;
                }

            } else {
                this.currencyFilter = null
                if (this.IsMultiWithReconcileMethodCodeEqualOne) {
                    this.PopAllLineWhenCurrencyIdNull();
                    this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
                    GLAccountSecurityLevelService.IsCheckBoxEnabledParameter = false;
                    GLAccountSecurityLevelService.IsCheckBoxEnabled.emit({});
                    this.IsCheckBoxEnabled = false;
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Reconciliation.O.WarningMultiRecoOne"));
                }
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    isCheckBoxEnabled: boolean = true;
    get IsCheckBoxEnabled() { return this.isCheckBoxEnabled; }
    set IsCheckBoxEnabled(value: boolean) {
        if (this.isCheckBoxEnabled != value) {
            this.isCheckBoxEnabled = value;
        }
    }

    openAmount: number;
    get OpenAmount() { return this.openAmount; }
    set OpenAmount(value: number) {
        if (this.openAmount != value) {
            this.openAmount = value;
        }
    }

    foreignAmount: number;
    get ForeignAmount() { return this.foreignAmount; }
    set ForeignAmount(value: number) {
        if (this.foreignAmount != value) {
            this.foreignAmount = value;
        }
    }



    private openAmountSelectedOperator: any;
    get OpenAmountSelectedOperator() { return this.openAmountSelectedOperator; }
    set OpenAmountSelectedOperator(value: any) {
        if (this.openAmountSelectedOperator != value) {
            this.openAmountSelectedOperator = value;
            this.OpenAmountTextChanged(this.openAmount, true);
        }
    }
    private foreignAmountSelectedOperator: any;
    get ForeignAmountSelectedOperator() { return this.foreignAmountSelectedOperator; }
    set ForeignAmountSelectedOperator(value: any) {
        if (this.foreignAmountSelectedOperator != value) {
            this.foreignAmountSelectedOperator = value;
            this.ForeignAmountTextChanged(this.foreignAmount, true);
        }
    }
    private selectedDateType: any;
    get SelectedDateType() { return this.selectedDateType; }
    set SelectedDateType(value: any) {
        if (!value)
            value = this.DateTypes[0];
        if (this.selectedDateType != value) {
            this.selectedDateType = value;
            if (this.dateFilter) {
                this.dateFilter.FieldName = value.FieldName;
            }
            this.ReloadScreen();
        }
    }

    private automaticReconcileId: string;
    get AutomaticReconcileId() { return this.automaticReconcileId; }
    set AutomaticReconcileId(value: string) {
        if (this.automaticReconcileId != value) {
            this.automaticReconcileId = value;

        }
    }

    public automaticReconcile: AutomaticReconcileMethodList;
    get AutomaticReconcileMethodList() { return this.automaticReconcile; }
    set AutomaticReconcileMethodList(value: AutomaticReconcileMethodList) {
        if (this.automaticReconcile != value) {
            this.automaticReconcile = value;

        }
    }

    AutomaticReconcileChanged(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            this.AutomaticReconcileMethodList = item;
        }
    }

    //#endregion

    //#region Search Fields
    public timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters(), MustIgnoreItems: this.MustIgnoreItems }); // refresh grid
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters(), MustIgnoreItems: this.MustIgnoreItems }); // refresh grid
        }
    }
    OpenAmountTextChanged(searchtext, OperatorChanged: boolean = false) {

        if (!AppTool.IsNullOrEmpty(searchtext) && !AppTool.IsNullOrEmpty(this.OpenAmountSelectedOperator)) {
            console.log(searchtext + "!!!!!!!!!!!!!!!!!!!!")
            this.timerToken = setTimeout(() => {
                var OpenAmountFilterOperator = this.OpenAmountSelectedOperator.EnglishName.replace(/ /g, ''); // remove white spaces
                if (OpenAmountFilterOperator == "Equals") {
                    this.openAmountFilter = new FilterItem("OpenAmount", searchtext, null, null, OpenAmountFilterOperator, false, false, false, "number", false);
                }
                else if (OpenAmountFilterOperator == "LessThan") {
                    this.openAmountFilter = new FilterItem("OpenAmount", searchtext, null, null, "LessThan", false, false, false, "number", false);
                }
                else if (OpenAmountFilterOperator == "LessThanOrEqual") {
                    this.openAmountFilter = new FilterItem("OpenAmount", searchtext, null, null, "LessThanOrEqual", false, false, false, "number", false);
                }
                else {
                    this.openAmountFilter = new FilterItem("OpenAmount", searchtext, null, null, OpenAmountFilterOperator, false, false, false, "number", false);
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
            }, 700);

        } else {
            if (OperatorChanged == true && AppTool.IsNullOrEmpty(searchtext)) {
                return;
            }
            this.openAmountFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        }

    }
    ForeignAmountTextChanged(searchtext, OperatorChanged: boolean = false) {
        if (!AppTool.IsNullOrEmpty(searchtext) && !AppTool.IsNullOrEmpty(this.ForeignAmountSelectedOperator)) {

            this.timerToken = setTimeout(() => {
                var ForeignAmountFilterOperator = this.ForeignAmountSelectedOperator.EnglishName.replace(/ /g, ''); // remove white spaces
                if (ForeignAmountFilterOperator == "Equals") {
                    this.foreignAmountFilter = new FilterItem("ForeignAmount", searchtext, -1 * searchtext, null, ForeignAmountFilterOperator, false, false, false, "number", false);
                }
                else if (ForeignAmountFilterOperator == "LessThan") {
                    searchtext = Math.abs(searchtext);
                    this.foreignAmountFilter = new FilterItem("ForeignAmount", -1 * --searchtext, +searchtext, null, "Between", false, false, false, "number", false);
                }
                else if (ForeignAmountFilterOperator == "LessThanOrEqual") {
                    searchtext = Math.abs(searchtext);
                    this.foreignAmountFilter = new FilterItem("ForeignAmount", -1 * searchtext, +searchtext, null, "Between", false, false, false, "number", false);
                }
                else {
                    this.foreignAmountFilter = new FilterItem("ForeignAmount", searchtext, null, null, ForeignAmountFilterOperator, false, false, false, "number", false);
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
            }, 700);

        } else {
            if (OperatorChanged == true && AppTool.IsNullOrEmpty(searchtext)) {
                return;
            }
            this.foreignAmountFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        }

    }
    //#endregion

    //#region Buttons Handlers
    IsReconcileButtonClicked: boolean = false;
    ReconcilButton(auto: boolean = false) {
        if (!auto) this.IsReconcileButtonClicked = true;
        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        // Local Validate
        if (this.SelectedLines.Length == 0) {
            errors.push(TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected"));//"No transactions selected
        }

        // lines errors
        if (this.SelectedLines.Collection.find(d => d.isLineValid == false)) {
            // errors.push(TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
            errors.push(TextCodeTranslator.Translate("Reconciliations.O.ErrorsInSelectedLines"));
        }

        // //multiple payment check
        // if (paymentsCount > 1)
        // {
        //     errors.push(TextCodeTranslator.Translate("Accounting.O.CantIncludeTwoOrMorePayment"));
        //     this.ValidationErrorsList = errors;
        //     return;
        // }

        // if (this.SelectedLines.Length > 0 && this.TotalDifference != 0) {
        //     if (this.IsMultiWithReconcileMethodCodeEqualOne) {
        //         errors.push(TextCodeTranslator.Translate("Reconciliations.O.ErrorsInMultiWithRecOne"));
        //     }
        // }

        var ledgerTransactionsPMs = this.GetLedgerTransactionsPMs();
        this.CurrentSession.StartBusyIndicatorSaving();
        this.CurrentSession.StopBusyIndicator();

                this.ValidationErrorsList = errors;
                if (this.ValidationErrorsList.length == 0) {
        
                    //Adjust
                    if (this.SelectedLines.Length > 0 && this.TotalDifference != 0) {


                        if (auto) {
                            this.ValidationErrorsList.push("TotalDifference");      
                            return
                        }

                        this.ShowAdjustmentConfirm();


                    }

                    if (this.SelectedLines.Length > 0 && this.TotalLocalDifference != 0) {
                        let isRFRToggleOnForeignReco: boolean = false;
                        if (this.GLAccountPM.ReconcileMethodCode === "1"){
                            isRFRToggleOnForeignReco = SessionLocator.FeatureToggles.filter(d => d.ToggleCode === "RFR")[0] ? true : false;
                        }
                        if (isRFRToggleOnForeignReco) {
                            var rfrConfirm = new ConfirmWindow();
                            rfrConfirm.Width = 390;
                            rfrConfirm.ShowCancelButton = true;
                            
                            let textMessage = `${TextCodeTranslator.Translate("Accounting.General.O.RFRDiff1")} ${this.TotalLocalDifference} ${SessionLocator.TenantPM.CurrencySign}.\
 ${TextCodeTranslator.Translate("Accounting.General.O.RFRDiff2")}`;

                            rfrConfirm.Show(textMessage);

                            rfrConfirm.WindowClosed.subscribe((event: any) => {
                                if (rfrConfirm.Yes) {
                                    this.revalOnForeignReco = true;  
                                    if (this.TotalDifference === 0) { // No need for adjustment journal
                                        this.RevaluationJournalScreen();
                                    }
                                }
                                if (rfrConfirm.No) {
                                    this.CurrentSession.StartBusyIndicatorSaving();
                                    var entity = this.CreateReconciliation();
                                    this.SubmitChanges(entity);
                                }
                            });
                        }
                    }
                    else {
                        this.CurrentSession.StartBusyIndicatorSaving();
                        var entity = this.CreateReconciliation();
                        this.SubmitChanges(entity);
                    }
                }
        
    }

    private ShowAdjustmentConfirm(): void {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 390;
        confirmWindow.Show(TextCodeTranslator.Translate("Accounting.O.NewReconcileWithAdjusment"));

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.AdjustWithNewJournalScreen();
            } else if (confirmWindow.No) {
                // Do nothing special
            }
        });

        return;
    }


    IsAutoRecClicked: boolean = false;
    AutomaticReconcileButton() {
        this.IsAutoRecClicked = true;
        if (this.SelectedLines.Length > 0) {
            // Show prompt
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 390;
            confirmWindow.Show(TextCodeTranslator.Translate("Accounting.O.AutomaticReconcileWillClearAllSelectedLines"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    for (var i = 0; i < this.SelectedLines.Collection.length; i++) {
                        var line = this.SelectedLines.Collection[i];//new LineModel(result[i], this, -1);
                        this.FireCheckBoxChecked.emit({ rowData: line.LedgerTransactionPM, IsChecked: false, RowIndex: -1, ById: true });
                    }
                    for (var i = 0; i < this.SelectedLines.Collection.length; i++) {
                        var line = this.SelectedLines.Collection[i];
                        this.FireCheckBoxChecked.emit({ rowData: line.LedgerTransactionPM, IsChecked: false, RowIndex: line.myRowIndex, ById: true });
                    }
                    this.SelectedLines.Clear();

                    this.RunAutomaticReconcile();

                } else if (confirmWindow.No) {

                }
            });
        } else {
            this.RunAutomaticReconcile();
        }
    }

    RunAutomaticReconcile() {
        this.ValidationErrorsList = [];



        //#region filters
        var filterAgrs = new ApiQueryFilters;

        if (this.dateFilter) {
            filterAgrs.AdditionalFilters.push(this.dateFilter);
        }

        if (this.currencyFilter) {
            filterAgrs.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filterAgrs.AdditionalFilters.push(this.openAmountFilter);
        }
        if (this.foreignAmountFilter) {
            filterAgrs.AdditionalFilters.push(this.foreignAmountFilter);
        }
        filterAgrs.PageSize = 30;
        filterAgrs.PageIndex = 1; // decremented 1 in the service
        filterAgrs.GetAll = true;
        filterAgrs.GetCount = true;
        //#endregion

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."


        var m1 = null;
        var m2 = null;
        var m3 = null;

        if (!AppTool.IsNullOrEmpty(this.AutomaticReconcileMethodList)) {
            m1 = this.AutomaticReconcileMethodList.AutomaticReconcile1;
            m2 = this.AutomaticReconcileMethodList.AutomaticReconcile2;
            m3 = this.AutomaticReconcileMethodList.AutomaticReconcile3;
        }

        this._LedgerTransactionExtendedListService.getAutomaticReconcileByFilter(m1, m2, m3, this.GLAccountPM.Id, filterAgrs).subscribe((myResult: ServiceResponse) => {

            var mm: ServiceResponse = myResult;
            var result = mm.Result;
            if (!mm.HasError) {

                if (!AppTool.IsNullOrEmpty(result)) {
                    this.SelectedLines.Clear();
                    //this.SelectedLines = [];
                    var array = [];
                    for (var i = 0; i < result.length; i++) {
                        var line = new LineModel(result[i], this, -1);
                        array.push(line);
                        //this.MarkIsChecked.emit({ MyRecord: result[i], AllRecords: result });
                        // this.FireCheckBoxChecked.emit({ rowData: line.LedgerTransactionPM, IsChecked: true, RowIndex: -1, ById: true });
                    }
                    this.SelectedLines.InsertCollection(array);
                    //this.SelectedLines.Length = this.SelectedLines.length;
                    //this.SelectedLines.Changed.emit(this.SelectedLines);

                    if (this.SelectedLines.Length == 0) {
                        this.ShowEmptyAutoReco();
                    }
                    this.CalculateTotals();
                    if (this.isFullAccounting) {
                        this.NumberOfselectedlines = this.SelectedLines.Length;
                        this.NumberOfFilteredlines = this.DataSource.rowCount;
                    }
                }
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    SaveAsDraftButton() {

        if (this.SelectedLines.Length == 0)
            this.ShowNoTransactionsSelectedValidationMessage();
        else
            this.SubmitDraftLedgerTransactions();

    }

    private ShowNoTransactionsSelectedValidationMessage() {
        const noTransactionsSelectedMSG = TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected");
        this.ValidationErrorsList = [noTransactionsSelectedMSG];
    }

    private ShowDraftTransactionsFaiorMessage(error) {
        var msg = new MessageWindow();
        msg.IsMessageMultiLine = true;
        msg.ShowErrorIcon = true;
        msg.RTL = this.isRTL;
        msg.Width = 400;
        msg.Show(error);
    }

    private SubmitDraftLedgerTransactions() {
        var ledgerTransactionsPMs = this.GetLedgerTransactionsPMs();

        this.CurrentSession.StartBusyIndicatorSaving();
        this._ReconciliationExtendedPMService.UpdateDraftReconciliationTransactions(ledgerTransactionsPMs).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.ShowDraftTransactionsSuccessMessage();
        });
    }

    private ShowDraftTransactionsSuccessMessage() {
        var msg = new MessageWindow();
        msg.ShowSuccessIcon = true;
        msg.RTL = this.isRTL;
        msg.Width = 400;
        msg.Show(TextCodeTranslator.Translate("Reconciliations.Q.reconciliationwassavedas"));
        msg.WindowClosed.subscribe((event: any) => {
            this.CancelButtonClicked();
        });
    }

    private GetLedgerTransactionsPMs() {
        var transactions: LineModel[] = this.SelectedLines.Collection;
        var ledgerTransactionsPMs = transactions.map(d => d.LedgerTransactionPM);

        this.ResetArrayOriginalIds(ledgerTransactionsPMs);
        return ledgerTransactionsPMs;
    }

    private ResetArrayOriginalIds(ledgerTransactionsPMs: LedgerTransactionPM[]) {
        for (let i = 0; i < ledgerTransactionsPMs.length; i++) {
            const ledger: any = ledgerTransactionsPMs[i];
            ledger.$id = i;
        }
    }

    AdjustWithNewJournalScreen() {

        if (this.SelectedLines.Length > 0 && this.TotalDifference != 0) {


            var next = true;
            if (next) {
                this.CurrentSession.entityResourceService.getEntityResourceByTableName("Journal").subscribe(response => {
                    this.CurrentSession.entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(response => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 450;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.B.Adjust");
                        logitudeWindow.WindowArgs = {
                            "SelectedLines": this.SelectedLines,
                            "SourceGLAccountPM": this.GLAccountPM,
                            TotalDifference: this.TotalDifference,
                            TotalCredit: this.TotalCredit,
                            TotalDebit: this.TotalDebit,
                            IsMultiWithReconcileMethodCodeEqualOne : this.IsMultiWithReconcileMethodCodeEqualOne
                        };
                        logitudeWindow.Show('./Accounting/Components/Others/JournalReconcileComponent');
                        logitudeWindow.WindowClosed.subscribe(($event: any) => {


                            // Refresh Data
                            this.ReloadScreen();
                            this.SelectedLines.Clear();


                        });
                    });
                });
            }

        } else {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.RTL = this.isRTL;
            myMessageWindow.Show("!(this.SelectedLines.Length > 0 && this.TotalDifference) ");
        }

    }


    RevaluationJournalScreen() {
        if (this.SelectedLines.Length > 0 && this.TotalLocalDifference !== 0) {


                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 300;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.B.RevaluationJournal");
                        logitudeWindow.WindowArgs = {
                            "SourceGLAccountPM": this.GLAccountPM,
                            TotalLocalDifference: this.TotalLocalDifference
                        };
                        logitudeWindow.Show('./Accounting/Components/Others/JournalRevaluationComponent');
                        logitudeWindow.WindowClosed.subscribe(($event: any) => {
                            this.revalJrnlRef1 = $event;  
                            console.log("Returned Reference:", this.revalJrnlRef1);


                                    this.CurrentSession.StartBusyIndicatorSaving();
                                    var entity = this.CreateReconciliation();
                                    this.SubmitChanges(entity);
                        });


        } else {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.RTL = this.isRTL;
            myMessageWindow.Show("!(this.SelectedLines.Length > 0 && this.TotalLocalDifference) ");
        }

    }




    onRowSelected($event) {

        if (this.firstMark) {
            this.firstMark = false;
            this.UpdateIsMark();
        }
        if ($event && GLAccountSecurityLevelService.IsMultiWithReconcileMethodCodeEqualOneParameter && GLAccountSecurityLevelService.IsCheckBoxEnabledParameter) {

            const row = $event.rowData;
            const rowId = row.Id;
            const RowIndex = $event.rowIndex;
            const index = this.SelectedLines.Collection.findIndex(c => c.Id == row.Id);
            if (index < 0) {

                this.PushLine(row, RowIndex);
                this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: true, RowIndex: RowIndex });
            } else {
                this.PopLine(rowId);
                this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: false, RowIndex: RowIndex, ById: true });
            }
            if (this.isFullAccounting) {
                this.NumberOfselectedlines = this.SelectedLines.Length;
                this.NumberOfFilteredlines = this.DataSource.rowCount;
            }


            this.SetUIProperty();
        }
    }

    CancelButtonClicked() {
        if(!this.isMark)
           this.UndoMark();
        this.CurrentSession.CloseCurrentWindow();
    }

    //#endregion

    //#region Grid Data Source
    public _entityListService: EntityListService = new EntityListService();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    dateFilter: FilterItem;
    currencyFilter: FilterItem;
    searchFieldFilter: FilterItem;
    openAmountFilter: FilterItem;
    foreignAmountFilter: FilterItem;

    public columns: any[] = null;
    public QueryColumns: QueryColumnPM[] = [];
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'Indicator',
            DataTypeCode: 'text',
            Display: '',
            Styles: { width: '20px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),//'Acc. Date',
            Styles: { width: '105px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'AccountingDate'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccountingDate", 'DateTime', TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate")));

        this.columns.push({
            FieldName: 'DocumentDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"), //'Ref. Date',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DocumentDate'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DocumentDate", 'DateTime', TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate")));

        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"), // 'Due Date',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DueDate'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DueDate", 'DateTime', TextCodeTranslator.Translate("LedgerTransaction.F.DueDate")));

        this.columns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Source"), // 'Source',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Source'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Source", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.Source")));

        //this.columns.push({
        //    FieldName: 'SourceType',
        //    DataTypeCode: 'String',
        //    Display: 'Source Type',
        //    Styles: { width: '113px' },
        //    IsCustomTemplate: true,
        // ServerSideSortable: true,
        // SortByName: 'AccountingDate'
        //});
        if (this.IsMultiWithReconcileMethodCodeEqualOne) {
            this.columns.push({ // Check ReconcileMethodCode.GLAccounts:
                FieldName: 'AmountInNIS',
                DataTypeCode: 'String',
                //Display: 'Original Amount (' + this.originalAmountCurrency + ')',
                Display: TextCodeTranslator.Translate("LedgerTransaction.F.AmountInNIS"),
                Styles: { width: '150px' },
                HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: 'AmountInNIS',
            });
            this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AmountInNIS", 'Decimal', this.OriginalAmountTextCode));
        }
        this.columns.push({ // Check ReconcileMethodCode.GLAccounts:
            FieldName: 'OriginalAmount',
            DataTypeCode: 'String',
            //Display: 'Original Amount (' + this.originalAmountCurrency + ')',
            Display: this.OriginalAmountTextCode,
            Styles: { width: '150px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'OriginalAmount',
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("OriginalAmount", 'Decimal', this.OriginalAmountTextCode));

        if (this.GLAccountPM.ReconcileMethodCode == "0" && this.GLAccountPM.CurrencyCode != 'NIS') {
            this.columns.push({ // Check ReconcileMethodCode.GLAccounts:
                FieldName: 'ForeignAmount',
                DataTypeCode: 'String',
                //Display: 'Original Amount (' + this.originalAmountCurrency + ')',
                Display: TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmount"),
                Styles: { width: '150px' },
                HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: 'ForeignAmount',
            });
            this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ForeignAmount", 'Decimal', TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmount")));
        }
        //this.columns.push({
        //    FieldName: 'OpenAmountCurrencyCode',
        //    DataTypeCode: 'String',
        //    Display: 'Open Amount Currency',
        //    Styles: { width: '120px' },
        //    IsCustomTemplate: true,
        // ServerSideSortable: true,
        // SortByName: 'AccountingDate'
        //});
        this.columns.push({
            FieldName: 'CurrencyCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.CurrencyId"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CurrencyCode',
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("CurrencyCode", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.CurrencyId")));

        this.columns.push({
            FieldName: 'OpenAmount',
            DataTypeCode: 'String',
            //Display: 'Open Amount (' + this.openAmountCurrency + ')',
            Display: this.OpenAmountTextCode,
            Styles: { width: '114px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'OpenAmount'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("OpenAmount", 'Decimal', this.OpenAmountTextCode));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AmountToReconcile", 'Decimal', TextCodeTranslator.Translate("LedgerTransaction.F.AmountToReconcile")));
        this.columns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"), // 'Ref. 1',
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Reference1'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference1", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.Reference1")));

        this.columns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"), // 'Ref. 2',
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Reference2'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference2", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.Reference2")));

        this.columns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"), // 'Ref. 3',
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Reference3'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference3", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.Reference3")));

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"), // 'Journal No.',
            Styles: { width: '80px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'JournalNumber'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("JournalNumber", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber")));

        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Notes"), // 'Notes',
            Styles: { width: '350px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
            // ServerSideSortable: true,
            // SortByName: 'Notes'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Notes", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.Notes")));

        this.columns.push({
            FieldName: 'InternalNote',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InternalNotes"),
            Styles: { width: '350px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsInternalNotesTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsInternalNotesTemplate',
            IsCustomTemplate: true,
            // ServerSideSortable: true,
            // SortByName: 'Notes'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InternalNote", 'Text', TextCodeTranslator.Translate("ARInvoice.F.InternalNotes")));

        ReconcileEventManager.CheckBoxChecked.subscribe(($event) => {

            if (!AppTool.IsNullOrEmpty($event)) {
                if ($event.SendSessionIndex != this.CurrentSession.SessionIndex)
                    return;
                var params = $event.Params;
                var row = params.line;
                var rowId = params.line.Id;
                var RowIndex = params.RowIndex;
                var isChecked = params.isChecked;
                var oneTime = params.oneTime;
                console.log("---->> Row Selected: ", rowId, row, isChecked);

                if (isChecked) {
                    this.PushLine(row, RowIndex);
                    this.SetUIProperty();
                } else {
                    this.PopLine(rowId);
                }
                if (this.MustIgnoreItems.filter(a => a.Id == rowId).length > 0) {
                    this.MustIgnoreItems = this.MustIgnoreItems.filter(a => a.Id != rowId);
                }
                this.MustIgnoreItems.push({ Id: rowId, IsChecked: isChecked });
                this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });

            }
        });
    }
    GetIndicatorText(transaction) {
        var showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        if ((transaction['LocalAmountDebit'] > 0 && transaction['OpenAmount'] != this.CalculateOriginalAmount(transaction)) || (transaction['LocalAmountCredit'] > 0 && transaction['OpenAmount'] != -1 * this.CalculateOriginalAmount(transaction)))
            return showLocal ? 'סכום פתוח חלקית' : 'Partial transaction';
        else
            return showLocal ? 'סכום פתוח ' : 'Open transaction';
    }
    MustIgnoreItems: any[] = [];
    onDataLoaded() {
        if (this.SelectedLines?.Collection && this.SelectedLines.Collection.length > 0) {
            this.SelectedLines.Collection.forEach(row => {
                if (this.IsReconcileButtonClicked && row?.rowData?.IsChecked) {
                    row.rowData.IsChecked = false;
                }
                if (row?.rowData?.IsChecked) {
                    this.PushLine(row?.rowData, row?.rowIndex);
                }
            });
        }
        if (this.IsReconcileButtonClicked) this.IsReconcileButtonClicked = false;
        this.MarkIsChecked.emit({ SelectedLines: this.SelectedLines });
        this.RaiseEvent();

        if (this.autoReconil) {
            this.autoReconil = false;
            this.ReconcilButton(true);
            const index = this.ValidationErrorsList.findIndex(c => c === "TotalDifference");
            var tetx=TextCodeTranslator.Translate("GLAccounts.O.MarkedByAnother ")
            this.yelloMessage = this.ValidationErrorsList.length > 0 ? TextCodeTranslator.Translate("GLAccounts.O.MarkedByAnother ").split(",")[0] + " -" + TextCodeTranslator.Translate("Reconciliations.O.ReviewAndCompleteReconcile.") :TextCodeTranslator.Translate("GLAccounts.O.MarkedByAnother ").split(",")[0] + " -"+TextCodeTranslator.Translate("Reconciliations.O.ReconciledForOpenTransactions");
            if (index !== -1) {
                this.ValidationErrorsList.splice(index, 1);
            }
            this.CurrentSession.StopBusyIndicator();
        }
    }
    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.filterAgrs = new ApiQueryFilters;

        if (this.dateFilter) {
            this.filterAgrs.AdditionalFilters.push(this.dateFilter);
        }

        if (this.currencyFilter) {
            this.filterAgrs.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            this.filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            this.filterAgrs.AdditionalFilters.push(this.openAmountFilter);
        }
        if (this.foreignAmountFilter) {
            this.filterAgrs.AdditionalFilters.push(this.foreignAmountFilter);
        }

        this.filterAgrs.PageSize = take;
        this.filterAgrs.PageIndex = skip + 1; // decremented 1 in the service
        this.filterAgrs.GetAll = true;
        this.filterAgrs.GetCount = true;

        this.filterAgrs.SortBy = sortingCol;
        this.filterAgrs.SortDirection = sortingDir;

        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");

        return this._entityListService.getOpenReconciliationsByFilter("LedgerTransaction", this.GLAccountPM.Id, this.filterAgrs);//this.ledgerTransactionListExtendedService.getByFilters(filters);      
    }

    OnSortInvoked(event) {
        // this.SelectedLines = new ObservableCollection([]);
    }


    PushLine(row, RowIndex) {
        let countLines: number = 500;

        var index = this.SelectedLines.Collection.findIndex(c => c.Id == row.Id);
        if (index < 0) { // DNE
            row.AmountToReconcile = row.OpenAmount;
            var r = new LineModel(row, this, RowIndex);
            this.SelectedLines.Insert(r);
            //this.SelectedLines.push(r);
            this.CalculateTotals();

            // change max lines if toggle feature is active:
            let selectMoreLines = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SML")[0] ? true : false;
            if (selectMoreLines) {
                countLines = 2000;
            }

            // update select all checkbox
            if (this.SelectedLines.Length >= this.DataSource.rowCount || this.SelectedLines.Length >= countLines)
                this._isAllSelected = true;
            if (this.isFullAccounting) {
                this.NumberOfFilteredlines = this.DataSource.rowCount;
                this.NumberOfselectedlines = this.SelectedLines.Length;
            }
        }
    }

    PopLine(id) {
        //var index = this.SelectedLines.findIndex(c => c.Id == id);
        //this.SelectedLines.splice(index, 1);
        this.SelectedLines.Remove(this.SelectedLines.Collection.find(c => c.Id == id));
        //ReconcileEventManager.RowUnselected.emit({ id: id });
        this.CalculateTotals();
        this._isAllSelected = false;
        if (this.isFullAccounting) {
            this.NumberOfFilteredlines = this.DataSource.rowCount;
            this.NumberOfselectedlines = this.SelectedLines.Length;
        }
    }

    CheckBoxValueChanged(Row) {

        this.PopLine(Row.LedgerTransactionPM.Id);
        this.FireCheckBoxChecked.emit({ rowData: Row.LedgerTransactionPM, IsChecked: false, RowIndex: Row.myRowIndex, ById: true });
    }

    CalculateOriginalAmount(row: LineModel) {
        if (!AppTool.IsNullOrEmpty(this.GLAccountPM.ReconcileMethodCode)) {

            if (this.GLAccountPM.ReconcileMethodCode == "0") { // 0-local currency

                if (AppTool.IsNullOrZero(row.LocalAmountCredit)) {
                    return row.LocalAmountDebit;
                } else {
                    return row.LocalAmountCredit; //-1 *
                }

            } else if (this.GLAccountPM.ReconcileMethodCode == "1") { // 1-foreign currency

                if (AppTool.IsNullOrZero(row.ForeignAmountCredit)) {
                    return row.ForeignAmountDebit;
                } else {
                    return row.ForeignAmountCredit; //-1 *
                }

            }

        }
    }

    ReloadScreen() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid

        this.CalculateTotals();
    }
    //#endregion

    //#region Totals Work
    TotalCredit: number = 0;
    TotalDebit: number = 0;
    TotalDifference: number = 0;

    TotalCurrCredit: number = 0;
    TotalCurrDebit: number = 0;
    TotalCurrDifference: number = 0;

    TotalLocalCredit: number = 0;
    TotalLocalDebit: number = 0;
    TotalLocalDifference: number = 0;

    OriginalDifference: number = 0;

    CalculateTotals() {
        this.TotalCredit = 0;
        this.TotalDebit = 0;
        this.TotalDifference = 0;

        this.TotalCurrCredit = 0;
        this.TotalCurrDebit = 0;
        this.TotalCurrDifference = 0;

        this.TotalLocalCredit = 0;  
        this.TotalLocalDebit = 0;
        this.TotalLocalDifference = 0;

        this.OriginalDifference = 0;
        
        for (let line of this.SelectedLines.Collection) {
            let rate: number = +line.ledgerTransaction.ExchangeRate;;
            let lineCurrAmountToReconcile: number = 0;  // if GLAccountPM.CurrencyId !== TenantPM.CurrencyId  => lineCurrAmountToReconcile is in GLAccountPM.CurrencyId 
            let lineLocalAmountToReconcile: number = 0; // if GLAccountPM.ReconcileMethodCode === "1"  => lineLocalAmountToReconcile is in TenantPM.CurrencyId
            if (rate !== 0 && !AppTool.IsNullOrEmpty(this.GLAccountPM.ReconcileMethodCode) && this.GLAccountPM.ReconcileMethodCode === "0" // NIS
                && !AppTool.IsNullOrEmpty(this.GLAccountPM.CurrencyId) && this.GLAccountPM.CurrencyId !== this.TenantPM.CurrencyId) {     // GLAcc is not multi, not NIS (say, USD)
                lineCurrAmountToReconcile = +line.AmountToReconcile / rate;  // lineCurrAmountToReconcile in USD, because GLAcc is USD, so all lines are
                lineLocalAmountToReconcile = +line.AmountToReconcile; // lineLocalAmountToReconcile is already in NIS
            }
            else if (rate !== 0 && !AppTool.IsNullOrEmpty(this.GLAccountPM.ReconcileMethodCode) && this.GLAccountPM.ReconcileMethodCode === "1" // let's say, USD
                && !AppTool.IsNullOrEmpty(this.GLAccountPM.CurrencyId) && this.GLAccountPM.CurrencyId !== this.TenantPM.CurrencyId) {     // GLAcc is not multi, not NIS (say, USD)
                lineCurrAmountToReconcile = +line.AmountToReconcile;  // lineCurrAmountToReconcile is already in USD 
                lineLocalAmountToReconcile = +line.AmountToReconcile * rate; // lineLocalAmountToReconcile in NIS
            }
            else {
                lineCurrAmountToReconcile = +line.AmountToReconcile;
                lineLocalAmountToReconcile = +line.AmountToReconcile;
            }


            if (line.AmountToReconcile < 0) {
                this.TotalCredit += +line.AmountToReconcile * -1; //cast number
                this.TotalCurrCredit += +lineCurrAmountToReconcile * -1; //cast number
                this.TotalLocalCredit += +lineLocalAmountToReconcile * -1; //cast number
            }
            else {
                this.TotalDebit += +line.AmountToReconcile;
                this.TotalCurrDebit += +lineCurrAmountToReconcile;
                this.TotalLocalDebit += +lineLocalAmountToReconcile;
            }


            // due this.TotalCredit + amountToReconcile;  == 335.78999999999996 <>335.79
            this.TotalDebit = AppTool.Round(this.TotalDebit, 2);
            this.TotalCredit = AppTool.Round(this.TotalCredit, 2);
            this.TotalCurrDebit = AppTool.Round(this.TotalCurrDebit, 2);
            this.TotalCurrCredit = AppTool.Round(this.TotalCurrCredit, 2);
            this.TotalLocalDebit = AppTool.Round(this.TotalLocalDebit, 2);
            this.TotalLocalCredit = AppTool.Round(this.TotalLocalCredit, 2);


        }
        var dif = (this.TotalCredit - this.TotalDebit)
        var currdif = (this.TotalCurrCredit - this.TotalCurrDebit)
        var localdif = (this.TotalLocalCredit - this.TotalLocalDebit)

    
        const round2 = (n: number) => Number(n.toFixed(2));

        this.OriginalDifference = round2(dif * -1);
        this.TotalDifference = round2(Math.abs(dif));
        this.TotalCurrDifference = round2(Math.abs(currdif));    // if GLAccountPM.CurrencyId != TenantPM.CurrencyId  => TotalCurrDifference is in GLAccountPM.CurrencyId 
        this.TotalLocalDifference = round2(Math.abs(localdif));  // if GLAccountPM.ReconcileMethodCode === "1"  => TotalLocalDifference is in TenantPM.CurrencyId 
    
    }
    //#endregion

    GetFirst500LedgerForReconciliation() {
        this.ValidationErrorsList = [];
        var filters = this.GetAPIFilters();
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
        this._LedgerTransactionExtendedListService.GetFirst500LedgerForReconciliation(this.GLAccountPM.Id, filters).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            var first100Transactions = mm.Result;
            if (!mm.HasError) {
                if (!AppTool.IsNullOrEmpty(first100Transactions)) {
                    this.SelectLines(first100Transactions);
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


    GetFirstXLedgerForReconciliationByParam() {
        this.ValidationErrorsList = [];
        var filters = this.GetAPIFilters();
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
        this._LedgerTransactionExtendedListService.GetFirstXLedgerForReconciliationByParam(this.GLAccountPM.Id, filters).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            var first100Transactions = mm.Result;
            if (!mm.HasError) {
                if (!AppTool.IsNullOrEmpty(first100Transactions)) {
                    this.SelectLines(first100Transactions);
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

    public SelectLines(result: any) {
        this.SelectedLines.Clear();
        let emittedArray = result.map((res: LedgerTransactionPM) => ({ rowData: res, IsChecked: true, RowIndex: -1, ById: true })); //result.map(res=>(new LineModel(res,this,-1)));//[];
        let selectedLines = result.map(res => (new LineModel(res, this, -1))); //[];
        this.SelectedLines.InsertCollection(selectedLines);
        this.ChangeCheckBoxesState.emit(emittedArray);
    }

    public GetAPIFilters() {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }
        if (this.foreignAmountFilter) {
            filters.AdditionalFilters.push(this.foreignAmountFilter);
        }
        filters.PageSize = 2000;
        filters.PageIndex = 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;
        filters.SortBy = this.DataSource.sortingCol;
        filters.SortDirection = this.DataSource.sortingDir;
        return filters;
    }

    CreateReconciliation() {
        var newEntity: any = {};

        newEntity.Id = "new";
        newEntity.ChangeSetOp = 1;
        newEntity.Tenant = SessionLocator.Tenant;
        newEntity.AccountId = this.GLAccountPM.Id
        newEntity.Number = "get";
        newEntity.CreateDate = new Date();
        newEntity.CreatedByUserId = null;
        newEntity.CreatedByUserId = null;
        if (this.IsMultiWithReconcileMethodCodeEqualOne) {
            newEntity.AccountCurrencyId = this.CurrencyId;
        } else {
            newEntity.AccountCurrencyId = this.GLAccountPM.CurrencyId;
        }
        newEntity.CurrencyCode = this.GLAccountPM.CurrencyCode;
        newEntity.AccountReconcileMethodCode = this.GLAccountPM.ReconcileMethodCode;
        newEntity.RevalOnForeignReco = this.revalOnForeignReco;
        newEntity.RevalJrnlRef1 = this.revalJrnlRef1;
        newEntity.ReconciliationLines = [];

        for (var i = 0; i < this.SelectedLines.Length; i++) {
            var selectedTransaction = this.SelectedLines.Collection[i];
            var newLine: any = {};
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = newEntity.Id;
            newLine.Tenant = newEntity.Tenant;
            newLine.Line = i;
            newLine.CurrencyId = selectedTransaction.OpenAmountCurrencyId;
            newLine.TransactionId = selectedTransaction.Id;
            newLine.CurrencyRate = selectedTransaction.ExchangeRate;
            newLine.ReconciliationAmount = selectedTransaction.AmountToReconcile;
            newLine.DueDate = selectedTransaction.DueDate;
            newLine.IsPartial = selectedTransaction.IsPartial;
            newLine.GroupNumber = selectedTransaction.GroupHash;

            newEntity.ReconciliationLines.push(newLine);
        }
        return newEntity;
    }
    SubmitChanges(entity) {
        // var sssss:RecoCallback = {reconciliationPM: null, splittedRecoCount: 0,isSplitted: false,communicationLogId: '1-1147'};
        // this.getReconciliationCommunicationLog(sssss);
        // return;
        SessionLocator.SelectedSession.StartBusyIndicator("Saving");

        this._ReconciliationExtendedPMService.insert(entity).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            var _callback: RecoCallback = mm.Result;

            if (_callback && _callback.communicationLogId && _callback.communicationLogId != null) {
                if (this.IsReconcileButtonClicked) this.IsReconcileButtonClicked = false;
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.getReconciliationCommunicationLog(_callback);
            }
            else {

                if (!mm.HasError) {

                    //});

                    this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
                    if (_callback) {
                        this.RecoPM = _callback.reconciliationPM;
                    }

                    this.recoCallback = _callback;
                    this.ShowSuccessAlert();

                    this.SelectedLines.Clear();


                    this.CurrentSession.StopBusyIndicator();

                    this.ReloadScreen();
                    //this.SelectedLines.Clear();
                    this.CalculateTotals();

                }

                else {
                    if (this.IsReconcileButtonClicked) this.IsReconcileButtonClicked = false;

                                       
                    this.ValidationErrorsList = mm.ErrorsArray;  
                    SessionLocator.SelectedSession.StopBusyIndicator();
                  
                    if (mm.ErrorsArray.length > 0 && mm.ErrorsArray.some(e => e.includes("GLAccounts.O.MarkedByAnother"))) {
                        this.ValidationErrorsList = mm.ErrorsArray.map(error =>
                            error === "GLAccounts.O.MarkedByAnother" ? TextCodeTranslator.Translate("GLAccounts.O.MarkedByAnother ") : error
                        );
                        this.SelectedLines.Clear();
                        this.ReloadScreen()
                        this.autoReconil = true
                    }




                }
            }

        });
    }

    getReconciliationCommunicationLog(_callback: RecoCallback) {
        SessionLocator.SelectedSession.StartBusyIndicator("Reconciliation in progress");
        this._ReconciliationExtendedPMService
            .getCommunicationLog(_callback.communicationLogId)
            .pipe(
                delay(3000),
                expand((response: any) => {
                    if ((response.HasError || response.Result.CommunicationStatusTypeCode === 'W') && !this.IsComponentDestroyed) {
                        return this._ReconciliationExtendedPMService
                            .getCommunicationLog(_callback.communicationLogId).pipe(delay(3000));
                    }
                    return EMPTY;
                }),
                takeLast(1)
            )
            .subscribe((result: ServiceResponse) => {
                if (!result.HasError) {
                    var communicationLogResult = result.Result;
                    if (communicationLogResult.CommunicationStatusTypeCode === 'D' && communicationLogResult.AdditionalFields && communicationLogResult.AdditionalFields != null) {
                        _callback = JSON.parse(communicationLogResult.AdditionalFields);
                        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
                        if (_callback) {
                            this.RecoPM = _callback.reconciliationPM;
                        }
                        this.recoCallback = _callback;
                        this.ShowSuccessAlert();
                        this.SelectedLines.Clear();
                        this.CurrentSession.StopBusyIndicator();
                    } else if (communicationLogResult.CommunicationStatusTypeCode === 'F') {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(communicationLogResult.ExceptionMessage);
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
                else {
                    this.ValidationErrorsList = result.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

            })
    }

    OpenSource(id: string, sourceTypeCode: string) {

        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = AccountingEntityHelper.getEntityObjectTableName(sourceTypeCode);


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
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if(this.failedJournalsInReconcileProcess)
                            this.GetFailedJournalsInReconcileProcess();

                    });
                });
        }
    }
    OpenReco() {
        if (!AppTool.IsNullOrEmpty(this.RecoPM.Id)) {
            this.showAlert = false;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.RecoPM.Id, ObjectTableName: 'Reconciliation', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.CurrentSession.CloseCurrentWindow();
                    });
                });

        }
    }

    showAlert: boolean = false;
    ShowSuccessAlert() {

        this.ReloadScreen();
        this.showAlert = true;
        this.timerToken = setTimeout(() => {
            this.showAlert = false;
        }, 5000); // 5 sec

    }
    ShowEmptyAutoReco() {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 400;
        messageWindow.Height = 150;
        messageWindow.RTL = this.isRTL;
        messageWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.NoReconcile");
        messageWindow.Show(TextCodeTranslator.Translate("Accounting.O.NoReconciliationFound"));
    }
    CloseAlert() {
        this.showAlert = false;
    }

    CloseGetInternalReconcileAPPaymentAlert() {
        this.showInternalReconcileAPPaymentAlert = false;
    }

    openAmountCurrency: string = "";
    originalAmountCurrency: string = "";

    //#region SaveAsDraft
    isDraftReconciliation: boolean = false;
    CheckBoxFilterChanged: EventEmitter<any> = new EventEmitter();
    CheckIfThereIsDraftReconcile() {

        this._ReconciliationExtendedPMService.getDraftReconciliations(this.GLAccountPM.Id).subscribe((response: ServiceResponse) => {
            console.log("_ReconciliationExtendedPMService.getDraftReconciliations", response);
            var list = response.Result;

            if (list && list.length > 0) {

                var array = [];
                for (var i = 0; i < list.length; i++) {
                    var line = new LineModel(list[i], this, -1);
                    array.push(line);
                }
                this.SelectedLines.InsertCollection(array);
                this.CalculateTotals();
                this.isDraftReconciliation = true;

                //set first grid checkboxs
                //this.SelectedLines.Collection.forEach((elem: LineModel) => {
                //    this.FireCheckBoxChecked.emit({ rowData: elem.LedgerTransactionPM, IsChecked: true, RowIndex: elem.RowIndex });
                //    console.log("event fired for:", { rowData: elem.LedgerTransactionPM, IsChecked: true, RowIndex: elem.RowIndex } );
                //});
                //this.CheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue:"true"});


                // Show confirm window
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Show(TextCodeTranslator.Translate("Reconciliations.Q.ThereisUncompletedReconciliation"));

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        if (this.IsAutoRecClicked == true) {
                            this.IsAutoRecClicked = false;
                        }
                        this.IsDraft = true;
                    } else if (confirmWindow.No) {
                        // Delete draft transactions
                        this.DeleteDraftReconciliation();
                        this.IsDraft = false;

                    }
                });
            }
        });


    }
    IsDraft: boolean = false;
    DeleteDraftReconciliation() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._ReconciliationExtendedPMService.deleteResetDraftOpenReconciliation(this.GLAccountPM.Id).subscribe((response: ServiceResponse) => {
            console.log("_ReconciliationExtendedPMService.deleteResetDraftOpenReconciliation", response);
            this.CurrentSession.StopBusyIndicator();
            this.SelectedLines.Clear();
            this.ReloadScreen();
        });

    }
    //#endregion


    public _showMoreFilters: boolean;
    public get showMoreFilters(): boolean {
        return this._showMoreFilters;
    }
    public set showMoreFilters(v: boolean) {
        this._showMoreFilters = v;
    }

    public DateFilterPresetsList: any[] = [];
    public DateTypes: any[] = [];
    public selectedDatePreset: any;
    get SelectedDatePreset() { return this.selectedDatePreset; }
    set SelectedDatePreset(value: any) {
        if (this.selectedDatePreset != value) {
            this.selectedDatePreset = value;

            this.ChangeDate();
            this.FiltersChanged();
            if (this.isFullAccounting) {
                this.NumberOfFilteredlines = this.DataSource.rowCount;
                this.NumberOfselectedlines = this.SelectedLines.Length;
            }
        }
    }
    ChangeDate() {
        if (this.SelectedDatePreset) {
            this.DisableDates();
            this.SetDatesFieldsByPreset();
        }
        else {
            this.FromDate = null;
            this.ToDate = null;
        }
        this.ReloadScreen();
    }


    fromDate: Date;
    public SetDatesFieldsByPreset() {
        var datesHelper = new DatesHelper();
        switch (this.SelectedDatePreset.EnglishName) {
            case "Today":
                {
                    this.FromDate = datesHelper.TodayDate;
                    this.ToDate = datesHelper.TomorrowDate;
                    break;
                }
            case "Yesterday":
                {
                    this.FromDate = datesHelper.YesterdayDate;
                    this.ToDate = datesHelper.TodayDate;
                    break;
                }
            case "Last 7 days":
                {
                    this.FromDate = datesHelper.LastSevenDaysDate;
                    this.ToDate = datesHelper.TomorrowDate;
                    break;
                }
            case "Last month":
                {
                    this.FromDate = datesHelper.LastThirtyDaysDate;
                    this.ToDate = datesHelper.TomorrowDate;
                    break;
                }
            case "Last 3 months":
                {
                    this.FromDate = datesHelper.LastThreeMonthDate;
                    this.ToDate = datesHelper.TomorrowDate;
                    break;
                }
            case "Current year":
                {
                    this.FromDate = datesHelper.CurrentYearFromDate;
                    this.ToDate = datesHelper.CurrentYearToDate;
                    break;
                }
            case "Last year":
                {
                    this.FromDate = datesHelper.LastYearFromDate;
                    this.ToDate = datesHelper.LastYearToDate;
                    break;
                }
            case "Custom":
                {
                    this.FromDate = null;
                    this.ToDate = null;
                    this.EnableDates();
                    this.CD.detectChanges();
                    break;
                }
        }
    }

    public EnableDates() {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, true);
    }
    public DisableDates() {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, false);
    }

    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate))
                this.dateFilter = new FilterItem(this.SelectedDateType.FieldName, new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            else
                this.dateFilter = null;
            this._isAllSelected = false;
            this.ReloadScreen();
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate))
                this.dateFilter = new FilterItem(this.SelectedDateType.FieldName, new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            else
                this.dateFilter = null;
            this._isAllSelected = false;
            this.ReloadScreen();
        }
    }

    FiltersChanged() {
        var t = setTimeout(() => {

        }, 700);
    }


    public get areFiltersSelected(): boolean {
        var selected =
            (!AppTool.IsNullOrEmpty(this.OpenAmount)
                || !AppTool.IsNullOrEmpty(this.ForeignAmount)
                || (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)));
        if (this.isFullAccounting) {
            this.NumberOfFilteredlines = this.DataSource.rowCount;
            this.NumberOfselectedlines = this.SelectedLines.Length;
        }
        return selected;
    }

    ResetFilters() {
        this.ForeignAmount = null;
        this.OpenAmount = null;
        this.FromDate = null;
        this.ToDate = null;
        this.openAmountSelectedOperator = this.Operators[0];
        this.foreignAmountSelectedOperator = this.Operators[0];
        this.openAmountFilter = null;
        this.foreignAmountFilter = null;

        this.SelectedDatePreset = null;
        this.DisableDates();

        this.ReloadScreen();
    }

    public get EnableCreateAPPaymentButton(): boolean {
        return this.TotalDifference >= 0 && this.SelectedLines.Length > 0;
    }

    CreatePaymentButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.OriginalDifference >= 0) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("APPayment.M.InvalidSelectedTransctionsDifference"));
        }

        if (AppTool.IsNullOrEmpty(this.GLAccountPM.CardId) && AppTool.IsNullOrEmpty(this.GLAccountPM.ParentCurrencyGLAccountCardId)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("TaxDeductionReport.O.AccountWithoutVendor"));
        }

        if (this.ValidationErrorsList.length == 0) {
            this.NewAPPaymentMethod();
        }
    }

    NewAPPaymentMethod() {
        var newApPaymentPM: APPaymentPM = new APPaymentPM();
        let paymcurr: string = this.CurrencyId != null ? this.CurrencyId : this.TenantPM.CurrencyId;

        // let recocurr: string = this.TenantPM.CurrencyId;
        // if (!AppTool.IsNullOrEmpty(this.GLAccountPM.ReconcileMethodCode) && this.GLAccountPM.ReconcileMethodCode == "1")
        //     recocurr = this.GLAccountPM.CurrencyId;  

        newApPaymentPM.ReconcileInternalTrans = this.GetSelectedPageLines();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        console.log('this.GLAccountPM', this.GLAccountPM);
        newApPaymentPM.VendorId = this.GLAccountPM.CardId != null ? this.GLAccountPM.CardId : this.GLAccountPM.ParentCurrencyGLAccountCardId;
        newApPaymentPM.AmountInLocalCurrency = this.TotalDifference;
        newApPaymentPM.AmountInPaymentCurrency = paymcurr != this.TenantPM.CurrencyId ? this.TotalCurrDifference : this.TotalDifference;
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();

        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.DontDisplayAPInvoices = true;
        newApPaymentPM.PaymentCurrencyId = paymcurr;
        newApPaymentPM.IsFromReconcilePage = true;
        newApPaymentPM.IsMultiCurrency = this.GLAccountPM.IsMultiCurrency;
        if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
            newApPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        }
        this.showAPPaymentEditcomponent(newApPaymentPM);
    }

    private showAPPaymentEditcomponent(newApPaymentPM: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, ObjectTableName: 'APPayment' });
                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                    // this.LoadAllScreenData();
                });

            });
    }

    private GetSelectedPageLines(): LedgerTransactionPM[] {
        return this.SelectedLines.Collection.map((x, index) => {
            x.LedgerTransactionPM.$id = index + 1;
            return x.LedgerTransactionPM as LedgerTransactionPM
        });
    }

    public GetInternalReconcileAPPaymentAlertMessage() {
        return TextCodeTranslator.Translate('APPayment.M.PaymentCreatedWithReconciliation').replace('#number', this.createdPaymentNumber);
    }
    public failedJournalList: any = null;
    public GetFailedJournalsInReconcileProcess(){
        this.failedJournalsInReconcileProcess = false;
        const journalExtendedPMService : JournalExtendedPMService = new JournalExtendedPMService();
        journalExtendedPMService.GetFailedJournalsInReconcileProcess(this.GLAccountPM.Id).subscribe((response: ServiceResponse) => {
            this.failedJournalList = response.Result;
            if (this.failedJournalList && this.failedJournalList.length > 0) {
                this.failedJournalsInReconcileProcess = true;
            }
        });
    }

}

export class DatesHelper {
    constructor() {

        this.InitDates();
    }

    public TomorrowDate: Date;
    public TodayDate: Date;
    public YesterdayDate: Date;
    public LastSevenDaysDate: Date;
    public LastThirtyDaysDate: Date;
    public LastThreeMonthDate: Date;
    public CurrentYearFromDate: Date;
    public CurrentYearToDate: Date;
    public LastYearFromDate: Date;
    public LastYearToDate: Date;


    public InitDates() {
        this.TomorrowDate = this.GetTomorrowDate();
        this.TodayDate = this.GetTodayDate();
        this.YesterdayDate = this.GetNewDateWithAddedDays(-1);
        this.LastSevenDaysDate = this.GetNewDateWithAddedDays(-7);
        this.LastThirtyDaysDate = this.GetNewDateWithAddedDays(-30);
        this.LastThreeMonthDate = this.GetNewDateWithAddedDays(-90);
        this.CurrentYearToDate = this.GetNewDateWithAddedDays(1);
        this.LastYearFromDate = this.GetNewDateWithAddedDays(-365);
        this.LastYearToDate = this.GetNewDateWithAddedDays(1);
        this.CurrentYearFromDate = this.GetCurrentYearDate();
    }

    public GetTomorrowDate() {
        var date = new Date();
        date.setHours(23, 59, 59, 59);
        return date;
    }

    public GetTodayDate() {
        var date = new Date();
        this.ResetHours(date);
        return date;
    }

    public GetCurrentYearDate() {
        var date = new Date(new Date().getFullYear(), 0, 1);
        this.ResetHours(date);
        return date;
    }

    public GetNewDateWithAddedDays(daysToAdd: number) {
        var date = DateTool.AddDays((new Date()), daysToAdd);
        this.ResetHours(date);
        return date;
    }

    public ResetHours(date: Date) {
        date.setUTCHours(0, 0, 0, 0);
    }
}
