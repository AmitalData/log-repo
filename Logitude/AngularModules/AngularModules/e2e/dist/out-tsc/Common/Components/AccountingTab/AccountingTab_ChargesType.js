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
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ChargeTypeAccountingPM_1 = require("../../EntityPMs/ChargeTypeAccountingPM");
var VatTypeListService_1 = require("../../Services/StandardLists/VatTypeListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var AccountingTab_ChargesType = /** @class */ (function (_super) {
    __extends(AccountingTab_ChargesType, _super);
    function AccountingTab_ChargesType(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.AllVatTypes = [];
        _this.IsAccountingActivated = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsExternalByProductsVisible = false;
        _this.isExternalByProductsRequestd = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        _this.SetUIProperties();
        _this.InitializeComponent();
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            if (SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "AI") {
                _this.IsExternalByProductsVisible = true;
            }
        }
        _this.ReceivableCreditGLAccountFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.PayableDebitGLAcountFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.ReceivableCreditGLAccountFilterItems.addAdditionalFilter("RevenueExpenseType", "1", null, null, "Equals", false, false, false, "string", false, true);
        _this.PayableDebitGLAcountFilterItems.addAdditionalFilter("RevenueExpenseType", "2", null, null, "Equals", false, false, false, "string", false, true);
        _this.Listen();
        return _this;
    }
    AccountingTab_ChargesType.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                        if (_this.isExternalByProductsRequestd) {
                            _this.ApplyExternalByProducts();
                        }
                    }
                    _this.isExternalByProductsRequestd = false;
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                    }
                });
            }
        }
    };
    AccountingTab_ChargesType.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    AccountingTab_ChargesType.prototype.SetUIProperties = function () {
        var isVATSplitEnabled = true;
        var isPayableFieldEnabled = false;
        var isReceivableFieldEnabled = false;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isVATSplitEnabled = false;
            }
        }
        if (isVATSplitEnabled) {
            isPayableFieldEnabled = this.EntityPM.IsPayable && !this.AccountingVATSplit ? true : false;
            isReceivableFieldEnabled = this.EntityPM.IsReceivable && !this.AccountingVATSplit ? true : false;
        }
        if (this.IsAccountingActivated) {
            this.UIProperties.SetEnabled("PayableDebitGLAcountId", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditGLAccountId", this.ObjectTableName, isReceivableFieldEnabled);
        }
        else {
            this.UIProperties.SetEnabled("AccountingVATSplit", this.ObjectTableName, isVATSplitEnabled);
            this.UIProperties.SetEnabled("PayableDebitAccount", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditAccount", this.ObjectTableName, isReceivableFieldEnabled);
        }
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    AccountingTab_ChargesType.prototype.InitializeComponent = function () {
        var _this = this;
        var myService = new VatTypeListService_1.VatTypeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
            _this.BuildItemsSource();
        });
    };
    AccountingTab_ChargesType.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.EntityPM.ChargeTypeAccountings.forEach(function (item) {
            _this.ItemsSource.push(new AccountingTabVATCharge(item, _this));
        });
        this.AllVatTypes.forEach(function (list) {
            var existingItem = _this.ItemsSource.filter(function (f) { return f.VatTypeId == list.Id; })[0];
            if (existingItem == null) {
                var newItemPM = new ChargeTypeAccountingPM_1.ChargeTypeAccountingPM(null);
                newItemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                newItemPM.VatTypeId = list.Id;
                newItemPM.VatTypeName = list.EnglishName;
                newItemPM.ChargeTypeId = _this.EntityPM.Id;
                newItemPM.ChargeTypeName = _this.EntityPM.EnglishName;
                _this.ItemsSource.push(new AccountingTabVATCharge(newItemPM, _this));
            }
        });
    };
    Object.defineProperty(AccountingTab_ChargesType.prototype, "AccountingVATSplit", {
        get: function () { return this.EntityPM.AccountingVATSplit; },
        set: function (value) {
            if (this.EntityPM.AccountingVATSplit != value) {
                this.EntityPM.AccountingVATSplit = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTab_ChargesType.prototype, "PayableDebitAccount", {
        get: function () { return this.EntityPM.PayableDebitAccount; },
        set: function (value) {
            if (this.EntityPM.PayableDebitAccount != value) {
                this.EntityPM.PayableDebitAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTab_ChargesType.prototype, "PayableDebitGLAcountId", {
        get: function () { return this.EntityPM.PayableDebitGLAcountId; },
        set: function (value) {
            if (this.EntityPM.PayableDebitGLAcountId != value) {
                this.EntityPM.PayableDebitGLAcountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTab_ChargesType.prototype, "ReceivableCreditAccount", {
        get: function () { return this.EntityPM.ReceivableCreditAccount; },
        set: function (value) {
            if (this.EntityPM.ReceivableCreditAccount != value) {
                this.EntityPM.ReceivableCreditAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTab_ChargesType.prototype, "ReceivableCreditGLAccountId", {
        get: function () { return this.EntityPM.ReceivableCreditGLAccountId; },
        set: function (value) {
            if (this.EntityPM.ReceivableCreditGLAccountId != value) {
                this.EntityPM.ReceivableCreditGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingTab_ChargesType.prototype.ExternalByProductsClicked = function () {
        if (!this.isExternalByProductsRequestd) {
            this.isExternalByProductsRequestd = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    AccountingTab_ChargesType.prototype.ApplyExternalByProducts = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Accounting advanced";
        logWindow.WindowArgs = { EntityPM: this.EntityPM };
        logWindow.Show('./Common/Components/AccountingTab/Advanced/ChargesExternalByProductsComponent');
    };
    AccountingTab_ChargesType = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AccountingTab_ChargesType.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AccountingTab_ChargesType);
    return AccountingTab_ChargesType;
}(BaseComponent_1.BaseComponent));
exports.AccountingTab_ChargesType = AccountingTab_ChargesType;
var AccountingTabVATCharge = /** @class */ (function (_super) {
    __extends(AccountingTabVATCharge, _super);
    function AccountingTabVATCharge(entityPM, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "ChargeTypeAccounting";
        _this.DataContext = _this;
        _this.IsAccountingActivated = false;
        _this.EntityPM = entityPM;
        _this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.SetUIProperties();
        return _this;
    }
    Object.defineProperty(AccountingTabVATCharge.prototype, "VatTypeId", {
        get: function () { return this.EntityPM.VatTypeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "VatTypeName", {
        get: function () { return this.EntityPM.VatTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "ChargeTypeId", {
        get: function () { return this.EntityPM.ChargeTypeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "ChargeTypeName", {
        get: function () { return this.EntityPM.ChargeTypeName; },
        enumerable: true,
        configurable: true
    });
    AccountingTabVATCharge.prototype.SetUIProperties = function () {
        var isPayableFieldEnabled = false;
        var isReceivableFieldEnabled = false;
        if (this.father.AccountingVATSplit) {
            if (SessionLocator_1.SessionLocator.Tenant == 65) {
                if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                    isPayableFieldEnabled = this.father.EntityPM.IsPayable ? true : false;
                    isReceivableFieldEnabled = this.father.EntityPM.IsReceivable ? true : false;
                }
            }
            else {
                isPayableFieldEnabled = this.father.EntityPM.IsPayable ? true : false;
                isReceivableFieldEnabled = this.father.EntityPM.IsReceivable ? true : false;
            }
        }
        if (this.IsAccountingActivated) {
            this.UIProperties.SetEnabled("PayableDebitGLAcountId", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditGLAccountId", this.ObjectTableName, isReceivableFieldEnabled);
        }
        else {
            this.UIProperties.SetEnabled("PayableDebitAccount", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditAccount", this.ObjectTableName, isReceivableFieldEnabled);
        }
    };
    Object.defineProperty(AccountingTabVATCharge.prototype, "PayableDebitAccount", {
        get: function () { return this.EntityPM.PayableDebitAccount; },
        set: function (value) {
            if (this.EntityPM.PayableDebitAccount != value) {
                this.EntityPM.PayableDebitAccount = value;
                this.OnDataInput();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "PayableDebitGLAcountId", {
        get: function () { return this.EntityPM.PayableDebitGLAcountId; },
        set: function (value) {
            if (this.EntityPM.PayableDebitGLAcountId != value) {
                this.EntityPM.PayableDebitGLAcountId = value;
                this.OnDataInput();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "ReceivableCreditAccount", {
        get: function () { return this.EntityPM.ReceivableCreditAccount; },
        set: function (value) {
            if (this.EntityPM.ReceivableCreditAccount != value) {
                this.EntityPM.ReceivableCreditAccount = value;
                this.OnDataInput();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingTabVATCharge.prototype, "ReceivableCreditGLAccountId", {
        get: function () { return this.EntityPM.ReceivableCreditGLAccountId; },
        set: function (value) {
            if (this.EntityPM.ReceivableCreditGLAccountId != value) {
                this.EntityPM.ReceivableCreditGLAccountId = value;
                this.OnDataInput();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingTabVATCharge.prototype.OnDataInput = function () {
        if (this.IsAccountingActivated) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PayableDebitGLAcountId) && Tools_1.AppTool.IsNullOrEmpty(this.ReceivableCreditGLAccountId)) {
                this.father.EntityPM.RemoveChargeTypeAccountingPM(this.EntityPM);
            }
            else {
                this.father.EntityPM.AddChargeTypeAccountingPM(this.EntityPM);
            }
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PayableDebitAccount) && Tools_1.AppTool.IsNullOrEmpty(this.ReceivableCreditAccount)) {
                this.father.EntityPM.RemoveChargeTypeAccountingPM(this.EntityPM);
            }
            else {
                this.father.EntityPM.AddChargeTypeAccountingPM(this.EntityPM);
            }
        }
    };
    return AccountingTabVATCharge;
}(BaseComponent_1.BaseComponent));
exports.AccountingTabVATCharge = AccountingTabVATCharge;
//# sourceMappingURL=AccountingTab_ChargesType.js.map