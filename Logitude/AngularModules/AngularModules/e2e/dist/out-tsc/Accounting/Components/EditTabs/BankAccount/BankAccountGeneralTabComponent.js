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
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var BankAccountGeneralTabComponent = /** @class */ (function (_super) {
    __extends(BankAccountGeneralTabComponent, _super);
    function BankAccountGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.oldCurrency = null;
        _this.EntityPM = null;
        _this.ObjectTableName = "BankAccount";
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperties();
        _this.InitLOVFilters();
        _this.Listen();
        return _this;
    }
    BankAccountGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
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
    BankAccountGeneralTabComponent.prototype.InitLOVFilters = function () {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("ChartOfAccountsTypeCode", "5", null, null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", false, null, null, "Equals", false, false, false, "string");
    };
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "AccountNumber", {
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
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "GLAccountId", {
        get: function () { return this.EntityPM.GLAccountId; },
        set: function (value) {
            if (this.EntityPM.GLAccountId != value) {
                this.EntityPM.GLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "GLAccount", {
        get: function () { return this.glAccount; },
        set: function (value) {
            if (this.glAccount != value) {
                this.glAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "DeferredGLAccountId", {
        get: function () { return this.EntityPM.DeferredGLAccountId; },
        set: function (value) {
            if (this.EntityPM.DeferredGLAccountId != value) {
                this.EntityPM.DeferredGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "DeferredGLAccount", {
        get: function () { return this.deferredGLAccount; },
        set: function (value) {
            if (this.deferredGLAccount != value) {
                this.deferredGLAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "TransferGLAcccountId", {
        get: function () { return this.EntityPM.TransferGLAcccountId; },
        set: function (value) {
            if (this.EntityPM.TransferGLAcccountId != value) {
                this.EntityPM.TransferGLAcccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "BankId", {
        get: function () { return this.EntityPM.BankId; },
        set: function (value) {
            if (this.EntityPM.BankId != value) {
                this.EntityPM.BankId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "BranchNumber", {
        get: function () { return this.EntityPM.BranchNumber; },
        set: function (value) {
            if (this.EntityPM.BranchNumber != value) {
                this.EntityPM.BranchNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "ChequeCounter", {
        get: function () { return this.EntityPM.ChequeCounter; },
        set: function (value) {
            if (this.EntityPM.ChequeCounter != value) {
                this.EntityPM.ChequeCounter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "IBAN", {
        get: function () { return this.EntityPM.IBAN; },
        set: function (value) {
            if (this.EntityPM.IBAN != value) {
                this.EntityPM.IBAN = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "SwiftCode", {
        get: function () { return this.EntityPM.SwiftCode; },
        set: function (value) {
            if (this.EntityPM.SwiftCode != value) {
                this.EntityPM.SwiftCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "BranchAddress", {
        get: function () { return this.EntityPM.BranchAddress; },
        set: function (value) {
            if (this.EntityPM.BranchAddress != value) {
                this.EntityPM.BranchAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "Inactive", {
        get: function () { return this.EntityPM.Inactive; },
        set: function (value) {
            if (this.EntityPM.Inactive != value) {
                this.EntityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccountGeneralTabComponent.prototype, "EnglishName", {
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
    BankAccountGeneralTabComponent.prototype.SetUIProperties = function () {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}
    };
    BankAccountGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BankAccountGeneralTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BankAccountGeneralTabComponent);
    return BankAccountGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BankAccountGeneralTabComponent = BankAccountGeneralTabComponent;
//# sourceMappingURL=BankAccountGeneralTabComponent.js.map