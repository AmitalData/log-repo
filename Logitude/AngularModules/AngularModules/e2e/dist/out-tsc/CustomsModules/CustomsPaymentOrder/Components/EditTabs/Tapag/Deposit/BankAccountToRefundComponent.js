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
var CustomMessageWrapperComponent_1 = require("../../../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityArgs_1 = require("../../../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var DepositPM_1 = require("../../../../../../Customs/EntityPMs/DepositPM");
var CustomBankListService_1 = require("../../../../../../Customs/Services/StandardLists/CustomBankListService");
var BankAccountToRefundRequestParams_1 = require("../../../../../../Customs/DataContract/RequestParams/BankAccountToRefundRequestParams");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var TapagMessagesService_1 = require("../../../../../../Customs/Services/WebServices/TapagMessagesService");
var BankAccountToRefundComponent = /** @class */ (function (_super) {
    __extends(BankAccountToRefundComponent, _super);
    function BankAccountToRefundComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = new DepositPM_1.DepositPM();
        _this.ObjectTableName = "Customs.Deposit";
        _this.DataContext = _this;
        _this._TapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this._CustomBankListService = new CustomBankListService_1.CustomBankListService();
        _this.banksList = [];
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._IsraelBankFieldsEnabled = false;
        _this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe(function (response) {
                    _this.LoadBanks();
                });
            });
        }
        return _this;
    }
    BankAccountToRefundComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    BankAccountToRefundComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new BankAccountToRefundRequestParams_1.BankAccountToRefundRequestParams();
            this.UIProperties.SetRequired("FileTypeCode", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
            this.UIProperties.SetRequired("IdentifierType", null, true);
            this.UIProperties.SetRequired("IdentifierCode", null, true);
            this.UIProperties.SetRequired("CountryCode", null, true);
            this.UIProperties.SetRequired("BankCode", null, true);
            this.UIProperties.SetRequired("AccountBranch", null, true);
            this.UIProperties.SetRequired("AccountNumber", null, true);
        }
    };
    BankAccountToRefundComponent.prototype.SetMenuArg = function (MenuArg) {
        this.OnMassageDisplayMethod();
        this.declarationId = MenuArg.DeclarationId;
    };
    Object.defineProperty(BankAccountToRefundComponent.prototype, "FileTypeCode", {
        get: function () { return this.RequestParams ? this.RequestParams.FileType : null; },
        set: function (value) {
            if (this.RequestParams.FileType != value) {
                this.RequestParams.FileType = value;
            }
            if (value) {
                this.UIProperties.SetRequired("FileTypeCode", null, false);
            }
            else {
                this.UIProperties.SetRequired("FileTypeCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "FileNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.FileNumber : null; },
        set: function (value) {
            if (this.RequestParams.FileNumber != value) {
                this.RequestParams.FileNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("FileNumber", null, false);
            }
            else {
                this.UIProperties.SetRequired("FileNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "Numeral", {
        get: function () { return this.RequestParams ? this.RequestParams.Numeral : null; },
        set: function (value) {
            if (this.RequestParams.Numeral != value) {
                this.RequestParams.Numeral = value;
            }
            if (value) {
                this.UIProperties.SetRequired("Numeral", null, false);
            }
            else {
                this.UIProperties.SetRequired("Numeral", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "IdentifierType", {
        get: function () { return this.RequestParams ? this.RequestParams.IdentifierType : null; },
        set: function (value) {
            if (this.RequestParams.IdentifierType != value) {
                this.RequestParams.IdentifierType = value;
            }
            if (value) {
                this.UIProperties.SetRequired("IdentifierType", null, false);
            }
            else {
                this.UIProperties.SetRequired("IdentifierType", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "IdentifierCode", {
        get: function () { return this.RequestParams ? this.RequestParams.IdentifierCode : null; },
        set: function (value) {
            if (this.RequestParams.IdentifierCode != value) {
                this.RequestParams.IdentifierCode = value;
            }
            if (value) {
                this.UIProperties.SetRequired("IdentifierCode", null, false);
            }
            else {
                this.UIProperties.SetRequired("IdentifierCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "CountryCode", {
        get: function () { return this.RequestParams ? this.RequestParams.CountryCode : null; },
        set: function (value) {
            if (this.RequestParams.CountryCode != value) {
                this.RequestParams.CountryCode = value;
            }
            if (value) {
                this.UIProperties.SetRequired("CountryCode", null, false);
            }
            else {
                this.UIProperties.SetRequired("CountryCode", null, true);
            }
            this.SetBankFieldsEnabled();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "InternalBankId", {
        get: function () { return this._InternalBank; },
        set: function (newValue) { this._InternalBank = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "BankCode", {
        get: function () { return this.RequestParams ? this.RequestParams.BankCode : null; },
        set: function (value) {
            if (this.RequestParams.BankCode != value) {
                this.RequestParams.BankCode = value;
            }
            if (value) {
                this.UIProperties.SetRequired("BankCode", null, false);
            }
            else {
                this.UIProperties.SetRequired("BankCode", null, true);
            }
            this.AccountBranch = null;
            this.AccountNumber = null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "AccountBranch", {
        get: function () { return this.RequestParams ? this.RequestParams.AccountBranch : null; },
        set: function (value) {
            if (this.RequestParams.AccountBranch != value) {
                this.RequestParams.AccountBranch = value;
            }
            if (value) {
                this.UIProperties.SetRequired("AccountBranch", null, false);
            }
            else {
                this.UIProperties.SetRequired("AccountBranch", null, true);
            }
            this.AccountNumber = null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "AccountNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.AccountNumber : null; },
        set: function (value) {
            if (this.RequestParams.AccountNumber != value) {
                this.RequestParams.AccountNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("AccountNumber", null, false);
            }
            else {
                this.UIProperties.SetRequired("AccountNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "AccountCurrency", {
        get: function () { return this.RequestParams ? this.RequestParams.AccountCurrency : null; },
        set: function (value) {
            if (this.RequestParams.AccountCurrency != value) {
                this.RequestParams.AccountCurrency = value;
            }
            this.UIProperties.SetRequired("AccountCurrency", null, false);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CountryCode) && this.CountryCode != "IL") {
                if (value) {
                    this.UIProperties.SetRequired("AccountCurrency", null, false);
                }
                else {
                    this.UIProperties.SetRequired("AccountCurrency", null, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountToRefundComponent.prototype, "IsraelBankFieldsEnabled", {
        get: function () { return this._IsraelBankFieldsEnabled; },
        set: function (newValue) { this._IsraelBankFieldsEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    BankAccountToRefundComponent.prototype.SetBankFieldsEnabled = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryCode)) {
            this.SetIsraelBankFieldsDisabled();
            return;
        }
        this.InternalBankId = null;
        this.SelectedBankIndex = null;
        this.BankCode = null;
        this.AccountBranch = null;
        this.AccountNumber = null;
        this.AccountCurrency = null;
        switch (this.CountryCode) {
            case "IL": // Israel
                this.SetIsraelBankFieldsEnabled();
                break;
            default: // Foreign
                this.SetIsraelBankFieldsDisabled();
                break;
        }
    };
    BankAccountToRefundComponent.prototype.SetIsraelBankFieldsDisabled = function () {
        this.IsraelBankFieldsEnabled = false;
        this.UIProperties.SetEnabled("InternalBankId", null, false);
        this.UIProperties.SetEnabled("AccountCurrency", null, true);
        this.UIProperties.SetRequired("AccountCurrency", null, true);
    };
    BankAccountToRefundComponent.prototype.SetIsraelBankFieldsEnabled = function () {
        this.IsraelBankFieldsEnabled = true;
        this.UIProperties.SetEnabled("InternalBankId", null, true);
        this.UIProperties.SetEnabled("AccountCurrency", null, false);
        this.UIProperties.SetRequired("AccountCurrency", null, false);
    };
    Object.defineProperty(BankAccountToRefundComponent.prototype, "SelectedBankIndex", {
        get: function () { return this._SelectedBankIndex; },
        set: function (value) {
            if (this._SelectedBankIndex != value) {
                this._SelectedBankIndex = value;
                if (value != null) {
                    this.BankCode = value.BankCode;
                    this.AccountBranch = Number(value.BranchCode).toString() + "," + value.BankCode;
                    this.AccountNumber = value.AccountNumber;
                }
                else {
                    this.BankCode = null;
                    this.AccountBranch = null;
                    this.AccountNumber = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    BankAccountToRefundComponent.prototype.LoadBanks = function () {
        var _this = this;
        this._CustomBankListService.getAllFromCache().subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.banksList = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                }
            }
        });
    };
    BankAccountToRefundComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    BankAccountToRefundComponent.prototype.FillErrors = function () {
        var errors = [];
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.FileType)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileTypeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.IdentifierType)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.IdentifierMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.IdentifierCode)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.IdentifierCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.CountryCode)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.CountryCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.BankCode)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.BankCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.AccountBranch)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.BankBranchMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.AccountNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.AccountNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CountryCode) && this.CountryCode != "IL") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.AccountCurrency)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.AccountCurrencyMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
    };
    BankAccountToRefundComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("");
        var currRequestParams = new BankAccountToRefundRequestParams_1.BankAccountToRefundRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FileType = this.FileTypeCode;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        currRequestParams.IdentifierType = this.IdentifierType;
        currRequestParams.IdentifierCode = this.IdentifierCode;
        currRequestParams.CountryCode = this.CountryCode;
        currRequestParams.BankCode = this.BankCode;
        currRequestParams.AccountBranch = this.AccountBranch;
        currRequestParams.AccountNumber = this.AccountNumber;
        currRequestParams.AccountCurrency = this.AccountCurrency;
        currRequestParams.DeclarationId = this.declarationId;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת בקשה להחזר פקדון", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._TapagMessagesService.PostBankAccountToRefundQueryRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], BankAccountToRefundComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    BankAccountToRefundComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BankAccountToRefundComponent.html',
            selector: 'BankAccountToRefundComponent',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], BankAccountToRefundComponent);
    return BankAccountToRefundComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.BankAccountToRefundComponent = BankAccountToRefundComponent;
//# sourceMappingURL=BankAccountToRefundComponent.js.map