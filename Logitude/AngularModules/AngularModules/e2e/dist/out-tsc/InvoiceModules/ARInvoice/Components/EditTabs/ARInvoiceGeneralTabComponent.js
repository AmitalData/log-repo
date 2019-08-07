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
var ARInvoiceGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ARInvoiceGeneralTabComponent, _super);
    function ARInvoiceGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "ARInvoice";
        // public TenantPM: TenantPM;
        _this.LabelColumnWidth = 100;
        _this.ControlColumnWidth = 200;
        _this.DataContext = _this;
        _this.ScreenCode = "ARInvoice.GeneralTabScreen";
        _this.DisplaySATSettings = false;
        _this.Retries = 0;
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        _this.EntityPM = entityArgs.EntityPM;
        //this.TenantPM = SessionLocator.TenantPM;
        _this.RunComponent();
        return _this;
    }
    ARInvoiceGeneralTabComponent.prototype.ngOnInit = function () {
    };
    ARInvoiceGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ARInvoiceGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ARInvoiceGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    Object.defineProperty(ARInvoiceGeneralTabComponent.prototype, "SATPaymentMethodCode", {
        // Properties 
        get: function () { return this.EntityPM.SATPaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.SATPaymentMethodCode != newValue) {
                this.EntityPM.SATPaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceGeneralTabComponent.prototype, "RelatedInvoice", {
        get: function () { return this.EntityPM.RelatedInvoice; },
        set: function (newValue) {
            if (this.EntityPM.RelatedInvoice != newValue) {
                this.EntityPM.RelatedInvoice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceGeneralTabComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.EntityPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.EntityPM.MetodoPagoCode != newValue) {
                this.EntityPM.MetodoPagoCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceGeneralTabComponent.prototype, "UsoCFDICode", {
        get: function () { return this.EntityPM.UsoCFDICode; },
        set: function (newValue) {
            if (this.EntityPM.UsoCFDICode != newValue) {
                this.EntityPM.UsoCFDICode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ARInvoiceGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ARInvoiceGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceGeneralTabComponent);
    return ARInvoiceGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceGeneralTabComponent = ARInvoiceGeneralTabComponent;
//# sourceMappingURL=ARInvoiceGeneralTabComponent.js.map