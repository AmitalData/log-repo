import { ExternalPageAdditionalDataPM } from './../../../EntityPMs/ExternalPageAdditionalDataPM';
import { ExternalPageAdditionalDataPMService } from './../../../Services/StandardPMs/ExternalPageAdditionalDataPMService';
import { LedgerTransactionExtendedListService } from './../../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ServiceResponse } from './../../../../Infrastructure/DataContracts/ServiceResponse';
declare var window: any;
import {Component, OnInit, Output, EventEmitter, AfterViewInit, OnDestroy, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BankAccountPM} from '../../../EntityPMs/BankAccountPM';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {ReconcileExternalPageExtendedPMService} from '../../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import {ReconcileExternalPagePMService} from '../../../Services/StandardPMs/ReconcileExternalPagePMService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {BankAccountPMService} from '../../../Services/StandardPMs/BankAccountPMService';
import { reject } from 'q';

@Component({
    
    templateUrl: './ExternalPagesTabComponent.html'
})

export class ExternalPagesTabComponent extends BaseComponent implements OnInit, OnDestroy {

    public DataContext = this;
    public ObjectTableName: string;
    public ObjectTable: any;
    public EntityPM: any = null;
    public ReconcilePageTable = "ReconcileExternalPage";

    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _entityListService: EntityListService = new EntityListService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _BankAccountPMService: BankAccountPMService = new BankAccountPMService();
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();

