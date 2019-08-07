"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
//import {CORE_DIRECTIVES, Control, NgFormControl} from '@angular/common';
//import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
//import {IconButton} from '../../../Controls/IconButton';
//import {LogGridComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent';
//import {AdvanceSearchComponent} from '../../../Infrastructure/Components/AdvanceSearchComponent/AdvanceSearchComponent';
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../DataContracts/ServiceArgs");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var InfraSettings_1 = require("../../Utilities/InfraSettings");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
//import {SearchTextBox} from '../../../Controls/SearchTextBox';
var LogEvents_1 = require("../../../Infrastructure/Utilities/LogEvents");
var ApiFiltersEvent_1 = require("../../../Infrastructure/Utilities/events/ApiFiltersEvent");
var ApiFiltersEvent1_1 = require("../../../Infrastructure/Utilities/events/ApiFiltersEvent1");
//import {QueryListComponent} from '../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/QueryListComponent';
var Tools_1 = require("../../Tools");
var Args_1 = require("../../Args");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityArgs_1 = require("../../DataContracts/EntityArgs");
var EntityPMService_1 = require("../../Services/EntityPMService");
var TotangoService_1 = require("../../Services/WebServices/TotangoService");
var QueryColumnsPMService_1 = require("../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService");
var GeneralEntitiesArgs_1 = require("../../../Infrastructure/DataContracts/GeneralEntitiesArgs");
var GeneralEntitiesService_1 = require("../../../Infrastructure/Services/StandardPMs/GeneralEntitiesService");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var TenantImportComponent_1 = require("../../../Common/Components/Maintenance/TenantImportComponent");
var JournalPM_1 = require("../../../Accounting/EntityPMs/JournalPM");
var APPaymentPM_1 = require("../../../Invoice/EntityPMs/APPaymentPM");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var AmitalGatewayUtil_1 = require("../../Utilities/AmitalGatewayUtil");
var AccountingIntegrityCheckPM_1 = require("../../../Accounting/EntityPMs/AccountingIntegrityCheckPM");
var ListComponent = /** @class */ (function () {
    function ListComponent(_http, _entityListService, _entityResourceService, pubSubAdvanceQueryFiltersService, temp, entityPMService, _totangoService, CD) {
        var _this = this;
        this._http = _http;
        this._entityListService = _entityListService;
        this._entityResourceService = _entityResourceService;
        this.pubSubAdvanceQueryFiltersService = pubSubAdvanceQueryFiltersService;
        this.temp = temp;
        this.entityPMService = entityPMService;
        this._totangoService = _totangoService;
        this.CD = CD;
        this.ComponentIndex = null;
        this.BackCompleted = new core_1.EventEmitter();
        this.LoadResourceCompleted = new core_1.EventEmitter();
        this.ColumnsReady = new core_1.EventEmitter();
        this.QueryListSourceChanged = new core_1.EventEmitter();
        this.FiltersBarLoaded = new core_1.EventEmitter();
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
        this.SeachBoxIsDisabled = false;
        //@Output() ShowTipEvent = new EventEmitter();
        this.IsNavigateButtonVisible = false;
        this.columnsObjectFields = [];
        this.items = [];
        this.columns = [];
        this.SearchText = "Search Partners / Ports / Ref.#";
        this.IsAdvancedSearchOpened = false;
        this.LayoutDirection = 'ltr';
        this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        this.IsShowTipArea = false;
        this.IsShowTipIcon = false;
        this.IsFirstTipLoad = false;
        this.AddButtonTitle = "";
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this.onSelectedQueryChangeEvent = new core_1.EventEmitter();
        this.dataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "",
            sortingDir: "",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                //console.log("dataSource.getRows callback function searchFields", searchFields);
                //console.log("sortingCol", sortingCol, "sortingDir", sortingDir);
                //if (sortingCol == '' || sortingDir == '') {
                //    return this.getRows(skip, take, "CreateDateTime", "Descending", getCount, searchFields);
                //}
                //else {
                _this.currentSortingCol = sortingCol;
                _this.currentSortingDir = sortingDir;
                _this.currentSearchFields = searchFields;
                _this.currentFilters = filters;
                return _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                //}
            },
        };
        this.firstCall = true;
        this.SelectedQuery = null;
        this.GridFilterchangeevent = new core_1.EventEmitter();
        this.SearchFieldchangeevent = new core_1.EventEmitter();
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        this.MethodName = null;
        this.SessionEvent = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.HasPermition = true;
        this.ReloadAllListEvent = null;
        this.FiltersMenu = null;
        this.Retries = 0;
        this.ShowViews = true;
        //ShowTipAreaClick() {
        //    this.IsShowTipArea = true;
        //  //  this.ShowTipEvent.emit("true");
        //}
        this.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
        this.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.ClearMySearch = false;
        this.EnableSpotLight = false;
        this.HasFilters = false;
        this.isEditControlOpened = false;
        this._DestroyMe = false;
        this.MyScrollTop = 0;
        this.showBackButton = true;
        this.showBackButtonAndTitle = true;
        //Add Button
        this.IsAddButtonVisible = false;
        // New
        this.NewEntityButtonLabel = null;
        this.IsNewEntityButtonVisible = false;
        this.IsNewEntityButtonDisabled = false;
        this.ShowIt = true;
        this.ComponentIndex = this.CurrentSession.GetNewListComponentIndex();
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this.serviceArgs.http = _http;
        this.pubSubAdvanceQueryFiltersServiceRecived = temp;
        this.AdvanceQFiltersService = pubSubAdvanceQueryFiltersService;
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.CurrentSession.SubscriptionAdd(this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res == "TenantImport") {
                _this.RefreshBtnClick();
            }
        }));
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "NewAirlineShippingLineClosed") {
                _this.RefreshBtnClick();
            }
        });
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    Object.defineProperty(ListComponent.prototype, "Title", {
        get: function () { return this.title; },
        set: function (newValue) {
            if (this.title != newValue) {
                this.title = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ListComponent.prototype.onOpenFilterAreaClick = function () {
        this.IsAdvancedSearchOpened = true;
    };
    ListComponent.prototype.onCloseFilterAreaClick = function () {
        this.IsAdvancedSearchOpened = false;
    };
    ListComponent.prototype.onColumnsClick = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.queryId = this.SelectedQueryId;
        windowArgs.isNewQueryMode = false;
        windowArgs.currentObjectTable = this.ObjectTableName;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 520;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.QueryColumnsEdit"); //"Query Columns Edit";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/QueryColumnsComponents/QueryColumnsEditComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            //var myfilterAgrs = this.CurrentQueryFilters;
            // if (this.AdvanceFilters) {
            //     this.AdvanceFilters.AdditionalFilters.forEach((filter, key) => {
            //         myfilterAgrs.AdditionalFilters.push(filter);
            //     });
            // }
            _this.QueryValueChanged({ QueryId: _this.SelectedQueryId, Title: TextCodeTranslator_1.TextCodeTranslator.Translate(_this.SelectedQuery.NameTextCodeCode), Filters: _this.CurrentQueryFilters, IgnoreSearchFields: true });
            //this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
        });
    };
    ListComponent.prototype.onSearchTextChangeEvent = function (searchtext) {
        var _this = this;
        console.log("Search");
        if ((this.searchFields != searchtext) && !(searchtext == null && this.searchFields == "")) {
            this.searchFields = searchtext;
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(function () { return _this.SearchMethod(); }, 400);
        }
        //this.searchFields = searchtext;
        //this.SearchFieldchangeevent.emit(this.searchFields);
    };
    ListComponent.prototype.SearchMethod = function () {
        this.ApplyPreDefinedFilters();
        this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != "SearchFields"; });
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
    };
    ListComponent.prototype.ApplyPreDefinedFilters = function () {
        var _this = this;
        if (this.SelectedQuery != null) {
            this.MethodName = this.SelectedQuery.QuerySection;
            if (this.MethodName.indexOf("Customs.") > -1) {
                this.MethodName = this.MethodName.split('.')[1];
            }
            this.SelectedQueryId = this.SelectedQuery.Id;
            if (this.listArgs && this.listArgs.Filters && !Tools_1.AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
                this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
            }
            else {
                this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
            }
            if (this.listArgs && this.listArgs.Filters && !Tools_1.AppTool.IsNullOrEmpty(this.listArgs.Filters.SortDirection)) {
                this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
            }
            else {
                this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
            }
            //this.GetQueryColumns(this.SelectedQuery.Id, this.UserId);
        }
        this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == _this.SelectedQuery.Id; }) != null) {
            var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == _this.SelectedQuery.Id; });
            predefinedFilters.forEach(function (filter, key) {
                var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                var value1 = filter.PredefinedValue;
                var value2 = filter.PredefinedValue2;
                if (value2 != null) {
                    filterOperator = "Between";
                }
                if (filter.DataTypeCode == "DateTime") {
                    var TodayDate = new Date();
                    TodayDate.setHours(0, 0, 0, 0);
                    if (value1 == '#today')
                        value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                    if (value2 == '#today')
                        value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                    var TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
                    TommorowDate.setUTCHours(0, 0, 0, 0);
                    var TodayDate = new Date();
                    TodayDate.setUTCHours(0, 0, 0, 0);
                    var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
                    YesterdayDate.setUTCHours(0, 0, 0, 0);
                    var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
                    LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                    var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
                    LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                    var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                    CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                    var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
                    CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                    var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
                    LastYearFromDate.setUTCHours(0, 0, 0, 0);
                    var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
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
                var field = window.ObjectFields.filter(function (a) { return a.Id == filter.ObjectFieldId; })[0];
                if (field) {
                    _this.CurrentQueryFilters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                }
            });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortColumn) && Tools_1.AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortBy)) {
            this.CurrentQueryFilters.SortBy = this.SelectedQuery.DefaultSortColumn;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.DefaultSortDirection) && Tools_1.AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortDirection)) {
            this.CurrentQueryFilters.SortDirection = this.SelectedQuery.DefaultSortDirection;
        }
        if (this.listArgs.SelectedTransportMode != "All") {
            this.CurrentQueryFilters.addAdditionalFilter("TransportModeId", this.listArgs.SelectedTransportMode, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedTransportMode == "All" ? true : false));
        }
        if (this.listArgs.SelectedDirection != "All") {
            this.CurrentQueryFilters.addAdditionalFilter("DirectionId", this.listArgs.SelectedDirection, null, null, "Equals", false, true, false, "string", (this.listArgs.SelectedDirection == "All" ? true : false));
        }
    };
    ListComponent.prototype.processAdvanceQueryFilters = function (filters) {
        var _this = this;
        if (this.IsAdvancedSearchOpened == false) {
            return;
        }
        this.dataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "",
            sortingDir: "",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                return _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            },
        };
        if (this.AdvanceFilters == null) {
            this.AdvanceFilters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        if (filters.IsDeleted || (filters.textValue == "" && filters.textValue.toString() != "false") || filters.textValue == "No Filter") {
            this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filters.FieldName; });
        }
        else if (filters.TextValue == "NoDate") {
            if (this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName == filters.FieldName; }).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filters.FieldName; });
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, null, null, null, "NoDate", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(filters.MyName)) {
            var TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
            TommorowDate.setUTCHours(0, 0, 0, 0);
            //TommorowDate.setHours(0, 0, 0, 0);
            var TodayDate = new Date();
            TodayDate.setUTCHours(0, 0, 0, 0);
            var TodayCustomDate = new Date();
            TodayCustomDate.setHours(0, 0, 0, 0);
            var TodayEndDate = new Date();
            TodayEndDate.setHours(23, 59, 59, 0);
            //TodayDate.setHours(0, 0, 0, 0);
            var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
            YesterdayDate.setUTCHours(0, 0, 0, 0);
            //YesterdayDate.setHours(0, 0, 0, 0);
            var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
            var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
            LastYearFromDate.setUTCHours(0, 0, 0, 0);
            var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
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
            if (this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName == filters.FieldName; }).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filters.FieldName; });
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, "Between", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else if (filters.TextValue1) {
            if (this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName == filters.FieldName; }).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filters.FieldName; });
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else {
            if (this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName == filters.FieldName; }).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filters.FieldName; });
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
    };
    ListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.NewButtonId = "NewButton_" + this.ObjectTableName;
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions(this.ObjectTableName, "NEW", false)) {
            this.IsNewEntityButtonDisabled = true;
        }
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", false)) {
            this.HasPermition = false;
        }
        this.Filterchangeevent = new LogEvents_1.LogEvents.EventManager();
        var subscription = this.pubSubAdvanceQueryFiltersService.Stream.subscribe(function (customer) { return _this.processAdvanceQueryFilters(customer); });
        //this.CurrentSession.pubSubAdvanceQueryFiltersService.emit(this.pubSubAdvanceQueryFiltersService)
        //this.ObjectTableName == "Customs.Declaration" || this.ObjectTableName == "Customs.PhysicalCheck" ||
        if (this.ObjectTableName.startsWith("Customs.")) {
            this.IsNavigateButtonVisible = true;
        }
        this.Listen();
    };
    ListComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.ReloadAllListEvent) {
            this.ReloadAllListEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "ReloadAllList") {
                    _this.RefreshBtnClick();
                }
            });
        }
    };
    ListComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.ReloadAllListEvent);
    };
    ListComponent.prototype.ngAfterViewInit = function () {
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
    };
    ListComponent.prototype.LinkAddDocumentFromLibraryClcik = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.ObjectTableId = "";
        windowArgs.EntityId = "";
        windowArgs.TransportModeId = "";
        windowArgs.ShipmentlevelCode = "";
        windowArgs.ChildEntityId = "";
        windowArgs.ChildObjectTableId = "";
        windowArgs.PageRequest = "Maintanice";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 550;
        logWindow.Title = "New Documents";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.isEditControlOpened = false;
            //this.OnBackFromEdit();
            _this.RefreshBtnClick();
        });
    };
    ListComponent.prototype.LinkAddQuoteTemplateFromLibraryClcik = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.Type = "Maintenance";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.NewQuoteTemplate");
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddQuoteTemplateFromLibraryComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.isEditControlOpened = false;
            //this.OnBackFromEdit();
            _this.RefreshBtnClick();
        });
    };
    ListComponent.prototype.RunComponent = function () {
        var _this = this;
        if (window.Tips) {
            var tip = window.Tips.filter(function (d) { return d.Code == _this.ObjectTable.MainTipCode; })[0];
            if (tip) {
                var hasTip = true;
                this.IsShowTipIcon = true;
                var isVisible = tip.VisibilityDefaultValue;
                var tipVisibility = window.TipsVisibilities.filter(function (d) { return d.TipCode == tip.Code && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId; })[0];
                if (tipVisibility)
                    isVisible = tipVisibility.IsVisible;
                if (!isVisible && hasTip)
                    this.IsShowTipArea = false;
                else
                    this.IsShowTipArea = true;
            }
        }
        if (this.ObjectTable.Name == "Customer") {
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "EXPORTEXCEL")) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTable.DownloadToExcelFeatureCode)) {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTable.Name, this.ObjectTable.DownloadToExcelFeatureCode)) {
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
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "FROMLIBRARY")) {
                this.IsShowAddFromLibraryLink = true;
            }
            else {
                this.IsShowAddFromLibraryLink = false;
            }
        }
        if (this.ObjectTable.Name == "QuoteTemplate") {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "FROMLIBRARY")) {
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
                    var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "MNH"; })[0];
                    if (myLocation != null) {
                        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/FiltersMenu/" + this.ObjectTable.Name + "FiltersMenuComponent";
                        SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, myLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            _this.FiltersBarLoaded.emit(cmpRef.instance);
                            cmpRef.instance.SelectedValueChanged.subscribe(function ($event) {
                                _this.FiltersMenu = new ApiQueryFilters_1.ApiQueryFilters();
                                _this.FiltersMenu = $event.Filters;
                                _this.MenuHeaderchangeevent.emit({ Filters: $event.Filters, RemoveFilter: $event.RemoveFilter });
                            });
                        });
                    }
                }
            }
            else {
                this.RunComponentTimer();
            }
        }
    };
    ListComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ListComponent.prototype.Run = function (args) {
        var _this = this;
        this.CurrentSession.AddMenuReference(this.ComponentRef);
        this.CurrentSession.AddListComponent(this);
        this.listArgs = args;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.listArgs.DisplayTitle)) {
            this.Title = this.listArgs.DisplayTitle;
        }
        this.QueryCode = args.QueryCode;
        this.ObjectTableName = args.ObjectTableName;
        this.SetAddButtonTitle();
        this.MethodName = args.MethodName;
        this.BackBtnTitle = args.BackButtonTitle;
        this.ShowViews = args.ShowViews;
        this.ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.SeachBoxIsDisabled = this.ObjectTable.DisableSearchBox;
        this.SearchTextValue = new forms_1.FormControl();
        this.NewButtonLable = args.NewButtonLabel;
        this.SearchTextValue.valueChanges
            .debounceTime(500)
            .distinctUntilChanged()
            .subscribe(function (search) {
            _this.searchFields = (search === "") ? _this.searchFields = "" : _this.searchFields = search;
            _this.SearchFieldchangeevent.emit(_this.searchFields);
        });
        //this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
        this.GetQueries();
        this.RunComponent();
        //});NewEntityButtonLabel
    };
    ListComponent.prototype.ViewInitCompleted = function (event) {
        var _this = this;
        //this.afterViewGridInitCompleted.emit(event);
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this.BackBtnTitle = _this.listArgs.BackButtonTitle;
            //this.Title = this.listArgs.DisplayTitle;
            _this.ViewQuery(_this.listArgs.Filters, _this.listArgs.DisplayTitle, _this.listArgs.BackButtonTitle, _this.listArgs.IsReadOnlyList, _this.listArgs.IsBackToCurrentListView);
            //this.CD.detectChanges();
        });
    };
    ListComponent.prototype.ShowFieldsList = function () {
        document.getElementById("FieldsDropdown").classList.toggle("showDDButton");
    };
    ListComponent.prototype.SetAddButtonTitle = function () {
        if (this.ObjectTableName == "Airline" || this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Port" || this.ObjectTableName == "Warehouse") {
            this.AddButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.ImportEntities").replace(/%Entity/g, TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural(this.ObjectTableName));
        }
    };
    //HideLogGrid: boolean = false;
    ListComponent.prototype.TipVisibilityChanged = function (event) {
        if (event == "true")
            this.IsShowTipArea = true;
        else
            this.IsShowTipArea = false;
        this.IsFirstTipLoad = false;
        //this.HideLogGrid = true;
        //this.HideLogGrid = false;
        this.RefreshBtnClick();
    };
    ListComponent.prototype.GetQueries = function () {
        var _this = this;
        var allQueries = window.Queries.filter(function (x) { return x.ObjectTableId === _this.ObjectTable.Id; }).sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
        this.Queries = allQueries.filter(function (x) { return x.UserId == null && FeatureLocator_1.FeatureLocator.IsFeatureGranted(x.FeatureId) && x.SystemLevel == true; });
        this.UserQueries = allQueries.filter(function (x) { return x.UserId != null && x.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant; });
        if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == false) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective)[0];
            this.SelectedQuery = allQueries.filter(function (f) { return f.Perspective == _this.listArgs.Perspective; })[0];
            this.Queries = allQueries.filter(function (f) { return (f.UserId == null && FeatureLocator_1.FeatureLocator.IsFeatureGranted(f.FeatureId) && f.SystemLevel == true) && f.Perspective == _this.listArgs.Perspective; });
        }
        else if (this.listArgs.Perspective != null && this.listArgs.IgnoreSelectedPerspective == true) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];
            this.SelectedQuery = allQueries.filter(function (f) { return f.Code == _this.QueryCode; })[0];
            this.Queries = allQueries.filter(function (f) { return (f.UserId == null && FeatureLocator_1.FeatureLocator.IsFeatureGranted(f.FeatureId) && f.SystemLevel == true) && f.Perspective == _this.listArgs.Perspective; });
        }
        else if (this.QueryCode) {
            //this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Code == this.QueryCode)[0];
            this.SelectedQuery = allQueries.filter(function (f) { return f.Code == _this.QueryCode; })[0];
        }
        else {
            this.SelectedQuery = allQueries[0];
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.Code;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Title)) {
            this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
        }
        if (this.SelectedQuery != null) {
            //console.log(this.SelectedQuery);
            this.SelectedQueryId = this.SelectedQuery.Id;
            //this.Query = this.SelectedQuery;
            if (this.listArgs && this.listArgs.Filters && !Tools_1.AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
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
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
                _this.GetQueryColumns(_this.SelectedQuery.Id, _this.UserId);
            });
        }
        this.SetNewEntityButton();
        this.SetAddButton();
    };
    ListComponent.prototype.GetQueryColumns = function (queryId, userId) {
        var _this = this;
        //var queryId = window.Queries.filter(x => x.Code === queryCode)[0].Id;
        this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryid=" + queryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + userId)
            .subscribe(function (response) {
            _this.QueryColumns = response.json();
            _this.QueryColumns = _this.QueryColumns.sort(function (a, b) { return (a.IndexOrder > b.IndexOrder) ? 1 : (a.IndexOrder < b.IndexOrder) ? -1 : 0; });
            _this.QueryColumns.forEach(function (value, key) {
                _this.columnsObjectFields.push(window.ObjectFields.filter(function (x) { return x.Id === value.ObjectFieldId; })[0]);
            });
            for (var i = 0; i < _this.QueryColumns.length; i++) {
                var CurColumn = _this.columns.filter(function (a) { return a.FieldName == _this.QueryColumns[i].ObjectFieldName; });
                if (_this.columns != null && (CurColumn == null || CurColumn.length == 0)) {
                    _this.columns.push({
                        FieldName: _this.columnsObjectFields[i].FieldName,
                        DataTypeCode: _this.columnsObjectFields[i].DataTypeCode,
                        Display: TextCodeTranslator_1.TextCodeTranslator.Translate(_this.columnsObjectFields[i].ListTextCodeCode),
                        Styles: { width: _this.QueryColumns[i].ColumnWidth + 'px' },
                        HtmlListComponentName: _this.columnsObjectFields[i].HtmlListComponentName,
                        HtmlListComponentUrl: _this.columnsObjectFields[i].HtmlListComponentUrl,
                        ServerSideSortable: true,
                        ColumnHeaderTemplateName: _this.columnsObjectFields[i].ColumnHeaderTemplateName,
                        ObjectField: _this.columnsObjectFields[i],
                        QueryId: queryId
                        //ColumnHeaderTemplateName: this.columnsObjectFields[i].ColumnHeaderTemplateName, //'./Shipment/Components/ListTemplates/TransportModeCellDisplayListTemplate',
                    });
                }
            }
            _this.ColumnsReady.emit("ColumnsReady");
        });
    };
    ListComponent.prototype.QueryValueChanged = function (Args) {
        var _this = this;
        this.AdvanceFilters = new ApiQueryFilters_1.ApiQueryFilters();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment != "customs") {
            if (Args.IgnoreSearchFields != true) {
                this.searchFields = "";
            }
        }
        this.ClearMySearch = true;
        this.UserQueries = window.Queries.filter(function (x) { return x.ObjectTableId === _this.ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant; });
        this.Title = Args.Title;
        this.columns = [];
        this.columnsObjectFields = [];
        if (this.Queries == null || this.Queries.length == 0) {
            var MyQueries = window.Queries.filter(function (x) { return x.ObjectTableId === _this.ObjectTable.Id; }).sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
            this.SelectedQuery = MyQueries.filter(function (x) { return x.Id === Args.QueryId; })[0];
        }
        else {
            this.SelectedQuery = this.Queries.filter(function (x) { return x.Id === Args.QueryId; })[0];
        }
        if (this.SelectedQuery == null) {
            this.SelectedQuery = this.UserQueries.filter(function (x) { return x.Id === Args.QueryId; })[0];
        }
        if (this.SelectedQuery != null) {
            this.QueryCode = this.SelectedQuery.Code;
            this.MethodName = this.SelectedQuery.QuerySection;
            if (this.MethodName.indexOf("Customs.") > -1) {
                this.MethodName = this.MethodName.split('.')[1];
            }
            this.SelectedQueryId = this.SelectedQuery.Id;
            if (this.listArgs && this.listArgs.Filters && !Tools_1.AppTool.IsNullOrEmpty(this.listArgs.Filters.SortBy)) {
                this.dataSource.sortingCol = this.listArgs.Filters.SortBy;
            }
            else {
                this.dataSource.sortingCol = this.SelectedQuery.DefaultSortColumn;
            }
            if (this.listArgs && this.listArgs.Filters && !Tools_1.AppTool.IsNullOrEmpty(this.listArgs.Filters.SortDirection)) {
                this.dataSource.sortingDir = this.listArgs.Filters.SortDirection;
            }
            else {
                this.dataSource.sortingDir = this.SelectedQuery.DefaultSortDirection;
            }
            if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == _this.SelectedQuery.Id; }) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == _this.SelectedQuery.Id; });
                predefinedFilters.forEach(function (filter, key) {
                    var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (filter.DataTypeCode == "DateTime") {
                        var TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
                        TommorowDate.setUTCHours(0, 0, 0, 0);
                        var TodayDate = new Date();
                        TodayDate.setUTCHours(0, 0, 0, 0);
                        if (value1 == '#today')
                            value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                        if (value2 == '#today')
                            value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                        var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
                        YesterdayDate.setUTCHours(0, 0, 0, 0);
                        var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
                        LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                        var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
                        LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                        CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
                        CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                        var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
                        LastYearFromDate.setUTCHours(0, 0, 0, 0);
                        var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
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
                    var field = window.ObjectFields.filter(function (a) { return a.Id == filter.ObjectFieldId; })[0];
                    if (field) {
                        if (Args.Filters.AdditionalFilters.filter(function (a) { return a.FieldName == filter.ObjectFieldName; }).length > 0) {
                            Args.Filters.AdditionalFilters = Args.Filters.AdditionalFilters.filter(function (a) { return a.FieldName != filter.ObjectFieldName; });
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
            sortingCol: "",
            sortingDir: "",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                return _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            },
        };
        this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: Args.Filters, Reload: true });
        this.SetNewEntityButton();
        this.SetAddButton();
    };
    ListComponent.prototype.QueriesChangedEvent = function (Args) {
        var _this = this;
        //alert("Hi");
        this.AdvanceFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.QueryCode = Args.Code;
        //this.GetQueries();
        //var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        //this.Queries = allQueries.filter(x => x.UserId == null && FeatureLocator.IsFeatureGranted(x.FeatureId));
        //this.Queries = window.Queries.filter(x => x.ObjectTableId === this.ObjectTable.Id && x.UserId == null);
        this.UserQueries = window.Queries.filter(function (x) { return x.ObjectTableId === _this.ObjectTable.Id && x.UserId != null && x.SystemLevel == false && x.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant; });
        this.QueryListSourceChanged.emit(this.UserQueries);
        var SelectedQuery = {};
        //if (this.listArgs.Perspective != null) {
        //    this.SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective)[0];
        //    this.Queries = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == this.listArgs.Perspective);
        //}
        if (this.QueryCode) {
            SelectedQuery = this.Queries.filter(function (x) { return x.Code === _this.QueryCode; })[0] != null ? this.Queries.filter(function (x) { return x.Code === _this.QueryCode; })[0] : this.UserQueries.filter(function (x) { return x.Code === _this.QueryCode; })[0];
        }
        else {
            SelectedQuery = this.Queries.filter(function (x) { return x.IndexOrder === 0; })[0] != null ? this.Queries.filter(function (x) { return x.IndexOrder === 0; })[0] != null : this.UserQueries.filter(function (x) { return x.IndexOrder === 0; })[0] != null;
        }
        if (SelectedQuery) {
            if (SelectedQuery.QueryGroupCode == "SFLU" || SelectedQuery.QueryGroupCode == "QFLU") {
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", "FollowUpQueryUse");
            }
        }
        this.onSelectedQueryChangeEvent.emit(SelectedQuery);
    };
    ListComponent.prototype.NewViewClosedEvent = function (args) {
        this.IsAdvancedSearchOpened = false;
    };
    ListComponent.prototype.ViewQuery = function (filterAgrs, queryDisplayName, backButtonLabel, readOnlyList, backToCurrentListView) {
        var _this = this;
        if (filterAgrs == null) {
            filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(queryDisplayName)) {
            this.Title = queryDisplayName;
        }
        var query = window.Queries.filter(function (q) { return q.ObjectTableId == _this.ObjectTable.Id && q.Code == _this.QueryCode; })[0];
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
            if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; }) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; });
                predefinedFilters.forEach(function (filter, key) {
                    var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    if (filter.DataTypeCode == "DateTime") {
                        var TodayDate = new Date();
                        TodayDate.setUTCHours(0, 0, 0, 0);
                        if (value1 == '#today')
                            value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                        if (value2 == '#today')
                            value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                        var TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
                        TommorowDate.setUTCHours(0, 0, 0, 0);
                        var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
                        YesterdayDate.setUTCHours(0, 0, 0, 0);
                        var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
                        LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                        var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
                        LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                        CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                        var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
                        CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                        var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
                        LastYearFromDate.setUTCHours(0, 0, 0, 0);
                        var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
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
                    var field = window.ObjectFields.filter(function (a) { return a.Id == filter.ObjectFieldId; })[0];
                    if (field) {
                        filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    }
                });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(query.DefaultSortColumn) && Tools_1.AppTool.IsNullOrEmpty(filterAgrs.SortBy)) {
                filterAgrs.SortBy = query.DefaultSortColumn;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(query.DefaultSortDirection) && Tools_1.AppTool.IsNullOrEmpty(filterAgrs.SortDirection)) {
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
                this.AdvanceFilters.AdditionalFilters.forEach(function (filter, key) {
                    filterAgrs.AdditionalFilters.push(filter);
                });
            }
            this.CurrentSession.PubSubFiltersChangeEventService.Stream.emit({ QueryId: query.Id, Filters: filterAgrs });
        }
    };
    ListComponent.prototype.onMenuHeaderchanged = function (event) {
        var _this = this;
        event.AdditionalFilters.forEach(function (filter, key) {
            if (filter.FieldName == "TransportModeId") {
                _this.listArgs.SelectedTransportMode = filter.FieldValue;
            }
            if (filter.FieldName == "DirectionId") {
                _this.listArgs.SelectedDirection = filter.FieldValue;
            }
            //if (filter.FieldName == "ShipmentLevelCode") {
            //    this.listArgs.select = filter.FieldValue;
            //}
        });
    };
    ListComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        if (filters.AdditionalFilters.length > 0) {
            //this.HasFilters = true;
            //this.CD.detectChanges();
        }
        var MyFilters = new ApiQueryFilters_1.ApiQueryFilters();
        //if (filters == null) {
        //    filters = new ApiQueryFilters();
        //}
        filters.AdditionalFilters.forEach(function (filter, key) {
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
    };
    Object.defineProperty(ListComponent.prototype, "DestroyMe", {
        get: function () {
            return this._DestroyMe;
        },
        set: function (val) {
            this._DestroyMe = val;
        },
        enumerable: true,
        configurable: true
    });
    ListComponent.prototype.onRowSelected = function ($event) {
        var _this = this;
        if (this.listArgs.SuppressOnRowSelected == true) {
            console.log("SuppressOnRowSelected");
            return;
        }
        //this.CurrentSession.StartBusyIndicator("Loading ...");
        //var BackGridEvent = $event.BackFromEdit;
        if ($event != null) {
            if (!this.isEditControlOpened) {
                var entityList = $event.rowData;
                var selectedEntityId = $event.rowData.Id;
                switch (this.ObjectTableName) {
                    case 'Customs.GovernmentProcedureType':
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsHouseType":
                    case "Customs.CustomDocumentType":
                    case "Customs.UIMessage":
                    case "Customs.CourierPendingReason":
                        //case "Customs.InternationalSite":
                        selectedEntityId = $event.rowData.Code;
                        break;
                    default:
                        break;
                }
                //if (this.ObjectTableName != "Customs.ProceduralFault" && this.ObjectTableName != "TicketEscalation") {
                if (this.ObjectTableName != "TicketEscalation") {
                    this.isEditControlOpened = true;
                    var myCodes = [];
                    myCodes.push("EAWB");
                    myCodes.push("BUBK");
                    if (FeatureLocator_1.FeatureLocator.IsPackageOneOf(myCodes) && (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master")) {
                        var isFullWizard = false;
                        var windowTitle = null;
                        if (entityList.DirectionId == "E" || entityList.DirectionId == "R") {
                            isFullWizard = true;
                        }
                        else if (entityList.DirectionId == "D") {
                            if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                                isFullWizard = true;
                            }
                        }
                        if (isFullWizard) {
                            switch (entityList.ShipmentLevelCode) {
                                case "D": {
                                    windowTitle = "Direct AWB Wizard";
                                    break;
                                }
                                case "H": {
                                    windowTitle = "House AWB Wizard";
                                    break;
                                }
                                case "C": {
                                    windowTitle = "Master AWB Wizard";
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                        }
                        else {
                            windowTitle = "Airline statuses";
                        }
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 960;
                        logWindow.Height = 600;
                        logWindow.Title = windowTitle;
                        logWindow.WindowArgs = selectedEntityId;
                        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
                        logWindow.WindowClosed.subscribe(function ($event1) {
                            _this.isEditControlOpened = false;
                            _this.OnBackFromEdit(selectedEntityId, $event);
                        });
                    }
                    else if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardComponentPath)) {
                        var windowArgs = {};
                        windowArgs.EntityId = entityList.Id;
                        windowArgs.IsNew = false;
                        windowArgs.IsNewTemplate = true;
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 960;
                        logWindow.Height = 570;
                        logWindow.WindowArgs = windowArgs;
                        var showHeaderButtons = true;
                        windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.EditEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName));
                        switch (this.ObjectTableName) {
                            case "ContainerFollowUp":
                                {
                                    showHeaderButtons = false;
                                    break;
                                }
                                ;
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
                        logWindow.Title = windowTitle;
                        logWindow.ShowHeaderButtons = showHeaderButtons;
                        logWindow.Show(this.SelectedQuery.EditWizardComponentPath);
                        logWindow.WindowClosed.subscribe(function ($event1) {
                            _this.isEditControlOpened = false;
                            _this.OnBackFromEdit(selectedEntityId, $event);
                        });
                    }
                    else if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardName) || this.ObjectTableName == "AgentSharedManifest" || this.ObjectTableName == "Customs.CourierMaster") {
                        if (this.SelectedQuery.EditWizardName == "Simplog.ShipmentLib.Views.AWBWizardEditControl") {
                            var isFullWizard = false;
                            var windowTitle = null;
                            if (entityList.DirectionId == "E" || entityList.DirectionId == "R") {
                                isFullWizard = true;
                            }
                            else if (entityList.DirectionId == "D") {
                                if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                                    isFullWizard = true;
                                }
                            }
                            if (isFullWizard) {
                                switch (entityList.ShipmentLevelCode) {
                                    case "D": {
                                        windowTitle = "Direct AWB Wizard";
                                        break;
                                    }
                                    case "H": {
                                        windowTitle = "House AWB Wizard";
                                        break;
                                    }
                                    case "C": {
                                        windowTitle = "Master AWB Wizard";
                                        break;
                                    }
                                    default: {
                                        break;
                                    }
                                }
                            }
                            else {
                                windowTitle = "Airline statuses";
                            }
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 600;
                            logWindow.Title = windowTitle;
                            logWindow.WindowArgs = selectedEntityId;
                            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
                            logWindow.WindowClosed.subscribe(function ($event1) {
                                _this.isEditControlOpened = false;
                                _this.OnBackFromEdit(selectedEntityId, $event);
                            });
                        }
                        else if (this.ObjectTableName == "Customs.CourierMaster") {
                            var windowArgs = {};
                            this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(function (response) {
                                _this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                        _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                            res.subscribe(function (myResponse) {
                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }
                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                    logWindow.Width = 1500;
                                                    logWindow.Height = 1000;
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    //logWindow.IsHideHeader = true;
                                                    logWindow.IsFillScreen = true;
                                                    AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
                                                    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                                        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
                                                        _this.isEditControlOpened = false;
                                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                }
                                            });
                                        });
                                    });
                                });
                            });
                        }
                        else if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.EditWizardName)) {
                            switch (this.ObjectTableName) {
                                case 'TenantManagmentPrivateLabels': {
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Edit Private Labels";
                                    logWindow.WindowArgs = selectedEntityId;
                                    logWindow.Show('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/PrivateLabelLoadComponent');
                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                        _this.isEditControlOpened = false;
                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                    });
                                    break;
                                }
                                case 'Booking': {
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Edit Booking Wizard";
                                    logWindow.WindowArgs = selectedEntityId;
                                    logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');
                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                        _this.isEditControlOpened = false;
                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                    });
                                    break;
                                }
                                case 'Customer': {
                                    var windowArgs = {};
                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 960;
                                    logWindow.Height = 570;
                                    logWindow.Title = "Invite Customers";
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.Show('./SharedLogistics/Components/InviteCustomersComponent');
                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                        _this.isEditControlOpened = false;
                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                    });
                                    break;
                                }
                                case 'Customs.Client': {
                                    var windowArgs = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(function (response) {
                                        _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                            res.subscribe(function (myResponse) {
                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }
                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                    logWindow.Width = 960;
                                                    logWindow.Height = 570;
                                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.EditClient"); // "Edit Client";
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');
                                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                                        _this.isEditControlOpened = false;
                                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                }
                                            });
                                        });
                                    });
                                    break;
                                }
                                case 'Customs.CustomsVendor': {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(selectedEntityId)) {
                                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
                                            _this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(function (response) {
                                                _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                                    res.subscribe(function (myResponse) {
                                                        if (myResponse.HasError) {
                                                            console.log("Error while getting EntityPM", myResponse);
                                                        }
                                                        else {
                                                            var entity = myResponse.Result;
                                                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                            //Title
                                                            if (!Tools_1.AppTool.IsNullOrEmpty(entity.VendorNumber) && !Tools_1.AppTool.IsNullOrEmpty(entity.VendorName)) {
                                                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorNumber + "-" + entity.VendorName;
                                                            }
                                                            else if (Tools_1.AppTool.IsNullOrEmpty(entity.VendorNumber)) {
                                                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorName;
                                                            }
                                                            else if (Tools_1.AppTool.IsNullOrEmpty(entity.VendorName)) {
                                                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor") + " " + entity.VendorNumber;
                                                            }
                                                            else if (Tools_1.AppTool.IsNullOrEmpty(entity.VendorName) && Tools_1.AppTool.IsNullOrEmpty(entity.VendorNumber)) {
                                                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.EditVendor");
                                                            }
                                                            //title
                                                            var windowArgs = {};
                                                            windowArgs.EntityPM = entity;
                                                            logWindow.Width = 960;
                                                            logWindow.Height = 570;
                                                            //logWindow.Title = "Edit Vendor";
                                                            logWindow.WindowArgs = windowArgs;
                                                            logWindow.ShowCloseButton = true;
                                                            logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/VendorEditComponent');
                                                            logWindow.WindowClosed.subscribe(function ($event1) {
                                                                _this.isEditControlOpened = false;
                                                                _this.OnBackFromEdit(selectedEntityId, $event);
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
                                    var windowArgs = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                                        _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(function (response) {
                                            _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                                _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(function (response) {
                                                    _this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                                                        _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                                            res.subscribe(function (myResponse) {
                                                                if (myResponse.HasError) {
                                                                    console.log("Error while getting EntityPM", myResponse);
                                                                }
                                                                else {
                                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                                    logWindow.Width = 600;
                                                                    logWindow.Height = 710;
                                                                    //      logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                                    logWindow.WindowArgs = windowArgs;
                                                                    logWindow.ShowCloseButton = true;
                                                                    logWindow.IsHideHeader = true;
                                                                    logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
                                                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                                                        _this.isEditControlOpened = false;
                                                                        _this.OnBackFromEdit(selectedEntityId, $event);
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
                                    var windowArgs = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.ProceduralFault").subscribe(function (response) {
                                        _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                            res.subscribe(function (myResponse) {
                                                if (myResponse.HasError) {
                                                    console.log("Error while getting EntityPM", myResponse);
                                                }
                                                else {
                                                    windowArgs.CurrentEntity = myResponse.Result;
                                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                    logWindow.Width = 800;
                                                    logWindow.Height = 400;
                                                    //logWindow.Title = TextCodeTranslator.Translate("Customs.ProceduralFault.Q.ProceduralFaults");
                                                    logWindow.Title = "ליקוי מספר " + myResponse.Result.ProceduralFaultNumber;
                                                    logWindow.WindowArgs = windowArgs;
                                                    logWindow.ShowCloseButton = true;
                                                    logWindow.Show('./CustomsModules/CustomsProceduralFault/Components/EditTabs/General/ProceduralFaultsGeneralTabComponent');
                                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                                        _this.isEditControlOpened = false;
                                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                                    });
                                                }
                                            });
                                        });
                                    });
                                    break;
                                }
                                case "AgentSharedManifest": {
                                    var windowArgs = {};
                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 830;
                                    logWindow.Height = 450;
                                    logWindow.Title = "Shared Manifest";
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent");
                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                        _this.isEditControlOpened = false;
                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                        _this.RefreshBtnClick();
                                    });
                                    break;
                                }
                                case "QuoteTemplate": {
                                    var windowArgs = {};
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    windowArgs.IsNewEntityCall = false;
                                    windowArgs.CurrentEntity = entityList;
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = entityList.Name;
                                    logWindow.Width = window.innerWidth - 150;
                                    logWindow.Height = window.innerHeight - 150;
                                    logWindow.IsShowCloseButton = true;
                                    logWindow.DataContext = this;
                                    logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");
                                    logWindow.WindowClosed.subscribe(function ($event1) {
                                        _this.isEditControlOpened = false;
                                        _this.OnBackFromEdit(selectedEntityId, $event);
                                    });
                                    break;
                                }
                                case 'Customs.DeclarationCargoSplit': {
                                    var windowArgs = {};
                                    this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(function (response) {
                                        _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                            _this.entityPMService.getSingle(_this.ObjectTableName, selectedEntityId).then(function (res) {
                                                res.subscribe(function (myResponse) {
                                                    if (myResponse.HasError) {
                                                        console.log("Error while getting EntityPM", myResponse);
                                                    }
                                                    else {
                                                        windowArgs.CurrentEntity = myResponse.Result;
                                                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                                        logWindow.Width = 770;
                                                        logWindow.Height = 750;
                                                        //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditDeclarationCargoSplit");
                                                        logWindow.Title = "בקשת פיצול מטען "; // + myResponse.Result != null ? ((!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber) ? myResponse.Result.RequestNumber : null) + ((!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName) ? " - " + myResponse.Result.ResponseStatusName : null))) : null;
                                                        if (myResponse.Result != null) {
                                                            if (!Tools_1.AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber)) {
                                                                logWindow.Title = logWindow.Title + myResponse.Result.RequestNumber;
                                                            }
                                                            if (!Tools_1.AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName)) {
                                                                logWindow.Title = logWindow.Title + " - " + myResponse.Result.ResponseStatusName;
                                                            }
                                                        }
                                                        logWindow.WindowArgs = windowArgs;
                                                        logWindow.ShowCloseButton = true;
                                                        //logWindow.IsHideHeader = true;
                                                        logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
                                                        logWindow.WindowClosed.subscribe(function ($event1) {
                                                            _this.isEditControlOpened = false;
                                                            _this.OnBackFromEdit(selectedEntityId, $event);
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
                    else if (this.ObjectTableName == "BIReport") {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({
                                DWQueryId: $event.rowData.DWQueryId,
                                ObjectTableName: 'BIReport',
                                EntityList: $event.rowData,
                                EntityId: $event.rowData.Id
                            });
                            cmpRef.instance.BackCompleted.subscribe(function ($event1) {
                                _this.isEditControlOpened = false;
                                _this.OnBackFromEdit(selectedEntityId, $event);
                            });
                        });
                    }
                    else {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            var label = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.SelectedQuery.NameTextCodeCode);
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({
                                EntityId: selectedEntityId,
                                ObjectTableName: _this.ObjectTableName,
                                BackButtonLabel: label
                            });
                            cmpRef.instance.BackCompleted.subscribe(function ($event1) {
                                _this.isEditControlOpened = false;
                                _this.OnBackFromEdit(selectedEntityId, $event);
                            });
                            //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                            _this.DestroyMe = true;
                            //}
                        });
                    }
                }
            }
            //this.CurrentSession.StopBusyIndicator();
        }
    };
    ListComponent.prototype.OnBackFromEdit = function (selectedEntityId, $event) {
        var _this = this;
        //this.onQueryChangeEvent = new EventEmitter();
        //this.Filterchangeevent = new LogEvents.EventManager();
        //this.SearchFieldchangeevent = new EventEmitter();
        //this.MenuHeaderchangeevent = new EventEmitter();
        //this.ColumnsReady = new EventEmitter();
        //this.isEditControlOpened = false;
        this._entityListService.getSingle(selectedEntityId, this.ObjectTableName, this.MethodName == undefined ? null : this.MethodName).then(function (res) {
            //var re = res;
            _this.DestroyMe = false;
            //this.IsAdvancedSearchOpened = false;
            res.subscribe(function (aa) {
                $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
                //this.CurrentSession.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
                _this.MyScrollTop = $event.scrollTop; //($event.rowIndex * $event.rowHeight) - $event.rowHeight;
                _this.SelectedItem = aa.Result;
                _this.MySelectedRowIndex = $event.rowIndex;
                if (_this.sortColDef && _this.sortColid) {
                    _this.SortServerProp = { colDef: _this.sortColDef, id: _this.sortColid, isBackFromEdit: true };
                }
            });
        });
        //$event.BackFromEdit.emit({ Data: Data, rowIndex:$event.rowIndex });
        //this.RefreshBtnClick();
        //if (this.ReattachToDetection) {
        //    this.ReattachToDetection = false;
        //}
        //else {
        //    this.ReattachToDetection = true;
        //}
    };
    //OnBackFromEdit() {
    //    this.RefreshBtnClick();
    //    //if (this.ReattachToDetection) {
    //    //    this.ReattachToDetection = false;
    //    //}
    //    //else {
    //    //    this.ReattachToDetection = true;
    //    //}
    //}
    ListComponent.prototype.BackButtonClicked = function () {
        this.DestroyListControl();
        this.BackCompleted.emit("event");
    };
    ListComponent.prototype.DestroyListControl = function () {
        if (this.ComponentRef != null) {
            this.CurrentSession.RemoveListComponent(this);
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    };
    ListComponent.prototype.RefreshBookings = function () {
        this.BackCompleted.emit(true);
    };
    Object.defineProperty(ListComponent.prototype, "BackButtonVisibility", {
        get: function () {
            //if (this.ObjectTableName == "Contact" || this.ObjectTableName == "Customer") {
            //    this.showBackButton = false;
            //}
            if (this.listArgs.HideBackButton) {
                this.showBackButton = false;
            }
            return this.showBackButton;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ListComponent.prototype, "IsBackButtonAndTitleVisibile", {
        get: function () {
            if (this.listArgs.IsTasksMenuClicked) {
                this.showBackButtonAndTitle = false;
            }
            return this.showBackButtonAndTitle;
        },
        enumerable: true,
        configurable: true
    });
    ListComponent.prototype.SetAddButton = function () {
        var isVisible = false;
        if (this.ObjectTableName == "Warehouse") {
            if (SessionLocator_1.SessionLocator.TenantPM.CountryCode == "US") {
                isVisible = true;
            }
        }
        else if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline" || this.ObjectTableName == "Port") {
            isVisible = true;
        }
        this.IsAddButtonVisible = isVisible;
    };
    ListComponent.prototype.SetNewEntityButton = function () {
        this.SetNewEntityLabel();
        this.SetNewEntityButtonDisabled();
        this.SetNewEntityButtonVisibility();
    };
    ListComponent.prototype.SetNewEntityLabel = function () {
        if (this.listArgs.NewButtonLabel != null) {
            this.NewEntityButtonLabel = this.listArgs.NewButtonLabel;
        }
        else if (this.ObjectTableName == "Currency") {
            this.NewEntityButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Add");
        }
        else {
            //this.NewEntityButtonLabel = "New " + TextCodeTranslator.TranslateTable(this.ObjectTableName);
            if (Tools_1.AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                var tempText = TextCodeTranslator_1.TextCodeTranslator.Translate(this.listArgs.ObjectTableName + ".NewButton");
                if (!Tools_1.AppTool.IsNullOrEmpty(tempText)) {
                    this.listArgs.NewButtonLabel = tempText;
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.listArgs.NewButtonLabel)) {
                var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
                if (useLocal == true) {
                    var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
                    var ChangedText = GeneralText.split('%')[0];
                    var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName);
                    var FinalText = NewText + " " + ChangedText;
                    this.NewEntityButtonLabel = FinalText; //TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
                }
                else {
                    this.NewEntityButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName));
                }
            }
            else {
                this.NewEntityButtonLabel = this.listArgs.NewButtonLabel;
            }
        }
    };
    ListComponent.prototype.SetNewEntityButtonDisabled = function () {
        var isEnabled = false;
        if (this.SelectedQuery != null) {
            if (this.SelectedQuery.IsAddNewEntityEnabled) {
                isEnabled = true;
                if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
                    var myCodes = [];
                    myCodes.push("EAWB");
                    myCodes.push("BUBK");
                    if (FeatureLocator_1.FeatureLocator.IsPackageOneOf(myCodes)) {
                        isEnabled = false;
                    }
                }
                if (this.TenantPM.Id == 65) {
                    isEnabled = false;
                    if (SessionInfo_1.SessionInfo.LoggedUserPM.IsCustomerCare && (this.ObjectTableName == "User" || this.ObjectTableName == "ChargesType")) {
                        isEnabled = true;
                    }
                }
            }
        }
        this.IsNewEntityButtonDisabled = !isEnabled;
    };
    ListComponent.prototype.SetNewEntityButtonVisibility = function () {
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
                            this.customsSettingListService.getSingleFromCache(this.TenantPM.Id.toString()).subscribe(function (response) {
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
                }
            }
        }
        this.IsNewEntityButtonVisible = isVisible;
    };
    ListComponent.prototype.AddNewEntity = function () {
        var _this = this;
        if (this.SelectedQuery != null) {
            if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions(this.ObjectTableName, "NEW", true)) {
                return;
            }
            if (this.TenantPM.Id != 0 && this.ObjectTableName == "Port") {
                var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
                if (useLocal == true) {
                    var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
                    var ChangedText = GeneralText.split('%')[0];
                    var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName);
                    var FinalText = NewText + " " + ChangedText;
                }
                else {
                    var FinalText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName));
                }
                //var title = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.AddingIsNotAvailable").replace(/%Entity/g, TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName));
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 450;
                messageWindow.Height = 190;
                messageWindow.Title = FinalText;
                messageWindow.Show(message);
                return;
            }
            else {
                var isNewWizard = this.SelectedQuery.ObjectTableIsNewWizard;
                this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
                    if (isNewWizard) {
                        var IsOriginalMaster = false;
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.SelectedQuery.OriginalQueryId)) {
                            var query = window.Queries.filter(function (q) { return q.ObjectTableId == _this.ObjectTable.Id && q.Id == _this.SelectedQuery.OriginalQueryId; })[0];
                            if (query.Code == "Masters" || query.Code == "Open Payables Masters" || query.Code == "All Masters") {
                                IsOriginalMaster = true;
                            }
                        }
                        if (_this.QueryCode == "Masters" || _this.QueryCode == "Open Payables Masters" || _this.QueryCode == "All Masters" || IsOriginalMaster) {
                            _this.RunNewMasterWizard();
                        }
                        else {
                            _this.RunNewEntityWizard(_this.SelectedQuery.ObjectTableNewWizardControlName);
                        }
                    }
                    else {
                        if (_this.ObjectTableName == "APPayment") {
                            _this.NewAPPaymentMethod();
                            // APPaymentTools.Create(eventAggregator, viewInjectionService, regionManager, container);
                        }
                        else if (_this.ObjectTableName == "Journal") {
                            _this.RunNewJournalWizard();
                        }
                        else if (_this.ObjectTableName == "AccountingIntegrityCheck") {
                            _this.RunNewAccountingIntegrityCheckWizard();
                        }
                        else {
                            _this.RunNewGenaricEntity();
                        }
                    }
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(_this.ObjectTableName, "New" + _this.ObjectTableName);
                });
            }
        }
    };
    ListComponent.prototype.RunNewEntityWizard = function (wizardControlName) {
        var _this = this;
        var componentPath = this.ObjectTable.NewWizardComponentPath;
        if (componentPath != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
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
            var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
            if (useLocal == true) {
                var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
                var ChangedText = GeneralText.split('%')[0];
                var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName);
                var FinalText = NewText + " " + ChangedText;
            }
            else {
                var FinalText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName));
            }
            //var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            var str = FinalText;
            //if (this.ObjectTableName == "BIReport") {
            //    str = "Query Builder";
            //}
            if (this.ObjectTableName == "Currency") {
                str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Add") + " Currency";
            }
            if (this.ObjectTableName == "Customs.CustomsVendor") {
                str = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.NewButtonLable)) {
                str = this.NewButtonLable;
            }
            logWindow.Title = str;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.Perspective)) {
                var args = new Args_1.NewEntityArgs();
                args.Perspective = this.SelectedQuery.Perspective;
                args.QueryNameTextCode = Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery) ? null : this.SelectedQuery.NameTextCodeCode;
                logWindow.WindowArgs = args;
            }
            else {
                if (this.ObjectTableName == "BIReport") {
                    var windowArgs = {};
                    windowArgs.IsBIReportWorkspace = true;
                    windowArgs.FolderId = this.listArgs.BIReportFolderId;
                    logWindow.WindowArgs = windowArgs;
                }
                if (this.ObjectTableName == "Tariff") {
                    var QueryCodeOriginal = this.QueryCode;
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQuery.OriginalQueryId)) {
                        var query = window.Queries.filter(function (q) { return q.ObjectTableId == _this.ObjectTable.Id && q.Id == _this.SelectedQuery.OriginalQueryId; })[0];
                        QueryCodeOriginal = query.Code;
                    }
                    var windowArgs = {};
                    logWindow.Width = 850;
                    logWindow.Height = 500;
                    if (QueryCodeOriginal == "Air Freight Cost Tariffs") {
                        logWindow.Title = "New Air Freight Cost";
                        windowArgs.TypeCode = "AFC";
                    }
                    else {
                        "Air Surcharges Cost Tariffs";
                        logWindow.Title = "New Air Surcharges Cost";
                        windowArgs.TypeCode = "ASC";
                    }
                    logWindow.WindowArgs = windowArgs;
                }
                if (this.ObjectTableName == "Questionnaire" || this.ObjectTableName == "CustomerFieldsUpdateSetting") {
                    var windowArgs = {};
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
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            //}
            logWindow.Show(componentPath);
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Show("Fill NewWizard Component Path and Name in ObjectTable !!");
        }
    };
    ListComponent.prototype.RunNewGenaricEntity = function () {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.ObjectTableName).then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = _this.ObjectTableName;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            //var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
            //var ChangedText = GeneralText.split('%')[0];
            //var NewText = TextCodeTranslator.TranslateTable(this.ObjectTableName);
            //var FinalText = NewText + " " + ChangedText;
            var FinalText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(_this.ObjectTableName));
            var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
            if (useLocal == true) {
                var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
                var ChangedText = GeneralText.split('%')[0];
                var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(_this.ObjectTableName);
                FinalText = NewText + " " + ChangedText;
            }
            var windowTitle = FinalText; //TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        });
    };
    ListComponent.prototype.OnNewEntityWindowClosed = function ($event) {
        this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
    };
    ListComponent.prototype.ImportEntitiesCommand = function () {
        var _this = this;
        var windowTitle = "Add " + this.ObjectTableName;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var args = new TenantImportComponent_1.ImportEntityArgs();
        args.ObjectTableId = this.ObjectTable.Id;
        args.ObjectTableName = this.ObjectTableName;
        logWindow.WindowArgs = args;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
        this.ShowIt = false;
        //this.CD.detectChanges();
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.CD.detectChanges();
        });
    };
    ListComponent.prototype.btnExcelCLicked = function () {
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", true)) {
            return;
        }
        else {
            var windowArgs = {};
            windowArgs.query = this.SelectedQuery;
            windowArgs.currentObjectTable = this.ObjectTableName;
            windowArgs.tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            windowArgs.userid = SessionInfo_1.SessionInfo.LoggedUserId;
            windowArgs.Filters = this.CurrentQueryFilters;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 200;
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.ExportingDataToExcel"); //"Exporting View Data List To Excel File";
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
            //logitudeWindow.WindowClosed.subscribe(($event: any) => {
            //    this.QueryValueChanged({ QueryId: this.SelectedQueryId })
            //});
            //});
        }
    };
    ListComponent.prototype.RefreshBtnClick = function () {
        var _this = this;
        if (this.RefreshBtntimerToken) {
            clearTimeout(this.RefreshBtntimerToken);
        }
        this.RefreshBtntimerToken = setTimeout(function () { return _this.DoRefresh(); }, 1000);
    };
    ListComponent.prototype.DoRefresh = function () {
        var _this = this;
        this.MyScrollTop = 0;
        this.MySelectedRowIndex = null;
        this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters(); //this.listArgs.Filters;
        if (this.CurrentQueryFilters == null) {
            this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions(this.ObjectTableName, "READ", true)) {
            return;
        }
        else {
            var query = window.Queries.filter(function (q) { return q.ObjectTableId == _this.ObjectTable.Id && q.Id == _this.SelectedQueryId; })[0];
            if (query != null) {
                if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; }) != null) {
                    var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; });
                    predefinedFilters.forEach(function (filter, key) {
                        var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                        var value1 = filter.PredefinedValue;
                        var value2 = filter.PredefinedValue2;
                        if (value2 != null) {
                            filterOperator = "Between";
                        }
                        if (filter.DataTypeCode == "DateTime") {
                            var TodayDate = new Date();
                            TodayDate.setUTCHours(0, 0, 0, 0);
                            if (value1 == '#today')
                                value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                            if (value2 == '#today')
                                value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                            var TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
                            TommorowDate.setUTCHours(0, 0, 0, 0);
                            var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
                            YesterdayDate.setUTCHours(0, 0, 0, 0);
                            var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
                            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
                            var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
                            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
                            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
                            var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
                            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
                            var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
                            LastYearFromDate.setUTCHours(0, 0, 0, 0);
                            var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
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
                        var field = window.ObjectFields.filter(function (a) { return a.Id == filter.ObjectFieldId; })[0];
                        _this.CurrentQueryFilters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, field.IsCustomFilter, filter.DisplayInList, field.IsCustom, filter.DataTypeCode);
                    });
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(query.DefaultSortColumn) && Tools_1.AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortBy)) {
                    this.CurrentQueryFilters.SortBy = query.DefaultSortColumn;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(query.DefaultSortDirection) && Tools_1.AppTool.IsNullOrEmpty(this.CurrentQueryFilters.SortDirection)) {
                    this.CurrentQueryFilters.SortDirection = query.DefaultSortDirection;
                }
            }
            if (this.FiltersMenu) {
                this.FiltersMenu.AdditionalFilters.forEach(function (filter, key) {
                    if (_this.CurrentQueryFilters && filter.IgnoreFilter) {
                        _this.CurrentQueryFilters.AdditionalFilters = _this.CurrentQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filter.FieldName; });
                    }
                    else {
                        if (_this.CurrentQueryFilters == null) {
                            _this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                        }
                        if (_this.CurrentQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName == filter.FieldName; }).length > 0) {
                            _this.CurrentQueryFilters.AdditionalFilters = _this.CurrentQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != filter.FieldName; });
                        }
                        _this.CurrentQueryFilters.AdditionalFilters.push(filter);
                        //this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, false, filter.DisplayInList, false, filter.FieldDataType);
                    }
                });
                //this.CurrentQueryFilters.AdditionalFilters = this.CurrentQueryFilters.AdditionalFilters.concat(this.FiltersMenu.AdditionalFilters);
            }
            if (this.AdvanceFilters) {
                this.AdvanceFilters.AdditionalFilters.forEach(function (filter, key) {
                    _this.CurrentQueryFilters.AdditionalFilters.push(filter);
                });
            }
            this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters, Reload: false });
            //     else {
            //         this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
            //     }
            //this.onQueryChangeEvent.emit({ QueryId: this.SelectedQueryId, Filters: this.CurrentQueryFilters });
        }
    };
    ListComponent.prototype.ColumnResisedevent = function (Param) {
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
    };
    ListComponent.prototype.SaveColNewChanges = function (Param) {
        var _this = this;
        var QColumns = null;
        this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + this.Tenant + "&queryid=" + Param.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionLocator_1.SessionLocator.LoggedUserId)
            .subscribe(function (response) {
            QColumns = response.json();
            if (QColumns != null) {
                if (_this.GeneralEntitiesArgs == null) {
                    _this.GeneralEntitiesArgs = new GeneralEntitiesArgs_1.GeneralEntitiesArgs();
                }
                _this.GeneralEntitiesArgs.QueryColumnsPMs = [];
                if (!Tools_1.AppTool.IsNullOrEmpty(QColumns[0].UserId)) {
                    if (_this.myQueryColumnsPMService == null) {
                        _this.myQueryColumnsPMService = new QueryColumnsPMService_1.QueryColumnsPMService();
                        _this.myQueryColumnsPMService.setServiceArgs(_this.serviceArgs);
                    }
                    QColumns.forEach(function (querycolumn, key) {
                        querycolumn.IndexOrder = Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Index;
                        if (Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Width > 0) {
                            querycolumn.ColumnWidth = Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Width;
                        }
                        _this.GeneralEntitiesArgs.QueryColumnsPMs.push(querycolumn);
                    });
                    _this.GeneralEntitiesArgs.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    var myGeneralService = new GeneralEntitiesService_1.GeneralEntitiesService();
                    myGeneralService.setServiceArgs(_this.serviceArgs);
                    myGeneralService.update(_this.GeneralEntitiesArgs).subscribe(function (myResult) {
                    });
                }
                else {
                    QColumns.forEach(function (querycolumn, key) {
                        //if (Param.Width > 0 && Param.FieldName == querycolumn.ObjectFieldName) {
                        //    querycolumn.ColumnWidth = Param.Width;
                        //}
                        querycolumn.IndexOrder = Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Index;
                        if (Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Width > 0) {
                            querycolumn.ColumnWidth = Param.ColIndexes.filter(function (a) { return a.FieldName == querycolumn.ObjectFieldName; })[0].Width;
                        }
                        querycolumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                        querycolumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                        _this.GeneralEntitiesArgs.QueryColumnsPMs.push(querycolumn);
                    });
                    _this.GeneralEntitiesArgs.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    var myGeneralService = new GeneralEntitiesService_1.GeneralEntitiesService();
                    myGeneralService.setServiceArgs(_this.serviceArgs);
                    myGeneralService.insert(_this.GeneralEntitiesArgs).subscribe(function (myResult) {
                    });
                    // });
                }
            }
        });
    };
    ListComponent.prototype.RunNewJournalWizard = function () {
        var _this = this;
        var windowTitle = "New Journal";
        var entityPM = new JournalPM_1.JournalPM();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FullAccounting")
            });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                //this.LoadAllScreenData();
                //this.isWindowOpened = false;
                _this.RefreshBtnClick();
            });
        });
    };
    ListComponent.prototype.RunNewMasterWizard = function () {
        var _this = this;
        var componentPath = "./Shipment/Components/NewEntity/NewMasterComponent";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Master";
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.NewWizardArgs = { IsNewEntity: true };
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
        logWindow.Show(componentPath);
    };
    ListComponent.prototype.RunNewAccountingIntegrityCheckWizard = function () {
        var _this = this;
        var __entity = new AccountingIntegrityCheckPM_1.AccountingIntegrityCheckPM();
        __entity.StatusCode = "1";
        __entity.HasException = false;
        __entity.CreateDateTimeUTC = new Date();
        __entity.Tenant = this.Tenant;
        var FinalText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName));
        var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        if (useLocal == true) {
            var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            var ChangedText = GeneralText.split('%')[0];
            var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTableName);
            FinalText = NewText + " " + ChangedText;
        }
        var windowTitle = FinalText;
        var windowArgs = {
            EntityPM: __entity
        };
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 240;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.RefreshBtnClick();
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
    };
    // Run New APPaymnet
    ListComponent.prototype.NewAPPaymentMethod = function () {
        var newApPaymentPM = new APPaymentPM_1.APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, BackButtonLabel: 'A/P Payments', ObjectTableName: 'APPayment' });
        });
    };
    ListComponent.prototype.Navigate = function () {
        var _this = this;
        this.CurrentQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        var MyFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.currentFilters.AdditionalFilters.forEach(function (filter, key) {
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
        var ids = [];
        this._entityListService.getByFilters(this.ObjectTableName, MyFilters, this.MethodName == undefined ? null : this.MethodName).then(function (observable) {
            observable.subscribe(function (response) {
                console.log(response);
                response.Result.forEach(function (item) {
                    ids.push(item.Id);
                });
                console.log(ids);
                var selectedEntityId = ids[0];
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    var label = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.SelectedQuery.NameTextCodeCode);
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: selectedEntityId,
                        ObjectTableName: _this.ObjectTableName,
                        BackButtonLabel: label,
                        NavigationIds: ids,
                    });
                    cmpRef.instance.BackCompleted.subscribe(function ($event1) {
                        _this.isEditControlOpened = false;
                        _this.DestroyMe = false;
                        // this.OnBackFromEdit(selectedEntityId, $event)
                    });
                    //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                    _this.DestroyMe = true;
                    //}
                });
            });
        });
    };
    ListComponent.prototype.OnSortInvoked = function ($event) {
        this.sortColDef = $event.colDef;
        this.sortColid = $event.id;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "BackCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "LoadResourceCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "ColumnsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "QueryListSourceChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ListComponent.prototype, "FiltersBarLoaded", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "onSelectedQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "GridFilterchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "SearchFieldchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], ListComponent.prototype, "AllLocations", void 0);
    ListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ListComponent.html',
            //directives: [CORE_DIRECTIVES, IconButton, LogGridComponent, NgFormControl, AdvanceSearchComponent, QueryListComponent, LocationDirective, SearchTextBox],
            //pipes: [TextCodeTranslationPipe],
            providers: [EntityListService_1.EntityListService, EntityResourceService_1.EntityResourceService, ApiFiltersEvent_1.PubSubService, ApiFiltersEvent1_1.PubSubService1, EntityPMService_1.EntityPMService, TotangoService_1.TotangoService],
        }),
        __metadata("design:paramtypes", [http_1.Http, EntityListService_1.EntityListService, EntityResourceService_1.EntityResourceService, ApiFiltersEvent_1.PubSubService, ApiFiltersEvent1_1.PubSubService1, EntityPMService_1.EntityPMService, TotangoService_1.TotangoService, core_1.ChangeDetectorRef])
    ], ListComponent);
    return ListComponent;
}());
exports.ListComponent = ListComponent;
//# sourceMappingURL=ListComponent.js.map