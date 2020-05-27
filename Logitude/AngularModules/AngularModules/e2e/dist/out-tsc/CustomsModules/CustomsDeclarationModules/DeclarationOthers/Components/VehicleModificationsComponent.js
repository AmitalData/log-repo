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
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DeclarationVehicleModificationListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationVehicleModificationListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var VehicleReductionTypeListService_1 = require("../../../../Customs/Services/StandardLists/VehicleReductionTypeListService");
var VehicleModificationsComponent = /** @class */ (function (_super) {
    __extends(VehicleModificationsComponent, _super);
    function VehicleModificationsComponent() {
        var _this = _super.call(this) || this;
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.DataContext = _this;
        _this.DummyList = new ObservableCollection_1.ObservableCollection([]);
        _this._DeclarationVehicleModificationListService = new DeclarationVehicleModificationListService_1.DeclarationVehicleModificationListService();
        _this.ChassisNumber = "";
        _this.AdjustmentTypeCode = "";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.Loaded = false;
        _this._VehicleReductionTypeList = [];
        var myVehicleReductionTypeListService = new VehicleReductionTypeListService_1.VehicleReductionTypeListService();
        myVehicleReductionTypeListService.getAllFromCache().
            subscribe(function (res) {
            _this._VehicleReductionTypeList = res.Result;
            _this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe(function (response) {
                _this.Loaded = true;
            });
        });
        return _this;
    }
    //public get HaveAdjustmentTypeCode(): boolean {
    //    if (AppTool.IsNullOrEmpty(this.AdjustmentTypeCode)) {
    //        return true;
    //    }
    //    return false;
    //}
    //_IsFucos: boolean = false;
    //public get ShowWaterMark(): boolean {
    //    if (!AppTool.IsNullOrEmpty(this.AdjustmentTypeCode)) {
    //        return false;
    //    }
    //    if (this._IsFucos) {
    //        return false;
    //    }
    //    return true;
    //}
    //IsFucos(val: boolean) {
    //    console.log("IsFucos", val);
    //    this._IsFucos = val;
    //}
    VehicleModificationsComponent.prototype.ChassisNumberTextChanged = function ($event) {
        this.ChassisNumber = $event;
        this.LoadDeclarationVehicleModifications();
    };
    VehicleModificationsComponent.prototype.OnLovItemChanged = function () {
        this.LoadDeclarationVehicleModifications();
    };
    VehicleModificationsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs.EntityPM;
        this.LoadDeclarationVehicleModifications();
    };
    VehicleModificationsComponent.prototype.LoadDeclarationVehicleModifications = function () {
        var _this = this;
        var test = false;
        if (test) {
            this.VehicleModiGroupList = [];
            var myGroupChassisNumber = new VehicleModiGroup();
            myGroupChassisNumber.ChassisNumber = "ChassisNumber   wqwa";
            myGroupChassisNumber.Total = 12323;
            myGroupChassisNumber.MyList = [];
            var myDeclarationVehicleModificationList = new DeclarationVehicleModificationListService_1.DeclarationVehicleModificationList();
            myDeclarationVehicleModificationList.AdjustmentType = "AdjustmentType";
            myDeclarationVehicleModificationList.DeductAmount = 1313131;
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            this.VehicleModiGroupList.push(myGroupChassisNumber);
            this.VehicleModiGroupList.push(myGroupChassisNumber);
            return;
        }
        var adjustmentTypeCode = this.AdjustmentTypeCode || "";
        var chassisNumber = this.ChassisNumber || "";
        this._DeclarationVehicleModificationListService
            .GetDeclarationVehicleModification(this.EntityPM.Id, chassisNumber, adjustmentTypeCode, this.EntityPM.Tenant).
            subscribe(function (res) {
            var aryDeclarationVehicleModificationList;
            aryDeclarationVehicleModificationList = res.Result;
            _this.VehicleModiGroupList = [];
            aryDeclarationVehicleModificationList.forEach(function (itemDb) {
                var vehicleReductionType = _this._VehicleReductionTypeList.filter(function (typeRec) { return typeRec.Code == itemDb.AdjustmentType; })[0];
                if (!Tools_1.AppTool.IsNullOrEmpty(vehicleReductionType)) {
                    itemDb.AdjustmentTypeName = vehicleReductionType.LocalName;
                }
                var listChassisNumber = _this.VehicleModiGroupList.filter(function (group) { return itemDb.ChassisNumber == group.ChassisNumber; });
                var myGroupChassisNumber = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(listChassisNumber) && listChassisNumber.length > 0) {
                    myGroupChassisNumber = listChassisNumber[0];
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myGroupChassisNumber)) {
                    myGroupChassisNumber = new VehicleModiGroup();
                    myGroupChassisNumber.ChassisNumber = itemDb.ChassisNumber;
                    _this.VehicleModiGroupList.push(myGroupChassisNumber);
                }
                myGroupChassisNumber.Total = myGroupChassisNumber.Total + Number(itemDb.DeductAmount);
                myGroupChassisNumber.MyList.push(itemDb);
            });
            //Order list by CreateDate
            _this.VehicleModiGroupList.sort(function (a, b) {
                return (a.ChassisNumber === b.ChassisNumber) ? 0 :
                    (a.ChassisNumber < b.ChassisNumber) ? -1 : 1;
            });
        });
    };
    VehicleModificationsComponent = __decorate([
        core_1.Component({
            selector: 'VehicleModificationsComponent',
            moduleId: module.id,
            templateUrl: './VehicleModificationsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VehicleModificationsComponent);
    return VehicleModificationsComponent;
}(BaseComponent_1.BaseComponent));
exports.VehicleModificationsComponent = VehicleModificationsComponent;
var VehicleModiGroup = /** @class */ (function () {
    function VehicleModiGroup() {
        this.Total = 0;
        this.MyList = [];
    }
    return VehicleModiGroup;
}());
//# sourceMappingURL=VehicleModificationsComponent.js.map