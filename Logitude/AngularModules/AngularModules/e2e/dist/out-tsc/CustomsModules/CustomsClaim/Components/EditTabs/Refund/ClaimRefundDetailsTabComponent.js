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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomBankListService_1 = require("../../../../../Customs/Services/StandardLists/CustomBankListService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimRefundDetailsTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRefundDetailsTabComponent, _super);
    function ClaimRefundDetailsTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.Claim";
        _this.banksList = [];
        _this._IsControlEnabled = true;
        _this._IsraelBankFieldsEnabled = false;
        _this._ForeignBankFieldsEnabled = false;
        _this._CustomBankListService = new CustomBankListService_1.CustomBankListService();
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                if (_this.entityArgs.EntityPM != null) {
                    _this.EntityPM = _this.entityArgs.EntityPM;
                    _this.SetBankFieldsEnabled();
                    _this.LoadBanks();
                }
                _this.Listen();
                _this.IsLoaded = true;
            });
        });
        return _this;
    }
    ClaimRefundDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMR") {
                        //this.RefreshEntity();
                    }
                }
            }));
        }
    };
    ClaimRefundDetailsTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRefundDetailsTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this._IsControlEnabled; },
        set: function (newValue) { this._IsControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "IsraelBankFieldsEnabled", {
        get: function () { return this._IsraelBankFieldsEnabled; },
        set: function (newValue) { this._IsraelBankFieldsEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "ForeignBankFieldsEnabled", {
        get: function () { return this._ForeignBankFieldsEnabled; },
        set: function (newValue) { this._ForeignBankFieldsEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "BeneficiaryActivityTypeCode", {
        get: function () { return this.EntityPM.BeneficiaryActivityTypeCode; },
        set: function (newValue) { this.EntityPM.BeneficiaryActivityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "AccountCurrencyTypeCode", {
        get: function () { return this.EntityPM.AccountCurrencyTypeCode; },
        set: function (newValue) { this.EntityPM.AccountCurrencyTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "BeneficiaryExternalID", {
        get: function () { return this.EntityPM.BeneficiaryExternalID; },
        set: function (newValue) { this.EntityPM.BeneficiaryExternalID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "BankTypeCode", {
        get: function () { return this.EntityPM.BankTypeCode; },
        set: function (newValue) { this.EntityPM.BankTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "ForeignBank", {
        get: function () { return this.EntityPM.ForeignBank; },
        set: function (newValue) { this.EntityPM.ForeignBank = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "AccountCountryCode", {
        get: function () { return this.EntityPM.AccountCountryCode; },
        set: function (newValue) {
            this.EntityPM.AccountCountryCode = newValue;
            this.SetBankFieldsEnabled();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "AccountBranchCode", {
        get: function () { return this.EntityPM.AccountBranchCode; },
        set: function (newValue) { this.EntityPM.AccountBranchCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "ForeignBranch", {
        get: function () { return this.EntityPM.ForeignBranch; },
        set: function (newValue) { this.EntityPM.ForeignBranch = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) { this.EntityPM.AccountNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "ForeignAccountNumber", {
        get: function () { return this.EntityPM.ForeignAccountNumber; },
        set: function (newValue) { this.EntityPM.ForeignAccountNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRefundDetailsTabComponent.prototype, "SelectedBankIndex", {
        get: function () { return this._SelectedBankIndex; },
        set: function (value) {
            if (this._SelectedBankIndex != value) {
                this._SelectedBankIndex = value;
                if (value != null) {
                    this.BankTypeCode = value.BankCode;
                    this.AccountBranchCode = value.BranchCode + "," + value.BankCode;
                    this.AccountNumber = value.AccountNumber;
                }
                else {
                    this.BankTypeCode = null;
                    this.AccountBranchCode = null;
                    this.AccountNumber = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ClaimRefundDetailsTabComponent.prototype.LoadBanks = function () {
        var _this = this;
        this._CustomBankListService.getAllFromCache().subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.banksList = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                }
            }
        });
    };
    ClaimRefundDetailsTabComponent.prototype.DeleteBankDetailsCommand = function () {
        this.SelectedBankIndex = null;
        this.BankTypeCode = null;
        this.AccountBranchCode = null;
        this.AccountNumber = null;
    };
    ClaimRefundDetailsTabComponent.prototype.SetBankFieldsEnabled = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.AccountCountryCode)) {
            this.SetAllBankFieldsEnabled();
            return;
        }
        switch (this.AccountCountryCode) {
            case "IL": // Israel
                this.SetIsraelBankFieldsEnabled();
                break;
            default: // Foreign
                this.SetForeignBankFieldsEnabled();
                break;
        }
    };
    ClaimRefundDetailsTabComponent.prototype.SetAllBankFieldsEnabled = function () {
        this.IsraelBankFieldsEnabled = false;
        this.ForeignBankFieldsEnabled = false;
        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, false);
    };
    ClaimRefundDetailsTabComponent.prototype.SetIsraelBankFieldsEnabled = function () {
        this.IsraelBankFieldsEnabled = true;
        this.ForeignBankFieldsEnabled = false;
        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, false);
        this.AccountCurrencyTypeCode = null;
        this.ForeignBank = null;
        this.ForeignBranch = null;
        this.ForeignAccountNumber = null;
    };
    ClaimRefundDetailsTabComponent.prototype.SetForeignBankFieldsEnabled = function () {
        this.IsraelBankFieldsEnabled = false;
        this.ForeignBankFieldsEnabled = true;
        this.UIProperties.SetEnabled("AccountBranchCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AccountCurrencyTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignBank", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignBranch", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ForeignAccountNumber", this.ObjectTableName, true);
        this.DeleteBankDetailsCommand();
    };
    ClaimRefundDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRefundDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimRefundDetailsTabComponent);
    return ClaimRefundDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRefundDetailsTabComponent = ClaimRefundDetailsTabComponent;
//# sourceMappingURL=ClaimRefundDetailsTabComponent.js.map