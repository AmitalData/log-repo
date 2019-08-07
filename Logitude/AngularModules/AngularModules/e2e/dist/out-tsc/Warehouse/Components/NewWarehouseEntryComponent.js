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
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var WarehouseEntryPM_1 = require("../../Warehouse/EntityPMs/WarehouseEntryPM");
var Tools_1 = require("../../Infrastructure/Tools");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var WarehouseHelper_1 = require("../Helpers/WarehouseHelper");
var LocationDirective_1 = require("../../Infrastructure/Utilities/LocationDirective");
var NewWarehouseEntryComponent = /** @class */ (function (_super) {
    __extends(NewWarehouseEntryComponent, _super);
    function NewWarehouseEntryComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.warehouseHelper = new WarehouseHelper_1.WarehouseHelper();
        _this.IsNotSetWarehouseIdForWarehouseLegShipment = false;
        _this.DataContext = _this;
        _this.warehouseEntryPM = new WarehouseEntryPM_1.WarehouseEntryPM();
        _this.IsNewEntity = false;
        _this.IsFromShipment = true;
        _this.IsLoadPage = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsLCLEntity = true;
        _this.Retries = 0;
        _this.warehouseEntryPM = _this.warehouseHelper.GetNewWarehouseEntry(_this);
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        _this.EventTypeCodeList = [];
        var table = window.ObjectTables.filter(function (d) { return d.Name == "WarehouseEntry"; })[0];
        if (table)
            _this.ObjectTableId = table.Id;
        return _this;
    }
    NewWarehouseEntryComponent.prototype.ngOnInit = function () {
    };
    NewWarehouseEntryComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe(function (response) {
            _this.Start(args);
        });
    };
    NewWarehouseEntryComponent.prototype.Start = function (args) {
        var _this = this;
        this.RunComponent();
        this.ShipmentPM = args.ShipmentPM;
        if (args.WarehouseEntryPackagesLists) {
            args.WarehouseEntryPackagesLists.forEach(function (item) {
                _this.warehouseEntryPM.AddWarehouseEntryPackage(item);
            });
        }
        this.SetValue(args);
        this.IsLoadPage = true;
    };
    NewWarehouseEntryComponent.prototype.SetValue = function (args) {
        var _this = this;
        if (this.warehouseEntryPM) {
            if (this.ShipmentPM) {
                if (this.ShipmentPM.ShipmentLevelCode == "D")
                    this.warehouseEntryPM.CustomerId = this.ShipmentPM.CustomerId;
                this.warehouseEntryPM.ShipmentId = this.ShipmentPM.Id;
                this.warehouseEntryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                this.warehouseEntryPM.HouseNumber = this.ShipmentPM.House;
                this.warehouseEntryPM.MasterNumber = this.ShipmentPM.LongMaster;
                this.warehouseEntryPM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
                this.warehouseEntryPM.TransportModeId = this.ShipmentPM.TransportModeId;
                this.warehouseEntryPM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
                this.warehouseEntryPM.DirectionId = this.ShipmentPM.DirectionId;
                this.warehouseEntryPM.ShipperId = this.ShipmentPM.ShipperId;
                this.warehouseEntryPM.ShipperName = this.ShipmentPM.ShipperName;
                this.warehouseEntryPM.ShipperReference1 = this.ShipmentPM.ShipperReference1;
                this.warehouseEntryPM.ShipperReference2 = this.ShipmentPM.ShipperReference2;
                this.warehouseEntryPM.ConsigneeId = this.ShipmentPM.ConsigneeId;
                this.warehouseEntryPM.ConsigneeName = this.ShipmentPM.ConsigneeName;
                this.warehouseEntryPM.ConsigneeReference1 = this.ShipmentPM.ConsigneeReference1;
                this.warehouseEntryPM.ConsigneeReference2 = this.ShipmentPM.ConsigneeReference2;
                if (this.warehouseEntryPM.DirectionId == "D" && this.warehouseEntryPM.TransportModeId == "I") {
                    this.warehouseEntryPM.FromAddressId = this.ShipmentPM.MainCarriageFromAddressId;
                    this.warehouseEntryPM.ToAddressId = this.ShipmentPM.MainCarriageToAddressId;
                    this.warehouseEntryPM.FromPartnerId = this.ShipmentPM.MainCarriageFromPartnerId;
                    this.warehouseEntryPM.ToPartnerId = this.ShipmentPM.MainCarriageToPartnerId;
                }
                else {
                    this.warehouseEntryPM.FromPortId = this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId;
                    this.warehouseEntryPM.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;
                }
            }
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            this.IsNotSetWarehouseIdForWarehouseLegShipment = args.IsNotSetWarehouseIdForWarehouseLegShipment;
            var myCommonDomain = new CommonDomainService_1.CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(warehouseId) && Tools_1.AppTool.IsNullOrEmpty(args.WarehouseId)) {
                        _this.warehouseEntryPM.WarehouseId = warehouseId;
                    }
                    else {
                        _this.warehouseEntryPM.WarehouseId = args.WarehouseId;
                    }
                }
                else {
                    _this.warehouseEntryPM.WarehouseId = args.WarehouseId;
                }
            });
            this.warehouseEntryPM.ExpectedEntryDate = args.ExpectedEntryDate;
            this.warehouseEntryPM.ActualEntryDate = args.ActualEntryDate;
            this.ActualEntryDate = this.warehouseEntryPM.ActualEntryDate;
        }
    };
    NewWarehouseEntryComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.LoadChildComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewWarehouseEntryComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewWarehouseEntryComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        var warehouseEntryPackagesDetailsComponenttLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "WEPD"; })[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(function (cmpRef) {
                var windowArgs = { WarehouseEntryPM: _this.warehouseEntryPM, ViewModelTrigger: _this };
                cmpRef.instance.SetWindowArgs(windowArgs);
            });
        }
    };
    NewWarehouseEntryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewWarehouseEntryComponent.prototype.SaveButtonClicked = function () {
        this.warehouseEntryPM.ConnectedToShipment = true;
        this.warehouseHelper.CreateWarehouseEntry(this.warehouseEntryPM, this);
    };
    NewWarehouseEntryComponent.prototype.OnActualEntryDateDatePickerChange = function (value) {
        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);
        if (!Tools_1.DateTool.IsActualDateValid(value)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
            this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
        }
    };
    Object.defineProperty(NewWarehouseEntryComponent.prototype, "ActualEntryDate", {
        get: function () {
            var actualEntryDate = null;
            if (this.warehouseEntryPM)
                actualEntryDate = this.warehouseEntryPM.ActualEntryDate;
            return actualEntryDate;
        },
        set: function (value) {
            if (this.warehouseEntryPM != null) {
                if (value != this.warehouseEntryPM.ActualEntryDate) {
                    this.warehouseEntryPM.ActualEntryDate = value;
                    this.OnActualEntryDateDatePickerChange(value);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewWarehouseEntryComponent.prototype.SetActualDateClicked = function (fieldName) {
        this.ActualEntryDate = Tools_1.DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], NewWarehouseEntryComponent.prototype, "AllLocations", void 0);
    NewWarehouseEntryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewWarehouseEntryComponent',
            templateUrl: './NewWarehouseEntryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewWarehouseEntryComponent);
    return NewWarehouseEntryComponent;
}(BaseComponent_1.BaseComponent));
exports.NewWarehouseEntryComponent = NewWarehouseEntryComponent;
//# sourceMappingURL=NewWarehouseEntryComponent.js.map