declare var window: any;
import { Component, OnInit, Type, Output, EventEmitter, ComponentRef, ViewChild, QueryList, ViewChildren, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ServiceArgs } from '../../DataContracts/ServiceArgs';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { LogEvents } from '../../../Infrastructure/Utilities/LogEvents';
import { PubSubService } from '../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import { PubSubService1 } from '../../../Infrastructure/Utilities/events/ApiFiltersEvent1';
import { AppTool, DateTool } from '../../Tools';
import { ListComponentArgs, NewEntityArgs } from '../../Args';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { EntityPMService } from '../../Services/EntityPMService';
import { TotangoService } from '../../Services/WebServices/TotangoService';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { QueryColumnsPMService } from '../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService';
import { GeneralEntitiesArgs } from '../../../Infrastructure/DataContracts/GeneralEntitiesArgs';
import { GeneralEntitiesService } from '../../../Infrastructure/Services/StandardPMs/GeneralEntitiesService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ImportEntityArgs } from '../../../Common/Components/Maintenance/TenantImportComponent';
import { JournalPM } from '../../../Accounting/EntityPMs/JournalPM';
import { APPaymentPM } from '../../../Invoice/EntityPMs/APPaymentPM';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { ServiceLocator } from '../../Locators/ServiceLocator';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../Utilities/AmitalGatewayUtil';
import { AccountingIntegrityCheckPM } from '../../../Accounting/EntityPMs/AccountingIntegrityCheckPM';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, throttleTime } from 'rxjs/operators';
import { LogGridComponent } from '../LogitudeComponents/LogGridComponent/LogGridComponent';
import { LogGridComponentV2 } from '../LogitudeComponents/LogGridComponent/LogGridComponentV2';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { EditComponent } from '../EditComponent/EditComponent';
import { AWBWizardLoadComponent } from 'ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent';
import { AWBWizardComponent } from 'ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent';
import { BIReportPreviewComponent } from 'InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent';
import { ClientEditComponent } from 'CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent';
import { VendorEditComponent } from 'CustomsModules/CustomsVendor/Components/EditTabs/VendorEditComponent';
import { CustomsCollateralComponent } from 'CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
import { ProceduralFaultsGeneralTabComponent } from 'CustomsModules/CustomsProceduralFault/Components/EditTabs/General/ProceduralFaultsGeneralTabComponent';
import { SharedManifestComponent } from 'ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent';
import { CargoSplitGeneralTabComponent } from 'CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent';
import { LogisticActionRequestGeneralTabComponent } from 'CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/General/LogisticActionRequestGeneralTabComponent';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
import { IsMultiUpdateValid } from 'Infrastructure/Helpers/MultiUpdateHelper';
import { IsMultiPrintValid } from 'Infrastructure/Helpers/MultiPrintHelper';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { QueryPM } from '../../EntityPMs/QueryPM';
import { CustomizationPermissionService } from '../../../InfrastructureModules/InfrastructureCustomization/ExternalService/CustomizationPermissionService';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { FastSearchResult, FastSearchSettings } from 'Customs/Services/WebServices/AzureSearchWebService';
import { FastSearchService } from './FastSearchService';

 
@Component({

    templateUrl: './ListComponent.html',
    providers: [ListComponentArgs, EntityListService, EntityResourceService, PubSubService, PubSubService1, EntityPMService, TotangoService],
    changeDetection: ChangeDetectionStrategy.OnPush

})

export class ListComponent implements OnInit, AfterViewInit {
    public IsDemoTenant: boolean = false;
    public ComponentIndex: number = null;
    private myQueryColumnsPMService: QueryColumnsPMService;
    @Output() BackCompleted = new EventEmitter();
    @Output() LoadResourceCompleted = new EventEmitter();
    @Output() ColumnsReady = new EventEmitter();
    @Output() QueryListSourceChanged = new EventEmitter();
    @Output() FiltersBarLoaded: EventEmitter<any> = new EventEmitter<any>();
    @Output() RowClicked = new EventEmitter();
    @Output() SelectedRows: EventEmitter<any> = new EventEmitter();
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    public SeachBoxIsDisabled: boolean = false;
    //@Output() ShowTipEvent = new EventEmitter();
    public IsNavigateButtonVisible: boolean = false;
    public ComponentRef: ComponentRef<ListComponent>;
    public ReattachToDetection: boolean;
    public columnsObjectFields: any[] = [];
    public rowCount: number;
    public CustomGetTotalCount: number = null;
    public items: any[] = [];
    public columns: any[] = [];
    public SearchText: string = "Search Partners / Ports / Ref.#";
    public SearchTextValue: FormControl;
    public EntityService: Type<any>;
    public IsAdvancedSearchOpened: boolean = false;
    LayoutDirection: string = 'ltr';
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    public HasCustomsFilterMenu: boolean = false;
    public IsPhysicalCheckObjectTable: boolean = false;
    public IsReportExecutionLogObjectTable: boolean = false;

    public IsLogisticActionRequestObjectTable: boolean = false;
    WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();
    private _declarationWebService: DeclarationWebService = new DeclarationWebService();
    public onChangeCheckBoxesState: EventEmitter<any> = new EventEmitter();
    public ScreenQueryAction = {};
    private ScreenQueryActions = {
        "Customs.Declaration.DiamondsDeclarations": {
            actions: ["SendDeclarationAction", "SendSignedDeclarationsAction", "SendDeclarationPaymentsAction"],
            actionTranslationPrefix: "Customs.Declaration.O."
        }
    };

    @ViewChild(LogGridComponent) MyLogGridComponent: LogGridComponent = null;
    @ViewChild(LogGridComponentV2) MyLogGridComponentV2: LogGridComponentV2 = null;
    public IsShowTipArea: boolean = false;
    public IsShowTipIcon: boolean = false;
    public IsFirstTipLoad: boolean = false;

    ConstantPageSize: number = 100;
    DontApplyVirtualization: boolean = false;
    HasMutliUpdateFeature: boolean = false;
    HasMultiPrintFeature: boolean = false;
    //public Title: string;
    private title: string;//= "";
    customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    get Title() { return this.title; }
    set Title(newValue: string) {
        if (this.title != newValue) {
            this.title = newValue;
        }
    }
    GeneralEntitiesArgs: GeneralEntitiesArgs;
    public BackBtnTitle: string;
    public AddButtonTitle: string = "";
    public serviceArgs: ServiceArgs;
    public ignoreRefresh: boolean = false;
    CurrentQueryFilters: ApiQueryFilters;    
    AdvanceFilters: ApiQueryFilters;
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() onRefershQueryEvent = new EventEmitter();
    @Output() onSelectedQueryChangeEvent = new EventEmitter();
    searchDropdownOptions: FastSearchResult[] = [];    
    fastSearchSettings: FastSearchSettings = null;
    $fastSearchEnable: BehaviorSubject<boolean> = null;
    fastSearchAllow: boolean = false;
    intialAdditionalFilters: string[] = [];
    searchRun = false;

