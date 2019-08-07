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
var QuoteSettingPM_1 = require("../../../../Quote/EntityPMs/QuoteSettingPM");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var QuoteSettingsComponent = /** @class */ (function (_super) {
    __extends(QuoteSettingsComponent, _super);
    function QuoteSettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "QuoteSetting";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.SaleCurrencySettings = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaleCurrencySettings.push(new CodeNameClass_1.CodeNameClass("F", "Fixed"));
        _this.SaleCurrencySettings.push(new CodeNameClass_1.CodeNameClass("S", "Same as cost currency"));
        _this.myService = new QuoteDomainService_1.QuoteDomainService();
        _this.entityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (res1) {
            _this.myService.GetQuoteSettings().subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    if (!_this.EntityPM) {
                        _this.EntityPM = new QuoteSettingPM_1.QuoteSettingPM();
                        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    }
                    if (_this.EntityPM.IsSaleAsCostCurrency) {
                        _this.selectedSaleCurrencySetting = _this.SaleCurrencySettings.filter(function (f) { return f.Code == "S"; })[0];
                    }
                    else {
                        _this.selectedSaleCurrencySetting = _this.SaleCurrencySettings.filter(function (f) { return f.Code == "F"; })[0];
                    }
                    _this.IsResourcesReady = true;
                }
            });
        });
        return _this;
    }
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyShipper", {
        get: function () { return this.EntityPM.CopyShipper; },
        set: function (value) {
            if (this.EntityPM.CopyShipper != value) {
                this.EntityPM.CopyShipper = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyConsignee", {
        get: function () { return this.EntityPM.CopyConsignee; },
        set: function (value) {
            if (this.EntityPM.CopyConsignee != value) {
                this.EntityPM.CopyConsignee = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyAgent", {
        get: function () { return this.EntityPM.CopyAgent; },
        set: function (value) {
            if (this.EntityPM.CopyAgent != value) {
                this.EntityPM.CopyAgent = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyNotify", {
        get: function () { return this.EntityPM.CopyNotify; },
        set: function (value) {
            if (this.EntityPM.CopyNotify != value) {
                this.EntityPM.CopyNotify = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyMainCarriage", {
        get: function () { return this.EntityPM.CopyMainCarriage; },
        set: function (value) {
            if (this.EntityPM.CopyMainCarriage != value) {
                this.EntityPM.CopyMainCarriage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyPickup", {
        get: function () { return this.EntityPM.CopyPickup; },
        set: function (value) {
            if (this.EntityPM.CopyPickup != value) {
                this.EntityPM.CopyPickup = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyDelivery", {
        get: function () { return this.EntityPM.CopyDelivery; },
        set: function (value) {
            if (this.EntityPM.CopyDelivery != value) {
                this.EntityPM.CopyDelivery = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyChargesTypes", {
        get: function () { return this.EntityPM.CopyChargesTypes; },
        set: function (value) {
            if (this.EntityPM.CopyChargesTypes != value) {
                this.EntityPM.CopyChargesTypes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyChargesCost", {
        get: function () { return this.EntityPM.CopyChargesCost; },
        set: function (value) {
            if (this.EntityPM.CopyChargesCost != value) {
                this.EntityPM.CopyChargesCost = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "CopyChargesSale", {
        get: function () { return this.EntityPM.CopyChargesSale; },
        set: function (value) {
            if (this.EntityPM.CopyChargesSale != value) {
                this.EntityPM.CopyChargesSale = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "EditMainCarriage", {
        get: function () { return this.EntityPM.EditMainCarriage; },
        set: function (value) {
            if (this.EntityPM.EditMainCarriage != value) {
                this.EntityPM.EditMainCarriage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "IsSaleAsCostCurrency", {
        get: function () { return this.EntityPM.IsSaleAsCostCurrency; },
        set: function (value) {
            if (this.EntityPM.IsSaleAsCostCurrency != value) {
                this.EntityPM.IsSaleAsCostCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteSettingsComponent.prototype, "SelectedSaleCurrencySetting", {
        get: function () { return this.selectedSaleCurrencySetting; },
        set: function (value) {
            if (this.selectedSaleCurrencySetting != value) {
                this.selectedSaleCurrencySetting = value;
                var isSaleAsCostCurrency = false;
                if (value) {
                    if (value.Code == "S") {
                        isSaleAsCostCurrency = true;
                    }
                }
                this.IsSaleAsCostCurrency = isSaleAsCostCurrency;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    QuoteSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var isAnyOptionChecked = this.ValidateAnyOptionIsChecked();
        if (isAnyOptionChecked == false) {
            errors.push("One Option at least  should be selected");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.UpdateQuoteSettings(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    QuoteSettingsComponent.prototype.ValidateAnyOptionIsChecked = function () {
        var myResult = false;
        if (this.CopyShipper) {
            myResult = true;
        }
        else if (this.CopyConsignee) {
            myResult = true;
        }
        else if (this.CopyAgent) {
            myResult = true;
        }
        else if (this.CopyNotify) {
            myResult = true;
        }
        else if (this.CopyMainCarriage) {
            myResult = true;
        }
        else if (this.CopyPickup) {
            myResult = true;
        }
        else if (this.CopyDelivery) {
            myResult = true;
        }
        else if (this.CopyChargesTypes) {
            myResult = true;
        }
        else if (this.CopyChargesCost) {
            myResult = true;
        }
        else if (this.CopyChargesSale) {
            myResult = true;
        }
        else if (this.EditMainCarriage) {
            myResult = true;
        }
        return myResult;
    };
    QuoteSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuoteSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], QuoteSettingsComponent);
    return QuoteSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.QuoteSettingsComponent = QuoteSettingsComponent;
//# sourceMappingURL=QuoteSettingsComponent.js.map