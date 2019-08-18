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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ConsoleShipmentPM_1 = require("../../../../../Shipment/EntityPMs/ConsoleShipmentPM");
var ShipmentListService_1 = require("../../../../../Shipment/Services/StandardLists/ShipmentListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_2 = require("../../../../../Shipment/Tools");
var Args_1 = require("../../../../../Shipment/Args");
var HAWBTabComponent = /** @class */ (function () {
    function HAWBTabComponent() {
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.IsEditingEnabled = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SummaryGrossWeight = 0;
        this.SummaryVolumetricWeight = 0;
        this.SummaryChargeableWeight = 0;
        this.isNewEntityRequested = false;
        this.isReloadRequested = false;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
    }
    HAWBTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.Listen();
        this.LoadAllHouses();
    };
    HAWBTabComponent.prototype.RefreshTab = function () {
        this.isNewEntityRequested = false;
    };
    HAWBTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllHouses();
    };
    Object.defineProperty(HAWBTabComponent.prototype, "IsCantConnectTextVisible", {
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    HAWBTabComponent.prototype.LoadAllHouses = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            if (this.myService == null) {
                this.myService = new ShipmentListService_1.ShipmentListService();
            }
            this.ItemsSource1 = [];
            this.ItemsSource2 = [];
            this.LoadItemsSource1();
        }
    };
    HAWBTabComponent.prototype.LoadItemsSource1 = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.Filter1Name = "MasterShipmentDataId";
        filters.Filter1Value = this.EntityPM.Id;
        filters.Filter1Operator = "Equals";
        filters.Filter2Name = "ShipmentLevelCode";
        filters.Filter2Value = "H";
        filters.Filter2Operator = "Equals";
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
                    _this.ItemsSource1 = _this.ItemsSource1.concat(list1);
                    _this.ItemsSource1 = _this.ItemsSource1.concat(list2);
                }
            }
            _this.BuildSummary();
            _this.LoadItemsSource2();
        });
    };
    HAWBTabComponent.prototype.LoadItemsSource2 = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.Filter1Name = "IsCancelled";
        filters.Filter1Value = false;
        filters.Filter1Operator = "Equals";
        filters.Filter2Name = "IsOperationalClosed";
        filters.Filter2Value = false;
        filters.Filter2Operator = "Equals";
        filters.Filter3Name = "DirectionId";
        filters.Filter3Value = this.EntityPM.DirectionId;
        filters.Filter3Operator = "Equals";
        filters.Filter4Name = "TransportModeId";
        filters.Filter4Value = this.EntityPM.TransportModeId;
        filters.Filter4Operator = "Equals";
        filters.Filter5Name = "FromPortId";
        filters.Filter5Value = this.EntityPM.MainCarriageFromPortId;
        filters.Filter5Operator = "Equals";
        filters.Filter6Name = "ToPortId";
        filters.Filter6Value = this.EntityPM.MainCarriageFinalDestinationPortId;
        filters.Filter6Operator = "Equals";
        filters.Filter7Name = "BranchId";
        filters.Filter7Value = this.EntityPM.BranchId;
        filters.Filter7Operator = "Equals";
        filters.Filter8Name = "ShipmentLevelCode";
        filters.Filter8Value = "H";
        filters.Filter8Operator = "Equals";
        // Custom
        filters.addAdditionalFilter("ConnectedToOtherMastersFilter", false, null, null, "Equals", true, false, false, "Boolean");
        if (this.EntityPM.ShipmentTypeId != null) {
            var filterValue = this.EntityPM.ShipmentTypeId;
            switch (this.EntityPM.ShipmentTypeId.toUpperCase()) {
                case "MYGO": {
                    filterValue = "LCLD";
                    break;
                }
                case "MYGI": {
                    filterValue = "LTL";
                    break;
                }
            }
            filters.Filter9Name = "ShipmentTypeId";
            filters.Filter9Value = filterValue;
            filters.Filter9Operator = "Equals";
        }
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
                    _this.ItemsSource2 = _this.ItemsSource2.concat(list1);
                    _this.ItemsSource2 = _this.ItemsSource2.concat(list2);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    HAWBTabComponent.prototype.BuildSummary = function () {
        //var myGrossWeight = 0;
        //var myVolumetricWeight = 0;
        //this.ItemsSource1.forEach(item => {
        //    if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
        //        myGrossWeight += item.GrossWeight;
        //    }
        //    if (!AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
        //        myVolumetricWeight += item.VolumetricWeight;
        //    }
        //});
        //this.SummaryGrossWeight = myGrossWeight;
        //this.SummaryVolumetricWeight = myVolumetricWeight;
        //this.SummaryChargeableWeight = this.SummaryGrossWeight > this.SummaryVolumetricWeight ? this.SummaryGrossWeight : this.SummaryVolumetricWeight;
        this.SummaryGrossWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "GrossWeight");
        this.SummaryChargeableWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "ChargeableWeight");
        this.SummaryVolumetricWeight = Tools_1.ArrayTool.Sum(this.ItemsSource1, "VolumetricWeight");
    };
    HAWBTabComponent.prototype.NewShipment = function () {
        if (!this.isNewEntityRequested) {
            this.isNewEntityRequested = true;
            this.Wizard.SaveClicked();
        }
    };
    HAWBTabComponent.prototype.RunNewShipment = function () {
        var _this = this;
        this.CurrentSession.StopBusyIndicator();
        var windowTitle = "House AWB Wizard";
        var windowArgs = new Args_1.AWBWizardArgs();
        windowArgs.IsNewEntity = true;
        windowArgs.ShipmentLevelCode = "H";
        windowArgs.MasterPM = this.EntityPM;
        windowArgs.IsCreatingHouseFromMaster = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.ReloadEntity(); });
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent");
    };
    HAWBTabComponent.prototype.ViewShipment = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = item.Id;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.ReloadEntity(); });
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    };
    HAWBTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    if (_this.isNewEntityRequested) {
                        _this.isNewEntityRequested = false;
                        _this.RunNewShipment();
                    }
                    if (_this.isReloadRequested) {
                        _this.ReloadEntity();
                    }
                }
                _this.StopListenFlags();
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    if (_this.isNewEntityRequested) {
                        _this.isNewEntityRequested = false;
                        _this.RunNewShipment();
                    }
                    if (_this.isReloadRequested) {
                        _this.isReloadRequested = false;
                        _this.LoadAllHouses();
                    }
                }
                _this.StopListenFlags();
            });
        }
    };
    HAWBTabComponent.prototype.StopListenFlags = function () {
        this.isReloadRequested = false;
        this.isNewEntityRequested = false;
    };
    HAWBTabComponent.prototype.ReloadEntity = function () {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    };
    HAWBTabComponent.prototype.Save = function () {
        this.isReloadRequested = true;
        this.Wizard.SaveClicked();
    };
    HAWBTabComponent.prototype.ngOnDestroy = function () {
    };
    HAWBTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HAWBTabComponent',
            templateUrl: './HAWBTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], HAWBTabComponent);
    return HAWBTabComponent;
}());
exports.HAWBTabComponent = HAWBTabComponent;
var HAWBItem = /** @class */ (function () {
    function HAWBItem(item, fatherComponent, isConnected) {
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsMatched = false;
        if (item != null) {
            this.Id = item.Id;
            this.ShipmentNumber = item.ShipmentNumber;
            this.CustomerName = item.CustomerName;
            this.House = item.House;
            this.GrossWeight = item.GrossWeight;
            this.VolumetricWeight = item.VolumetricWeight;
            this.ChargeableWeight = item.ChargeableWeight;
            this.FHLStatusName = item.FHLStatusName;
            this.FNAReason = item.FNAReason;
            this.isChecked = isConnected;
            this.SetIsMatched();
        }
    }
    HAWBItem.prototype.SetIsMatched = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.FromPortId == this.fatherComponent.EntityPM.MainCarriageFromPortId && this.item.ToPortId == this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId && this.item.BranchId == this.fatherComponent.EntityPM.BranchId) {
            this.IsMatched = true;
        }
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
//# sourceMappingURL=HAWBTabComponent.js.map