    onOpenFilterAreaClick() {
        this.IsAdvancedSearchOpened = true;
    }
    onCloseFilterAreaClick() {
        this.IsAdvancedSearchOpened = false;
    }
    onColumnsClick() {
        var windowArgs: any = {};
        windowArgs.queryId = this.SelectedQueryId;
        windowArgs.queryCode = this.SelectedQueryCode;
        windowArgs.isNewQueryMode = false;
        windowArgs.currentObjectTable = this.ObjectTableName;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.O.QueryColumnsEdit");//"Query Columns Edit";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/QueryColumnsComponents/QueryColumnsEditComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.QueryValueChanged({ QueryCode: this.SelectedQueryCode, Title: TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode), Filters: this.CurrentQueryFilters, IgnoreSearchFields: true });
        });
    }

    onSearchTextChangeEvent(searchtext) {
        const timer: number = this.$fastSearchEnable.value ? this.fastSearchService.Settings.idleSearchTimeMs : 400;

        console.log("Search");
        if ((this.searchFields != searchtext) && !(searchtext == null && this.searchFields == "")) {
            this.searchFields = searchtext;
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(() => this.searchMethod(), timer);
        }

        this.showRecentSearches() 
    }

    async searchMethod() {       
             if (this.ignoreRefresh) {
            this.ignoreRefresh = false;
            return;
        } 
        const searchFieldName: string = this.IsUseCardSearchMechanism() ? "CardSearchField" : "SearchFields";
        this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.filter(a => a.FieldName != searchFieldName);
        
        if (this.fastSearchService.$fastSearchEnable.value) {
            try {
                this.searchRun = true;
                this.CD.detectChanges();
                this.searchDropdownOptions = await this.fastSearchService.search(this.CurrentQueryFilters, this.searchFields)
                this.searchRun = false;
                if (this.searchDropdownOptions) {
                    this.CD.detectChanges();
                    return;
                }    
            } catch (error) {
                if(error instanceof HttpErrorResponse && error.error.ErrorType === "FieldsNotExistsInIndexException")
                    this.fastSearchCheckbox(false);
            } finally {
                this.searchRun = false;
            }
        }
            
        this.CurrentQueryFilters.addAdditionalFilter(searchFieldName, this.searchFields, null, null, "Contains", false, true, false, "String");
        this.onQueryChangeEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: this.CurrentQueryFilters, SearchFieldChanged: true, Reload: true });
        this.CD.detectChanges();            
    }

    searchDropdownSelected(optionSelected: FastSearchResult | string) {
        this.fastSearchService.searchDropdownSelected(optionSelected, this.CurrentQueryFilters, this.SelectedQueryCode, this.searchDropdownOptions, this.MethodName, this.onRowSelected.bind(this), this.onQueryChangeEvent);
    }
   
    fastSearchCheckbox(check: boolean) {
         const hasAdvancedFilters = this.CurrentQueryFilters.AdditionalFilters.some(f => !this.intialAdditionalFilters.includes(f.FieldName) && f.FieldName != "SearchFields");
         if(hasAdvancedFilters && check) {
                this.fastSearchAllow = false;
                this.CD.detectChanges();
                this.fastSearchAllow = true;
                this.CD.detectChanges();
            return;
         }

        this.fastSearchService.$fastSearchEnable.next(check);
        if(!check && this.fastSearchService.orginalCurrentAdditionalFilters != null) {
            this.CurrentQueryFilters.AdditionalFilters = [...this.fastSearchService.orginalCurrentAdditionalFilters];
            this.fastSearchService.orginalCurrentAdditionalFilters = null;
        }

        this.searchMethod();
    }

    async showRecentSearches() {
        if (!this.fastSearchService.$fastSearchEnable.value || this.searchFields?.length > 0) return;

        this.searchDropdownOptions = await this.fastSearchService.getRecentSearches();
        this.CD.detectChanges();    
    }

    GetMethodName() {
        if (this.MenuTableQuerySection) return this.ObjectTableName;
        let methodName = this.SelectedQuery.QuerySection;
        if (methodName.indexOf("Customs.") > -1) {
            methodName = methodName.split('.')[1];
        }
        return methodName;


    }
    ApplyPreDefinedFilters() {
        if (this.SelectedQuery != null) {
            this.MethodName = this.GetMethodName();
            this.SelectedQueryCode = this.SelectedQuery.UniqueCode;
            this.SelectedQueryId = this.SelectedQuery.Id;

            if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
                this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
            }
            else {
                this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
            }
            if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortDirection)) {
                this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
            }
            else {
                this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
            }
        }
        this.CurrentQueryFilters = new ApiQueryFilters();
        if (window.PreDefinedFilters.filter(d => d.QueryCode == this.SelectedQuery.UniqueCode) != null) {
            var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryCode == this.SelectedQuery.UniqueCode);
            predefinedFilters.forEach((filter, key) => {
                var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                var value1 = filter.PredefinedValue;
                var value2 = filter.PredefinedValue2;
                if (value2 != null) {
                    filterOperator = "Between";
                }
                if (filter.DataTypeCode == "DateTime" || filter.DataTypeCode == "Date") {
                    var TodayDate = new Date();
                    TodayDate.setHours(0, 0, 0, 0);

                    if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                    if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);

                    var TommorowDate = DateTool.AddDays((new Date()), 1);
                    TommorowDate.setUTCHours(0, 0, 0, 0);
                    var TodayDate = new Date();
                    TodayDate.setUTCHours(0, 0, 0, 0);
                    var YesterdayDate = DateTool.AddDays((new Date()), -1);
                    YesterdayDate.setUTCHours(0, 0, 0, 0);
                    var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                    LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                    var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                    LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                    var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 2);
                    CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                    var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
                    CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                    var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                    LastYearFromDate.setUTCHours(0, 0, 0, 0);
                    var LastYearToDate = DateTool.AddDays((new Date()), 1);
                    LastYearToDate.setUTCHours(0, 0, 0, 0);

                    if (value1 == "Today") {
                        value1 = TodayDate;
                        value2 = TommorowDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "Yesterday") {
                        value1 = YesterdayDate;
                        value2 = TodayDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "Last 7 Days") {
                        value1 = LastSevenDaysDate;
                        value2 = TommorowDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "Last 30 Days") {
                        value1 = LastThirtyDaysDate;
                        value2 = TommorowDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "Current Year") {
                        value1 = CurrentYearFromDate;
                        value2 = CurrentYearToDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "Last Year") {
                        value1 = LastYearFromDate;
                        value2 = LastYearToDate;
                        filterOperator = "Between";
                    }
                    else if (value1 == "NoDate" || value1 == "No Date") {
                        value1 = "NoDate";
                        filterOperator = "NoDate";
                    }
                    else if (value1 == "Less than Today") {
                        value1 = TodayDate;
                        filterOperator = "LessThan";
                    }
                    else if (value1 == "Less than or equal Today") {
                        value1 = TommorowDate;
                        filterOperator = "LessThan";
                    }
                }
                var field = window.ObjectFields.filter(a => a.FieldCode == filter.ObjectFieldCode)[0];
                if (field) {
                    this.CurrentQueryFilters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortColumn) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortBy)) {
            this.CurrentQueryFilters.SortBy = this.SelectedQuery.DefaultSortColumn;
        }
        if (!AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortDirection) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortDirection)) {
            this.CurrentQueryFilters.SortDirection = this.SelectedQuery.DefaultSortDirection;
        }
        if (this.listArgs.SelectedTransportMode != "All") {
            this.CurrentQueryFilters.addAdditionalFilter("TransportModeId", this.listArgs.SelectedTransportMode, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedTransportMode == "All" ? true : false));
        }
        if (this.listArgs.SelectedDirection != "All") {
            this.CurrentQueryFilters.addAdditionalFilter("DirectionId", this.listArgs.SelectedDirection, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedDirection == "All" ? true : false));
        }
    }


    public rowData: Array<any>;
    public showGrid: boolean;
    public dataCount: number;
    public searchFields: string;

    dataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "",//"CreateDateTime",
        sortingDir: "",//"Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            this.currentSortingCol = sortingCol;
            this.currentSortingDir = sortingDir;
            this.currentSearchFields = searchFields;
            this.currentFilters = filters;

            return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
        },
    };

    public ObjectTableName: string;
    public ObjectTableDisplayName: string;
    public ObjectTable: ObjectTablePM;
    //public Query: any;
    public Queries: any[];
    public UserQueries: any[];
    public QueryColumns: any[];
    public firstCall: boolean = true;
    public SelectedQueryId: string;
    public SelectedQueryCode: string;
    private _SelectedQuery: any = null;
    public get SelectedQuery(): any {
        return this._SelectedQuery;
    }
    public set SelectedQuery(value: any) {
        this._SelectedQuery = value;
    }

    public QueryCode: string;
    public NewButtonLable: string;

    Filterchangeevent: LogEvents.EventManager;
    pubSubAdvanceQueryFiltersServiceRecived: PubSubService1;
    @Output() GridFilterchangeevent = new EventEmitter();
    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() FilterChangedEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    AdvanceQFiltersService: PubSubService;
    public TenantPM: TenantPM;
    MethodName: string = null;
    ListComponentId: string;
    UsingLogGridV2: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private SessionEvent: any = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _ListComponentArgs: ListComponentArgs, private _http: HttpClient, private _entityListService: EntityListService, private _entityResourceService: EntityResourceService, public pubSubAdvanceQueryFiltersService: PubSubService, private temp: PubSubService1, private entityPMService: EntityPMService, private _totangoService: TotangoService, private CD: ChangeDetectorRef, private fastSearchService: FastSearchService) {
        var UsingV2FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LV2")[0];
        if (UsingV2FeatureToggle || SessionLocator.LoggedUserPM.Email == "ahmada@logitudeworld.com") { this.UsingLogGridV2 = true; }

        if (this.CurrentSession == null) {
            this.ListComponentId = "ListComponentId_-1_-1";

        }

        else {
            this.ListComponentId = "ListComponentId_" + this.CurrentSession.LogitudeGridHelper.GetLListComponentIndexId();

        }

        this.ComponentIndex = this.CurrentSession.GetNewListComponentIndex();

        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = _http;
        this.pubSubAdvanceQueryFiltersServiceRecived = temp;
        this.AdvanceQFiltersService = pubSubAdvanceQueryFiltersService;
        this.TenantPM = InfraSettings.TenantPM;
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.SessionEvent.subscribe((res) => {
                if (res == "TenantImport") {
                    this.RefreshBtnClick();
                }
            })
        );

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "NewAirlineShippingLineClosed") {
                this.RefreshBtnClick();
            }
        });

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

        this.ExcludedItems = new ObservableCollection([]);
        this.SelectedItems = new ObservableCollection([]);
    }

    name: string;
    processAdvanceQueryFilters(filters) {        
        if (this.IsAdvancedSearchOpened == false) {
            return;
        }

        this.fastSearchCheckbox(false);        

        this.dataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "",//"CreateDateTime",
            sortingDir: "",//"Descending",
            getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            },
        };
        if (this.AdvanceFilters == null) {
            this.AdvanceFilters = new ApiQueryFilters();
        }

        if (filters.IsDeleted || (filters.textValue == "" && filters.textValue.toString() != "false") || filters.textValue == "No Filter") {
            this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
        }
        else if (filters.TextValue == "NoDate") {
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, null, null, null, "NoDate", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

        }
        else if (!AppTool.IsNullOrEmpty(filters.MyName)) {
            var filterOperator = "Between";

            var TommorowDate = DateTool.AddDays((new Date()), 1);
            TommorowDate.setUTCHours(0, 0, 0, 0);
            //TommorowDate.setHours(0, 0, 0, 0);
            var TodayDate = new Date();
            TodayDate.setUTCHours(0, 0, 0, 0);
            var TodayCustomDate = new Date();
            TodayCustomDate.setHours(0, 0, 0, 0);
            var TodayEndDate = new Date();
            TodayEndDate.setHours(23, 59, 59, 0);
            //TodayDate.setHours(0, 0, 0, 0);
            var YesterdayDate = DateTool.AddDays((new Date()), -1);
            YesterdayDate.setUTCHours(0, 0, 0, 0);
            //YesterdayDate.setHours(0, 0, 0, 0);
            var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 2);
            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
            var LastYearFromDate = DateTool.AddDays((new Date()), -365);
            LastYearFromDate.setUTCHours(0, 0, 0, 0);
            var LastYearToDate = DateTool.AddDays((new Date()), 1);
            LastYearToDate.setUTCHours(0, 0, 0, 0);

            if (filters.TextValue == "Today") {
                filters.TextValue = TodayCustomDate;
                filters.TextValue1 = TodayEndDate;
                filters.MyName = "Today";
            }
            else if (filters.TextValue == "Yesterday") {
                filters.TextValue = YesterdayDate;
                filters.TextValue1 = TodayDate;
                filters.MyName = "Yesterday";
            }
            else if (filters.TextValue == "Last 7 Days") {
                filters.TextValue = LastSevenDaysDate;
                filters.TextValue1 = TommorowDate;
                filters.MyName = "Last 7 Days";
            }
            else if (filters.TextValue == "Last 30 Days") {
                filters.TextValue = LastThirtyDaysDate;
                filters.TextValue1 = TommorowDate;
                filters.MyName = "Last 30 Days";
            }
            else if (filters.TextValue == "Current Year") {
                filters.TextValue = CurrentYearFromDate;
                filters.TextValue1 = CurrentYearToDate;
                filters.MyName = "Current Year";
            }
            else if (filters.TextValue == "Last Year") {
                filters.TextValue = LastYearFromDate;
                filters.TextValue1 = LastYearToDate;
                filters.MyName = "Last Year";
            }
            else if (filters.TextValue == "NoDate" || filters.TextValue == "No Date") {
                filters.TextValue = "NoDate";
                filterOperator = "NoDate";
            }
            else if (filters.TextValue == "Less than Today") {
                filters.TextValue = TodayDate;
                filters.MyName = "Less than Today";
                filterOperator = "LessThan";
            }
            else if (filters.TextValue == "Less than or equal Today") {
                filters.TextValue = TommorowDate;
                filters.MyName = "Less than or equal Today";
                filterOperator = "LessThan";
            }
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, filterOperator, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

        }
        else if (filters.TextValue1) {
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            if (filters.FieldName == "ForeignAmount") {
                if (filters.TextValue.startsWith("-")) {
                    filters.TextValue1 = filters.TextValue.slice(1);
                }
                else {
                    filters.TextValue1 = "-" + filters.TextValue.toString();
                }
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else {
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            if (filters.ObjectField.DataTypeCode == 'Boolean') {
                this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue.toString().toLowerCase(), null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
            }
            else {
                if (filters.FieldName == "CompetitorFields")
                    this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, null, null, "Contains", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
                else {
                    if (filters.FieldName == "ForeignAmount") {
                        if (filters.TextValue.startsWith("-")) {
                            filters.TextValue1 = filters.TextValue.slice(1);
                        }
                        else {
                            filters.TextValue1 = "-" + filters.TextValue.toString();
                        }
                        /*var decimalValue = parseFloat(filters.TextValue);
                        var absoluteValue = Math.abs(decimalValue);
                        filters.TextValue1 = absoluteValue;*/
                        this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, "Contains", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

                    }
                    else {

                        this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
                    }
                }
            }
        }
        this.pubSubAdvanceQueryFiltersServiceRecived.Stream.emit(filters);
    }
    HasPermition: boolean = true;

    NewButtonId: string;

    ngOnInit() {

        if (this.IsUseCardSearchMechanism()) {
            this.DontApplyVirtualization = true;
        }

        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare && this.ObjectTableName == "Contact") {
            this.IsDemoTenant = true;
        }

        if (this.ObjectTableName == "LedgerTransaction") {
            this.IsAdvancedSearchOpened = true;
        }
        this.NewButtonId = "NewButton_" + this.ObjectTableName;

        if (!this.CheckPermissions(this.ObjectTableName, "NEW", false) && !this.ObjectTable?.IsCustom) {
            this.IsNewEntityButtonDisabled = true;
        }

        if (this.ObjectTableName != "PortTimeZone") {
            if (!this.CheckPermissions(this.ObjectTableName, "READ", false)) {
                this.HasPermition = false;
            }
        }
        this.Filterchangeevent = new LogEvents.EventManager();

        this.Listen();
        //this.CD.detectChanges();
        if (this.ObjectTable.ClientModuleName == "Customs") {
            this.HasCustomsFilterMenu = true;

        }

        if (this.ObjectTableName == "Customs.PhysicalCheck") {
            this.IsPhysicalCheckObjectTable = true;
        }
        else if (this.ObjectTableName == 'ReportExecutionLog') {
            this.IsReportExecutionLogObjectTable = true;
        }
        else if (this.ObjectTableName == "Customs.LogisticActionRequest") {
            this.IsLogisticActionRequestObjectTable = true;
        }

        if (["Customs.DeclarationReferantData", "Customs.DeclarationCargoSplit", "Customs.LogisticActionRequest"].includes(this.ObjectTableName)) {
            this.HasCustomsFilterMenu = true;
        }
        if (this.ObjectTableName == "Customs.ExportStorge") {

            this.LayoutDirection = "ltr";
            this.RTL = false;
            this.ShowViews = false;
            this.EnglishView = true;
        }

        this.fastSearchService.subscribeMenuHeaderchangeevent(this.MenuHeaderchangeevent);
    }
    public ReloadAllListEvent: any = null;
    Listen() {
        if (!this.ReloadAllListEvent) {
            this.ReloadAllListEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ReloadAllList") {
                    this.RefreshBtnClick();
                } else if (s == "ReloadAllList" + this.ObjectTableName) {
                    this.RefreshBtnClick();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ReloadAllListEvent);
    }

    ngAfterViewInit() {
        this.intialAdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.map(f => f.FieldName)
    }

    public CheckPermissions(objectTableName: string, featureCode: string, showWindow: boolean) {
        if (objectTableName == "DeploymentPackage")
            return CustomizationPermissionService.HasEntityPermessions(objectTableName, featureCode, showWindow);
        return FeatureLocator.HasEntityPermessions(objectTableName, featureCode, showWindow);
    }


    IsShowAddFromLibraryLink: boolean;
    IsShowAddReportFromLibraryLink: boolean = false;
    IsEditBIReportVisible: boolean = false;
    HasExcelExportButton: boolean;

    LinkAddDocumentFromLibraryClcik() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.ObjectTableId = "";
        windowArgs.EntityId = "";
        windowArgs.TransportModeId = "";
        windowArgs.ShipmentlevelCode = "";
        windowArgs.ChildEntityId = "";
        windowArgs.ChildObjectTableId = "";
        windowArgs.PageRequest = "Maintanice";

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 550;
        logWindow.Title = "New Documents";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.isEditControlOpened = false;
            //this.OnBackFromEdit();
            this.RefreshBtnClick();
        });
    }

    LinkAddReportFromLibraryClick() {
        var windowTitle = "Add Report From Library";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        var windowArgs: any = {};
        windowArgs.IsCopyFromLibrary = true;
        windowArgs.FolderId = this.listArgs.BIReportFolderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.ComponentLoaded.subscribe(s => {
            //
        });
    }

    EditBIReportFolderClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Folder";
        var windowArgs: any = {};
        windowArgs.IsNew = false;
        windowArgs.FolderId = this.listArgs.BIReportFolderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReportFolderComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event) {
                this.Title = event;
            }
        });
    }

    IsShowAddQuoteTemplateFromLibraryLink: boolean;
    LinkAddQuoteTemplateFromLibraryClcik() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.Type = "Maintenance";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;
        logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.NewQuoteTemplate");
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddQuoteTemplateFromLibraryComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.isEditControlOpened = false;
            //this.OnBackFromEdit();
            this.RefreshBtnClick();
        });
    }





    FiltersMenu: ApiQueryFilters = null;
    private isLoaderReady: boolean;
    DeclarationsTable = false;
    View: string;
    async RunComponent() {


        if (window.Tips) {
            var tip = window.Tips.filter(d => d.Code == this.ObjectTable.MainTipCode)[0];
            if (tip) {
                var hasTip = true;
                this.IsShowTipIcon = true;
                var isVisible: boolean = tip.VisibilityDefaultValue;

                var tipVisibility = window.TipsVisibilities.filter(d => d.TipCode == tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];
                if (tipVisibility) isVisible = tipVisibility.IsVisible;

                if (!isVisible && hasTip) this.IsShowTipArea = false;
                else this.IsShowTipArea = true;
            }

        }

        if (this.ObjectTable.Name == "Customer") {
            this.View = "View"
        }
        else { this.View = TextCodeTranslator.Translate("General.O.View"); }
        if (FeatureLocator.HasFeaturePermession("General", "EXPORTEXCEL")) {
            if (!AppTool.IsNullOrEmpty(this.ObjectTable.DownloadToExcelFeatureCode)) {
                if (FeatureLocator.HasFeaturePermession(this.ObjectTable.Name, this.ObjectTable.DownloadToExcelFeatureCode)) {
                    this.HasExcelExportButton = true;
                }
                else {
                    this.HasExcelExportButton = false;
                }

            }
            else
                this.HasExcelExportButton = true;
        }
        if (this.ObjectTable.Name == "Customs.Declaration") {
            this.DeclarationsTable = true;
        }

        if (this.ObjectTable.Name == "DocumentType") {
            if (FeatureLocator.HasFeaturePermession("DocumentType", "FROMLIBRARY")) {
                this.IsShowAddFromLibraryLink = true;
            }
            else {
                this.IsShowAddFromLibraryLink = false;
            }

        }

        if (this.ObjectTable.Name == "BIReport") {
            if (FeatureLocator.HasFeaturePermession("BIReport", "BIReportCopyFromLibrary")) {
                if (SessionLocator.Tenant != 0) {
                    this.IsShowAddReportFromLibraryLink = true;
                }
            }
            if (FeatureLocator.HasFeaturePermession("BIReportFolder", "UPDATE")) {
                this.IsEditBIReportVisible = true;
            }
        }

        if (this.ObjectTable.Name == "QuoteTemplate") {
            if (FeatureLocator.HasFeaturePermession("QuoteTemplate", "FROMLIBRARY")) {
                this.IsShowAddQuoteTemplateFromLibraryLink = true;
            }
            else {
                this.IsShowAddQuoteTemplateFromLibraryLink = false;
            }

        }


        if (this.ObjectTable.HasFiltersMenu || this.HasActionBar()) {
            if (this.AllLocations) {

                if (this.AllLocations.length == 0) {
                    this.RunComponentTimer();
                }

                else {
                    this.isLoaderReady = true;

                    if (this.ObjectTable.Name == "Customs.PhysicalCheck" || this.ObjectTable.Name == "Customs.LogisticActionRequest") {
                        this.LoadedActionBar("MNO", "ListActionBar");
                    } else {
                        this.LoadedActionBar("MNA", "ListActionBar");
                    }

                    if (this.ObjectTable.HasFiltersMenu) {

                        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "MNH")[0];
                        if (myLocation != null) {

                            let myObjectTableName = this.ObjectTable.Name;
                            if (myObjectTableName.startsWith(this.ObjectTable.ClientModuleName + '.')) {
                                myObjectTableName = myObjectTableName.substr((this.ObjectTable.ClientModuleName + '.').length)
                            }
                            let isCustomsObjectTableWith = false;
                            if (this.ObjectTable.ClientModuleName == "Customs") {
                                isCustomsObjectTableWith = true
                            }
                            var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/FiltersMenu/" + /*this.ObjectTable.Name*/myObjectTableName + "FiltersMenuComponent";
                            if (isCustomsObjectTableWith) {
                                myComponentPath = "./CustomsModules";
                                if(myObjectTableName == "DeclarationReferantData") {
                                    myComponentPath += "/CustomsReferant";
                                    this.ignoreRefresh = true
                                } 
                                myComponentPath = (myObjectTableName == "DeclarationCargoSplit") ? myComponentPath += "/CustomsDeclarationCargoSplit" : myComponentPath;
                                myComponentPath = (myObjectTableName == "LogisticActionRequest") ? myComponentPath += "/CustomsLogisticActionRequest" : myComponentPath;
                                myComponentPath = (myObjectTableName == "PhysicalCheck") ? myComponentPath += "/CustomsPhysicalCheck" : myComponentPath;
                                myComponentPath = (myObjectTableName == "Declaration") ? myComponentPath += "/CustomsDeclarationModules/DeclarationOthers" : myComponentPath;
                                myComponentPath = (myObjectTableName == "Containerization") ? myComponentPath += "/CustomsContainerization/" : myComponentPath;
                                myComponentPath += "/Components/FiltersMenu/" + /*this.ObjectTable.Name*/myObjectTableName + "FiltersMenuComponent";
                            }

                            SessionLocator.DynamicLoader.Load(myComponentPath, myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.FiltersBarLoaded.emit(cmpRef.instance);
                                    if (this.listArgs.Filters != null && isCustomsObjectTableWith) {
                                        cmpRef.instance.SetFiltersMenu(this.listArgs.Filters);
                                    }
                                    cmpRef.instance.SelectedValueChanged.subscribe(($event: any) => {
                                        if($event?.Filters.AdditionalFilters?.length > 1 || !$event?.Filters.AdditionalFilters[0]?.FieldName.includes('TransportMode')) 
                                            this.fastSearchCheckbox(false);

                                        this.SelectedFilterChanged($event);

                                        this.FiltersMenu = new ApiQueryFilters();
                                        this.FiltersMenu = $event.Filters;
                                        this.MenuHeaderchangeevent.emit({ Filters: $event.Filters, RemoveFilter: $event.RemoveFilter });
                                    });
                                    cmpRef.instance?.CustomGetTotalCount?.subscribe(($event: number) => {
                                        this.CustomGetTotalCount = $event;
                                    });
                                });
                        }
                    }

                }

            }

            else {
                this.RunComponentTimer();
            }
        }
        
        await this.fastSearchService.initFastSearch(this.ObjectTable, this.ObjectTableName, this.MenuTableQuerySection);
        this.$fastSearchEnable = this.fastSearchService.$fastSearchEnable;
        this.fastSearchAllow = this.$fastSearchEnable.value;        
        if (this.fastSearchAllow)
            this.fastSearchSettings = this.fastSearchService.Settings;
    }
  
    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private listArgs: ListComponentArgs;
    ShowViews: boolean = true;
    EnglishView: boolean = false;
    ResourcesLoaded: boolean = false;
    MenuTableQuerySection: string;

    Run(args: ListComponentArgs) {
        this.CurrentSession.AddMenuReference(this.ComponentRef);
        this.CurrentSession.AddListComponent(this);
        this.listArgs = args;
        if (!this.IsDemoTenant) {
            if (!AppTool.IsNullOrEmpty(this.listArgs.DisplayTitle)) {
                this.Title = this.listArgs.DisplayTitle;
            }


            this.QueryCode = args.QueryCode;
            this.ObjectTableName = args.ObjectTableName;
            this.HasMutliUpdateFeature = this.HasMultiUpdateFeature();
            this.HasMultiPrintFeature = this.IsMultiPrintFeatureOn();
            this.SetAddButtonTitle();
            this.MethodName = args.MethodName;
            this.BackBtnTitle = args.BackButtonTitle;
            this.ShowViews = args.ShowViews;
            this.MenuTableQuerySection = args.QuerySection ? args.QuerySection : args.ObjectTableName;


            this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
            this.ObjectTableDisplayName = new TextCodeTranslationPipe().transform(this.ObjectTable.Name)
            this.SeachBoxIsDisabled = this.ObjectTable.DisableSearchBox;

            this.SearchTextValue = new FormControl();
            this.NewButtonLable = args.NewButtonLabel;

            this.SearchTextValue.valueChanges
                .pipe(
                    debounceTime(500),
                    distinctUntilChanged()
                ).subscribe((search: string): any => {
                    this.searchFields = (search === "") ? this.searchFields = "" : this.searchFields = search;
                    this.SearchFieldchangeevent.emit(this.searchFields);
                });

            this.GetQueries();
            this.RunComponent();

        }
    }

    private HasMultiUpdateFeature(): boolean {
        if (this.ObjectTableName == "Shipment") {
            return FeatureLocator.HasFeaturePermession("Shipment", "MULTIUPDATE");
        }

        else if (this.ObjectTableName == "Container") {
            return FeatureLocator.HasFeaturePermession("Container", "MULTIUPDATE");
        }

        else return false;
    }
    private IsMultiPrintFeatureOn(): boolean {
        if (this.ObjectTableName == "ARInvoice") {
            return true;
        }
        else {
            return ((this.ObjectTableName == "Shipment") && FeatureLocator.HasFeaturePermession("General", "MultiPrint"));
        }
    }


    MutliUpdate() {
        if (!IsMultiUpdateValid(this.ObjectTable.DBTableName, this.dataSource.rowCount)) {
            return;
        }
        let newWindow = new LogitudeWindow();
        newWindow.Width = 1050;
        newWindow.Height = 700;
        newWindow.Title = "Multi Update " + this.ObjectTable.DBTableName;

        let windowArgs: any = {};
        windowArgs.QueryCode = this.SelectedQueryCode;
        windowArgs.Filters = this.CurrentQueryFilters;
        windowArgs.Columns = this.columns;
        windowArgs.ObjectTable = this.ObjectTable;
        windowArgs.Title = this.Title;

        newWindow.WindowArgs = windowArgs;
        newWindow.Show('./Infrastructure/Components/MultiUpdateComponent/MultiEntityUpdateBaseComponent');
        newWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshBtnClick();
        });
    }

    ViewInitCompleted(event) {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {
            this.BackBtnTitle = this.listArgs.BackButtonTitle;
            this.ResourcesLoaded = true;
            this.ViewQuery(this.listArgs.Filters, this.listArgs.DisplayTitle, this.listArgs.BackButtonTitle, this.listArgs.IsReadOnlyList, this.listArgs.IsBackToCurrentListView);
        });
    }

    ShowFieldsList() {
        document.getElementById("FieldsDropdown").classList.toggle("showDDButton");
    }

    SetAddButtonTitle() {
        if (this.ObjectTableName == "Airline" || this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Port" || this.ObjectTableName == "Warehouse") {
            this.AddButtonTitle = TextCodeTranslator.Translate("General.O.ImportEntities").replace(/%Entity/g, TextCodeTranslator.TranslateTablePlural(this.ObjectTableName));
        }
    }

    TipVisibilityChanged(event) {
        if (event == "true") this.IsShowTipArea = true;
        else this.IsShowTipArea = false;

        this.IsFirstTipLoad = false;
        this.RefreshBtnClick();
    }


    FilterQuerysByQuerySection(allQueries: any) {
        let querySection: string = !AppTool.IsNullOrEmpty(this.MenuTableQuerySection) ? this.MenuTableQuerySection : this.ObjectTableName;
        return allQueries.filter(d => d.QuerySection == querySection || d.QuerySection == (querySection + "FollowUp"));
    }
    public UserId: string = SessionInfo.LoggedUserId;
    public Tenant: number = SessionInfo.LoggedUserTenant;

    ApplyQueriesAdvancedFilter(allQueries: any) {
        if (this.ObjectTableName != "Shipment")
            return this.Queries;
        if (SessionLocator.TenantPM.ApproveUploadedDocuments == false) {
            return allQueries.filter(x => (x.Code != "Pending Approval Documents" && x.UserId == null && x.SystemLevel == true));
        }
        return this.Queries;
    }
    CustomerCareDeploymentPackage(objectTableName: string) {
        if (objectTableName != "DeploymentPackage")
            return false;
        if (FeatureLocator.IsFeatureGrantedByUniqeCode("General.Customization.DeploymentPackage"))
            return false;
        if ((SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev"))
            return true;
        return false;
    }
    GetQueries() {

        var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        allQueries = this.FilterQuerysByQuerySection(allQueries);

        this.Queries = allQueries.filter(x => x.UserId == null && x.SystemLevel == true);

        if (this.ObjectTableName == "DeploymentPackage")
            this.listArgs.DontCheckQueryFeature = true;
        if (!this.ObjectTable.IsClosed && !this.listArgs.DontCheckQueryFeature) {
            this.Queries = allQueries.filter(x => (FeatureLocator.IsFeatureGrantedByUniqeCode(x.FeatureUniqeCode)) || this.CustomerCareDeploymentPackage(x.ObjectTableName));

        }
        this.Queries = this.ApplyQueriesAdvancedFilter(allQueries);

        this.UserQueries = allQueries.filter(x => x.UserId != null && x.Tenant == SessionInfo.LoggedUserTenant);

        if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == false) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective)[0];
            this.SelectedQuery = allQueries.filter(f => f.Perspective == this.listArgs.Perspective)[0];

            this.Queries = allQueries.filter(f => (f.UserId == null && (FeatureLocator.IsFeatureGrantedByUniqeCode(f.FeatureUniqeCode) || this.ObjectTable?.IsCustom || this.CustomerCareDeploymentPackage(f.ObjectTableName)) && f.SystemLevel == true) && f.Perspective == this.listArgs.Perspective);
        }
        else if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == true) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];
            if (this.QueryCode.includes(this.ObjectTableName + ".")) {
                this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (/*this.ObjectTableName+ (f.UserId!= undefined?"." + f.UserId:"") + '.'+*/this.QueryCode))[0];
                if (this.SelectedQuery == null) {
                    this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (this.ObjectTableName + (f.UserId != undefined ? "." + f.UserId : "") + '.' + this.QueryCode))[0];
                }
            }
            else {
                this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (this.ObjectTableName + (f.UserId != undefined ? "." + f.UserId : "") + '.' + this.QueryCode))[0];
            }
            this.Queries = allQueries.filter(f => (f.UserId == null && (FeatureLocator.IsFeatureGrantedByUniqeCode(f.FeatureUniqeCode) || this.CustomerCareDeploymentPackage(f.ObjectTableName)) && f.SystemLevel == true) && f.Perspective == this.listArgs.Perspective);
        }

        else if (this.QueryCode) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];

            if (this.QueryCode.includes(this.ObjectTableName + ".")) {
                this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (/*this.ObjectTableName + (f.UserId != undefined ? "." + f.UserId : "") + '.'+*/this.QueryCode))[0];
                if (this.SelectedQuery == null) {
                    this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (this.ObjectTableName + (f.UserId != undefined ? "." + f.UserId : "") + '.' + this.QueryCode))[0];
                }
            }
            else {
                this.SelectedQuery = allQueries.filter(f => f.UniqueCode == (this.ObjectTableName + (f.UserId != undefined ? "." + f.UserId : "") + '.' + this.QueryCode))[0];
            }
        }

        else {
            this.SelectedQuery = this.Queries[0];
        }

        if (this.listArgs.DontCheckQueryFeature && this.IFSelectedQueryEmpty()) {
            this.SelectedQuery = this.Queries[0];
        }

        this.CheckIfQueriesConatinDefaultPerspectiveQuery();
        let forceExistQuery = (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs");

        if (/*forceExistQuery &&*/  this.SelectedQuery != null) {
            let existSelectedQuery: boolean = false;
            let alternativeUQuery: any = null;
            let alternativeQuery: any = null;
            if (!existSelectedQuery && this.UserQueries != null) {
                existSelectedQuery = this.UserQueries.filter(r => r.Code == this.SelectedQuery.Code).length > 0;
                alternativeUQuery = this.UserQueries[0];
            }
            if (!existSelectedQuery && this.Queries != null) {
                existSelectedQuery = this.Queries.filter(r => r.Code == this.SelectedQuery.Code).length > 0;
                alternativeQuery = this.Queries[0];
            }
            if (!existSelectedQuery) {
                if (!AppTool.IsNullOrEmpty(alternativeQuery)) {
                    this.SelectedQuery = alternativeQuery;
                } else if (!AppTool.IsNullOrEmpty(alternativeUQuery)) {
                    this.SelectedQuery = alternativeUQuery;
                }
            }
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.UniqueCode;
        }
        if (AppTool.IsNullOrEmpty(this.Title) && this.SelectedQuery) {
            this.Title = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
        }
        if (this.SelectedQuery != null) {

            this.SelectedQueryId = this.SelectedQuery.Id;
            this.SelectedQueryCode = this.SelectedQuery.UniqueCode;


            if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
                this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
            }
            else {
                if (this.SelectedQuery.DefaultSortColumn) {
                    this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
                }
            }

            if (this.listArgs && this.listArgs.Filters && this.listArgs.Filters.SortDirection) {
                this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
            }
            else {
                if (this.SelectedQuery.DefaultSortDirection) {
                    this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
                }
            }

            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {
                this.ResourcesLoaded = true;
                this.GetQueryColumns(this.SelectedQuery.UniqueCode, this.UserId);
            });
        }

        this.SetNewEntityButton();
        this.SetAddButton();
    }

    IFSelectedQueryEmpty() {
        return (!this.SelectedQuery && this.Queries && this.Queries.length > 0) ? true : false;
    }


    CheckIfQueriesConatinDefaultPerspectiveQuery() {
        if (this.Queries && !AppTool.IsNullOrEmpty(this.listArgs.Perspective)) {
            var isQueriesConatinDefaultPerspective: boolean = false;
            for (let i = 0; i < this.Queries.length; i++) {
                if (this.Queries[i].UniqueCode == this.SelectedQuery.UniqueCode) {
                    isQueriesConatinDefaultPerspective = true;
                    break;
                }
            }
            if (!isQueriesConatinDefaultPerspective) {
                this.Queries.push(this.SelectedQuery);
            }
        }
    }
    GetQueryColumns(queryCode, userId) {

        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryCode=" + queryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + userId + "&getfromsystemlevel=false")
            .subscribe((response: any) => {
                this.QueryColumns = response;
                this.QueryColumns = this.QueryColumns.sort((a, b) => { return (a.IndexOrder > b.IndexOrder) ? 1 : (a.IndexOrder < b.IndexOrder) ? -1 : 0 });

                this.QueryColumns.forEach((value, key) => {
                    var mutaztouch0 = value.ObjectFieldCode;
                    var mutaztouch14 = window.ObjectFields.filter(x => x.FieldCode === value.ObjectFieldCode);
                    var mutazTouch = window.ObjectFields.filter(x => x.FieldCode === value.ObjectFieldCode)[0];

                    this.columnsObjectFields.push(window.ObjectFields.filter(x => x.FieldCode === value.ObjectFieldCode)[0]);
                });

                if (this.IsSelectAllCheckboxVisible) {
                    this.columns.push({
                        FieldName: "",
                        DataTypeCode: 'String',
                        Display: '',
                        IsCustomTemplate: true,
                        Styles: { width: '27px' },
                        IsCheckBox: true
                    });
                }

                for (var i = 0; i < this.QueryColumns.length; i++) {
                    var CurColumn = this.columns.filter(a => a.FieldName == this.QueryColumns[i].ObjectFieldName);
                    if (this.columns != null && (CurColumn == null || CurColumn.length == 0)) {
                        this.columns.push({
                            FieldName: this.columnsObjectFields[i].FieldName,//this.columnsObjectFields[i].ListPropertyPath ? this.columnsObjectFields[i].ListPropertyPath : this.columnsObjectFields[i].FieldName,
                            DataTypeCode: this.columnsObjectFields[i].DataTypeCode,
                            Display: TextCodeTranslator.Translate(this.columnsObjectFields[i].ListTextCodeCode),
                            Styles: { width: this.QueryColumns[i].ColumnWidth + 'px' },
                            HtmlListComponentName: this.columnsObjectFields[i].HtmlListComponentName, //'TransportModeCellDisplayListTemplate',
                            HtmlListComponentUrl: this.columnsObjectFields[i].HtmlListComponentUrl, //'./Shipment/Components/ListTemplates/TransportModeCellDisplayListTemplate',
                            ServerSideSortable: true, //this.columnsObjectFields[i].CanFilter
                            ColumnHeaderTemplateName: this.columnsObjectFields[i].ColumnHeaderTemplateName, //'TransportModeCellDisplayListTemplate',
                            ObjectField: this.columnsObjectFields[i],
                            QueryCode: queryCode
                            //ColumnHeaderTemplateName: this.columnsObjectFields[i].ColumnHeaderTemplateName, //'./Shipment/Components/ListTemplates/TransportModeCellDisplayListTemplate',
                        });
                    }
                }
                this.CD.detectChanges();
                this.ColumnsReady.emit("ColumnsReady");
            });
    }
    ClearMySearch: boolean = false;
    QueryValueChanged(Args) {
        this.AdvanceFilters = new ApiQueryFilters();
        if (ObjectsLocator.GlobalSetting.WorkEnvironment != "customs") {
            if (Args.IgnoreSearchFields != true) {
                this.searchFields = "";
            }
        }
        this.ClearMySearch = true;
        this.UserQueries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo.LoggedUserTenant);
        this.Title = Args.Title;
        this.columns = [];
        this.columnsObjectFields = [];
        if (this.Queries == null || this.Queries.length == 0) {
            var MyQueries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
            this.SelectedQuery = MyQueries.filter(x => x.UniqueCode === Args.QueryCode)[0];
        }
        else {
            this.SelectedQuery = this.Queries.filter(x => x.UniqueCode === Args.QueryCode)[0];
        }

        if (this.SelectedQuery == null) {
            this.SelectedQuery = this.UserQueries.filter(x => x.UniqueCode === Args.QueryCode)[0];
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.UniqueCode;

            this.MethodName = this.GetMethodName();


            this.SelectedQueryCode = this.SelectedQuery.UniqueCode;
            this.SelectedQueryId = this.SelectedQuery.Id;

            if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
                this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
            }
            else {
                this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
            }
            if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortDirection)) {
                this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
            }
            else {
                this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
            }
            if (window.PreDefinedFilters.filter(d => d.QueryCode == this.SelectedQuery.UniqueCode) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryCode == this.SelectedQuery.UniqueCode);
                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;

                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (filter.DataTypeCode == "DateTime" || filter.DataTypeCode == "Date") {
                        var TommorowDate = DateTool.AddDays((new Date()), 1);
                        TommorowDate.setUTCHours(0, 0, 0, 0);
                        var TodayDate = new Date();
                        TodayDate.setUTCHours(0, 0, 0, 0);
                        if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                        if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                        var YesterdayDate = DateTool.AddDays((new Date()), -1);
                        YesterdayDate.setUTCHours(0, 0, 0, 0);
                        var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                        LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                        var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                        LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 2);
                        CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
                        CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                        var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                        LastYearFromDate.setUTCHours(0, 0, 0, 0);
                        var LastYearToDate = DateTool.AddDays((new Date()), 1);
                        LastYearToDate.setUTCHours(0, 0, 0, 0);

                    
                        if (value1 == "Today") {
                            value1 = TodayDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Yesterday") {
                            value1 = YesterdayDate;
                            value2 = TodayDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last 7 Days") {
                            value1 = LastSevenDaysDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last 30 Days") {
                            value1 = LastThirtyDaysDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Current Year") {
                            value1 = CurrentYearFromDate;
                            value2 = CurrentYearToDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last Year") {
                            value1 = LastYearFromDate;
                            value2 = LastYearToDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "NoDate" || value1 == "No Date") {
                            value1 = "NoDate";
                            filterOperator = "NoDate";
                        }
                        else if (value1 == "Less than Today") {
                            value1 = TodayDate;
                            filterOperator = "LessThan";
                        }
                        else if (value1 == "Less than or equal Today") {
                            value1 = TommorowDate;
                            filterOperator = "LessThan";
                        }
                    }
                    var field = window.ObjectFields.filter(a => a.FieldCode == filter.ObjectFieldCode)[0];
                    if (field) {
                        if (Args.Filters.AdditionalFilters.filter(a => a.FieldName == filter.ObjectFieldName).length > 0) {
                            Args.Filters.AdditionalFilters = Args.Filters.AdditionalFilters.filter(a => a.FieldName != filter.ObjectFieldName);
                        }
                        Args.Filters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    }
                });
            }

            this.SetSelectAllCheckBox(this.SelectedQuery.UniqueCode);

            this.GetQueryColumns(this.SelectedQuery.UniqueCode, this.UserId);
        }

        this.SelectedQueryCode = Args.QueryCode;

        this.dataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "",//"CreateDateTime",
            sortingDir: "",//"Descending",
            getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            },
        };
        if (this.FiltersMenu) {
            this.FiltersMenu.AdditionalFilters.forEach((filter, key) => {
                if (Args.Filters && (filter.IgnoreFilter || filter["SpecificMenuFilter"])) {
                    Args.Filters.AdditionalFilters = Args.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                }
                else {
                    if (Args.Filters == null) {
                        Args.Filters = new ApiQueryFilters();
                    }
                    if (Args.Filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                        Args.Filters.AdditionalFilters = Args.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                    }
                    Args.Filters.AdditionalFilters.push(filter);
                }
            });

        }
        this.onQueryChangeEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: Args.Filters, Reload: true });
        this.SetNewEntityButton();
        this.SetAddButton();
    }
    EnableSpotLight: boolean = false;
    QueriesChangedEvent(Args) {
        if (!Args) {
            this.SelectedQuery = null;
            this.SelectedQueryCode = null;
            this.onSelectedQueryChangeEvent.emit(new QueryPM());
            return;
        }
        this.AdvanceFilters = new ApiQueryFilters();
        this.QueryCode = Args.UniqueCode;

        this.UserQueries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id && x.UserId != null && x.SystemLevel == false && x.Tenant == SessionInfo.LoggedUserTenant && x.QuerySection == this.MenuTableQuerySection);
        this.QueryListSourceChanged.emit(this.UserQueries);
        var SelectedQuery: any = {};
        if (this.QueryCode) {
            SelectedQuery = this.Queries.filter(x => x.UniqueCode === this.QueryCode)[0] != null ? this.Queries.filter(x => x.UniqueCode === this.QueryCode)[0] : this.UserQueries.filter(x => x.UniqueCode === this.QueryCode)[0];
        }
        else {
            SelectedQuery = this.Queries.filter(x => x.IndexOrder === 0)[0] != null ? this.Queries.filter(x => x.IndexOrder === 0)[0] != null : this.UserQueries.filter(x => x.IndexOrder === 0)[0] != null;
        }
        if (SelectedQuery) {
            if (SelectedQuery.QueryGroupCode == "SFLU" || SelectedQuery.QueryGroupCode == "QFLU") {
                ServiceLocator.SendTotangoUserActivity("Shipment", "FollowUpQueryUse");
            }
        }

        this.onSelectedQueryChangeEvent.emit(SelectedQuery);
    }
    NewViewClosedEvent(args) {
        this.IsAdvancedSearchOpened = false;
    }
    public temp1: any;
    private ViewQuery(filterAgrs: ApiQueryFilters, queryDisplayName: string, backButtonLabel: string, readOnlyList: boolean, backToCurrentListView: boolean) {

        if (filterAgrs == null) {
            filterAgrs = new ApiQueryFilters();
        }
        if (!AppTool.IsNullOrEmpty(queryDisplayName)) {
            this.Title = queryDisplayName;
        }
        var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.UniqueCode == this.QueryCode)[0];

        if (query != null) {
            this.temp1 = query;
            if (window.PreDefinedFilters.filter(d => d.QueryCode == query.UniqueCode) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryCode == query.UniqueCode);
                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (value1 == '#logged-user') value1 = SessionLocator.LoggedUserId;

                    if (filter.DataTypeCode == "DateTime" || filter.DataTypeCode == "Date") {

                        var TodayDate = new Date();
                        TodayDate.setUTCHours(0, 0, 0, 0);

                        if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                        if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);

                        var TommorowDate = DateTool.AddDays((new Date()), 1);
                        TommorowDate.setUTCHours(0, 0, 0, 0);
                        var YesterdayDate = DateTool.AddDays((new Date()), -1);
                        YesterdayDate.setUTCHours(0, 0, 0, 0);
                        var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                        LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                        var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                        LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 2);
                        CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
                        CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                        var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                        LastYearFromDate.setUTCHours(0, 0, 0, 0);
                        var LastYearToDate = DateTool.AddDays((new Date()), 1);
                        LastYearToDate.setUTCHours(0, 0, 0, 0);

                        //var TodayDate = new Date();
                        //var YesterdayDate = DateTool.AddDays((new Date()), -1);
                        //var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                        //var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                        //var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                        //var CurrentYearToDate = new Date();
                        //var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                        //var LastYearToDate = new Date();
                        /*
                          case "Today":
                    {
                        this.Text = "Today " + this.Today;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.TodayDate, ToDate: this.TommorowDate, Operation: "Between" });
                        break;
                    }
                case "Yesterday":
                    {
                        this.Text = "Yesterday " + this.Yesterday;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.YesterdayDate, ToDate: this.TodayDate, Operation: "Between" });
                        break;
                    }
                case "Last 7 Days":
                    {
                        this.Text = "Last 7 Days " + this.LastSevenDays;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastSevenDaysDate, ToDate: this.TommorowDate, Operation: "Between" });
                        break;
                    }
                case "Last 30 Days":
                    {
                        this.Text = "Last 30 Days " + this.LastThirtyDays;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastThirtyDaysDate, ToDate: this.TommorowDate, Operation: "Between" });
                        break;
                    }
                case "Current Year":
                    {
                        this.Text = "Current Year " + this.CurrentYear;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.CurrentYearFromDate, ToDate: this.CurrentYearToDate, Operation: "Between" });
                        break;
                    }
                case "Last Year":
                    {
                        this.Text = "Last Year " + this.LastYear;
                        //this.SetDisplayText();
                        this.SelectedItemChanged.emit({ FromDate: this.LastYearFromDate, ToDate: this.LastYearToDate, Operation: "Between" });
                        break;
                    }
                        */
                        if (value1 == "Today") {
                            value1 = TodayDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Yesterday") {
                            value1 = YesterdayDate;
                            value2 = TodayDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last 7 Days") {
                            value1 = LastSevenDaysDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last 30 Days") {
                            value1 = LastThirtyDaysDate;
                            value2 = TommorowDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Current Year") {
                            value1 = CurrentYearFromDate;
                            value2 = CurrentYearToDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "Last Year") {
                            value1 = LastYearFromDate;
                            value2 = LastYearToDate;
                            filterOperator = "Between";
                        }
                        else if (value1 == "NoDate" || value1 == "No Date") {
                            value1 = "NoDate";
                            filterOperator = "NoDate";
                        }
                        else if (value1 == "Less than Today") {
                            value1 = TodayDate;
                            filterOperator = "LessThan";
                        }
                        else if (value1 == "Less than or equal Today") {
                            value1 = TommorowDate;
                            filterOperator = "LessThan";
                        }
                    }
                    var field = window.ObjectFields.filter(a => a.FieldCode == filter.ObjectFieldCode)[0];
                    if (field) {
                        filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    }
                });
            }

            if (!AppTool.IsNullOrEmpty(query.DefaultSortColumn) && AppTool.IsNullOrEmpty(filterAgrs.SortBy)) {
                filterAgrs.SortBy = query.DefaultSortColumn;
            }
            if (!AppTool.IsNullOrEmpty(query.DefaultSortDirection) && AppTool.IsNullOrEmpty(filterAgrs.SortDirection)) {
                filterAgrs.SortDirection = query.DefaultSortDirection;
            }
            //this.dataSource = {
            //    pageSize: 30,
            //    rowCount: null,
            //    sortingCol: "CreateDateTime",
            //    sortingDir: "Descending",
            //    getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            //        return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            //    },
            //};
            //this.FiltersMenu = new ApiQueryFilters();

            if (this.listArgs.SelectedTransportMode != "All") {
                filterAgrs.addAdditionalFilter("TransportModeId", this.listArgs.SelectedTransportMode, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedTransportMode == "All" ? true : false));
            }
            if (this.listArgs.SelectedDirection != "All") {
                filterAgrs.addAdditionalFilter("DirectionId", this.listArgs.SelectedDirection, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedDirection == "All" ? true : false));
            }
            if (this.AdvanceFilters) {
                this.AdvanceFilters.AdditionalFilters.forEach((filter, key) => {
                    filterAgrs.AdditionalFilters.push(filter);
                });
            }
            var ListComponentPostFex = this.ListComponentId.replace('ListComponentId_', '');
            //this.CurrentSession.PubSubFiltersChangeEventService.Stream.emit({ QueryCode: query.UniqueCode, Filters: filterAgrs, ListComponentPostFex: ListComponentPostFex });
            this.FilterChangedEvent.emit({ QueryCode: query.UniqueCode, Filters: filterAgrs, ListComponentPostFex: ListComponentPostFex });
        }
    }
    onMenuHeaderchanged(event) {
        event.AdditionalFilters.forEach((filter, key) => {

            if (filter.FieldName == "TransportModeId") {
                this.listArgs.SelectedTransportMode = filter.FieldValue;
            }
            if (filter.FieldName == "DirectionId") {
                this.listArgs.SelectedDirection = filter.FieldValue;
            }
        });
    }
    HasFilters: boolean = false;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CurrentQueryFilters = new ApiQueryFilters();
        if (filters.AdditionalFilters.length > 0) {
        }
        var MyFilters = new ApiQueryFilters();

        if (this.listArgs.DefaultFilterItems && this.listArgs.DefaultFilterItems.length > 0) {
            this.listArgs.DefaultFilterItems.forEach((filter) => {
                MyFilters.AdditionalFilters.push(filter)
            });
        }

        filters.AdditionalFilters.forEach((filter, key) => {
            if (filter.FieldName == "CompetitorFields")
                filter.Operator = "Contains";
            if (!((filter.FieldDataType == "Decimal" || filter.FieldDataType == "DateTime") && filter.FieldValue == "")) {
                MyFilters.AdditionalFilters.push(filter);
            }

        });
        if (searchfields && !this.IsUseCardSearchMechanism()) {
            MyFilters.Filter1Name = "SearchFields";
            MyFilters.Filter1Operator = "Contains";
            MyFilters.Filter1Value = searchfields;
        }
        MyFilters.GetCount = getCount;
        MyFilters.PageIndex = skip;

        if (this.IsUseCardSearchMechanism()) {
            MyFilters.PageSize = this.ConstantPageSize;
            MyFilters.DontApplyVirtualization = this.DontApplyVirtualization;
        } else MyFilters.PageSize = take;


        MyFilters.SortBy = sortingCol;
        MyFilters.SortDirection = sortingDir;
        this.CurrentQueryFilters = MyFilters;

        return this._entityListService.getByFilters(this.ObjectTableName, MyFilters, this.MethodName == undefined ? null : this.MethodName);
    }

    private isEditControlOpened: boolean = false;
    _DestroyMe: boolean = false;
    public get DestroyMe() {
        return this._DestroyMe;
    }
    public set DestroyMe(val) {
        this._DestroyMe = val;
    }

    onRowSelected($event) {
        if (this.ObjectTableName == "Customs.ConfirmationNumberTokenLog") return;

        if (this.listArgs.SuppressOnRowSelected == true) {
            console.log("SuppressOnRowSelected");
            return;
        }

        if (this.ObjectTableName == "ARPaymentCheque") {
            this._ListComponentArgs.SuppressOnRowSelectedField = true;
        }

        if (this.ObjectTableName == "LedgerTransaction") {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: $event.rowData.JournalId, ObjectTableName: 'Journal' });
                });
            return;
        }
        if (this.ObjectTableName == "Customs.CustomBank") {

            let logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 570;
            logWindow.IsEditComponent = true;
            logWindow.WindowArgs = {
                EntityPM: $event.rowData,
                EntityId: $event.row
            };
            logWindow.EditComponentArguments = { EntityId: $event.rowData.Id, ObjectTableName: 'Customs.CustomBank', IsNew: false, EntityPM: $event.rowData };
            logWindow.Show(this.ObjectTable.NewWizardComponentPath);
            logWindow.WindowClosed.subscribe(($event1: any) => {
                this.DoRefresh();
            });

            return;
        }

        var myObjectTableName = this.ObjectTableName;

        if (this.ObjectTableName == "OccasionContact") {
            myObjectTableName = "Contact";
        }

        if (this._ListComponentArgs.SuppressOnRowSelectedField == true) {
            this._ListComponentArgs.SuppressOnRowSelectedField = false;
            console.log("SuppressOnRowSelectedField");
            return;
        }

        if ($event != null) {
            if (!this.isEditControlOpened) {
                var entityList = $event.rowData;
                var selectedEntityId = $event.rowData.Id;

                switch (myObjectTableName) {
                    case 'Customs.GovernmentProcedureType':
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsHouseType":
                    case "Customs.InternalBorderSiteType":
                    case "Customs.CustomDocumentType":
                    case "Customs.UIMessage":
                    case "Customs.CurrencyType":
                    case "Customs.CustomsCountry":
                    case "Customs.ExceptionReason":
                    case "Customs.ReferantTeam":
                    case "Customs.ReferantTeam":
                    case "Customs.InternalBorderSiteType":
                    case "Customs.CertificateOfOriginMandatoryFields":
                    case "HelpResource":
                    case "PortTimeZone":
                        selectedEntityId = $event.rowData.Code;
                        break;
                    case "Customs.DeclarationReferantData":
                        selectedEntityId = $event.rowData.DeclarationId;
                        break;
                    case "Customs.ExternalFieldMapping":
                    case "Customs.ServersName":
                        selectedEntityId = $event.rowData.Id;
                        break;
                    case "Customs.ExternalFieldMapping":
                        selectedEntityId = $event.rowData.Id;
                        break;


                    default:
                        {
                            break;
                        }
                }

                if (myObjectTableName != "TicketEscalation") {

                    this.isEditControlOpened = true;

                    var myCodes: string[] = [];
                    myCodes.push("EAWB");
                    myCodes.push("BUBK");

                    if (FeatureLocator.IsPackageOneOf(myCodes) && (myObjectTableName == "Shipment" || myObjectTableName == "Master")) {

                        var isFullWizard = false;
                        var windowTitle = null;

                        if (entityList.DirectionId == "E" || entityList.DirectionId == "R") {
                            isFullWizard = true;
                        }

                        else if (entityList.DirectionId == "D") {
                            if (!FeatureLocator.IsPackage_EAWB()) {
                                isFullWizard = true;
                            }
                        }

                        if (isFullWizard) {
                            switch (entityList.ShipmentLevelCode) {
                                case "D": { windowTitle = "Direct AWB Wizard"; break; }
                                case "H": { windowTitle = "House AWB Wizard"; break; }
                                case "C": { windowTitle = "Master AWB Wizard"; break; }
                                default: { break; }
                            }
                        }

                        else {
                            windowTitle = "Airline statuses";
                        }

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 960;
                        logWindow.Height = 600;
                        logWindow.Title = windowTitle;
                        logWindow.WindowArgs = selectedEntityId;
                        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');

                        logWindow.WindowClosed.subscribe(($event1: any) => {
                            $event.isEntityChange = this.getIsEntityChange(logWindow);
                            this.isEditControlOpened = false;
                            this.OnBackFromEdit(selectedEntityId, $event)
                        });
                    }

                    else if (!AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardComponentPath)) {
                        var windowArgs: any = {};
                        windowArgs.EntityId = entityList.Id;
                        windowArgs.IsNew = false;
                        windowArgs.IsNewTemplate = true;

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 960;
                        logWindow.Height = 570;
                        logWindow.WindowArgs = windowArgs;


                        var showHeaderButtons: boolean = true;

                        windowTitle = TextCodeTranslator.Translate("General.O.EditEntity").replace("%Entity", TextCodeTranslator.Translate(myObjectTableName));

                        switch (myObjectTableName) {
                            case "ContainerFollowUp": {
                                showHeaderButtons = false;
                                break;
                            };

                            case "Questionnaire": {
                                windowTitle = $event.rowData.Name;
                                break;
                            }

                            case "DefaultAndConfiguration": {
                                logWindow.Height = 400;
                                break;
                            }

                            case "ApiCredintials": {
                                windowTitle = "Add/Edit Api Credentials";
                                logWindow.Height = 450;
                                logWindow.Width = 650;
                                break;
                            }
                        }

                        logWindow.Title = windowTitle
                        logWindow.ShowHeaderButtons = showHeaderButtons;

                        logWindow.Show(this.SelectedQuery.EditWizardComponentPath);
                        logWindow.WindowClosed.subscribe(($event1: any) => {
                            this.isEditControlOpened = false;
                            this.OnBackFromEdit(selectedEntityId, $event);
                        });
                    }

                    else if (!AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardName) || myObjectTableName == "AgentSharedManifest" || myObjectTableName == "Customs.CourierMaster") {

                        if (this.SelectedQuery.EditWizardName == "SimulatorBookingComponent") {
                            this.ShowINTTRABookingWizard(selectedEntityId, $event);
                        }
                        if (this.SelectedQuery.EditWizardName == "Simplog.ShipmentLib.Views.AWBWizardEditControl") {
                            var isFullWizard = false;
                            var windowTitle = null;

                            if (entityList.DirectionId == "E" || entityList.DirectionId == "R") {
                                isFullWizard = true;
                            }

                            else if (entityList.DirectionId == "D") {
                                if (!FeatureLocator.IsPackage_EAWB()) {
                                    isFullWizard = true;
                                }
                            }

                            if (isFullWizard) {
                                switch (entityList.ShipmentLevelCode) {
                                    case "D": { windowTitle = "Direct AWB Wizard"; break; }
                                    case "H": { windowTitle = "House AWB Wizard"; break; }
                                    case "C": { windowTitle = "Master AWB Wizard"; break; }
                                    default: { break; }
                                }
                            }

                            else {
                                windowTitle = "Airline statuses";
                            }

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 600;
                            logWindow.Title = windowTitle;
                            logWindow.WindowArgs = selectedEntityId;
                            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');

                            logWindow.WindowClosed.subscribe(($event1: any) => {
                                $event.isEntityChange = this.getIsEntityChange(logWindow);
                                this.isEditControlOpened = false;
                                this.OnBackFromEdit(selectedEntityId, $event)
                            });
                        }

                        else if (myObjectTableName == "Customs.CourierMaster") {
                            var windowArgs: any = {};
                            this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => {
                                this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe((response: any) => {
                                    this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {

                                        this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                            res.subscribe((myResponse: any) => {

                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }
                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow();
                                                    logWindow.Width = 1500;
                                                    logWindow.Height = 1000;
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    //logWindow.IsHideHeader = true;
                                                    logWindow.IsFillScreen = true;
                                                    AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
                                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                                        AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
                                                        this.isEditControlOpened = false;
                                                        this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                    this.customsSettingExtendedListService.GetAppSettingByCode("PrimeNG")
                                                        .subscribe((response: ServiceResponse) => {
                                                            if (response.HasError) {
                                                                logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                                                            } else {
                                                                if (response.Result.val == "1") {
                                                                    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetNGComponent');
                                                                } else {
                                                                    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                                                                }
                                                            }


                                                            //logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkspaces/CourierWorksheetNGTComponent');
                                                        });



                                                }
                                            });

                                        });
                                    });
                                });
                            });
                        }

                        else if (!AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardName)) {

                            switch (myObjectTableName) {
                                case 'TenantManagmentPrivateLabels': {
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Edit Private Labels";
                                    logWindow.WindowArgs = selectedEntityId;
                                    logWindow.Show('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/PrivateLabelLoadComponent');

                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                        this.isEditControlOpened = false;
                                        this.OnBackFromEdit(selectedEntityId, $event)
                                    });

                                    break;
                                }


                                case 'Booking': {
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Edit Booking Wizard";
                                    logWindow.WindowArgs = selectedEntityId;
                                    logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');

                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                        this.isEditControlOpened = false;
                                        this.OnBackFromEdit(selectedEntityId, $event)
                                    });

                                    break;
                                }
                                case 'Customer':
                                case 'Card': {
                                    var windowArgs: any = {};
                                    if (this.listArgs.IsDigitalPortalMenuClicked) {
                                        windowArgs.IsDigitalPortal = true;
                                    }
                                    windowArgs.CurrentEntity = entityList;
                                    windowArgs.IsCargoTrackingMenuClicked = this.listArgs.IsCargoTrackingMenuClicked;
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 1100;
                                    logWindow.Height = 570;
                                    logWindow.Title = this.ObjectTableName == "Card" ? "Invite Partners" : "Invite Contacts";
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.Show('./SharedLogistics/Components/InviteCustomersComponent');
                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                        this.isEditControlOpened = false;
                                        this.OnBackFromEdit(selectedEntityId, $event)
                                    });

                                    break;
                                }
                                case 'Customs.Client': {

                                    var windowArgs: any = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.Client").subscribe((response: any) => {

                                        this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                            res.subscribe((myResponse: any) => {

                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }


                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow();
                                                    logWindow.Width = 960;
                                                    logWindow.Height = 570;
                                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Client.O.EditClient");// "Edit Client";
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');

                                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                                        $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as ClientEditComponent).isEntityChange;
                                                        this.isEditControlOpened = false;
                                                        this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                }
                                            });

                                        });
                                    });

                                    break;
                                }
                                case 'Customs.CustomsVendor': {
                                    if (!AppTool.IsNullOrEmpty(selectedEntityId)) {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((response: any) => {
                                            this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe((response: any) => {
                                                this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                                    res.subscribe((myResponse: any) => {

                                                        if (myResponse.HasError) {
                                                            console.log("Error while getting EntityPM", myResponse);
                                                        }
                                                        else {
                                                            var entity = myResponse.Result;
                                                            var logWindow = new LogitudeWindow();

                                                            //Title
                                                            if (!AppTool.IsNullOrEmpty(entity.VendorNumber) && !AppTool.IsNullOrEmpty(entity.VendorName)) {
                                                                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorNumber + "-" + entity.VendorName;

                                                            }
                                                            else if (AppTool.IsNullOrEmpty(entity.VendorNumber)) {
                                                                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorName;

                                                            }
                                                            else if (AppTool.IsNullOrEmpty(entity.VendorName)) {
                                                                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorNumber;
                                                            }
                                                            else if (AppTool.IsNullOrEmpty(entity.VendorName) && AppTool.IsNullOrEmpty(entity.VendorNumber)) {
                                                                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor");
                                                            }
                                                            //title

                                                            var windowArgs: any = {};
                                                            windowArgs.EntityPM = entity;

                                                            logWindow.Width = 960;
                                                            logWindow.Height = 570;
                                                            //logWindow.Title = "Edit Vendor";
                                                            logWindow.WindowArgs = windowArgs;
                                                            logWindow.ShowCloseButton = true;
                                                            logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/VendorEditComponent');

                                                            logWindow.WindowClosed.subscribe(($event1: any) => {
                                                                $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as VendorEditComponent).GENERAL.isEntityChange;
                                                                this.isEditControlOpened = false;
                                                                this.OnBackFromEdit(selectedEntityId, $event);
                                                            });
                                                        }
                                                    });
                                                });
                                            });
                                        });
                                    }

                                    break;
                                }
                                case 'Customs.CustomsCollateral': {

                                    var windowArgs: any = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response: any) => {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe((response: any) => {
                                            this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                                                this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe((response: any) => {
                                                    this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {


                                                        this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                                            res.subscribe((myResponse: any) => {

                                                                if (myResponse.HasError) {
                                                                    console.log("Error while getting EntityPM", myResponse);
                                                                }


                                                                else {
                                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                                    var logWindow = new LogitudeWindow();

                                                                    logWindow.Width = 600;
                                                                    logWindow.Height = 710;
                                                                    //      logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                                    logWindow.WindowArgs = windowArgs;
                                                                    logWindow.ShowCloseButton = true;
                                                                    logWindow.IsHideHeader = true;
                                                                    logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');

                                                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                                                        $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as CustomsCollateralComponent).isEntityChange;
                                                                        this.isEditControlOpened = false;
                                                                        this.OnBackFromEdit(selectedEntityId, $event);
                                                                    });
                                                                }
                                                            });

                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });

                                    break;
                                }
                                case 'Customs.ProceduralFault': {
                                    var windowArgs: any = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.ProceduralFault").subscribe((response: any) => {

                                        this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                            res.subscribe((myResponse: any) => {

                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }
                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow();
                                                    logWindow.Width = 800;
                                                    logWindow.Height = 400;
                                                    //logWindow.Title = TextCodeTranslator.Translate("Customs.ProceduralFault.Q.ProceduralFaults");
                                                    logWindow.Title = "ליקוי מספר " + myResponse.Result.ProceduralFaultNumber;
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    logWindow.Show('./CustomsModules/CustomsProceduralFault/Components/EditTabs/General/ProceduralFaultsGeneralTabComponent');

                                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                                        $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as ProceduralFaultsGeneralTabComponent).isEntityChange;
                                                        this.isEditControlOpened = false;
                                                        this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                }
                                            });

                                        });
                                    });

                                    break;
                                }
                                case "AgentSharedManifest": {
                                    var windowArgs: any = {};

                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 830;
                                    logWindow.Height = 450;
                                    logWindow.Title = "Shared Manifest";
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent");

                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                        this.isEditControlOpened = false;
                                        $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as SharedManifestComponent).isEntityChange;
                                        this.OnBackFromEdit(selectedEntityId, $event);
                                        this.RefreshBtnClick();
                                    });

                                    break;
                                }
                                case "QuoteTemplate": {
                                    var windowArgs: any = {};
                                    var logWindow = new LogitudeWindow();
                                    windowArgs.IsNewEntityCall = false;
                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow();
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = entityList.Name;
                                    logWindow.Width = window.innerWidth - 150;
                                    logWindow.Height = window.innerHeight - 150;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.DataContext = this;
                                    logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");

                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                        this.isEditControlOpened = false;
                                        this.OnBackFromEdit(selectedEntityId, $event);
                                    });

                                    break;
                                }
                                case 'Customs.DeclarationCargoSplit': {

                                    var windowArgs: any = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe((response: any) => {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                                            this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                                res.subscribe((myResponse: any) => {
                                                    if (myResponse.HasError) {
                                                        console.log("Error while getting EntityPM", myResponse);
                                                    }
                                                    else {
                                                        windowArgs.CurrentEntity = myResponse.Result;
                                                        var logWindow = new LogitudeWindow();

                                                        logWindow.Width = 770;
                                                        logWindow.Height = 750;
                                                        //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditDeclarationCargoSplit");
                                                        logWindow.Title = "בקשת פיצול מטען ";// + myResponse.Result != null ? ((!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber) ? myResponse.Result.RequestNumber : null) + ((!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName) ? " - " + myResponse.Result.ResponseStatusName : null))) : null;
                                                        if (myResponse.Result != null) {
                                                            if (!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber)) {
                                                                logWindow.Title = logWindow.Title + myResponse.Result.RequestNumber;
                                                            }
                                                            if (!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName)) {
                                                                logWindow.Title = logWindow.Title + " - " + myResponse.Result.ResponseStatusName;
                                                            }
                                                        }

                                                        logWindow.WindowArgs = windowArgs;
                                                        logWindow.ShowCloseButton = true;
                                                        //logWindow.IsHideHeader = true;
                                                        logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');

                                                        logWindow.WindowClosed.subscribe(($event1: any) => {
                                                            $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as CargoSplitGeneralTabComponent).isEntityChange;
                                                            this.isEditControlOpened = false;
                                                            this.OnBackFromEdit(selectedEntityId, $event);
                                                        });
                                                    }
                                                });

                                            });
                                        });

                                    });

                                    break;
                                }
                                case 'Customs.LogisticActionRequest': {

                                    var windowArgs: any = {};

                                    // this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe((response:any) => {
                                    //     this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
                                    this.entityPMService.getSingle(myObjectTableName, selectedEntityId).then((res: any) => {
                                        res.subscribe((myResponse: any) => {

                                            if (myResponse.HasError) {
                                                console.log("Error while getting EntityPM", myResponse);
                                            }
                                            else {
                                                windowArgs.CurrentEntity = myResponse.Result;
                                                var logWindow = new LogitudeWindow();

                                                logWindow.Width = 920;
                                                logWindow.Height = 750;
                                                //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditDeclarationCargoSplit");
                                                let title: string = (myResponse.Result?.RequestCancelStatus || myResponse.Result?.OperationalStatus);
                                                title = title ? ' - ' + title : '';
                                                logWindow.Title = TextCodeTranslator.Translate('Customs.General.O.CancelExportRequest') + title //TextCodeTranslator.Translate('General.MH.LogisticActionRequest'); //"בקשת פיצול מטען ";// + myResponse.Result != null ? ((!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber) ? myResponse.Result.RequestNumber : null) + ((!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName) ? " - " + myResponse.Result.ResponseStatusName : null))) : null;
                                                logWindow.WindowArgs = windowArgs;
                                                logWindow.ShowCloseButton = true;
                                                //logWindow.IsHideHeader = true;
                                                logWindow.Show('./CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/General/LogisticActionRequestGeneralTabComponent');

                                                logWindow.WindowClosed.subscribe(($event1: any) => {
                                                    $event.isEntityChange = (logWindow.InstanceComponent.ComponentInstance as LogisticActionRequestGeneralTabComponent).isEntityChange;
                                                    this.isEditControlOpened = false;
                                                    this.OnBackFromEdit(selectedEntityId, $event);
                                                });
                                            }
                                        });

                                    });
                                    //     });

                                    // });

                                    break;
                                }
                                default: {
                                    //this.onQueryChangeEvent = new EventEmitter();
                                    //this.Filterchangeevent = new LogEvents.EventManager();
                                    //this.SearchFieldchangeevent = new EventEmitter();
                                    //this.MenuHeaderchangeevent = new EventEmitter();
                                    //this.ColumnsReady = new EventEmitter();
                                    this.isEditControlOpened = false;
                                    break;
                                }
                            }
                        }
                    }

                    else if (myObjectTableName == "BIReport") {
                        SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    DWQueryId: $event.rowData.DWQueryId,
                                    Name: $event.rowData.Name,
                                    ObjectTableName: 'BIReport',
                                    EntityList: $event.rowData,
                                    EntityId: $event.rowData.Id
                                });

                                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                                    $event.isEntityChange = (cmpRef.instance as BIReportPreviewComponent).isEntityChange;
                                    this.isEditControlOpened = false;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                    this.RefreshBtnClick();
                                });
                            });
                    }
                    else if (myObjectTableName == "Customs.Containerization") {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    EntityId: $event.rowData.Id,
                                    ObjectTableName: "Customs.Containerization"
                                });
                                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                                    $event.isEntityChange = cmpRef.instance.isEntityChange;
                                    this.isEditControlOpened = false;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                    // this.RefreshBtnClick();
                                });
                            });
                    }

                    else if (myObjectTableName == "WorkFlow") {
                        var entitypm: WorkFlowPM = new WorkFlowPM();
                        entitypm.Id = $event.rowData.Id
                        entitypm.IsDirty = false;
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({ EntityId: $event.rowData.Id, EntityPM: entitypm, ObjectTableName: 'WorkFlow', BackButtonLabel: "Workflows" });

                                cmpRef.instance.BackCompleted.subscribe(() => {
                                    this.isEditControlOpened = false;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                    //this.RefreshBtnClick();
                                });
                            });
                    }
                    else if (myObjectTableName == "WorkFlowVersion" || myObjectTableName == "WorkFlowInstance") {
                        this.isEditControlOpened = false;
                        this.RowClicked.emit($event);
                    }
                    else if (myObjectTableName == "Task") {

                    }
                    else if (this.ObjectTableName == "Customs.DeclarationReferantData") {
                        var customFile = "";
                        if ($event != null) customFile = $event.rowData.CustomFileNo;
                        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIUFILEFromDeclarationReferantData";
                        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                                .subscribe(
                                    (mess: UnifreightMessageM) => {
                                        var IsMatchUnifreightCallbackCommand = (
                                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                                            mess.LogitudeEntityNumber == selectedEntityId &&
                                            mess.LogitudeViewModel == myViewModelName);
                                        if (IsMatchUnifreightCallbackCommand) {
                                            sub.unsubscribe();
                                            SessionLocator.SelectedSession.StopBusyIndicator();
                                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                                            ///SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                                            this.isEditControlOpened = false;
                                            this.OnBackFromEdit(selectedEntityId, $event);
                                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

                                            //alert("reload");
                                        }
                                    }
                                );

                            SessionLocator.SelectedSession.StartBusyIndicator("");
                            var unifreightMessageM =
                                AmitalGatewayUtil.Instance.
                                    DeclarationMessaging.GetMessage(customFile, selectedEntityId,
                                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());


                            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
                                "CFIHMAIN.LogitudeTask",
                                "ShowCustomFileOPCFromDeclaration",
                                unifreightMessageM,
                                " הצגת מסך :הזנת תיק כללי עמילות מכס");

                        }
                        else {
                            alert("ShowCustomFileOPCFromDeclaration");
                            this.isEditControlOpened = false;
                            this.OnBackFromEdit(selectedEntityId, $event);
                        }

                        /*
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    EntityId: selectedEntityId,///$event.rowData.Id
                                    ObjectTableName: "Customs.Declaration",
                                    BackButtonLabel: label
                                });
                                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                                    this.isEditControlOpened = false;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                });
                                //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                                this.DestroyMe = true;
                                //}

                            });
                            */

                    }
                    else if (this.ObjectTableName == "Customs.DeclarationReferantData") {
                        var customFile = "";
                        if ($event != null) customFile = $event.rowData.CustomFileNo;
                        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIUFILEFromDeclarationReferantData";
                        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                                .subscribe(
                                    (mess: UnifreightMessageM) => {
                                        var IsMatchUnifreightCallbackCommand = (
                                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                                            mess.LogitudeEntityNumber == selectedEntityId &&
                                            mess.LogitudeViewModel == myViewModelName);
                                        if (IsMatchUnifreightCallbackCommand) {
                                            sub.unsubscribe();
                                            SessionLocator.SelectedSession.StopBusyIndicator();
                                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                                            this.isEditControlOpened = false;
                                            this.OnBackFromEdit(selectedEntityId, $event)
                                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

                                            //alert("reload");
                                        }
                                    }
                                );

                            SessionLocator.SelectedSession.StartBusyIndicator("");
                            var unifreightMessageM =
                                AmitalGatewayUtil.Instance.
                                    DeclarationMessaging.GetMessage(customFile, selectedEntityId,
                                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());


                            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
                                "CFIHMAIN.LogitudeTask",
                                "ShowCustomFileOPCFromDeclaration",
                                unifreightMessageM,
                                " הצגת מסך :הזנת תיק כללי עמילות מכס");

                        }

                        else {
                            alert("ShowCustomFileOPCFromDeclaration");
                        }
                    }
                    else if (myObjectTableName == "ContainerFollowUp") {
                        var windowArgs: any = {};
                        windowArgs.EntityId = entityList.Id;
                        windowArgs.IsNew = false;
                        windowArgs.IsNewTemplate = true;

                        windowTitle = TextCodeTranslator.Translate("General.O.EditEntity").replace("%Entity", TextCodeTranslator.Translate(myObjectTableName));

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 960;
                        logWindow.Height = 570;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.Title = windowTitle
                        logWindow.ShowHeaderButtons = false;

                        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWizardComponent");
                        logWindow.WindowClosed.subscribe(($event1: any) => {
                            $event.isEntityChange = logWindow.ComponentRef.instance.isEntityChange;
                            this.isEditControlOpened = false;
                            this.OnBackFromEdit(selectedEntityId, $event);
                        });
                    }
                    else if (myObjectTableName == "GLAccount") {

                        GLAccountSecurityLevelService.CheckLevel(selectedEntityId)
                            .then(hasAccess => {

                                if (hasAccess) {
                                    this.OpenEditComponent(selectedEntityId, myObjectTableName, $event);
                                }
                                else {
                                    this.isEditControlOpened = false;
                                    GLAccountSecurityLevelService.ShowSecurityBockingMessage();
                                    return;
                                }
                            });

                    }
                    else if (myObjectTableName == "Card") {

                        this._entityListService.getAllFromCache("PartnerType", new ApiQueryFilters()).then((res3: any) => {
                            res3.subscribe(res4 => {
                                this.PartnerTypes = res4.Result;
                                var id = $event.rowData['Id'];
                                var table = this.GetObjectTableNameForDependency($event.rowData["PartnerTypeId"], "Card");
                                if (!AppTool.IsNullOrEmpty(id)) {
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Title = "Edit " + TextCodeTranslator.TranslateTable(table);
                                    logWindow.ShowEditComponent(id, table);
                                    logWindow.WindowClosed.subscribe(($event: any) => {
                                        this.isEditControlOpened = false;
                                        this.OnBackFromEdit(selectedEntityId, $event)
                                    });
                                }

                            })
                        });

                    }
                    else {

                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then((cmpRef: ComponentRef<EditComponent>) => {
                                var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    EntityId: selectedEntityId,///$event.rowData.Id
                                    ObjectTableName: myObjectTableName,
                                    BackButtonLabel: label,
                                    QuerySection: this.MenuTableQuerySection,
                                    SelectedQueryCode: this.SelectedQuery.Code
                                });
                                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                                    this.isEditControlOpened = false;
                                    $event.isEntityChange = (cmpRef.instance as any).isEntityChange;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                });
                                //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                                this.DestroyMe = true;
                                //}

                            });
                    }
                }

                if(this.$fastSearchEnable.value)
                    this.fastSearchService.AddHistorySearch('', selectedEntityId).then();
            }            
        }
    }

    private getIsEntityChange(logWindow: LogitudeWindow): any {
        return (((logWindow.InstanceComponent.ComponentInstance as AWBWizardLoadComponent).childComponentInstance as AWBWizardComponent) as any)?.isEntityChange;
    }

    private OpenEditComponent(selectedEntityId: any, myObjectTableName: string, $event: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: selectedEntityId,
                    ObjectTableName: myObjectTableName,
                    BackButtonLabel: label,
                    QuerySection: this.MenuTableQuerySection
                });
                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                    this.isEditControlOpened = false;
                    this.OnBackFromEdit(selectedEntityId, $event);
                });
                this.DestroyMe = true;
            });
    }

    private ShowINTTRABookingWizard(selectedEntityId: string, $event) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "INTTRA e-booking Wizard";
        logWindow.Width = 1020;
        logWindow.Height = 570;
        logWindow.WindowArgs = selectedEntityId;
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorBookingLoadComponent');

        logWindow.WindowClosed.subscribe(($event1: any) => {
            this.isEditControlOpened = false;
            this.OnBackFromEdit(selectedEntityId, $event)
        });
    }

    public MyScrollTop: number = 0;
    public SelectedItem: any;
    public MySelectedRowIndex: number;
    OnBackFromEdit(selectedEntityId, $event) {
        //this.onQueryChangeEvent = new EventEmitter();
        //this.Filterchangeevent = new LogEvents.EventManager();
        //this.SearchFieldchangeevent = new EventEmitter();
        //this.MenuHeaderchangeevent = new EventEmitter();
        //this.ColumnsReady = new EventEmitter();
        //this.isEditControlOpened = false;
        if (this.MethodName != undefined && this.MethodName.indexOf("Customs.") > -1) {
            this.MethodName = this.MethodName.split('.')[1];
        }
        this._entityListService.getSingle(selectedEntityId, this.ObjectTableName, this.MethodName == undefined ? null : this.MethodName).then((res: any) => {
            //var re = res;
            this.DestroyMe = false;
            //this.IsAdvancedSearchOpened = false;
            res.subscribe((aa: any) => {
                const backFromEdid: any = { Data: aa.Result, rowIndex: $event.rowIndex, rowData: $event.rowData, isEntityChange: $event.isEntityChange };

                if (!AppTool.IsNullOrEmpty($event) && !AppTool.IsNullOrEmpty($event.BackFromEdit)) {
                    $event.BackFromEdit.emit(backFromEdid);
                } else {
                    if (AppTool.IsNullOrEmpty($event.rowIndex)) {
                        console.warn('$event.rowIndex is null' + aa.Result)
                    } else {

                        if (this.MyLogGridComponent) { this.MyLogGridComponent.BackFromEditAction({ Data: aa.Result, rowIndex: $event.rowIndex }); }
                        if (this.MyLogGridComponentV2) { this.MyLogGridComponentV2.BackFromEditAction({ Data: aa.Result, rowIndex: $event.rowIndex }); }

                    }

                }

                //this.CurrentSession.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });

                this.MyScrollTop = $event.scrollTop;//($event.rowIndex * $event.rowHeight) - $event.rowHeight;
                this.SelectedItem = aa.Result;
                this.MySelectedRowIndex = $event.rowIndex;
                if (this.sortColDef && this.sortColid) {
                    this.SortServerProp = { colDef: this.sortColDef, id: this.sortColid, isBackFromEdit: true };
                }
            })
        });
        //$event.BackFromEdit.emit({ Data: Data, rowIndex:$event.rowIndex });
        //this.RefreshBtnClick();
        //if (this.ReattachToDetection) {
        //    this.ReattachToDetection = false;
        //}
        //else {
        //    this.ReattachToDetection = true;
        //}
    }

    BackButtonClicked() {

        this.DestroyListControl();
        this.BackCompleted.emit("event");
    }

    DestroyListControl() {
        if (this.ComponentRef != null) {
            this.CurrentSession.RemoveListComponent(this);
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    }

    RefreshBookings() {
        this.BackCompleted.emit(true);
    }

    private showBackButton: boolean = true;
    get BackButtonVisibility() {
        //if (this.ObjectTableName == "Contact" || this.ObjectTableName == "Customer") {
        //    this.showBackButton = false;
        //}
        if (this.listArgs.HideBackButton) {
            this.showBackButton = false;
        }
        return this.showBackButton;
    }

    private showBackButtonAndTitle: boolean = true;
    get IsBackButtonAndTitleVisibile() {
        if (this.listArgs.IsTasksMenuClicked) {
            this.showBackButtonAndTitle = false;
        }
        return this.showBackButtonAndTitle;
    }

    //Add Button
    public IsAddButtonVisible: boolean = false;
    private SetAddButton() {
        var isVisible = false;

        if (this.ObjectTableName == "Warehouse") {
            if (SessionLocator.TenantPM.CountryCode == "US") {
                isVisible = true;
            }

            else if (FeatureLocator.HasFeaturePermession("Warehouse", "AddWarehouses")) {
                isVisible = true;
            }
        }

        else if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline" || this.ObjectTableName == "Port") {
            isVisible = true;
        }

        this.IsAddButtonVisible = isVisible;
    }

    public IsSelectAllCheckboxVisible: boolean = false;
    private SetSelectAllCheckBox(queryCode) {
        var isVisible = false;

        if (this.ScreenQueryActions[queryCode]) {
            isVisible = true;
            this.ScreenQueryAction = this.ScreenQueryActions[queryCode];
        }

        this.IsSelectAllCheckboxVisible = isVisible;

        this.SelectAllRowsChecked(false);
    }

    SelectedFilterChanged($event) {
        if ($event.RowCount) {
            this.dataCount = $event.RowCount;
        }

        this.SelectAllRowsChecked(false);
    }

    SelectAllRowsChecked(selected) {
        this.IsSelected = selected;

        this.SelectedItems.Collection = [];
        this.ExcludedItems.Collection = [];

        this.CalculateSelectedCount();
    }

    // New
    public NewEntityButtonLabel: string = null;
    public IsNewEntityButtonVisible: boolean = false;
    public IsNewEntityButtonDisabled: boolean = false;
    private SetNewEntityButton() {

        if ((!this.HaveFeatureNewExportDeclararion()) || (this.HaveFeatureNewExportDeclararion() && !AmitalGatewayUtil.Instance.AmitalBrowserInUse)) {
            this.SetNewEntityLabel();
            this.SetNewEntityButtonDisabled();
            this.SetNewEntityButtonVisibility();
        }
    }

    private SetNewEntityLabel() {

        if (this.HaveFeatureNewExportDeclararion()) {
            this.NewEntityButtonLabel = TextCodeTranslator.Translate('Customs.General.O.NewExportDeclaration');
        } else
            if (this.listArgs.NewButtonLabel != null) {
                this.NewEntityButtonLabel = this.listArgs.NewButtonLabel;
            }
            else if (this.ObjectTableName == "Customs.LogisticActionRequest") {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate('Customs.General.O.OpenLogisticActionRequest')
            }
            else if (this.MenuTableQuerySection == 'CustomsShipments') {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate('Shipment.O.OpenNewCustomShipment')
            }
            else if (this.ObjectTableName == "InterestReport") {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate('InterestReport.O.NewReport');
            }
            else if (this.ObjectTableName == "OpenFormatReport") {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate('OpenFormatReport.O.NewReport');
            }
            else if (this.ObjectTableName == "Currency") {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate("General.B.Add");
            }
            else if (this.ObjectTableName == "ChargesType") {
                this.NewEntityButtonLabel = TextCodeTranslator.Translate("ChargesType.O.NewChargeType");
            }

            else {
                //this.NewEntityButtonLabel = "New " + TextCodeTranslator.TranslateTable(this.ObjectTableName);
                if (AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                    var tempText = TextCodeTranslator.Translate(this.listArgs.ObjectTableName + ".NewButton");
                    if (!AppTool.IsNullOrEmpty(tempText)) {
                        this.listArgs.NewButtonLabel = tempText;
                    }
                }
                if (AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                    var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
                    if (useLocal == true) {
                        var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");

                        var ChangedText = GeneralText.split('%')[0];
                        if (this.ObjectTableName == "Customs.Vehicle") {

                            ChangedText = TextCodeTranslator.Translate("General.O.New");

                        }


                        var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
                        var FinalText = NewText + " " + ChangedText;
                        if (this.ObjectTableName == "Customer") {
                            FinalText = "New" + " " + NewText;
                        }
                        this.NewEntityButtonLabel = FinalText;//TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));

                    }
                    else {
                        this.NewEntityButtonLabel = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
                    }
                }
                else {
                    this.NewEntityButtonLabel = this.listArgs.NewButtonLabel;
                }
            }


    }
    private SetNewEntityButtonDisabled() {
        var isEnabled = false;

        if (this.SelectedQuery != null) {
            if (this.SelectedQuery.IsAddNewEntityEnabled) {

                isEnabled = true;

                if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
                    var myCodes: string[] = [];
                    myCodes.push("EAWB");
                    myCodes.push("BUBK");

                    if (FeatureLocator.IsPackageOneOf(myCodes)) {
                        isEnabled = false;
                    }
                }
                

                if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString())) {
                    isEnabled = false;

                    if (SessionInfo.LoggedUserPM.IsCustomerCare && (this.ObjectTableName == "User" || this.ObjectTableName == "ChargesType")) {
                        isEnabled = true;
                    }
                }
            }
        }

        this.IsNewEntityButtonDisabled = !isEnabled;
    }

   

    private SetNewEntityButtonVisibility() {

        var isVisible = true;
        if (this.TenantPM.IsHybrid && (this.ObjectTableName == "User" || this.ObjectTableName == "Branche" || this.ObjectTableName == "Department" || this.ObjectTableName == "City" || this.ObjectTableName == "Vessel" || this.ObjectTableName == " Specialservice")) {
            isVisible = false;
        }

        else if (this.SelectedQuery == null && this.ObjectTableName == null) {
            isVisible = false;
        }

        else {
            if (this.SelectedQuery != null) {
                if (this.SelectedQuery.IsNewFromTenantZeroOnly) {
                    if (this.TenantPM.Id != 0) {
                        isVisible = false;
                    }
                }
            }

            if (isVisible) {
                switch (this.ObjectTableName) {
                    case "Customs.PhysicalCheck":
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsSetting":
                    case "Customs.CustomsCollateral":
                    case "Customs.ProceduralFault":
                        {
                            isVisible = false;
                            break;
                        }

                    case "ARPaymentCheque": {
                        isVisible = false;
                        break;
                    }
                    case "Customs.Declaration":
                        {
                            isVisible = false;
                            this.customsSettingListService.getSingleFromCache(this.TenantPM.Id.toString()).subscribe((response: ServiceResponse) => {
                                var list = response.Result;

                                if (list != null) {
                                    if (list.IsConnectedToUniFreight) {
                                        isVisible = false;
                                    }
                                }
                                if (!isVisible && (this.HaveFeatureNewExportDeclararion() || (this.MenuTableQuerySection == "Customs.Declaration" && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ADDNEWDECLARATION")))) {
                                    isVisible = true;
                                }
                                this.IsNewEntityButtonVisible = isVisible;
                            });


                            break;
                        }



                    case "CustomerTenantAccess":
                        {
                            isVisible = false;
                            break;
                        }

                    case "AgentSharedManifest":
                        {
                            isVisible = false;
                            break;
                        }

                    case "Airline":
                    case "ShippingLine":
                        {
                            isVisible = false;
                            break;
                        }

                    case "OccasionContact":
                        {
                            isVisible = false;
                            break;
                        }
                    case "Card":
                        {
                            isVisible = false;
                            break;
                        }
                    case "WorkFlowVersion":
                        {
                            isVisible = false;
                            break;
                        }
                    case "WorkFlowInstance":
                        {
                            isVisible = false;
                            break;
                        }
                }
            }
        }

        this.IsNewEntityButtonVisible = isVisible;
    }

    HaveFeatureNewExportDeclararion(): boolean {
        let b1 = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EXPORTDECLARATIONNEW2");
        let b2 = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EXPORTDECLARATIONPSCREEN");
        if (this.MenuTableQuerySection == "Customs.ExportDeclaration") {

            return b1 && b2;
        }
    }

    AddNewEntity() {

        if (this.ObjectTable?.IsCustom && AppTool.IsNullOrEmpty(this.ObjectTable.ParentObjectTableId) && AppTool.IsNullOrEmpty(this.ObjectTable.NewWizardControlName)) {
            this.ShowAddNewEntityValidationMsg();
            return;
        }
        if (this.SelectedQuery != null) {

            if (!this.CheckPermissions(this.ObjectTableName, "NEW", true) && !this.ObjectTable?.IsCustom) {
                return;
            }

            if (this.TenantPM.Id != 0 && this.ObjectTableName == "Port") {


                var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
                if (useLocal == true) {
                    var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
                    var ChangedText = GeneralText.split('%')[0];


                    var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
                    var FinalText = NewText + " " + ChangedText;
                }
                else {
                    var FinalText = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
                }

                //var title = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
                var message = TextCodeTranslator.Translate("General.M.AddingIsNotAvailable").replace(/%Entity/g, TextCodeTranslator.Translate(this.ObjectTableName));

                var messageWindow = new MessageWindow();
                messageWindow.Width = 450;
                messageWindow.Height = 190;
                messageWindow.Title = FinalText;
                messageWindow.Show(message);
                return;
            }
            else {
                var isNewWizard = this.SelectedQuery.ObjectTableIsNewWizard;
                this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {
                    this.ResourcesLoaded = true;
                    if (isNewWizard) {
                        var IsOriginalMaster: boolean = false;
                        if (!AppTool.IsNullOrEmpty(this.SelectedQuery.OriginalQueryCode)) {
                            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.UniqueCode == this.SelectedQuery.OriginalQueryCode)[0];
                            if (query.Code == "Masters" || query.Code == "Open Payables Masters" || query.Code == "All Masters") {
                                IsOriginalMaster = true;
                            }
                        }
                        if (this.QueryCode == "Masters" || this.QueryCode == "Open Payables Masters" || this.QueryCode == "All Masters" || IsOriginalMaster) {
                            this.RunNewMasterWizard();
                        } else {
                            if (this.SelectedQuery.ObjectTableNewWizardControlName == "Logitude.Customs.NewDeclarationControlCommand") {
                                if (this.HaveFeatureNewExportDeclararion())
                                    this.RunNewExportDeclaration();
                                else if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ADDNEWDECLARATION") && this.MenuTableQuerySection == "Customs.Declaration") {
                                    this.RunNewDeclaration();
                                }

                            } else if (this.ObjectTableName == "Customs.LogisticActionRequest")
                                this.RunNewLogisticActionRequest();

                            else {
                                if (this.SelectedQuery.ObjectTableNewWizardControlName == "Logitude.Customs.NewContainerizationControlCommand") {
                                    this.RunNewContainerization();
                                }

                                else {
                                    this.RunNewEntityWizard(this.SelectedQuery.ObjectTableNewWizardControlName);
                                }
                            }
                        }
                    }
                    else {

                        if (this.ObjectTableName == "APPayment") {
                            this.NewAPPaymentMethod();
                            // APPaymentTools.Create(eventAggregator, viewInjectionService, regionManager, container);
                        } else if (this.ObjectTableName == "Journal") {
                            this.RunNewJournalWizard();
                        } else if (this.ObjectTableName == "UserDefinedReport") {
                            this.RunNewUserDefinedReportWizard();
                        } else if (this.ObjectTableName == "AccountingIntegrityCheck") {
                            this.RunNewAccountingIntegrityCheckWizard();
                        }

                        else if (this.ObjectTableName == "Customs.DeclarationReferantData") {
                            this.RunNewCustomsFileWizard();
                        }
                        else if (this.ObjectTable?.IsCustom) {
                            this.RunNewCustomObjectWizard();
                            return;
                        }

                        else {

                            this.RunNewGenaricEntity();
                        }
                    }

                    ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "New" + this.ObjectTableName);
                });
            }
        }
    }
    private ShowAddNewEntityValidationMsg() {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 450;
        messageWindow.Height = 190;
        messageWindow.Show("You can define the \"New " + this.ObjectTableDisplayName + "\" screen by selecting a one from the Views tab of the " + this.ObjectTableDisplayName + " object in the Customization");
    }
    RunNewExportDeclaration() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate('Customs.General.O.OpenNewDeclarationExport');
        logWindow.Width = 800;
        logWindow.Height = 500;
        logWindow.NewWizardArgs = { IsNewEntity: true };
        logWindow.Show("./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/NewEntity/NewExportDeclarationComponent");
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
    }

    RunNewDeclaration() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate('Customs.Declaration.O.NewDeclaration');
        logWindow.Width = 800;
        logWindow.Height = 500;
        logWindow.NewWizardArgs = { IsNewEntity: true };
        logWindow.Show("./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/NewEntity/NewDeclarationComponent");
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
    }

    RunNewContainerization() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1220;
        logWindow.Height = 550;
        logWindow.Title = ("המכלה חדשה");
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsContainerization/Components/NewEntity/NewContainerizationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnContainerizationWindowClosed($event));
    }
    private OnContainerizationWindowClosed($event: any) {


        if ($event != null && $event != "0" && $event != "cancel") {
            var item = this.CurrentQueryFilters.AdditionalFilters.filter(d => d.FieldName == "Id")[0];

            if (item) {
                var index = this.CurrentQueryFilters.AdditionalFilters.indexOf(item);
                this.CurrentQueryFilters.AdditionalFilters.splice(index, 1);
            }
            this.CurrentQueryFilters.addAdditionalFilter("Id", $event, null, null, "InListExact", false, false, false, "string", false, true);
        }
        this.onQueryChangeEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: this.CurrentQueryFilters, Reload: true });
    }
    private RunNewEntityWizard(wizardControlName: string) {

        var componentPath: string = this.ObjectTable.NewWizardComponentPath;

        if (componentPath != null) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.NewWizardArgs = { IsNewEntity: true };

            switch (this.ObjectTableName) {
                case "BIReport": {
                    logWindow.Width = 1200;
                    logWindow.Height = 780;
                    break;
                }

                case "Customs.Vehicle":
                    {
                        logWindow.Width = 1300;
                        logWindow.Height = 650;
                        logWindow.ShowCloseButton = true;
                        break;
                    }

                case "Customs.Client": {
                    logWindow.Width = 800;
                    logWindow.Height = 500;
                    logWindow.ShowCloseButton = true;
                    break;
                }
                case "Customs.PendingByKeyword": {
                    logWindow.Width = 430;
                    logWindow.Height = 350;
                    logWindow.ShowCloseButton = true;
                    break;
                }
                case "Customs.ExternalFieldMapping": {
                    logWindow.Width = 430;
                    logWindow.Height = 300;
                    logWindow.ShowCloseButton = true;
                    break;
                }
                case "Customs.Declaration":
                case "Customs.PaymentOrder":
                case "Customs.Claim":
                case "Customs.CourierMaster":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 300;
                        logWindow.ShowCloseButton = true;
                        break;
                    }
                case "Customs.DeclarationCargoSplit":
                    {
                        logWindow.Width = 970;
                        logWindow.Height = 750;
                        logWindow.ShowCloseButton = true;
                        break;
                    }

                case "Customs.CustomsVendor": {
                    logWindow.ShowCloseButton = true;
                    break;
                }
                case "User": {
                    logWindow.Width = 965;
                    logWindow.Height = 600;
                    break;
                }
                case "QuoteTemplate": {
                    logWindow.Width = window.innerWidth - 150;
                    logWindow.Height = window.innerWidth - 150;
                    break;
                }

                case "BankDeposit": {
                    logWindow.Width = 520;
                    logWindow.Height = 230;
                    break;
                }
                case "CashBook": {
                    logWindow.Width = 530;
                    logWindow.Height = 400;
                    break;
                }

                case "BankDeposit": {
                    logWindow.Width = 450;
                    logWindow.Height = 350;
                    break;
                }
                case "Customs.CourierPendingReason": {
                    logWindow.Width = 500;
                    logWindow.Height = 520;
                    break;
                }
                case "BankAccount": {
                    logWindow.Width = 500;
                    logWindow.Height = 400;
                    break;
                }
                case "Customs.CustomsAirline":
                case "Customs.CouriersVat":
                    {

                        logWindow.Width = 500;
                        logWindow.Height = 350;
                        break;
                    }
                case "Revaluation":
                case "PaymentCheque": {
                    logWindow.Width = 600;
                    logWindow.Height = 500;
                    break;
                }
                case "TaxWithholdingAssessOffice":
                    {
                        logWindow.Width = 500;
                        logWindow.Height = 400;
                        break;
                    }

                case "TaxReport":

                    {

                        logWindow.Width = 400;
                        logWindow.Height = 200;
                        break;
                    }

                case "TaxDeductionReport":

                    {

                        logWindow.Width = 400;
                        logWindow.Height = 280;
                        break;
                    }
                case "OpenFormatReport":
                    {

                        logWindow.Width = 400;
                        logWindow.Height = 220;
                        break;
                    }
                case "InterestBasesType":
                    {
                        logWindow.Width = 680;
                        logWindow.Height = 400;
                        var windowArgs: any = {};
                        windowArgs.IsNew = true;
                        logWindow.WindowArgs = windowArgs;
                        break;
                    }
                case "AdditionalCurrencyRate":
                case "InterestReport":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 200;
                        break;
                    }
                case "UserDefinedReport":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 200;
                        break;
                    }


            }

            var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
            if (useLocal == true) {
                var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
                var ChangedText = GeneralText.split('%')[0];
                var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
                var FinalText = NewText + " " + ChangedText;
            }
            else {
                var FinalText = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
            }

            //var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            var str = FinalText;

            //if (this.ObjectTableName == "BIReport") {
            //    str = "Query Builder";
            //}

            if (this.ObjectTableName == "Currency") {
                str = TextCodeTranslator.Translate("General.B.Add") + " Currency";
            }

            if (this.ObjectTableName == "Customs.Vehicle") {
                str = TextCodeTranslator.TranslateTable(this.ObjectTableName) + " " + TextCodeTranslator.Translate("General.O.New");
            }

            if (this.ObjectTableName == "Vendor") {
                str = TextCodeTranslator.Translate("Vendor.O.NewVendor");
            }


            if (this.ObjectTableName == "Customs.CustomsVendor") {
                str = TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors");
            }

            if (this.ObjectTableName == "TaxReport") {
                str = TextCodeTranslator.Translate("TaxReport.O.NewTaxReport");
            }

            if (this.ObjectTableName == "BankAccount") {
                str = TextCodeTranslator.Translate("BankAccount.O.NewBankAccount");
            }

            if (this.ObjectTableName == "Revaluation") {
                str = TextCodeTranslator.Translate("Revaluation.O.NewRevaluation");
            }

            if (this.ObjectTableName == "InterestBasesType") {
                str = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
            }

            if (this.ObjectTableName == "TaxDeductionReport") {
                str = TextCodeTranslator.Translate("General.O.NewReport");
            }
            if (this.ObjectTableName == 'Shipment') {
                var args = new NewEntityArgs();
                args.QueryNameTextCode = AppTool.IsNullOrEmpty(this.SelectedQuery) ? null : this.SelectedQuery.QuerySection;
                logWindow.WindowArgs = args;

                str = TextCodeTranslator.Translate('Shipment.O.NewShipment');

                if (this.MenuTableQuerySection == 'CustomsShipments') {
                    logWindow.Width = 650;
                    logWindow.Height = 300;
                }
            }
            else if (this.ObjectTableName == "AdditionalCurrencyRate") {
                str = TextCodeTranslator.Translate("AdditionalCurrencyRate.O.NewAdditionalCurrencyRate");
            }
            else if (this.ObjectTableName == "InterestReport") {
                str = TextCodeTranslator.Translate('InterestReport.O.NewReport');
            }
            else if (this.ObjectTableName == "ChargesType") {
                str = TextCodeTranslator.Translate('ChargesType.O.NewChargeType');
            }
            else if (this.ObjectTableName == "OpenFormatReport") {
                str = TextCodeTranslator.Translate('OpenFormatReport.O.NewReport');
            }

            if (!AppTool.IsNullOrEmpty(this.NewButtonLable)) {
                str = this.NewButtonLable;
            }

            logWindow.Title = str;

            if (!AppTool.IsNullOrEmpty(this.SelectedQuery.Perspective)) {
                var args = new NewEntityArgs();
                args.Perspective = this.SelectedQuery.Perspective;
                args.QueryNameTextCode = AppTool.IsNullOrEmpty(this.SelectedQuery) ? null : this.SelectedQuery.NameTextCodeCode;
                logWindow.WindowArgs = args;
            }

            else {
                if (this.ObjectTableName == "BIReport") {
                    var windowArgs: any = {};
                    windowArgs.IsBIReportWorkspace = true;
                    windowArgs.FolderId = this.listArgs.BIReportFolderId;
                    logWindow.WindowArgs = windowArgs;
                }

                if (this.ObjectTableName == "Tariff") {
                    var QueryCodeOriginal = this.QueryCode;
                    if (!AppTool.IsNullOrEmpty(this.SelectedQuery.OriginalQueryCode)) {
                        var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.UniqueCode == this.SelectedQuery.OriginalQueryCode)[0];
                        QueryCodeOriginal = query.UniqueCode;
                    }

                    var windowArgs: any = {};
                    logWindow.Width = 850;
                    logWindow.Height = 500;

                    switch (QueryCodeOriginal) {
                        case "Tariff.Air Freight Cost Tariffs": {
                            logWindow.Title = "New Air Freight Cost";
                            windowArgs.TypeCode = "AFC";
                            break;
                        }

                        case "Tariff.Air Surcharges Cost Tariffs": {
                            logWindow.Title = "New Air Surcharges Cost";
                            windowArgs.TypeCode = "ASC";
                            break;
                        }

                        case "Tariff.Ocean LCL Freight Cost": {
                            logWindow.Title = "New Ocean LCL Freight Cost";
                            windowArgs.TypeCode = "OLC";
                            break;
                        }

                        case "Tariff.Ocean.LCL.Surcharges.Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.Ocean.LCL.Surcharges.Cost");
                            windowArgs.TypeCode = "OSC";
                            break;
                        }

                        case "Tariff.Ocean FCL Freight Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.OceanFCLFreightCost");
                            windowArgs.TypeCode = "OFC";
                            break;
                        }

                        case "Tariff.Ocean FCL Surcharges Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.OceanFCLSurchargesCost");
                            windowArgs.TypeCode = "OFS";
                            break;
                        }

                        case "Tariff.Import Customs Charges Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.Translate("Tariff.Q.ImportCustomsChargesCost");
                            windowArgs.TypeCode = "ICC";
                            break;
                        }

                        case "Tariff.Export Customs Charges Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.Translate("Tariff.Q.ExportCustomsChargesCost");
                            windowArgs.TypeCode = "ECC";
                            break;
                        }

                        case "Tariff.Inland FTL": {
                            logWindow.Title = "New " + TextCodeTranslator.Translate("Tariff.Q.InlandFTL");
                            windowArgs.TypeCode = "IFT";
                            break;
                        }
                        case "Tariff.Import Local Charges Sale": {
                            logWindow.Title = "New " + TextCodeTranslator.Translate("Tariff.Q.ImportLocalChargesSale");
                            windowArgs.TypeCode = "ICS";
                            break;
                        }
                        case "Tariff.Export Local Charges Sale": {
                            logWindow.Title = "New " + TextCodeTranslator.Translate("Tariff.Q.ExportLocalChargesSale");
                            windowArgs.TypeCode = "ECS";
                            break;
                        }
                    }

                    logWindow.WindowArgs = windowArgs;
                }

                if (this.ObjectTableName == "Questionnaire" || this.ObjectTableName == "CustomerFieldsUpdateSetting") {
                    var windowArgs: any = {};
                    windowArgs.IsNew = true;
                    logWindow.WindowArgs = windowArgs;
                }
            }

            //if (this.ObjectTableName == "BIReport") {
            //    logWindow.ComponentLoaded.subscribe(s => {
            //        logWindow.WindowClosed.subscribe(d => {
            //            if (s != null && d != "cancel") {
            //                SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            //                    .then(cmpRef => {
            //                        cmpRef.instance.ComponentRef = cmpRef;
            //                        cmpRef.instance.Run({
            //                            DWQueryId: s.QID,
            //                            ObjectTableName: 'BIReport',
            //                            EntityId: null,
            //                            FolderId: this.listArgs.BIReportFolderId
            //                        });

            //                        cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
            //                            //this.isEditControlOpened = false;
            //                            //this.OnBackFromEdit(selectedEntityId, $event);
            //                            this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
            //                        });
            //                    });
            //            }
            //        });
            //    });
            //}

            //else {
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            //}

            logWindow.Show(componentPath);
        }

        else {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Show("Fill NewWizard Component Path and Name in ObjectTable !!");
        }
    }

    RunNewLogisticActionRequest() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 750;
        logWindow.Title = TextCodeTranslator.Translate('Customs.General.O.NewCancelExport');
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/General/LogisticActionRequestGeneralTabComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
    }

    private RunNewGenaricEntity() {

        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.ObjectTableName).then(response => {

            var args = new EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = this.ObjectTableName;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;


            //var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
            //var ChangedText = GeneralText.split('%')[0];
            //var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
            //var FinalText = NewText + " " + ChangedText;
            var FinalText = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
            var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
            if (useLocal == true) {
                var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
                var ChangedText = GeneralText.split('%')[0];
                var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
                FinalText = NewText + " " + ChangedText;

            }

            if (this.ObjectTableName == "InterestBasesType") {
                FinalText = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
            }


            var windowTitle = FinalText; //TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }
    private OnNewEntityWindowClosed($event: any) {
        this.onQueryChangeEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: this.CurrentQueryFilters });
    }

    ShowIt: boolean = true;
    ImportEntitiesCommand() {
        var windowTitle = "Add " + this.ObjectTableName;
        var logWindow = new LogitudeWindow();
        var args: ImportEntityArgs = new ImportEntityArgs();
        args.ObjectTableId = this.ObjectTable.Id;
        args.ObjectTableName = this.ObjectTableName;
        logWindow.WindowArgs = args;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
        this.ShowIt = false;
        logWindow.WindowClosed.subscribe(($event: any) => {

            this.CD.detectChanges();
        });
    }

    btnExcelCLicked() {
        if (!this.CheckPermissions(this.ObjectTableName, "READ", true)) {
            return;
        }
        else {
            var windowArgs: any = {};
            windowArgs.query = this.SelectedQuery;
            windowArgs.currentObjectTable = this.ObjectTableName;
            windowArgs.tenant = SessionInfo.LoggedUserTenant;
            windowArgs.userid = SessionInfo.LoggedUserId;
            this.CurrentQueryFilters.addAdditionalFilter("IsXslxFormat", true, null, null, "Equal", true, false, false, "string");
            windowArgs.Type = "SaveToMicrosoftExcel2007";
            windowArgs.Filters = this.CurrentQueryFilters
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 200;
            logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");//"Exporting View Data List To Excel File";
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
            //logitudeWindow.WindowClosed.subscribe(($event: any) => {
            //    this.QueryValueChanged({ QueryId: this.SelectedQueryId })
            //});
            //});
        }

    }

    RefreshBtntimerToken: any;
    RefreshBtnClick() {
        if (this.RefreshBtntimerToken) {
            clearTimeout(this.RefreshBtntimerToken);
        }
        this.RefreshBtntimerToken = setTimeout(() => this.DoRefresh(), 1000);
    }

    DoRefresh() {
   
        this.MyScrollTop = 0;
        this.MySelectedRowIndex = null;
        this.CurrentQueryFilters = new ApiQueryFilters();//this.listArgs.Filters;
        if (this.CurrentQueryFilters == null) {
            this.CurrentQueryFilters = new ApiQueryFilters();
        }
        if (!this.CheckPermissions(this.ObjectTableName, "READ", true) && !this.ObjectTable?.IsCustom) {
            return;
        }
        else {
            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.UniqueCode == this.SelectedQueryCode)[0];
            if (query != null) {

                if (window.PreDefinedFilters.filter(d => d.QueryCode == query.UniqueCode) != null) {
                    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryCode == query.UniqueCode);
                    predefinedFilters.forEach((filter, key) => {
                        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                        var value1 = filter.PredefinedValue;
                        var value2 = filter.PredefinedValue2;
                        if (value2 != null) {
                            filterOperator = "Between";
                        }
                        if (filter.DataTypeCode == "DateTime" || filter.DataTypeCode == "Date") {
                            var TodayDate = new Date();
                            TodayDate.setUTCHours(0, 0, 0, 0);

                            if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                            if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);

                            var TommorowDate = DateTool.AddDays((new Date()), 1);
                            TommorowDate.setUTCHours(0, 0, 0, 0);
                            var YesterdayDate = DateTool.AddDays((new Date()), -1);
                            YesterdayDate.setUTCHours(0, 0, 0, 0);
                            var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                            var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 2);
                            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                            var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
                            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                            var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                            LastYearFromDate.setUTCHours(0, 0, 0, 0);
                            var LastYearToDate = DateTool.AddDays((new Date()), 1);
                            LastYearToDate.setUTCHours(0, 0, 0, 0);

                            if (value1 == "Today") {
                                value1 = TodayDate;
                                value2 = TommorowDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "Yesterday") {
                                value1 = YesterdayDate;
                                value2 = TodayDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "Last 7 Days") {
                                value1 = LastSevenDaysDate;
                                value2 = TommorowDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "Last 30 Days") {
                                value1 = LastThirtyDaysDate;
                                value2 = TommorowDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "Current Year") {
                                value1 = CurrentYearFromDate;
                                value2 = CurrentYearToDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "Last Year") {
                                value1 = LastYearFromDate;
                                value2 = LastYearToDate;
                                filterOperator = "Between";
                            }
                            else if (value1 == "NoDate" || value1 == "No Date") {
                                value1 = "NoDate";
                                filterOperator = "NoDate";
                            }
                            else if (value1 == "Less than Today") {
                                value1 = TodayDate;
                                filterOperator = "LessThan";
                            }
                            else if (value1 == "Less than or equal Today") {
                                value1 = TommorowDate;
                                filterOperator = "LessThan";
                            }
                        }
                        var field = window.ObjectFields.filter(a => a.FieldCode == filter.ObjectFieldCode)[0];
                        this.CurrentQueryFilters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    });
                }

                if (!AppTool.IsNullOrEmpty(query.DefaultSortColumn) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortBy)) {
                    this.CurrentQueryFilters.SortBy = query.DefaultSortColumn;
                }

                if (!AppTool.IsNullOrEmpty(query.DefaultSortDirection) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortDirection)) {
                    this.CurrentQueryFilters.SortDirection = query.DefaultSortDirection;
                }
            }
            if (this.FiltersMenu) {
                this.FiltersMenu.AdditionalFilters.forEach((filter, key) => {
                    if (this.CurrentQueryFilters && filter.IgnoreFilter) {
                        this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                    }
                    else {
                        if (this.CurrentQueryFilters == null) {
                            this.CurrentQueryFilters = new ApiQueryFilters();
                        }
                        if (this.CurrentQueryFilters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                            this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                        }
                        this.CurrentQueryFilters.AdditionalFilters.push(filter);
                        //this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, false, filter.DisplayInList, false, filter.FieldDataType);
                    }
                });

                //this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.concat(this.FiltersMenu.AdditionalFilters);
            }
            if (this.AdvanceFilters) {
                this.AdvanceFilters.AdditionalFilters.forEach((filter, key) => {
                    this.CurrentQueryFilters.AdditionalFilters.push(filter);
                });
            }
            this.onQueryChangeEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: this.CurrentQueryFilters, Reload: false });
            this.onRefershQueryEvent.emit({ QueryCode: this.SelectedQueryCode, Filters: this.CurrentQueryFilters, Reload: false });

            //     else {
            //         this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
            //     }

            //this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
        }
    }

    ColumnResisedevent(Param) {
        //var QColumn = this.QueryColumns.filter(a => a.ObjectFieldName == Param.FieldName)[0];
        //if (Param.Width > 0) {
        //    QColumn.ColumnWidth = Param.Width;
        //}
        //QColumn.IndexOrder = Param.index;
        //if (this.myQueryColumnsPMService == null) {
        //    this.myQueryColumnsPMService = new QueryColumnsPMService();
        //    this.myQueryColumnsPMService.setServiceArgs(this.serviceArgs);
        //}
        //this.myQueryColumnsPMService.update(QColumn).subscribe((myResult:any) => {
        //});
        this.SaveColNewChanges(Param);
        //console.log("Oh Yea !!");
    }

    SaveColNewChanges(Param: any) {
        var QColumns = null;
        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryCode=" + Param.QueryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionLocator.LoggedUserId + "&getfromsystemlevel=false")
            .subscribe((response: any) => {
                QColumns = response;
                if (QColumns != null) {
                    if (this.GeneralEntitiesArgs == null) {
                        this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
                    }
                    this.GeneralEntitiesArgs.QueryColumnsPMs = [];
                    if (!AppTool.IsNullOrEmpty(QColumns[0].UserId)) {

                        if (this.myQueryColumnsPMService == null) {
                            this.myQueryColumnsPMService = new QueryColumnsPMService();
                            this.myQueryColumnsPMService.setServiceArgs(this.serviceArgs);
                        }

                        QColumns.forEach((querycolumn, key) => {
                            querycolumn.IndexOrder = Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Index;
                            if (Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Width > 0) {
                                querycolumn.ColumnWidth = Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Width;
                            }
                            else if (SessionLocator.HomeComponent.SelectedTabItem.Index && Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[SessionLocator.HomeComponent.SelectedTabItem.Index].Width > 0) {
                                querycolumn.ColumnWidth = Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[SessionLocator.HomeComponent.SelectedTabItem.Index].Width;
                            }
                            this.GeneralEntitiesArgs.QueryColumnsPMs.push(querycolumn);
                        });
                        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
                        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
                        myGeneralService.setServiceArgs(this.serviceArgs);
                        myGeneralService.update(this.GeneralEntitiesArgs).subscribe((myResult: any) => {
                        });
                    }
                    else {
                        QColumns.forEach((querycolumn, key) => {
                            //if (Param.Width > 0 && Param.FieldName == querycolumn.ObjectFieldName) {
                            //    querycolumn.ColumnWidth = Param.Width;
                            //}
                            querycolumn.IndexOrder = Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Index;
                            if (Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Width > 0) {
                                querycolumn.ColumnWidth = Param.ColIndexes.filter(a => a.FieldName == querycolumn.ObjectFieldName)[0].Width;
                            }
                            querycolumn.UserId = SessionInfo.LoggedUserId;
                            querycolumn.Tenant = SessionInfo.LoggedUserTenant;
                            this.GeneralEntitiesArgs.QueryColumnsPMs.push(querycolumn);
                        });
                        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
                        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
                        myGeneralService.setServiceArgs(this.serviceArgs);
                        myGeneralService.insert(this.GeneralEntitiesArgs).subscribe((myResult: any) => {
                        });
                        // });
                    }
                }
            });
    }

    RunNewJournalWizard() {
        var windowTitle = "New Journal";
        var entityPM: JournalPM = new JournalPM();
        entityPM.IsNew = true;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.FullAccounting")
                });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    //this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                    this.RefreshBtnClick();

                });
            });
    }

    RunNewUserDefinedReportWizard() {
        var entityPM: UserDefinedReportPM = new UserDefinedReportPM();
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'UserDefinedReport'
                });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    //this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                    this.RefreshBtnClick();

                });
            });
    }

    RunNewCustomsFileWizard() {
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.NewCustomsFileScreen(
                "ShowCFIFILEMMoveSIToOCRScreen");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "NewCustomsFileScreen";
            myMessageWindow.Show(mess);
            this.RefreshBtnClick();
        }
    }

    RunNewMasterWizard() {
        var componentPath: string = "./Shipment/Components/NewEntity/NewMasterComponent";
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Master";
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.NewWizardArgs = { IsNewEntity: true };
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
        logWindow.Show(componentPath);
    }

    private RunNewCustomObjectWizard() {
        var componentPath: string = "./Infrastructure/Components/NewEntity/NewCustomObjectComponent";
        var logWindow = new LogitudeWindow();
        logWindow.Title = this.NewEntityButtonLabel;
        logWindow.Width = 950;
        logWindow.Height = 530;

        logWindow.WindowArgs = {
            ObjectTablePM: this.ObjectTable,
            FatherComponent: this,
        }
        logWindow.NewWizardArgs = { IsNewEntity: true };
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
        logWindow.Show(componentPath);
    }

    RunNewAccountingIntegrityCheckWizard() {
        var __entity: AccountingIntegrityCheckPM = new AccountingIntegrityCheckPM();
        __entity.StatusCode = "1";
        __entity.HasException = false;
        __entity.CreateDateTimeUTC = new Date();
        __entity.Tenant = this.Tenant;

        var FinalText = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
        var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        if (useLocal == true) {
            var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
            var ChangedText = GeneralText.split('%')[0];
            var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
            FinalText = NewText + " " + ChangedText;
        }
        var windowTitle = FinalText;

        var windowArgs = {
            EntityPM: __entity
        };

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 240;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshBtnClick();
        });
        logWindow.Show('./Accounting/Components/NewEntity/NewIntegrityCheckComponent');

        // SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        // .then(cmpRef => {
        //     cmpRef.instance.ComponentRef = cmpRef;
        //     cmpRef.instance.Run({ EntityPM: __entity, ObjectTableName: 'AccountingIntegrityCheck' });
        //     cmpRef.instance.BackCompleted.subscribe(($event: any) => {
        //         this.RefreshBtnClick();
        //     });
        // });
    }

    // Run New APPaymnet
    NewAPPaymentMethod() {
        var newApPaymentPM: APPaymentPM = new APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, BackButtonLabel: 'A/P Payments', ObjectTableName: 'APPayment' });
            });
    }


    IsUseCardSearchMechanism() {
        var result: boolean = false;
        if (this.ObjectTableName == "Customer" && ObjectsLocator.GlobalSetting.WorkEnvironment != "customs") {
            result = true;
        }
        return result;
    }

    private currentFilters: ApiQueryFilters;
    private currentSearchFields: string;
    private currentSortingCol: string;
    private currentSortingDir: string;
    Navigate() {

        this.CurrentQueryFilters = new ApiQueryFilters();

        var MyFilters = new ApiQueryFilters();

        if (this.listArgs.DefaultFilterItems && this.listArgs.DefaultFilterItems.length > 0) {
            this.listArgs.DefaultFilterItems.forEach((filter) => {
                MyFilters.AdditionalFilters.push(filter)
            });
        }

        this.currentFilters.AdditionalFilters.forEach((filter, key) => {
            if (filter.FieldName == "CompetitorFields")
                filter.Operator = "Contains";
            MyFilters.AdditionalFilters.push(filter);
        });
        if (this.currentSearchFields) {
            MyFilters.Filter1Name = "SearchFields";
            MyFilters.Filter1Operator = "Contains";
            MyFilters.Filter1Value = this.currentSearchFields;
        }
        MyFilters.GetCount = false;
        MyFilters.PageIndex = 0;
        MyFilters.PageSize = 100;
        MyFilters.SortBy = this.currentSortingCol;
        MyFilters.SortDirection = this.currentSortingDir;
        this.CurrentQueryFilters = MyFilters;
        var ids: string[] = [];
        this._entityListService.getByFilters(this.ObjectTableName, MyFilters, this.MethodName == undefined ? null : this.MethodName).then((observable: Observable<any>) => {


            observable.subscribe((response: ServiceResponse) => {
                console.log(response);

                response.Result.forEach((item) => {
                    ids.push(item.Id);
                });

                console.log(ids);


                var selectedEntityId = ids[0];
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            EntityId: selectedEntityId,///$event.rowData.Id
                            ObjectTableName: this.ObjectTableName,
                            BackButtonLabel: label,
                            NavigationIds: ids,

                        });
                        cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                            this.isEditControlOpened = false;
                            this.DestroyMe = false;
                            // this.OnBackFromEdit(selectedEntityId, $event)
                        });
                        //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                        this.DestroyMe = true;
                        //}

                    });
            });

        });
    }

    public SortServerProp: any;
    public sortColDef: any;
    public sortColid: any;
    OnSortInvoked($event) {
        this.sortColDef = $event.colDef;
        this.sortColid = $event.id;
    }

    HasActionBar() {//ADD TO LXML\METADATA OBJECTTABLE- to be continue
        switch (this.ObjectTable.Name) {
            case "Customs.DeclarationReferantData":
            case "Customs.PhysicalCheck":
            case "Customs.LogisticActionRequest":
                return true;
                //return false;
                break;
            default:
                return false;
        }
    }
    LoadedActionBar(locationCode: string, prefixComponent: string) {
        if (!this.HasActionBar()) { return; }
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == locationCode/*"MNH"*/)[0];
        if (myLocation != null) {

            let myObjectTableName = this.ObjectTable.Name;
            if (myObjectTableName.startsWith(this.ObjectTable.ClientModuleName + '.')) {
                myObjectTableName = myObjectTableName.substr((this.ObjectTable.ClientModuleName + '.').length)
            }
            var myComponentPath = "./" + this.ObjectTable.ClientModuleName
                //+ "/Components/FiltersMenu/" + myObjectTableName + "FiltersMenuComponent";
                + "/Components/" + prefixComponent + "/" + myObjectTableName + prefixComponent + "Component";
            SessionLocator.DynamicLoader.Load(myComponentPath, myLocation.viewContainerRef)
                .then(cmpRef => {

                    //this.FiltersBarLoaded.emit(cmpRef.instance);

                    // event not needed - meanwhile ?!?!

                });
        }
    }

    public IsSelected: boolean = false;
    public SelectedItems: ObservableCollection;
    public ExcludedItems: ObservableCollection;
    public SelectedCount: number = 0;

    onCheckBoxChecked($event) {
        if ($event.IsChecked) {

            if (!this.SelectedItems.Collection.includes($event.rowData.Id)) {
                this.SelectedItems.Insert($event.rowData.Id);

                if (this.IsSelected) {
                    if (this.ExcludedItems.Collection.includes($event.rowData.Id)) {
                        this.ExcludedItems.Remove($event.rowData.Id);
                    }
                }
            }
        }
        else {
            if (this.SelectedItems.Collection.includes($event.rowData.Id)) {
                this.SelectedItems.Remove($event.rowData.Id);
            }

            if (this.IsSelected) {
                if (!this.ExcludedItems.Collection.includes($event.rowData.Id)) {
                    this.ExcludedItems.Insert($event.rowData.Id);
                }
            }
        }

        this.CalculateSelectedCount();
    }

    CalculateSelectedCount() {
        this.SelectedCount = this.IsSelected ? this.dataCount - this.ExcludedItems.Collection.length : this.SelectedItems.Collection.length;
        this.SelectedRows.emit(this.IsSelected || this.SelectedItems.Collection.length);
    }

    ShowActionConfirmationWindow(action) {

        var confirmWindow = new ConfirmWindow();
        var confirmMsg;
        if (this.IsSelected) {
            confirmMsg = "נבחרו כל הצהרות ל{actionTranslation}, הםם להמשיך? ";
        }
        else {
            confirmMsg = "נבחרו {count} הצהרות ל{actionTranslation}, הםם להמשיך? "
                .replace("{count}", this.SelectedCount.toString());
        }
        confirmMsg = confirmMsg.replace("{actionTranslation}", TextCodeTranslator.Translate(this.ScreenQueryAction["actionTranslationPrefix"] + action));
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Width = 400;
        confirmWindow.Height = 180;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Show(confirmMsg);

        var params = {
            Action: action,
            IsAllSelected: this.IsSelected,
            SelectedIds: this.IsSelected ? this.ExcludedItems.Collection : this.SelectedItems.Collection,
            LoggingUserId: SessionLocator.LoggedUserId
        };

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (this.ObjectTable.Name == "Customs.Declaration") {
                    this._declarationWebService.PostActionOnDeclarationBatch(params, this.CurrentQueryFilters).subscribe((response: any) => {

                        // deselect the rows
                        if (this.SelectedItems.Collection.length > 0) {
                            let emittedArray = this.SelectedItems.Collection.map((res) => ({ rowData: { Id: res }, IsChecked: false, RowIndex: -1, ById: true }));
                            this.onChangeCheckBoxesState.emit(emittedArray);
                        }
                        if (this.ExcludedItems.Collection.length > 0) {
                            let emittedArray = this.ExcludedItems.Collection.map((res) => ({ rowData: { Id: res }, IsChecked: false, RowIndex: -1, ById: true }));
                            this.onChangeCheckBoxesState.emit(emittedArray);
                        }
                        this.SelectAllRowsChecked(false);
                        SessionLocator.SelectedSession.StopBusyIndicator();

                        // show message
                        var myMessageWindow = new MessageWindow();
                        if (!AppTool.IsNullOrEmpty(response.RequestInProgressList)) {
                            myMessageWindow.ShowEventButton = true;
                            myMessageWindow.EventButtonText = TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                        }
                        myMessageWindow.Show(response.Message);
                        // myMessageWindow.WindowClosed.subscribe(s => {
                        //     this.RefreshButtonClicked();
                        // });
                        // myMessageWindow.SendEvent.subscribe(s=>{
                        //     if(s){
                        //         this.LoadCustomsRequestSheetsScreen(response.RequestInProgressList)
                        //     }
                        // });
                    });
                }
            }
        });
    }

    MultiPrintClicked() {
        if (!IsMultiPrintValid(this.ObjectTable.DBTableName, this.dataSource.rowCount)) return;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1050;
        logitudeWindow.Height = 700;
        logitudeWindow.Title = "Batch Print " + this.ObjectTable.DBTableName;

        var windowArgs: any = {};
        windowArgs.QueryCode = this.SelectedQueryCode;
        windowArgs.Filters = this.CurrentQueryFilters;
        windowArgs.Columns = this.columns;
        windowArgs.ObjectTable = this.ObjectTable;
        windowArgs.Title = this.Title;

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/MultiPrint/MultiPrintMainComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshBtnClick();
        });
    }
    private PartnerTypes: Array<any> = [];

    private GetObjectTableNameForDependency(dependency: string, parentObjectName: string) {

        if (parentObjectName == "Card" && dependency == "PO") {
            dependency = "CS";
        }

        var partnerType = this.PartnerTypes.filter(p => p.Id.toLowerCase() == dependency.toLowerCase())[0];
        if (partnerType != null && partnerType != undefined) {
            var name: string = partnerType.Name.replace(" ", "");
            var table: ObjectTablePM = window.ObjectTables.filter(d => d.Name.toLowerCase() === name.toLocaleLowerCase())[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }

    }
}
