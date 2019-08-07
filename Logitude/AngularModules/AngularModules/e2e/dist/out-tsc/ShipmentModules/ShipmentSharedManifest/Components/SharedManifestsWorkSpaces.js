"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SharedAgentManifestService_1 = require("../../../Shipment/Services/Others/SharedAgentManifestService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../Infrastructure/Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var GroupByClass_1 = require("../../../Infrastructure/DataContracts/Dashboard/GroupByClass");
var SharedManifestsWorkSpaces = /** @class */ (function (_super) {
    __extends(SharedManifestsWorkSpaces, _super);
    function SharedManifestsWorkSpaces(_sharedAgentManifestService) {
        var _this = _super.call(this) || this;
        _this._sharedAgentManifestService = _sharedAgentManifestService;
        _this.ReloadAgentSharedManifestsQueries = new core_1.EventEmitter();
        _this.AgentSharedManifestsAirCount = 0;
        _this.AgentSharedManifestsOceanCount = 0;
        _this.AgentSharedManifestsInlandCount = 0;
        _this.DataContext = _this;
        _this.IsShareDocumentsButtonVisible = false;
        _this.AgentSharedManifestsAllVisibility = false;
        _this.AgentSharedManifestsCancelledVisibility = false;
        _this.AgentSharedManifestsAirVisibility = false;
        _this.AgentSharedManifestsOceanVisibility = false;
        _this.AgentSharedManifestsInlandVisibility = false;
        _this.SharedManinfestInDashboardId = "SharedManinfestDashboardId_";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SearchText = "Search";
        _this.barChartLabels = [];
        _this.barChartData = [{ data: [], label: '', dateRange: [] }, { data: [], label: '', dateRange: [] }, { data: [], label: '', dateRange: [] }];
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedDocument", "NEW")) {
            _this.IsShareDocumentsButtonVisible = true;
        }
        _this.SharedManinfestInDashboardId = _this.SharedManinfestInDashboardId + _this.CurrentSession.GetChartId();
        return _this;
    }
    SharedManifestsWorkSpaces.prototype.InitComponent = function () {
        this.LoadAllData();
    };
    SharedManifestsWorkSpaces.prototype.BuildCustomQueriesList = function () {
        this.ReloadAgentSharedManifestsQueries.emit();
    };
    SharedManifestsWorkSpaces.prototype.EditAgentSharedManifest = function (item) {
        var windowArgs = {};
        windowArgs.CurrentEntity = item;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 830;
        logWindow.Height = 450;
        logWindow.Title = "Shared Manifest";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent");
        logWindow.WindowClosed.subscribe(function ($event1) {
        });
    };
    SharedManifestsWorkSpaces.prototype.ViewAgentSharedManifestQuery = function (myCode, nameTextCodeCode) {
        var _this = this;
        if (nameTextCodeCode === void 0) { nameTextCodeCode = null; }
        var queryCode = "";
        var displayTitle = "";
        if (myCode != null) {
            if (nameTextCodeCode) {
                queryCode = myCode;
                displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(nameTextCodeCode);
            }
            else {
                switch (myCode) {
                    case "Air":
                        {
                            queryCode = "AirAgentSharedManifests";
                            displayTitle = "Air Agent Shared Manifests";
                            break;
                        }
                    case "Ocean":
                        {
                            queryCode = "OceanAgentSharedManifests";
                            displayTitle = "Ocean Agent Shared Manifests";
                            break;
                        }
                    case "Inland":
                        {
                            queryCode = "InlandAgentSharedManifests";
                            displayTitle = "Inland Agent Shared Manifests";
                            break;
                        }
                    case "Cancelled":
                        {
                            queryCode = "CancelledAgentSharedManifests";
                            displayTitle = "Cancelled Agent Shared Manifests";
                            break;
                        }
                    case "All":
                        {
                            queryCode = "Agent Shared Manifests";
                            displayTitle = "All Agent Shared Manifests";
                            break;
                        }
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = new ApiQueryFilters_1.ApiQueryFilters();
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "AgentSharedManifest";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Shared Manifests";
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllData(); });
            });
        }
    };
    SharedManifestsWorkSpaces.prototype.LoadAllData = function () {
        this.LoadDataSummary();
        this.SetPropertyVisibility();
        this.BuildCustomQueriesList();
        if (!this.SelectedTimeRangeItem) {
            this.FillTimeRangeFilterList();
        }
        else {
            this.LoadBarQueries();
        }
    };
    SharedManifestsWorkSpaces.prototype.SetPropertyVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "AgentSharedManifestQ"))
            this.AgentSharedManifestsAllVisibility = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "AirAgentSharedManifestsQ"))
            this.AgentSharedManifestsAirVisibility = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "OceanAgentSharedManifestsQ"))
            this.AgentSharedManifestsOceanVisibility = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "InlandAgentSharedManifestsQ"))
            this.AgentSharedManifestsInlandVisibility = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "CancelledAgentSharedManifestsQ"))
            this.AgentSharedManifestsCancelledVisibility = true;
    };
    SharedManifestsWorkSpaces.prototype.LoadDataSummary = function () {
        var _this = this;
        this._sharedAgentManifestService.getAgentSharedManifestsWorkspaceSummary().subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.AgentSharedManifestsAirCount = myResult.AgentSharedManifestsAirCount > 1000 ? "1000+" : myResult.AgentSharedManifestsAirCount.toString();
                    _this.AgentSharedManifestsOceanCount = myResult.AgentSharedManifestsOceanCount > 1000 ? "1000+" : myResult.AgentSharedManifestsOceanCount.toString();
                    _this.AgentSharedManifestsInlandCount = myResult.AgentSharedManifestsInlandCount > 1000 ? "1000+" : myResult.AgentSharedManifestsInlandCount.toString();
                    //this.AgentSharedManifestsAllCount = myResult.AgentSharedManifestsAllCount > 1000 ? "1000+" : myResult.AgentSharedManifestsAllCount.toString();
                    //this.AgentSharedManifestsCancelledCount = myResult.AgentSharedManifestsCancelledCount > 1000 ? "1000+" : myResult.AgentSharedManifestsCancelledCount.toString();
                }
            }
        });
    };
    SharedManifestsWorkSpaces.prototype.RefreshButtonClicked = function () {
        this.LoadAllData();
    };
    SharedManifestsWorkSpaces.prototype.onAgentSharedManifestQueriesBackComplete = function (event) {
    };
    SharedManifestsWorkSpaces.prototype.DocumentsPermissionsLinkClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Documents Permissions";
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent");
    };
    SharedManifestsWorkSpaces.prototype.FillTimeRangeFilterList = function () {
        var list2 = LastFilter_1.LastFilter.ActivitymyList();
        this.TimeRangeFilterList = [];
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list2[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list2[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list2[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list2[3].lastTitle, "3"));
        this.SelectedTimeRangeItem = this.TimeRangeFilterList[1];
    };
    Object.defineProperty(SharedManifestsWorkSpaces.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (value != this.fromDate) {
                this.fromDate = value;
                // this.LoadBarQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestsWorkSpaces.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (value != this.toDate) {
                this.toDate = value;
                //   this.LoadBarQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestsWorkSpaces.prototype, "SelectedTimeRangeItem", {
        get: function () { return this.selectedTimeRangeItem; },
        set: function (value) {
            if (this.selectedTimeRangeItem != value) {
                this.selectedTimeRangeItem = value;
                this.LoadBarQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedManifestsWorkSpaces.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        this.fromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedTimeRangeItem.Index == "0") {
            days = -7;
            Todate.setDate(Todate.getDate() - 6);
            this.toDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "1") {
            days = -30;
            Todate.setMonth(Todate.getMonth() - 1);
            this.toDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.toDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "3") {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.toDate = Todate;
        }
        return days;
    };
    SharedManifestsWorkSpaces.prototype.LoadBarQueries = function () {
        var _this = this;
        var days = this.ComputeDays();
        this.BarData = [];
        this._sharedAgentManifestService.GetAgentSharedManifestsForDashBoard(0, days, +this.SelectedTimeRangeItem.Index).subscribe(function (myResult) {
            _this.BarData = myResult.Result;
            var groupedData = [];
            //this.BarData.sort((a, b) => { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1 });
            _this.BarData.forEach(function (item) {
                var existsedItem = groupedData.filter(function (f) { return f.DateRange == item.DateRange && f.DataType == item.DataType; })[0];
                if (existsedItem == null) {
                    existsedItem = new GroupByClass_1.GroupByClass();
                    existsedItem.XField = item.DateRange;
                    existsedItem.DataType = item.DataType;
                    if (item.TotalAmount == null)
                        existsedItem.YField = 0;
                    else
                        existsedItem.YField = parseInt(item.TotalAmount + "");
                    existsedItem.dateRange = item.DateRange;
                    groupedData.push(existsedItem);
                }
                else {
                    if (item.TotalAmount == null)
                        existsedItem.YField += 0;
                    else
                        existsedItem.YField += parseInt(item.TotalAmount + "");
                }
            });
            _this.BarData = groupedData;
            _this.FillBars();
        });
    };
    SharedManifestsWorkSpaces.prototype.FillBars = function () {
        var _this = this;
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        this.barChartData[0].data = [];
        this.barChartData[1].data = [];
        this.barChartData[2].data = [];
        this.barChartLabels = [];
        var max = 0;
        var even = 0;
        this.BarData.forEach(function (element) {
            if (i == 0) {
                Graphs = [{
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-1" + i,
                        "title": "A",
                        "type": "column",
                        "valueField": "col1",
                        "fillColors": ["#487E9F", "#c8d8e2"],
                        "lineAlpha": 0,
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": "O",
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col2",
                        "fillColors": ["#DA7B38", "#ecbd9b"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-3" + i,
                        "title": "I",
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col3",
                        "fillColors": ["#21782E", "#90bb96"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    }
                ];
            }
            if (element.DataType == 'A') {
                _this.barChartData[0].label = element.DataType;
                _this.barChartData[0].data[i] = element.YField;
                _this.barChartData[0].dateRange[i] = element.DateRange;
            }
            else if (element.DataType == 'O') {
                _this.barChartData[1].label = element.DataType;
                _this.barChartData[1].data[i] = element.YField;
                _this.barChartData[1].dateRange[i] = element.DateRange;
            }
            else if (element.DataType == 'I') {
                _this.barChartData[2].label = element.DataType;
                _this.barChartData[2].data[i] = element.YField;
                _this.barChartData[2].dateRange[i] = element.DateRange;
            }
            if (!_this.barChartLabels.includes(element.XField)) {
                _this.barChartLabels[i] = element.XField;
            }
            even++;
            if (even % 3 == 0)
                i++;
            if (element.YField > max)
                max = element.YField;
        });
        var i = 0;
        this.barChartLabels.forEach(function (item) {
            DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i], "col2": _this.barChartData[1].data[i], "col3": _this.barChartData[2].data[i] };
            i++;
        });
        makeAmBarChart(this.SharedManinfestInDashboardId, Graphs, DataProvider, max, null, null, 0);
    };
    SharedManifestsWorkSpaces.prototype.BarClicking = function () {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    };
    SharedManifestsWorkSpaces.prototype.OnBarClick = function (e) {
        var _this = this;
        var flag = false;
        var displayName = "";
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        if (item && item.values && item.values.value != 0) {
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var Key = e.target.columnIndex;
            var category = item.category;
            var values = item.values;
            if (flag) {
                var typeName = "";
                var typeCode = "";
                switch (Key + "") {
                    case "0": {
                        typeName = "Air";
                        typeCode = "A";
                        break;
                    }
                    case "1": {
                        typeName = "Ocean";
                        typeCode = "O";
                        break;
                    }
                    case "2": {
                        typeName = "Inland";
                        typeCode = "I";
                        break;
                    }
                }
                var date = this.barChartData[Key].dateRange[item.index];
                var customFilterValue = date + "@" + this.SelectedTimeRangeItem.Index;
                this.filterAgrs.addAdditionalFilter("BarDataCustomFilter", customFilterValue, null, null, "Equals", true, false, false, "date");
                var queryCode = "";
                var displayTitle = "";
                switch (typeName) {
                    case "Air":
                        {
                            queryCode = "AirAgentSharedManifests";
                            displayTitle = "Air Agent Shared Manifests";
                            break;
                        }
                    case "Ocean":
                        {
                            queryCode = "OceanAgentSharedManifests";
                            displayTitle = "Ocean Agent Shared Manifests";
                            break;
                        }
                    case "Inland":
                        {
                            queryCode = "InlandAgentSharedManifests";
                            displayTitle = "Inland Agent Shared Manifests";
                            break;
                        }
                }
                var listArgs = new Args_1.ListComponentArgs();
                listArgs.Filters = this.filterAgrs;
                listArgs.QueryCode = queryCode;
                listArgs.ObjectTableName = "AgentSharedManifest";
                listArgs.DisplayTitle = displayTitle;
                listArgs.BackButtonTitle = "Shared Manifests";
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllData(); });
                });
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SharedManifestsWorkSpaces.prototype, "ReloadAgentSharedManifestsQueries", void 0);
    SharedManifestsWorkSpaces = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestsWork',
            templateUrl: './SharedManifestsWorkSpaces.html',
            providers: [SharedAgentManifestService_1.SharedAgentManifestService],
        }),
        __metadata("design:paramtypes", [SharedAgentManifestService_1.SharedAgentManifestService])
    ], SharedManifestsWorkSpaces);
    return SharedManifestsWorkSpaces;
}(BaseComponent_1.BaseComponent));
exports.SharedManifestsWorkSpaces = SharedManifestsWorkSpaces;
//# sourceMappingURL=SharedManifestsWorkSpaces.js.map