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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ShipmentAssemblyPM_1 = require("../../../../Shipment/EntityPMs/ShipmentAssemblyPM");
var WarehouseHelper_1 = require("../../../../Warehouse/Helpers/WarehouseHelper");
var ConnectionsTabComponent = /** @class */ (function () {
    function ConnectionsTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.IsNoDataTextVisible = false;
        this.ItemsSource = [];
        this.IsWarehouseEntryVisible = false;
        this.IsNewWarehouseEntryVisible = false;
        this.IsWarehouseReleaseVisible = false;
        this.IsNewWarehouseReleaseVisible = false;
        this.IsAssembliesVisivle = false;
        this.IsDisconnectQuoteVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.EntriesGridHeight = 90;
        this.ReleasesGridHeight = 90;
        this.AssembliesGridHeight = 90;
        this.TicketsGridHeight = 90;
        this.IsQuoteGridVisible = false;
        this.IsMasterGridVisible = false;
        this.IsCustomFileGridVisible = false;
        this.IsTicketsGridVisible = false;
        this.IsNewWarehouseEntryRequested = false;
        this.IsOpenWarehouseEntryScreen = false;
        this.IsNewWarehouseReleaseRequested = false;
        this.IsOpenWarehouseReleaseScreen = false;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.IsNewWarehouseEntryVisible = true;
            }
            this.IsWarehouseEntryVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.IsNewWarehouseReleaseVisible = true;
            }
            this.IsWarehouseReleaseVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "DisconnectQuote")) {
            this.IsDisconnectQuoteVisible = true;
        }
        this.Listen();
        this.LoadData();
    }
    ConnectionsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("ShipmentAssembly").subscribe(function (res1) {
                if (_this.EntityPM.ShipmentLevelCode == "D" || _this.EntityPM.ShipmentLevelCode == "H") {
                    _this.IsAssembliesVisivle = true;
                    _this.FillAssemblies();
                }
            });
        }
    };
    ConnectionsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.IsNewWarehouseEntryRequested)
                        _this.ShowWarehouseScreen("Entry");
                    else if (_this.IsNewWarehouseReleaseRequested)
                        _this.ShowWarehouseScreen("Release");
                    else {
                        _this.LoadData();
                        _this.FillAssemblies();
                    }
                }
                _this.IsOpenWarehouseEntryScreen = false;
                _this.IsOpenWarehouseReleaseScreen = false;
                _this.IsNewWarehouseEntryRequested = false;
                _this.IsNewWarehouseReleaseRequested = false;
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.LoadData();
                    _this.FillAssemblies();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHCN") {
                    _this.LoadData();
                    _this.FillAssemblies();
                }
            });
        }
    };
    ConnectionsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    ConnectionsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetShipmentConnectedEntities(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.FillItemSources(list);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ConnectionsTabComponent.prototype.FillItemSources = function (list) {
        var _this = this;
        this.ItemsSource = [];
        this.WarehouseEntriesItemsSource = [];
        this.WarehouseReleasesItemsSource = [];
        this.QuotesItemsSource = [];
        this.MastersItemsSource = [];
        this.CustomFilesItemsSource = [];
        this.TicketsItemsSource = [];
        list.forEach(function (item) {
            _this.ItemsSource.push(new ShipmentConnectedEntityItem(item, _this));
        });
        this.WarehouseEntriesItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Cross Dock Entry"; });
        this.WarehouseReleasesItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Cross Dock Release"; });
        this.QuotesItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Quote"; });
        this.MastersItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Master"; });
        this.CustomFilesItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Custom File"; });
        this.TicketsItemsSource = this.ItemsSource.filter(function (d) { return d.EntityType == "Ticket"; });
        this.IsQuoteGridVisible = this.QuotesItemsSource.length == 0 ? false : true;
        this.IsMasterGridVisible = this.MastersItemsSource.length == 0 ? false : true;
        this.IsCustomFileGridVisible = this.CustomFilesItemsSource.length == 0 ? false : true;
        this.IsTicketsGridVisible = this.TicketsItemsSource.length == 0 ? false : true;
        this.EntriesGridHeight = this.ComputeGridHeight(this.WarehouseEntriesItemsSource);
        this.ReleasesGridHeight = this.ComputeGridHeight(this.WarehouseReleasesItemsSource);
        this.TicketsGridHeight = this.ComputeGridHeight(this.TicketsItemsSource);
    };
    ConnectionsTabComponent.prototype.ComputeGridHeight = function (list) {
        var height = 90;
        if (list.length == 0 || list.length == 1) {
            height = 90;
        }
        else if (list.length == 2) {
            height = 110;
        }
        else if (list.length == 3) {
            height = 130;
        }
        else {
            height = 160;
        }
        return height;
    };
    ConnectionsTabComponent.prototype.ViewEntity = function (item) {
        var _this = this;
        var myBackButtonLabel = "Shipment: " + this.EntityPM.ShipmentNumber;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.EntityId, ObjectTableName: item.ObjectTableName, BackButtonLabel: myBackButtonLabel, EntityParentPM: _this.EntityPM });
        });
    };
    ConnectionsTabComponent.prototype.NewWarehouseEntryButtonClicked = function () {
        if (!this.IsOpenWarehouseEntryScreen) {
            this.IsOpenWarehouseEntryScreen = true;
            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.IsNewWarehouseEntryRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.ShowWarehouseScreen("Entry");
            }
        }
    };
    ConnectionsTabComponent.prototype.NewWarehouseReleaseButtonClicked = function () {
        if (!this.IsOpenWarehouseReleaseScreen) {
            this.IsOpenWarehouseReleaseScreen = true;
            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.IsNewWarehouseReleaseRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.ShowWarehouseScreen("Release");
            }
        }
    };
    ConnectionsTabComponent.prototype.ShowWarehouseScreen = function (widnowName) {
        var _this = this;
        var windowArgs = {};
        windowArgs.ShipmentPM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = widnowName == "Release" ? "New Cross Dock Release" : "New Cross Dock Entry";
        if (this.EntityPM) {
            var shipmentPackages = this.EntityPM.ShipmentPackages;
            if (shipmentPackages && shipmentPackages.length > 0 && widnowName == "Entry") {
                var wrehouseHelper = new WarehouseHelper_1.WarehouseHelper();
                windowArgs.WarehouseEntryPackagesLists = wrehouseHelper.FullWarehouseEntryPackagePM(shipmentPackages, this.EntityPM, "ShipmentPackages");
            }
        }
        logWindow.WindowArgs = windowArgs;
        var widnowPath = widnowName == "Release" ? "./Warehouse/Components/NewWarehouseReleaseComponent" : "./Warehouse/Components/NewWarehouseEntryComponent";
        logWindow.Show(widnowPath);
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "Refresh") {
                _this.LoadData();
            }
            _this.IsOpenWarehouseReleaseScreen = false;
            _this.IsOpenWarehouseEntryScreen = false;
        });
    };
    ConnectionsTabComponent.prototype.FillAssemblies = function () {
        this.AssembliesItemsSource = [];
        this.AssembliesItemsSource = this.EntityPM.ShipmentAssemblies;
        this.AssembliesGridHeight = this.ComputeGridHeight(this.AssembliesItemsSource);
    };
    ConnectionsTabComponent.prototype.AddAssembly = function () {
        var itemPM = new ShipmentAssemblyPM_1.ShipmentAssemblyPM(null);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        itemPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        itemPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        itemPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.RunAssemblyWindow(itemPM, "Add Shipment Assembly");
    };
    ConnectionsTabComponent.prototype.EditAssembly = function (itemPM) {
        this.RunAssemblyWindow(itemPM, "Edit Shipment Assembly");
    };
    ConnectionsTabComponent.prototype.DeleteAssembly = function (itemPM) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this assembly?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var index = _this.EntityPM.ShipmentAssemblies.indexOf(itemPM);
                if (index > -1) {
                    _this.EntityPM.RemoveAssembly(itemPM);
                    _this.FillAssemblies();
                }
            }
        });
    };
    ConnectionsTabComponent.prototype.RunAssemblyWindow = function (item, windowTitle) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.WindowArgs = { EntityPM: item, ShipmentPM: this.EntityPM };
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Connections/AddEditShipmentAssemblyComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "ok") {
                _this.FillAssemblies();
            }
        });
    };
    ConnectionsTabComponent.prototype.DisconnectQuote = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("After disconnecting the quote from the shipment you will not be able to generate Receivables / Payables from this quote");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.myDomainService.DisconnectQuote(_this.EntityPM.Id).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                            _this.entityArgs.EditComponent.ReloadEntityPM();
                            _this.CurrentSession.FireEvent("LoadConnectedShipments");
                            _this.LoadData();
                        }
                    }
                });
            }
        });
    };
    ConnectionsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ConnectionsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ConnectionsTabComponent);
    return ConnectionsTabComponent;
}());
exports.ConnectionsTabComponent = ConnectionsTabComponent;
var ShipmentConnectedEntityItem = /** @class */ (function () {
    function ShipmentConnectedEntityItem(entity, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.myEntity = new ShipmentDomainService_1.ShipmentConnectedEntity();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myEntity = entity;
        this.ComputeDateProperties();
    }
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "EntityId", {
        get: function () { return this.myEntity.EntityId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "EntityType", {
        get: function () { return this.myEntity.EntityType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "EntityNumber", {
        get: function () { return this.myEntity.Reference; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "ObjectTableName", {
        get: function () { return this.myEntity.ObjectTableName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "EntityStatus", {
        get: function () { return this.myEntity.EntityStatus; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "OpenDate", {
        get: function () { return this.myEntity.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "AcceptedDate", {
        get: function () { return this.myEntity.AcceptedDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentConnectedEntityItem.prototype, "Salesman", {
        get: function () { return this.myEntity.Salesman; },
        enumerable: true,
        configurable: true
    });
    ShipmentConnectedEntityItem.prototype.ComputeDateProperties = function () {
        if (this.myEntity.ActualDate != null) {
            this.EntityDate = this.myEntity.ActualDate;
            this.Foreground = Tools_1.FontTool.Green;
            this.DateType = "(actual)";
        }
        else if (this.myEntity.ExpectedDate != null) {
            this.EntityDate = this.myEntity.ExpectedDate;
            this.Foreground = Tools_1.FontTool.Red;
            this.DateType = "(expected)";
        }
        else {
            this.DateType = "";
        }
    };
    return ShipmentConnectedEntityItem;
}());
//# sourceMappingURL=ConnectionsTabComponent.js.map