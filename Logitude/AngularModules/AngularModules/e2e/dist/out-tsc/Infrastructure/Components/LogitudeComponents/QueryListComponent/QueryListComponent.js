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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
//import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../../Infrastructure/DataContracts/ServiceArgs");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var QueriesPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/QueriesPMService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var QueryListComponent = /** @class */ (function () {
    function QueryListComponent(CD) {
        this.CD = CD;
        this.Text = null;
        this.Binding = null;
        this.SelectedItem = null;
        this.ControlId = null;
        this.DropdownId = null;
        this.NewViewId = null;
        this.ListControlId = null;
        this.MinHeight = 30;
        this.MaxHeight = 1000;
        this.itemSelectedEvent = new core_1.EventEmitter();
        this.QueriesChangedEvent = new core_1.EventEmitter();
        this.NewViewClosedEvent = new core_1.EventEmitter();
        this.ignoreMouseDown = false;
        this.ignoreItemClicked = false;
        this.ignorePublicClicked = false;
        this.newViewClicked = false;
        this.ShowButtons = false;
        this.LayoutDirection = 'ltr';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsOpened = false;
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this.serviceArgs.http = ServiceHelper_1.ServiceHelper.Http;
        this.ItemsSource = [];
        this.UserItemSource = [];
        this.HandledUserItemSource = [];
        this.NotSharedUserItemSource = [];
        this.SharedUserItemSource = [];
        if (this.CurrentSession == null) {
            this.ControlId = "QueryList_-1_-1";
            this.DropdownId = "QueryListDropdown_-1_-1";
            this.ListControlId = "QueryListList_-1_-1";
            this.NewViewId = "NewViewId_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("ComboBox");
            this.ControlId = "QueryList_" + idIndex;
            this.DropdownId = "QueryListDropdown_" + idIndex;
            this.ListControlId = "QueryListList_" + idIndex;
            this.NewViewId = "NewViewId_" + idIndex;
        }
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    Object.defineProperty(QueryListComponent.prototype, "ItemsSource", {
        get: function () { return this.itemsSource; },
        set: function (newValue) {
            this.itemsSource = newValue;
        },
        enumerable: true,
        configurable: true
    });
    QueryListComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
        var xx = this.DropdownId;
        var temp = this.ignorePublicClicked;
        this.onSelectedQueryChangeEvent.subscribe(function (res) {
            _this.ItemClicked(res, true);
            _this.ComputeListHeight(_this.ItemsSource.length + _this.UserItemSource.length);
        });
        this.FillUserItemSource_Share();
        this.QueryListSourceChanged.subscribe(function (res) {
            _this.UserItemSource = res;
        });
    };
    QueryListComponent.prototype.FillUserItemSource_Share = function () {
        this.NotSharedUserItemSource = [];
        this.SharedUserItemSource = [];
        this.NotSharedUserItemSource = this.UserItemSource.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.SharedByUserId); });
        this.SharedUserItemSource = this.UserItemSource.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.SharedByUserId); });
    };
    QueryListComponent.prototype.ngAfterViewInit = function () {
        this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
    };
    QueryListComponent.prototype.ComputeListHeight = function (ItemsCount) {
        //if (ItemsCount == 0) {
        //    document.getElementById(this.ListControlId).style.height = this.MinHeight + "px";
        //}
        //else {
        //    var itemsHeight = ((ItemsCount * 23) + 3);
        //    if (itemsHeight > this.MaxHeight) {
        //        document.getElementById(this.ListControlId).style.height = this.MaxHeight + "px";
        //    }
        //    else {
        //        document.getElementById(this.ListControlId).style.height = itemsHeight + 36 + "px";
        //    }
        //}
    };
    QueryListComponent.prototype.mousedown = function (event) {
        if (this.ItemsSource.length == 0 && this.UserItemSource.length == 0) {
            document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
        }
        else {
            var itemsHeight = ((this.ItemsSource.length * 23) + (this.UserItemSource.length * 23) + 3);
            if (itemsHeight > this.MaxHeight) {
                document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
            }
            else {
                document.getElementById(this.DropdownId).style.height = itemsHeight + 36 + "px";
                document.getElementById(this.ListControlId).style.height = "100%";
            }
        }
        document.getElementById(this.DropdownId).style.visibility = "visible";
    };
    QueryListComponent.prototype.OnFocus = function () {
    };
    QueryListComponent.prototype.OnLostFocus = function () {
        document.getElementById(this.DropdownId).style.height = "0px";
        document.getElementById(this.DropdownId).style.visibility = "hidden";
    };
    QueryListComponent.prototype.ComboBoxClicked = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;
            if (!this.IsOpened) {
                item.blur();
            }
        }
    };
    QueryListComponent.prototype.ItemClicked = function (clickedItem, IgnoreChange) {
        if (IgnoreChange === void 0) { IgnoreChange = false; }
        if (clickedItem != null && this.ignoreItemClicked == false) {
            this.ignoreMouseDown = true;
            if ((this.SelectedItem != clickedItem) || IgnoreChange) {
                this.SelectedItem = clickedItem;
                var myComboBox = document.getElementById(this.ControlId);
                if (myComboBox != null) {
                    myComboBox.blur();
                }
                this.SetDisplayText();
                var filters = new ApiQueryFilters_1.ApiQueryFilters();
                if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == clickedItem.Id; }) != null) {
                    var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == clickedItem.Id; });
                    predefinedFilters.forEach(function (filter, key) {
                        var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                        var value1 = filter.PredefinedValue;
                        var value2 = filter.PredefinedValue2;
                        if (value2 != null) {
                            filterOperator = "Between";
                        }
                        if (filter.DataTypeCode == "DateTime") {
                            var TodayDate = new Date();
                            if (value1 == '#today')
                                value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                            if (value2 == '#today')
                                value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);
                            var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
                            var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
                            var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
                            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                            var CurrentYearToDate = new Date();
                            var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
                            var LastYearToDate = new Date();
                            if (value1 == "Today") {
                                value1 = TodayDate;
                                filterOperator = "Equals";
                            }
                            else if (value1 == "Yesterday") {
                                value1 = YesterdayDate;
                                filterOperator = "GreaterThanOrEqual";
                            }
                            else if (value1 == "Last 7 Days") {
                                value1 = LastSevenDaysDate;
                                filterOperator = "GreaterThanOrEqual";
                            }
                            else if (value1 == "Last 30 Days") {
                                value1 = LastThirtyDaysDate;
                                filterOperator = "GreaterThanOrEqual";
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
                        var ObjectField = window.ObjectFields.filter(function (d) { return d.Id == filter.ObjectFieldId; });
                        filters.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, ObjectField.IsCustomFilter, filter.DisplayInList, ObjectField.IsCustom, filter.DataTypeCode);
                    });
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(clickedItem.DefaultSortColumn)) {
                    filters.SortBy = clickedItem.DefaultSortColumn;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(clickedItem.DefaultSortDirection)) {
                    filters.SortDirection = clickedItem.DefaultSortDirection;
                }
                this.itemSelectedEvent.emit({ QueryId: clickedItem.Id, Filters: filters, Title: TextCodeTranslator_1.TextCodeTranslator.Translate(clickedItem.NameTextCodeCode) });
            }
        }
        else if (this.ignoreItemClicked == true) {
            this.ignoreItemClicked = false;
        }
    };
    QueryListComponent.prototype.SetDisplayText = function () {
        var myDisplayText = null;
        if (this.SelectedItem != null) {
            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }
            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }
        this.Text = TextCodeTranslator_1.TextCodeTranslator.Translate(myDisplayText);
    };
    QueryListComponent.prototype.NewViewClicked = function () {
        var _this = this;
        this.newViewClicked = true;
        this.ignoreMouseDown = false;
        var windowArgs = {};
        windowArgs.queryId = this.SelectedItem.Id;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = true;
        windowArgs.pubSubAdvanceQueryFiltersService = this.pubSubAdvanceQueryFiltersService;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.CreateNewView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
            if ($event != _this.SelectedItem.Id) {
                CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                    var Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.Id == $event; })[0];
                    _this.UserItemSource.push(Query);
                    _this.ComputeListHeight(_this.ItemsSource.length + _this.UserItemSource.length);
                    _this.SelectedItem = Query;
                    _this.SetDisplayText();
                    _this.FillUserItemSource_Share();
                    _this.QueriesChangedEvent.emit(Query);
                    _this.NewViewClosedEvent.emit("");
                });
            }
        });
    };
    QueryListComponent.prototype.IgnoreMouseDown = function () {
        if (this.newViewClicked == true) {
            this.ignoreMouseDown = false;
        }
        else {
            this.ignoreMouseDown = true;
        }
    };
    QueryListComponent.prototype.DeleteButtonClicked = function (Item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.DeletQuery");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                var query = window.Queries.filter(function (q) { return q.Id == Item.Id; })[0];
                var myService = new QueriesPMService_1.QueriesPMService();
                myService.setServiceArgs(_this.serviceArgs);
                myService.delete(query, SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (myResult) {
                    _this.CurrentSession.StopBusyIndicator();
                    window.Queries = window.Queries.filter(function (a) { return a.Id != query.Id; });
                    var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
                    var Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0; })[0];
                    _this.UserItemSource = _this.UserItemSource.filter(function (a) { return a.Id != query.Id; });
                    _this.ComputeListHeight(_this.ItemsSource.length + _this.UserItemSource.length);
                    _this.FillUserItemSource_Share();
                    _this.QueriesChangedEvent.emit(Query);
                });
            }
        });
        //this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
        //this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        //this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        //this.ignoreItemClicked = true;
        //var ObjectTable = window.ObjectTables.filter(a => a.Name == this.ObjectTableName)[0];
        //var confirmWindow = new ConfirmWindow();
        //confirmWindow.Title = TextCodeTranslator.Translate("General.O.DeletQuery");
        //confirmWindow.Show(TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
        //confirmWindow.WindowClosed.subscribe((event: any) => {
        //    if (confirmWindow.Yes) {
        //        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        //        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        //        var myQCService: QueryColumnsPMService = new QueryColumnsPMService();
        //        myQCService.setServiceArgs(this.serviceArgs);
        //        myQCService.GetQueryColumnPMs(SessionInfo.LoggedUserTenant, Item.Id, ObjectTable.Id, SessionInfo.LoggedUserId).subscribe(myResult => {
        //            var queryColumns = myResult;
        //            queryColumns.forEach((column, key) => {
        //                this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(column);
        //            });
        //            if (this.myAdvancedQueryFiltersPMService == null) {
        //                this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //            }
        //            this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        //            this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, Item.Id).subscribe(myResult => {
        //                if (myResult == null) {
        //                    this.AdvancedQueryFilterPMs = [];
        //                }
        //                else {
        //                    this.AdvancedQueryFilterPMs = myResult;
        //                    var advanceQueryFilters = this.AdvancedQueryFilterPMs.filter(c => c.QueryId == Item.Id);
        //                    advanceQueryFilters.forEach((filter, key) => {
        //                        this.GeneralEntitiesArgs.RemovedQueryFilters.push(filter);
        //                    });
        //                }
        //                var query = window.Queries.filter(q => q.Id == Item.Id)[0];
        //                var myService: QueriesPMService = new QueriesPMService();
        //                myService.setServiceArgs(this.serviceArgs);
        //                var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        //                myGeneralService.setServiceArgs(this.serviceArgs);
        //                myGeneralService.update(this.GeneralEntitiesArgs).subscribe(myResult => {
        //                    myService.delete(query).subscribe(myResult => {
        //                        this.CurrentSession.StopBusyIndicator();
        //                        window.Queries = window.Queries.filter(a => a.Id != query.Id);
        //                        var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        //                        var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];
        //                        this.UserItemSource = this.UserItemSource.filter(a => a.Id != query.Id);
        //                        this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
        //                        this.FillUserItemSource_Share();
        //                        this.QueriesChangedEvent.emit(Query);
        //                    });
        //                });
        //            });
        //        });
        //    }
        //});
    };
    QueryListComponent.prototype.EditButtonClicked = function (Item) {
        var _this = this;
        this.ignoreItemClicked = true;
        this.ignoreMouseDown = false;
        var windowArgs = {};
        windowArgs.queryId = Item.Id;
        windowArgs.pubSubAdvanceQueryFiltersService = this.pubSubAdvanceQueryFiltersService;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = false;
        windowArgs.QueryName = TextCodeTranslator_1.TextCodeTranslator.Translate(Item[this.Binding]);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.EditView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
            var Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.Id == $event; })[0];
            if (!Query) {
                Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0 && a.Id != $event; })[0];
            }
            _this.SetDisplayText();
            _this.QueriesChangedEvent.emit(Query);
            _this.FillUserItemSource_Share();
            _this.NewViewClosedEvent.emit("");
            _this.CD.detectChanges();
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryListComponent.prototype, "itemSelectedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryListComponent.prototype, "QueriesChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryListComponent.prototype, "NewViewClosedEvent", void 0);
    QueryListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'QueryList',
            templateUrl: './QueryListComponent.html',
            inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'UserItemSource', 'ObjectTableName', 'onSelectedQueryChangeEvent', 'LoadResourceCompleted', 'pubSubAdvanceQueryFiltersService', 'QueryListSourceChanged'],
            providers: [http_1.Http],
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], QueryListComponent);
    return QueryListComponent;
}());
exports.QueryListComponent = QueryListComponent;
//# sourceMappingURL=QueryListComponent.js.map