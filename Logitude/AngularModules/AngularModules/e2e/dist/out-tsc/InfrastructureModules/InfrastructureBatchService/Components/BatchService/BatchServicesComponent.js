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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var BatchServicesComponent = /** @class */ (function () {
    function BatchServicesComponent() {
        this.ItemsSource = [];
        this.LogsItemsSource = [];
        this.loadedDataList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.BatchFilterList = [];
        this.filterTypeCode = "AL";
        this.IsLogsGridVsisible = false;
        this.globalDomainService = new GlobalDomainService_1.GlobalDomainService();
        this.infraDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this.FillBatchFilterList();
        this.GetBatchServicesDefinitions();
    }
    BatchServicesComponent.prototype.FillBatchFilterList = function () {
        this.BatchFilterList = [];
        this.BatchFilterList.push(new CodeNameClass_1.CodeNameClass("L1D", "Last Day"));
        this.BatchFilterList.push(new CodeNameClass_1.CodeNameClass("L2D", "Last Two Days"));
        this.BatchFilterList.push(new CodeNameClass_1.CodeNameClass("L1H", "Last Hour"));
        this.BatchFilterList.push(new CodeNameClass_1.CodeNameClass("L2H", "Last Two Hours"));
        this.BatchFilterList.push(new CodeNameClass_1.CodeNameClass("L1Y", "Last Year"));
        this.selectedBatchFilter = this.BatchFilterList.filter(function (d) { return d.Code == "L1D"; })[0];
        this.GetBatchServicesDefinitions();
    };
    Object.defineProperty(BatchServicesComponent.prototype, "SelectedBatchFilter", {
        get: function () { return this.selectedBatchFilter; },
        set: function (value) {
            if (this.selectedBatchFilter != value) {
                this.selectedBatchFilter = value;
                this.GetBatchServicesDefinitions();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServicesComponent.prototype, "FilterTypeCode", {
        get: function () { return this.filterTypeCode; },
        set: function (value) {
            if (this.filterTypeCode != value) {
                this.filterTypeCode = value;
                this.IsLogsGridVsisible = false;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    BatchServicesComponent.prototype.Selecting = function (item) {
        this.SelectedRow = item;
        if (item == null) {
            this.IsLogsGridVsisible = false;
        }
        else {
            this.LoadBatchServicesLogs();
        }
    };
    BatchServicesComponent.prototype.GetBatchServicesDefinitions = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.IsLogsGridVsisible = false;
        this.globalDomainService.GetAllBatchServicesDefinitionsPMs(this.SelectedBatchFilter.Code).subscribe(function (myResult) {
            if (myResult == null) {
                _this.ItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.loadedDataList = myResponse.Result;
                    _this.BuildItemsSource();
                }
            }
        });
    };
    BatchServicesComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var items = [];
        if (this.FilterTypeCode == "AC") {
            items = this.loadedDataList.filter(function (d) { return d.InActive == false; });
        }
        else if (this.FilterTypeCode == "IN") {
            items = this.loadedDataList.filter(function (d) { return d.InActive == true; });
        }
        else {
            items = this.loadedDataList;
        }
        items.forEach(function (item) {
            _this.ItemsSource.push(new BatchServiceItemClass(item));
        });
        this.CurrentSession.StopBusyIndicator();
    };
    BatchServicesComponent.prototype.LoadBatchServicesLogs = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.infraDomainService.GetBatchServicesLogs(this.SelectedRow.Code, this.SelectedBatchFilter.Code).subscribe(function (myResult) {
            if (myResult == null) {
                _this.LogsItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.LogsItemsSource = myResponse.Result;
                    _this.IsLogsGridVsisible = true;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    BatchServicesComponent.prototype.EditClicked = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Batch Service";
        logWindow.DataContext = item;
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/BatchService/EditBatchServiceComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.RefreshButtonClicked();
            }
        });
    };
    BatchServicesComponent.prototype.RefreshButtonClicked = function () {
        this.GetBatchServicesDefinitions();
    };
    BatchServicesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    BatchServicesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BatchServicesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BatchServicesComponent);
    return BatchServicesComponent;
}());
exports.BatchServicesComponent = BatchServicesComponent;
var BatchServiceItemClass = /** @class */ (function (_super) {
    __extends(BatchServiceItemClass, _super);
    function BatchServiceItemClass(item) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BatchServicesDefinition";
        _this.EntityPM = item;
        return _this;
    }
    Object.defineProperty(BatchServiceItemClass.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "LastActivity", {
        get: function () { return this.EntityPM.LastActivity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "NumberOfDoneItems", {
        get: function () { return this.EntityPM.NumberOfDoneItems; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "DoneItemsInOneMinute", {
        get: function () { return this.EntityPM.DoneItemsInOneMinute; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "DoneItemsInFiveMinutes", {
        get: function () { return this.EntityPM.DoneItemsInFiveMinutes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "DoneItemsInOneHour", {
        get: function () { return this.EntityPM.DoneItemsInOneHour; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "NumberOfThreads", {
        get: function () { return this.EntityPM.NumberOfThreads; },
        set: function (newValue) {
            if (this.EntityPM.NumberOfThreads != newValue) {
                this.EntityPM.NumberOfThreads = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BatchServiceItemClass.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return BatchServiceItemClass;
}(BaseComponent_1.BaseComponent));
exports.BatchServiceItemClass = BatchServiceItemClass;
//# sourceMappingURL=BatchServicesComponent.js.map