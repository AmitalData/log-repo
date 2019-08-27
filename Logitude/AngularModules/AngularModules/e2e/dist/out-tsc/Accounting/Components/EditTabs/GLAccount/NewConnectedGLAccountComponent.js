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
var GLAccountPM_1 = require("../../../EntityPMs/GLAccountPM");
var GLAccountPMService_1 = require("../../../Services/StandardPMs/GLAccountPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var GLAccountCurrencyExtendedPMService_1 = require("../../../Services/ExtendedPMs/GLAccountCurrencyExtendedPMService");
var GLAccountCurrencyPM_1 = require("../../../EntityPMs/GLAccountCurrencyPM");
var NewConnectedGLAccountComponent = /** @class */ (function (_super) {
    __extends(NewConnectedGLAccountComponent, _super);
    function NewConnectedGLAccountComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "GLAccount";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.GLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.gLAccountCurrencyExtendedPMService = new GLAccountCurrencyExtendedPMService_1.GLAccountCurrencyExtendedPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.valid = true;
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        return _this;
    }
    NewConnectedGLAccountComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
            _this.entityPM = args.EntityPM;
            _this.Parent = args.Parent;
            _this.accountPM = new GLAccountPM_1.GLAccountPM();
            _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            _this.UIProperties.SetEnabled("DisplayNumber", "GLAccount", false);
        });
    };
    Object.defineProperty(NewConnectedGLAccountComponent.prototype, "CurrencyId", {
        get: function () { return this.accountPM.CurrencyId; },
        set: function (value) {
            this.UIProperties.SetValidity("CurrencyId", "GLAccount", true, null);
            var currency = this.Parent.ConnectedGLAccounts.Collection.filter(function (d) { return d.CurrencyId == value; })[0];
            if (currency) {
                this.valid = false;
                this.UIProperties.SetValidity("CurrencyId", "GLAccount", false, TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.CurrencyExists"));
            }
            else {
                this.valid = true;
            }
            this.accountPM.CurrencyId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConnectedGLAccountComponent.prototype, "CurrencyCode", {
        get: function () { return this.accountPM.CurrencyCode; },
        set: function (value) {
            this.accountPM.CurrencyCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConnectedGLAccountComponent.prototype, "DisplayNumber", {
        get: function () { return this.entityPM.DisplayNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConnectedGLAccountComponent.prototype, "Currency", {
        get: function () { return this.currency; },
        set: function (value) {
            if (this.currency != value) {
                this.currency = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.CurrencyCode = value.Code;
            }
            else {
                this.CurrencyCode = null;
                this.CurrencyId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewConnectedGLAccountComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    NewConnectedGLAccountComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (!this.CurrencyId) {
            errors.push(this.GetRequierdFieldErrorText("GlAccount.F.CurrencyId"));
        }
        if (!this.valid) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.CurrencyExists"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Saving"));
            this.GLAccountPMService.get(this.entityPM.ControlAccountId).subscribe(function (response) {
                if (response) {
                    if (!response.HasError) {
                        if (response.Result) {
                            _this.accountPM.AccountTypeCode = "2";
                            _this.accountPM.DisplayNumber = _this.entityPM.DisplayNumber + "\\" + _this.CurrencyCode;
                            _this.accountPM.LocalName = _this.entityPM.LocalName + "\\" + _this.CurrencyCode;
                            _this.accountPM.EnglishName = _this.entityPM.EnglishName + "\\" + _this.CurrencyCode;
                            _this.accountPM.IsMultiCurrency = false;
                            _this.accountPM.CurrencyId = _this.CurrencyId;
                            _this.accountPM.RevenueExpenseType = "3";
                            _this.accountPM.CustomerGLAccountId = _this.entityPM.Id;
                            _this.accountPM.IsControlAccount = false;
                            _this.accountPM.ChartOfAccountsId = _this.entityPM.ChartOfAccountsId;
                            _this.accountPM.ChartOfAccountsTypeCode = response.Result.ChartOfAccountsTypeCode;
                            _this.accountPM.ReconcileMethodCode = "1";
                            _this.accountPM.AutomaticReconcileId = _this.entityPM.AutomaticReconcileId;
                            _this.accountPM.ControlAccountId = _this.entityPM.ControlAccountId;
                            _this.accountPM.Tenant = _this.entityPM.Tenant;
                            _this.accountPM.Type = "ADDED";
                            //   this.accountPM.ParentAccountByCurrency = this.entityPM.in;
                            _this.GLAccountPMService.insert(_this.accountPM).subscribe(function (response) {
                                if (response) {
                                    if (!response.HasError) {
                                        var glaccountCurrency = new GLAccountCurrencyPM_1.GLAccountCurrencyPM(_this.accountPM);
                                        glaccountCurrency.CurrencyId = _this.CurrencyId;
                                        glaccountCurrency.MainGLAccountId = _this.entityPM.Id;
                                        glaccountCurrency.GLAccountId = response.Result.Id;
                                        glaccountCurrency.Tenant = _this.entityPM.Tenant;
                                        _this.gLAccountCurrencyExtendedPMService.insert(glaccountCurrency).subscribe(function (response) {
                                            if (response) {
                                                if (!response.HasError) {
                                                    _this.CurrentSession.StopBusyIndicator();
                                                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                                                }
                                                else {
                                                    _this.CurrentSession.StopBusyIndicator();
                                                    _this.ValidationErrorsList = response.ErrorsArray;
                                                }
                                            }
                                        });
                                        //  this.CurrentSession.CloseCurrentWindow();
                                    }
                                    else {
                                        _this.CurrentSession.StopBusyIndicator();
                                        _this.ValidationErrorsList = response.ErrorsArray;
                                    }
                                }
                                //  this.CurrentSession.StopBusyIndicator();
                            });
                        }
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.ControlAccountNotFound"));
                        _this.ValidationErrorsList = errors;
                    }
                }
            });
        }
    };
    NewConnectedGLAccountComponent.prototype.CancelButtonClicked = function () {
        this.accountPM = null;
        this.CurrentSession.CloseCurrentWindow();
    };
    NewConnectedGLAccountComponent.prototype.SubmitChanges = function () {
    };
    NewConnectedGLAccountComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewConnectedGLAccountComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewConnectedGLAccountComponent);
    return NewConnectedGLAccountComponent;
}(BaseComponent_1.BaseComponent));
exports.NewConnectedGLAccountComponent = NewConnectedGLAccountComponent;
//# sourceMappingURL=NewConnectedGLAccountComponent.js.map