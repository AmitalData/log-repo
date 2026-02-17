
declare var System: any;
declare var window: any;
import { Component, OnInit, Type, Output, EventEmitter, ComponentRef, ViewChild, QueryList, ViewChildren, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import * as Rx from 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { FormControl } from '@angular/forms';
//import {CORE_DIRECTIVES, Control, NgFormControl} from '@angular/common';
//import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
//import {IconButton} from '../../../Controls/IconButton';
//import {LogGridComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent';
//import {AdvanceSearchComponent} from '../../../Infrastructure/Components/AdvanceSearchComponent/AdvanceSearchComponent';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { Http } from '@angular/http';
import { ServiceArgs } from '../../DataContracts/ServiceArgs';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
//import {SearchTextBox} from '../../../Controls/SearchTextBox';
import { LogEvents } from '../../../Infrastructure/Utilities/LogEvents';
import { PubSubService } from '../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import { PubSubService1 } from '../../../Infrastructure/Utilities/events/ApiFiltersEvent1';
//import {QueryListComponent} from '../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/QueryListComponent';
import { AppTool, DateTool } from '../../Tools';
import { ListComponentArgs, NewEntityArgs } from '../../Args';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { EntityPMService } from '../../Services/EntityPMService';
import { TotangoService } from '../../Services/WebServices/TotangoService';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { QueryColumnsPMService } from '../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService';
import { QueryColumnPM } from '../../../Infrastructure/EntityPMs/QueryColumnPM';
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
import { AmitalGatewayUtil } from '../../Utilities/AmitalGatewayUtil';
import { AccountingIntegrityCheckPM } from '../../../Accounting/EntityPMs/AccountingIntegrityCheckPM';

@Component({
    moduleId: module.id,

    templateUrl: './ListComponent.html',
    //directives: [CORE_DIRECTIVES, IconButton, LogGridComponent, NgFormControl, AdvanceSearchComponent, QueryListComponent, LocationDirective, SearchTextBox],
    //pipes: [TextCodeTranslationPipe],
    providers: [EntityListService, EntityResourceService, PubSubService, PubSubService1, EntityPMService, TotangoService],
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
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    public SeachBoxIsDisabled: boolean = false;
    //@Output() ShowTipEvent = new EventEmitter();
    public IsNavigateButtonVisible: boolean = false;
    public ComponentRef: ComponentRef<ListComponent>;
    public ReattachToDetection: boolean;
    public columnsObjectFields: any[] = [];
    public rowCount: number;
    public items: any[] = [];
    public columns: any[] = [];
    public SearchText: string = "Search Partners / Ports / Ref.#";
    public SearchTextValue: FormControl;
    public EntityService: Type<any>;
    public IsAdvancedSearchOpened: boolean = false;
    LayoutDirection: string = 'ltr';
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService();

    public IsShowTipArea: boolean = false;
    public IsShowTipIcon: boolean = false;
    public IsFirstTipLoad: boolean = false;

    //public Title: string;
    private title: string;//= "";
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
    CurrentQueryFilters: ApiQueryFilters;
    AdvanceFilters: ApiQueryFilters;
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() onSelectedQueryChangeEvent = new EventEmitter();
    onOpenFilterAreaClick() {
        this.IsAdvancedSearchOpened = true;
    }
    onCloseFilterAreaClick() {
        this.IsAdvancedSearchOpened = false;
    }
    onColumnsClick() {
        var windowArgs: any = {};
        windowArgs.queryId = this.SelectedQueryId;
        windowArgs.isNewQueryMode = false;
        windowArgs.currentObjectTable = this.ObjectTableName;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 520;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.O.QueryColumnsEdit");//"Query Columns Edit";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/QueryColumnsComponents/QueryColumnsEditComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            //var myfilterAgrs = this.CurrentQueryFilters;
            // if (this.AdvanceFilters) {
            //     this.AdvanceFilters.AdditionalFilters.forEach((filter, key) => {
            //         myfilterAgrs.AdditionalFilters.push(filter);
            //     });
            // }
            this.QueryValueChanged({ QueryId: this.SelectedQueryId, Title: TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode), Filters: this.CurrentQueryFilters, IgnoreSearchFields: true });
            //this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
        });
    }

    onSearchTextChangeEvent(searchtext) {
        console.log("Search");
        if ((this.searchFields != searchtext) && !(searchtext == null && this.searchFields == "")) {
            this.searchFields = searchtext;
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(() => this.SearchMethod(), 400);

        }
        //this.searchFields = searchtext;

        //this.SearchFieldchangeevent.emit(this.searchFields);
    }

    SearchMethod() {
        //this.ApplyPreDefinedFilters();
        this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
        //if (this.searchFields && this.searchFields != "") {
        //    this.searchFields = this.searchFields.replace(/"/g, '');
        //    //this.searchFields = this.searchFields.replace(/\//g, '');//("\\", "\\");
        //    this.searchFields = this.searchFields.replace(/\\/g, "\\\\");
        //    //this.searchFields = this.searchFields.replace('"', '');
        //    //this.searchFields = this.searchFields.trim();
        //}
        //if (this.ClearMySearch == false) {
        this.CurrentQueryFilters.addAdditionalFilter("SearchFields", this.searchFields, null, null, "Contains", false, true, false, "String");
        this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters, SearchFieldChanged: true, Reload: true });
        //}
        //else {
        //    this.ClearMySearch = false;
        //}
    }

    ApplyPreDefinedFilters() {
        if (this.SelectedQuery != null) {
            this.MethodName = this.SelectedQuery.QuerySection;
            if (this.MethodName.indexOf("Customs.") > -1) {
                this.MethodName = this.MethodName.split('.')[1];
            }
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

            //this.GetQueryColumns(this.SelectedQuery.Id, this.UserId);
        }
        this.CurrentQueryFilters = new ApiQueryFilters();
        if (window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id) != null) {
            var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id);
            predefinedFilters.forEach((filter, key) => {
                var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                var value1 = filter.PredefinedValue;
                var value2 = filter.PredefinedValue2;
                if (value2 != null) {
                    filterOperator = "Between";
                }
                if (filter.DataTypeCode == "DateTime") {
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
                    var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
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
                }
                var field = window.ObjectFields.filter(a => a.Id == filter.ObjectFieldId)[0];
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

    //ApplyPreDefinedFilters() {
    //    if (this.SelectedQuery != null) {
    //        this.MethodName = this.SelectedQuery.QuerySection;
    //        this.SelectedQueryId = this.SelectedQuery.Id;
    //        if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
    //            this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
    //        }
    //        else {
    //            this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
    //        }
    //        if (this.listArgs && this.listArgs.Filters && !AppTool.IsNullOrEmpty(this.listArgs.Filters.SortDirection)) {
    //            this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
    //        }
    //        else {
    //            this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
    //        }

    //        this.GetQueryColumns(this.SelectedQuery.Id, this.UserId);
    //    }
    //    this.CurrentQueryFilters = new ApiQueryFilters();
    //    if (window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id) != null) {
    //        var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id);
    //        predefinedFilters.forEach((filter, key) => {
    //            var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
    //            var value1 = filter.PredefinedValue;
    //            var value2 = filter.PredefinedValue2;
    //            if (value2 != null) {
    //                filterOperator = "Between";
    //            }
    //            if (filter.DataTypeCode == "DateTime") {
    //                var TodayDate = new Date();
    //                if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
    //                if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);

    //                var TommorowDate = new Date();
    //                DateTool.AddDays(TommorowDate, 1);
    //                TommorowDate.setHours(0, 0, 0, 0);
    //                var TodayDate = new Date();
    //                TodayDate.setHours(0, 0, 0, 0);
    //                var YesterdayDate = DateTool.AddDays((new Date()), -1);
    //                YesterdayDate.setHours(0, 0, 0, 0);
    //                var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
    //                LastSevenDaysDate.setHours(0, 0, 0, 0);
    //                var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
    //                LastThirtyDaysDate.setHours(0, 0, 0, 0);
    //                var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
    //                CurrentYearFromDate.setHours(0, 0, 0, 0);
    //                var CurrentYearToDate = new Date();
    //                DateTool.AddDays(CurrentYearToDate, 1);
    //                CurrentYearToDate.setHours(0, 0, 0, 0);
    //                var LastYearFromDate = DateTool.AddDays((new Date()), -365);
    //                LastYearFromDate.setHours(0, 0, 0, 0);
    //                var LastYearToDate = new Date();
    //                DateTool.AddDays(LastYearToDate, 1);
    //                LastYearToDate.setHours(0, 0, 0, 0);

    //                if (value1 == "Today") {
    //                    value1 = TodayDate;
    //                    value2 = TommorowDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "Yesterday") {
    //                    value1 = YesterdayDate;
    //                    value2 = TodayDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "Last 7 Days") {
    //                    value1 = LastSevenDaysDate;
    //                    value2 = TommorowDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "Last 30 Days") {
    //                    value1 = LastThirtyDaysDate;
    //                    value2 = TommorowDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "Current Year") {
    //                    value1 = CurrentYearFromDate;
    //                    value2 = CurrentYearToDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "Last Year") {
    //                    value1 = LastYearFromDate;
    //                    value2 = LastYearToDate;
    //                    filterOperator = "Between";
    //                }
    //                else if (value1 == "NoDate" || value1 == "No Date") {
    //                    value1 = "NoDate";
    //                    filterOperator = "NoDate";
    //                }
    //            }
    //            var field = window.ObjectFields.filter(a => a.Id == filter.ObjectFieldId)[0];
    //            if (field) {
    //                this.CurrentQueryFilters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
    //            }
    //        });
    //    }

    //    if (!AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortColumn) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortBy)) {
    //        this.CurrentQueryFilters.SortBy = this.SelectedQuery.DefaultSortColumn;
    //    }
    //    if (!AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortDirection) && AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortDirection)) {
    //        this.CurrentQueryFilters.SortDirection = this.SelectedQuery.DefaultSortDirection;
    //    }
    //    if (this.listArgs.SelectedTransportMode != "All") {
    //        this.CurrentQueryFilters.addAdditionalFilter("TransportModeId", this.listArgs.SelectedTransportMode, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedTransportMode == "All" ? true : false));
    //    }
    //    if (this.listArgs.SelectedDirection != "All") {
    //        this.CurrentQueryFilters.addAdditionalFilter("DirectionId", this.listArgs.SelectedDirection, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedDirection == "All" ? true : false));
    //    }
    //}

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
            //console.log("dataSource.getRows callback function searchFields", searchFields);
            //console.log("sortingCol", sortingCol, "sortingDir", sortingDir);
            //if (sortingCol == '' || sortingDir == '') {
            //    return this.getRows(skip, take, "CreateDateTime", "Descending", getCount, searchFields);
            //}
            //else {
            this.currentSortingCol = sortingCol;
            this.currentSortingDir = sortingDir;
            this.currentSearchFields = searchFields;
            this.currentFilters = filters;
            return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            //}
        },
    };

    public ObjectTableName: string;
    public ObjectTable: ObjectTablePM;
    //public Query: any;
    public Queries: any[];
    public UserQueries: any[];
    public QueryColumns: any[];
    public firstCall: boolean = true;
    public SelectedQueryId: string;
    public SelectedQuery: any = null;
    public QueryCode: string;
    public NewButtonLable: string;

    Filterchangeevent: LogEvents.EventManager;
    pubSubAdvanceQueryFiltersServiceRecived: PubSubService1;
    @Output() GridFilterchangeevent = new EventEmitter();
    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    AdvanceQFiltersService: PubSubService;
    public TenantPM: TenantPM;
    MethodName: string = null;
    ListComponentId: string;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private SessionEvent: any = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _http: Http, private _entityListService: EntityListService, private _entityResourceService: EntityResourceService, public pubSubAdvanceQueryFiltersService: PubSubService, private temp: PubSubService1, private entityPMService: EntityPMService, private _totangoService: TotangoService, private CD: ChangeDetectorRef) {

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
    }
    name: string;
    processAdvanceQueryFilters(filters) {
        if (this.IsAdvancedSearchOpened == false) {
            return;

        }
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
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
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
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, "Between", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

        }
        else if (filters.TextValue1) {
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
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
                    this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
                }
            }
        }
        this.pubSubAdvanceQueryFiltersServiceRecived.Stream.emit(filters);
    }
    HasPermition: boolean = true;

    NewButtonId: string;

    ngOnInit() {
        if (SessionLocator.Tenant == 65 && !SessionLocator.LoggedUserPM.IsCustomerCare && this.ObjectTableName == "Contact") {
            this.IsDemoTenant = true;
        }
        this.NewButtonId = "NewButton_" + this.ObjectTableName;

        if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "NEW", false)) {
            this.IsNewEntityButtonDisabled = true;
        }
        if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", false)) {
            this.HasPermition = false;
        }
        this.Filterchangeevent = new LogEvents.EventManager();
        var subscription = this.pubSubAdvanceQueryFiltersService.Stream.subscribe(customer => this.processAdvanceQueryFilters(customer));
        //this.CurrentSession.pubSubAdvanceQueryFiltersService.emit(this.pubSubAdvanceQueryFiltersService)
        //this.ObjectTableName == "Customs.Declaration" || this.ObjectTableName == "Customs.PhysicalCheck" ||
        if (this.ObjectTableName.startsWith("Customs.")) {
            this.IsNavigateButtonVisible = true;
        }
        this.Listen();
    }

    private ReloadAllListEvent: any = null;
    Listen() {
        if (!this.ReloadAllListEvent) {
            this.ReloadAllListEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ReloadAllList") {
                    this.RefreshBtnClick();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ReloadAllListEvent);
    }


    ngAfterViewInit() {
        //if (this.ObjectTable.HasFiltersMenu) {
        //    let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "MNH")[0];
        //    if (myLocation != null) {

        //        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/FiltersMenu/" + this.ObjectTable.Name + "FiltersMenuComponent";

        //        SessionLocator.DynamicLoader.Load(myComponentPath, myLocation.viewContainerRef)
        //            .then(cmpRef => {
        //                cmpRef.instance.SelectedValueChanged.subscribe(($event: any) => this.MenuHeaderchangeevent.emit({ Filters: $event.Filters, RemoveFilter: $event.RemoveFilter }));
        //            });
        //    }
        //}


    }



    IsShowAddFromLibraryLink: boolean;
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
   View:string;
    RunComponent() {


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
          this.View ="View"
        }
