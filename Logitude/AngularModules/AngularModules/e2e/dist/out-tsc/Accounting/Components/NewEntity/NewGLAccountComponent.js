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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var GLAccountPM_1 = require("../../EntityPMs/GLAccountPM");
var GLAccountPMService_1 = require("../../Services/StandardPMs/GLAccountPMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var FullAccountingSettingPM_1 = require("../../EntityPMs/FullAccountingSettingPM");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var NewGLAccountComponent = /** @class */ (function (_super) {
    __extends(NewGLAccountComponent, _super);
    function NewGLAccountComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccount";
        _this.ValidationErrorsList = [];
        _this.IsFromArgs = false;
        _this.AccountTypeCode = "1";
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsMultiCurrencyCheckboxEnabled = true;
        _this.FullAccountingSetting = new FullAccountingSettingPM_1.FullAccountingSettingPM();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = new GLAccountPM_1.GLAccountPM();
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.EntityPM.IsMultiCurrency = false;
        _this.myService = new GLAccountPMService_1.GLAccountPMService();
        _this.InitLOVFilters();
        _this.SetUIProperties();
        _this.SelectDefaultValues();
        return _this;
    }
    NewGLAccountComponent.prototype.InitLOVFilters = function () {
        //#region initialize query filters for ChartOfAccountType
        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        if (!this.IsFromArgs) {
            this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
            //this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
        }
        else {
            this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, false);
            // customer
            this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
        }
        //#endregion
        //#region initialize query filters for Parent Account
        this.ParentsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.ParentsFilterItems.addAdditionalFilter("ChartOfAccountsId", this.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
        this.ParentsFilterItems.addAdditionalFilter("ParentAccountId", "Please Don't Erase Me", null, null, "IsNull", false, false, false, "string", false, true);
        //this.ParentsFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");
        //#endregion
    };
    NewGLAccountComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.IsFromArgs = true;
            if (args.AccountType == "2") { // customer
                this.ChartOfAccountsTypeCode = args.ChartOfAccountType;
                this.DisplayNumber = args.DisplayNo;
                this.LocalName = args.LocalName;
                this.EnglishName = args.EnglishName;
                this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
                this.AccountTypeCode = args.AccountType;
                this.EntityPM.NewGLAccountCardId = args.CardId;
                this.EntityPM.RevenueExpenseType = args.RevenueExpenseType;
                this.InitLOVFilters();
            }
            else if (args.AccountType == "3") { // vendor
                this.ChartOfAccountsTypeCode = args.ChartOfAccountType;
                this.DisplayNumber = args.DisplayNo;
                this.LocalName = args.LocalName;
                this.EnglishName = args.EnglishName;
                this.AccountTypeCode = args.AccountType;
                this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
                this.EntityPM.NewGLAccountCardId = args.CardId;
                this.EntityPM.RevenueExpenseType = args.RevenueExpenseType;
                this.InitLOVFilters();
            }
        }
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
    };
    Object.defineProperty(NewGLAccountComponent.prototype, "IsMultiCurrency", {
        //#region Properties
        get: function () { return this.EntityPM.IsMultiCurrency == null ? false : this.EntityPM.IsMultiCurrency; },
        set: function (value) {
            if (value == true) {
                this.EntityPM.IsMultiCurrency = value;
                this.ReconcileMethodCode = '0';
                this.CurrencyId = null;
                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
                this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, false);
                //this.UIProperties.SetEnabled("ReconcileMethodCode", this.ObjectTableName, false);
                this.CD.detectChanges();
            }
            else if (value == false) {
                this.EntityPM.IsMultiCurrency = value;
                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
                this.ReconcileMethodCode = null;
                this.CD.detectChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "IsVATExempt", {
        get: function () { return this.EntityPM.IsVATExempt; },
        set: function (value) {
            if (this.EntityPM.IsVATExempt != value) {
                this.EntityPM.IsVATExempt = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "RevaluationEnabled", {
        get: function () { return this.EntityPM.RevaluationEnabled; },
        set: function (value) {
            if (this.EntityPM.RevaluationEnabled != value) {
                this.EntityPM.RevaluationEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "ChartOfAccountsTypeCode", {
        get: function () { return this.EntityPM.ChartOfAccountsTypeCode; },
        set: function (value) {
            if (this.EntityPM.ChartOfAccountsTypeCode != value) {
                this.EntityPM.ChartOfAccountsTypeCode = value;
                this.ChartOfAccountsId = null;
            }
            this.OnLovItemChanged(value);
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                // if (value == "1") { // 1-Revenues
                //     this.RevenueExpenseType = "1";
                //     this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
                // } else if (value == "2") { // 2-Expenses
                //     this.RevenueExpenseType = "2";
                //     this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
                // } else {
                //     this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
                // }
                //
                if (value == "1" || value == "2") { // 1-Revenues, 2-Expenses
                    // disable fields
                    this.IsMultiCurrency = true;
                    this.CurrencyId = null;
                    this.IsMultiCurrencyCheckboxEnabled = false;
                    this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                    // disable fields
                    this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
                    // set type
                    this.RevenueExpenseType = value;
                }
                else {
                    // enable fields
                    this.IsMultiCurrency = false;
                    this.IsMultiCurrencyCheckboxEnabled = true;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
                }
            }
            else {
                this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "ChartOfAccountsId", {
        get: function () { return this.EntityPM.ChartOfAccountsId; },
        set: function (value) {
            if (this.EntityPM.ChartOfAccountsId != value) {
                this.EntityPM.ChartOfAccountsId = value;
                if (value != null) {
                    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
                }
                else {
                    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
                }
                var filter = this.ParentsFilterItems.AdditionalFilters.find(function (d) { return d.FieldName == "ChartOfAccountsId"; });
                filter.FieldValue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "DisplayNumber", {
        get: function () { return this.EntityPM.DisplayNumber; },
        set: function (value) {
            if (this.EntityPM.DisplayNumber != value) {
                this.EntityPM.DisplayNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                if (value != null)
                    this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
                else
                    this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, false, "");
                if (value == SessionLocator_1.SessionLocator.TenantPM.CurrencyId)
                    this.ReconcileMethodCode = "0";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "ReconcileMethodCode", {
        get: function () { return this.EntityPM.ReconcileMethodCode; },
        set: function (value) {
            if (this.EntityPM.ReconcileMethodCode != value) {
                this.EntityPM.ReconcileMethodCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "AutomaticReconcileId", {
        get: function () { return this.EntityPM.AutomaticReconcileId; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcileId != value) {
                this.EntityPM.AutomaticReconcileId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "ParentAccountId", {
        get: function () { return this.EntityPM.ParentAccountId; },
        set: function (value) {
            if (this.EntityPM.ParentAccountId != value) {
                this.EntityPM.ParentAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "Category1Id", {
        get: function () { return this.EntityPM.Category1Id; },
        set: function (value) {
            if (this.EntityPM.Category1Id != value) {
                this.EntityPM.Category1Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "Category2Id", {
        get: function () { return this.EntityPM.Category2Id; },
        set: function (value) {
            if (this.EntityPM.Category2Id != value) {
                this.EntityPM.Category2Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "Category3Id", {
        get: function () { return this.EntityPM.Category3Id; },
        set: function (value) {
            if (this.EntityPM.Category3Id != value) {
                this.EntityPM.Category3Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "Category4Id", {
        get: function () { return this.EntityPM.Category4Id; },
        set: function (value) {
            if (this.EntityPM.Category4Id != value) {
                this.EntityPM.Category4Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "Category5Id", {
        get: function () { return this.EntityPM.Category5Id; },
        set: function (value) {
            if (this.EntityPM.Category5Id != value) {
                this.EntityPM.Category5Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGLAccountComponent.prototype, "RevenueExpenseType", {
        get: function () { return this.EntityPM.RevenueExpenseType; },
        set: function (value) {
            if (this.EntityPM.RevenueExpenseType != value) {
                this.EntityPM.RevenueExpenseType = value;
                if (value == "3") {
                    this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewGLAccountComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        if (this.IsMultiCurrency) {
            if (this.ReconcileMethodCode != '0') {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.LocalCurrencyErr"));
                //errors.push("The reconcile method for multi currency GLAaccount must be local currency"); // need a textcode to enable translations to hebrew
            }
        }
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    NewGLAccountComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    NewGLAccountComponent.prototype.SubmitChanges = function () {
        var _this = this;
        if (this.AccountTypeCode == "3") { // vendor
            this.EntityPM.ControlAccountId = this.FullAccountingSetting.VendorControlAccountId;
        }
        this.CurrentSession.StartBusyIndicatorSaving();
        this.EntityPM.AccountTypeCode = Tools_1.AppTool.IsNullOrEmpty(this.AccountTypeCode) ? "1" : this.AccountTypeCode;
        this.EntityPM.Inactive = false;
        this.EntityPM.IsControlAccount = false;
        this.myService.insert(this.EntityPM).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
            }
        });
    };
    NewGLAccountComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    };
    NewGLAccountComponent.prototype.OnLovItemChanged = function (item) {
        if (item == null) {
            this.ChartOfAccountsId = null;
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
        }
        else {
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
        }
    };
    NewGLAccountComponent.prototype.SelectDefaultValues = function () {
        var _this = this;
        // Select default value for Automatic Reconcilation
        this.entityListService.getSingle(this.TenantPM.Id.toString(), "FullAccountingSetting").then(function (res) {
            res.subscribe(function (myResponse) {
                if (myResponse != null) {
                    var res = myResponse.Result;
                    _this.FullAccountingSetting = res;
                    _this.AutomaticReconcileId = res["AutomaticReconcileMethodId"];
                    //console.log(res);
                }
            });
        });
    };
    NewGLAccountComponent.prototype.GetDisplayMemberPath = function () {
        var showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        return showLocal ? "LocalName" : "EnglishName";
    };
    NewGLAccountComponent = __decorate([
        core_1.Component({
            selector: 'NewGLAccountComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './NewGLAccountComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], NewGLAccountComponent);
    return NewGLAccountComponent;
}(BaseComponent_1.BaseComponent));
exports.NewGLAccountComponent = NewGLAccountComponent;
//# sourceMappingURL=NewGLAccountComponent.js.map