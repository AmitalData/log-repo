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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var SystemCurrenciesComponent = /** @class */ (function (_super) {
    __extends(SystemCurrenciesComponent, _super);
    function SystemCurrenciesComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.TenantPM = null;
        _this.IsResourcesReady = false;
        _this.DemoMessageVisibility = false;
        _this.ShipmentsQuotesCount = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = false;
        _this.entityPMService = new TenantPMService_1.TenantPMService();
        entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (res) {
            _this.GetDemoMessageVisibility();
            _this.entityPMService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.TenantPM = myResponse.Result;
                    var myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
                    myShipmentDomainService.GetShipmentsQuotesCount().subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            _this.ShipmentsQuotesCount = myResponse2.Result;
                        }
                        _this.SetUIProperties();
                        _this.IsResourcesReady = true;
                    });
                }
            });
        });
        return _this;
    }
    SystemCurrenciesComponent.prototype.GetDemoMessageVisibility = function () {
        var myResult = false;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            myResult = true;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                myResult = false;
            }
        }
        this.DemoMessageVisibility = myResult;
    };
    SystemCurrenciesComponent.prototype.SetUIProperties = function () {
        var isFeildEnabled = true;
        if (this.TenantPM.Id == 65) {
            isFeildEnabled = false;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                isFeildEnabled = true;
            }
        }
        this.IsEditingEnabled = isFeildEnabled;
        this.UIProperties.SetEnabled("CurrencyId", "Tenant", false);
        this.UIProperties.SetEnabled("ProfitCurrencyId", "Tenant", this.ShipmentsQuotesCount == 0 ? true : false);
        this.UIProperties.SetEnabled("FreightCurrencyId", "Tenant", isFeildEnabled);
        this.UIProperties.SetEnabled("OtherChargesCurrencyId", "Tenant", isFeildEnabled);
        this.UIProperties.SetEnabled("QuoteSaleCurrencyId", "Tenant", isFeildEnabled);
    };
    Object.defineProperty(SystemCurrenciesComponent.prototype, "CurrencyId", {
        get: function () { return this.TenantPM.CurrencyId; },
        set: function (value) {
            if (this.TenantPM.CurrencyId != value) {
                this.TenantPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemCurrenciesComponent.prototype, "FreightCurrencyId", {
        get: function () { return this.TenantPM.FreightCurrencyId; },
        set: function (value) {
            if (this.TenantPM.FreightCurrencyId != value) {
                this.TenantPM.FreightCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemCurrenciesComponent.prototype, "OtherChargesCurrencyId", {
        get: function () { return this.TenantPM.OtherChargesCurrencyId; },
        set: function (value) {
            if (this.TenantPM.OtherChargesCurrencyId != value) {
                this.TenantPM.OtherChargesCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemCurrenciesComponent.prototype, "QuoteSaleCurrencyId", {
        get: function () { return this.TenantPM.QuoteSaleCurrencyId; },
        set: function (value) {
            if (this.TenantPM.QuoteSaleCurrencyId != value) {
                this.TenantPM.QuoteSaleCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemCurrenciesComponent.prototype, "ProfitCurrencyId", {
        get: function () { return this.TenantPM.ProfitCurrencyId; },
        set: function (value) {
            if (this.TenantPM.ProfitCurrencyId != value) {
                this.TenantPM.ProfitCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SystemCurrenciesComponent.prototype.DailyExchangeRates = function () {
        var windowTitle = "Edit exchange rates";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        this.entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(function (response) {
            logWindow.Show('./Common/Components/Maintenance/RatesMainTabComponent');
        });
    };
    SystemCurrenciesComponent.prototype.EditCurrency = function () {
        var _this = this;
        if (this.IsEditingEnabled) {
            var windowTitle = "Edit Accounting Currency";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.Width = 650;
            logWindow.Height = 450;
            logWindow.WindowArgs = { EntityPM: this.TenantPM, ShipmentsQuotesCount: this.ShipmentsQuotesCount };
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.TenantPM = comp.EntityPM;
                    }
                });
            });
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/CurrencyRatesComponent');
        }
    };
    SystemCurrenciesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SystemCurrenciesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.TenantPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.TenantPM, this.ObjectTableName, errors);
            if (Tools_1.AppTool.IsNullOrEmpty(this.CurrencyId)) {
                errors.push("Accounting Currency Is Required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ProfitCurrencyId)) {
                errors.push("Profit Currency Is Required");
            }
            this.ValidationErrorsList = errors;
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.entityPMService.update(this.TenantPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        InfraSettings_1.InfraSettings.TenantPM = myResponse.Result;
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    };
    SystemCurrenciesComponent = __decorate([
        core_1.Component({
            selector: 'SystemCurrenciesComponent',
            moduleId: module.id,
            templateUrl: './SystemCurrenciesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SystemCurrenciesComponent);
    return SystemCurrenciesComponent;
}(BaseComponent_1.BaseComponent));
exports.SystemCurrenciesComponent = SystemCurrenciesComponent;
//# sourceMappingURL=SystemCurrenciesComponent.js.map