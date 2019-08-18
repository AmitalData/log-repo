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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var VatTypePercentagePM_1 = require("../../../../Common/EntityPMs/VatTypePercentagePM");
var VatTypePMService_1 = require("../../../../Common/Services/StandardPMs/VatTypePMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var UpdateVATPercentageComponent = /** @class */ (function (_super) {
    __extends(UpdateVATPercentageComponent, _super);
    function UpdateVATPercentageComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.VatTypePM = null;
        _this.EntityPM = null;
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "VatTypePercentage";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new VatTypePMService_1.VatTypePMService();
        return _this;
    }
    UpdateVATPercentageComponent.prototype.SetWindowArgs = function (myVatTypeId) {
        var _this = this;
        this.VatTypeId = myVatTypeId;
        this.EntityPM = new VatTypePercentagePM_1.VatTypePercentagePM(null);
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.EntityPM.VatTypeId = this.VatTypeId;
        this.EntityPM.FromDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.VatTypeId)) {
                _this.CurrentSession.StartBusyIndicatorLoading();
                _this.myService.get(_this.VatTypeId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.VatTypePM = myResponse.Result;
                        if (_this.VatTypePM != null) {
                            _this.VatTypePM.AddVatTypePercentagePM(_this.EntityPM);
                        }
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    };
    Object.defineProperty(UpdateVATPercentageComponent.prototype, "FromDate", {
        get: function () { return this.EntityPM.FromDate; },
        set: function (newValue) {
            if (this.EntityPM.FromDate != newValue) {
                this.EntityPM.FromDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateVATPercentageComponent.prototype, "Percentage", {
        get: function () { return this.EntityPM.Percentage; },
        set: function (newValue) {
            if (this.EntityPM.Percentage != newValue) {
                this.EntityPM.Percentage = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    UpdateVATPercentageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UpdateVATPercentageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (this.FromDate == null) {
                errors.push("From Date is required");
            }
            if (this.Percentage == null) {
                errors.push("Percentage is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.update(this.VatTypePM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    };
    UpdateVATPercentageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UpdateVATPercentageComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], UpdateVATPercentageComponent);
    return UpdateVATPercentageComponent;
}(BaseComponent_1.BaseComponent));
exports.UpdateVATPercentageComponent = UpdateVATPercentageComponent;
//# sourceMappingURL=UpdateVATPercentageComponent.js.map