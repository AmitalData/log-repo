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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ConsoleShipmentPM_1 = require("../../../../Shipment/EntityPMs/ConsoleShipmentPM");
var ShipmentListService_1 = require("../../../../Shipment/Services/StandardLists/ShipmentListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var Args_1 = require("../../../../Shipment/Args");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ShipmentsTabComponent = /** @class */ (function (_super) {
    __extends(ShipmentsTabComponent, _super);
    function ShipmentsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.TransportModeId = null;
        _this.DataContext = _this;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.ItemsSource1Hidden = false;
        _this.ItemsSource2Hidden = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.FromLabel = null;
        _this.ToLabel = null;
        _this.IsEditingEnabled = true;
        _this.HasFilters = false;
        _this.isAdvancedSearchOpened = false;
        _this.searchText = null;
        _this.fromPortId = null;
        _this.toPortId = null;
        _this.branchId = null;
        _this.isDirectShipmentsIncluded = false;
        _this.isConnectedShipmentsIncluded = false;
        _this.isLoadHousesRequested = false;
        _this.isLoadMasterRequested = false;
        _this.CellNotesWidth = 0;
        _this.SummaryQuantity = 0;
        _this.SummaryGrossWeight = 0;
        _this.SummaryVolumetricWeight = 0;
        _this.SummaryChargeableWeight = 0;
        _this.isInProgress = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.TransportModeId = _this.EntityPM.TransportModeId;
        _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
        _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
        _this.myService = new ShipmentListService_1.ShipmentListService();
        _this.InitializeComponent();
        _this.SetUIProperties();
        _this.LoadAllHouses();
        _this.Listen();
        return _this;
    }
    ShipmentsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.UpdateFiltersFields();
                    _this.SetUIProperties();
                    if (_this.isLoadMasterRequested) {
                        _this.isLoadHousesRequested = true;
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    else {
                        switch (_this.myRequestedCommandCode) {
                            case "N": {
                                _this.RunNewShipment();
                                break;
                            }
                            case "V": {
                                _this.RunViewShipment();
                                break;
                            }
                        }
                    }
                }
                _this.isInProgress = false;
                _this.isLoadMasterRequested = false;
                _this.myRequestedCommandCode = null;
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.UpdateFiltersFields();
                    _this.SetUIProperties();
                    if (_this.isLoadHousesRequested) {
                        _this.LoadAllHouses();
                    }
                }
                _this.isLoadHousesRequested = false;
            });
        }
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "ReloadHouses") {
                _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                _this.UpdateFiltersFields();
                _this.LoadAllHouses();
            }
        });
    };
    ShipmentsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ShipmentsTabComponent.prototype.InitializeComponent = function () {
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.Gateway") + ":";
                this.ToLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.Destination") + ":";
                break;
            }
            case "O": {
                this.FromLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.LoadingPort") + ":";
                this.ToLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.DischargePort") + ":";
                break;
            }
            case "I": {
                this.FromLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.From") + ":";
                this.ToLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.NewShipment.To") + ":";
                break;
            }
        }
        this.UpdateFiltersFields();
    };
    ShipmentsTabComponent.prototype.UpdateFiltersFields = function () {
        this.fromPortId = this.EntityPM.MainCarriageFromPortId;
        this.toPortId = this.EntityPM.MainCarriageFinalDestinationPortId;
        this.branchId = this.EntityPM.BranchId;
    };
    ShipmentsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
    };
    ShipmentsTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllHouses();
    };
    Object.defineProperty(ShipmentsTabComponent.prototype, "IsAdvancedSearchOpened", {
        get: function () { return this.isAdvancedSearchOpened; },
        set: function (value) {
            if (this.isAdvancedSearchOpened != value) {
                this.isAdvancedSearchOpened = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentsTabComponent.prototype.AdvancedSearchButtonClicked = function () {
        this.IsAdvancedSearchOpened = !this.IsAdvancedSearchOpened;
    };
    Object.defineProperty(ShipmentsTabComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsTabComponent.prototype, "FromPortId", {
        get: function () { return this.fromPortId; },
        set: function (newValue) {
            if (this.fromPortId != newValue) {
                this.fromPortId = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsTabComponent.prototype, "ToPortId", {
        get: function () { return this.toPortId; },
        set: function (newValue) {
            if (this.toPortId != newValue) {
                this.toPortId = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsTabComponent.prototype, "BranchId", {
        get: function () { return this.branchId; },
        set: function (newValue) {
            if (this.branchId != newValue) {
                this.branchId = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsTabComponent.prototype, "IsDirectShipmentsIncluded", {
        get: function () { return this.isDirectShipmentsIncluded; },
        set: function (newValue) {
            if (this.isDirectShipmentsIncluded != newValue) {
                this.isDirectShipmentsIncluded = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentsTabComponent.prototype, "IsConnectedShipmentsIncluded", {
        get: function () { return this.isConnectedShipmentsIncluded; },
        set: function (newValue) {
            if (this.isConnectedShipmentsIncluded != newValue) {
                this.isConnectedShipmentsIncluded = newValue;
                this.LoadItemsSource2();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentsTabComponent.prototype.SetDirectShipmentsIncluded = function (value) {
        this.IsDirectShipmentsIncluded = value;
    };
    ShipmentsTabComponent.prototype.SetConnectedShipmentsIncluded = function (value) {
        this.IsConnectedShipmentsIncluded = value;
    };
    ShipmentsTabComponent.prototype.LoadAllHouses = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.LoadItemsSource1();
    };
    ShipmentsTabComponent.prototype.LoadItemsSource1 = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("MasterShipmentDataId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
        filters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.ItemsSource1 = [];
                    var list1 = [];
                    var list2 = [];
                    myResponse.Result.forEach(function (item) {
                        if (item != null) {
                            if (item.FHLStatusCode == "SENT") {
                                list1.push(new HAWBItem(item, _this, true));
                            }
                            else {
                                list2.push(new HAWBItem(item, _this, true));
                            }
                        }
                    });
                    list1 = list1.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    list2 = list2.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    if (list1) {
                        _this.ItemsSource1 = _this.ItemsSource1.concat(list1);
                    }
                    if (list2) {
                        _this.ItemsSource1 = _this.ItemsSource1.concat(list2);
                    }
                }
            }
            _this.BuildSummary();
            _this.LoadItemsSource2();
        });
    };
    ShipmentsTabComponent.prototype.LoadItemsSource2 = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("DirectionId", this.EntityPM.DirectionId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", this.EntityPM.TransportModeId, null, null, "Equals", false, false, false, "string");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPortId)) {
            filters.addAdditionalFilter("FromPortId", this.FromPortId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPortId)) {
            filters.addAdditionalFilter("ToPortId", this.ToPortId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BranchId)) {
            filters.addAdditionalFilter("BranchId", this.BranchId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
            var myShipmentTypeId = null;
            switch (this.EntityPM.ShipmentTypeId.toUpperCase()) {
                case "MYGO": {
                    myShipmentTypeId = "LCLD";
                    break;
                }
                case "MYGI": {
                    myShipmentTypeId = "LTL";
                    break;
                }
                default: {
                    myShipmentTypeId = this.EntityPM.ShipmentTypeId;
                    break;
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(myShipmentTypeId)) {
                filters.addAdditionalFilter("ShipmentTypeId", myShipmentTypeId, null, null, "Equals", false, false, false, "string");
            }
        }
        // Custom
        if (!this.IsDirectShipmentsIncluded) {
            filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, false, false, "string");
        }
        filters.addAdditionalFilter("ConnectedToOtherMastersFilter", this.IsConnectedShipmentsIncluded, null, null, "Equals", true, false, false, "Boolean");
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.ItemsSource2 = [];
                    var list1 = [];
                    var list2 = [];
                    myResponse.Result.forEach(function (item) {
                        if (item != null) {
                            if (item.MasterShipmentDataId != _this.EntityPM.Id) {
                                var newItem = new HAWBItem(item, _this, false);
                                if (newItem.IsMatched) {
                                    list1.push(newItem);
                                }
                                else {
                                    list2.push(newItem);
                                }
                            }
                        }
                    });
                    list1 = list1.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    list2 = list2.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    if (list1) {
                        _this.ItemsSource2 = _this.ItemsSource2.concat(list1);
                    }
                    if (list2) {
                        _this.ItemsSource2 = _this.ItemsSource2.concat(list2);
                    }
                }
            }
            _this.SetCellNotesWidth();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ShipmentsTabComponent.prototype.SetCellNotesWidth = function () {
        var myColumnWidth = 0;
        this.ItemsSource2.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CellNotes)) {
                var widthOfLabel = Tools_1.AppTool.GetTextWidth(item.CellNotes) + 10;
                if (widthOfLabel > myColumnWidth) {
                    myColumnWidth = widthOfLabel;
                }
            }
        });
        this.CellNotesWidth = myColumnWidth;
    };
    ShipmentsTabComponent.prototype.BuildSummary = function () {
        this.SummaryQuantity = Tools_1.ArrayTool.Sum(this.ItemsSource1, "Quantity");
        this.SummaryGrossWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "GrossWeight");
        this.SummaryChargeableWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "ChargeableWeight");
        this.SummaryVolumetricWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "VolumetricWeight");
    };
    ShipmentsTabComponent.prototype.Save = function () {
        this.isLoadMasterRequested = true;
        this.entityArgs.EditComponent.SaveChanges();
    };
    ShipmentsTabComponent.prototype.NewShipmentClicked = function () {
        this.myRequestedCommandCode = "N";
        this.entityArgs.EditComponent.SaveChanges();
    };
    ShipmentsTabComponent.prototype.ViewShipmentClicked = function (item) {
        if (item) {
            this.myRequestedHouseId = item.Id;
            this.myRequestedCommandCode = "V";
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    ShipmentsTabComponent.prototype.RunNewShipment = function () {
        var _this = this;
        var args = new Args_1.NewShipmentComponentArgs();
        args.ShipmentLevelCode = "H";
        args.IsShipmentLevelFixed = true;
        args.IsCreatedFromMasterHouses = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New House Shipment";
        logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
        logWindow.ComponentLoaded.subscribe(function (cmp) {
            var myShipmentTypeId = _this.EntityPM.ShipmentTypeId;
            if (_this.EntityPM.ShipmentTypeName) {
                if (_this.EntityPM.ShipmentTypeName.toLowerCase().indexOf("my groupage") > -1) {
                    if (_this.EntityPM.TransportModeId == "O") {
                        myShipmentTypeId = "LCLD";
                    }
                    else {
                        myShipmentTypeId = "LTL";
                    }
                }
            }
            cmp.DirectionId = _this.EntityPM.DirectionId;
            cmp.TransportModeId = _this.EntityPM.TransportModeId;
            cmp.ShipmentTypeId = myShipmentTypeId;
            cmp.MainCarriageFromPortId = _this.EntityPM.FromPortId;
            cmp.MainCarriageToPortId = _this.EntityPM.ToPortId;
            cmp.SalesmanUserId = _this.EntityPM.SalesmanUserId;
            cmp.EntityPM.BranchId = _this.EntityPM.BranchId;
            cmp.EntityPM.DepartmentId = _this.EntityPM.DepartmentId;
            cmp.EntityPM.MasterShipmentDataId = _this.EntityPM.MasterShipmentDataId;
            cmp.EntityPM.SCI = _this.EntityPM.SCI;
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.isLoadHousesRequested = true;
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    };
    ShipmentsTabComponent.prototype.RunViewShipment = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.myRequestedHouseId, ObjectTableName: 'Shipment', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.isLoadHousesRequested = true;
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
            cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    ShipmentsTabComponent.prototype.ConnectAllClicked = function () {
        var _this = this;
        if (!this.isInProgress) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Please confirm connecting all the disconnected shipments to this Master shipment");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.isInProgress = true;
                    var saving = false;
                    _this.ItemsSource2.forEach(function (item) {
                        if (item.IsMatched) {
                            var itemPM = _this.EntityPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == item.Id; })[0];
                            if (itemPM == null) {
                                itemPM = new ConsoleShipmentPM_1.ConsoleShipmentPM(_this.EntityPM);
                                itemPM.Id = item.Id;
                                itemPM.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                                itemPM.MasterShipmentDataId = _this.EntityPM.Id;
                                _this.EntityPM.AddConsoleShipment(itemPM);
                                saving = true;
                            }
                        }
                    });
                    if (saving) {
                        _this.Save();
                    }
                    else {
                        _this.isInProgress = false;
                    }
                }
            });
        }
    };
    ShipmentsTabComponent.prototype.DisconnectAllClicked = function () {
        var _this = this;
        if (!this.isInProgress) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Please confirm disconnecting all the connected shipments from this Master shipment");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.isInProgress = true;
                    var saving = false;
                    _this.ItemsSource1.forEach(function (item) {
                        var itemPM = _this.EntityPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == item.Id; })[0];
                        if (itemPM != null) {
                            _this.EntityPM.RemoveConsoleShipment(itemPM);
                            saving = true;
                        }
                    });
                    if (saving) {
                        _this.Save();
                    }
                    else {
                        _this.isInProgress = false;
                    }
                }
            });
        }
    };
    ShipmentsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentsTabComponent.html',
        })
        // islam: merge test
        ,
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ShipmentsTabComponent);
    return ShipmentsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ShipmentsTabComponent = ShipmentsTabComponent;
