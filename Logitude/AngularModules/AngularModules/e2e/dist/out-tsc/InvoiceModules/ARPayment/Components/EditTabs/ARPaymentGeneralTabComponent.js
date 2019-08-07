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
var Tools_1 = require("../../../../Infrastructure/Tools");
var ARPaymentGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ARPaymentGeneralTabComponent, _super);
    function ARPaymentGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "ARPayment";
        // public TenantPM: TenantPM;
        _this.LabelColumnWidth = 100;
        _this.ControlColumnWidth = 200;
        _this.DataContext = _this;
        _this.ScreenCode = "ARPayment.GeneralTabScreen";
        _this.DisplaySATSettings = false;
        _this.Retries = 0;
        _this.TipoCadenaPagoList = [];
        _this.selectedTipoCadenaPago = null;
        _this.EntityPM = entityArgs.EntityPM;
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
            if (_this.EntityPM.PaymentInvoices.length > 0 && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.MetodoPagoCode)) {
                _this.EntityPM.UIProperties.SetEnabled("MetodoPagoCode", _this.ObjectTableName, false);
            }
        }
        _this.FillTipoCadenaPagoList();
        //this.TenantPM = SessionLocator.TenantPM;
        _this.RunComponent();
        return _this;
    }
    ARPaymentGeneralTabComponent.prototype.ngOnInit = function () {
    };
    ARPaymentGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ARPaymentGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ARPaymentGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "SATPaymentMethodCode", {
        // Properties 
        get: function () { return this.EntityPM.SATPaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.SATPaymentMethodCode != newValue) {
                this.EntityPM.SATPaymentMethodCode = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentGeneralTabComponent.prototype.FillTipoCadenaPagoList = function () {
        this.TipoCadenaPagoList.push({ Code: null, Name: null });
        this.TipoCadenaPagoList.push({ Code: "01", Name: "SPEI (Electronic Payment System between Banks)" });
    };
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "SelectedTipoCadenaPago", {
        get: function () {
            var tipoCadenaPago = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TipoCadenaPago)) {
                tipoCadenaPago = this.TipoCadenaPago.toUpperCase();
            }
            switch (tipoCadenaPago) {
                case "01":
                    {
                        this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(function (d) { return d.Code == tipoCadenaPago; })[0];
                        break;
                    }
                default:
                    {
                        this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(function (d) { return d.Code == null; })[0];
                        break;
                    }
            }
            return this.selectedTipoCadenaPago;
        },
        set: function (newValue) {
            if (this.selectedTipoCadenaPago != newValue) {
                this.selectedTipoCadenaPago = newValue;
                if (newValue == null) {
                    this.TipoCadenaPago = null;
                }
                else {
                    this.TipoCadenaPago = newValue.Code;
                }
            }
            this.ValidateTipoCadenaPagoFields();
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentGeneralTabComponent.prototype.ValidateTipoCadenaPagoFields = function () {
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.CertPago))
                    this.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(this.CadPago))
                    this.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(this.SelloPago))
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    };
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "TipoCadenaPago", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.TipoCadenaPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.EntityPM.TipoCadenaPago != newValue) {
                this.EntityPM.TipoCadenaPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "CadPago", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.CadPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.EntityPM.CadPago != newValue) {
                this.EntityPM.CadPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "CertPago", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.CertPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.EntityPM.CertPago != newValue) {
                this.EntityPM.CertPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentGeneralTabComponent.prototype, "SelloPago", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.SelloPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.EntityPM.SelloPago != newValue) {
                this.EntityPM.SelloPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ARPaymentGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ARPaymentGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARPaymentGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARPaymentGeneralTabComponent);
    return ARPaymentGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ARPaymentGeneralTabComponent = ARPaymentGeneralTabComponent;
var TipoCadenaPagoClass = /** @class */ (function () {
    function TipoCadenaPagoClass() {
    }
    return TipoCadenaPagoClass;
}());
//# sourceMappingURL=ARPaymentGeneralTabComponent.js.map