else{ this.View = TextCodeTranslator.Translate("General.O.View");}
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


        if (this.ObjectTable.Name == "DocumentType") {
            if (FeatureLocator.HasFeaturePermession("DocumentType", "FROMLIBRARY")) {
                this.IsShowAddFromLibraryLink = true;
            }
            else {
                this.IsShowAddFromLibraryLink = false;
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







        if (this.ObjectTable.HasFiltersMenu) {
            if (this.AllLocations) {

                if (this.AllLocations.length == 0) {
                    this.RunComponentTimer();
                }

                else {
                    this.isLoaderReady = true;

                    let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "MNH")[0];
                    if (myLocation != null) {

                        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/FiltersMenu/" + this.ObjectTable.Name + "FiltersMenuComponent";

                        SessionLocator.DynamicLoader.Load(myComponentPath, myLocation.viewContainerRef)
                            .then(cmpRef => {

                                this.FiltersBarLoaded.emit(cmpRef.instance);

                                cmpRef.instance.SelectedValueChanged.subscribe(($event: any) => {
                                    this.FiltersMenu = new ApiQueryFilters();
                                    this.FiltersMenu = $event.Filters;
                                    this.MenuHeaderchangeevent.emit({ Filters: $event.Filters, RemoveFilter: $event.RemoveFilter })
                                });

                            });
                    }
                }
            }

            else {
                this.RunComponentTimer();
            }
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private listArgs: ListComponentArgs;
    ShowViews: boolean = true;
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
            this.SetAddButtonTitle();
            this.MethodName = args.MethodName;
            this.BackBtnTitle = args.BackButtonTitle;
            this.ShowViews = args.ShowViews;
            this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
            this.SeachBoxIsDisabled = this.ObjectTable.DisableSearchBox;

            this.SearchTextValue = new FormControl();
            this.NewButtonLable = args.NewButtonLabel;
            this.SearchTextValue.valueChanges
                .debounceTime(500)
                .distinctUntilChanged()
                .subscribe((search: string): any => {
                    this.searchFields = (search === "") ? this.searchFields = "" : this.searchFields = search;
                    this.SearchFieldchangeevent.emit(this.searchFields);
                });

            this.GetQueries();
            this.RunComponent();
        }
    }

    ViewInitCompleted(event) {
        //this.afterViewGridInitCompleted.emit(event);
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this.BackBtnTitle = this.listArgs.BackButtonTitle;
            //this.Title = this.listArgs.DisplayTitle;
            this.ViewQuery(this.listArgs.Filters, this.listArgs.DisplayTitle, this.listArgs.BackButtonTitle, this.listArgs.IsReadOnlyList, this.listArgs.IsBackToCurrentListView);
            //this.CD.detectChanges();
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


    //HideLogGrid: boolean = false;
    TipVisibilityChanged(event) {

        if (event == "true") this.IsShowTipArea = true;
        else this.IsShowTipArea = false;

        this.IsFirstTipLoad = false;
        //this.HideLogGrid = true;
        //this.HideLogGrid = false;
        this.RefreshBtnClick();

    }

    //ShowTipAreaClick() {

    //    this.IsShowTipArea = true;
    //  //  this.ShowTipEvent.emit("true");
    //}




    public UserId: string = SessionInfo.LoggedUserId;
    public Tenant: number = SessionInfo.LoggedUserTenant;
    GetQueries() {
        var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });

        this.Queries = allQueries.filter(x => x.UserId == null && FeatureLocator.IsFeatureGranted(x.FeatureId) && x.SystemLevel == true);
        this.UserQueries = allQueries.filter(x => x.UserId != null && x.Tenant == SessionInfo.LoggedUserTenant);

        if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == false) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective)[0];
            this.SelectedQuery = allQueries.filter(f => f.Perspective == this.listArgs.Perspective)[0];

            this.Queries = allQueries.filter(f => (f.UserId == null && FeatureLocator.IsFeatureGranted(f.FeatureId) && f.SystemLevel == true) && f.Perspective == this.listArgs.Perspective);
        }
        else if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == true) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];
            this.SelectedQuery = allQueries.filter(f => f.Code == this.QueryCode)[0];

            this.Queries = allQueries.filter(f => (f.UserId == null && FeatureLocator.IsFeatureGranted(f.FeatureId) && f.SystemLevel == true) && f.Perspective == this.listArgs.Perspective);
        }

        else if (this.QueryCode) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];
            this.SelectedQuery = allQueries.filter(f => f.Code == this.QueryCode)[0];
        }

        else {
            this.SelectedQuery = allQueries[0];
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.Code;
        }
        if (AppTool.IsNullOrEmpty(this.Title)) {
            this.Title = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
        }
        if (this.SelectedQuery != null) {
            //console.log(this.SelectedQuery);
            this.SelectedQueryId = this.SelectedQuery.Id;
            //this.Query = this.SelectedQuery;

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
            //console.log("dataSource", this.dataSource);
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
                this.GetQueryColumns(this.SelectedQuery.Id, this.UserId);
            });
        }

        this.SetNewEntityButton();
        this.SetAddButton();
    }

    GetQueryColumns(queryId, userId) {
        //var queryId = window.Queries.filter(x => x.Code === queryCode)[0].Id;
        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryid=" + queryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + userId)
            .subscribe((response) => {
                this.QueryColumns = response.json();
                this.QueryColumns = this.QueryColumns.sort((a, b) => { return (a.IndexOrder > b.IndexOrder) ? 1 : (a.IndexOrder < b.IndexOrder) ? -1 : 0 });

                this.QueryColumns.forEach((value, key) => {
                    this.columnsObjectFields.push(window.ObjectFields.filter(x => x.Id === value.ObjectFieldId)[0]);
                });

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
                            QueryId: queryId
                            //ColumnHeaderTemplateName: this.columnsObjectFields[i].ColumnHeaderTemplateName, //'./Shipment/Components/ListTemplates/TransportModeCellDisplayListTemplate',
                        });
                    }
                }
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
            this.SelectedQuery = MyQueries.filter(x => x.Id === Args.QueryId)[0];
        }
        else {
            this.SelectedQuery = this.Queries.filter(x => x.Id === Args.QueryId)[0];
        }

        if (this.SelectedQuery == null) {
            this.SelectedQuery = this.UserQueries.filter(x => x.Id === Args.QueryId)[0];
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.Code;
            this.MethodName = this.SelectedQuery.QuerySection;
            if (this.MethodName.indexOf("Customs.") > -1) {
                this.MethodName = this.MethodName.split('.')[1];
            }
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
            if (window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == this.SelectedQuery.Id);
                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;

                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (filter.DataTypeCode == "DateTime") {
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
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
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
                    }
                    var field = window.ObjectFields.filter(a => a.Id == filter.ObjectFieldId)[0];
                    if (field) {
                        if (Args.Filters.AdditionalFilters.filter(a => a.FieldName == filter.ObjectFieldName).length > 0) {
                            Args.Filters.AdditionalFilters = Args.Filters.AdditionalFilters.filter(a => a.FieldName != filter.ObjectFieldName);
                        }
                        Args.Filters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    }
                });
            }

            this.GetQueryColumns(this.SelectedQuery.Id, this.UserId);
        }
        //if (!AppTool.IsNullOrEmpty(this.SelectedQuery.SpotlightDataTemplate)) {
        //    this.EnableSpotLight = true;
        //    //this.CD.detectChanges();
        //}

        //console.log("QueryValueChanged()", queryId);
        //var userId = JSON.parse(sessionStorage.getItem("userData")).Id;
        //this.GetQueryColumns(queryId, this.UserId);
        this.SelectedQueryId = Args.QueryId;

        this.dataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "",//"CreateDateTime",
            sortingDir: "",//"Descending",
            getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                return this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            },
        };
        this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: Args.Filters, Reload: true });
        this.SetNewEntityButton();
        this.SetAddButton();
    }
    EnableSpotLight: boolean = false;
    QueriesChangedEvent(Args) {
        //alert("Hi");
        this.AdvanceFilters = new ApiQueryFilters();
        this.QueryCode = Args.Code;
        //this.GetQueries();
        //var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });

        //this.Queries = allQueries.filter(x => x.UserId == null && FeatureLocator.IsFeatureGranted(x.FeatureId));
        //this.Queries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id && x.UserId == null);
        this.UserQueries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id && x.UserId != null && x.SystemLevel == false && x.Tenant == SessionInfo.LoggedUserTenant);
        this.QueryListSourceChanged.emit(this.UserQueries);
        var SelectedQuery: any = {};
        //if (this.listArgs.Perspective != null) {
        //    this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective)[0];
        //    this.Queries = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective);
        //}
        if (this.QueryCode) {
            SelectedQuery = this.Queries.filter(x => x.Code === this.QueryCode)[0] != null ? this.Queries.filter(x => x.Code === this.QueryCode)[0] : this.UserQueries.filter(x => x.Code === this.QueryCode)[0];
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
        var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Code == this.QueryCode)[0];

        //if (!AppTool.IsNullOrEmpty(query.SpotlightDataTemplate)) {
        //    this.EnableSpotLight = true;
        //    this.CD.detectChanges();
        //}
        if (query != null) {
            this.temp1 = query;
            //if (!AppTool.IsNullOrEmpty(queryDisplayName)) {
            //    this.Title = queryDisplayName;
            //}
            //else {
            //    this.Title = TextCodeTranslator.Translate(query.NameTextCodeCode);
            //    //this.CD.detectChanges();
            //}
            if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);
                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (filter.DataTypeCode == "DateTime") {

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
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
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
                    }
                    var field = window.ObjectFields.filter(a => a.Id == filter.ObjectFieldId)[0];
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
            var ListComponentPostFex = this.ListComponentId.replace('ListComponentId_','');
            this.CurrentSession.PubSubFiltersChangeEventService.Stream.emit({ QueryId: query.Id, Filters: filterAgrs, ListComponentPostFex: ListComponentPostFex });
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
            //if (filter.FieldName == "ShipmentLevelCode") {
            //    this.listArgs.select = filter.FieldValue;
            //}
        });
    }
    HasFilters: boolean = false;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CurrentQueryFilters = new ApiQueryFilters();
        if (filters.AdditionalFilters.length > 0) {
            //this.HasFilters = true;
            //this.CD.detectChanges();
        }
        var MyFilters = new ApiQueryFilters();
        //if (filters == null) {
        //    filters = new ApiQueryFilters();
        //}
        filters.AdditionalFilters.forEach((filter, key) => {
            if (filter.FieldName == "CompetitorFields")
                filter.Operator = "Contains";
            MyFilters.AdditionalFilters.push(filter);
        });
        //console.log(searchfields);
        if (searchfields) {
            //filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", null, null, null, "Text");
            MyFilters.Filter1Name = "SearchFields";
            MyFilters.Filter1Operator = "Contains";
            MyFilters.Filter1Value = searchfields;
            //console.log(filters.AdditionalFilters);
        }
        //if (this.firstCall == true) {
        //filters.GetCount = true;
        MyFilters.GetCount = getCount;
        //this.firstCall = false;
        //}
        MyFilters.PageIndex = skip;
        MyFilters.PageSize = take;
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
        if (this.listArgs.SuppressOnRowSelected == true) {
            console.log("SuppressOnRowSelected");
            return;
        }

        var myObjectTableName = this.ObjectTableName;

        if (this.ObjectTableName ==  "OccasionContact"){
            myObjectTableName = "Contact";
        }

        if ($event != null) {
            if (!this.isEditControlOpened) {

                var entityList = $event.rowData;
                var selectedEntityId = $event.rowData.Id;

                switch (myObjectTableName) {
                    case 'Customs.GovernmentProcedureType':
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsHouseType":
                    case "Customs.CustomDocumentType":
                    case "Customs.UIMessage":
                    case "Customs.CourierPendingReason":
                        {
                            selectedEntityId = $event.rowData.Code;
                            break;
                        }

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
                                this.isEditControlOpened = false;
                                this.OnBackFromEdit(selectedEntityId, $event)
                            });
                        }

                        else if (myObjectTableName == "Customs.CourierMaster") {
                            var windowArgs: any = {};
                            this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(response => {
                                this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(response => {
                                    this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {

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
                                                    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                                                    logWindow.WindowClosed.subscribe(($event1: any) => {
                                                        AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
                                                        this.isEditControlOpened = false;
                                                        this.OnBackFromEdit(selectedEntityId, $event);
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
                                case 'Customer': {
                                    var windowArgs: any = {};

                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Invite Customers";
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
                                    this._entityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(response => {

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
                                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(response => {
                                            this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(response => {
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
                                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(response => {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(response => {
                                            this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                                                this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(response => {
                                                    this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {


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
                                    this._entityResourceService.getEntityResourceByTableName("Customs.ProceduralFault").subscribe(response => {

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
                                    this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(response => {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
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
                                    ObjectTableName: 'BIReport',
                                    EntityList: $event.rowData,
                                    EntityId: $event.rowData.Id
                                });

                                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                                    this.isEditControlOpened = false;
                                    this.OnBackFromEdit(selectedEntityId, $event)
                                    this.RefreshBtnClick();
                                });
                            });
                    }

                    else {

                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    EntityId: selectedEntityId,///$event.rowData.Id
                                    ObjectTableName: myObjectTableName,
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
                    }
                }
            }
            //this.CurrentSession.StopBusyIndicator();
        }
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
        this._entityListService.getSingle(selectedEntityId, this.ObjectTableName, this.MethodName == undefined ? null : this.MethodName).then((res: any) => {
            //var re = res;
            this.DestroyMe = false;
            //this.IsAdvancedSearchOpened = false;
            res.subscribe((aa: any) => {
                $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
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

    //OnBackFromEdit() {
    //    this.RefreshBtnClick();
    //    //if (this.ReattachToDetection) {
    //    //    this.ReattachToDetection = false;
    //    //}
    //    //else {
    //    //    this.ReattachToDetection = true;
    //    //}
    //}

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
        }

        else if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline" || this.ObjectTableName == "Port") {
            isVisible = true;
        }

        this.IsAddButtonVisible = isVisible;
    }

    // New
    public NewEntityButtonLabel: string = null;
    public IsNewEntityButtonVisible: boolean = false;
    public IsNewEntityButtonDisabled: boolean = false;
    private SetNewEntityButton() {
        this.SetNewEntityLabel();
        this.SetNewEntityButtonDisabled();
        this.SetNewEntityButtonVisibility();
    }
    private SetNewEntityLabel() {
        if (this.listArgs.NewButtonLabel != null) {
            this.NewEntityButtonLabel = this.listArgs.NewButtonLabel;
        }

        else if (this.ObjectTableName == "Currency") {
            this.NewEntityButtonLabel = TextCodeTranslator.Translate("General.B.Add");
        }

        else {
            //this.NewEntityButtonLabel = "New " + TextCodeTranslator.TranslateTable(this.ObjectTableName);
            if (AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                var tempText = TextCodeTranslator.Translate(this.listArgs.ObjectTableName + ".NewButton");
                if (!AppTool.IsNullOrEmpty(tempText) ) {
                    this.listArgs.NewButtonLabel = tempText;
                }
            }
            if (AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
                if (useLocal == true ) {
                    var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
                  
                        var ChangedText = GeneralText.split('%')[0];
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

                if (this.TenantPM.Id == 65) {
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

                    case "Customs.Declaration":
                        {
                            this.customsSettingListService.getSingleFromCache(this.TenantPM.Id.toString()).subscribe((response: ServiceResponse) => {
                                var list = response.Result;

                                if (list != null) {
                                    if (list.IsConnectedToUniFreight) {
                                        isVisible = false;
                                    }
                                }
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
                }
            }
        }

        this.IsNewEntityButtonVisible = isVisible;
    }
    AddNewEntity() {
        if (this.SelectedQuery != null) {

            if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "NEW", true)) {
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
                this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
                    if (isNewWizard) {
                        var IsOriginalMaster: boolean = false;
                        if (!AppTool.IsNullOrEmpty(this.SelectedQuery.OriginalQueryId)) {
                            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Id == this.SelectedQuery.OriginalQueryId)[0];
                            if (query.Code == "Masters" || query.Code == "Open Payables Masters" || query.Code == "All Masters") {
                                IsOriginalMaster = true;
                            }
                        }
                        if (this.QueryCode == "Masters" || this.QueryCode == "Open Payables Masters" || this.QueryCode == "All Masters" || IsOriginalMaster) {
                            this.RunNewMasterWizard();
                        }
                        else {
                            this.RunNewEntityWizard(this.SelectedQuery.ObjectTableNewWizardControlName);
                        }
                    }
                    else {

                        if (this.ObjectTableName == "APPayment") {
                            this.NewAPPaymentMethod();
                            // APPaymentTools.Create(eventAggregator, viewInjectionService, regionManager, container);
                        } else if (this.ObjectTableName == "Journal") {
                            this.RunNewJournalWizard();
                        } else if (this.ObjectTableName == "AccountingIntegrityCheck") {
                            this.RunNewAccountingIntegrityCheckWizard();
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
                        logWindow.Width = 770;
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
                case "BankAccount": {
                    logWindow.Width = 500;
                    logWindow.Height = 400;
                    break;
                }
                case "Customs.CustomsAirline":
                case "Customs.CouriersVat":
                case "Customs.CourierPendingReason":
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
                        logWindow.Height = 240;
                        break;
                    }
                case "OpenFormatReport":
                    {

                        logWindow.Width = 400;
                        logWindow.Height = 220;
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

            if (this.ObjectTableName == "Customs.CustomsVendor") {
                str = TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors");
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
                    if (!AppTool.IsNullOrEmpty(this.SelectedQuery.OriginalQueryId)) {
                        var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Id == this.SelectedQuery.OriginalQueryId)[0];
                        QueryCodeOriginal = query.Code;
                    }

                    var windowArgs: any = {};
                    logWindow.Width = 850;
                    logWindow.Height = 500;

                    switch (QueryCodeOriginal) {
                        case "Air Freight Cost Tariffs": {
                            logWindow.Title = "New Air Freight Cost";
                            windowArgs.TypeCode = "AFC";
                            break;
                        }

                        case "Air Surcharges Cost Tariffs": {
                            logWindow.Title = "New Air Surcharges Cost";
                            windowArgs.TypeCode = "ASC";
                            break;
                        }

                        case "Ocean LCL Freight Cost": {
                            logWindow.Title = "New Ocean LCL Freight Cost";
                            windowArgs.TypeCode = "OLC";
                            break;
                        }

                        case "Ocean.LCL.Surcharges.Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.Ocean.LCL.Surcharges.Cost");
                            windowArgs.TypeCode = "OSC";
                            break;
                        }
                            
                        case "Ocean FCL Freight Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.OceanFCLFreightCost");
                            windowArgs.TypeCode = "OFC";
                            break;
                        }

                        case "Ocean FCL Surcharges Cost": {
                            logWindow.Title = "New " + TextCodeTranslator.TranslateTable("Tariff.Q.OceanFCLSurchargesCost");
                            windowArgs.TypeCode = "OFS";
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


            var windowTitle = FinalText; //TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }
    private OnNewEntityWindowClosed($event: any) {
        this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
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
        //this.CD.detectChanges();
        logWindow.WindowClosed.subscribe(($event: any) => {

            this.CD.detectChanges();
        });
    }

    btnExcelCLicked() {
        if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", true)) {
            return;
        }
        else {
            var windowArgs: any = {};
            windowArgs.query = this.SelectedQuery;
            windowArgs.currentObjectTable = this.ObjectTableName;
            windowArgs.tenant = SessionInfo.LoggedUserTenant;
            windowArgs.userid = SessionInfo.LoggedUserId;
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
        if (!FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", true)) {
            return;
        }
        else {
            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Id == this.SelectedQueryId)[0];
            if (query != null) {

                if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
                    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);
                    predefinedFilters.forEach((filter, key) => {
                        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                        var value1 = filter.PredefinedValue;
                        var value2 = filter.PredefinedValue2;
                        if (value2 != null) {
                            filterOperator = "Between";
                        }
                        if (filter.DataTypeCode == "DateTime") {
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
                            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
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
                        }
                        var field = window.ObjectFields.filter(a => a.Id == filter.ObjectFieldId)[0];
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
            this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters, Reload: false });
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
        //this.myQueryColumnsPMService.update(QColumn).subscribe(myResult => {
        //});
        this.SaveColNewChanges(Param);
        //console.log("Oh Yea !!");
    }

    SaveColNewChanges(Param: any) {
        var QColumns = null;
        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryid=" + Param.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionLocator.LoggedUserId)
            .subscribe((response) => {
                QColumns = response.json();
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
                            this.GeneralEntitiesArgs.QueryColumnsPMs.push(querycolumn);
                        });
                        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
                        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
                        myGeneralService.setServiceArgs(this.serviceArgs);
                        myGeneralService.update(this.GeneralEntitiesArgs).subscribe(myResult => {
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
                        myGeneralService.insert(this.GeneralEntitiesArgs).subscribe(myResult => {
                        });
                        // });
                    }
                }
            });
    }

    RunNewJournalWizard() {
        var windowTitle = "New Journal";
        var entityPM: JournalPM = new JournalPM();
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



    private currentFilters: ApiQueryFilters;
    private currentSearchFields: string;
    private currentSortingCol: string;
    private currentSortingDir: string;
    Navigate() {

        this.CurrentQueryFilters = new ApiQueryFilters();

        var MyFilters = new ApiQueryFilters();

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
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
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

}
