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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var SpotLightDateComponent = /** @class */ (function (_super) {
    __extends(SpotLightDateComponent, _super);
    function SpotLightDateComponent() {
        var _this = _super.call(this) || this;
        _this.PopupClosed = new core_1.EventEmitter();
        _this.ShowHelp = false;
        _this.ObjectTableName = null;
        _this.legname = null;
        _this.State = "Departure";
        _this.DataContext = _this;
        _this.ControlId = null;
        _this.DropdownId = null;
        _this.IsMouseIn = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.expectedDepartedDateIsChecked = false;
        _this.actualDepartedDateIsChecked = false;
        if (_this.CurrentSession == null) {
            _this.ControlId = "SpotLightDate_-1_-1";
            _this.DropdownId = "SpotLightDateDropdownId_-1_-1";
        }
        else {
            var idIndex = _this.CurrentSession.GetNewId("SpotLightDate");
            _this.ControlId = "SpotLightDate" + idIndex;
            _this.DropdownId = "SpotLightDateDropdownId" + idIndex;
        }
        _this.CurrentSession.MouseDownEvent.subscribe(function (res) {
            if (_this.IsMouseIn == false) {
                _this.OnLostFocus();
            }
        });
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        return _this;
    }
    SpotLightDateComponent.prototype.onMouseOver = function () {
        this.IsMouseIn = true;
    };
    SpotLightDateComponent.prototype.onMouseOut = function () {
        this.IsMouseIn = false;
    };
    Object.defineProperty(SpotLightDateComponent.prototype, "ExpectedDepartedDateIsChecked", {
        get: function () {
            if (this.DepartedDate_Estimate != null) {
                this.expectedDepartedDateIsChecked = true;
            }
            return this.expectedDepartedDateIsChecked;
        },
        set: function (newValue) {
            this.expectedDepartedDateIsChecked = newValue;
            if (!newValue) {
                this.DepartedDate_Estimate = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpotLightDateComponent.prototype, "ActualDepartedDateIsChecked", {
        get: function () {
            if (this.DepartedDate_Actual != null) {
                this.actualDepartedDateIsChecked = true;
            }
            return this.actualDepartedDateIsChecked;
        },
        set: function (newValue) {
            this.actualDepartedDateIsChecked = newValue;
            if (newValue) {
                this.DepartedDate_Actual = this.DepartedDate_Estimate;
            }
            else {
                this.DepartedDate_Actual = null;
            }
            //this.actualDepartedDateIsChecked = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpotLightDateComponent.prototype, "DepartedDate_Estimate", {
        get: function () {
            if (this.State == "Departure") {
                switch (this.legname) {
                    case "Pick Up": {
                        return this.PickUpPM.ETD;
                    }
                    case "Delivery": {
                        return this.DeliveryPM.ETD;
                    }
                    case "Pre Carriage": {
                        return this.EntityPM.PreCarriageETD;
                    }
                    case "Main Carriage": {
                        return this.EntityPM.MainCarriageETD;
                    }
                    case "Transshipment1": {
                        return this.EntityPM.Transshipment1ETD;
                    }
                    case "Transshipment2": {
                        return this.EntityPM.Transshipment2ETD;
                    }
                    case "Transshipment3": {
                        return this.EntityPM.Transshipment3ETD;
                    }
                    case "On Carriage": {
                        return this.EntityPM.OnCarriageETD;
                    }
                    case "WarehouseLeg": {
                        return this.EntityPM.WarehouseLegExpectedEntryDate;
                    }
                    default: {
                        break;
                    }
                }
            }
            else {
                switch (this.legname) {
                    case "Pick Up": {
                        return this.PickUpPM.ETA;
                    }
                    case "Delivery": {
                        return this.DeliveryPM.ETA;
                    }
                    case "Pre Carriage": {
                        return this.EntityPM.PreCarriageETA;
                    }
                    case "Main Carriage": {
                        return this.EntityPM.MainCarriageETA;
                    }
                    case "Transshipment1": {
                        return this.EntityPM.Transshipment1ETA;
                    }
                    case "Transshipment2": {
                        return this.EntityPM.Transshipment2ETA;
                    }
                    case "Transshipment3": {
                        return this.EntityPM.Transshipment3ETA;
                    }
                    case "On Carriage": {
                        return this.EntityPM.OnCarriageETA;
                    }
                    case "WarehouseLeg": {
                        return this.EntityPM.WarehouseLegExpectedReleaseDate;
                    }
                    default: {
                        break;
                    }
                }
            }
        },
        set: function (newValue) {
            if (this.State == "Departure") {
                switch (this.legname) {
                    case "Pick Up": {
                        this.PickUpPM.ETD = newValue;
                        break;
                    }
                    case "Delivery": {
                        this.DeliveryPM.ETD = newValue;
                        break;
                    }
                    case "Pre Carriage": {
                        this.EntityPM.PreCarriageETD = newValue;
                        break;
                    }
                    case "Main Carriage": {
                        this.EntityPM.MainCarriageETD = newValue;
                        break;
                    }
                    case "Transshipment1": {
                        this.EntityPM.Transshipment1ETD = newValue;
                        break;
                    }
                    case "Transshipment2": {
                        this.EntityPM.Transshipment2ETD = newValue;
                        break;
                    }
                    case "Transshipment3": {
                        this.EntityPM.Transshipment3ETD = newValue;
                        break;
                    }
                    case "On Carriage": {
                        this.EntityPM.OnCarriageETD = newValue;
                        break;
                    }
                    case "WarehouseLeg": {
                        this.EntityPM.WarehouseLegExpectedEntryDate = newValue;
                        break;
                    }
                    default: {
                        break;
                    }
                }
            }
            else {
                switch (this.legname) {
                    case "Pick Up": {
                        this.PickUpPM.ETA = newValue;
                        break;
                    }
                    case "Delivery": {
                        this.DeliveryPM.ETA = newValue;
                        break;
                    }
                    case "Pre Carriage": {
                        this.EntityPM.PreCarriageETA = newValue;
                        break;
                    }
                    case "Main Carriage": {
                        this.EntityPM.MainCarriageETA = newValue;
                        break;
                    }
                    case "Transshipment1": {
                        this.EntityPM.Transshipment1ETA = newValue;
                        break;
                    }
                    case "Transshipment2": {
                        this.EntityPM.Transshipment2ETA = newValue;
                        break;
                    }
                    case "Transshipment3": {
                        this.EntityPM.Transshipment3ETA = newValue;
                        break;
                    }
                    case "On Carriage": {
                        this.EntityPM.OnCarriageETA = newValue;
                        break;
                    }
                    case "WarehouseLeg": {
                        this.EntityPM.WarehouseLegExpectedReleaseDate = newValue;
                        break;
                    }
                    default: {
                        break;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpotLightDateComponent.prototype, "DepartedDate_Actual", {
        //private departedDate_Actual: Date;
        get: function () {
            if (this.State == "Departure") {
                switch (this.legname) {
                    case "Pick Up": {
                        return this.PickUpPM.ATD;
                    }
                    case "Delivery": {
                        return this.DeliveryPM.ATD;
                    }
                    case "Pre Carriage": {
                        return this.EntityPM.PreCarriageATD;
                    }
                    case "Main Carriage": {
                        return this.EntityPM.MainCarriageATD;
                    }
                    case "Transshipment1": {
                        return this.EntityPM.Transshipment1ATD;
                    }
                    case "Transshipment2": {
                        return this.EntityPM.Transshipment2ATD;
                    }
                    case "Transshipment3": {
                        return this.EntityPM.Transshipment3ATD;
                    }
                    case "On Carriage": {
                        return this.EntityPM.OnCarriageATD;
                    }
                    case "WarehouseLeg": {
                        return this.EntityPM.WarehouseLegActualEntryDate;
                    }
                    default: {
                        return null;
                    }
                }
            }
            else {
                switch (this.legname) {
                    case "Pick Up": {
                        return this.PickUpPM.ATA;
                    }
                    case "Delivery": {
                        return this.DeliveryPM.ATA;
                    }
                    case "Pre Carriage": {
                        return this.EntityPM.PreCarriageATA;
                    }
                    case "Main Carriage": {
                        return this.EntityPM.MainCarriageATA;
                    }
                    case "Transshipment1": {
                        return this.EntityPM.Transshipment1ATA;
                    }
                    case "Transshipment2": {
                        return this.EntityPM.Transshipment2ATA;
                    }
                    case "Transshipment3": {
                        return this.EntityPM.Transshipment3ATA;
                    }
                    case "On Carriage": {
                        return this.EntityPM.OnCarriageATA;
                    }
                    case "WarehouseLeg": {
                        return this.EntityPM.WarehouseLegActualReleaseDate;
                    }
                    default: {
                        return null;
                    }
                }
            }
        },
        set: function (newValue) {
            if (this.State == "Departure") {
                switch (this.legname) {
                    case "Pick Up":
                        {
                            this.PickUpPM.ATD = newValue;
                            //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                            //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber);
                            //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber + PickUpPM.PickUpDeliveryNumber);
                            break;
                        }
                    case "Delivery":
                        {
                            this.DeliveryPM.ATD = newValue;
                            //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                            //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber);
                            //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber + DeliveryPM.PickUpDeliveryNumber);
                            break;
                        }
                    case "Pre Carriage":
                        {
                            this.EntityPM.PreCarriageATD = newValue;
                            //this.SetActualMethod("PreCarriageDeparture");
                            break;
                        }
                    case "Main Carriage":
                        {
                            this.EntityPM.MainCarriageATD = newValue;
                            //this.SetActualMethod("MainCarriageDeparture");
                            break;
                        }
                    case "Transshipment1":
                        {
                            this.EntityPM.Transshipment1ATD = newValue;
                            //this.SetActualMethod("Transshipment1Departure");
                            break;
                        }
                    case "Transshipment2":
                        {
                            this.EntityPM.Transshipment2ATD = newValue;
                            //this.SetActualMethod("Transshipment2Departure");
                            break;
                        }
                    case "Transshipment3":
                        {
                            this.EntityPM.Transshipment3ATD = newValue;
                            //this.SetActualMethod("Transshipment2Departure");
                            break;
                        }
                    case "On Carriage":
                        {
                            this.EntityPM.OnCarriageATD = newValue;
                            //this.SetActualMethod("OnCarriageDeparture");
                            break;
                        }
                    case "WarehouseLeg":
                        {
                            this.EntityPM.WarehouseLegActualEntryDate = newValue;
                            break;
                        }
                    default: {
                        break;
                    }
                }
            }
            else {
                switch (this.legname) {
                    case "Pick Up":
                        {
                            this.PickUpPM.ATA = newValue;
                            //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                            //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber);
                            //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber + PickUpPM.PickUpDeliveryNumber);
                            break;
                        }
                    case "Delivery":
                        {
                            this.DeliveryPM.ATA = newValue;
                            //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                            //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber);
                            //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber + DeliveryPM.PickUpDeliveryNumber);
                            break;
                        }
                    case "Pre Carriage":
                        {
                            this.EntityPM.PreCarriageATA = newValue;
                            //this.SetActualMethod("PreCarriageDeparture");
                            break;
                        }
                    case "Main Carriage":
                        {
                            this.EntityPM.MainCarriageATA = newValue;
                            //this.SetActualMethod("MainCarriageDeparture");
                            break;
                        }
                    case "Transshipment1":
                        {
                            this.EntityPM.Transshipment1ATA = newValue;
                            //this.SetActualMethod("Transshipment1Departure");
                            break;
                        }
                    case "Transshipment2":
                        {
                            this.EntityPM.Transshipment2ATA = newValue;
                            //this.SetActualMethod("Transshipment2Departure");
                            break;
                        }
                    case "Transshipment3":
                        {
                            this.EntityPM.Transshipment3ATA = newValue;
                            //this.SetActualMethod("Transshipment2Departure");
                            break;
                        }
                    case "On Carriage":
                        {
                            this.EntityPM.OnCarriageATA = newValue;
                            //this.SetActualMethod("OnCarriageDeparture");
                            break;
                        }
                    case "WarehouseLeg":
                        {
                            this.EntityPM.WarehouseLegActualReleaseDate = newValue;
                            break;
                        }
                    default: {
                        break;
                    }
                }
            }
            //this.departedDate_Actual = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpotLightDateComponent.prototype, "DepartedTime_Actual", {
        get: function () { return this.departedTime_Actual; },
        set: function (newValue) { this.departedTime_Actual = newValue; },
        enumerable: true,
        configurable: true
    });
    SpotLightDateComponent.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    SpotLightDateComponent.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.SetControlPosition(); }, 1);
    };
    SpotLightDateComponent.prototype.SetControlPosition = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top + 22) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    };
    SpotLightDateComponent.prototype.OnSelectedDepartedDate_ActualChanged = function (date) {
        this.DepartedDate_Actual = date.SelectedDate;
    };
    SpotLightDateComponent.prototype.OnSelectedDepartedDate_EstimateChanged = function (date) {
        this.DepartedDate_Estimate = date.SelectedDate;
    };
    SpotLightDateComponent.prototype.ngAfterViewInit = function () {
    };
    SpotLightDateComponent.prototype.ngOnInit = function () {
    };
    SpotLightDateComponent.prototype.onOKBtnClick = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this._ShipmentPMService.update(this.EntityPM).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.OnLostFocus();
                _this.PopupClosed.emit(_this);
            }
            else {
                _this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });
    };
    SpotLightDateComponent.prototype.onCancelBtnClick = function () {
        this.OnLostFocus();
    };
    SpotLightDateComponent.prototype.ToggleButtonFocus = function () {
        var elem = document.getElementById(this.DropdownId);
        if (elem) {
            elem.style.visibility = "visible";
            elem.style.opacity = "1";
            elem.style.pointerEvents = "auto";
        }
        this.RunPositionTimer();
    };
    SpotLightDateComponent.prototype.OnLostFocus = function () {
        this.StopPositionTimer();
        var elem = document.getElementById(this.DropdownId);
        if (elem) {
            elem.style.visibility = "hidden";
            elem.style.opacity = "0";
            elem.style.pointerEvents = "none";
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SpotLightDateComponent.prototype, "PopupClosed", void 0);
    SpotLightDateComponent = __decorate([
        core_1.Component({
            selector: 'SpotLightDate',
            moduleId: module.id,
            templateUrl: './SpotLightDateComponent.html',
            inputs: ['EntityPM', 'legname', 'PickUpPM', 'DeliveryPM', 'State'],
        }),
        __metadata("design:paramtypes", [])
    ], SpotLightDateComponent);
    return SpotLightDateComponent;
}(BaseComponent_1.BaseComponent));
exports.SpotLightDateComponent = SpotLightDateComponent;
//# sourceMappingURL=SpotLightDateComponent.js.map