var HAWBItem = /** @class */ (function () {
    function HAWBItem(item, fatherComponent, isConnected) {
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsMatched = false;
        this.Quantity = 0;
        this.CellNotes = null;
        if (item != null) {
            this.isChecked = isConnected;
            this.SetIsMatched();
            this.SetCellNotes();
            if (fatherComponent.IsFCLEntity) {
                this.Quantity = item.NumberOfContainers;
            }
            else {
                this.Quantity = item.NumberOfPackages;
            }
        }
    }
    Object.defineProperty(HAWBItem.prototype, "Id", {
        get: function () { return this.item.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "ShipmentNumber", {
        get: function () { return this.item.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "ShipmentType", {
        get: function () { return this.item.ShipmentType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "CreateDateTime", {
        get: function () { return this.item.CreateDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "StatusName", {
        get: function () { return this.item.StatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "BranchName", {
        get: function () { return this.item.BranchName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "House", {
        get: function () { return this.item.House; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "CustomerName", {
        get: function () { return this.item.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "FromPort", {
        get: function () { return this.item.FromPort; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "ToPort", {
        get: function () { return this.item.ToPort; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "GrossWeight", {
        get: function () { return this.item.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "VolumetricWeight", {
        get: function () { return this.item.VolumetricWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "ChargeableWeight", {
        get: function () { return this.item.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HAWBItem.prototype, "JobNumber", {
        get: function () { return (this.item.ShipmentNumber == this.item.MasterShipmentNumber) ? "" : this.item.MasterShipmentNumber; ; },
        enumerable: true,
        configurable: true
    });
    HAWBItem.prototype.SetIsMatched = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.FromPortId == this.fatherComponent.EntityPM.MainCarriageFromPortId && this.item.ToPortId == this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId && this.item.BranchId == this.fatherComponent.EntityPM.BranchId) {
            this.IsMatched = true;
        }
    };
    HAWBItem.prototype.SetCellNotes = function () {
        var _this = this;
        var myResult = null;
        if (this.item.ShipmentLevelCode == "D") {
            myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.M.Shipments.OnlyHouseConnected");
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.MasterShipmentDataId != this.fatherComponent.EntityPM.Id) {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.M.Shipments.AlreadyConnected");
            }
            else if (this.item.FromPortId != this.fatherComponent.EntityPM.MainCarriageFromPortId || this.item.ToPortId != this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId) {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.M.Shipments.DoesntMatch");
            }
            else if (this.fatherComponent.EntityPM.ShipmentARInvoices.filter(function (d) { return d.InvoiceTypeCode == "MN"; }).length > 0) {
                if (this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == _this.Id; }).length > 0) {
                    myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.M.Shipments.HasManifestInvoice");
                }
            }
        }
        this.CellNotes = myResult;
    };
    Object.defineProperty(HAWBItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            if (this.isChecked != newValue) {
                this.isChecked = newValue;
                this.AddRemove();
            }
        },
        enumerable: true,
        configurable: true
    });
    HAWBItem.prototype.AddRemove = function () {
        var _this = this;
        if (this.IsChecked) {
            var itemPM = this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == _this.Id; })[0];
            if (itemPM == null) {
                itemPM = new ConsoleShipmentPM_1.ConsoleShipmentPM(this.fatherComponent.EntityPM);
                itemPM.Id = this.Id;
                itemPM.ShipmentNumber = this.fatherComponent.EntityPM.ShipmentNumber;
                itemPM.MasterShipmentDataId = this.fatherComponent.EntityPM.Id;
                this.fatherComponent.EntityPM.AddConsoleShipment(itemPM);
                this.fatherComponent.Save();
            }
        }
        else {
            var itemPM = this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == _this.Id; })[0];
            if (itemPM != null) {
                this.fatherComponent.EntityPM.RemoveConsoleShipment(itemPM);
                this.fatherComponent.Save();
            }
        }
    };
    return HAWBItem;
}());
//# sourceMappingURL=ShipmentsTabComponent.js.map