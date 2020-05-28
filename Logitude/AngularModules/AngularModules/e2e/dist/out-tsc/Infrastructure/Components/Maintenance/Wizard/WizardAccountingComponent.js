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
var BaseComponent_1 = require("../../LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Tools");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var TextCodeTranslator_1 = require("../../../Utilities/TextCodeTranslator");
var WizardAccountingComponent = /** @class */ (function (_super) {
    __extends(WizardAccountingComponent, _super);
    function WizardAccountingComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        return _this;
    }
    WizardAccountingComponent.prototype.InitializeComponent = function (tenantPM) {
        var _this = this;
        this.EntityPM = tenantPM;
        if (Tools_1.AppTool.IsNullOrEmpty(this.PaymentTermId)) {
            var myService = new PaymentTermListService_1.PaymentTermListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var data = myResponse.Result;
                    var list = data.filter(function (f) { return f.EnglishName == "Net 30"; })[0];
                    if (list) {
                        _this.PaymentTermId = list.Id;
                    }
                    else {
                        myService.getAll().subscribe(function (myResponse2) {
                            if (!myResponse2.HasError) {
                                data = myResponse.Result;
                                list = data.filter(function (f) { return f.EnglishName == "Net 30"; })[0];
                                if (list) {
                                    _this.PaymentTermId = list.Id;
                                }
                            }
                        });
                    }
                }
            });
        }
    };
    WizardAccountingComponent.prototype.Validate = function (errors) {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.CurrencyId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ProfitCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.ProfitCurrencyId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ProfitCurrencyRate) || this.ProfitCurrencyRate <= 0) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.ProfitCurrencyRate")));
        }
        return errors;
    };
    Object.defineProperty(WizardAccountingComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                var myRate = null;
                var isRateEnabled = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (value == this.ProfitCurrencyId) {
                        myRate = 1;
                        isRateEnabled = false;
                    }
                }
                this.ProfitCurrencyRate = myRate;
                this.UIProperties.SetEnabled("ProfitCurrencyRate", this.ObjectTableName, isRateEnabled);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAccountingComponent.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyId != value) {
                this.EntityPM.ProfitCurrencyId = value;
                var myRate = null;
                var isRateEnabled = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (value == this.CurrencyId) {
                        myRate = 1;
                        isRateEnabled = false;
                    }
                }
                this.ProfitCurrencyRate = myRate;
                this.UIProperties.SetEnabled("ProfitCurrencyRate", this.ObjectTableName, isRateEnabled);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAccountingComponent.prototype, "ProfitCurrencyRate", {
        get: function () { return this.EntityPM.ProfitCurrencyRate; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyRate != value) {
                this.EntityPM.ProfitCurrencyRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAccountingComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM.VatNumber; },
        set: function (value) {
            if (this.EntityPM.VatNumber != value) {
                this.EntityPM.VatNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAccountingComponent.prototype, "STDVatPercentage", {
        get: function () { return this.EntityPM.STDVatPercentage; },
        set: function (value) {
            if (this.EntityPM.STDVatPercentage != value) {
                this.EntityPM.STDVatPercentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAccountingComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
        set: function (value) {
            if (this.EntityPM.PaymentTermId != value) {
                this.EntityPM.PaymentTermId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardAccountingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardAccountingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WizardAccountingComponent);
    return WizardAccountingComponent;
}(BaseComponent_1.BaseComponent));
exports.WizardAccountingComponent = WizardAccountingComponent;
//# sourceMappingURL=WizardAccountingComponent.js.map