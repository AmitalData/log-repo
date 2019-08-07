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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BillingTabComponent = /** @class */ (function (_super) {
    __extends(BillingTabComponent, _super);
    function BillingTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DisplaySATSettings = false;
        _this.DataContext = _this;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.Retries = 0;
        _this.ScreenCode = entityArgs.ObjectTableName + ".BillingTabScreen";
        _this.RunComponent();
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        return _this;
    }
    BillingTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    };
    BillingTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    BillingTabComponent.prototype.RunComponent = function () {
        var _this = this;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.viewContainerRef) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
            });
            this.Listen();
        }
        else {
            this.RunComponentTimer();
        }
    };
    BillingTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(BillingTabComponent.prototype, "PaymentMethodCode", {
        get: function () { return this.entityArgs.EntityPM.PaymentMethodCode; },
        set: function (newValue) {
            if (this.entityArgs.EntityPM.PaymentMethodCode != newValue) {
                this.entityArgs.EntityPM.PaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BillingTabComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.EntityPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.EntityPM.MetodoPagoCode != newValue) {
                this.EntityPM.MetodoPagoCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BillingTabComponent.prototype, "UsoCFDICode", {
        get: function () { return this.EntityPM.UsoCFDICode; },
        set: function (newValue) {
            if (this.EntityPM.UsoCFDICode != newValue) {
                this.EntityPM.UsoCFDICode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BillingTabComponent.prototype, "SATForeignRFC", {
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
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], BillingTabComponent.prototype, "viewContainerRef", void 0);
    BillingTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BillingTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BillingTabComponent);
    return BillingTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BillingTabComponent = BillingTabComponent;
//# sourceMappingURL=BillingTabComponent.js.map