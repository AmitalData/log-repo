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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ShipmentPickUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPickUpPM");
var ShipmentDeliveryPM_1 = require("../../../../Shipment/EntityPMs/ShipmentDeliveryPM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var RoutingsTabComponent = /** @class */ (function (_super) {
    __extends(RoutingsTabComponent, _super);
    function RoutingsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.ShipmentLevelCode = null;
        _this.TransportModeId = null;
        _this.IsInlandDomestic = false;
        _this.CardLOVDependencyProperty1 = null;
        _this.ItemsSource = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = true;
        _this.IsAddPreCarriageDisabled = false;
        _this.IsAddOnCarriageDisabled = false;
        // Warehouse Leg
        _this.WarehouseLegTerminalName = "";
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        _this.ShipmentLevelCode = _this.EntityPM.ShipmentLevelCode;
        _this.TransportModeId = _this.EntityPM.TransportModeId;
        _this.SetUIProperties();
        _this.Listen();
        return _this;
    }
    RoutingsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "AWBWizardClosed") {
                    _this.UpdateScreen();
                }
                if (s == "RefreshWareHouseLeg") {
                    _this.GetWarehouseAddress();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.UpdateScreen();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.IsInlandDomestic = Tools_2.ShipmentTool.IsInlandDomestic(_this.EntityPM);
                    _this.UpdateScreen();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHRT" || tabCode == "JHRT") {
                    _this.UpdateScreen();
                }
            });
        }
    };
    RoutingsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    RoutingsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = Tools_2.ShipmentTool.IsInlandDomestic(this.EntityPM);
            if (this.IsInlandDomestic) {
                this.CardLOVDependencyProperty1 = (this.ShipmentLevelCode == "C") ? "AG" : "CS";
                this.InitializePartners();
            }
            this.UpdateScreen();
            this.GetWarehouseAddress();
        }
    };
    RoutingsTabComponent.prototype.UpdateScreen = function () {
        this.SetUIProperties();
        if (this.IsInlandDomestic) {
        }
        else {
            this.BuildItemsCollection();
        }
    };
    RoutingsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        if (this.IsInlandDomestic) {
            this.SetUIProperties_InlandDomestic();
        }
    };
    RoutingsTabComponent.prototype.SetAddButtonsIsDisabled = function () {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.IsAddPreCarriageDisabled = true;
            this.IsAddOnCarriageDisabled = true;
        }
        else {
            if (this.EntityPM.PreCarriageFromPortId == null && this.EntityPM.PreCarriageToPortId == null) {
                this.IsAddPreCarriageDisabled = false;
            }
            else {
                this.IsAddPreCarriageDisabled = true;
            }
            if (this.EntityPM.OnCarriageFromPortId == null && this.EntityPM.OnCarriageToPortId == null) {
                this.IsAddOnCarriageDisabled = false;
            }
            else {
                this.IsAddOnCarriageDisabled = true;
            }
        }
    };
    RoutingsTabComponent.prototype.BuildItemsCollection = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.EntityPM.ShipmentPickUps.length == 0) {
            var newPickup = new ShipmentPickUpPM_1.ShipmentPickUpPM(null);
            newPickup.ShipmentId = this.EntityPM.Id;
            newPickup.ShipmentNumber = this.EntityPM.ShipmentNumber;
            newPickup.Tenant = this.EntityPM.Tenant;
            this.ItemsSource.push(new RoutingItem(newPickup, "Pick Up", this));
        }
        else {
            var myShipmentPickUps = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });
            myShipmentPickUps.forEach(function (item) {
                _this.ItemsSource.push(new RoutingItem(item, "Pick Up", _this));
            });
        }
        // WarehouseLeg_Pickups
        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.IsInlandDomestic) {
                }
                else {
                    this.ItemsSource.push(new RoutingItem(this.EntityPM, "WarehouseLeg_Pickups", this));
                }
            }
        }
        if (this.EntityPM.PreCarriageFromPortId != null && this.EntityPM.PreCarriageToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Pre Carriage", this));
        }
        this.ItemsSource.push(new RoutingItem(this.EntityPM, "Main Carriage", this));
        if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment1ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment1", this));
        }
        if (this.EntityPM.Transshipment2FromPortId != null && this.EntityPM.Transshipment2ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment2", this));
        }
        if (this.EntityPM.Transshipment3FromPortId != null && this.EntityPM.Transshipment3ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment3", this));
        }
        if (this.EntityPM.OnCarriageFromPortId != null && this.EntityPM.OnCarriageToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "On Carriage", this));
        }
        // WarehouseLeg_Deliveries
        if (this.EntityPM.DirectionId == "I") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.ItemsSource.push(new RoutingItem(this.EntityPM, "WarehouseLeg", this));
            }
        }
        var allDeliveries = this.EntityPM.ShipmentDeliveries.filter(function (f) { return f.PickUpDeliveryTypeCode == "DELV"; }).sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });
        var allEmptyContainerReturns = this.EntityPM.ShipmentDeliveries.filter(function (f) { return f.PickUpDeliveryTypeCode == "EMPT"; }).sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });
        if (allDeliveries.length == 0) {
            var newDelivery = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
            newDelivery.ShipmentId = this.EntityPM.Id;
            newDelivery.ShipmentNumber = this.EntityPM.ShipmentNumber;
            newDelivery.Tenant = this.EntityPM.Tenant;
            this.ItemsSource.push(new RoutingItem(newDelivery, "Delivery", this));
        }
        else {
            allDeliveries.forEach(function (item) {
                _this.ItemsSource.push(new RoutingItem(item, "Delivery", _this));
            });
        }
        allEmptyContainerReturns.forEach(function (item) {
            _this.ItemsSource.push(new RoutingItem(item, "EmptyCR", _this));
        });
        this.SetAddButtonsIsDisabled();
    };
    RoutingsTabComponent.prototype.AddLeg = function (myLegType) {
        var _this = this;
        var isNewEntity = true;
        var windowTitle = "Add " + myLegType;
        switch (myLegType) {
            case "Pick Up": {
                var myPickUpIndex = 1;
                if (this.EntityPM.ShipmentPickUpIndex) {
                    myPickUpIndex = this.EntityPM.ShipmentPickUpIndex + 1;
                }
                var newPickupPM = new ShipmentPickUpPM_1.ShipmentPickUpPM(null);
                newPickupPM.FullResponsibility = true;
                newPickupPM.Tenant = this.EntityPM.Tenant;
                newPickupPM.ShipmentId = this.EntityPM.Id;
                newPickupPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                newPickupPM.PickUpDeliveryNumber = this.EntityPM.ShipmentNumber + "/" + myPickUpIndex;
                newPickupPM.PickUpDeliveryTypeCode = "PICK";
                newPickupPM.PickUpDeliveryFromTypeCode = "PART";
                newPickupPM.PickUpDeliveryToTypeCode = "PORT";
                newPickupPM.TransportModeCode = "BYTR";
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddPickup");
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: newPickupPM, IsNewEntity: true };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPickupComponent');
                logitudeWindow.WindowClosed.subscribe(function (s) {
                    _this.BuildItemsCollection();
                });
                break;
            }
            case "Delivery": {
                var myDeliveryIndex = 1;
                if (this.EntityPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.EntityPM.ShipmentDeliveryIndex + 1;
                }
                var newDeliveryPM = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
                newDeliveryPM.FullResponsibility = true;
                newDeliveryPM.Tenant = this.EntityPM.Tenant;
                newDeliveryPM.ShipmentId = this.EntityPM.Id;
                newDeliveryPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                newDeliveryPM.PickUpDeliveryNumber = this.EntityPM.ShipmentNumber + "/" + myDeliveryIndex;
                newDeliveryPM.PickUpDeliveryTypeCode = "DELV";
                newDeliveryPM.TransportModeCode = "BYTR";
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: newDeliveryPM, IsNewEntity: true };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
                logitudeWindow.WindowClosed.subscribe(function (s) {
                    _this.BuildItemsCollection();
                });
                break;
            }
            case "Pre Carriage":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddPreCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPreCarriageComponent');
                    break;
                }
            case "On Carriage":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddOnCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Height = 580;
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditOnCarriageComponent');
                    break;
                }
            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 500;
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddWarehouseLeg");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this, LegType: myLegType, IsNewLeg: true };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditWarehouseLegComponent');
                    break;
                }
            default: {
                if (this.EntityPM.ShipmentLevelCode == "H" && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditMainCarriagePorts");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditHouseRoutingComponent');
                }
                else {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.IsFillScreen = true;
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddEditMainCarriageLegs");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditMainCarriageComponent');
                }
                break;
            }
        }
    };
    RoutingsTabComponent.prototype.EditLeg = function (myRoutingItem) {
        var _this = this;
        var myLegType = myRoutingItem.LegType;
        switch (myLegType) {
            case "Pick Up": {
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditPickup");
                if (!Tools_1.AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Pickup, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPickupComponent');
                logitudeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.BuildItemsCollection();
                    }
                });
                break;
            }
            case "Delivery": {
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                if (!Tools_1.AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Delivery, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
                logitudeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.BuildItemsCollection();
                    }
                });
                break;
            }
            case "EmptyCR": {
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                if (!Tools_1.AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Delivery, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
                logitudeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.BuildItemsCollection();
                    }
                });
                break;
            }
            case "Pre Carriage":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditPreCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPreCarriageComponent');
                    break;
                }
            case "On Carriage":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditOnCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Height = 580;
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditOnCarriageComponent');
                    break;
                }
            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 500;
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditWarehouseLeg");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this, LegType: myLegType };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditWarehouseLegComponent');
                    break;
                }
            default: {
                if (this.EntityPM.ShipmentLevelCode == "H" && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditMainCarriagePorts");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditHouseRoutingComponent');
                }
                else {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.IsFillScreen = true;
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddEditMainCarriageLegs");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this };
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditMainCarriageComponent');
                }
                break;
            }
        }
    };
    RoutingsTabComponent.prototype.DeleteLeg = function (myRoutingItem) {
        var _this = this;
        if (myRoutingItem) {
            var message = null;
            var myLegType = myRoutingItem.LegType;
            switch (myLegType) {
                case "Pick Up": {
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPickup");
                    break;
                }
                case "Pre Carriage": {
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPreCarriage");
                    break;
                }
                case "On Carriage": {
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisOnCarriage");
                    break;
                }
                case "Delivery": {
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
                    break;
                }
                case "WarehouseLeg":
                case "WarehouseLeg_Pickups": {
                    message = "Delete Warehouse \ Terminal?";
                    break;
                }
            }
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    switch (myLegType) {
                        case "Pick Up": {
                            var followups = _this.EntityPM.FollowUps.filter(function (f) { return f.LegType != null; });
                            followups = followups.filter(function (f) { return f.LegType.indexOf(myRoutingItem.Pickup.PickUpDeliveryNumber) > -1; });
                            if (followups.length > 0) {
                                followups.forEach(function (item) {
                                    _this.EntityPM.RemoveShipmentFollowUp(item);
                                });
                                _this.CurrentSession.FireEvent("FollowupsChanged");
                            }
                            _this.EntityPM.RemovePickUp(myRoutingItem.Pickup);
                            break;
                        }
                        case "Pre Carriage": {
                            Tools_2.RoutingHelper.RemovePreCarriageLeg(_this.EntityPM);
                            _this.SetAddButtonsIsDisabled();
                            break;
                        }
                        case "On Carriage": {
                            Tools_2.RoutingHelper.RemoveOnCarriageLeg(_this.EntityPM);
                            _this.SetAddButtonsIsDisabled();
                            break;
                        }
                        case "Delivery": {
                            var followups = _this.EntityPM.FollowUps.filter(function (f) { return f.LegType != null; });
                            followups = followups.filter(function (f) { return f.LegType.indexOf(myRoutingItem.Delivery.PickUpDeliveryNumber) > -1; });
                            if (followups.length > 0) {
                                followups.forEach(function (item) {
                                    _this.EntityPM.RemoveShipmentFollowUp(item);
                                });
                                _this.CurrentSession.FireEvent("FollowupsChanged");
                            }
                            _this.EntityPM.RemoveDelivery(myRoutingItem.Delivery);
                            break;
                        }
                        case "WarehouseLeg":
                        case "WarehouseLeg_Pickups": {
                            _this.EntityPM.WarehouseLegWarehouseId = null;
                            _this.EntityPM.WarehouseLegAddressId = null;
                            _this.EntityPM.WarehouseLegReference = null;
                            _this.EntityPM.WarehouseLegTerminalCode = null;
                            _this.EntityPM.WarehouseLegLastFreeDate = null;
                            _this.EntityPM.TerminalAvailable = null;
                            _this.EntityPM.WarehouseLegCutOffDate = null;
                            _this.EntityPM.WarehouseLegRemarks = null;
                            _this.EntityPM.WarehouseLegExpectedEntryDate = null;
                            _this.EntityPM.WarehouseLegExpectedReleaseDate = null;
                            _this.EntityPM.WarehouseLegActualEntryDate = null;
                            _this.EntityPM.WarehouseLegActualReleaseDate = null;
                            _this.EntityPM.WarehouseLegVGMCutOffDate = null;
                            var followups = _this.EntityPM.FollowUps.filter(function (f) { return f.LegType != null; });
                            followups = followups.filter(function (f) { return f.LegType.indexOf("WarehouseLeg") > -1; });
                            if (followups.length > 0) {
                                followups.forEach(function (item) {
                                    _this.EntityPM.RemoveShipmentFollowUp(item);
                                });
                                _this.CurrentSession.FireEvent("FollowupsChanged");
                            }
                            break;
                        }
                    }
                    _this.BuildItemsCollection();
                }
            });
        }
    };
    RoutingsTabComponent.prototype.InitializePartners = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.LoadFromAddress();
        this.LoadToAddress();
    };
    RoutingsTabComponent.prototype.SetUIProperties_InlandDomestic = function () {
        if (this.IsInlandDomestic) {
            this.UIProperties.SetEnabled("MainCarriageFromPartnerId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageFromAddressId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageToPartnerId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageToAddressId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("Driver", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("TruckNumber", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("TrailerNumber", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("Master", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
            this.SetUIProperties_ValidateActualDates();
        }
    };
    RoutingsTabComponent.prototype.SetUIProperties_ValidateActualDates = function () {
        this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    };
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageFromPartnerId", {
        get: function () { return this.EntityPM.MainCarriageFromPartnerId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPartnerId != value) {
                this.EntityPM.MainCarriageFromPartnerId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.MainCarriageFromAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.MainCarriageFromAddressId = list.MainAddressId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageFromAddressId", {
        get: function () { return this.EntityPM.MainCarriageFromAddressId; },
        set: function (value) {
            if (this.EntityPM.MainCarriageFromAddressId != value) {
                this.EntityPM.MainCarriageFromAddressId = value;
                this.LoadFromAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageToPartnerId", {
        get: function () { return this.EntityPM.MainCarriageToPartnerId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageToPartnerId != value) {
                this.EntityPM.MainCarriageToPartnerId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.MainCarriageToAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.MainCarriageToAddressId = list.MainAddressId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageToAddressId", {
        get: function () { return this.EntityPM.MainCarriageToAddressId; },
        set: function (value) {
            if (this.EntityPM.MainCarriageToAddressId != value) {
                this.EntityPM.MainCarriageToAddressId = value;
                this.LoadToAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    RoutingsTabComponent.prototype.LoadToAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageToAddressId)) {
            this.ToAddressList = null;
            if (this.EntityPM.ToCountryId != null) {
                this.EntityPM.ToCountryId = null;
            }
            if (this.EntityPM.ToCountryIsEC != false) {
                this.EntityPM.ToCountryIsEC = false;
            }
        }
        else {
            this.myAddressListService.getSingle(this.MainCarriageToAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.ToAddressList = list;
                        if (_this.EntityPM.ToCountryId != list.CountryId) {
                            _this.EntityPM.ToCountryId = list.CountryId;
                        }
                        if (_this.EntityPM.ToCountryIsEC != list.CountryEC) {
                            _this.EntityPM.ToCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    };
    RoutingsTabComponent.prototype.LoadFromAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromAddressId)) {
            this.FromAddressList = null;
            if (this.EntityPM.FromCountryId != null) {
                this.EntityPM.FromCountryId = null;
            }
            if (this.EntityPM.FromCountryIsEC != false) {
                this.EntityPM.FromCountryIsEC = false;
            }
        }
        else {
            this.myAddressListService.getSingle(this.MainCarriageFromAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.FromAddressList = list;
                        if (_this.EntityPM.FromCountryId != list.CountryId) {
                            _this.EntityPM.FromCountryId = list.CountryId;
                        }
                        if (_this.EntityPM.FromCountryIsEC != list.CountryEC) {
                            _this.EntityPM.FromCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    };
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierId != value) {
                this.EntityPM.MainCarriageCarrierId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "Driver", {
        get: function () { return this.EntityPM.Driver; },
        set: function (value) {
            if (this.EntityPM.Driver != value) {
                this.EntityPM.Driver = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "TruckNumber", {
        get: function () { return this.EntityPM.TruckNumber; },
        set: function (value) {
            if (this.EntityPM.TruckNumber != value) {
                this.EntityPM.TruckNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "TrailerNumber", {
        get: function () { return this.EntityPM.TrailerNumber; },
        set: function (value) {
            if (this.EntityPM.TrailerNumber != value) {
                this.EntityPM.TrailerNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (value) {
            if (this.EntityPM.Master != value) {
                this.EntityPM.Master = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageETD", {
        get: function () { return this.EntityPM.MainCarriageETD; },
        set: function (value) {
            if (this.EntityPM.MainCarriageETD != value) {
                this.EntityPM.MainCarriageETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageETA", {
        get: function () { return this.EntityPM.MainCarriageETA; },
        set: function (value) {
            if (this.EntityPM.MainCarriageETA != value) {
                this.EntityPM.MainCarriageETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageATD", {
        get: function () { return this.EntityPM.MainCarriageATD; },
        set: function (value) {
            if (this.EntityPM.MainCarriageATD != value) {
                this.EntityPM.MainCarriageATD = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsTabComponent.prototype, "MainCarriageATA", {
        get: function () { return this.EntityPM.MainCarriageATA; },
        set: function (value) {
            if (this.EntityPM.MainCarriageATA != value) {
                this.EntityPM.MainCarriageATA = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    RoutingsTabComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "MainCarriageETD": {
                this.MainCarriageATD = Tools_1.DateTool.GetDateParts(this.MainCarriageETD).DateObject;
                break;
            }
            case "MainCarriageETA": {
                this.MainCarriageATA = Tools_1.DateTool.GetDateParts(this.MainCarriageETA).DateObject;
                break;
            }
        }
    };
    RoutingsTabComponent.prototype.EditAddressClicked = function (myCode) {
        var _this = this;
        var myAddressId = myCode == "F" ? this.MainCarriageFromAddressId : this.MainCarriageToAddressId;
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: this.CardLOVDependencyProperty1 };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (myCode == "F") {
                        _this.LoadFromAddress();
                    }
                    else {
                        _this.LoadToAddress();
                    }
                }
            });
        }
    };
    RoutingsTabComponent.prototype.AddAddressClicked = function (myCode) {
        var _this = this;
        var entityPM = new AddressPM_1.AddressPM();
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = myCode == "F" ? this.MainCarriageFromPartnerId : this.MainCarriageToPartnerId;
        if (entityPM != null) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: this.CardLOVDependencyProperty1 };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (myCode == "F") {
                        _this.MainCarriageFromAddressId = entityPM.Id;
                    }
                    else {
                        _this.MainCarriageToAddressId = entityPM.Id;
                    }
                }
            });
        }
    };
    Object.defineProperty(RoutingsTabComponent.prototype, "WarehouseAddressList", {
        get: function () { return this.myWarehouseAddressList; },
        set: function (newValue) {
            this.myWarehouseAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    RoutingsTabComponent.prototype.GetWarehouseAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegAddressId)) {
            this.WarehouseLegTerminalName = this.EntityPM.WarehouseLegTerminalName;
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.EntityPM.WarehouseLegAddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.WarehouseAddressList = myResponse.Result;
                    }
                }
            });
        }
    };
    RoutingsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './RoutingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], RoutingsTabComponent);
    return RoutingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.RoutingsTabComponent = RoutingsTabComponent;
var RoutingItem = /** @class */ (function (_super) {
    __extends(RoutingItem, _super);
    function RoutingItem(entity, type, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.IsWarehouseLeg = false;
        _this.IsCarrierTextVisible = false;
        _this.IsCarrierSiteVisible = false;
        _this.IsVesselVisible = false;
        _this.IsBookingConfirmationVisible = false;
        _this.IsSetActualDepartureVisible = false;
        _this.IsSetActualArrivalVisible = false;
        _this.HasContainers = false;
        _this.AllContainers = null;
        _this.DisplayContainers = null;
        if (entity instanceof ShipmentPickUpPM_1.ShipmentPickUpPM) {
            _this.Pickup = entity;
            _this.EntityPM = fatherComponent.EntityPM;
            _this.ObjectTableName = "ShipmentPickUpDelivery";
            _this.PickUpDeliveryNumber = _this.Pickup.PickUpDeliveryNumber;
            _this.FollowupLegTypeDeparture = type + 'Departure' + _this.PickUpDeliveryNumber;
            _this.FollowupLegTypeArrival = type + 'Arrival' + _this.PickUpDeliveryNumber;
        }
        else if (entity instanceof ShipmentDeliveryPM_1.ShipmentDeliveryPM) {
            _this.Delivery = entity;
            _this.EntityPM = fatherComponent.EntityPM;
            _this.ObjectTableName = "ShipmentPickUpDelivery";
            _this.PickUpDeliveryNumber = _this.Delivery.PickUpDeliveryNumber;
            _this.FollowupLegTypeDeparture = type + 'Departure' + _this.PickUpDeliveryNumber;
            _this.FollowupLegTypeArrival = type + 'Arrival' + _this.PickUpDeliveryNumber;
        }
        else {
            _this.EntityPM = entity;
            _this.ObjectTableName = fatherComponent.ObjectTableName;
            if (type == "WarehouseLeg" || type == "WarehouseLeg_Pickups") {
                _this.IsWarehouseLeg = true;
                _this.FollowupLegTypeDeparture = 'WarehouseLegEntry';
                _this.FollowupLegTypeArrival = 'WarehouseLegRelease';
            }
            else {
                _this.FollowupLegTypeDeparture = type + 'Departure';
                _this.FollowupLegTypeArrival = type + 'Arrival';
            }
        }
        _this.LegType = type;
        _this.SetLegAppearance();
        _this.GetLegName();
        _this.GetImageSource();
        _this.GetFromCountryData();
        _this.GetToCountryData();
        _this.GetCarrierData();
        _this.GetDates();
        _this.GetContainersNumbers();
        return _this;
    }
    RoutingItem.prototype.SetLegAppearance = function () {
        this.LegHeight = 100;
        this.IsMainLeg = false;
        this.IsLegExists = true;
        this.NoLegTextCode = null;
        this.IsAddButtonVisible = false;
        this.IsEditButtonVisible = false;
        this.IsDeleteButtonVisible = false;
        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup == null || this.EntityPM.ShipmentPickUps.indexOf(this.Pickup) == -1) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoPickup";
                }
                break;
            }
            case "WarehouseLeg_Pickups": {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegWarehouseId)) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoWarehouseTerminal";
                }
                else {
                    this.LegHeight = 130;
                }
                break;
            }
            case "WarehouseLeg": {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegWarehouseId)) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoWarehouseTerminal";
                }
                else {
                    this.LegHeight = 130;
                }
                break;
            }
            case "Delivery": {
                if (this.Delivery == null || this.EntityPM.ShipmentDeliveries.indexOf(this.Delivery) == -1) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoDelivery";
                }
                break;
            }
        }
        if (this.IsLegExists) {
            this.IsEditButtonVisible = true;
            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        this.IsMainLeg = true;
                        break;
                    }
                case "Delivery": {
                    var isDeleteButtonVisible = true;
                    if (this.Delivery) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.Delivery.ConnectedPackageId)) {
                            isDeleteButtonVisible = false;
                        }
                        if (this.Delivery.AllConnectedPackagesId) {
                            if (this.Delivery.AllConnectedPackagesId.length > 0) {
                                isDeleteButtonVisible = false;
                            }
                        }
                    }
                    this.IsDeleteButtonVisible = isDeleteButtonVisible;
                    break;
                }
                case "Pick Up":
                case "On Carriage":
                case "Pre Carriage":
                case "WarehouseLeg":
                case "WarehouseLeg_Pickups":
                    {
                        this.IsDeleteButtonVisible = true;
                        break;
                    }
            }
        }
    };
    RoutingItem.prototype.GetLegName = function () {
        var myTextCode;
        var myLegTransportModeId;
        var myExtention = "";
        switch (this.LegType) {
            case "Pick Up": {
                myTextCode = "Shipment.O.Routings.Pickup";
                myLegTransportModeId = "I";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Pickup.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Pickup.PickUpDeliveryNumber;
                }
                break;
            }
            case "WarehouseLeg_Pickups": {
                myTextCode = "Shipment.O.Routings.WarehouseLeg";
                break;
            }
            case "Pre Carriage": {
                myTextCode = "Shipment.O.Routings.PreCarriage";
                myLegTransportModeId = this.EntityPM.PreCarriageTransportModeId;
                break;
            }
            case "Main Carriage": {
                myTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }
            case "Transshipment1": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg2" : "Shipment.O.Routings.Transshipment1";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }
            case "Transshipment2": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg3" : "Shipment.O.Routings.Transshipment2";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }
            case "Transshipment3": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg4" : "Shipment.O.Routings.Transshipment3";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }
            case "On Carriage": {
                myTextCode = "Shipment.O.Routings.OnCarriage";
                myLegTransportModeId = this.EntityPM.OnCarriageTransportModeId;
                break;
            }
            case "WarehouseLeg": {
                myTextCode = "Shipment.O.Routings.WarehouseLeg";
                break;
            }
            case "Delivery": {
                myTextCode = "Shipment.O.Routings.Delivery";
                myLegTransportModeId = "I";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Delivery.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Delivery.PickUpDeliveryNumber;
                }
                break;
            }
            case "EmptyCR": {
                myTextCode = "Shipment.O.Routings.EmptyCR";
                myLegTransportModeId = "I";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Delivery.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Delivery.PickUpDeliveryNumber;
                }
                break;
            }
        }
        this.LegName = TextCodeTranslator_1.TextCodeTranslator.Translate(myTextCode) + myExtention;
        this.LegTransportModeId = myLegTransportModeId;
    };
    RoutingItem.prototype.GetImageSource = function () {
        switch (this.LegTransportModeId) {
            case "A": {
                this.ImageSource = 'Images/Airline.png';
                break;
            }
            case "O": {
                this.ImageSource = 'Images/Vessel.png';
                break;
            }
            case "I": {
                this.ImageSource = 'Images/Trucker.png';
                break;
            }
        }
    };
    RoutingItem.prototype.GetFromCountryData = function () {
        var myCountryCode = "";
        var myCountryName = "";
        var myTextPart1 = "";
        var myTextPart2 = "";
        var myTextParts = "";
        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup != null) {
                    if (this.Pickup.FromPortId != null) {
                        myCountryCode = this.Pickup.FromPortCountryCode;
                        myCountryName = this.Pickup.FromPortCountryName;
                    }
                    else {
                        myCountryCode = this.Pickup.FromAddressCountryCode;
                        myCountryName = this.Pickup.FromAddressCountryName;
                    }
                    switch (this.Pickup.PickUpDeliveryFromTypeCode) {
                        case "PORT": {
                            myTextPart1 = this.Pickup.FromPortCode;
                            myTextPart2 = this.Pickup.FromPortName;
                            break;
                        }
                        case "PART": {
                            if (this.Pickup.FromAddressId != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }
                            break;
                        }
                        default: {
                            if (this.Pickup.FromAddressCity != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }
                            break;
                        }
                    }
                }
                break;
            }
            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageFromPortCode;
                myTextPart2 = this.EntityPM.PreCarriageFromPortName;
                break;
            }
            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageFromPortCode;
                myTextPart2 = this.EntityPM.MainCarriageFromPortName;
                break;
            }
            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment1FromPortName;
                break;
            }
            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment2FromPortName;
                break;
            }
            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment3FromPortName;
                break;
            }
            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageFromPortCode;
                myTextPart2 = this.EntityPM.OnCarriageFromPortName;
                break;
            }
            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {
                    if (this.Delivery.FromPortId != null) {
                        myCountryCode = this.Delivery.FromPortCountryCode;
                        myCountryName = this.Delivery.FromPortCountryName;
                    }
                    else {
                        myCountryCode = this.Delivery.FromAddressCountryCode;
                        myCountryName = this.Delivery.FromAddressCountryName;
                    }
                    switch (this.Delivery.PickUpDeliveryFromTypeCode) {
                        case "PORT": {
                            myTextPart1 = this.Delivery.FromPortCode;
                            myTextPart2 = this.Delivery.FromPortName;
                            break;
                        }
                        case "PART": {
                            if (this.Delivery.FromAddressId != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }
                            break;
                        }
                        default: {
                            if (this.Delivery.FromAddressCity != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }
                            break;
                        }
                    }
                }
                break;
            }
        }
        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }
        this.FromCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.FromCountryName = myCountryName == null ? "" : myCountryName;
        this.FromCountryText = myTextParts;
    };
    RoutingItem.prototype.GetToCountryData = function () {
        var myCountryCode = "";
        var myCountryName = "";
        var myTextPart1 = "";
        var myTextPart2 = "";
        var myTextParts = "";
        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup != null) {
                    if (this.Pickup.ToPortId != null) {
                        myCountryCode = this.Pickup.ToPortCountryCode;
                        myCountryName = this.Pickup.ToPortCountryName;
                    }
                    else {
                        myCountryCode = this.Pickup.ToAddressCountryCode;
                        myCountryName = this.Pickup.ToAddressCountryName;
                    }
                    switch (this.Pickup.PickUpDeliveryToTypeCode) {
                        case "PORT": {
                            myTextPart1 = this.Pickup.ToPortCode;
                            myTextPart2 = this.Pickup.ToPortName;
                            break;
                        }
                        case "PART": {
                            if (this.Pickup.ToAddressId != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }
                            break;
                        }
                        default: {
                            if (this.Pickup.ToAddressCity != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }
                            break;
                        }
                    }
                }
                break;
            }
            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageToPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageToPortCode;
                myTextPart2 = this.EntityPM.PreCarriageToPortName;
                break;
            }
            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageToPortCode;
                myTextPart2 = this.EntityPM.MainCarriageToPortName;
                break;
            }
            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment1ToPortName;
                break;
            }
            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment2ToPortName;
                break;
            }
            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment3ToPortName;
                break;
            }
            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageToPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageToPortCode;
                myTextPart2 = this.EntityPM.OnCarriageToPortName;
                break;
            }
            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {
                    if (this.Delivery.ToPortId != null) {
                        myCountryCode = this.Delivery.ToPortCountryCode;
                        myCountryName = this.Delivery.ToPortCountryName;
                    }
                    else {
                        myCountryCode = this.Delivery.ToAddressCountryCode;
                        myCountryName = this.Delivery.ToAddressCountryName;
                    }
                    switch (this.Delivery.PickUpDeliveryToTypeCode) {
                        case "PORT": {
                            myTextPart1 = this.Delivery.ToPortCode;
                            myTextPart2 = this.Delivery.ToPortName;
                            break;
                        }
                        case "PART": {
                            if (this.Delivery.ToAddressId != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }
                            break;
                        }
                        default: {
                            if (this.Delivery.ToAddressCity != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }
                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }
                            break;
                        }
                    }
                }
                break;
            }
        }
        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }
        this.ToCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.ToCountryName = myCountryName == null ? "" : myCountryName;
        this.ToCountryText = myTextParts;
    };
    RoutingItem.prototype.GetCarrierData = function () {
        var myCarrierCode = "";
        var myCarrierName = "";
        var myCarrierSite = "";
        var myCarrierNumber = "";
        var myVesselName = "";
        var isBookingnVisible = false;
        var isVesselVisible = false;
        var isCarrierSiteVisible = false;
        var isCarrierTextVisible = false;
        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup != null) {
                    myCarrierCode = this.Pickup.CarrierCode;
                    myCarrierName = this.Pickup.CarrierName;
                    myCarrierSite = this.Pickup.CarrierWebSite;
                    myCarrierNumber = this.Pickup.CarrierNumber;
                }
                break;
            }
            case "Pre Carriage": {
                myCarrierCode = this.EntityPM.PreCarriageCarrierCode;
                myCarrierName = this.EntityPM.PreCarriageCarrierName;
                myCarrierSite = this.EntityPM.PreCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.PreCarriageCarrierNumber;
                myVesselName = this.EntityPM.PreCarriageVesselName;
                if (this.EntityPM.PreCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "Main Carriage": {
                myCarrierCode = this.EntityPM.MainCarriageCarrierCode;
                myCarrierName = this.EntityPM.MainCarriageCarrierName;
                myCarrierSite = this.EntityPM.MainCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.MainCarriageCarrierNumber;
                myVesselName = this.EntityPM.MainCarriageVesselName;
                if (this.EntityPM.BookingConfirmationNumber != null) {
                    isBookingnVisible = true;
                }
                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "Transshipment1": {
                myCarrierCode = this.EntityPM.Transshipment1CarrierCode;
                myCarrierName = this.EntityPM.Transshipment1CarrierName;
                myCarrierSite = this.EntityPM.Transshipment1CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment1CarrierNumber;
                myVesselName = this.EntityPM.Transshipment1VesselName;
                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "Transshipment2": {
                myCarrierCode = this.EntityPM.Transshipment2CarrierCode;
                myCarrierName = this.EntityPM.Transshipment2CarrierName;
                myCarrierSite = this.EntityPM.Transshipment2CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment2CarrierNumber;
                myVesselName = this.EntityPM.Transshipment2VesselName;
                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "Transshipment3": {
                myCarrierCode = this.EntityPM.Transshipment3CarrierCode;
                myCarrierName = this.EntityPM.Transshipment3CarrierName;
                myCarrierSite = this.EntityPM.Transshipment3CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment3CarrierNumber;
                myVesselName = this.EntityPM.Transshipment3VesselName;
                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "On Carriage": {
                myCarrierCode = this.EntityPM.OnCarriageCarrierCode;
                myCarrierName = this.EntityPM.OnCarriageCarrierName;
                myCarrierSite = this.EntityPM.OnCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.OnCarriageCarrierNumber;
                myVesselName = this.EntityPM.OnCarriageVesselName;
                if (this.EntityPM.OnCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }
                break;
            }
            case "Delivery":
            case "EmptyCR":
                {
                    if (this.Delivery != null) {
                        myCarrierCode = this.Delivery.CarrierCode;
                        myCarrierName = this.Delivery.CarrierName;
                        myCarrierSite = this.Delivery.CarrierWebSite;
                        myCarrierNumber = this.Delivery.CarrierNumber;
                    }
                    break;
                }
        }
        // remove undefined text
        if (Tools_1.AppTool.IsNullOrEmpty(myCarrierCode)) {
            myCarrierCode = "";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myCarrierName)) {
            myCarrierName = "";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myCarrierNumber)) {
            myCarrierNumber = "";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myCarrierSite)) {
            myCarrierSite = "";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myVesselName)) {
            myVesselName = "";
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myCarrierCode) || !Tools_1.AppTool.IsNullOrEmpty(myCarrierName)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myCarrierSite)) {
                isCarrierSiteVisible = true;
                if (myCarrierSite.indexOf('http://') == -1) {
                    myCarrierSite = "http://" + myCarrierSite;
                }
            }
            else {
                isCarrierTextVisible = true;
            }
        }
        this.CarrierSite = myCarrierSite;
        this.CarrierText = myCarrierCode + " " + myCarrierName;
        this.CarrierNumber = myCarrierNumber;
        this.VesselName = myVesselName;
        this.IsCarrierSiteVisible = isCarrierSiteVisible;
        this.IsCarrierTextVisible = isCarrierTextVisible;
        this.IsVesselVisible = isVesselVisible;
        this.IsBookingConfirmationVisible = isBookingnVisible;
    };
    RoutingItem.prototype.GetDates = function () {
        var myETD;
        var myATD;
        var myETA;
        var myATA;
        var myDepartureDate;
        var myArrivalDate;
        var myDepartureColor = Tools_1.FontTool.Orange;
        var myArrivalColor = Tools_1.FontTool.Orange;
        this.IsSetActualDepartureVisible = false;
        this.IsSetActualArrivalVisible = false;
        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup != null) {
                    myETD = this.Pickup.ETD;
                    myATD = this.Pickup.ATD;
                    myETA = this.Pickup.ETA;
                    myATA = this.Pickup.ATA;
                }
                break;
            }
            case "Pre Carriage": {
                myETD = this.EntityPM.PreCarriageETD;
                myATD = this.EntityPM.PreCarriageATD;
                myETA = this.EntityPM.PreCarriageETA;
                myATA = this.EntityPM.PreCarriageATA;
                break;
            }
            case "Main Carriage": {
                myETD = this.EntityPM.MainCarriageETD;
                myATD = this.EntityPM.MainCarriageATD;
                myETA = this.EntityPM.MainCarriageETA;
                myATA = this.EntityPM.MainCarriageATA;
                break;
            }
            case "Transshipment1": {
                myETD = this.EntityPM.Transshipment1ETD;
                myATD = this.EntityPM.Transshipment1ATD;
                myETA = this.EntityPM.Transshipment1ETA;
                myATA = this.EntityPM.Transshipment1ATA;
                break;
            }
            case "Transshipment2": {
                myETD = this.EntityPM.Transshipment2ETD;
                myATD = this.EntityPM.Transshipment2ATD;
                myETA = this.EntityPM.Transshipment2ETA;
                myATA = this.EntityPM.Transshipment2ATA;
                break;
            }
            case "Transshipment3": {
                myETD = this.EntityPM.Transshipment3ETD;
                myATD = this.EntityPM.Transshipment3ATD;
                myETA = this.EntityPM.Transshipment3ETA;
                myATA = this.EntityPM.Transshipment3ATA;
                break;
            }
            case "On Carriage": {
                myETD = this.EntityPM.OnCarriageETD;
                myATD = this.EntityPM.OnCarriageATD;
                myETA = this.EntityPM.OnCarriageETA;
                myATA = this.EntityPM.OnCarriageATA;
                break;
            }
            case "WarehouseLeg":
            case "WarehouseLeg_Pickups": {
                myETD = this.EntityPM.WarehouseLegExpectedEntryDate;
                myATD = this.EntityPM.WarehouseLegActualEntryDate;
                myETA = this.EntityPM.WarehouseLegExpectedReleaseDate;
                myATA = this.EntityPM.WarehouseLegActualReleaseDate;
                break;
            }
            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {
                    myETD = this.Delivery.ETD;
                    myATD = this.Delivery.ATD;
                    myETA = this.Delivery.ETA;
                    myATA = this.Delivery.ATA;
                }
                break;
            }
        }
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        if (myATD != null) {
            myDepartureDate = myATD;
            myDepartureColor = Tools_1.FontTool.Black;
        }
        else if (myETD != null) {
            myDepartureDate = myETD;
            if (Tools_1.DateTool.GetDateParts(myDepartureDate).DateTicks < Tools_1.DateTool.GetDateParts(todayDate).DateTicks) {
                myDepartureColor = Tools_1.FontTool.Red;
            }
            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.IsSetActualDepartureVisible = true;
                        }
                        break;
                    }
                default: {
                    this.IsSetActualDepartureVisible = true;
                    break;
                }
            }
        }
        if (myATA != null) {
            myArrivalDate = myATA;
            myArrivalColor = Tools_1.FontTool.Black;
        }
        else if (myETA != null) {
            myArrivalDate = myETA;
            if (Tools_1.DateTool.GetDateParts(myArrivalDate).DateTicks < Tools_1.DateTool.GetDateParts(todayDate).DateTicks) {
                myArrivalColor = Tools_1.FontTool.Red;
            }
            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.IsSetActualArrivalVisible = true;
                        }
                        break;
                    }
                default: {
                    this.IsSetActualArrivalVisible = true;
                    break;
                }
            }
        }
        this.DepartureDate = myDepartureDate;
        this.DepartureColor = myDepartureColor;
        this.ArrivalDate = myArrivalDate;
        this.ArrivalColor = myArrivalColor;
    };
    RoutingItem.prototype.SetActualDepartureClicked = function () {
        switch (this.LegType) {
            case "Pick Up": {
                this.Pickup.ATD = this.Pickup.ETD;
                break;
            }
            case "Pre Carriage": {
                this.EntityPM.PreCarriageATD = this.EntityPM.PreCarriageETD;
                break;
            }
            case "Main Carriage": {
                this.EntityPM.MainCarriageATD = this.EntityPM.MainCarriageETD;
                break;
            }
            case "Transshipment1": {
                this.EntityPM.Transshipment1ATD = this.EntityPM.Transshipment1ETD;
                break;
            }
            case "Transshipment2": {
                this.EntityPM.Transshipment2ATD = this.EntityPM.Transshipment2ETD;
                break;
            }
            case "Transshipment3": {
                this.EntityPM.Transshipment3ATD = this.EntityPM.Transshipment3ETD;
                break;
            }
            case "On Carriage": {
                this.EntityPM.OnCarriageATD = this.EntityPM.OnCarriageETD;
                break;
            }
            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    this.EntityPM.WarehouseLegActualEntryDate = this.EntityPM.WarehouseLegExpectedEntryDate;
                    break;
                }
            case "EmptyCR":
            case "Delivery": {
                this.Delivery.ATD = this.Delivery.ETD;
                break;
            }
        }
        this.GetDates();
    };
    RoutingItem.prototype.SetActualArrivalClicked = function () {
        switch (this.LegType) {
            case "Pick Up": {
                this.Pickup.ATA = this.Pickup.ETA;
                break;
            }
            case "Pre Carriage": {
                this.EntityPM.PreCarriageATA = this.EntityPM.PreCarriageETA;
                break;
            }
            case "Main Carriage": {
                this.EntityPM.MainCarriageATA = this.EntityPM.MainCarriageETA;
                break;
            }
            case "Transshipment1": {
                this.EntityPM.Transshipment1ATA = this.EntityPM.Transshipment1ETA;
                break;
            }
            case "Transshipment2": {
                this.EntityPM.Transshipment2ATA = this.EntityPM.Transshipment2ETA;
                break;
            }
            case "Transshipment3": {
                this.EntityPM.Transshipment3ATA = this.EntityPM.Transshipment3ETA;
                break;
            }
            case "On Carriage": {
                this.EntityPM.OnCarriageATA = this.EntityPM.OnCarriageETA;
                break;
            }
            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    this.EntityPM.WarehouseLegActualReleaseDate = this.EntityPM.WarehouseLegExpectedReleaseDate;
                    break;
                }
            case "EmptyCR":
            case "Delivery": {
                this.Delivery.ATA = this.Delivery.ETA;
                break;
            }
        }
        this.GetDates();
    };
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegWarehouseId", {
        // Warehouse Leg Fields 
        get: function () { return this.EntityPM.WarehouseLegWarehouseId; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegWarehouseId != value) {
                this.EntityPM.WarehouseLegWarehouseId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegReference", {
        get: function () { return this.EntityPM.WarehouseLegReference; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegReference != value) {
                this.EntityPM.WarehouseLegReference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegTerminalCode", {
        get: function () { return this.EntityPM.WarehouseLegTerminalCode; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegTerminalCode != value) {
                this.EntityPM.WarehouseLegTerminalCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegLastFreeDate", {
        get: function () { return this.EntityPM.WarehouseLegLastFreeDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegLastFreeDate != value) {
                this.EntityPM.WarehouseLegLastFreeDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "TerminalAvailable", {
        get: function () { return this.EntityPM.TerminalAvailable; },
        set: function (value) {
            if (this.EntityPM.TerminalAvailable != value) {
                this.EntityPM.TerminalAvailable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegCutOffDate != value) {
                this.EntityPM.WarehouseLegCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingItem.prototype, "WarehouseLegVGMCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegVGMCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegVGMCutOffDate != value) {
                this.EntityPM.WarehouseLegVGMCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    RoutingItem.prototype.GetContainersNumbers = function () {
        var _this = this;
        this.HasContainers = false;
        this.AllContainers = null;
        this.DisplayContainers = null;
        var AllContainersNumbers = [];
        if (this.fatherComponent.IsFCLEntity) {
            switch (this.LegType) {
                case "Pick Up": {
                    if (this.Pickup) {
                        this.Pickup.ShipmentPickUpDeliveryPackages.forEach(function (item) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }
                    break;
                }
                case "EmptyCR":
                case "Delivery": {
                    if (this.Delivery) {
                        this.Delivery.ShipmentPickUpDeliveryPackages.forEach(function (item) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }
                    break;
                }
                case "On Carriage": {
                    if (this.EntityPM.SplitOnCarriage == true) {
                        this.EntityPM.ShipmentPackages.forEach(function (item) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }
                    break;
                }
            }
            if (AllContainersNumbers.length > 0) {
                var count = 0;
                AllContainersNumbers.forEach(function (item) {
                    count++;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.AllContainers)) {
                        _this.AllContainers = item;
                        _this.DisplayContainers = item;
                    }
                    else {
                        _this.AllContainers += "," + item;
                        if (count <= 5) {
                            _this.DisplayContainers += "," + item;
                        }
                    }
                });
                this.HasContainers = true;
            }
        }
    };
    return RoutingItem;
}(BaseComponent_1.BaseComponent));
exports.RoutingItem = RoutingItem;
//# sourceMappingURL=RoutingsTabComponent.js.map