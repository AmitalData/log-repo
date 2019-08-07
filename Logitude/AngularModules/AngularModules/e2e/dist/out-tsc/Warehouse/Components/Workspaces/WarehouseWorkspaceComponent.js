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
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Args_1 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var WarehouseReleasePMExtendedService_1 = require("../../Services/ExtendedPMs/WarehouseReleasePMExtendedService");
var WarehouseEntryListExtendedService_1 = require("../../Services/ExtendedLists/WarehouseEntryListExtendedService");
var WarehouseReleaseListExtendedService_1 = require("../../Services/ExtendedLists/WarehouseReleaseListExtendedService");
var WarehouseWorkspaceComponent = /** @class */ (function (_super) {
    __extends(WarehouseWorkspaceComponent, _super);
    function WarehouseWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ReloadWarehouseEntryQueries = new core_1.EventEmitter();
        _this.ReloadWarehouseReleaseQueries = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.WarehouseEntryCreatedQueryCount = 0;
        _this.WarehouseEntryEnteredQueryCount = 0;
        _this.WarehouseEntryConnectedToShipmentsQueryCount = 0;
        _this.WarehouseEntryNotConnectedToShipmentsQueryCount = 0;
        _this.WarehouseEntryAllQueryCount = 0;
        _this.WarehouseReleaseCreatedQueryCount = 0;
        _this.WarehouseReleaseReleasedQueryCount = 0;
        _this.WarehouseReleaseAllQueryCount = 0;
        _this.WarehouseReleaseCanceledQueryCount = 0;
        _this.SearchText = "Search";
        _this.RecentWarehouseReleasesLists = [];
        _this.IsNoDataVisible_RecentWarehouseReleases = false;
        // Release Queries Features
        _this.WarehouseReleaseVisibility = false;
        _this.AllReleaseQueriesVisibility = false;
        _this.ReleasedQueriesVisibility = false;
        _this.CancledReleaseQueriesVisibility = false;
        _this.CreatedReleaseQueriesVisibility = false;
        _this.CreatedEntryQueriesVisibility = false;
        _this.EnteredEntryQueriesVisibility = false;
        _this.ConnectedToShipmentsEntryQueriesVisibility = false;
        _this.NotConnectedToShipmentsEntryQueriesVisibility = false;
        _this.AllEntryQueriesVisibility = false;
        _this.RecentWarehouseEntriesLists = [];
        _this.IsNoDataVisible_RecentWarehouseEntries = false;
        _this.mySelectedDirectionFilter = "All";
        _this.mySelectedTransportFilter = "All";
        _this.SetReleasesQueriesVisibility();
        _this.SetEntrysQueriesVisibility();
        _this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService_1.WarehouseReleasePMExtendedService();
        _this.warehouseEntryListExtendedService = new WarehouseEntryListExtendedService_1.WarehouseEntryListExtendedService();
        _this.warehouseReleaseListExtendedService = new WarehouseReleaseListExtendedService_1.WarehouseReleaseListExtendedService();
        _this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe(function (response) {
        });
        return _this;
    }
    WarehouseWorkspaceComponent.prototype.InitComponent = function () {
        this.LoadAllData();
    };
    WarehouseWorkspaceComponent.prototype.BuildCustomQueriesList = function () {
        this.ReloadWarehouseEntryQueries.emit();
        this.ReloadWarehouseReleaseQueries.emit();
    };
    WarehouseWorkspaceComponent.prototype.LoadAllData = function () {
        this.LoadDataSummary();
        this.LoadRecentWarehouseEntries();
        this.LoadRecentWarehouseReleases();
        this.BuildCustomQueriesList();
    };
    WarehouseWorkspaceComponent.prototype.LoadRecentWarehouseReleases = function () {
        var _this = this;
        this.RecentWarehouseReleasesLists = [];
        this.IsNoDataVisible_RecentWarehouseReleases = false;
        this.warehouseReleaseListExtendedService.GetRecentWarehouseReleases().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.RecentWarehouseReleasesLists = myResponse.Result;
                        if (_this.RecentWarehouseReleasesLists.length == 0) {
                            _this.IsNoDataVisible_RecentWarehouseReleases = true;
                        }
                    }
                }
            }
        });
    };
    WarehouseWorkspaceComponent.prototype.SetReleasesQueriesVisibility = function () {
        this.AllReleaseQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.AllReleasesQuery") ? true : false;
        this.ReleasedQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.ReleasedQuery") ? true : false;
        this.CancledReleaseQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.CancelledReleasesQuery") ? true : false;
        this.CreatedReleaseQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.CreatedReleasesQuery") ? true : false;
        this.WarehouseReleaseVisibility = this.AllReleaseQueriesVisibility || this.ReleasedQueriesVisibility || this.CancledReleaseQueriesVisibility || this.CreatedReleaseQueriesVisibility ? true : false;
    };
    WarehouseWorkspaceComponent.prototype.SetEntrysQueriesVisibility = function () {
        this.CreatedEntryQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.CreatedEntriesQuery") ? true : false;
        this.EnteredEntryQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.EnteredEntriesQuery") ? true : false;
        this.ConnectedToShipmentsEntryQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.ConnectedEntriesQuery") ? true : false;
        this.NotConnectedToShipmentsEntryQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.NotConnectedEntriesQuery") ? true : false;
        this.AllEntryQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.AllEntriesQuery") ? true : false;
    };
    WarehouseWorkspaceComponent.prototype.LoadRecentWarehouseEntries = function () {
        var _this = this;
        this.RecentWarehouseEntriesLists = [];
        this.IsNoDataVisible_RecentWarehouseEntries = false;
        this.warehouseEntryListExtendedService.GetRecentWarehouseEntries().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.RecentWarehouseEntriesLists = myResponse.Result;
                    if (_this.RecentWarehouseEntriesLists.length == 0) {
                        _this.IsNoDataVisible_RecentWarehouseEntries = true;
                    }
                }
            }
        });
    };
    Object.defineProperty(WarehouseWorkspaceComponent.prototype, "SelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (value) {
            if (this.mySelectedDirectionFilter != value) {
                this.mySelectedDirectionFilter = value;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseWorkspaceComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (value) {
            if (this.mySelectedTransportFilter != value) {
                this.mySelectedTransportFilter = value;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseWorkspaceComponent.prototype.LoadDataSummary = function () {
        var _this = this;
        // this.CurrentSession.StartBusyIndicatorLoading();
        this.warehouseReleasePMExtendedService.GetCrossDockWorkspaceSummary(this.SelectedTransportFilter, this.SelectedDirectionFilter).subscribe(function (res) {
            var pmResponse = res;
            //  this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var crossDockWorkspaceSummaryClass = pmResponse.Result;
                if (crossDockWorkspaceSummaryClass != null) {
                    _this.WarehouseEntryCreatedQueryCount = crossDockWorkspaceSummaryClass.CreatedWarehouseEntriesCount;
                    _this.WarehouseEntryEnteredQueryCount = crossDockWorkspaceSummaryClass.EnteredWarehouseEntriesCount;
                    _this.WarehouseEntryConnectedToShipmentsQueryCount = crossDockWorkspaceSummaryClass.ConnectedToShipmentsWarehouseEntriesCount;
                    _this.WarehouseEntryNotConnectedToShipmentsQueryCount = crossDockWorkspaceSummaryClass.NotConnectedToShipmentsWarehouseEntriesCount;
                    _this.WarehouseEntryAllQueryCount = crossDockWorkspaceSummaryClass.AllWarehouseEntriesCount;
                    _this.WarehouseReleaseCreatedQueryCount = crossDockWorkspaceSummaryClass.CreatedWarehouseReleasesCount;
                    _this.WarehouseReleaseReleasedQueryCount = crossDockWorkspaceSummaryClass.WarehouseReleasedCount;
                    _this.WarehouseReleaseAllQueryCount = crossDockWorkspaceSummaryClass.AllWarehouseReleasesCount;
                    _this.WarehouseReleaseCanceledQueryCount = crossDockWorkspaceSummaryClass.CanceledReleasesCount;
                }
            }
        });
    };
    WarehouseWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllData();
    };
    WarehouseWorkspaceComponent.prototype.onWarehouseEntryQueriesBackComplete = function (event) {
    };
    WarehouseWorkspaceComponent.prototype.onWarehouseReleaseQueriesBackComplete = function (event) {
    };
    WarehouseWorkspaceComponent.prototype.NewEntryButtonclick = function () {
        var _this = this;
        var args = {};
        args.ShipmentLevelCode = "D";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New Cross Dock Entry";
        logWindow.Show('./Warehouse/Components/NewEntity/NewFullWarehouseEntryComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.LoadAllData();
            }
        });
    };
    WarehouseWorkspaceComponent.prototype.EditWarehouseEntry = function () {
    };
    WarehouseWorkspaceComponent.prototype.SetDirectionTransportFilter = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedDirectionFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, true, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTransportFilter) && this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string");
        }
    };
    WarehouseWorkspaceComponent.prototype.ViewWarehouseEntryQuery = function (queryName) {
        var _this = this;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var queryCode = "";
        var displayTitle = "";
        switch (queryName) {
            case "All":
                {
                    queryCode = "AllEntriesQuery";
                    displayTitle = "All Entries";
                    break;
                }
            case "Created":
                {
                    queryCode = "CreatedEntriesQuery";
                    displayTitle = "Created Entries";
                    break;
                }
            case "Entered":
                {
                    queryCode = "EnteredEntriesQuery";
                    displayTitle = "Entered Entries";
                    break;
                }
            case "ConnectedToShipments":
                {
                    queryCode = "ConnectedEntriesQuery";
                    displayTitle = "Connected To Shipments Entries";
                    break;
                }
            case "NotConnectedToShipments":
                {
                    queryCode = "NotConnectedEntriesQuery";
                    displayTitle = "Not Connected To Shipments Entries";
                    break;
                }
        }
        this.SetDirectionTransportFilter();
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = "WarehouseEntry";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Cross Docks";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                var filtersBar = null;
                cmpRef.instance.FiltersBarLoaded.subscribe(function (myBar) {
                    filtersBar = myBar;
                    if (filtersBar) {
                        if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                            filtersBar.SetTransport(_this.SelectedTransportFilter);
                        }
                        if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                            filtersBar.SetDirection(_this.SelectedDirectionFilter);
                        }
                    }
                });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    if (filtersBar) {
                        if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                            _this.mySelectedTransportFilter = filtersBar.SelectedValue;
                        }
                        if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                            _this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                        }
                    }
                    _this.LoadAllData();
                });
                listArgs.SelectedDirection = _this.mySelectedDirectionFilter;
                listArgs.SelectedTransportMode = _this.mySelectedTransportFilter;
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    WarehouseWorkspaceComponent.prototype.ViewWarehouseReleaseQuery = function (queryName) {
        var _this = this;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var queryCode = "";
        var displayTitle = "";
        switch (queryName) {
            case "All":
                {
                    queryCode = "AllReleasesQuery";
                    displayTitle = "All Releases";
                    break;
                }
            case "Created":
                {
                    queryCode = "CreatedReleasesQuery";
                    displayTitle = "Created Releases";
                    break;
                }
            case "Released":
                {
                    queryCode = "ReleasedQuery";
                    displayTitle = "Entered Releases";
                    break;
                }
            case "Canceled":
                {
                    queryCode = "CancelledReleasesQuery";
                    displayTitle = "Canceled Releases";
                    break;
                }
        }
        this.SetDirectionTransportFilter();
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = "WarehouseRelease";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Cross Docks";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                var filtersBar = null;
                cmpRef.instance.FiltersBarLoaded.subscribe(function (myBar) {
                    filtersBar = myBar;
                    if (filtersBar) {
                        if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                            filtersBar.SetTransport(_this.SelectedTransportFilter);
                        }
                        if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                            filtersBar.SetDirection(_this.SelectedDirectionFilter);
                        }
                    }
                });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    if (filtersBar) {
                        if (filtersBar.SelectedValue != _this.SelectedTransportFilter) {
                            _this.mySelectedTransportFilter = filtersBar.SelectedValue;
                        }
                        if (filtersBar.SelectedDirection != _this.SelectedDirectionFilter) {
                            _this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                        }
                    }
                    _this.LoadAllData();
                });
                listArgs.SelectedDirection = _this.mySelectedDirectionFilter;
                listArgs.SelectedTransportMode = _this.mySelectedTransportFilter;
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    WarehouseWorkspaceComponent.prototype.EditWarehouseEntries = function (item) {
        var _this = this;
        var myBackButtonLabel = "Cross Docks";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseEntry", BackButtonLabel: myBackButtonLabel });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.LoadAllData();
            });
        });
    };
    WarehouseWorkspaceComponent.prototype.EditWarehouseReleases = function (item) {
        var _this = this;
        var myBackButtonLabel = "Cross Docks";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.LoadAllData();
            });
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], WarehouseWorkspaceComponent.prototype, "ReloadWarehouseEntryQueries", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], WarehouseWorkspaceComponent.prototype, "ReloadWarehouseReleaseQueries", void 0);
    WarehouseWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'WarehouseWorkspaceComponent',
            templateUrl: './WarehouseWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WarehouseWorkspaceComponent);
    return WarehouseWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.WarehouseWorkspaceComponent = WarehouseWorkspaceComponent;
//# sourceMappingURL=WarehouseWorkspaceComponent.js.map