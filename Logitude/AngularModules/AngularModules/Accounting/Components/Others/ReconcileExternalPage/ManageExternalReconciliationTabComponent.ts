import {Component, OnInit, Output, EventEmitter, AfterViewInit, OnDestroy, ChangeDetectorRef, ViewChild}  from '@angular/core';
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
import { Operators } from 'Accounting/DataContracts/Operators';
import { LogGridComponent } from 'Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent';

@Component({
    templateUrl: './ManageExternalReconciliationTabComponent.html'
})

export class ManageExternalReconciliationTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: BankAccountPM = null;
    public ObjectTableName = "ExternalReconciliation";
    public DataContext = this;
    public isRTL: boolean = false;
    public CurrentEditComponentId: string;

    @ViewChild('DataGrid') DataGrid:LogGridComponent;

    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;
    amountFieldFilter: FilterItem;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _entityListService: EntityListService = new EntityListService();
    private CurrentSession = SessionLocator.SelectedSession;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Title: string;

    operatorsList =
    [{Code:Operators.Equals, EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
        {Code:Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
        {Code:Operators.LargerThan, EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
        {Code:Operators.LessThan, EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
        {Code:Operators.LessThanOrEqual, EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
        {Code:Operators.GreaterThanOrEqual, EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
        {Code:Operators.Between, EnglishName: 'Between', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Between") },
    ];
    selectedAmountOperator:{Code:string, EnglishName: string, LocalName: string };
    amount: number;
    amountFrom: number;
    amountTo: number;
    get Amount() { return this.amount; }
    set Amount(value: number) {
        if (this.amount != value) {
            this.amount = value;
        }
    }

    showLocals:boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.SetTitles();

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();

        //#region Default date filter value
        var today = new Date();
        this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1); // month backward
        this.FromDate = new Date(lastmonth);
        //#endregion

        this.Listen();
    }

    SetTitles(){
        switch (this.entityArgs.ObjectTableName) {
            case "BankAccount": {
                this.Title = TextCodeTranslator.Translate("Accounting.General.O.ManageReconciliation");
                break;
            }
            case "GLAccount": {
                this.Title = TextCodeTranslator.Translate("GLAccount.TH.ManageExternalReco");

                break;
            }
            default: {
                this.Title = TextCodeTranslator.Translate("Accounting.General.O.ManageReconciliation");

                break;
            }
        }
    }

    Listen() {


        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }

            //
            if (this.TabSelectedEvent == null) {
                this.TabSelectedEvent = this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "BAMR") {
                            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                            this.ReloadData();
                        }
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
    SetUIProperties() {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}

    }


    ngOnInit() {
        this.BuildColumns();
        this.ReloadData();
    }

    //#region Filters Properties
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                const toDateEndOfDay = new Date(this.ToDate);
                toDateEndOfDay.setHours(23, 59, 59, 999);

                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), toDateEndOfDay, null, "Between", false, false, false, "Date", false);
                this.ReloadData();
            }
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                const toDateEndOfDay = new Date(this.ToDate);
                toDateEndOfDay.setHours(23, 59, 59, 999);

                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), toDateEndOfDay, null, "Between", false, false, false, "Date", false);
                this.ReloadData();
            }
        }
    }


    private showCrossYearReconciliations : boolean = false;
    public get ShowCrossYearReconciliations() : boolean {
        return this.showCrossYearReconciliations;
    }
    public set ShowCrossYearReconciliations(v : boolean) {
        this.showCrossYearReconciliations = v;
        this.ReloadData();

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

    AmountOperatorChanged($event){
        this.selectedAmountOperator = $event
        this.Amount=null;
        this.AmountTextChanged(this.Amount);
    }
    AmountTextChanged(num) {
        if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.amount)&& this.selectedAmountOperator ) {
            var amountFieldName = 'ReconciliationAmount';
            this.timerToken = setTimeout(() => {
                if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.amount)) {
                    this.amountFieldFilter = new FilterItem(amountFieldName, num, null, null, this.selectedAmountOperator.Code, true, false, false, "number", false);
                    this.RefreshButtonClicked();
                } else {
                    this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
                }
            }, 700);

        } else {
            this.timerToken = setTimeout(() => {
                this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
            }, 700);
        }
    }
    AmountTextToChanged(num) {
        this.amountTo = num;
        this.filterByAmountFromAndTo();
    }
    AmountTextFromChanged(num) {
        this.amountFrom = num;
        this.filterByAmountFromAndTo();
    }
    
    private filterByAmountFromAndTo() {
        if (!AppTool.IsNullOrEmpty(this.amountFrom) && !AppTool.IsNullOrEmpty(this.amountTo) && this.selectedAmountOperator ) {
            var amountFieldName = 'ReconciliationAmount';
            this.timerToken = setTimeout(() => {
                if (!AppTool.IsNullOrEmpty(this.amountFrom) && !AppTool.IsNullOrEmpty(this.amountTo)) {
                    this.amountFieldFilter = new FilterItem(amountFieldName,this.amountFrom, this.amountTo, null, this.selectedAmountOperator.Code, true, false, false, "number", false);
                    this.RefreshButtonClicked();
                } else {
                    this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
                }
            }, 700);

        } else {
            this.timerToken = setTimeout(() => {
                this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
            }, 700);
        }
    }
    //#endregion

    //#region Data
    public columns: any[] = null;

    ReloadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'ReconciliationNumber',
            DataTypeCode: 'String',
            //Display: 'Reconciliation No.',
            Display: TextCodeTranslator.Translate("ExternalReconciliation.F.ReconciliationNumber"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            //Display: 'Create Date',
            Display: TextCodeTranslator.Translate("ExternalReconciliation.F.CreateDate"),
            Styles: { width: '130px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreatedByUserName',
            DataTypeCode: 'String',
            //Display: 'Created By',
            Display: TextCodeTranslator.Translate("ExternalReconciliation.F.CreatedByUserName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: this.showLocals ? 'AccountLocalName' :  'AccountName',
            DataTypeCode: 'String',
            //Display: 'Created By',
            Display: TextCodeTranslator.Translate("ExternalReconciliation.F.AccountLocalName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });



        this.columns.push({
            FieldName: 'IsCancelled',
            DataTypeCode: 'boolean',
            Display: TextCodeTranslator.Translate("ExternalReconciliation.F.IsCancelled"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
        });
        //this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            this.DataGrid.DetectChangesTimer();
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
        if (this.amountFieldFilter) {
            filters.AdditionalFilters.push(this.amountFieldFilter);
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetCount = getCount;

        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        var glaccountId = this.getGLAccountId();

        var bankTransferGLAccountId = "";
        if (this.entityArgs.ObjectTableName == "BankAccount")
            bankTransferGLAccountId = this.EntityPM.TransferGLAcccountId;

        filters.addAdditionalFilter("GLAccountId", glaccountId + "," + bankTransferGLAccountId, null, null, "InListExact", false, false, false, "string");

        if(this.ShowCrossYearReconciliations)
            filters.addAdditionalFilter("CrossYearReconcile", true, null, null, "Equals", false, false, false, "boolean");
        // filters.addAdditionalFilter("GLAccountId", glaccountId, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        //#endregion

        return this._entityListService.getByFilters("ExternalReconciliation", filters);

    }

    private getGLAccountId()
    {
        var glaccountId;
        if (this.entityArgs.ObjectTableName == "GLAccount")
            glaccountId = this.EntityPM.Id;
        else if (this.entityArgs.ObjectTableName == "BankAccount")
            glaccountId = this.EntityPM.GLAccountId;
        return glaccountId;
    }
    //#endregion

    //#region Buttons
    RefreshButtonClicked() {
        this.ReloadData();
    }

    //#endregion

    // returned value{ colDef, colIndex, rowData, rowIndex }
    onRowSelected(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            var lineData = item.rowData;
            var entityId = lineData.Id;
            this.OpenReco(entityId);
        }
    }

    OpenReco(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ExternalReconciliation' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.ReloadData();
                    });
                });

        }
    }

}
