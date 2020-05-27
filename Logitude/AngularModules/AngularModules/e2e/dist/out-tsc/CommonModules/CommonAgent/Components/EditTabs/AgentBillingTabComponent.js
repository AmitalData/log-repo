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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var AgentBillingTabComponent = /** @class */ (function (_super) {
    __extends(AgentBillingTabComponent, _super);
    function AgentBillingTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Agent";
        _this.DataContext = _this;
        _this.HasCreditLimitFeature = false;
        _this.IsCreditLimitActivated = false;
        _this.DisplaySATSettings = false;
        _this.Retries = 0;
        _this.EntityPM = entityArgs.EntityPM;
        _this.HasCreditLimitFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        if (_this.HasCreditLimitFeature) {
            _this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        return _this;
    }
    AgentBillingTabComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
        this.RunComponent();
    };
    AgentBillingTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    AgentBillingTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AgentBillingTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, "Agent.BillingTabScreen");
        });
    };
    AgentBillingTabComponent.prototype.SetUIProperties = function () {
        var isFieldActivated = false;
        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    isFieldActivated = true;
                }
            }
        }
        this.UIProperties.SetEnabled("BlockNewInvoiceCreation", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewShipmentCreation", this.ObjectTableName, isFieldActivated);
    };
    Object.defineProperty(AgentBillingTabComponent.prototype, "IsCreditLimitEnabled", {
        get: function () { return this.EntityPM.IsCreditLimitEnabled; },
        set: function (value) {
            if (this.EntityPM.IsCreditLimitEnabled != value) {
                this.EntityPM.IsCreditLimitEnabled = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "BlockNewInvoiceCreation", {
        get: function () { return this.EntityPM.BlockNewInvoiceCreation; },
        set: function (value) {
            if (this.EntityPM.BlockNewInvoiceCreation != value) {
                this.EntityPM.BlockNewInvoiceCreation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "BlockNewShipmentCreation", {
        get: function () { return this.EntityPM.BlockNewShipmentCreation; },
        set: function (value) {
            if (this.EntityPM.BlockNewShipmentCreation != value) {
                this.EntityPM.BlockNewShipmentCreation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "PaymentMethodCode", {
        get: function () { return this.EntityPM.PaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.PaymentMethodCode != newValue) {
                this.EntityPM.PaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.EntityPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.EntityPM.MetodoPagoCode != newValue) {
                this.EntityPM.MetodoPagoCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "UsoCFDICode", {
        get: function () { return this.EntityPM.UsoCFDICode; },
        set: function (newValue) {
            if (this.EntityPM.UsoCFDICode != newValue) {
                this.EntityPM.UsoCFDICode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentBillingTabComponent.prototype, "SATForeignRFC", {
        get: function () { return this.EntityPM.SATForeignRFC; },
        set: function (newValue) {
            if (this.EntityPM.SATForeignRFC != newValue) {
                this.EntityPM.SATForeignRFC = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('BillingChild', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AgentBillingTabComponent.prototype, "viewContainerRef", void 0);
    AgentBillingTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgentBillingTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AgentBillingTabComponent);
    return AgentBillingTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AgentBillingTabComponent = AgentBillingTabComponent;
//# sourceMappingURL=AgentBillingTabComponent.js.map