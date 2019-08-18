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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var AWBSpecialHandlingCodeExtendedPMService_1 = require("../../../../Shipment/Services/ExtendedPMs/AWBSpecialHandlingCodeExtendedPMService");
var IATACodeExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/IATACodeExtendedPMService");
var BookingProductExtendedPMService_1 = require("../../../../Booking/Services/ExtendedPMs/BookingProductExtendedPMService");
var CommodityPMService_1 = require("../../../../Common/Services/StandardPMs/CommodityPMService");
var AddEditAirlineAdaptationItemComponent = /** @class */ (function (_super) {
    __extends(AddEditAirlineAdaptationItemComponent, _super);
    function AddEditAirlineAdaptationItemComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsResourcesReady = false;
        return _this;
    }
    AddEditAirlineAdaptationItemComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.AirlinePM = windowArgs['AirlinePM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        this.ObjectTableName = windowArgs['ObjectTableName'];
        this.SetUIProperties();
        this.Clone();
        this.IsResourcesReady = true;
    };
    AddEditAirlineAdaptationItemComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNew);
    };
    Object.defineProperty(AddEditAirlineAdaptationItemComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineAdaptationItemComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineAdaptationItemComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAirlineAdaptationItemComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAirlineAdaptationItemComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SetService();
            if (this.IsNew) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.insert(this.EntityPM).subscribe(function (Result) {
                    var mm = Result;
                    if (!mm.HasError) {
                        CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                        CachedDataManager_1.CachedDataManager.RefreshCompleted.subscribe(function ($event) { return _this.Close($event); });
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.update(this.EntityPM).subscribe(function (Result) {
                    var mm = Result;
                    if (!mm.HasError) {
                        CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                        CachedDataManager_1.CachedDataManager.RefreshCompleted.subscribe(function ($event) { return _this.Close($event); });
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    AddEditAirlineAdaptationItemComponent.prototype.Close = function (event) {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    AddEditAirlineAdaptationItemComponent.prototype.SetService = function () {
        switch (this.ObjectTableName) {
            case "AWBSpecialHandlingCode": {
                this.myService = new AWBSpecialHandlingCodeExtendedPMService_1.AWBSpecialHandlingCodeExtendedPMService();
                break;
            }
            case "Commodity": {
                this.myService = new CommodityPMService_1.CommodityPMService();
                break;
            }
            case "IATACode": {
                this.myService = new IATACodeExtendedPMService_1.IATACodeExtendedPMService();
                break;
            }
            case "BookingProduct": {
                this.myService = new BookingProductExtendedPMService_1.BookingProductExtendedPMService();
                break;
            }
        }
    };
    AddEditAirlineAdaptationItemComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Code');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditAirlineAdaptationItemComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAirlineAdaptationItemComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAirlineAdaptationItemComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAirlineAdaptationItemComponent);
    return AddEditAirlineAdaptationItemComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAirlineAdaptationItemComponent = AddEditAirlineAdaptationItemComponent;
//# sourceMappingURL=AddEditAirlineAdaptationItemComponent.js.map