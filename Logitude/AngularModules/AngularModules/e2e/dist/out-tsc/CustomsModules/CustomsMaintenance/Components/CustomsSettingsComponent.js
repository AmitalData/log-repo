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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\CustomsSettingPMService.ts
var CustomsSettingPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsSettingPMService");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomsSettingsComponent = /** @class */ (function (_super) {
    __extends(CustomsSettingsComponent, _super);
    function CustomsSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsSetting";
        _this.columns = null;
        _this._CustomsSettingPMService = new CustomsSettingPMService_1.CustomsSettingPMService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        return _this;
    }
    CustomsSettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            var filters = new ApiQueryFilters_1.ApiQueryFilters(true);
            filters.addAdditionalFilter("Tenant", SessionLocator_1.SessionLocator.Tenant, null, null, "Equals", false, false, false, "string");
            _this._CustomsSettingListService.getByFilters(filters)
                .subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var listCustomsSetting = myResponse.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(listCustomsSetting)) {
                            _this._TenantCustomsSettingList = listCustomsSetting[0];
                            _this._CustomsSettingPMService.get(_this._TenantCustomsSettingList.Id)
                                .subscribe(function (myResponse) {
                                _this.entityPM = myResponse.Result;
                                _this.Loaded = true;
                                _this.ValidScreen();
                            });
                        }
                    }
                }
            });
            //this.RefreshBtnClick()
        });
    };
    CustomsSettingsComponent.prototype.ValidScreen = function () {
        if (!this.IsConnectedToUniFreight) {
            this.IsUnifreightCertificateActivatedEnabled = false;
            this.UnifreightCertificateActivated = false;
        }
        else {
            this.IsUnifreightCertificateActivatedEnabled = true;
        }
    };
    Object.defineProperty(CustomsSettingsComponent.prototype, "UnifreightCertificateActivated", {
        ///#region Properties
        //IsUnifreightCertificateActivatedEnabled: boolean = true;
        get: function () { return this.entityPM != null ? this.entityPM.UnifreightCertificateActivated : false; },
        set: function (value) { this.entityPM.UnifreightCertificateActivated = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "AutoFillAccountType", {
        get: function () { return this.entityPM != null ? this.entityPM.AutoFillAccountType : false; },
        set: function (value) { this.entityPM.AutoFillAccountType = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "AutoUnitMeasurement", {
        get: function () { return this.entityPM != null ? this.entityPM.AutoUnitMeasurement : false; },
        set: function (value) { this.entityPM.AutoUnitMeasurement = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "PaymentOrderAccCard", {
        get: function () { return this.entityPM != null ? this.entityPM.PaymentOrderAccCard : null; },
        set: function (value) { this.entityPM.PaymentOrderAccCard = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "IsConnectedToUniFreight", {
        get: function () { return this.entityPM != null ? this.entityPM.IsConnectedToUniFreight : false; },
        set: function (value) {
            this.entityPM.IsConnectedToUniFreight = value;
            if (!value) {
                this.IsUnifreightCertificateActivatedEnabled = false;
                this.UnifreightCertificateActivated = false;
            }
            else {
                this.IsUnifreightCertificateActivatedEnabled = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "IsUnifreightCertificateActivatedEnabled", {
        set: function (val) {
            this.UIProperties.SetEnabled("UnifreightCertificateActivated", this.ObjectTableName, val);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "BlockAgentBankForMasab", {
        get: function () { return this.entityPM != null ? this.entityPM.BlockAgentBankForMasab : false; },
        set: function (value) { this.entityPM.BlockAgentBankForMasab = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "TehilaDca", {
        get: function () { return this.entityPM != null ? this.entityPM.TehilaDca : false; },
        set: function (value) { this.entityPM.TehilaDca = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "CustomsAgentId", {
        get: function () { return this.entityPM != null ? this.entityPM.CustomsAgentId : null; },
        set: function (value) { this.entityPM.CustomsAgentId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "SignServiceAddress", {
        get: function () { return this.entityPM != null ? this.entityPM.SignServiceAddress : null; },
        set: function (value) { this.entityPM.SignServiceAddress = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "IIGServiceAddress", {
        get: function () { return this.entityPM != null ? this.entityPM.IIGServiceAddress : null; },
        set: function (value) { this.entityPM.IIGServiceAddress = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "DCAServiceAddress", {
        get: function () { return this.entityPM != null ? this.entityPM.DCAServiceAddress : null; },
        set: function (value) { this.entityPM.DCAServiceAddress = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "DCAPartnerVault", {
        get: function () { return this.entityPM != null ? this.entityPM.DCAPartnerVault : null; },
        set: function (value) { this.entityPM.DCAPartnerVault = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "UServerServiceAddress", {
        get: function () { return this.entityPM != null ? this.entityPM.UServerServiceAddress : null; },
        set: function (value) { this.entityPM.UServerServiceAddress = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "DefaultNotificationAssignee", {
        get: function () { return this.entityPM != null ? this.entityPM.DefaultNotificationAssignee : null; },
        set: function (value) { this.entityPM.DefaultNotificationAssignee = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "CustomsEnvoirmentTypeCode", {
        get: function () { return this.entityPM != null ? this.entityPM.CustomsEnvoirmentTypeCode : null; },
        set: function (value) { this.entityPM.CustomsEnvoirmentTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsSettingsComponent.prototype, "UnfConnectionString", {
        get: function () { return this.entityPM != null ? this.entityPM.UnfConnectionString : null; },
        set: function (value) { this.entityPM.UnfConnectionString = value; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CustomsSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var IsNew = false; //itzik : there is a row that come with defualt DB !!!
        if (IsNew) {
            console.error("New Setting Record !!!!!?!?!?!?");
            return;
        }
        //let msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //List < ValidationResult > errors = new List<ValidationResult>();
        //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);
        this._CustomsSettingPMService.update(this.entityPM)
            .subscribe(function (resp) {
            if (resp.HasError) {
                _this.ValidationErrorsList = [];
                _this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                return;
            }
            _this.CancelButtonClicked();
        });
    };
    CustomsSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsSettingsComponent);
    return CustomsSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsSettingsComponent = CustomsSettingsComponent;
//# sourceMappingURL=CustomsSettingsComponent.js.map