    private CurrentSession = SessionLocator.SelectedSession;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;
    preventSelect: boolean = false;
    public isRTL: boolean = false;
    public Title: string = "";
    public DontShowLocal: boolean = false;



    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef)
    {
        super();

        this.LoadResources();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.DontShowLocal = SessionLocator.LoggedUserPM.DontShowLocal;
        this.SetComponentArgs(entityArgs);

        this.SetUIProperties();
        this.InitializeDates();
        this.Listen();
    }




    private SetComponentArgs(entityArgs: EntityArgs)
    {
        console.log("[ExternalPagesTabComponent.Args] ", entityArgs);

        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;

        this.ObjectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (!this.ObjectTable)
            console.log("[ERROR] no object table found! ");

        this.GetAdditionalData(this.ObjectTable.Id, this.EntityPM.Id)


        this.SetTitles();


    }

    private SetTitles()
    {
        var title = "";

        switch (this.ObjectTableName) {
            case "BankAccount":
                title = TextCodeTranslator.Translate("BankAccount.TH.BankPages");
                break;
            case "GLAccount":
                title = TextCodeTranslator.Translate("GLAccount.TH.ExternalTransactions");
                break;
            default:
                title = "External Pages";
                break;
        }

        this.Title = title;
    }

    private InitializeDates()
    {
        var today = new Date();
        this.ToDate = new Date();
        this.oldToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        this.oldFromDate = new Date(lastmonth);
    }

    private LoadResources()
    {
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { });
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }


        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res == "noselect") {
                this.preventSelect = true;
            }
        });

    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    ngOnInit() {
        this.BuildColumns();
        this.ReloadData();
    }

    //#region Properties


    //deferredGLAccount: GLAccountPM;
    //get DeferredGLAccount() { return this.deferredGLAccount; }
    //set DeferredGLAccount(value: GLAccountPM) {
    //    if (this.deferredGLAccount != value) {
    //        this.deferredGLAccount = value;
    //    }
    //}

    //#endregion

    SetUIProperties() {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}

    }

    //#region Filters Properties
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.oldFromDate = this.fromDate;
            this.fromDate = value;

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.oldToDate = this.toDate;
            this.toDate = value;

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }
        }
    }
    //#endregion

    //#region Date Filters Validation
    oldToDate: Date;
    oldFromDate: Date;
    isValidate: boolean = false;
    validateDates() {
        if (this.FromDate >= this.ToDate) {

            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ReconcilePageTable, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.UIProperties.SetValidity("FromDate", this.ReconcilePageTable, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);


        } else {
            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ReconcilePageTable, true, "");
                this.UIProperties.SetValidity("FromDate", this.ReconcilePageTable, true, "");
                this.CD.detectChanges();
            }, 200);

            this.LoadData();

        }
    }

    //load data after validate date
    LoadData() {
        if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
            this.dateFilter = new FilterItem("FromDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            this.RefreshButtonClicked();

        }
    }
    //#endregion

    //#region Search
    private timerToken: any;
    TextChanged(searchtext) {
        if (!AppTool.IsNullOrEmpty(searchtext)) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.RefreshButtonClicked();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    }
    //#endregion

    //#region Data
    public columns: any[] = null;

    ReloadData() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

        this.GetAdditionalData(this.ObjectTable.Id, this.EntityPM.Id)

    }

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'PageNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo"),
            Styles: { width: '95px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'FromDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.FromDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ToDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.ToDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StartBalance',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.StartBalance"),
            Styles: { width: '105px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CloseBalance',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.CloseBalance"),
            Styles: { width: '105px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.CreateDate"),
            Styles: { width: '115px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreatedByUserName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.CreatedByUserName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: this.DontShowLocal ? 'StatusName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.StatusName"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: this.DontShowLocal ? 'EntryTypeEnglishName' : 'EntryTypeLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.EntryTypeEnglishName"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Event',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("AccountingPeriod.TH.Events"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });

        //this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        //#region Filters
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        } else {
            return;
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }

        filters.GetCount = true;

        filters.PageSize = take;
        filters.PageIndex = skip;

        filters.SortBy = "PageNo"; //FromDate
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("ObjectTableId", this.ObjectTable.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("EntityId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        //#endregion

        return this._entityListService.getByFilters("ReconcileExternalPage", filters);

    }

    //#endregion

    //#region Buttons





    RefreshButtonClicked() {
        this.ReloadData();
    }
    AddButtonClicked() {
        this.OpenExternalPageWindow();
    }
    //#endregion

    onRowSelected(item)
    {
        if (!this.preventSelect) {
            if (!AppTool.IsNullOrEmpty(item)) {
                var entity = item.rowData;
                var entityId = entity.Id;

                this.OpenExternalPageWindow(entity);
            }

        }
        this.preventSelect = false;
    }

    IsRestoreButtonVisibile: boolean = false;
    message: string = null;
    RestoreToolTipMessage: string = null;
    OpenExternalPageWindow(externalPage: any = null) {

        if (externalPage)
            this.OpenEditExternalPageWindow(externalPage);
        else
            this.OpenNewExternalPageWindow();
    }

    private OpenEditExternalPageWindow(externalPage: any)
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        var externalPagePM;
        this._ReconcileExternalPagePMService.get(externalPage.Id).subscribe((myResult:any) =>
        {
            externalPagePM = myResult.Result;
            this._ReconcileExternalPageExtendedPMService.CheckLastApprovedBankPageAndReconciledLine(externalPage.Id, this.ObjectTableName).subscribe((myResult:any) =>
            {
                if (!myResult.HasError) {
                    if (myResult.Result == null) {
                        this.EnableReconcileEditButton = true;
                        this.message = null;
                    }
                    else {
                        this.EnableReconcileEditButton = false;
                        this.message = myResult.Result;
                    }

                    if (externalPagePM.StatusCode == "3") {
                        this.CheckRestorePossibility(externalPage.Id, externalPagePM);
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();

                        this.IsRestoreButtonVisibile = false;
                        this.RestoreToolTipMessage = null;
                        this.ShowExternalPageWindow(externalPagePM);
                    }
                }
            });
        });
    }

    private OpenNewExternalPageWindow()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EnableReconcileEditButton = false;
        this._ReconcileExternalPageExtendedPMService.GetDraftPage(this.EntityPM.Id, this.ObjectTableName).subscribe((myResult:any) =>
        {
            this.CurrentSession.StopBusyIndicator();
            var draftPage = myResult.Result;
            if (!AppTool.IsNullOrEmpty(draftPage)) {
                this.ShowCannotCreateNewPageMessage(draftPage);
            }
            else {
                this.ShowExternalPageWindow();
            }
        });
    }

    private ShowCannotCreateNewPageMessage(draftPage: any)
    {
        var msg = new MessageWindow();
        //msg.Title = "Error";
        msg.Width = 360;
        msg.RTL = this.isRTL;
        //msg.ShowErrorIcon = true;
        msg.Show(TextCodeTranslator.Translate("ReconcileExternalPage.O.CantNewBankPageDraft") + " (" + draftPage.PageNo + ") ");
    }

    CheckRestorePossibility(id: string, entityPM: any) {

        this._ReconcileExternalPageExtendedPMService.CheckRestorePossibility(id).subscribe((response:ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (!response.HasError) {
                this.SetRestoreButtonVisibility(response.Result)


                this.ShowExternalPageWindow(entityPM);
            }

        });

    }

    SetRestoreButtonVisibility(result: any) {

        if (result == null) {
            this.IsRestoreButtonVisibile = true;
            this.IsRestoreButtonEnabled = true;
            this.RestoreToolTipMessage = null;
        }
        else {
            this.RestoreToolTipMessage = result;
            this.IsRestoreButtonEnabled = false;
            this.IsRestoreButtonVisibile = true;
        }
    }
    IsRestoreButtonEnabled: boolean;
    EnableReconcileEditButton: boolean = false;

    private ShowExternalPageWindow(externalPage: any = null)
    {
        var entity = this.EntityPM;
        var windowArgs: any = {};
        var windowTitle = externalPage ? (TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + externalPage.PageNo) : TextCodeTranslator.Translate("Accounting.General.O.NewPage");

        if (this.ObjectTableName == "BankAccount") {
            windowArgs.GLAccountId = entity.GLAccountId;
        }
        else if (this.ObjectTableName == "GLAccount") {
            windowArgs.GLAccountId = entity.Id;

            if (externalPage)
                windowTitle = TextCodeTranslator.Translate("GLAccount.O.ExternalTransaction") + " " + externalPage.PageNo;
            else
                windowTitle = TextCodeTranslator.Translate("GLAccount.O.NewExternalTransaction");

        }

        windowArgs.AdditionalDataPM = this.additionalDataPM;
        windowArgs.externalPage = externalPage;
        windowArgs.PageObjectTableName = this.ObjectTableName;
        windowArgs.EntityPM = entity;
        windowArgs.BankAccountId = this.EntityPM.Id;
        windowArgs.IsRestoreButtonEnabled = this.IsRestoreButtonEnabled;
        windowArgs.IsRestoreButtonVisibile = this.IsRestoreButtonVisibile;
        windowArgs.RestoreToolTipMessage = this.RestoreToolTipMessage;
        windowArgs.EnableReconcileEditButton = this.EnableReconcileEditButton;



        windowArgs.message = this.message;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(() => this.ReloadData());
        logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');
    }

    additionalDataPM: ExternalPageAdditionalDataPM;
    _ExternalPageAdditionalDataPMService: ExternalPageAdditionalDataPMService = new ExternalPageAdditionalDataPMService();
    GetAdditionalData(objectTableId: string, entityId: string)
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._ExternalPageAdditionalDataPMService.get(objectTableId, entityId)
            .subscribe((response:any) =>
            {
                console.log("[GetAdditionalData]", response);
                this.CurrentSession.StopBusyIndicator();

                if (!response.HasError) {
                    this.additionalDataPM = response.Result

                }
                else {
                    console.error(response.ErrorsArray.toString());

                }
            });
    }

    ExternalAdjustButtonClicked()
    {
        if(this.ObjectTableName != 'GLAccount')
            console.error("[ExternalAdjustButtonClicked] table is not glaccount !!!!!!!");

        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) =>
        {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result;
                var openAmountCurrency = transaction ? transaction.OpenAmountCurrencySign : "";
                this.showReconcileWindow(openAmountCurrency);

            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    showReconcileWindow(currency: any)
    {

        var windowArgs: any = {};
        windowArgs.BankAccountPM = this.EntityPM;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.openAmountCurrency = currency; // CurrencySign
        windowArgs.ObjectTableName = this.ObjectTableName;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 900;
        logitudeWindow.Height = 800;
        logitudeWindow.IsFullScreen = true;
        var BankName = this.DontShowLocal ? this.EntityPM.EnglishName : this.EntityPM.LocalName == null ? this.EntityPM.EnglishName : this.EntityPM.LocalName;

        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.ExternalReconcile") + ' - ' + BankName;

        logitudeWindow.IsFullScreen = true;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/ExternalReconcileComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) =>
        {
            // show alert
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

}
