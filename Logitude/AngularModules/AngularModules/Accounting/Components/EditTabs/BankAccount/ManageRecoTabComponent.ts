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
    templateUrl: './ManageRecoTabComponent.html'
})

export class ManageRecoTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: BankAccountPM = null;
    public ObjectTableName = "ExternalReconciliation";
    public DataContext = this;
    public isRTL: boolean = false;
    public CurrentEditComponentId: string;

    // Events
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    // Filters
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;

    // Services
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _entityListService: EntityListService = new EntityListService();
    //_ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    //_ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    //_BankAccountPMService: BankAccountPMService = new BankAccountPMService();

    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

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

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Listen() {


        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;

            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            } 

            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }

            //
            if (this.TabSelectedEvent == null) {
                this.TabSelectedEvent = SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "BAMR") {
                            this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
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
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
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
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.ReloadData();
            }
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

        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("GLAccountId", this.EntityPM.GLAccountId, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        //#endregion

        return this._entityListService.getByFilters("ExternalReconciliation", filters);

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
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
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