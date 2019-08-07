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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TariffVersionAllInChargePM_1 = require("../../../EntityPMs/TariffVersionAllInChargePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var AddEditAllInChargesComponent = /** @class */ (function () {
    function AddEditAllInChargesComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.TariffPM = null;
        this.ItemsSource = [];
        this.ObjectTableName = "TariffVersionAllInCharge";
        this.IsEditingEnabled = true;
        this.IsVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isVersionDirty = false;
        this.isTariffDirty = false;
    }
    AddEditAllInChargesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            if (args) {
                _this.IsEditingEnabled = args['IsEditingEnabled'];
                _this.EntityPM = args['VersionPM'];
                _this.TariffPM = args['TariffPM'];
                _this.isVersionDirty = _this.EntityPM.IsDirty;
                _this.isTariffDirty = _this.TariffPM.IsDirty;
                _this.BuildQueryFilters();
                _this.EntityPM.TariffAllInCharges.forEach(function (item) {
                    _this.ItemsSource.push(new AllInChargeItemClass(item, _this));
                });
                _this.Clone();
            }
            _this.IsVisibile = true;
        });
    };
    AddEditAllInChargesComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsAir", true, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "FRT", null, null, "NotEqual", false, false, false, "string");
    };
    AddEditAllInChargesComponent.prototype.AddButtonClicked = function () {
        var item = new TariffVersionAllInChargePM_1.TariffVersionAllInChargePM(this.EntityPM);
        item.Version = this.EntityPM.Version;
        item.TariffId = this.TariffPM.Id;
        item.AddDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        item.AddedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        item.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.ItemsSource.push(new AllInChargeItemClass(item, this));
    };
    AddEditAllInChargesComponent.prototype.DeleteItem = function (item) {
        if (item) {
            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }
        }
    };
    AddEditAllInChargesComponent.prototype.CancelButtonClicked = function () {
        this.ItemsSource.forEach(function (item) {
            if (item.ChargesTypeId != item.OldValue) {
                item.ChargesTypeId = item.OldValue;
            }
        });
        this.EntityPM.IsDirty = this.isVersionDirty;
        this.TariffPM.IsDirty = this.isTariffDirty;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAllInChargesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        this.ItemsSource.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item.EntityPM, _this.ObjectTableName, errors);
        });
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var allItemsPM = [];
            this.ItemsSource.forEach(function (item) {
                var index = _this.EntityPM.TariffAllInCharges.indexOf(item.EntityPM);
                if (index > -1) {
                    var itemPM = _this.EntityPM.TariffAllInCharges[index];
                    if (itemPM) {
                    }
                }
                else {
                    _this.EntityPM.TariffAllInCharges.push(item.EntityPM);
                }
                allItemsPM.push(item.EntityPM);
            });
            for (var i = this.EntityPM.TariffAllInCharges.length - 1; i >= 0; i--) {
                var index = allItemsPM.indexOf(this.EntityPM.TariffAllInCharges[i]);
                if (index == -1) {
                    var item = this.EntityPM.TariffAllInCharges[i];
                    this.EntityPM.RemoveTariffVersionAllInCharge(item);
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    };
    AddEditAllInChargesComponent.prototype.Clone = function () {
    };
    AddEditAllInChargesComponent.prototype.RejectChanges = function () {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    };
    AddEditAllInChargesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAllInChargesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditAllInChargesComponent);
    return AddEditAllInChargesComponent;
}());
exports.AddEditAllInChargesComponent = AddEditAllInChargesComponent;
var AllInChargeItemClass = /** @class */ (function (_super) {
    __extends(AllInChargeItemClass, _super);
    function AllInChargeItemClass(item, parent) {
        var _this = _super.call(this) || this;
        _this.parent = parent;
        _this.Id = null;
        _this.ObjectTableName = "TariffVersionAllInCharge";
        _this.OldValue = null;
        _this.Id = item.Id;
        _this.EntityPM = item;
        _this.OldValue = item.ChargesTypeId;
        _this.SetUIProperties();
        return _this;
    }
    Object.defineProperty(AllInChargeItemClass.prototype, "ChargesTypeId", {
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AllInChargeItemClass.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.parent.IsEditingEnabled);
    };
    return AllInChargeItemClass;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=AddEditAllInChargesComponent.js.map