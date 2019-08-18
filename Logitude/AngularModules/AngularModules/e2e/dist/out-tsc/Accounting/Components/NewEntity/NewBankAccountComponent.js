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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var BankAccountPM_1 = require("../../EntityPMs/BankAccountPM");
var BankAccountPMService_1 = require("../../Services/StandardPMs/BankAccountPMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var NewBankAccountComponent = /** @class */ (function (_super) {
    __extends(NewBankAccountComponent, _super);
    function NewBankAccountComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "BankAccount";
        _this.ValidationErrorsList = [];
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = new BankAccountPM_1.BankAccountPM();
        _this.EntityPM.Inactive = false;
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.myService = new BankAccountPMService_1.BankAccountPMService();
        _this.SetUIProperties();
        _this.SelectDefaultValues();
        _this.InitLOVFilters();
        return _this;
    }
    NewBankAccountComponent.prototype.InitLOVFilters = function () {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("ChartOfAccountsTypeCode", "5", null, null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", false, null, null, "Equals", false, false, false, "string");
    };
    Object.defineProperty(NewBankAccountComponent.prototype, "AccountNumber", {
        //#region Properties
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (value) {
            if (this.EntityPM.AccountNumber != value) {
                this.EntityPM.AccountNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "GLAccountId", {
        get: function () { return this.EntityPM.GLAccountId; },
        set: function (value) {
            if (this.EntityPM.GLAccountId != value) {
                this.EntityPM.GLAccountId = value;
                //if (!this.DeferredGLAccountId)
                //this.DeferredGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "DeferredGLAccountId", {
        get: function () { return this.EntityPM.DeferredGLAccountId; },
        set: function (value) {
            if (this.EntityPM.DeferredGLAccountId != value) {
                this.EntityPM.DeferredGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "TransferGLAcccountId", {
        get: function () { return this.EntityPM.TransferGLAcccountId; },
        set: function (value) {
            if (this.EntityPM.TransferGLAcccountId != value) {
                this.EntityPM.TransferGLAcccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "BankId", {
        get: function () { return this.EntityPM.BankId; },
        set: function (value) {
            if (this.EntityPM.BankId != value) {
                this.EntityPM.BankId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "BranchNumber", {
        get: function () { return this.EntityPM.BranchNumber; },
        set: function (value) {
            if (this.EntityPM.BranchNumber != value) {
                this.EntityPM.BranchNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "GLAccount", {
        get: function () { return this.glAccount; },
        set: function (value) {
            if (this.glAccount != value) {
                this.glAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "DeferredGLAccount", {
        get: function () { return this.deferredGLAccount; },
        set: function (value) {
            if (this.deferredGLAccount != value) {
                this.deferredGLAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "TransferGLAcccount", {
        get: function () { return this.transferGLAcccount; },
        set: function (value) {
            if (this.transferGLAcccount != value) {
                this.transferGLAcccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankAccountComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewBankAccountComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        //this.EntityPM.EnglishName = "­­ ";
        //this.EntityPM.BranchAddress = "­­ ";
        //}
        // Class Validator
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        // Custom Validation
        if (!Tools_1.AppTool.IsNullOrEmpty(this.GLAccount) && !Tools_1.AppTool.IsNullOrEmpty(this.DeferredGLAccount)) {
            if (this.GLAccount.IsMultiCurrency || this.DeferredGLAccount.IsMultiCurrency) {
                errors.push("The GLAccount and Difffered GLAccount must be single currency");
            }
            else if (this.GLAccount.CurrencyId != this.DeferredGLAccount.CurrencyId) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("BankAccounts.O.CurrencyGLAccountAndDefferredMustSame")); // The currencies of the GLAccount must be the same
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    NewBankAccountComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBankAccountComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewBankAccountComponent.prototype.SetUIProperties = function () {
        //this.UIProperties.SetEnabled("BankAccountsId", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    };
    NewBankAccountComponent.prototype.SelectDefaultValues = function () {
        this.EntityPM.Inactive = false;
    };
    NewBankAccountComponent = __decorate([
        core_1.Component({
            selector: 'NewBankAccountComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './NewBankAccountComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], NewBankAccountComponent);
    return NewBankAccountComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBankAccountComponent = NewBankAccountComponent;
//# sourceMappingURL=NewBankAccountComponent.js.map