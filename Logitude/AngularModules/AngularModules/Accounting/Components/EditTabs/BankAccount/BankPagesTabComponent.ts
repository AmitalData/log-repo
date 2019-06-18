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

@Component({
    moduleId: module.id,
    templateUrl: './BankPagesTabComponent.html'
})

export class BankPagesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: BankAccountPM = null;
    public ObjectTableName = "ReconcileExternalPage";
    public DataContext = this;

    // Events
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    // Filters
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;

    // Services
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _entityListService: EntityListService = new EntityListService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _BankAccountPMService: BankAccountPMService = new BankAccountPMService();

    public isRTL: boolean = false;


    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe((response: any) => { });

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();

        //#region Fill Date Default Values
        var today = new Date();
        this.ToDate = new Date();
        this.oldToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        this.oldFromDate = new Date(lastmonth);
        //#endregion
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
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
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);


        } else {
            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
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
        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;

        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
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
            FieldName: SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.StatusName"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ReconcileExternalPageListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: SessionLocator.LoggedUserPM.DontShowLocal ? 'EntryTypeEnglishName' : 'EntryTypeLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ReconcileExternalPage.F.EntryTypeEnglishName"),
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

        filters.PageSize = 50;
        filters.PageIndex = 0;
        filters.GetCount = true;

        filters.SortBy = "PageNo"; //FromDate
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("BankAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
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
        this.OpenWindow();
    }
    //#endregion

    onRowSelected(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var entity = item.rowData;
            var entityId = entity.Id;

            this.OpenWindow(entity);
        }
    }

    OpenWindow(entity: any = null) {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        if (entity)
        {
            var entityPM;
            this._ReconcileExternalPagePMService.get(entity.Id).subscribe((myResult) => {
                entityPM = myResult.Result;
                this.ShowWindow(entityPM);
            });
        }
        else
        {
            this._ReconcileExternalPageExtendedPMService.GetDraftPage(this.EntityPM.Id).subscribe((myResult) =>
            {
                var draftPage = myResult.Result;
                if (!AppTool.IsNullOrEmpty(draftPage))
                {
                SessionLocator.CurrentSession.StopBusyIndicator();

                    var msg = new MessageWindow();
                    //msg.Title = "Error";
                    msg.Width = 360;
                    msg.RTL = this.isRTL;
                    //msg.ShowErrorIcon = true;
                    msg.Show(TextCodeTranslator.Translate("ReconcileExternalPage.O.CantNewBankPageDraft") + " (" + draftPage.PageNo + ") ");
                }
                else {
                    this.ShowWindow(entity);

                }
            });
        }
    }
    ShowWindow(entity: any = null) {

        // get bank account, then open window
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
        this._BankAccountPMService.get(this.EntityPM.Id).subscribe((myResult) =>
        {
            SessionLocator.CurrentSession.StopBusyIndicator();
            var bankAccount = myResult.Result;

            if (!AppTool.IsNullOrEmpty(bankAccount))
            {

                //SHOW WNIDOW
                var windowTitle = entity ? (TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + entity.PageNo) : TextCodeTranslator.Translate("Accounting.General.O.NewPage");
                var windowArgs: any = {};
                windowArgs.entity = entity;
                windowArgs.BankAccountId = this.EntityPM.Id;
                windowArgs.GLAccountId = bankAccount.GLAccountId;
                windowArgs.BankAccount = bankAccount;
                var logWindow = new LogitudeWindow();
                logWindow.Width = 1000;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => this.ReloadData());
                logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');
                //

            }
            else
            {
                console.error("ERROR!! no bank account found!!!!");
            }
        });


    }

}
