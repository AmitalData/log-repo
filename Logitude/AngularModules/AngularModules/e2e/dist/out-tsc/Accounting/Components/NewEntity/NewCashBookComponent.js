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
var CashBookPM_1 = require("../../EntityPMs/CashBookPM");
var CashBookPMService_1 = require("../../Services/StandardPMs/CashBookPMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var NewCashBookComponent = /** @class */ (function (_super) {
    __extends(NewCashBookComponent, _super);
    function NewCashBookComponent(CD, entityListService, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "CashBook";
        _this.ValidationErrorsList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        // Create new cashbook
        _this.EntityPM = new CashBookPM_1.CashBookPM();
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.EntityPM.Inactive = false;
        _this.EntityPM.TotalAmount = 0;
        _this.myService = new CashBookPMService_1.CashBookPMService();
        _this.SetUIProperties();
        _this.SelectDefaultValues();
        _this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe(function (response) { });
        _this.InitLOVFilters();
        return _this;
    }
    NewCashBookComponent.prototype.InitLOVFilters = function () {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        // this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", true, null, null, "Equals", false, false, false, "boolean");
        // this.GLAccountsFilterItems.addAdditionalFilter("CurrencyId", this.CurrencyId, "OOORRR", null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("SingleAndMultiCurrencyAccount", this.CurrencyId, null, null, "Equals", true, false, false, "string");
    };
    NewCashBookComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewCashBookComponent.prototype, "LocalName", {
        //#region Properties
        //get Code() { return this.EntityPM.Id; }
        //set Code(value: string) {
        //    if (this.EntityPM.Id != value) {
        //        this.EntityPM.Id = value;
        //    }
        //}
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "CashBookTypeCode", {
        get: function () { return this.EntityPM.CashBookTypeCode; },
        set: function (value) {
            if (this.EntityPM.CashBookTypeCode != value) {
                this.EntityPM.CashBookTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                this.CheckCurrency();
            }
            // var filter = this.GLAccountsFilterItems.AdditionalFilters.find(d => d.FieldName == "CurrencyId");
            var filter = this.GLAccountsFilterItems.AdditionalFilters.find(function (d) { return d.FieldName == "SingleAndMultiCurrencyAccount"; });
            filter.FieldValue = this.CurrencyId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "AccountId", {
        get: function () { return this.EntityPM.AccountId; },
        set: function (value) {
            if (this.EntityPM.AccountId != value) {
                this.EntityPM.AccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "BranchId", {
        get: function () { return this.EntityPM.BranchId; },
        set: function (value) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCashBookComponent.prototype, "Account", {
        get: function () { return this.account; },
        set: function (value) {
            if (this.account != value) {
                this.account = value;
                this.CheckCurrency();
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewCashBookComponent.prototype.OkButtonClicked = function () {
        this.CheckCurrency();
        if (this.ValidationErrorsList.length == 0) {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (errors.length == 0) {
                //if (AppTool.IsNullOrEmpty(this.BranchId)) {
                //    this.ValidationErrorsList.push("Branch fields is requierd");
                //}
                this.SubmitChanges();
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
    };
    NewCashBookComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewCashBookComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.CancelButtonClicked();
                    });
                });
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewCashBookComponent.prototype.SetUIProperties = function () {
        //this.UIProperties.SetEnabled("CashBooksId", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("BranchId", this.ObjectTableName, true);
    };
    NewCashBookComponent.prototype.SelectDefaultValues = function () {
        this.EntityPM.CreateDate = new Date();
        this.EntityPM.UpdateDate = new Date();
        this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
    };
    NewCashBookComponent.prototype.OnLovItemChanged = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.Account = item;
        }
    };
    NewCashBookComponent.prototype.CheckCurrency = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Account) && !Tools_1.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.Account.CurrencyId != this.CurrencyId) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.GLAccountcurnotmatchcashbookcur")); // "The GLAccount currency does not match the cashbook currency!");
            }
            else {
                this.ValidationErrorsList = [];
            }
        }
    };
    NewCashBookComponent = __decorate([
        core_1.Component({
            selector: 'NewCashBookComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './NewCashBookComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService, EntityResourceService_1.EntityResourceService])
    ], NewCashBookComponent);
    return NewCashBookComponent;
}(BaseComponent_1.BaseComponent));
exports.NewCashBookComponent = NewCashBookComponent;
//# sourceMappingURL=NewCashBookComponent.js.map