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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var GLAccountValidator_1 = require("../../../Validators/GLAccountValidator");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var GLAccountGeneralTabComponent = /** @class */ (function (_super) {
    __extends(GLAccountGeneralTabComponent, _super);
    function GLAccountGeneralTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.oldCurrency = null;
        _this.oldIsMultiCurrency = null;
        _this.EntityPM = null;
        _this.ObjectTableName = "GLAccount";
        _this.DataContext = _this;
        _this.DisableGLAccount = false;
        _this.IsEditMode = false;
        _this.IsCustomerAccount = false;
        _this.IsVendor = false;
        _this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.TabSelectedEvent = null;
        _this.IsMultiCurrencyCheckboxEnabled = true;
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        _this.oldCurrency = _this.EntityPM.CurrencyId;
        _this.oldIsMultiCurrency = _this.EntityPM.IsMultiCurrency;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //#region initialize query filters for ChartOfAccountType
        _this.ChartOfAccountTypeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
        //this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "4", null, null, "NotEqual", false, false, false, "string", false, true);
        //#endregion
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) { // Edit Mode
            _this.IsEditMode = true;
            _this.IsVendor = false;
            if (_this.EntityPM.AccountTypeCode == "5" || _this.EntityPM.AccountTypeCode == "4") {
                _this.DisableGLAccount = true;
                _this.SetFieldsEditablility(false);
            }
            else if (_this.EntityPM.AccountTypeCode == "2" || _this.EntityPM.AccountTypeCode == "3") {
                _this.IsCustomerAccount = true;
                _this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", _this.ObjectTableName, false);
                _this.UIProperties.SetEnabled("EnglishName", _this.ObjectTableName, false);
                _this.UIProperties.SetEnabled("LocalName", _this.ObjectTableName, false);
            }
            if (_this.EntityPM.AccountTypeCode == "3") {
                _this.IsVendor = true;
            }
            //#region initialize query filters for Parent Account
            _this.ParentsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
            _this.ParentsFilterItems.addAdditionalFilter("Id", _this.EntityPM.Id, null, null, "Exclude", false, false, false, "string", false, true);
            _this.ParentsFilterItems.addAdditionalFilter("ChartOfAccountsId", _this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
            _this.ParentsFilterItems.addAdditionalFilter("ParentAccountId", "Please Don't Erase Me", null, null, "IsNull", false, false, false, "string", false, true);
            if (_this.IsVendor) {
                _this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "3", null, null, "Equals", false, false, false, "string", false, true);
            }
            else if (_this.IsCustomerAccount) {
                _this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "2", null, null, "Equals", false, false, false, "string", false, true);
            }
            //#endregion
            if (_this.ChartOfAccountsTypeCode == "1") { // 1-Revenues
                _this.UIProperties.SetEnabled("RevenueExpenseType", _this.ObjectTableName, false);
            }
            else if (_this.ChartOfAccountsTypeCode == "2") { // 2-Expenses
                _this.UIProperties.SetEnabled("RevenueExpenseType", _this.ObjectTableName, false);
            }
            else {
                _this.UIProperties.SetEnabled("RevenueExpenseType", _this.ObjectTableName, true);
            }
        }
        _this.SetUIProperties();
        _this.Listen();
        return _this;
    }
    GLAccountGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "IsMultiCurrency", {
        //DownloadButtonClicked() {
        //    this._GLAccountExtendedListService.CalculateFututreCheques().subscribe(myResult => {
        //    });
        //}
        //#region Properties
        get: function () { return this.EntityPM.IsMultiCurrency == null ? false : this.EntityPM.IsMultiCurrency; },
        set: function (value) {
            if (this.EntityPM.IsMultiCurrency != value) {
                this.EntityPM.IsMultiCurrency = value;
                if (value != this.oldIsMultiCurrency)
                    GLAccountValidator_1.GLAccountValidator.ValidateIsMultiCurrency(this.EntityPM);
                if (value) {
                    // Change UI Property
                    this.CurrencyId = null;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                    this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, false);
                    this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
                    this.ReconcileMethodCode = '0';
                    this.CD.detectChanges();
                }
                else {
                    this.EntityPM.IsMultiCurrency = value;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                    this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
                    this.ReconcileMethodCode = null;
                    this.CD.detectChanges();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "RevaluationEnabled", {
        get: function () { return this.EntityPM.RevaluationEnabled; },
        set: function (value) {
            if (this.EntityPM.RevaluationEnabled != value) {
                this.EntityPM.RevaluationEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "IsVATExempt", {
        get: function () { return this.EntityPM.IsVATExempt; },
        set: function (value) {
            if (this.EntityPM.IsVATExempt != value) {
                this.EntityPM.IsVATExempt = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "IsEquipmentVendor", {
        get: function () { return this.EntityPM.IsEquipmentVendor; },
        set: function (value) {
            if (this.EntityPM.IsEquipmentVendor != value) {
                this.EntityPM.IsEquipmentVendor = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "ExcludeFromDeductionReport", {
        get: function () { return this.EntityPM.ExcludeFromDeductionReport; },
        set: function (value) {
            if (this.EntityPM.ExcludeFromDeductionReport != value) {
                this.EntityPM.ExcludeFromDeductionReport = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "ChartOfAccountsTypeCode", {
        get: function () { return this.EntityPM.ChartOfAccountsTypeCode; },
        set: function (value) {
            if (this.EntityPM.ChartOfAccountsTypeCode != value) {
                this.EntityPM.ChartOfAccountsTypeCode = value;
                this.ChartOfAccountsId = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
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
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "ChartOfAccountsId", {
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
                filter.FieldValue = this.EntityPM.ChartOfAccountsId;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "DisplayNumber", {
        get: function () { return this.EntityPM.DisplayNumber; },
        set: function (value) {
            if (this.EntityPM.DisplayNumber != value) {
                this.EntityPM.DisplayNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                // Check transactions
                GLAccountValidator_1.GLAccountValidator.ValidateCurrency(this.EntityPM, this.oldCurrency);
                if (value != null) {
                    this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
                }
                else {
                    this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, false, "");
                    this.EntityPM.CurrencyCode = null;
                }
                if (value == SessionLocator_1.SessionLocator.TenantPM.CurrencyId)
                    this.ReconcileMethodCode = "0";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "ReconcileMethodCode", {
        get: function () { return this.EntityPM.ReconcileMethodCode; },
        set: function (value) {
            if (this.EntityPM.ReconcileMethodCode != value) {
                this.EntityPM.ReconcileMethodCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "AutomaticReconcileId", {
        get: function () { return this.EntityPM.AutomaticReconcileId; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcileId != value) {
                this.EntityPM.AutomaticReconcileId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "ParentAccountId", {
        get: function () { return this.EntityPM.ParentAccountId; },
        set: function (value) {
            if (this.EntityPM.ParentAccountId != value) {
                this.EntityPM.ParentAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "Category1Id", {
        get: function () { return this.EntityPM.Category1Id; },
        set: function (value) {
            if (this.EntityPM.Category1Id != value) {
                this.EntityPM.Category1Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "Category2Id", {
        get: function () { return this.EntityPM.Category2Id; },
        set: function (value) {
            if (this.EntityPM.Category2Id != value) {
                this.EntityPM.Category2Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "Category3Id", {
        get: function () { return this.EntityPM.Category3Id; },
        set: function (value) {
            if (this.EntityPM.Category3Id != value) {
                this.EntityPM.Category3Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "Category4Id", {
        get: function () { return this.EntityPM.Category4Id; },
        set: function (value) {
            if (this.EntityPM.Category4Id != value) {
                this.EntityPM.Category4Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "Category5Id", {
        get: function () { return this.EntityPM.Category5Id; },
        set: function (value) {
            if (this.EntityPM.Category5Id != value) {
                this.EntityPM.Category5Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountGeneralTabComponent.prototype, "RevenueExpenseType", {
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
    GLAccountGeneralTabComponent.prototype.SetUIProperties = function () {
        if (!this.EntityPM || this.DisableGLAccount)
            return;
        if (this.EntityPM.IsMultiCurrency) {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
        if (this.EntityPM.ChartOfAccountsTypeCode) {
            this.ChartOfAccountsTypeCode = this.EntityPM.ChartOfAccountsTypeCode;
        }
        if (this.EntityPM.ChartOfAccountsId) {
            this.ChartOfAccountsId = this.EntityPM.ChartOfAccountsId;
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
        }
        if (this.EntityPM.RevenueExpenseType == "3") {
            this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, true);
        }
        if (this.ChartOfAccountsTypeCode == "1" || this.ChartOfAccountsTypeCode == "2") { // 1-Revenues, 2-Expenses
            this.IsMultiCurrencyCheckboxEnabled = false;
            this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
    };
    GLAccountGeneralTabComponent.prototype.SetFieldsEditablility = function (enable) {
        this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("DisplayNumber", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ReconcileMethodCode", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("AutomaticReconcileId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ParentAccountId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category1Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category2Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category3Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category4Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category5Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, enable);
    };
    GLAccountGeneralTabComponent.prototype.OnLovItemChanged = function (item) {
        if (item == null) {
            this.ChartOfAccountsId = null;
            if (!this.DisableGLAccount) {
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
                this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
                this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
            }
        }
        else {
            if (!this.DisableGLAccount) {
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
                if (!this.EntityPM.ChartOfAccountsId) {
                    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
                    this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
                }
            }
        }
    };
    GLAccountGeneralTabComponent.prototype.GetDisplayMemberPath = function () {
        var showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        return showLocal ? "LocalName" : "EnglishName";
    };
    GLAccountGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountGeneralTabComponent.html',
            providers: [GLAccountExtendedListService_1.GLAccountExtendedListService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], GLAccountGeneralTabComponent);
    return GLAccountGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountGeneralTabComponent = GLAccountGeneralTabComponent;
//# sourceMappingURL=GLAccountGeneralTabComponent.js.map